using DevExpress.XtraEditors;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.MainFrame
{
    public class NewTempUser : Form
    {
        private IContainer components = null;

        private PanelControl panelControl1;

        private GroupControl groupControl1;

        private LabelControl labelControl3;

        private LabelControl labelControl2;

        private LabelControl labelControl1;

        private DateEdit dateEditend;

        private LabelControl labelControl6;

        private DateEdit dateEditstart;

        private LabelControl labelControl5;

        private TextEdit txt_Pswd2;

        private LabelControl labelControl4;

        private TextEdit txt_Pswd;

        private TextEdit txt_Name;

        private TextEdit txt_ID;

        private SimpleButton btnCancel;

        private SimpleButton btnsave;

        private IEmrHost m_App;

        private DataTable _usersData;

        private DataTable _tempUsers;

        private DataTable UsersData
        {
            get
            {
                if (this._usersData == null)
                {
                    this._usersData = this.m_App.SqlHelper.ExecuteDataTable("select * from Users where 1=1");
                    this.m_App.SqlHelper.ResetTableSchema(this._usersData, "Users");
                }
                return this._usersData;
            }
        }

        private DataTable TempUsers
        {
            get
            {
                if (this._tempUsers == null)
                {
                    this._tempUsers = this.m_App.SqlHelper.ExecuteDataTable("select * from TempUsers where 1=1");
                    this.m_App.SqlHelper.ResetTableSchema(this._tempUsers, "TempUsers");
                }
                return this._tempUsers;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTempUser));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnsave = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.dateEditend = new DevExpress.XtraEditors.DateEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.dateEditstart = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Pswd2 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txt_Pswd = new DevExpress.XtraEditors.TextEdit();
            this.txt_Name = new DevExpress.XtraEditors.TextEdit();
            this.txt_ID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditend.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditend.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditstart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditstart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Pswd2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Pswd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnsave);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 155);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(369, 33);
            this.panelControl1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(199, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnsave
            // 
            this.btnsave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnsave.Location = new System.Drawing.Point(76, 6);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(75, 23);
            this.btnsave.TabIndex = 0;
            this.btnsave.Text = "确定";
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.dateEditend);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.dateEditstart);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.txt_Pswd2);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.txt_Pswd);
            this.groupControl1.Controls.Add(this.txt_Name);
            this.groupControl1.Controls.Add(this.txt_ID);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(369, 155);
            this.groupControl1.TabIndex = 2;
            // 
            // dateEditend
            // 
            this.dateEditend.EditValue = null;
            this.dateEditend.Location = new System.Drawing.Point(257, 116);
            this.dateEditend.Name = "dateEditend";
            this.dateEditend.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditend.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditend.Size = new System.Drawing.Size(100, 20);
            this.dateEditend.TabIndex = 11;
            this.dateEditend.EditValueChanged += new System.EventHandler(this.dateEditend_EditValueChanged);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(199, 119);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(12, 14);
            this.labelControl6.TabIndex = 10;
            this.labelControl6.Text = "---";
            // 
            // dateEditstart
            // 
            this.dateEditstart.EditValue = null;
            this.dateEditstart.Location = new System.Drawing.Point(76, 116);
            this.dateEditstart.Name = "dateEditstart";
            this.dateEditstart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditstart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditstart.Size = new System.Drawing.Size(100, 20);
            this.dateEditstart.TabIndex = 9;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(13, 123);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(24, 14);
            this.labelControl5.TabIndex = 8;
            this.labelControl5.Text = "期限";
            // 
            // txt_Pswd2
            // 
            this.txt_Pswd2.Location = new System.Drawing.Point(257, 78);
            this.txt_Pswd2.Name = "txt_Pswd2";
            this.txt_Pswd2.Properties.PasswordChar = '*';
            this.txt_Pswd2.Size = new System.Drawing.Size(100, 20);
            this.txt_Pswd2.TabIndex = 7;
            this.txt_Pswd2.EditValueChanged += new System.EventHandler(this.txt_Pswd2_EditValueChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(199, 81);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "重复密码";
            // 
            // txt_Pswd
            // 
            this.txt_Pswd.Location = new System.Drawing.Point(76, 81);
            this.txt_Pswd.Name = "txt_Pswd";
            this.txt_Pswd.Properties.PasswordChar = '*';
            this.txt_Pswd.Size = new System.Drawing.Size(100, 20);
            this.txt_Pswd.TabIndex = 5;
            // 
            // txt_Name
            // 
            this.txt_Name.Location = new System.Drawing.Point(257, 36);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(100, 20);
            this.txt_Name.TabIndex = 4;
            // 
            // txt_ID
            // 
            this.txt_ID.Location = new System.Drawing.Point(76, 36);
            this.txt_ID.Name = "txt_ID";
            this.txt_ID.Properties.ReadOnly = true;
            this.txt_ID.Size = new System.Drawing.Size(100, 20);
            this.txt_ID.TabIndex = 3;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(13, 81);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 14);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "密码";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(199, 39);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "姓名";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 39);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "临时工号";
            // 
            // NewTempUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 188);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewTempUser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "临时账号授权";
            this.Load += new System.EventHandler(this.NewTempUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditend.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditend.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditstart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditstart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Pswd2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Pswd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Name.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_ID.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        private bool CommitData()
        {
            bool result;
            if (string.IsNullOrEmpty(this.txt_Name.Text))
            {
                this.m_App.CustomMessageBox.MessageShow("用户名不能为空！");
                result = false;
            }
            else if (this.dateEditstart.DateTime > this.dateEditend.DateTime)
            {
                this.m_App.CustomMessageBox.MessageShow("输入日期不对！");
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        private void CreateUserPassword(string password, out string encryptDateTime, out string encryptNewPassword)
        {
            DateTime now = DateTime.Now;
            encryptDateTime = now.ToString("yyyyMMdd") + now.ToString("T");
            encryptNewPassword = HisEncryption.EncodeString(encryptDateTime, 8, password);
        }

        private void AddNewRow()
        {
        }

        private void Save2Data()
        {
            try
            {
                if (this.CommitData())
                {
                    this.AddNewRow();
                    this.m_App.SqlHelper.UpdateTable(this.UsersData, "Users", false);
                    this.m_App.SqlHelper.UpdateTable(this.TempUsers, "TempUsers", false);
                    base.DialogResult = DialogResult.OK;
                }
            }
            catch (SqlException ex)
            {
                this.m_App.CustomMessageBox.MessageShow(ex.Message);
            }
            finally
            {
            }
        }

        public NewTempUser(IEmrHost app)
        {
            this.InitializeComponent();
            this.m_App = app;
            this.dateEditstart.DateTime = DateTime.Now;
            this.dateEditend.DateTime = DateTime.Now.AddDays(7.0);
        }

        private void NewTempUser_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            this.txt_ID.Text = random.Next(3000, 8000).ToString();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (!this.txt_Pswd.Text.Equals(this.txt_Pswd2.Text))
            {
                this.m_App.CustomMessageBox.MessageShow("两次输入的秘密不一致");
                this.txt_Pswd.Text = string.Empty;
                this.txt_Pswd2.Text = string.Empty;
            }
            else
            {
                this.Save2Data();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void txt_Pswd2_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void dateEditend_EditValueChanged(object sender, EventArgs e)
        {
        }
    }
}
