#region Using directives

using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using DrectSoft.Core.Permission;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#endregion

namespace DrectSoft.MainFrame
{
    partial class FormChangePassword : DevBaseForm
    {
        #region private var

        DSMessageBox m_messagebox;

        private IUser m_User;
        private IEmrHost m_app;
        const string col_UserId = "UserID";
        const string col_UserName = "Name";
        const string col_StartDate = "startdate";
        const string col_EndDate = "enddate";
        const string str_QuerySql = "select b.UserID,b.StartDate,b.EndDate,a.Name from TempUsers b "
                                        + " left Join Users a on b.UserID=a.ID where b.MasterID='{0}'";

        const string str_DeleteUserSql = "delete from Users where ID='{0}'";
        const string str_DeleteMasterSql = "delete from TempUsers where UserID='{0}' and MasterID='{1}'";



        Account m_Acnt;

        private DataTable TempUsers
        {
            get
            {
                if (_tempUsers == null)
                {
                    InitTempUserSchema();
                }
                return _tempUsers;

            }
        }
        private DataTable _tempUsers;



        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="user"></param>
        public FormChangePassword(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_User = app.User;
                m_app = app;
                m_Acnt = new Account();
                m_messagebox = new DSMessageBox();
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        private void InitTempUserSchema()
        {
            try
            {
                DataColumn colid = new DataColumn(col_UserId);
                DataColumn colname = new DataColumn(col_UserName);
                DataColumn colstart = new DataColumn(col_StartDate);
                DataColumn colend = new DataColumn(col_EndDate);
                _tempUsers = new DataTable();
                _tempUsers.Columns.Add(colid);
                _tempUsers.Columns.Add(colname);
                _tempUsers.Columns.Add(colstart);
                _tempUsers.Columns.Add(colend);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        private void LoadTempUsers()
        {
            try
            {
                TempUsers.Clear();
                DataTable sourceTable = m_app.SqlHelper.ExecuteDataTable(string.Format(str_QuerySql, m_app.User.Id));
                foreach (DataRow row in sourceTable.Rows)
                {
                    DataRow newRow = TempUsers.NewRow();
                    newRow["UserID"] = row["UserID"];
                    newRow["Name"] = row["Name"];
                    newRow["StartDate"] = row["StartDate"];
                    newRow["EndDate"] = row["EndDate"];
                    TempUsers.Rows.Add(newRow);
                }
                gridControl1.DataSource = TempUsers;
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }


        private DataRow GetFocusedRow()
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0) return null;
                DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                return row;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void FormChangePassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                    this.SelectNextControl(this.ActiveControl, true, true, true, false);
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
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


        /// <summary>
        /// 确定事件
        /// edit by cyq 2012-11-02
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }

                m_Acnt.ChangePassword(m_User.Id, textBoxOldPassword.Text, textBoxNewPassword.Text);
                FormMain.Instance.CustomMessageBox.MessageShow("修改成功", CustomMessageBoxKind.InformationOk);
                this.Close();
            }
            catch (Exception ex)
            {
                this.textBoxOldPassword.Focus();
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 画面验证
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-02</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(this.textBoxNewPassword.Text))
                {
                    this.textBoxNewPassword.Focus();
                    return "新密码不能为空";
                }
                else if (this.textBoxNewPassword.Text.Contains(" "))
                {
                    this.textBoxNewPassword.Text = string.Empty;
                    this.textBoxConform.Text = string.Empty;
                    this.textBoxNewPassword.Focus();
                    return "新密码不能包含空格";
                }
                else if (this.textBoxNewPassword.Text != this.textBoxConform.Text)
                {
                    this.textBoxNewPassword.Text = string.Empty;
                    this.textBoxConform.Text = string.Empty;
                    this.textBoxNewPassword.Focus();
                    return "新密码和确认密码不一致，请重新输入。";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return string.Empty;
        }

        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void repositoryItemDateEdit1_QueryCloseUp(object sender, CancelEventArgs e)
        {
            try
            {
                DataRow focuseRow = GetFocusedRow();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }


        }

        private void FormChangePassword_Load(object sender, EventArgs e)
        {
            try
            {
                LoadTempUsers();
                if (((Users)m_app.User).Status == 0)//临时用户隐藏相关功能
                {
                    xtraTabPage2.PageVisible = false;
                }
                this.ActiveControl = this.textBoxOldPassword;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow focuseRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (focuseRow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选中一条临时账号记录");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除该临时账号吗？", "删除临时账号", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                m_app.SqlHelper.ExecuteNoneQuery(string.Format(str_DeleteMasterSql, focuseRow["UserID"], m_app.User.Id));
                m_app.SqlHelper.ExecuteNoneQuery(string.Format(str_DeleteUserSql, focuseRow["UserID"]));
                gridView1.DeleteRow(gridView1.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                NewTempUser newForm = new NewTempUser(m_app);
                if (newForm.ShowDialog() == DialogResult.OK)
                {
                    LoadTempUsers();
                }
                LoadTempUsers();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textBoxNewPassword_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string pwd = textBoxNewPassword.Text;
                int validateNum = ValidateZiMu(pwd);
                switch (validateNum)
                {
                    case 1:
                        lblValidate.Text = "弱";
                        lblValidate.ForeColor = Color.Red;
                        break;
                    case 2:
                        lblValidate.Text = "中";
                        lblValidate.ForeColor = Color.Orange;
                        break;
                    case 3:
                        lblValidate.Text = "强";
                        lblValidate.ForeColor = Color.Green;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 长度小于6 弱
        /// 长度大于6但纯数字 中
        /// 长度大于6 字母数字混合 强
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

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-01</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// tab页切换事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-01</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (this.xtraTabControl1.SelectedTabPage == xtraTabPage1)
                {
                    textBoxOldPassword.Focus();
                }
                else if (this.xtraTabControl1.SelectedTabPage == xtraTabPage2)
                {
                    btn_Add.Focus();
                }
                else if (this.xtraTabControl1.SelectedTabPage == xtraTabPagePicSign)
                {
                    BindTable();
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 绑定图片签名窗体
        /// <auth>zyx</auth>
        /// <date>2012-12-13</date>
        /// </summary>
        private void BindTable()
        {
            try
            {
                string sqlStr = "select a.name UserName,a.id  UserId ,b.name DepartmentName,c.userpicflow, case when c.Valide=1 then '是' else '否' end as  Valide from users a left join department b on a.deptid=b.id left join userpicsign  c on a.id=c.userid where a.id=@userid";
                SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@userid", SqlDbType.NVarChar) };
                parms[0].Value = m_app.User.Id;
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, parms, CommandType.Text);
                DataRow dr = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                }
                PicSignForm picSignForm = new PicSignForm(m_app, dr);
                picSignForm.FormClosed += new FormClosedEventHandler(picSignForm_FormClosed);
                picSignForm.Dock = DockStyle.Fill;
                picSignForm.TopLevel = false;
                picSignForm.FormBorderStyle = FormBorderStyle.None;
                picSignForm.Show();
                xtraTabPagePicSign.Controls.Clear();
                xtraTabPagePicSign.Controls.Add(picSignForm);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        void picSignForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}