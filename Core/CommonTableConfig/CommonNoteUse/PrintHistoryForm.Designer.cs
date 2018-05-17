namespace YiDanSoft.Core.CommonTableConfig.CommonNoteUse
{
    partial class PrintHistoryForm
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.C1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.C5 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(628, 404);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.C1,
            this.C2,
            this.C3,
            this.C4,
            this.C5});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 30;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFind.AllowFindPanel = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridView1.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridView1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridView1.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            // 
            // C1
            // 
            this.C1.Caption = "打印时间";
            this.C1.Name = "C1";
            this.C1.Visible = true;
            this.C1.VisibleIndex = 0;
            // 
            // C2
            // 
            this.C2.Caption = "起始页";
            this.C2.Name = "C2";
            this.C2.Visible = true;
            this.C2.VisibleIndex = 1;
            // 
            // C3
            // 
            this.C3.Caption = "结束页";
            this.C3.Name = "C3";
            this.C3.Visible = true;
            this.C3.VisibleIndex = 2;
            // 
            // C4
            // 
            this.C4.Caption = "打印份数";
            this.C4.Name = "C4";
            this.C4.Visible = true;
            this.C4.VisibleIndex = 3;
            // 
            // C5
            // 
            this.C5.Caption = "打印者";
            this.C5.Name = "C5";
            this.C5.Visible = true;
            this.C5.VisibleIndex = 4;
            // 
            // PrintHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 404);
            this.Controls.Add(this.gridControl1);
            this.Name = "PrintHistoryForm";
            this.Text = "打印历史记录";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn C1;
        private DevExpress.XtraGrid.Columns.GridColumn C2;
        private DevExpress.XtraGrid.Columns.GridColumn C3;
        private DevExpress.XtraGrid.Columns.GridColumn C4;
        private DevExpress.XtraGrid.Columns.GridColumn C5;

    }
}