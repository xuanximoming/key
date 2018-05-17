using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Card;
using DrectSoft.Common.Library;
using DrectSoft.Core;
using DrectSoft.Resources;
using DrectSoft.Wordbook;
using MainFrame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace DrectSoft.MainFrame
{
    public class FormLock : Form
    {
        private IContainer components = null;

        private GridControl gridControlLock;

        private CardView cardView1;

        private LabelControl labelControl1;

        private SimpleButton simpleButtonCancel;

        private SimpleButton simpleButtonConfirm;

        private GroupControl groupControl1;

        private LookUpWindow lookUpWindowLock;

        private LookUpEditor lookUpEditorLock;

        private TextEdit textEditPassWord;

        private LabelControl labelControlPassWord;

        private LabelControl labelControlShow;

        private GridColumn Images;

        private GridColumn ID;

        private RepositoryItemPictureEdit repositoryItemPictureEdit1;

        private IDataAccess m_SqlHelper;

        private DrectSoftLog m_Logger;

        private IUser m_CurrentUser;

        private Account m_Account;

        private DataTable m_DataTable;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 512;
                return createParams;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLock));
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.gridControlLock = new DevExpress.XtraGrid.GridControl();
            this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
            this.Images = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControlShow = new DevExpress.XtraEditors.LabelControl();
            this.textEditPassWord = new DevExpress.XtraEditors.TextEdit();
            this.labelControlPassWord = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditorLock = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowLock = new DrectSoft.Common.Library.LookUpWindow();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlLock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassWord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorLock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowLock)).BeginInit();
            this.SuspendLayout();
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.CustomHeight = 80;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            // 
            // gridControlLock
            // 
            this.gridControlLock.Location = new System.Drawing.Point(22, 5);
            this.gridControlLock.MainView = this.cardView1;
            this.gridControlLock.Name = "gridControlLock";
            this.gridControlLock.Size = new System.Drawing.Size(223, 136);
            this.gridControlLock.TabIndex = 1;
            this.gridControlLock.TabStop = false;
            this.gridControlLock.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.cardView1});
            // 
            // cardView1
            // 
            this.cardView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Images,
            this.ID});
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.gridControlLock;
            this.cardView1.MaximumCardRows = 1;
            this.cardView1.Name = "cardView1";
            this.cardView1.OptionsBehavior.FieldAutoHeight = true;
            this.cardView1.OptionsPrint.AutoHorzWidth = true;
            this.cardView1.OptionsView.ShowCardExpandButton = false;
            this.cardView1.OptionsView.ShowQuickCustomizeButton = false;
            // 
            // Images
            // 
            this.Images.ColumnEdit = this.repositoryItemPictureEdit1;
            this.Images.FieldName = "Images";
            this.Images.ImageAlignment = System.Drawing.StringAlignment.Center;
            this.Images.Name = "Images";
            this.Images.OptionsColumn.FixedWidth = true;
            this.Images.OptionsColumn.ReadOnly = true;
            this.Images.OptionsColumn.ShowCaption = false;
            this.Images.OptionsColumn.ShowInCustomizationForm = false;
            this.Images.Visible = true;
            this.Images.VisibleIndex = 0;
            // 
            // ID
            // 
            this.ID.Caption = "ID";
            this.ID.FieldName = "ID";
            this.ID.Name = "ID";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(22, 150);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "用户名";
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Location = new System.Drawing.Point(251, 180);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(11, 27);
            this.simpleButtonCancel.TabIndex = 4;
            this.simpleButtonCancel.Text = "取消";
            this.simpleButtonCancel.Visible = false;
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // simpleButtonConfirm
            // 
            this.simpleButtonConfirm.Location = new System.Drawing.Point(64, 204);
            this.simpleButtonConfirm.Name = "simpleButtonConfirm";
            this.simpleButtonConfirm.Size = new System.Drawing.Size(144, 27);
            this.simpleButtonConfirm.TabIndex = 3;
            this.simpleButtonConfirm.Text = "确定";
            this.simpleButtonConfirm.Click += new System.EventHandler(this.simpleButtonConfirm_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupControl1.Controls.Add(this.labelControlShow);
            this.groupControl1.Controls.Add(this.textEditPassWord);
            this.groupControl1.Controls.Add(this.labelControlPassWord);
            this.groupControl1.Controls.Add(this.lookUpEditorLock);
            this.groupControl1.Controls.Add(this.gridControlLock);
            this.groupControl1.Controls.Add(this.simpleButtonCancel);
            this.groupControl1.Controls.Add(this.simpleButtonConfirm);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.LookAndFeel.SkinName = "Blue";
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(277, 250);
            this.groupControl1.TabIndex = 12;
            this.groupControl1.Text = "groupControl1";
            // 
            // labelControlShow
            // 
            this.labelControlShow.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControlShow.Location = new System.Drawing.Point(21, 180);
            this.labelControlShow.Name = "labelControlShow";
            this.labelControlShow.Size = new System.Drawing.Size(224, 14);
            this.labelControlShow.TabIndex = 13;
            this.labelControlShow.Text = "提   醒    与锁屏前用户不同,点击确定登录";
            this.labelControlShow.Visible = false;
            // 
            // textEditPassWord
            // 
            this.textEditPassWord.Location = new System.Drawing.Point(64, 177);
            this.textEditPassWord.Name = "textEditPassWord";
            this.textEditPassWord.Properties.PasswordChar = '*';
            this.textEditPassWord.Size = new System.Drawing.Size(181, 20);
            this.textEditPassWord.TabIndex = 2;
            this.textEditPassWord.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textEditPassWord_KeyPress);
            // 
            // labelControlPassWord
            // 
            this.labelControlPassWord.Location = new System.Drawing.Point(22, 180);
            this.labelControlPassWord.Name = "labelControlPassWord";
            this.labelControlPassWord.Size = new System.Drawing.Size(32, 14);
            this.labelControlPassWord.TabIndex = 11;
            this.labelControlPassWord.Text = "密  码";
            // 
            // lookUpEditorLock
            // 
            this.lookUpEditorLock.EnterMoveNextControl = true;
            this.lookUpEditorLock.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorLock.ListWindow = null;
            this.lookUpEditorLock.Location = new System.Drawing.Point(64, 149);
            this.lookUpEditorLock.Name = "lookUpEditorLock";
            this.lookUpEditorLock.ReadOnly = true;
            this.lookUpEditorLock.ShowFormImmediately = true;
            this.lookUpEditorLock.ShowToolTips = false;
            this.lookUpEditorLock.Size = new System.Drawing.Size(181, 18);
            this.lookUpEditorLock.TabIndex = 1;
            this.lookUpEditorLock.CodeValueChanged += new System.EventHandler(this.lookUpEditorLock_CodeValueChanged);
            // 
            // lookUpWindowLock
            // 
            this.lookUpWindowLock.AlwaysShowWindow = true;
            this.lookUpWindowLock.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowLock.GenShortCode = null;
            this.lookUpWindowLock.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowLock.Owner = null;
            this.lookUpWindowLock.SqlHelper = null;
            // 
            // FormLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 254);
            this.Controls.Add(this.groupControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLock";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "锁屏";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLock_FormClosing);
            this.Load += new System.EventHandler(this.FormLock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlLock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassWord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorLock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowLock)).EndInit();
            this.ResumeLayout(false);

        }

        public FormLock()
        {
            this.InitializeComponent();
        }

        private void FormLock_Load(object sender, EventArgs e)
        {
            try
            {
                this.Init();
            }
            catch (Exception message)
            {
                this.m_Logger.Error(message);
            }
        }

        private void FormLock_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (base.DialogResult != DialogResult.OK)
            {
                if (base.DialogResult == DialogResult.Retry)
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                    base.DialogResult = DialogResult.Cancel;
                }
            }
        }

        private void simpleButtonConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lookUpEditorLock.CodeValue != string.Empty)
                {
                    this.Confirm();
                }
                else
                {
                    FormMain.Instance.MessageShow("请选择用户!", CustomMessageBoxKind.InformationOk);
                }
            }
            catch (Exception message)
            {
                this.m_Logger.Error(message);
            }
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void lookUpEditorLock_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                string codeValue = this.lookUpEditorLock.CodeValue;
                this.textEditPassWord.Text = string.Empty;
                this.GetGridSource(codeValue);
                if (this.m_CurrentUser.Id == codeValue)
                {
                    this.labelControlShow.Visible = false;
                    this.labelControlPassWord.Visible = true;
                    this.textEditPassWord.Visible = true;
                }
                else
                {
                    this.labelControlShow.Visible = true;
                    this.labelControlPassWord.Visible = false;
                    this.textEditPassWord.Visible = false;
                }
                this.cardView1.SelectRow(0);
            }
            catch (Exception message)
            {
                this.m_Logger.Error(message);
            }
        }

        private void textEditPassWord_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                this.Confirm();
            }
        }

        private void Init()
        {
            try
            {
                base.FormBorderStyle = FormBorderStyle.None;
                this.m_SqlHelper = DataAccessFactory.DefaultDataAccess;
                this.m_Logger = new DrectSoftLog("系统锁屏模块跟踪");
                this.m_CurrentUser = FormMain.Instance.User;
                this.m_Account = new Account();
                this.InitControls();
            }
            catch (Exception message)
            {
                this.m_Logger.Error(message);
            }
        }

        private void InitControls()
        {
            try
            {
                this.lookUpWindowLock.SqlHelper = this.m_SqlHelper;
                this.m_DataTable = this.m_SqlHelper.ExecuteDataTable(" SELECT ID , ID + '_' + Name as 用户名,Images  FROM Users  WHERE Valid = 1 order by ID");
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                dictionary.Add("用户名", 165);
                SqlWordbook sqlWordbook = new SqlWordbook("ID", this.m_DataTable, "ID", "用户名", dictionary, true);
                this.lookUpEditorLock.SqlWordbook = sqlWordbook;
                this.lookUpEditorLock.ListWindow = this.lookUpWindowLock;
                this.lookUpEditorLock.CodeValue = this.m_CurrentUser.Id;
                this.textEditPassWord.Focus();
            }
            catch (SqlException message)
            {
                this.m_Logger.Error(message);
            }
            catch (Exception message2)
            {
                this.m_Logger.Error(message2);
            }
        }

        private bool LoginDb()
        {
            bool result;
            try
            {
                IUser user = this.m_Account.Login(this.lookUpEditorLock.CodeValue, this.textEditPassWord.Text, 1);
                this.m_Logger.Info(string.Concat(new string[]
				{
					"用户:",
					user.Name,
					"于",
					DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
					"成功登录系统"
				}));
            }
            catch (Exception ex)
            {
                this.textEditPassWord.SelectAll();
                this.textEditPassWord.Focus();
                result = false;
                return result;
            }
            result = true;
            return result;
        }

        private void Confirm()
        {
            try
            {
                if (this.lookUpEditorLock.CodeValue != string.Empty)
                {
                    if (this.lookUpEditorLock.CodeValue == this.m_CurrentUser.Id)
                    {
                        if (this.LoginDb())
                        {
                            base.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("密码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        string codeValue = this.lookUpEditorLock.CodeValue;
                        Program.m_StrUserId = codeValue;
                        base.DialogResult = DialogResult.Retry;
                    }
                }
            }
            catch (Exception message)
            {
                this.m_Logger.Error(message);
            }
        }

        private void GetDbImages(DataTable dataTable)
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (!dataRow.IsNull("Images"))
                {
                    MemoryStream memoryStream = new MemoryStream((byte[])dataRow["Images"]);
                    this.cardView1.SetRowCellValue(0, this.cardView1.Columns["Images"], memoryStream.ToArray());
                }
            }
        }

        private void GetGridSource(string codeValue)
        {
            try
            {
                DataTable dataTable = this.m_DataTable.Copy();
                dataTable.Clear();
                DataRow[] array = this.m_DataTable.Select("ID='" + codeValue + "'");
                if (array.Length > 0)
                {
                    dataTable.ImportRow(array[0]);
                    dataTable.AcceptChanges();
                }
                MemoryStream memoryStream = new MemoryStream();
                Image image = ResourceManager.GetImage("NoImage.bmp");
                image.Save(memoryStream, ImageFormat.Bmp);
                if (dataTable.Rows.Count > 0)
                {
                    if (dataTable.Rows[0].IsNull("Images"))
                    {
                        dataTable.Rows[0]["Images"] = memoryStream.ToArray();
                    }
                }
                dataTable.AcceptChanges();
                this.groupControl1.SuspendLayout();
                this.gridControlLock.DataSource = dataTable;
                this.groupControl1.ResumeLayout();
            }
            catch (Exception message)
            {
                this.m_Logger.Error(message);
            }
        }
    }
}
