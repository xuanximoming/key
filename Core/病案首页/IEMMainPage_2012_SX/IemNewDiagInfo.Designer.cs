namespace DrectSoft.Core.IEMMainPage
{
    partial class IemNewDiagInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IemNewDiagInfo));
            this.checkEditPY = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditWB = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditName = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditICD = new DevExpress.XtraEditors.CheckEdit();
            this.textEditInput = new DevExpress.XtraEditors.TextEdit();
            this.gridControlDiag = new DevExpress.XtraGrid.GridControl();
            this.gridViewDiag = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditWB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditICD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditInput.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiag)).BeginInit();
            this.SuspendLayout();
            // 
            // checkEditPY
            // 
            this.checkEditPY.EditValue = true;
            this.checkEditPY.Location = new System.Drawing.Point(3, 13);
            this.checkEditPY.Name = "checkEditPY";
            this.checkEditPY.Properties.Caption = "拼音字头(&P)";
            this.checkEditPY.Size = new System.Drawing.Size(92, 19);
            this.checkEditPY.TabIndex = 1;
            this.checkEditPY.CheckedChanged += new System.EventHandler(this.checkEditPY_CheckedChanged);
            // 
            // checkEditWB
            // 
            this.checkEditWB.Location = new System.Drawing.Point(103, 13);
            this.checkEditWB.Name = "checkEditWB";
            this.checkEditWB.Properties.Caption = "五笔字头(&W)";
            this.checkEditWB.Size = new System.Drawing.Size(92, 19);
            this.checkEditWB.TabIndex = 4;
            this.checkEditWB.CheckedChanged += new System.EventHandler(this.checkEditWB_CheckedChanged);
            // 
            // checkEditName
            // 
            this.checkEditName.EditValue = true;
            this.checkEditName.Location = new System.Drawing.Point(203, 13);
            this.checkEditName.Name = "checkEditName";
            this.checkEditName.Properties.Caption = "名称(&M)";
            this.checkEditName.Size = new System.Drawing.Size(66, 19);
            this.checkEditName.TabIndex = 2;
            this.checkEditName.CheckedChanged += new System.EventHandler(this.checkEditName_CheckedChanged);
            // 
            // checkEditICD
            // 
            this.checkEditICD.Location = new System.Drawing.Point(277, 13);
            this.checkEditICD.Name = "checkEditICD";
            this.checkEditICD.Properties.Caption = "编码(&C)";
            this.checkEditICD.Size = new System.Drawing.Size(66, 19);
            this.checkEditICD.TabIndex = 3;
            this.checkEditICD.CheckedChanged += new System.EventHandler(this.checkEditICD_CheckedChanged);
            // 
            // textEditInput
            // 
            this.textEditInput.Location = new System.Drawing.Point(351, 13);
            this.textEditInput.Name = "textEditInput";
            this.textEditInput.Size = new System.Drawing.Size(115, 20);
            this.textEditInput.TabIndex = 0;
            this.textEditInput.TextChanged += new System.EventHandler(this.textEditInput_TextChanged);
            this.textEditInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEditInput_KeyDown);
            // 
            // gridControlDiag
            // 
            this.gridControlDiag.Location = new System.Drawing.Point(5, 40);
            this.gridControlDiag.MainView = this.gridViewDiag;
            this.gridControlDiag.Name = "gridControlDiag";
            this.gridControlDiag.Size = new System.Drawing.Size(461, 284);
            this.gridControlDiag.TabIndex = 5;
            this.gridControlDiag.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDiag});
            // 
            // gridViewDiag
            // 
            this.gridViewDiag.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gridViewDiag.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewDiag.GridControl = this.gridControlDiag;
            this.gridViewDiag.Name = "gridViewDiag";
            this.gridViewDiag.OptionsBehavior.Editable = false;
            this.gridViewDiag.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewDiag.OptionsCustomization.AllowFilter = false;
            this.gridViewDiag.OptionsCustomization.AllowGroup = false;
            this.gridViewDiag.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridViewDiag.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewDiag.OptionsFilter.AllowFilterEditor = false;
            this.gridViewDiag.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewDiag.OptionsMenu.EnableColumnMenu = false;
            this.gridViewDiag.OptionsMenu.EnableFooterMenu = false;
            this.gridViewDiag.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewDiag.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gridViewDiag.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridViewDiag.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridViewDiag.OptionsView.ShowGroupPanel = false;
            this.gridViewDiag.DoubleClick += new System.EventHandler(this.gridViewDiag_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "拼音";
            this.gridColumn1.FieldName = "PY";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "五笔";
            this.gridColumn2.FieldName = "WB";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 60;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "名称";
            this.gridColumn3.FieldName = "NAME";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 215;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "编码";
            this.gridColumn4.FieldName = "ICD";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // IemNewDiagInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 326);
            this.Controls.Add(this.gridControlDiag);
            this.Controls.Add(this.textEditInput);
            this.Controls.Add(this.checkEditICD);
            this.Controls.Add(this.checkEditName);
            this.Controls.Add(this.checkEditWB);
            this.Controls.Add(this.checkEditPY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IemNewDiagInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "搜索结果";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.IemNewDiagInfo_FormClosed);
            this.Load += new System.EventHandler(this.IemNewDiagInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditWB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditICD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditInput.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiag)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit checkEditPY;
        private DevExpress.XtraEditors.CheckEdit checkEditWB;
        private DevExpress.XtraEditors.CheckEdit checkEditName;
        private DevExpress.XtraEditors.CheckEdit checkEditICD;
        private DevExpress.XtraEditors.TextEdit textEditInput;
        private DevExpress.XtraGrid.GridControl gridControlDiag;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDiag;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;


    }
}