namespace DrectSoft.MainFrame
{
    partial class FormLock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelControlShow = new DevExpress.XtraEditors.LabelControl();
            this.textEditPassWord = new DevExpress.XtraEditors.TextEdit();
            this.labelControlPassWord = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditorLock = new DrectSoft.Common.Library.LookUpEditor();
            this.lookUpWindowLock = new DrectSoft.Common.Library.LookUpWindow(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlLock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.gridControlLock.Location = new System.Drawing.Point(24, 321);
            this.gridControlLock.MainView = this.cardView1;
            this.gridControlLock.Name = "gridControlLock";
            this.gridControlLock.Size = new System.Drawing.Size(36, 20);
            this.gridControlLock.TabIndex = 1;
            this.gridControlLock.TabStop = false;
            this.gridControlLock.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.cardView1});
            this.gridControlLock.Visible = false;
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
            this.labelControl1.Location = new System.Drawing.Point(26, 247);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "用户名";
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.Location = new System.Drawing.Point(293, 282);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(13, 31);
            this.simpleButtonCancel.TabIndex = 4;
            this.simpleButtonCancel.Text = "取消";
            this.simpleButtonCancel.Visible = false;
            this.simpleButtonCancel.Click += new System.EventHandler(this.simpleButtonCancel_Click);
            // 
            // simpleButtonConfirm
            // 
            this.simpleButtonConfirm.Location = new System.Drawing.Point(75, 310);
            this.simpleButtonConfirm.Name = "simpleButtonConfirm";
            this.simpleButtonConfirm.Size = new System.Drawing.Size(168, 31);
            this.simpleButtonConfirm.TabIndex = 3;
            this.simpleButtonConfirm.Text = "确定";
            this.simpleButtonConfirm.Click += new System.EventHandler(this.simpleButtonConfirm_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupControl1.Controls.Add(this.pictureBox1);
            this.groupControl1.Controls.Add(this.labelControlShow);
            this.groupControl1.Controls.Add(this.textEditPassWord);
            this.groupControl1.Controls.Add(this.labelControlPassWord);
            this.groupControl1.Controls.Add(this.lookUpEditorLock);
            this.groupControl1.Controls.Add(this.gridControlLock);
            this.groupControl1.Controls.Add(this.simpleButtonCancel);
            this.groupControl1.Controls.Add(this.simpleButtonConfirm);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.LookAndFeel.SkinName = "Blue";
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.ShowCaption = false;
            this.groupControl1.Size = new System.Drawing.Size(373, 360);
            this.groupControl1.TabIndex = 12;
            this.groupControl1.Text = "groupControl1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(75, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(211, 235);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // labelControlShow
            // 
            this.labelControlShow.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControlShow.Location = new System.Drawing.Point(24, 282);
            this.labelControlShow.Name = "labelControlShow";
            this.labelControlShow.Size = new System.Drawing.Size(224, 14);
            this.labelControlShow.TabIndex = 13;
            this.labelControlShow.Text = "提   醒    与锁屏前用户不同,点击确定登录";
            this.labelControlShow.Visible = false;
            // 
            // textEditPassWord
            // 
            this.textEditPassWord.Location = new System.Drawing.Point(75, 278);
            this.textEditPassWord.Name = "textEditPassWord";
            this.textEditPassWord.Properties.PasswordChar = '*';
            this.textEditPassWord.Size = new System.Drawing.Size(211, 20);
            this.textEditPassWord.TabIndex = 2;
            this.textEditPassWord.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textEditPassWord_KeyPress);
            // 
            // labelControlPassWord
            // 
            this.labelControlPassWord.Location = new System.Drawing.Point(26, 282);
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
            this.lookUpEditorLock.Location = new System.Drawing.Point(75, 246);
            this.lookUpEditorLock.Name = "lookUpEditorLock";
            this.lookUpEditorLock.ReadOnly = true;
            this.lookUpEditorLock.ShowFormImmediately = true;
            this.lookUpEditorLock.ShowToolTips = false;
            this.lookUpEditorLock.Size = new System.Drawing.Size(211, 18);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 384);
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassWord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorLock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowLock)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlLock;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.SimpleButton simpleButtonConfirm;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindowLock;
        private DrectSoft.Common.Library.LookUpEditor lookUpEditorLock;
        private DevExpress.XtraEditors.TextEdit textEditPassWord;
        private DevExpress.XtraEditors.LabelControl labelControlPassWord;
        private DevExpress.XtraEditors.LabelControl labelControlShow;
        private DevExpress.XtraGrid.Columns.GridColumn Images;
        private DevExpress.XtraGrid.Columns.GridColumn ID;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}