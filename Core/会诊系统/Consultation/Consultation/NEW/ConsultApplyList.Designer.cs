namespace Consultation.NEW
{
    partial class ConsultApplyList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsultApplyList));
            this.gridControlApplyList = new DevExpress.XtraGrid.GridControl();
            this.gridViewApplyList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlApplyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewApplyList)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlApplyList
            // 
            this.gridControlApplyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlApplyList.Location = new System.Drawing.Point(0, 0);
            this.gridControlApplyList.MainView = this.gridViewApplyList;
            this.gridControlApplyList.Name = "gridControlApplyList";
            this.gridControlApplyList.Size = new System.Drawing.Size(462, 305);
            this.gridControlApplyList.TabIndex = 0;
            this.gridControlApplyList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewApplyList});
            // 
            // gridViewApplyList
            // 
            this.gridViewApplyList.Appearance.GroupPanel.BackColor = System.Drawing.Color.Transparent;
            this.gridViewApplyList.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gridViewApplyList.Appearance.ViewCaption.ForeColor = System.Drawing.Color.Green;
            this.gridViewApplyList.Appearance.ViewCaption.Options.UseForeColor = true;
            this.gridViewApplyList.Appearance.ViewCaption.Options.UseTextOptions = true;
            this.gridViewApplyList.Appearance.ViewCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewApplyList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gridViewApplyList.GridControl = this.gridControlApplyList;
            this.gridViewApplyList.Name = "gridViewApplyList";
            this.gridViewApplyList.OptionsBehavior.Editable = false;
            this.gridViewApplyList.OptionsBehavior.ReadOnly = true;
            this.gridViewApplyList.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewApplyList.OptionsCustomization.AllowFilter = false;
            this.gridViewApplyList.OptionsCustomization.AllowGroup = false;
            this.gridViewApplyList.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridViewApplyList.OptionsMenu.EnableColumnMenu = false;
            this.gridViewApplyList.OptionsMenu.EnableFooterMenu = false;
            this.gridViewApplyList.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewApplyList.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewApplyList.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewApplyList.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewApplyList.OptionsView.ShowGroupPanel = false;
            this.gridViewApplyList.OptionsView.ShowViewCaption = true;
            this.gridViewApplyList.ViewCaption = "双击选中需修改申请记录";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "会诊单号";
            this.gridColumn1.FieldName = "CONSULTAPPLYSN";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "紧急程度";
            this.gridColumn2.FieldName = "URGENCYTYPEID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "申请医师";
            this.gridColumn3.FieldName = "APPLYUSER";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "申请时间";
            this.gridColumn4.FieldName = "APPLYTIME";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "状态";
            this.gridColumn5.FieldName = "STATEID";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // ConsultApplyList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 305);
            this.Controls.Add(this.gridControlApplyList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConsultApplyList";
            this.Text = "会诊申请列表";
            ((System.ComponentModel.ISupportInitialize)(this.gridControlApplyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewApplyList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlApplyList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewApplyList;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}