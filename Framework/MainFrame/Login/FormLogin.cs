#region Using directives

using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using DrectSoft.FrameWork;
using DrectSoft.MainFrame.Login;
using MainFrame;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
#endregion

namespace DrectSoft.MainFrame
{
    /// <summary>
    /// Winform登录窗口
    /// </summary>
    public partial class FormLogin : DevBaseForm, ICustomMessageBox
    {
        #region vars
        const string noVerifyUser = "NoVerify";
        Account m_Acnt;
        DrectSoftLog m_Log;
        FormMain m_FormMain;
        DSMessageBox m_messagebox;

        /// <summary>
        ///当前登录用户 
        /// </summary>
        public Account CurrentAccount
        {
            get { return m_Acnt; }
        }

        public ICustomMessageBox CustomMessageBox
        {
            get { return this; }
        }

        Account m_MasterAcnt;
        /// <summary>
        /// 带教老师帐户
        /// </summary>
        public Account MasterAccount
        {
            get
            {
                return m_MasterAcnt;
            }
        }

        private DSMessageBox MessageBoxInstance
        {
            get
            {
                if (m_messagebox == null)
                    m_messagebox = new DSMessageBox();
                return m_messagebox;
            }
        }
        #endregion


        /// <summary>
        /// 构造
        /// </summary>
        public FormLogin(FormMain formMain)
            : this()
        {
            m_FormMain = formMain;
            //m_FormMain.ShowMessageWindow(null, true);
        }

        /// <summary>
        /// 构造
        /// </summary>
        public FormLogin()
        {
            InitializeComponent();

            InitializeUI();
            //this.textBoxUserID.Leave += new EventHandler(textBoxUserID_Leave);
            //this.textBoxPassword.KeyPress += new KeyPressEventHandler(textBoxPassword_KeyPress);
            m_Acnt = new Account();
            m_MasterAcnt = new Account();
            m_Log = FormMain.Instance.Logger;
            this.TransparencyKey = this.BackColor;
            this.TopMost = true;

            //加载界面图片 xll
            try
            {
                //  panelForm.ContentImage = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Icon\\LoginMain.jpg");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 版本加载
        /// </summary>
        private void LoadVersion()
        {
            lblVersion.Text = "1.7";
        }

        private void InitializeUI()
        {
            //TODO
            // 调出背景图
            try
            {
                // 加载登录按钮图标
                buttonLogIn.BackgroundImageLayout = ImageLayout.Stretch;
                //add by xjt,2010-05-26,读取锁屏后,重新选择的用户ID
                if (Program.m_StrUserId != string.Empty && Program.m_StrUserId != null)
                {
                    this.textBoxUserID.Text = Program.m_StrUserId;
                }
                Program.m_StrUserId = string.Empty;
                //add end,xjt
            }
            catch (System.IO.IOException ex)
            {
                m_Log.Error("加载窗口背景图失败:" + ex.Message + "");
            }
        }

        void hyperLinkEdit1_Click(object sender, EventArgs e)
        {
            ExitLogin();
        }

        void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') ProcessLogin();
        }

        void textBoxUserID_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxUserID.Text))
            {
                return;
            }

            try
            {
                if (textBoxUserID.Text.Contains("用户已过期"))
                {
                    m_TempUserID = textBoxUserID.Text;
                    throw new InvalidUserIdException("用户已过期");
                }
                bool available = true;
                {
                    if (textBoxUserID.Text.Trim() != "00")
                    {
                        string extraCodeLong = textBoxUserID.Text.Trim().Insert(0, "000000");
                        string strLoginCode = extraCodeLong.Substring(extraCodeLong.Length - 6, 6);

                        textBoxUserID.Text = strLoginCode;
                    }

                    string username = m_Acnt.GetAUserName(DS_Common.FilterSpecialCharacter(textBoxUserID.Text), out available);
                    m_TempUserID = textBoxUserID.Text;
                    if (IsUserPwdNull(textBoxUserID.Text.Trim()))//允许登录则切换到密码框
                    {
                        ShowInitPwd(textBoxUserID.Text.Trim());
                    }
                    //Add By wwj 2011-06-07 在初始化登录人的Account之后初始化带教老师的Account
                    string masterID = m_Acnt.User.MasterID;//得到带教老师的ID
                    if (!string.IsNullOrEmpty(masterID))
                    {
                        m_MasterAcnt.InitMaster(masterID);//得到带教老师的帐户信息
                    }

                    textBoxUserID.Text = username;
                }
                if (!available) throw new Exception("不存在当前用户");
            }
            catch (Exception ex)
            {
                m_Log.Error(ex.Message);
                textBoxUserID.Text = ex.Message;
                textBoxUserID.Focus();
            }
        }
        public string userid_check(string userid, ref string user_name)
        {
            if (string.IsNullOrEmpty(userid))
            {
                return "用户id为空";
            }

            //IAccount acnt = bizbus.BuildUp<IAccount>();
            try
            {
                if (userid.Contains("用户已过期"))
                {
                    m_TempUserID = userid;
                    throw new InvalidUserIdException("用户已过期");
                }
                bool available = true;
                {
                    if (userid.Trim() != "00")
                    {
                        string extraCodeLong = userid.Trim().Insert(0, "000000");
                        string strLoginCode = extraCodeLong.Substring(extraCodeLong.Length - 6, 6);

                        // textBoxUserID.Text = strLoginCode;
                    }

                    string username = m_Acnt.GetAUserName(DS_Common.FilterSpecialCharacter(userid), out available);
                    m_TempUserID = userid;
                    if (IsUserPwdNull(userid.Trim()))//允许登录则切换到密码框
                    {
                        ShowInitPwd(userid.Trim());
                    }
                    //Add By wwj 2011-06-07 在初始化登录人的Account之后初始化带教老师的Account
                    string masterID = m_Acnt.User.MasterID;//得到带教老师的ID
                    if (!string.IsNullOrEmpty(masterID))
                    {
                        m_MasterAcnt.InitMaster(masterID);//得到带教老师的帐户信息
                    }
                    user_name = username;
                    //textBoxUserID.Text = username;
                }
                if (!available) throw new Exception("不存在当前用户");

                return "不存在当前用户";
            }
            catch (Exception ex)
            {
                m_Log.Error(ex.Message);
                return ex.ToString();
            }
        }


        /// <summary> 
        /// 是否为空密码 false表示无需校验或已设置密码可登陆 true表示密码为空需重置
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private bool IsUserPwdNull(string userID)
        {
            try
            {
                //appcfg表配置用来判断是否校验密码
                string value = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("IsNeedPWDVailde");
                if (value == "0" || value == null || value.Trim() == "")
                {
                    return false;
                }
                else
                {
                    string sql = @"select u.id,u.passwd,u.regdate from users u where u.valid='1'and u.id=@userID";
                    SqlParameter[] sps ={
                                   new SqlParameter("@userID",SqlDbType.VarChar,50)
                                   };
                    sps[0].Value = userID;
                    DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, sps, CommandType.Text);
                    string password = dt.Rows[0]["PASSWD"].ToString().Trim();
                    string nullPwd = HisEncryption.EncodeString(dt.Rows[0]["REGDATE"].ToString().Trim(), HisEncryption.PasswordLength, "");
                    if (password == nullPwd)
                    {
                        return true;//密码为空字符加密
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 展示修改密码界面方法
        /// Add by xlb 2013-03-16
        /// </summary>
        /// <param name="userID"></param>
        private void ShowInitPwd(string userID)
        {
            try
            {
                FormResetPassword frmChangePass = new FormResetPassword(userID, MasterAccount);
                if (frmChangePass == null)
                {
                    return;
                }
                frmChangePass.StartPosition = FormStartPosition.CenterParent;
                frmChangePass.TopMost = true;
                if (frmChangePass.ShowDialog() == DialogResult.OK)
                {
                    textBoxPassword.Focus();
                }
                else
                {
                    textBoxUserID.Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            ProcessLogin();
        }

        private void FormLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                this.SelectNextControl(this.ActiveControl, true, true, true, false);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ExitLogin();
        }

        void ExitLogin()
        {
            ExitApplication();
        }

        private void ExitApplication()
        {
            m_FormMain.FormClosing -= new FormClosingEventHandler(m_FormMain.FormMain_FormClosing);
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            { }
            //System.Environment.Exit(System.Environment.ExitCode);
            //}
            //else
            //{

            //}
        }

        bool isloging = false;

        string m_TempUserID = string.Empty;
        void ProcessLogin()
        {
            try
            {
                m_FormMain.FormClosing -= new FormClosingEventHandler(m_FormMain.FormMain_FormClosing);
                LoadVersion();
            }
            catch (Exception ex)
            {
                this.CustomMessageBox.MessageShow(ex.Message);
                Application.Exit();
            }

            if (isloging) return;
            isloging = true;
            progressBarControlWait.Visible = true;
            labelControlWait.Visible = true;
            progressBarControlWait.Properties.ShowTitle = true;
            progressBarControlWait.Properties.PercentView = true;

            //进度到20
            ChangeProgressBar(0, 20);

            if (CheckNeedVerify())
            {
                if (!LoginDb())
                {
                    isloging = false;
                    m_FormMain.Visible = false;
                    return;
                }
            }
            else
            {

                //(FormMain.Maininfo.LoginUser as User).SetUser("-1", "UnVerify_User");
            }

            //加载病区概况
            //FormMain.Instance.LoadPlugIn("DrectSoft.Core.DoctorTasks.TaskCenter,DrectSoft.Core.DoctorTasks");

            m_FormMain.ProcessLogin();



            //从数据库中下载用于使用的打印模板
            //徐亮亮 加载速度慢 暂时不用 2012-12-19
            LoadxrpReport();

            //进度到100
            ChangeProgressBar(91, 100);

            //this.DialogResult = DialogResult.OK;
            isloging = false;
            //add by cyq 2013-02-02 修复在登录界面按 alt+F4 退出登录提示点击否后界面异常的bug
            m_FormMain.FormClosing += new FormClosingEventHandler(m_FormMain.FormMain_FormClosing);
        }
        public void ProcessNOLogin()
        {
            try
            {
                m_FormMain.FormClosing -= new FormClosingEventHandler(m_FormMain.FormMain_FormClosing);
                LoadVersion();
            }
            catch (Exception ex)
            {
                this.CustomMessageBox.MessageShow(ex.Message);
                Application.Exit();
            }

            if (isloging) return;
            isloging = true;

            //进度到20
            //  ChangeProgressBar(0, 20);

            if (CheckNeedVerify())
            {
                if (!NOLoginDb())
                {
                    isloging = false;
                    m_FormMain.Visible = false;
                    return;
                }
            }
            else
            {

                //(FormMain.Maininfo.LoginUser as User).SetUser("-1", "UnVerify_User");
            }

            //加载病区概况
            //FormMain.Instance.LoadPlugIn("DrectSoft.Core.DoctorTasks.TaskCenter,DrectSoft.Core.DoctorTasks");

            m_FormMain.ProcessLogin();



            //从数据库中下载用于使用的打印模板
            //徐亮亮 加载速度慢 暂时不用 2012-12-19
            LoadxrpReport();

            //进度到100
            // ChangeProgressBar(91, 100);

            //this.DialogResult = DialogResult.OK;
            isloging = false;
            //add by cyq 2013-02-02 修复在登录界面按 alt+F4 退出登录提示点击否后界面异常的bug
            m_FormMain.FormClosing += new FormClosingEventHandler(m_FormMain.FormMain_FormClosing);
        }
        /// <summary>
        /// 从数据库中下载用于使用的打印模板
        /// xll
        /// 2012-12-17
        /// </summary>
        private void LoadxrpReport()
        {
            FileStream fstrem = null;
            try
            {
                string appPath = Application.StartupPath;
                if (!Directory.Exists(appPath + @"\Report"))
                {
                    Directory.CreateDirectory(appPath + @"\Report");
                }
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
                string sqlGetTemp = @"select tempflow,tempname,ModifyDateTime from commonnoteprinttemp where valide='1'";
                DataTable dtTemPrint = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlGetTemp, CommandType.Text);
                if (dtTemPrint == null || dtTemPrint.Rows.Count <= 0) return;
                foreach (DataRow item in dtTemPrint.Rows)
                {

                    string filePath = "";
                    //为兼容早期版本做处理
                    if (item["TEMPNAME"].ToString().Contains("."))
                    {
                        filePath = appPath + @"\Report\" + item["TEMPNAME"].ToString();
                        // fileinfo = new FileInfo(appPath + @"\Report\" + item["TEMPNAME"].ToString());
                    }
                    else
                    {
                        filePath = appPath + @"\Report\" + item["TEMPNAME"].ToString() + ".xrp";
                        // fileinfo = new FileInfo(appPath + @"\Report\" + item["TEMPNAME"].ToString()+".xrp");
                    }
                    FileInfo fileinfo = new FileInfo(filePath);
                    if (fileinfo.LastWriteTime < Convert.ToDateTime(item["MODIFYDATETIME"]))
                    {
                        File.Delete(filePath);
                        fileinfo = new FileInfo(filePath);
                        string sqlTempDetail = @"select TEMPCONTENT from commonnoteprinttemp where tempflow=@tempflow";
                        SqlParameter[] sqlParams = new SqlParameter[]
                        { 
                            new SqlParameter("@tempflow", SqlDbType.Text)
                        };
                        sqlParams[0].Value = item["TEMPFLOW"].ToString();

                        DataTable dtTemPrintDetali = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlTempDetail, sqlParams, CommandType.Text);
                        if (dtTemPrintDetali == null || dtTemPrintDetali.Rows.Count <= 0) continue;
                        string gzipStr = dtTemPrintDetali.Rows[0]["TEMPCONTENT"].ToString();
                        if (string.IsNullOrEmpty(gzipStr)) continue;
                        string content = DS_Common.UnzipEmrXml(gzipStr);
                        byte[] bytecontent = Convert.FromBase64String(content);
                        fstrem = fileinfo.OpenWrite();
                        fstrem.Write(bytecontent, 0, bytecontent.Length);
                        fstrem.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fstrem != null)
                    fstrem.Close();
            }
        }

        #region 异步调用进度条

        /// <summary>
        /// 设置进度条的进度
        /// </summary>
        public void ChangeProgressBar(int fromValue, int toValue)
        {
            bool flag = false;
            for (int i = fromValue; i <= toValue; i++)
            {
                progressBarControlWait.Position = i;

                if (i >= 100)
                {
                    this.DialogResult = DialogResult.OK;
                }

                System.Threading.Thread.Sleep(10);
                Application.DoEvents();
            }
        }

        /*
        private void CallBackMethod(IAsyncResult ar)
        {
            //从异步状态ar.AsyncState中，获取委托对象
            AsyncEventHandler dn = (AsyncEventHandler)ar.AsyncState;

            //一定要EndInvoke，否则你的下场很惨
            dn.EndInvoke(ar);
        }

        public delegate void AsyncEventHandler(int fromValue, int toValue);

        private void AsynChangeProgressBar(int fromValue, int toValue)
        {
            AsyncEventHandler asy = new AsyncEventHandler(ChangeProgressBar);
            AsyncCallback acb = new AsyncCallback(CallBackMethod);
            IAsyncResult iar = asy.BeginInvoke(fromValue, toValue, acb, asy);

        }
        */
        #endregion

        bool CheckNeedVerify()
        {
            AppSettingsReader appSetReader = new AppSettingsReader();
            string canSkipVerify = "";
            try
            {
                canSkipVerify = (string)appSetReader.GetValue(noVerifyUser, typeof(string));
            }
            catch
            {
                Trace.WriteLine("CanSkipVerify Config Not Find!");
            }

            return canSkipVerify != bool.TrueString;
        }

        bool LoginDb()
        {
            try
            {
                if (m_TempUserID.Contains("用户已过期"))
                {
                    throw new InvalidUserIdException("用户已过期");
                }
                m_Acnt.Login(m_TempUserID, textBoxPassword.Text, 0);

                //Add By wwj 2011-06-07
                string masterID = m_Acnt.User.MasterID;//得到带教老师的ID
                if (!string.IsNullOrEmpty(masterID))
                {
                    m_MasterAcnt.InitMaster(masterID);//初始化带教老师用户信息
                }

                //m_Bizbus.UpdateObject<Users>("CurrentUser", user);
                m_Log.Info("用户:" + m_Acnt.User.Name + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "成功登录系统");
            }
            catch (Exception ex)
            {
                textBoxUserID.Focus();
                textBoxUserID.Text = ex.Message;
                textBoxUserID.SelectAll();
                progressBarControlWait.Position = 0;
                return false;
            }

            return true;
        }
        bool NOLoginDb()
        {
            try
            {
                if (m_TempUserID.Contains("用户已过期"))
                {
                    throw new InvalidUserIdException("用户已过期");
                }
                m_Acnt.Login(m_TempUserID, "", 1);

                //Add By wwj 2011-06-07
                string masterID = m_Acnt.User.MasterID;//得到带教老师的ID
                if (!string.IsNullOrEmpty(masterID))
                {
                    m_MasterAcnt.InitMaster(masterID);//初始化带教老师用户信息
                }

                //m_Bizbus.UpdateObject<Users>("CurrentUser", user);
                m_Log.Info("用户:" + m_Acnt.User.Name + "于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "成功登录系统");
            }
            catch (Exception ex)
            {
                //textBoxUserID.Focus();
                //textBoxUserID.Text = ex.Message;
                //textBoxUserID.SelectAll();
                //progressBarControlWait.Position = 0;
                return false;
            }

            return true;
        }

        void timer1_Tick(object sender, System.EventArgs e)
        {
            //快速显示此次设置窗体的透明度 modified by  zhouhui 2012.421
            if (this.Opacity < 1)
            {
                this.Opacity += 0.5;
            }
            else
            {
                timer1.Enabled = false;
            }
        }
        private string updateUrl = string.Empty;
        private string tempUpdatePath = string.Empty;
        XmlFiles updaterXmlFiles = null;
        private int availableUpdate = 0;
        #region Ping方法
        public bool Ping(string ip)
        {
            try
            {
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
                options.DontFragment = true;
                string data = "Test Data!";
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
                int timeout = 1000;//Timeout时间，单位：毫秒。
                System.Net.NetworkInformation.PingReply reply = p.Send(ip, timeout, buffer, options);
                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion
        bool ISIp(string ipStr)
        {

            System.Net.IPAddress ip;
            if (System.Net.IPAddress.TryParse(ipStr, out ip))
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        private void FormLogin_Load(object sender, EventArgs e)
        {


            try
            {
                #region 自动更新
                /*------------------自动更新----------------------------------*/
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
                string ip = "";
                string sql = " select * from appcfg where configkey = 'ServiceIp'  ";
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ip = dt.Rows[0]["value"].ToString().Trim();
                }
                if (!ISIp(ip))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("配置的自动更新服务器IP地址不是有效的ip地址!");
                    return;
                }
                if (Ping(ip))//这里填写自动更新服务器IP
                {
                    #region 自动启动AutoUpdate
                    string localXmlFile = Application.StartupPath + "\\UpdateList.xml";
                    string serverXmlFile = string.Empty;


                    try
                    {
                        //从本地读取更新配置文件信息
                        updaterXmlFiles = new XmlFiles(localXmlFile);

                    }
                    catch
                    {
                        MessageBox.Show("更新配置文件出错，暂时无法获取新版本！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //获取服务器地址
                    updateUrl = updaterXmlFiles.GetNodeValue("//Url");
                    AppUpdater appUpdater = new AppUpdater();
                    appUpdater.UpdaterURL = updateUrl + "/UpdateList.xml";

                    //与服务器连接，下载更新配置文件。
                    try
                    {
                        tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
                        appUpdater.DownAutoUpdateFile(tempUpdatePath);
                    }
                    catch
                    {
                        MessageBox.Show("与服务器连接失败,操作超时!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                        return;
                    }

                    //获取更新文件列表
                    Hashtable htUpdateFile = new Hashtable();

                    serverXmlFile = tempUpdatePath + "\\UpdateList.xml";
                    if (!File.Exists(serverXmlFile))
                    {
                        MessageBox.Show("没有发现服务器更新文件列表！");
                        return;
                    }

                    availableUpdate = appUpdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);
                    if (availableUpdate > 0)
                    {
                        //this.Close();
                        //this.Hide();
                        Application.Exit();

                        System.Diagnostics.Process.Start("AutoUpdate.exe");
                        System.Threading.Thread.Sleep(1000);//延时1秒

                        return;
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("无法Ping通服务器IP:" + ip + "，暂时无法判断软件版本更新！");
                }

                /*----------------------------------------------------*/
                #endregion
                labelControlWait.Visible = false;
                progressBarControlWait.Visible = false;
                LoadVersion();
            }
            catch (Exception ex)
            {
                this.CustomMessageBox.MessageShow(ex.Message);
                m_FormMain.FormClosing -= new FormClosingEventHandler(m_FormMain.FormMain_FormClosing);
                Application.Exit();
            }
        }


        private void textBoxUserID_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>  
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void pictureEditLogIn_Click(object sender, EventArgs e)
        {
            ProcessLogin();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureEditExit_Click(object sender, EventArgs e)
        {
            ExitLogin();
        }

        public DialogResult MessageShow(string message, CustomMessageBoxKind kind)
        {

            Collection<string> messageInfos = new Collection<string>();

            messageInfos.Add(message);

            MessageBoxInstance.SetMessageInfo(messageInfos);
            MessageBoxInstance.SetButtonInfo(kind);
            //MessageBoxInstance.SetIconInfo(kind);
            return m_messagebox.ShowDialog();
        }

        public DialogResult MessageShow(string message)
        {
            return MessageShow(message, CustomMessageBoxKind.InformationOk);
        }

        private Point mouseOffset;//记录鼠标指针的坐标
        private bool isMouseDown;//记录鼠标左键是否按下
        private void panelForm_MouseDown(object sender, MouseEventArgs e)
        {
            int xOffset;
            int yOffset;
            if (e.Button == MouseButtons.Left)
            {
                xOffset = -e.X /*- SystemInformation.FrameBorderSize.Width*/;//减去的属性为窗体边线宽
                yOffset = -e.Y /*- SystemInformation.CaptionHeight - SystemInformation.FrameBorderSize.Height*/;//减去的两个属性分别是标题栏高、窗体边线高。如果FormBorderStyle = None，则上述两句代码不需要减去后面的属性值

                mouseOffset = new Point(xOffset, yOffset);
                isMouseDown = true;
            }
        }

        private void panelForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)//按下的是鼠标左键
            {
                isMouseDown = false;
            }
        }

        private void panelForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                //获取鼠标的位置
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                Location = mousePos;
            }
        }
    }

    /// <summary>
    /// 加载登录
    /// </summary>
    public class LoadLogin : IFrameStartup
    {
        #region IFrameStartup Members

        /// <summary>
        /// 运行
        /// </summary>
        /// <returns></returns>
        public bool Run()
        {
            FormLogin frmlogin = new FormLogin();
            return (DialogResult.OK == frmlogin.ShowDialog());
        }

        #endregion
    }
}