namespace DrectSoft.Core.OwnBedInfo
{
    partial class FormMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMessage));
            this.gridMessage = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridCol2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCol4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridMessage
            // 
            this.gridMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMessage.Location = new System.Drawing.Point(0, 0);
            this.gridMessage.MainView = this.gridView1;
            this.gridMessage.Name = "gridMessage";
            this.gridMessage.Size = new System.Drawing.Size(810, 495);
            this.gridMessage.TabIndex = 3;
            this.gridMessage.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridCol2,
            this.gridCol3,
            this.gridCol4});
            this.gridView1.CustomizationFormBounds = new System.Drawing.Rectangle(951, 454, 216, 187);
            this.gridView1.GridControl = this.gridMessage;
            this.gridView1.GroupCount = 1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
            this.gridView1.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridCol2, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gridCol2
            // 
            this.gridCol2.Caption = "是否完成";
            this.gridCol2.FieldName = "RESULT";
            this.gridCol2.Name = "gridCol2";
            this.gridCol2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridCol2.Visible = true;
            this.gridCol2.VisibleIndex = 0;
            this.gridCol2.Width = 132;
            // 
            // gridCol3
            // 
            this.gridCol3.Caption = "违规时间";
            this.gridCol3.FieldName = "CONDITIONTIME";
            this.gridCol3.Name = "gridCol3";
            this.gridCol3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridCol3.Visible = true;
            this.gridCol3.VisibleIndex = 0;
            this.gridCol3.Width = 126;
            // 
            // gridCol4
            // 
            this.gridCol4.Caption = "病历违规信息";
            this.gridCol4.FieldName = "FOULMESSAGE";
            this.gridCol4.Name = "gridCol4";
            this.gridCol4.Visible = true;
            this.gridCol4.VisibleIndex = 1;
            this.gridCol4.Width = 564;
            // 
            // FormMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 495);
            this.Controls.Add(this.gridMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMessage";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "病历违规信息";
            this.Load += new System.EventHandler(this.病历违规信息_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridMessage;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol2;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol3;
        private DevExpress.XtraGrid.Columns.GridColumn gridCol4;
    }
}