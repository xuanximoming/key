namespace DrectSoft.Core.MainEmrPad.New
{
    partial class ChooseDeptForDailyEmrPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseDeptForDailyEmrPrint));
            this.gridControlDept = new DevExpress.XtraGrid.GridControl();
            this.gridViewDept = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn0 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnOK = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.btnExit = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDept)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlDept
            // 
            this.gridControlDept.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridControlDept.Location = new System.Drawing.Point(0, 0);
            this.gridControlDept.MainView = this.gridViewDept;
            this.gridControlDept.Name = "gridControlDept";
            this.gridControlDept.Size = new System.Drawing.Size(452, 178);
            this.gridControlDept.TabIndex = 2;
            this.gridControlDept.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDept});
            this.gridControlDept.DoubleClick += new System.EventHandler(this.gridControlDept_DoubleClick);
            this.gridControlDept.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControlDept_KeyDown);
            // 
            // gridViewDept
            // 
            this.gridViewDept.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.gridViewDept.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridViewDept.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn0,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn3});
            this.gridViewDept.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewDept.GridControl = this.gridControlDept;
            this.gridViewDept.Name = "gridViewDept";
            this.gridViewDept.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewDept.OptionsBehavior.Editable = false;
            this.gridViewDept.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewDept.OptionsCustomization.AllowColumnResizing = false;
            this.gridViewDept.OptionsCustomization.AllowFilter = false;
            this.gridViewDept.OptionsCustomization.AllowGroup = false;
            this.gridViewDept.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridViewDept.OptionsCustomization.AllowSort = false;
            this.gridViewDept.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewDept.OptionsFilter.AllowFilterEditor = false;
            this.gridViewDept.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewDept.OptionsFind.AllowFindPanel = false;
            this.gridViewDept.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewDept.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridViewDept.OptionsView.ShowGroupPanel = false;
            this.gridViewDept.OptionsView.ShowIndicator = false;
            this.gridViewDept.OptionsView.ShowViewCaption = true;
            this.gridViewDept.ViewCaption = "科室列表";
            // 
            // gridColumn0
            // 
            this.gridColumn0.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn0.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn0.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn0.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn0.Caption = "转科ID";
            this.gridColumn0.FieldName = "ID";
            this.gridColumn0.Name = "gridColumn0";
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.Caption = "科室代码";
            this.gridColumn1.FieldName = "DEPTID";
            this.gridColumn1.MaxWidth = 60;
            this.gridColumn1.MinWidth = 50;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 60;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn2.Caption = "科室名称";
            this.gridColumn2.FieldName = "DEPTNAME";
            this.gridColumn2.MaxWidth = 80;
            this.gridColumn2.MinWidth = 50;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 80;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "病区代码";
            this.gridColumn4.FieldName = "WARDID";
            this.gridColumn4.MaxWidth = 60;
            this.gridColumn4.MinWidth = 50;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 60;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "病区名称";
            this.gridColumn5.FieldName = "WARDNAME";
            this.gridColumn5.MaxWidth = 80;
            this.gridColumn5.MinWidth = 50;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 80;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn3.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn3.Caption = "入科时间";
            this.gridColumn3.FieldName = "INDATETIME";
            this.gridColumn3.MinWidth = 100;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 4;
            this.gridColumn3.Width = 120;
            // 
            // btnOK
            // 
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.Location = new System.Drawing.Point(276, 184);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 27);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定(&Y)";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.Location = new System.Drawing.Point(362, 184);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 27);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "取消(&C)";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ChooseDeptForDailyEmrPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 217);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gridControlDept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseDeptForDailyEmrPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择科室";
            this.Load += new System.EventHandler(this.ChooseDeptForDailyEmrPrint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDept)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlDept;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDept;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK btnOK;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel btnExit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn0;

    }
}