namespace DrectSoft.Core.MainEmrPad.HistoryEMR
{
    partial class HistoryEmrTimeAndCaption
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryEmrTimeAndCaption));
            this.gridControlDailyEmr = new DevExpress.XtraGrid.GridControl();
            this.gridViewDailyEmr = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnEmrID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnEmrName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnEmrDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.gridColumnEmrTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTimeEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
            this.gridColumnEmrTitle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoExEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.repositoryItemRichTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.DevButtonCancel1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.DevButtonOK1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDailyEmr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDailyEmr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlDailyEmr
            // 
            this.gridControlDailyEmr.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridControlDailyEmr.Location = new System.Drawing.Point(0, 0);
            this.gridControlDailyEmr.MainView = this.gridViewDailyEmr;
            this.gridControlDailyEmr.Name = "gridControlDailyEmr";
            this.gridControlDailyEmr.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemTimeEdit1,
            this.repositoryItemMemoExEdit1,
            this.repositoryItemRichTextEdit1});
            this.gridControlDailyEmr.Size = new System.Drawing.Size(580, 268);
            this.gridControlDailyEmr.TabIndex = 0;
            this.gridControlDailyEmr.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDailyEmr});
            // 
            // gridViewDailyEmr
            // 
            this.gridViewDailyEmr.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnEmrID,
            this.gridColumnEmrName,
            this.gridColumnEmrDate,
            this.gridColumnEmrTime,
            this.gridColumnEmrTitle});
            this.gridViewDailyEmr.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewDailyEmr.GridControl = this.gridControlDailyEmr;
            this.gridViewDailyEmr.Name = "gridViewDailyEmr";
            this.gridViewDailyEmr.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewDailyEmr.OptionsCustomization.AllowFilter = false;
            this.gridViewDailyEmr.OptionsCustomization.AllowSort = false;
            this.gridViewDailyEmr.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewDailyEmr.OptionsFilter.AllowFilterEditor = false;
            this.gridViewDailyEmr.OptionsFind.AllowFindPanel = false;
            this.gridViewDailyEmr.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewDailyEmr.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridViewDailyEmr.OptionsView.ShowGroupPanel = false;
            this.gridViewDailyEmr.OptionsView.ShowIndicator = false;
            this.gridViewDailyEmr.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewDailyEmr_CellValueChanged);
            // 
            // gridColumnEmrID
            // 
            this.gridColumnEmrID.Caption = "病历编号";
            this.gridColumnEmrID.Name = "gridColumnEmrID";
            // 
            // gridColumnEmrName
            // 
            this.gridColumnEmrName.Caption = "病程名称";
            this.gridColumnEmrName.FieldName = "EMRNAME";
            this.gridColumnEmrName.Name = "gridColumnEmrName";
            this.gridColumnEmrName.OptionsColumn.AllowEdit = false;
            this.gridColumnEmrName.OptionsFilter.AllowAutoFilter = false;
            this.gridColumnEmrName.OptionsFilter.AllowFilter = false;
            this.gridColumnEmrName.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.gridColumnEmrName.Visible = true;
            this.gridColumnEmrName.VisibleIndex = 0;
            this.gridColumnEmrName.Width = 210;
            // 
            // gridColumnEmrDate
            // 
            this.gridColumnEmrDate.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.gridColumnEmrDate.AppearanceCell.Options.UseBackColor = true;
            this.gridColumnEmrDate.Caption = "病程日期";
            this.gridColumnEmrDate.ColumnEdit = this.repositoryItemDateEdit1;
            this.gridColumnEmrDate.FieldName = "EMRDATE";
            this.gridColumnEmrDate.Name = "gridColumnEmrDate";
            this.gridColumnEmrDate.OptionsFilter.AllowAutoFilter = false;
            this.gridColumnEmrDate.OptionsFilter.AllowFilter = false;
            this.gridColumnEmrDate.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.gridColumnEmrDate.Visible = true;
            this.gridColumnEmrDate.VisibleIndex = 1;
            this.gridColumnEmrDate.Width = 90;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // gridColumnEmrTime
            // 
            this.gridColumnEmrTime.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.gridColumnEmrTime.AppearanceCell.Options.UseBackColor = true;
            this.gridColumnEmrTime.Caption = "病程时间";
            this.gridColumnEmrTime.ColumnEdit = this.repositoryItemTimeEdit1;
            this.gridColumnEmrTime.FieldName = "EMRTIME";
            this.gridColumnEmrTime.Name = "gridColumnEmrTime";
            this.gridColumnEmrTime.OptionsFilter.AllowAutoFilter = false;
            this.gridColumnEmrTime.OptionsFilter.AllowFilter = false;
            this.gridColumnEmrTime.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.gridColumnEmrTime.Visible = true;
            this.gridColumnEmrTime.VisibleIndex = 2;
            this.gridColumnEmrTime.Width = 90;
            // 
            // repositoryItemTimeEdit1
            // 
            this.repositoryItemTimeEdit1.AutoHeight = false;
            this.repositoryItemTimeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
            // 
            // gridColumnEmrTitle
            // 
            this.gridColumnEmrTitle.AppearanceCell.BackColor = System.Drawing.Color.Silver;
            this.gridColumnEmrTitle.AppearanceCell.Options.UseBackColor = true;
            this.gridColumnEmrTitle.Caption = "病程标题";
            this.gridColumnEmrTitle.FieldName = "EMRTITLE";
            this.gridColumnEmrTitle.Name = "gridColumnEmrTitle";
            this.gridColumnEmrTitle.OptionsFilter.AllowAutoFilter = false;
            this.gridColumnEmrTitle.OptionsFilter.AllowFilter = false;
            this.gridColumnEmrTitle.OptionsFilter.ImmediateUpdateAutoFilter = false;
            this.gridColumnEmrTitle.Visible = true;
            this.gridColumnEmrTitle.VisibleIndex = 3;
            this.gridColumnEmrTitle.Width = 188;
            // 
            // repositoryItemMemoExEdit1
            // 
            this.repositoryItemMemoExEdit1.AutoHeight = false;
            this.repositoryItemMemoExEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemMemoExEdit1.Name = "repositoryItemMemoExEdit1";
            // 
            // repositoryItemRichTextEdit1
            // 
            this.repositoryItemRichTextEdit1.Name = "repositoryItemRichTextEdit1";
            this.repositoryItemRichTextEdit1.ShowCaretInReadOnly = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Location = new System.Drawing.Point(13, 278);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(220, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "病程日期、病程时间、病程标题 可以修改";
            // 
            // DevButtonCancel1
            // 
            this.DevButtonCancel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DevButtonCancel1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonCancel1.Image")));
            this.DevButtonCancel1.Location = new System.Drawing.Point(487, 274);
            this.DevButtonCancel1.Name = "DevButtonCancel1";
            this.DevButtonCancel1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonCancel1.TabIndex = 10;
            this.DevButtonCancel1.Text = "取消(&C)";
            this.DevButtonCancel1.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // DevButtonOK1
            // 
            this.DevButtonOK1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DevButtonOK1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonOK1.Image")));
            this.DevButtonOK1.Location = new System.Drawing.Point(392, 274);
            this.DevButtonOK1.Name = "DevButtonOK1";
            this.DevButtonOK1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonOK1.TabIndex = 9;
            this.DevButtonOK1.Text = "确定(&Y)";
            this.DevButtonOK1.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // HistoryEmrTimeAndCaption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 302);
            this.Controls.Add(this.DevButtonCancel1);
            this.Controls.Add(this.DevButtonOK1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.gridControlDailyEmr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HistoryEmrTimeAndCaption";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置病程时间、病程标题";
            this.Load += new System.EventHandler(this.HistoryEmrTimeAndCaption_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDailyEmr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDailyEmr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRichTextEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlDailyEmr;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDailyEmr;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEmrName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEmrDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEmrTitle;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEmrTime;
        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit repositoryItemMemoExEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit repositoryItemRichTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEmrID;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel DevButtonCancel1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK DevButtonOK1;

    }
}