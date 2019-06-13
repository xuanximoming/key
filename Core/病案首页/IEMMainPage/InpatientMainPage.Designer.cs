namespace YidanSoft.Core.IEMMainPage
{
    partial class InpatientMainPage
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
            this.ucOthers = new YidanSoft.Core.IEMMainPage.UCOthers();
            this.ucIemOperInfo = new YidanSoft.Core.IEMMainPage.UCIemOperInfo();
            this.ucIemDiagnose = new YidanSoft.Core.IEMMainPage.UCIemDiagnose();
            this.ucIemBasInfo = new YidanSoft.Core.IEMMainPage.UCIemBasInfo();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.barManagerIme = new DevExpress.XtraBars.BarManager(this.components);
            this.barButton = new DevExpress.XtraBars.Bar();
            this.barEditItemChoose = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barStaticItemName = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItemSave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerIme)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ucOthers
            // 
            this.ucOthers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ucOthers.Location = new System.Drawing.Point(3, 1638);
            this.ucOthers.Name = "ucOthers";
            this.ucOthers.Size = new System.Drawing.Size(999, 299);
            this.ucOthers.TabIndex = 7;
            // 
            // ucIemOperInfo
            // 
            this.ucIemOperInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ucIemOperInfo.Location = new System.Drawing.Point(3, 1014);
            this.ucIemOperInfo.Name = "ucIemOperInfo";
            this.ucIemOperInfo.Size = new System.Drawing.Size(999, 626);
            this.ucIemOperInfo.TabIndex = 6;
            // 
            // ucIemDiagnose
            // 
            this.ucIemDiagnose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ucIemDiagnose.Location = new System.Drawing.Point(4, 423);
            this.ucIemDiagnose.Name = "ucIemDiagnose";
            this.ucIemDiagnose.Size = new System.Drawing.Size(999, 577);
            this.ucIemDiagnose.TabIndex = 5;
            // 
            // ucIemBasInfo
            // 
            this.ucIemBasInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ucIemBasInfo.Location = new System.Drawing.Point(4, 54);
            this.ucIemBasInfo.Name = "ucIemBasInfo";
            this.ucIemBasInfo.Size = new System.Drawing.Size(998, 420);
            this.ucIemBasInfo.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(927, 1943);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "±£´æ";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // barManagerIme
            // 
            this.barManagerIme.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barButton});
            this.barManagerIme.DockControls.Add(this.barDockControlTop);
            this.barManagerIme.DockControls.Add(this.barDockControlBottom);
            this.barManagerIme.DockControls.Add(this.barDockControlLeft);
            this.barManagerIme.DockControls.Add(this.barDockControlRight);
            this.barManagerIme.Form = this;
            this.barManagerIme.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barEditItemChoose,
            this.barStaticItemName,
            this.barButtonItemSave,
            this.barButtonItem2});
            this.barManagerIme.MaxItemId = 6;
            this.barManagerIme.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit1,
            this.repositoryItemTextEdit1});
            // 
            // barButton
            // 
            this.barButton.BarName = "Tools";
            this.barButton.DockCol = 0;
            this.barButton.DockRow = 0;
            this.barButton.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barButton.FloatLocation = new System.Drawing.Point(256, 196);
            this.barButton.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(((DevExpress.XtraBars.BarLinkUserDefines)((DevExpress.XtraBars.BarLinkUserDefines.PaintStyle | DevExpress.XtraBars.BarLinkUserDefines.Width))), this.barEditItemChoose, "", true, true, true, 123, null, DevExpress.XtraBars.BarItemPaintStyle.Caption),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemName, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2, true)});
            this.barButton.Offset = 201;
            this.barButton.OptionsBar.AllowQuickCustomization = false;
            this.barButton.OptionsBar.DisableClose = true;
            this.barButton.Text = "²¡»¼Ñ¡Ôñ";
            // 
            // barEditItemChoose
            // 
            this.barEditItemChoose.Caption = "Ñ¡Ôñ²¡»¼£º";
            this.barEditItemChoose.Edit = this.repositoryItemTextEdit1;
            this.barEditItemChoose.Id = 0;
            this.barEditItemChoose.Name = "barEditItemChoose";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barStaticItemName
            // 
            this.barStaticItemName.Caption = "                 ";
            this.barStaticItemName.Id = 2;
            this.barStaticItemName.Name = "barStaticItemName";
            this.barStaticItemName.OwnFont = new System.Drawing.Font("Î¢ÈíÑÅºÚ", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.World);
            this.barStaticItemName.TextAlignment = System.Drawing.StringAlignment.Near;
            this.barStaticItemName.UseOwnFont = true;
            // 
            // barButtonItemSave
            // 
            this.barButtonItemSave.Caption = "±£´æ";
            this.barButtonItemSave.Id = 4;
            this.barButtonItemSave.Name = "barButtonItemSave";
            this.barButtonItemSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSave_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "´òÓ¡";
            this.barButtonItem2.Id = 5;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // InpatientMainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1173, 874);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ucOthers);
            this.Controls.Add(this.ucIemOperInfo);
            this.Controls.Add(this.ucIemDiagnose);
            this.Controls.Add(this.ucIemBasInfo);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "InpatientMainPage";
            this.Text = "²¡°¸Ê×Ò³";
            this.Load += new System.EventHandler(this.InpatientMainPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManagerIme)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UCOthers ucOthers;
        private UCIemOperInfo ucIemOperInfo;
        private UCIemDiagnose ucIemDiagnose;
        private UCIemBasInfo ucIemBasInfo;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraBars.BarManager barManagerIme;
        private DevExpress.XtraBars.Bar barButton;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarEditItem barEditItemChoose;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarStaticItem barStaticItemName;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSave;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;





    }
}