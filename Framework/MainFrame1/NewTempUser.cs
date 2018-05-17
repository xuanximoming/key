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
using DrectSoft.DSSqlHelper;

using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
namespace DrectSoft.MainFrame
{
    public partial class NewTempUser : DevBaseForm
    {
        //当前页面的状态
        public EditState m_EditState = EditState.View;
        private IEmrHost m_App;

        private DataTable UsersData
        {
            get
            {
                if (_usersData == null)
                {
                    _usersData = m_App.SqlHelper.ExecuteDataTable("select * from Users where 1=1 ");
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
                    //_tempUsers = m_App.SqlHelper.ExecuteDataTable("select * from TempUsers a inner join users b on a.masterid=b.id where 1=1 ");
                    _tempUsers = m_App.SqlHelper.ExecuteDataTable("select * from TempUsers where 1=1 order by userid  ");
                    m_App.SqlHelper.ResetTableSchema(_tempUsers, "TempUsers");
                }
                return _tempUsers;
            }

        }
        private DataTable _tempUsers;


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

        private void AddNewRow()
        {

            try
            {
                //从3000以后的作为临时用户
                //todo 权限控制
                DataRow newRow = UsersData.NewRow();
                txt_ID.Text = "00123";
                if (txt_ID.Text.Trim() != "00")
                {
                    string extraCodeLong = txt_ID.Text.Trim().Insert(0, "000000");
                    string strLoginCode = extraCodeLong.Substring(extraCodeLong.Length - 6, 6);

                    newRow["ID"] = strLoginCode;
                }

                newRow["Name"] = txt_Name.Text;
                newRow["DEPTID"] = m_App.User.CurrentDeptId;
                newRow["WARDID"] = m_App.User.CurrentWardId;
                //newRow["NarcosisMark"] = DBNull.Value;
                DateTime now = DateTime.Now;
                string encryptDateTime = now.ToString("yyyyMMdd") + now.ToString("T");
                newRow["Passwd"] = HisEncryption.EncodeString(
                            encryptDateTime, HisEncryption.PasswordLength, txt_Pswd.Text.Trim().ToString());
                newRow["RegDate"] = encryptDateTime;
                newRow["JOBID"] = m_App.User.GWCodes;
                newRow["Operator"] = m_App.User.Id;
                //string regdate = string.Empty;
                //CreateUserPassword(txt_Pswd.Text, out regdate, out pswd);
                //newRow["Passwd"] = pswd;
                //newRow["RegDate"] = regdate;
                //newRow["Operator"] = m_App.User.ID;
                newRow["Valid"] = 1;
                newRow["Status"] = 0;
                UsersData.Rows.Add(newRow);

                DataRow newTpRow = TempUsers.NewRow();
                if (txt_ID.Text.Trim() != "00")
                {
                    string extraCodeLong = txt_ID.Text.Trim().Insert(0, "000000");
                    string strLoginCode = extraCodeLong.Substring(extraCodeLong.Length - 6, 6);

                    newTpRow["UserID"] = strLoginCode;
                }

                // newTpRow["Name"] = txt_Name.Text;
                newTpRow["MasterID"] = m_App.User.Id;
                newTpRow["StartDate"] = dateEditstart.Text;
                newTpRow["EndDate"] = dateEditend.Text;
                TempUsers.Rows.Add(newTpRow);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
        }

        /// <summary>
        /// 临时工号的插入方法
        /// <auth>张业兴</auth>
        /// <date>2012-12-18</date>
        /// </summary>
        private void Insert()
        {
            try
            {
                string strLoginCode = string.Empty;
                SqlParameter[] parms;
                if (txt_ID.Text.Trim() != "00")
                {
                    string extraCodeLong = txt_ID.Text.Trim().Insert(0, "000000");
                    strLoginCode = extraCodeLong.Substring(extraCodeLong.Length - 6, 6);
                }
                if (ValidateExist(strLoginCode, "users") < 0 && ValidateExist(strLoginCode, "tempUsers") < 0)
                {
                    //向USERS表中插入新数据
                    string insertUsers = "insert into USERS(ID,NAME,DEPTID,WARDID,CATEGORY,GRADE,PASSWD,REGDATE,JOBID,OPERATOR,VALID,STATUS)values(@ID,@NAME,@DEPTID,@WARDID,(select a.category from Users a where a.valid =1 and a.ID =@OPERATOR),(select a.grade from Users a where a.valid =1 and a.ID =@OPERATOR),@PASSWD,@REGDATE,@JOBID,@OPERATOR,@VALID,@STATUS)";
                    parms = new SqlParameter[]
                        {
                           new SqlParameter("@ID",SqlDbType.NVarChar,6),
                           new SqlParameter("@NAME",SqlDbType.NVarChar,16),
                           new SqlParameter("@DEPTID",SqlDbType.NVarChar,12),
                           new SqlParameter("@WARDID",SqlDbType.NVarChar,12),
                           new SqlParameter("@PASSWD",SqlDbType.NVarChar,32),
                           new SqlParameter("@REGDATE",SqlDbType.NVarChar,19),
                           new SqlParameter("@JOBID",SqlDbType.NVarChar,255),
                           new SqlParameter("@OPERATOR",SqlDbType.NVarChar,6),
                           new SqlParameter("@VALID",SqlDbType.Int),
                           new SqlParameter("@STATUS",SqlDbType.Int)
                        };
                    parms[0].Value = strLoginCode;
                    parms[1].Value = txt_Name.Text.Trim().ToString();
                    parms[2].Value = m_App.User.CurrentDeptId;
                    parms[3].Value = m_App.User.CurrentWardId;
                    DateTime now = DateTime.Now;
                    string encryptDateTime = now.ToString("yyyyMMdd") + now.ToString("T");
                    parms[4].Value = HisEncryption.EncodeString(encryptDateTime, HisEncryption.PasswordLength, txt_Pswd.Text.Trim().ToString());
                    parms[5].Value = encryptDateTime;
                    parms[6].Value = m_App.User.GWCodes;
                    parms[7].Value = m_App.User.Id;
                    parms[8].Value = 1;
                    parms[9].Value = 0;
                    int count = DS_SqlHelper.ExecuteNonQuery(insertUsers, parms, CommandType.Text);

                    //向TempUsers表中插入新数据
                    string insertTempUsers = "insert into TEMPUSERS(USERID,MASTERID,STARTDATE,ENDDATE) values(@USERID,@MASTERID,@STARTDATE,@ENDDATE)";
                    parms = new SqlParameter[] 
                    {
                        new SqlParameter("@USERID",SqlDbType.NVarChar,6),
                        new SqlParameter("@MASTERID",SqlDbType.NVarChar,6),
                        new SqlParameter("@STARTDATE",SqlDbType.Char,19),
                        new SqlParameter("@ENDDATE",SqlDbType.Char,19)
                    };
                    parms[0].Value = strLoginCode;
                    parms[1].Value = m_App.User.Id;
                    parms[2].Value = dateEditstart.Text;
                    parms[3].Value = dateEditend.Text;
                    DS_SqlHelper.ExecuteNonQuery(insertTempUsers, parms, CommandType.Text);
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该临时工号已经存在，新增失败！");
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 判断临时工号是否存在
        /// <auth>张业兴</auth>
        /// <date>2012-12-18</date>
        /// </summary>
        /// <param name="id">临时工号</param>
        /// <param name="table">表名</param>
        /// <returns>受影响的行数</returns>
        private int ValidateExist(string id, string table)
        {
            try
            {
                int count = 0;//记录受影响的行数
                string strSqlUsersById = "select * from users where id=@id";
                string strSqlTempUsers = "select * from TEMPUSERS where userid=@userid";
                SqlParameter[] parms;
                switch (table)
                {
                    case "users":
                        parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.NVarChar, 6) };
                        parms[0].Value = id;
                        count = DS_SqlHelper.ExecuteNonQuery(strSqlUsersById, parms, CommandType.Text);
                        break;
                    case "tempUsers":
                        parms = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.NVarChar, 6) };
                        parms[0].Value = id;
                        count = DS_SqlHelper.ExecuteNonQuery(strSqlTempUsers, parms, CommandType.Text);
                        break;
                }
                return count;
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
                //if (!CommitData())
                //    return;
                AddNewRow();
                //_tempUsers = m_App.SqlHelper.ExecuteDataTable("select * from TempUsers a left join users b on a.masterid=b.id where 1=1 ");
                // m_App.SqlHelper.ResetTableSchema(_tempUsers, "TempUsers");
                m_App.SqlHelper.UpdateTable(UsersData, "Users", false);
                m_App.SqlHelper.UpdateTable(TempUsers, "TempUsers", false);

            }
            catch (SqlException ex)
            {
                m_App.CustomMessageBox.MessageShow(ex.Message);
            }
            finally
            {
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="app"></param>
        public NewTempUser(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_App = app;
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
                txt_ID.Text = rand.Next(3000, 8000).ToString();
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
            

        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }
                //Save2Data();
                Insert();
                this.Close();
                this.DialogResult = DialogResult.OK;
                m_EditState = EditState.Add;
                _usersData = null;
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

        /// <summary>
        /// 画面检查
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-01</date>
        /// </summary>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(txt_Name.Text))
                {
                    this.txt_Name.Focus();
                    return "用户名不能为空";
                }
                else if (txt_Name.Text.Contains(" "))
                {
                    this.txt_Name.Focus();
                    return "用户名不能包含空格";
                }
                else if (string.IsNullOrEmpty(this.txt_Pswd.Text))
                {
                    txt_Pswd.Focus();
                    return "密码不能为空";
                }
                else if (this.txt_Pswd.Text.Contains(" "))
                {
                    txt_Pswd.Text = string.Empty;
                    txt_Pswd2.Text = string.Empty;
                    txt_Pswd.Focus();
                    return "密码不能包含空格";
                }
                else if (!txt_Pswd.Text.Equals(txt_Pswd2.Text))
                {
                    txt_Pswd.Text = string.Empty;
                    txt_Pswd2.Text = string.Empty;
                    txt_Pswd.Focus();
                    return "密码和确认密码不一致，请重新输入。";
                }
                else if (dateEditstart.DateTime > dateEditend.DateTime)
                {
                    dateEditstart.Focus();
                    return "开始日期不能大于结束日期";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 清空事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-02</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            try
            {
                ;
                this.txt_Name.Text = string.Empty;
                this.txt_Pswd.Text = string.Empty;
                this.txt_Pswd2.Text = string.Empty;
                this.dateEditstart.Text = string.Empty;
                this.dateEditend.Text = string.Empty;
                this.txt_Name.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
