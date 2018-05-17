using MainFrame;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DrectSoft.Core;

namespace DrectSoft.MainFrame
{
    public partial class FormLogin : Form
    {
        private const string noVerifyUser = "NoVerify";
        private Account m_Acnt;
        private DrectSoftLog m_Log;
        private FormMain m_FormMain;
        private Account m_MasterAcnt;
        private bool isloging = false;
        private string m_TempUserID = string.Empty;
        public bool ontherFlg = false;
        private IContainer components = null;
        public Account CurrentAccount
        {
            get
            {
                return this.m_Acnt;
            }
        }

        public Account MasterAccount
        {
            get
            {
                return this.m_MasterAcnt;
            }
        }

        public FormLogin(FormMain formMain)
            : this()
        {
            this.m_FormMain = formMain;
        }

        public FormLogin()
        {
            this.InitializeComponent();
            this.InitializeUI();
            this.m_Acnt = new Account();
            this.m_MasterAcnt = new Account();
            this.m_Log = FormMain.Instance.Logger;
            base.TransparencyKey = this.BackColor;
            base.TopMost = true;
        }

        private void InitializeUI()
        {
            try
            {
                this.buttonLogIn.BackgroundImageLayout = ImageLayout.Stretch;
                if (Program.m_StrUserId != string.Empty && Program.m_StrUserId != null)
                {
                    this.textBoxUserID.Text = Program.m_StrUserId;
                }
                Program.m_StrUserId = string.Empty;
            }
            catch (IOException ex)
            {
                this.m_Log.Error("加载窗口背景图失败:" + ex.Message);
            }
        }

        private void hyperLinkEdit1_Click(object sender, EventArgs e)
        {
            this.ExitLogin();
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.ProcessLogin();
            }
        }

        public void textBoxUserID_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxUserID.Text))
            {
                try
                {
                    bool flag = true;
                    if (this.textBoxUserID.Text.Trim() != "00")
                    {
                        string str = "";
                        for (int i = this.textBoxUserID.Text.Trim().Length; i < 6; i++)
                        {
                            str += "0";
                        }
                        this.textBoxUserID.Text = str + this.textBoxUserID.Text.Trim();
                    }
                    string aUserName = this.m_Acnt.GetAUserName(this.textBoxUserID.Text, out flag);
                    string masterID = this.m_Acnt.User.MasterID;
                    if (!string.IsNullOrEmpty(masterID))
                    {
                        this.m_MasterAcnt.InitMaster(masterID);
                    }
                    this.m_TempUserID = this.textBoxUserID.Text;
                    this.textBoxUserID.Text = aUserName;
                    if (!flag)
                    {
                        throw new Exception("不存在当前用户");
                    }
                }
                catch (Exception ex)
                {
                    this.m_Log.Error(ex.Message);
                    this.textBoxUserID.Text = ex.Message;
                    this.textBoxUserID.Focus();
                }
            }
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            ProcessLogin();
        }

        private void FormLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                base.SelectNextControl(base.ActiveControl, true, true, true, false);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ExitLogin();
        }

        private void ExitLogin()
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void ProcessLogin()
        {
            if (!this.isloging)
            {
                this.isloging = true;
                this.memoEdit1.Visible = true;
                this.progressBarControlWait.Visible = true;
                this.labelControlWait.Visible = true;
                this.progressBarControlWait.Properties.ShowTitle = true;
                this.progressBarControlWait.Properties.PercentView = true;
                this.ChangeProgressBar(0, 20);
                if (this.CheckNeedVerify())
                {
                    if (!this.LoginDb())
                    {
                        this.isloging = false;
                        return;
                    }
                }
                this.m_FormMain.ProcessLogin();
                this.ChangeProgressBar(91, 100);
                this.isloging = false;
            }
        }

        public void ChangeProgressBar(int fromValue, int toValue)
        {
            for (int i = fromValue; i <= toValue; i++)
            {
                this.progressBarControlWait.Position = i;
                if (i >= 100)
                {
                    base.DialogResult = DialogResult.OK;
                }
                Thread.Sleep(10);
                Application.DoEvents();
            }
        }

        private bool CheckNeedVerify()
        {
            AppSettingsReader appSettingsReader = new AppSettingsReader();
            string a = "";
            try
            {
                a = (string)appSettingsReader.GetValue("NoVerify", typeof(string));
            }
            catch
            {
                Trace.WriteLine("CanSkipVerify Config Not Find!");
            }
            return a != bool.TrueString;
        }

        private bool LoginDb()
        {
            bool result;
            try
            {
                this.m_Acnt.Login(this.m_TempUserID, this.textBoxPassword.Text, 0);
                string masterID = this.m_Acnt.User.MasterID;
                if (!string.IsNullOrEmpty(masterID))
                {
                    this.m_MasterAcnt.InitMaster(masterID);
                }
                this.m_Log.Info(string.Concat(new string[]
				{
					"用户:",
					this.m_Acnt.User.Name,
					"于",
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					"成功登录系统"
				}));
            }
            catch (Exception ex)
            {
                this.textBoxUserID.Focus();
                this.textBoxUserID.Text = ex.Message;
                this.textBoxUserID.SelectAll();
                this.progressBarControlWait.Position = 0;
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (base.Opacity < 1.0)
            {
                base.Opacity += 0.5;
            }
            else
            {
                this.timer1.Enabled = false;
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            this.memoEdit1.Visible = false;
            this.labelControlWait.Visible = false;
            this.progressBarControlWait.Visible = false;
            if (this.textBoxUserID.Text.Trim() != string.Empty)
            {
                this.ProcessLogin();
            }
        }

        private void textBoxUserID_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
        }

        private void pictureEditLogIn_Click(object sender, EventArgs e)
        {
            this.ProcessLogin();
        }

        private void pictureEditExit_Click(object sender, EventArgs e)
        {
            this.ExitLogin();
        }
    }
}
