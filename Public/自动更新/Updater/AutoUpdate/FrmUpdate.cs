using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace AutoUpdate
{
    public partial class FrmUpdate : Form
    {
        private string updateUrl = string.Empty;
        private string tempUpdatePath = string.Empty;
        private XmlFiles updaterXmlFiles = null;
        private int availableUpdate = 0;
        private bool isRun = false;
        private string mainAppExe = "";
        public FrmUpdate()
        {
            InitializeComponent();
        }


        [STAThread]
        private static void Main()
        {
            Application.Run(new FrmUpdate());
        }

        private void FrmUpdate_Load(object sender, EventArgs e)
        {
            this.panel2.Visible = false;
            this.btnFinish.Visible = false;
            string text = Application.StartupPath + "\\UpdateList.xml";
            string text2 = string.Empty;
            try
            {
                this.updaterXmlFiles = new XmlFiles(text);
            }
            catch
            {
                MessageBox.Show("配置文件出错!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.Close();
                return;
            }
            this.updateUrl = this.updaterXmlFiles.GetNodeValue("//Url");
            AppUpdater appUpdater = new AppUpdater();
            appUpdater.UpdaterUrl = this.updateUrl + "/UpdateList.xml";
            try
            {
                this.tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\_" + this.updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_y_x_m_\\";
                appUpdater.DownAutoUpdateFile(this.tempUpdatePath);
            }
            catch
            {
                MessageBox.Show("与服务器连接失败,操作超时!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                base.Close();
                return;
            }
            Hashtable hashtable = new Hashtable();
            text2 = this.tempUpdatePath + "\\UpdateList.xml";
            if (File.Exists(text2))
            {
                this.availableUpdate = appUpdater.CheckForUpdate(text2, text, out hashtable);
                if (this.availableUpdate > 0)
                {
                    for (int i = 0; i < hashtable.Count; i++)
                    {
                        string[] items = (string[])hashtable[i];
                        this.lvUpdateList.Items.Add(new ListViewItem(items));
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
            Application.ExitThread();
            Application.Exit();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.availableUpdate > 0)
            {
                new Thread(new ThreadStart(this.DownUpdateFile))
                {
                    IsBackground = true
                }.Start();
            }
            else
            {
                MessageBox.Show("没有可用的更新!", "自动更新", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void DownUpdateFile()
        {
            this.mainAppExe = this.updaterXmlFiles.GetNodeValue("//EntryPoint");
            Process[] processes = Process.GetProcesses();
            Process[] array = processes;
            for (int i = 0; i < array.Length; i++)
            {
                Process process = array[i];
                if (process.ProcessName.ToLower() + ".exe" == this.mainAppExe.ToLower())
                {
                    for (int j = 0; j < process.Threads.Count; j++)
                    {
                        process.Threads[j].Dispose();
                    }
                    process.Kill();
                    this.isRun = true;
                }
            }
            WebClient webClient = new WebClient();
            for (int j = 0; j < this.lvUpdateList.Items.Count; j++)
            {
                try
                {
                    string str = this.lvUpdateList.Items[j].Text.Trim();
                    string requestUriString = this.updateUrl + this.lvUpdateList.Items[j].Text.Trim();
                    WebRequest webRequest = WebRequest.Create(requestUriString);
                    WebResponse response = webRequest.GetResponse();
                    long contentLength = response.ContentLength;
                    this.lbState.Text = "正在下载更新文件,请稍后...";
                    this.pbDownFile.Value = 0;
                    this.pbDownFile.Maximum = (int)contentLength;

                    Stream responseStream = response.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream);
                    byte[] array2 = new byte[contentLength];
                    int num = array2.Length;
                    int num2 = 0;
                    while (contentLength > 0L)
                    {
                        Application.DoEvents();
                        int num3 = responseStream.Read(array2, num2, num);
                        if (num3 == 0)
                        {
                            break;
                        }
                        num2 += num3;
                        num -= num3;
                        this.pbDownFile.Value += num3;
                        float num4 = (float)num2 / 1024f;
                        float num5 = (float)array2.Length / 1024f;
                        int num6 = Convert.ToInt32(num4 / num5 * 100f);
                        this.lvUpdateList.Items[j].SubItems[2].Text = num6.ToString() + "%";
                    }
                    string path = this.tempUpdatePath + str;
                    this.CreateDirtory(path);
                    FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    fileStream.Write(array2, 0, array2.Length);
                    responseStream.Close();
                    streamReader.Close();
                    fileStream.Close();
                }
                catch (WebException ex)
                {
                    MessageBox.Show("更新文件下载失败！" + ex.Message.ToString(), "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            this.InvalidateControl();
            this.Cursor = Cursors.Default;
        }

        private void CreateDirtory(string path)
        {
            if (!File.Exists(path))
            {
                string[] array = path.Split(new char[]
                {
                    '\\'
                });
                string text = string.Empty;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    text = text + array[i].Trim() + "\\";
                    if (!Directory.Exists(text))
                    {
                        Directory.CreateDirectory(text);
                    }
                }
            }
        }

        public void CopyFile(string sourcePath, string objPath)
        {
            if (!Directory.Exists(objPath))
            {
                Directory.CreateDirectory(objPath);
            }
            string[] files = Directory.GetFiles(sourcePath);
            for (int i = 0; i < files.Length; i++)
            {
                string[] array = files[i].Split(new char[]
                {
                    '\\'
                });
                File.Copy(files[i], objPath + "\\" + array[array.Length - 1], true);
            }
            string[] directories = Directory.GetDirectories(sourcePath);
            for (int i = 0; i < directories.Length; i++)
            {
                string[] array2 = directories[i].Split(new char[]
                {
                    '\\'
                });
                this.CopyFile(directories[i], objPath + "\\" + array2[array2.Length - 1]);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            base.Close();
            base.Dispose();
            try
            {
                this.CopyFile(this.tempUpdatePath, Directory.GetCurrentDirectory());
                Directory.Delete(this.tempUpdatePath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            if (this.isRun)
            {
                Process.Start(this.mainAppExe);
            }
        }

        private void InvalidateControl()
        {
            this.panel2.Location = this.panel1.Location;
            this.panel2.Size = this.panel1.Size;
            this.panel1.Visible = false;
            this.panel2.Visible = true;
            this.btnNext.Visible = false;
            this.btnCancel.Visible = false;
            this.btnFinish.Location = this.btnCancel.Location;
            this.btnFinish.Visible = true;
        }

        private bool IsMainAppRun()
        {
            string nodeValue = this.updaterXmlFiles.GetNodeValue("//EntryPoint");
            bool result = false;
            Process[] processes = Process.GetProcesses();
            Process[] array = processes;
            for (int i = 0; i < array.Length; i++)
            {
                Process process = array[i];
                if (process.ProcessName.ToLower() + ".exe" == nodeValue.ToLower())
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
