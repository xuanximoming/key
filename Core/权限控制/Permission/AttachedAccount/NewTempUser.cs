using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.Permission
{
    public partial class NewTempUser : DevBaseForm
    {


        private IEmrHost m_App;
        private string m_UserId = string.Empty;

        /*整个表复制方法在Oracle中不可用，修改为Insert语句插入数据
          private DataTable UsersData
          {
              get
              {
                  if (_usersData == null)
                  {
                      _usersData = m_App.SqlHelper.ExecuteDataTable("select * from Users where 1=1");
                      m_App.SqlHelper.ResetTableSchema(_usersData, "Users");
                  }
                  return _usersData;
              }

          }
          private DataTable _usersData;

          private DataTable TempUsers
          {

              get
              {
                  if (_tempUsers == null)
                  {
                      _tempUsers = m_App.SqlHelper.ExecuteDataTable("select * from TempUsers where 1=1");
                      m_App.SqlHelper.ResetTableSchema(_tempUsers, "TempUsers");
                  }
                  return _tempUsers;
              }

          }
          private DataTable _tempUsers;

          private void AddNewRow()
          {

              //从3000以后的作为临时用户
              DataRow newRow = UsersData.NewRow();
              newRow["ID"] = txt_ID.Text;
              newRow["Name"] = txt_Name.Text;
              newRow["Category"] = m_App.User.Category;
              newRow["RecipeMark"] = DBNull.Value;
              newRow["NarcosisMark"] = DBNull.Value;
              string pswd = string.Empty;
              string regdate = string.Empty;
              CreateUserPassword(txt_Pswd.Text, out regdate, out pswd);
              newRow["Passwd"] = pswd;
              newRow["RegDate"] = regdate;
              //newRow["Operator"] = m_App.User.ID;
              newRow["Operator"] = m_UserId;
              newRow["Valid"] = 1;
              newRow["Status"] = 0;
              UsersData.Rows.Add(newRow);

              DataRow newTpRow = TempUsers.NewRow();
              newTpRow["UserID"] = txt_ID.Text;
              newTpRow["MasterID"] = m_App.User.ID;
              newTpRow["StartDate"] = dateEditstart.Text;
              newTpRow["EndDate"] = dateEditend.Text;
              TempUsers.Rows.Add(newTpRow);
          }
         * */

        private bool CommitData()
        {
            try
            {
                if (string.IsNullOrEmpty(txt_Name.Text))
                {
                    m_App.CustomMessageBox.MessageShow("用户名不能为空！");
                    return false;
                }

                if (dateEditstart.DateTime > dateEditend.DateTime)
                {
                    m_App.CustomMessageBox.MessageShow("输入日期不对！");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void CreateUserPassword(string password, out string encryptDateTime, out string encryptNewPassword)
        {
            try
            {
                DateTime now = DateTime.Now;
                encryptDateTime = now.ToString("yyyyMMdd") + now.ToString("T");
                encryptNewPassword = HisEncryption.EncodeString(
                   encryptDateTime, HisEncryption.PasswordLength, password);
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void Save2Data()
        {
            try
            {
                if (!CommitData())
                    return;
                //AddNewRow();
                //m_App.SqlHelper.UpdateTable(UsersData, "Users", false);
                //m_App.SqlHelper.UpdateTable(TempUsers, "TempUsers", false);
                string id = txt_ID.Text;
                string name = txt_Name.Text;
                string category = string.Empty;
                string recipeMark = string.Empty;
                string narcosisMark = string.Empty;
                string pswd = string.Empty;
                string regdate = string.Empty;
                CreateUserPassword(txt_Pswd.Text, out regdate, out pswd);
                string operatorId = m_UserId;
                string valid = "1";
                string status = "0";
                //string sqlInsertUsers = "insert into Users(ID,Name,Category,Recipemark,Narcosismark,Passwd,Regdate,Operator,Valid,Status,deptid,wardid,grade,jobid) "
                //          + "values('" + id + "','" + name + "',(select a.category from Users a where a.valid =1 and a.ID = '" + operatorId + "'),'"
                //          + recipeMark + "','" + narcosisMark + "','" + pswd + "','" + regdate + "','" + operatorId + "','" + valid + "','"
                //          + status + "',(select deptid,wardid,grade,jobid from Users a where a.valid=1 and a.ID='" + operatorId + "'))";
                string sqlInsertUsers = "insert into Users(ID,Name,Category,Recipemark,Narcosismark,Passwd,Regdate,Operator,Valid,Status) "
                                    + "values('" + id + "','" + name + "',(select a.category from Users a where a.valid =1 and a.ID = '" + operatorId + "'),'" + recipeMark + "','" + narcosisMark + "','" + pswd + "','" + regdate + "','" + operatorId + "','" + valid + "','" + status + "')";

                string updateSql = string.Format(@"update Users set (deptid,wardid,grade,jobid,jobtitle)=(select deptid,wardid,grade,jobid,jobtitle from Users  a where a.valid =1 and a.ID='{0}') where
                id='{1}' ", operatorId, id);
                string userID = txt_ID.Text;
                string masterID = m_UserId;
                string startDate = dateEditstart.Text;
                string endDate = dateEditend.Text;
                string sqlInsertTempusers = "insert into Tempusers(Userid,Masterid,Startdate,Enddate) "
                                        + "values('" + userID + "','" + masterID + "','" + startDate + "','" + endDate + "')";

                m_App.SqlHelper.ExecuteScalar(sqlInsertUsers);
                m_App.SqlHelper.ExecuteScalar(updateSql);
                m_App.SqlHelper.ExecuteScalar(sqlInsertTempusers);

                this.DialogResult = DialogResult.OK;
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("临时账号创建成功");
            }
            catch (SqlException ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }




        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="app"></param>
        public NewTempUser(IEmrHost app, string userId)
        {
            try
            {
                InitializeComponent();
                m_App = app;
                m_UserId = userId;
                dateEditstart.DateTime = DateTime.Now;
                dateEditend.DateTime = DateTime.Now.AddDays(7);

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void NewTempUser_Load(object sender, EventArgs e)
        {
            try
            {
                Random rand = new Random();
                txt_ID.Text = "00" + rand.Next(3000, 8000).ToString();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }


        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txt_Name.Text))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("用户名不能为空");
                    this.txt_Name.Focus();
                    return;
                }
                else if (txt_Name.Text.Contains(" "))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("用户名不能包含空格");
                    this.txt_Name.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.txt_Pswd.Text))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("密码不能为空");
                    txt_Pswd.Focus();
                    return;
                }
                else if (this.txt_Pswd.Text.Contains(" "))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("密码不能包含空格");
                    txt_Pswd.Text = string.Empty;
                    txt_Pswd2.Text = string.Empty;
                    txt_Pswd.Focus();
                    return;
                }
                else if (!txt_Pswd.Text.Equals(txt_Pswd2.Text))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("密码和确认密码不一致，请重新输入。");
                    txt_Pswd.Text = string.Empty;
                    txt_Pswd2.Text = string.Empty;
                    txt_Pswd.Focus();
                    return;
                }
                else if (this.txt_Pswd.Text.Length > 16)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("密码长度不能超过16位");
                    txt_Pswd.Focus();
                    return;
                }
                else if (dateEditstart.DateTime.ToString() == "0001-1-1 0:00:00")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("开始日期不能为空");
                    dateEditstart.Focus();
                    return;
                }
                else if (dateEditend.DateTime.ToString() == "0001-1-1 0:00:00")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("结束日期不能为空");
                    dateEditend.Focus();
                    return;
                }
                else if (dateEditstart.DateTime > dateEditend.DateTime)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("开始日期不能大于结束日期");
                    dateEditstart.Focus();
                    return;
                }
                Save2Data();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        private void txt_Pswd2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void dateEditend_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
