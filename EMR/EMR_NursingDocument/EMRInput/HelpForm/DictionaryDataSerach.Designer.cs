namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HelpForm
{
    partial class DictionaryDataSerach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DictionaryDataSerach));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.checkEditWB = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditICD = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditName = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditPY = new DevExpress.XtraEditors.CheckEdit();
            this.textEditInput = new DevExpress.XtraEditors.TextEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControlDiag = new DevExpress.XtraGrid.GridControl();
            this.gridViewDiag = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditWB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditICD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditInput.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiag)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.checkEditWB);
            this.panelControl1.Controls.Add(this.checkEditICD);
            this.panelControl1.Controls.Add(this.checkEditName);
            this.panelControl1.Controls.Add(this.checkEditPY);
            this.panelControl1.Controls.Add(this.textEditInput);
            this.panelControl1.Location = new System.Drawing.Point(-5, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(461, 36);
            this.panelControl1.TabIndex = 0;
            // 
            // checkEditWB
            // 
            this.checkEditWB.Location = new System.Drawing.Point(98, 9);
            this.checkEditWB.Name = "checkEditWB";
            this.checkEditWB.Properties.Caption = "五笔字头(&W)";
            this.checkEditWB.Size = new System.Drawing.Size(94, 19);
            this.checkEditWB.TabIndex = 3;
            this.checkEditWB.CheckedChanged += new System.EventHandler(this.checkEditWB_CheckedChanged);
            // 
            // checkEditICD
            // 
            this.checkEditICD.Location = new System.Drawing.Point(258, 9);
            this.checkEditICD.Name = "checkEditICD";
            this.checkEditICD.Properties.Caption = "编码(&C)";
            this.checkEditICD.Size = new System.Drawing.Size(65, 19);
            this.checkEditICD.TabIndex = 5;
            this.checkEditICD.CheckedChanged += new System.EventHandler(this.checkEditICD_CheckedChanged);
            // 
            // checkEditName
            // 
            this.checkEditName.EditValue = true;
            this.checkEditName.Location = new System.Drawing.Point(191, 9);
            this.checkEditName.Name = "checkEditName";
            this.checkEditName.Properties.Caption = "名称(&M)";
            this.checkEditName.Size = new System.Drawing.Size(66, 19);
            this.checkEditName.TabIndex = 4;
            this.checkEditName.CheckedChanged += new System.EventHandler(this.checkEditName_CheckedChanged);
            // 
            // checkEditPY
            // 
            this.checkEditPY.EditValue = true;
            this.checkEditPY.Location = new System.Drawing.Point(10, 9);
            this.checkEditPY.Name = "checkEditPY";
            this.checkEditPY.Properties.Caption = "拼音字头(&P)";
            this.checkEditPY.Size = new System.Drawing.Size(92, 19);
            this.checkEditPY.TabIndex = 2;
            this.checkEditPY.CheckedChanged += new System.EventHandler(this.checkEditPY_CheckedChanged);
            // 
            // textEditInput
            // 
            this.textEditInput.Location = new System.Drawing.Point(329, 8);
            this.textEditInput.Name = "textEditInput";
            this.textEditInput.Size = new System.Drawing.Size(115, 20);
            this.textEditInput.TabIndex = 1;
            this.textEditInput.TextChanged += new System.EventHandler(this.textEditInput_TextChanged);
            this.textEditInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEditInput_KeyDown);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gridControlDiag);
            this.panelControl2.Location = new System.Drawing.Point(-5, 36);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(461, 343);
            this.panelControl2.TabIndex = 1;
            // 
            // gridControlDiag
            // 
            this.gridControlDiag.Location = new System.Drawing.Point(13, 7);
            this.gridControlDiag.MainView = this.gridViewDiag;
            this.gridControlDiag.Name = "gridControlDiag";
            this.gridControlDiag.Size = new System.Drawing.Size(431, 320);
            this.gridControlDiag.TabIndex = 6;
            this.gridControlDiag.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDiag});
            this.gridControlDiag.DoubleClick += new System.EventHandler(this.gridControlDiag_DoubleClick);
            this.gridControlDiag.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridControlDiag_MouseUp);
            // 
            // gridViewDiag
            // 
            this.gridViewDiag.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn4,
            this.gridColumn2,
            this.gridColumn3});
            this.gridViewDiag.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewDiag.GridControl = this.gridControlDiag;
            this.gridViewDiag.Name = "gridViewDiag";
            this.gridViewDiag.OptionsBehavior.Editable = false;
            this.gridViewDiag.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewDiag.OptionsCustomization.AllowColumnResizing = false;
            this.gridViewDiag.OptionsCustomization.AllowFilter = false;
            this.gridViewDiag.OptionsCustomization.AllowGroup = false;
            this.gridViewDiag.OptionsCustomization.AllowSort = false;
            this.gridViewDiag.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewDiag.OptionsView.ShowGroupPanel = false;
            this.gridViewDiag.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "拼音";
            this.gridColumn1.FieldName = "py";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 60;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "五笔";
            this.gridColumn4.FieldName = "wb";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 60;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "名称";
            this.gridColumn2.FieldName = "NAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 215;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "编码";
            this.gridColumn3.FieldName = "icd";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 80;
            // 
            // DictionaryDataSerach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 372);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DictionaryDataSerach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据字典检索";
            this.Load += new System.EventHandler(this.DictionaryDataSerach_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEditWB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditICD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditInput.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiag)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.CheckEdit checkEditPY;
        private DevExpress.XtraEditors.TextEdit textEditInput;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.CheckEdit checkEditICD;
        private DevExpress.XtraEditors.CheckEdit checkEditName;
        private DevExpress.XtraGrid.GridControl gridControlDiag;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDiag;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.CheckEdit checkEditWB;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}