using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.MainFrame
{
    internal class FormChangePassword : Form
    {
        private const string col_UserId = "UserID";

        private const string col_UserName = "Name";

        private const string col_StartDate = "startdate";

        private const string col_EndDate = "enddate";

        private const string str_QuerySql = "select b.UserID,b.StartDate,b.EndDate,a.Name from TempUsers b  left Join Users a on b.UserID=a.ID where b.MasterID='{0}'";

        private const string str_DeleteUserSql = "delete from Users where ID='{0}'";

        private const string str_DeleteMasterSql = "delete from TempUsers where UserID='{0}' and MasterID='{1}'";

        private MyMessageBox m_messagebox;

        private IUser m_User;

        private IEmrHost m_app;

        private Account m_Acnt;

        private DataTable _tempUsers;

        private IContainer components = null;

        private XtraTabControl xtraTabControl1;

        private XtraTabPage xtraTabPage1;

        private PanelControl panel1;

        private PanelControl panel2;

        private PictureBox pictureBox1;

        private PictureBox pictureBox2;

        private TextEdit textBoxConform;

        private Label label4;

        private TextEdit textBoxNewPassword;

        private Label label3;

        private Label label2;

        private TextEdit textBoxOldPassword;

        private SimpleButton buttonCancel;

        private SimpleButton buttonOK;

        private XtraTabPage xtraTabPage2;

        private GridControl gridControl1;

        private GridView gridView1;

        private GridColumn col_userid;

        private GridColumn gridColumn2;

        private GridColumn gridColumn3;

        private GridColumn gridColumn1;

        private PanelControl panelControl1;

        private RepositoryItemDateEdit repositoryItemDateEdit1;

        private RepositoryItemDateEdit repositoryItemDateEdit2;

        private SimpleButton btn_Exit;

        private SimpleButton btn_Delete;

        private SimpleButton btn_Add;

        private DataTable TempUsers
        {
            get
            {
                if (this._tempUsers == null)
                {
                    this.InitTempUserSchema();
                }
                return this._tempUsers;
            }
        }

        public FormChangePassword(IEmrHost app)
        {
            this.InitializeComponent();
            this.m_User = app.User;
            this.m_app = app;
            this.m_Acnt = new Account();
            this.m_messagebox = new MyMessageBox();
        }

        private void InitTempUserSchema()
        {
            DataColumn column = new DataColumn("UserID");
            DataColumn column2 = new DataColumn("Name");
            DataColumn column3 = new DataColumn("startdate");
            DataColumn column4 = new DataColumn("enddate");
            this._tempUsers = new DataTable();
            this._tempUsers.Columns.Add(column);
            this._tempUsers.Columns.Add(column2);
            this._tempUsers.Columns.Add(column3);
            this._tempUsers.Columns.Add(column4);
        }

        private void LoadTempUsers()
        {
            DataTable dataTable = this.m_app.SqlHelper.ExecuteDataTable(string.Format("select b.UserID,b.StartDate,b.EndDate,a.Name from TempUsers b  left Join Users a on b.UserID=a.ID where b.MasterID='{0}'", this.m_app.User.Id));
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DataRow dataRow2 = this.TempUsers.NewRow();
                dataRow2["UserID"] = dataRow["UserID"];
                dataRow2["Name"] = dataRow["Name"];
                dataRow2["StartDate"] = dataRow["StartDate"];
                dataRow2["EndDate"] = dataRow["EndDate"];
                this.TempUsers.Rows.Add(dataRow2);
            }
            this.gridControl1.DataSource = this.TempUsers;
        }

        private DataRow GetFocusedRow()
        {
            DataRow result;
            if (this.gridView1.FocusedRowHandle < 0)
            {
                result = null;
            }
            else
            {
                DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
                result = dataRow;
            }
            return result;
        }

        private void FormChangePassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                base.SelectNextControl(base.ActiveControl, true, true, true, false);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.textBoxNewPassword.Text != this.textBoxConform.Text)
            {
                FormMain.Instance.CustomMessageBox.MessageShow("两次输入的新密码不一致", CustomMessageBoxKind.ErrorOk);
                this.textBoxNewPassword.Focus();
            }
            else
            {
                try
                {
                    this.m_Acnt.ChangePassword(this.m_User.Id, this.textBoxOldPassword.Text, this.textBoxNewPassword.Text);
                    FormMain.Instance.CustomMessageBox.MessageShow("密码修改成功", CustomMessageBoxKind.InformationOk);
                    base.Close();
                }
                catch (Exception ex)
                {
                    this.textBoxOldPassword.Focus();
                    throw new Exception("密码修改失败:" + ex.Message);
                }
            }
        }

        private void gridView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void repositoryItemDateEdit1_QueryCloseUp(object sender, CancelEventArgs e)
        {
            DataRow focusedRow = this.GetFocusedRow();
        }

        private void FormChangePassword_Load(object sender, EventArgs e)
        {
            this.LoadTempUsers();
            if (((Users)this.m_app.User).Status == 0)
            {
                this.xtraTabPage2.PageVisible = false;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow dataRow = this.gridView1.GetDataRow(this.gridView1.FocusedRowHandle);
            if (dataRow != null)
            {
                this.m_app.SqlHelper.ExecuteNoneQuery(string.Format("delete from TempUsers where UserID='{0}' and MasterID='{1}'", dataRow["UserID"], this.m_app.User.Id));
                this.m_app.SqlHelper.ExecuteNoneQuery(string.Format("delete from Users where ID='{0}'", dataRow["UserID"]));
                this.gridView1.DeleteRow(this.gridView1.FocusedRowHandle);
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            NewTempUser newTempUser = new NewTempUser(this.m_app);
            if (newTempUser.ShowDialog() == DialogResult.OK)
            {
                this.LoadTempUsers();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormChangePassword));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.panel1 = new DevExpress.XtraEditors.PanelControl();
            this.panel2 = new DevExpress.XtraEditors.PanelControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.textBoxConform = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxNewPassword = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxOldPassword = new DevExpress.XtraEditors.TextEdit();
            this.buttonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.buttonOK = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_userid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Add = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Exit = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panel2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxConform.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxNewPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxOldPassword.Properties)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(433, 264);
            this.xtraTabControl1.TabIndex = 15;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.panel1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(427, 235);
            this.xtraTabPage1.Text = "密码管理";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.textBoxConform);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxNewPassword);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxOldPassword);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 235);
            this.panel1.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(427, 77);
            this.panel2.TabIndex = 20;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(78, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(349, 77);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox2.ErrorImage = null;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(78, 77);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 20;
            this.pictureBox2.TabStop = false;
            // 
            // textBoxConform
            // 
            this.textBoxConform.Location = new System.Drawing.Point(140, 141);
            this.textBoxConform.Name = "textBoxConform";
            this.textBoxConform.Properties.PasswordChar = '*';
            this.textBoxConform.Size = new System.Drawing.Size(191, 20);
            this.textBoxConform.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(61, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 14);
            this.label4.TabIndex = 18;
            this.label4.Text = "确认新密码";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxNewPassword
            // 
            this.textBoxNewPassword.Location = new System.Drawing.Point(140, 112);
            this.textBoxNewPassword.Name = "textBoxNewPassword";
            this.textBoxNewPassword.Properties.PasswordChar = '*';
            this.textBoxNewPassword.Size = new System.Drawing.Size(191, 20);
            this.textBoxNewPassword.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(61, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 17;
            this.label3.Text = "新 密 码";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(61, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 16;
            this.label2.Text = "原 密 码";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxOldPassword
            // 
            this.textBoxOldPassword.Location = new System.Drawing.Point(140, 83);
            this.textBoxOldPassword.Name = "textBoxOldPassword";
            this.textBoxOldPassword.Properties.PasswordChar = '*';
            this.textBoxOldPassword.Size = new System.Drawing.Size(191, 20);
            this.textBoxOldPassword.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonCancel.Appearance.Options.UseFont = true;
            this.buttonCancel.Location = new System.Drawing.Point(242, 170);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(89, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "取消(&C)";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOK.Appearance.Options.UseFont = true;
            this.buttonOK.Location = new System.Drawing.Point(140, 170);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(89, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "确定(&O)";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gridControl1);
            this.xtraTabPage2.Controls.Add(this.panelControl1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(427, 235);
            this.xtraTabPage2.Text = "附属账号";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemDateEdit2});
            this.gridControl1.Size = new System.Drawing.Size(427, 202);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_userid,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn1});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowColumnResizing = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // col_userid
            // 
            this.col_userid.Caption = "临时工号";
            this.col_userid.FieldName = "UserID";
            this.col_userid.Name = "col_userid";
            this.col_userid.OptionsColumn.AllowEdit = false;
            this.col_userid.OptionsColumn.ReadOnly = true;
            this.col_userid.Visible = true;
            this.col_userid.VisibleIndex = 0;
            this.col_userid.Width = 60;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "姓名";
            this.gridColumn2.FieldName = "Name";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.ReadOnly = true;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 70;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "开始日期";
            this.gridColumn3.ColumnEdit = this.repositoryItemDateEdit1;
            this.gridColumn3.FieldName = "startdate";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.ReadOnly = true;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 128;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemDateEdit1.EditFormat.FormatString = "d";
            this.repositoryItemDateEdit1.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            this.repositoryItemDateEdit1.QueryCloseUp += new System.ComponentModel.CancelEventHandler(this.repositoryItemDateEdit1_QueryCloseUp);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "结束日期";
            this.gridColumn1.ColumnEdit = this.repositoryItemDateEdit2;
            this.gridColumn1.FieldName = "enddate";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 134;
            // 
            // repositoryItemDateEdit2
            // 
            this.repositoryItemDateEdit2.AutoHeight = false;
            this.repositoryItemDateEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit2.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemDateEdit2.EditFormat.FormatString = "yyyy/MM/dd";
            this.repositoryItemDateEdit2.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.repositoryItemDateEdit2.Name = "repositoryItemDateEdit2";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_Delete);
            this.panelControl1.Controls.Add(this.btn_Add);
            this.panelControl1.Controls.Add(this.btn_Exit);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 202);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(427, 33);
            this.panelControl1.TabIndex = 1;
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(195, 6);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(59, 23);
            this.btn_Delete.TabIndex = 3;
            this.btn_Delete.Text = "删除";
            this.btn_Delete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(117, 6);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(59, 23);
            this.btn_Add.TabIndex = 2;
            this.btn_Add.Text = "增加";
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Exit.Location = new System.Drawing.Point(278, 5);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(59, 23);
            this.btn_Exit.TabIndex = 0;
            this.btn_Exit.Text = "退出";
            this.btn_Exit.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FormChangePassword
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(433, 264);
            this.Controls.Add(this.xtraTabControl1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChangePassword";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "账号管理";
            this.Load += new System.EventHandler(this.FormChangePassword_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormChangePassword_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panel2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxConform.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxNewPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxOldPassword.Properties)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
