namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HelpForm
{
    partial class DoctorCustomForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DoctorCustomForm));
            this.lookUpEditType = new DevExpress.XtraEditors.LookUpEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.gridControlDiag = new DevExpress.XtraGrid.GridControl();
            this.gridViewDiag = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.checkEditWB = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditICD = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditName = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditPY = new DevExpress.XtraEditors.CheckEdit();
            this.textEditInput = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiag)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditWB.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditICD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditInput.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lookUpEditType
            // 
            this.lookUpEditType.Location = new System.Drawing.Point(43, 10);
            this.lookUpEditType.Name = "lookUpEditType";
            this.lookUpEditType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditType.Size = new System.Drawing.Size(84, 20);
            this.lookUpEditType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "类别";
            // 
            // gridControlDiag
            // 
            this.gridControlDiag.Location = new System.Drawing.Point(10, 37);
            this.gridControlDiag.MainView = this.gridViewDiag;
            this.gridControlDiag.Name = "gridControlDiag";
            this.gridControlDiag.Size = new System.Drawing.Size(541, 382);
            this.gridControlDiag.TabIndex = 7;
            this.gridControlDiag.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDiag});
            this.gridControlDiag.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControlDiag_KeyDown);
            this.gridControlDiag.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gridControlDiag_MouseDoubleClick);
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
            this.gridColumn1.FieldName = "PY";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 60;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "五笔";
            this.gridColumn4.FieldName = "WB";
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
            this.gridColumn3.FieldName = "ICD";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 80;
            // 
            // checkEditWB
            // 
            this.checkEditWB.Location = new System.Drawing.Point(229, 11);
            this.checkEditWB.Name = "checkEditWB";
            this.checkEditWB.Properties.Caption = "五笔字头(&W)";
            this.checkEditWB.Size = new System.Drawing.Size(94, 19);
            this.checkEditWB.TabIndex = 9;
            this.checkEditWB.CheckedChanged += new System.EventHandler(this.checkEditWB_CheckedChanged);
            // 
            // checkEditICD
            // 
            this.checkEditICD.Location = new System.Drawing.Point(389, 11);
            this.checkEditICD.Name = "checkEditICD";
            this.checkEditICD.Properties.Caption = "编码(&C)";
            this.checkEditICD.Size = new System.Drawing.Size(65, 19);
            this.checkEditICD.TabIndex = 11;
            this.checkEditICD.CheckedChanged += new System.EventHandler(this.checkEditICD_CheckedChanged);
            // 
            // checkEditName
            // 
            this.checkEditName.EditValue = true;
            this.checkEditName.Location = new System.Drawing.Point(322, 11);
            this.checkEditName.Name = "checkEditName";
            this.checkEditName.Properties.Caption = "名称(&M)";
            this.checkEditName.Size = new System.Drawing.Size(66, 19);
            this.checkEditName.TabIndex = 10;
            this.checkEditName.CheckedChanged += new System.EventHandler(this.checkEditName_CheckedChanged);
            // 
            // checkEditPY
            // 
            this.checkEditPY.EditValue = true;
            this.checkEditPY.Location = new System.Drawing.Point(141, 11);
            this.checkEditPY.Name = "checkEditPY";
            this.checkEditPY.Properties.Caption = "拼音字头(&P)";
            this.checkEditPY.Size = new System.Drawing.Size(92, 19);
            this.checkEditPY.TabIndex = 8;
            this.checkEditPY.CheckedChanged += new System.EventHandler(this.checkEditPY_CheckedChanged);
            // 
            // textEditInput
            // 
            this.textEditInput.Location = new System.Drawing.Point(460, 10);
            this.textEditInput.Name = "textEditInput";
            this.textEditInput.Size = new System.Drawing.Size(90, 20);
            this.textEditInput.TabIndex = 12;
            this.textEditInput.TextChanged += new System.EventHandler(this.textEditInput_TextChanged);
            // 
            // DoctorCustomForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 427);
            this.Controls.Add(this.textEditInput);
            this.Controls.Add(this.checkEditWB);
            this.Controls.Add(this.checkEditICD);
            this.Controls.Add(this.checkEditName);
            this.Controls.Add(this.checkEditPY);
            this.Controls.Add(this.gridControlDiag);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lookUpEditType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DoctorCustomForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "工具字典";
            this.Load += new System.EventHandler(this.DoctorCustomForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiag)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditWB.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditICD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditInput.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit lookUpEditType;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gridControlDiag;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDiag;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.CheckEdit checkEditWB;
        private DevExpress.XtraEditors.CheckEdit checkEditICD;
        private DevExpress.XtraEditors.CheckEdit checkEditName;
        private DevExpress.XtraEditors.CheckEdit checkEditPY;
        private DevExpress.XtraEditors.TextEdit textEditInput;
    }
}