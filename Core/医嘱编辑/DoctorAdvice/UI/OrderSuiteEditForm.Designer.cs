namespace DrectSoft.Core.DoctorAdvice
{
   partial class OrderSuiteEditForm
   {
      /// <summary>
      /// 必需的设计器变量。
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// 清理所有正在使用的资源。
      /// </summary>
      /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows 窗体设计器生成的代码

      /// <summary>
      /// 设计器支持所需的方法 - 不要
      /// 使用代码编辑器修改此方法的内容。
      /// </summary>
      private void InitializeComponent()
      {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderSuiteEditForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.showListWindow1 = new DrectSoft.Common.Library.LookUpWindow();
            this.gridCtrlSuiteDetail = new DevExpress.XtraGrid.GridControl();
            this.gridViewSuiteDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColCatalog = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColGroupFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColItemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColAmount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.gridColUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColUsage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColFrequency = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColDays = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.gridColMemo = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.showListWindow1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlSuiteDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSuiteDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnOk);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 345);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(837, 40);
            this.panelControl1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::DrectSoft.Core.DoctorAdvice.Properties.Resources.取消;
            this.btnCancel.Location = new System.Drawing.Point(745, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 27);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消 (&C)";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Appearance.Options.UseFont = true;
            this.btnOk.Image = global::DrectSoft.Core.DoctorAdvice.Properties.Resources.确定;
            this.btnOk.Location = new System.Drawing.Point(659, 6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 27);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "确定 (&Y)";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // showListWindow1
            // 
            this.showListWindow1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.showListWindow1.GenShortCode = null;
            this.showListWindow1.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.showListWindow1.Owner = null;
            this.showListWindow1.SqlHelper = null;
            // 
            // gridCtrlSuiteDetail
            // 
            this.gridCtrlSuiteDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCtrlSuiteDetail.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridCtrlSuiteDetail.Location = new System.Drawing.Point(0, 0);
            this.gridCtrlSuiteDetail.MainView = this.gridViewSuiteDetail;
            this.gridCtrlSuiteDetail.Name = "gridCtrlSuiteDetail";
            this.gridCtrlSuiteDetail.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSpinEdit1,
            this.repositoryItemSpinEdit2});
            this.gridCtrlSuiteDetail.Size = new System.Drawing.Size(837, 345);
            this.gridCtrlSuiteDetail.TabIndex = 1;
            this.gridCtrlSuiteDetail.TabStop = false;
            this.gridCtrlSuiteDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewSuiteDetail});
            // 
            // gridViewSuiteDetail
            // 
            this.gridViewSuiteDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColCatalog,
            this.gridColGroupFlag,
            this.gridColItemName,
            this.gridColAmount,
            this.gridColUnit,
            this.gridColUsage,
            this.gridColFrequency,
            this.gridColDays,
            this.gridColMemo});
            this.gridViewSuiteDetail.GridControl = this.gridCtrlSuiteDetail;
            this.gridViewSuiteDetail.Name = "gridViewSuiteDetail";
            this.gridViewSuiteDetail.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewSuiteDetail.OptionsBehavior.Editable = false;
            this.gridViewSuiteDetail.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewSuiteDetail.OptionsCustomization.AllowFilter = false;
            this.gridViewSuiteDetail.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewSuiteDetail.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewSuiteDetail.OptionsMenu.EnableColumnMenu = false;
            this.gridViewSuiteDetail.OptionsMenu.EnableFooterMenu = false;
            this.gridViewSuiteDetail.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewSuiteDetail.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewSuiteDetail.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewSuiteDetail.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewSuiteDetail.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridViewSuiteDetail.OptionsView.ColumnAutoWidth = false;
            this.gridViewSuiteDetail.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewSuiteDetail.OptionsView.ShowGroupPanel = false;
            this.gridViewSuiteDetail.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewSuiteDetail_CellValueChanged);
            // 
            // gridColCatalog
            // 
            this.gridColCatalog.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColCatalog.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColCatalog.Caption = "医嘱类别";
            this.gridColCatalog.FieldName = "yzlbmc";
            this.gridColCatalog.Name = "gridColCatalog";
            this.gridColCatalog.OptionsColumn.AllowEdit = false;
            this.gridColCatalog.OptionsColumn.AllowFocus = false;
            this.gridColCatalog.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColCatalog.Visible = true;
            this.gridColCatalog.VisibleIndex = 0;
            this.gridColCatalog.Width = 80;
            // 
            // gridColGroupFlag
            // 
            this.gridColGroupFlag.AppearanceCell.ForeColor = System.Drawing.Color.Navy;
            this.gridColGroupFlag.AppearanceCell.Options.UseForeColor = true;
            this.gridColGroupFlag.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColGroupFlag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColGroupFlag.FieldName = "fzfh";
            this.gridColGroupFlag.MinWidth = 18;
            this.gridColGroupFlag.Name = "gridColGroupFlag";
            this.gridColGroupFlag.OptionsColumn.AllowEdit = false;
            this.gridColGroupFlag.OptionsColumn.AllowFocus = false;
            this.gridColGroupFlag.OptionsColumn.AllowSize = false;
            this.gridColGroupFlag.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColGroupFlag.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.gridColGroupFlag.Visible = true;
            this.gridColGroupFlag.VisibleIndex = 1;
            this.gridColGroupFlag.Width = 18;
            // 
            // gridColItemName
            // 
            this.gridColItemName.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColItemName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColItemName.Caption = "药品/项目";
            this.gridColItemName.FieldName = "ypmc";
            this.gridColItemName.Name = "gridColItemName";
            this.gridColItemName.OptionsColumn.AllowEdit = false;
            this.gridColItemName.OptionsColumn.AllowFocus = false;
            this.gridColItemName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColItemName.Visible = true;
            this.gridColItemName.VisibleIndex = 2;
            this.gridColItemName.Width = 200;
            // 
            // gridColAmount
            // 
            this.gridColAmount.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColAmount.Caption = "数量";
            this.gridColAmount.ColumnEdit = this.repositoryItemSpinEdit1;
            this.gridColAmount.FieldName = "ypjl";
            this.gridColAmount.Name = "gridColAmount";
            this.gridColAmount.Visible = true;
            this.gridColAmount.VisibleIndex = 3;
            this.gridColAmount.Width = 60;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit1.Mask.EditMask = "####.###";
            this.repositoryItemSpinEdit1.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            this.repositoryItemSpinEdit1.NullText = "1";
            // 
            // gridColUnit
            // 
            this.gridColUnit.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColUnit.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColUnit.Caption = "单位";
            this.gridColUnit.FieldName = "jldw";
            this.gridColUnit.Name = "gridColUnit";
            this.gridColUnit.OptionsColumn.AllowEdit = false;
            this.gridColUnit.OptionsColumn.AllowFocus = false;
            this.gridColUnit.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColUnit.Visible = true;
            this.gridColUnit.VisibleIndex = 4;
            this.gridColUnit.Width = 60;
            // 
            // gridColUsage
            // 
            this.gridColUsage.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColUsage.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColUsage.Caption = "用法";
            this.gridColUsage.FieldName = "yfmc";
            this.gridColUsage.Name = "gridColUsage";
            this.gridColUsage.Visible = true;
            this.gridColUsage.VisibleIndex = 5;
            this.gridColUsage.Width = 80;
            // 
            // gridColFrequency
            // 
            this.gridColFrequency.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColFrequency.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColFrequency.Caption = "频次";
            this.gridColFrequency.FieldName = "pcmc";
            this.gridColFrequency.Name = "gridColFrequency";
            this.gridColFrequency.Visible = true;
            this.gridColFrequency.VisibleIndex = 6;
            this.gridColFrequency.Width = 80;
            // 
            // gridColDays
            // 
            this.gridColDays.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColDays.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColDays.Caption = "天数";
            this.gridColDays.ColumnEdit = this.repositoryItemSpinEdit2;
            this.gridColDays.FieldName = "zxts";
            this.gridColDays.Name = "gridColDays";
            this.gridColDays.Visible = true;
            this.gridColDays.VisibleIndex = 7;
            this.gridColDays.Width = 60;
            // 
            // repositoryItemSpinEdit2
            // 
            this.repositoryItemSpinEdit2.AutoHeight = false;
            this.repositoryItemSpinEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit2.Mask.EditMask = "d";
            this.repositoryItemSpinEdit2.MaxValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.repositoryItemSpinEdit2.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repositoryItemSpinEdit2.Name = "repositoryItemSpinEdit2";
            this.repositoryItemSpinEdit2.NullText = "1";
            // 
            // gridColMemo
            // 
            this.gridColMemo.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColMemo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColMemo.Caption = "嘱托";
            this.gridColMemo.FieldName = "ztnr";
            this.gridColMemo.Name = "gridColMemo";
            this.gridColMemo.OptionsColumn.AllowEdit = false;
            this.gridColMemo.OptionsColumn.AllowFocus = false;
            this.gridColMemo.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColMemo.Visible = true;
            this.gridColMemo.VisibleIndex = 8;
            this.gridColMemo.Width = 130;
            // 
            // OrderSuiteEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(837, 385);
            this.ControlBox = false;
            this.Controls.Add(this.gridCtrlSuiteDetail);
            this.Controls.Add(this.panelControl1);
            this.Font = new System.Drawing.Font("宋体", 10.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderSuiteEditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改成套医嘱";
            this.Shown += new System.EventHandler(this.OrderSuiteEditForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.showListWindow1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlSuiteDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSuiteDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).EndInit();
            this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnCancel;
      private DevExpress.XtraEditors.SimpleButton btnOk;
      private DrectSoft.Common.Library.LookUpWindow showListWindow1;
      private DevExpress.XtraGrid.GridControl gridCtrlSuiteDetail;
      private DevExpress.XtraGrid.Views.Grid.GridView gridViewSuiteDetail;
      private DevExpress.XtraGrid.Columns.GridColumn gridColGroupFlag;
      private DevExpress.XtraGrid.Columns.GridColumn gridColItemName;
      private DevExpress.XtraGrid.Columns.GridColumn gridColAmount;
      private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
      private DevExpress.XtraGrid.Columns.GridColumn gridColUnit;
      private DevExpress.XtraGrid.Columns.GridColumn gridColUsage;
      private DevExpress.XtraGrid.Columns.GridColumn gridColFrequency;
      private DevExpress.XtraGrid.Columns.GridColumn gridColMemo;
      private DevExpress.XtraGrid.Columns.GridColumn gridColCatalog;
      private DevExpress.XtraGrid.Columns.GridColumn gridColDays;
      private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit2;
   }
}