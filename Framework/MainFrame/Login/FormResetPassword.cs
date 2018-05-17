using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DrectSoft.MainFrame.Login
{
    /// <summary>
    /// 重置密码界面
    /// Add by xlb 2013-03-16
    /// </summary>
    public partial class FormResetPassword : DevBaseForm
    {
        Account m_Acnt;
        string userId;

        #region 方法 Add by xlb 2013-03-16

        public FormResetPassword(string userID, Account account)
        {
            try
            {
                InitializeComponent();
                m_Acnt = account;
                RegisterEvent();
                userId = userID;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 注册事件方法
        /// Add by xlb 2013-03-16
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                btnOk.Click += new EventHandler(btnOk_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 校验方法
        /// Add by  xlb 2013-03-16
        /// </summary>
        /// <param name="message"></param>
        private bool Check(ref string message)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
                {
                    message = "密码不能为空";
                    txtNewPassword.Focus();
                    return false;
                }
                else if (txtNewPassword.Text.Trim().Length > 32)
                {
                    message = "密码长度不能超过32位";
                    txtNewPassword.Focus();
                    return false;
                }
                else if (txtConfirmPass.Text.Trim() != txtNewPassword.Text.Trim())
                {
                    message = "两次密码输入不一致，请重新输入";
                    txtNewPassword.Text = string.Empty;
                    txtConfirmPass.Text = string.Empty;
                    txtNewPassword.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add by xlb 2013-03-18
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns>1弱 2中 3强</returns>
        private int ValidateZiMu(string pwd)
        {
            try
            {
                if (pwd.Trim().Length <= 6)
                {
                    return 1;
                }
                //数字加字母
                string strpattern2 = @"^[a-z]*\d*[a-z]+\d+[a-z]*\d*$i||^[a-z]*\d*\d+[a-z]+[a-z]*\d*$i";
                //纯数字
                string strpattern1 = @"^\d*$";

                bool match1 = Regex.IsMatch(pwd, strpattern1);
                if (match1)
                {
                    return 2;
                }
                bool match2 = Regex.IsMatch(pwd, strpattern2);
                if (match2)
                {
                    return 3;
                }
                return 3;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region 事件 Add by xlb 2013-03-16

        /// <summary>
        /// 窗体加载事件
        /// Add by xlb 2013-03-16
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormResetPassword_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = "您的密码为初始密码，请重置密码";
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 确定事件
        /// Add by xlb 2013-03-16
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                if (!Check(ref message))
                {//校验不通过显示提示信息
                    labelControl7.Visible = true;
                    labelControl7.Text = message;
                    labelControl7.ForeColor = Color.Green;
                    return;
                }
                m_Acnt.ChangePassword(userId, null, txtNewPassword.Text.Trim());
                MessageBox.Show("修改成功");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 密码文本框文本改变触发事件
        /// Add by xlb 2013-03-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string pwd = txtNewPassword.Text;
                int validateNum = ValidateZiMu(pwd);
                switch (validateNum)
                {
                    case 1:
                        lblValidate.Text = "弱";
                        lblValidate.Visible = true;
                        lblValidate.ForeColor = Color.Red;
                        break;
                    case 2:
                        lblValidate.Text = "中";
                        lblValidate.Visible = true;
                        lblValidate.ForeColor = Color.Orange;
                        break;
                    case 3:
                        lblValidate.Text = "强";
                        lblValidate.Visible = true;
                        lblValidate.ForeColor = Color.Green;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}