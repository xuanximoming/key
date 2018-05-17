namespace DrectSoft.Core.RedactPatientInfo.UserControls
{
    partial class UCDiseaseHistory
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dateEditDate = new DevExpress.XtraEditors.DateEdit();
            this.radioGroupCure = new DevExpress.XtraEditors.RadioGroup();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnDel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.btnModify = new DevExpress.XtraEditors.SimpleButton();
            this.memoEditDiscuss = new DevExpress.XtraEditors.MemoEdit();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl24 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl25 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl26 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl27 = new DevExpress.XtraEditors.LabelControl();
            this.gridControlDiseaseHistory = new DevExpress.XtraGrid.GridControl();
            this.gridViewDiseaseHistory = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn61 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn62 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn63 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn64 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView7 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn66 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn67 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn68 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn69 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn70 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn71 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn72 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn73 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn74 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn75 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn76 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn77 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn78 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn79 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn80 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lookUpWindowDiagnosisID = new DrectSoft.Common.Library.LookUpWindow(this.components);
            this.lookUpEditorDiagnosisID = new DrectSoft.Common.Library.LookUpEditor();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDate.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupCure.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditDiscuss.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiseaseHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiseaseHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDiagnosisID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDiagnosisID)).BeginInit();
            this.SuspendLayout();
            // 
            // dateEditDate
            // 
            this.dateEditDate.EditValue = null;
            this.dateEditDate.Location = new System.Drawing.Point(462, 275);
            this.dateEditDate.Name = "dateEditDate";
            this.dateEditDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditDate.Size = new System.Drawing.Size(186, 21);
            this.dateEditDate.TabIndex = 117;
            // 
            // radioGroupCure
            // 
            this.radioGroupCure.EditValue = false;
            this.radioGroupCure.Location = new System.Drawing.Point(103, 302);
            this.radioGroupCure.Name = "radioGroupCure";
            this.radioGroupCure.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroupCure.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroupCure.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroupCure.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "否"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "是")});
            this.radioGroupCure.Size = new System.Drawing.Size(182, 23);
            this.radioGroupCure.TabIndex = 116;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(299, 198);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 115;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(210, 198);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 114;
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(35, 198);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 113;
            this.btnAdd.Text = "添加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(122, 198);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 112;
            this.btnModify.Text = "修改";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // memoEditDiscuss
            // 
            this.memoEditDiscuss.Location = new System.Drawing.Point(102, 331);
            this.memoEditDiscuss.Name = "memoEditDiscuss";
            this.memoEditDiscuss.Size = new System.Drawing.Size(546, 47);
            this.memoEditDiscuss.TabIndex = 111;
            // 
            // panelControl4
            // 
            this.panelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl4.Location = new System.Drawing.Point(16, 229);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(788, 2);
            this.panelControl4.TabIndex = 110;
            // 
            // labelControl22
            // 
            this.labelControl22.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl22.Appearance.Options.UseFont = true;
            this.labelControl22.Location = new System.Drawing.Point(16, 247);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(112, 18);
            this.labelControl22.TabIndex = 109;
            this.labelControl22.Text = "病人疾病史维护";
            // 
            // labelControl24
            // 
            this.labelControl24.Location = new System.Drawing.Point(408, 278);
            this.labelControl24.Name = "labelControl24";
            this.labelControl24.Size = new System.Drawing.Size(48, 14);
            this.labelControl24.TabIndex = 108;
            this.labelControl24.Text = "发病时间";
            // 
            // labelControl25
            // 
            this.labelControl25.Location = new System.Drawing.Point(48, 333);
            this.labelControl25.Name = "labelControl25";
            this.labelControl25.Size = new System.Drawing.Size(48, 14);
            this.labelControl25.TabIndex = 107;
            this.labelControl25.Text = "疾病评论";
            // 
            // labelControl26
            // 
            this.labelControl26.Location = new System.Drawing.Point(48, 305);
            this.labelControl26.Name = "labelControl26";
            this.labelControl26.Size = new System.Drawing.Size(48, 14);
            this.labelControl26.TabIndex = 106;
            this.labelControl26.Text = "是否治愈";
            // 
            // labelControl27
            // 
            this.labelControl27.Location = new System.Drawing.Point(48, 278);
            this.labelControl27.Name = "labelControl27";
            this.labelControl27.Size = new System.Drawing.Size(48, 14);
            this.labelControl27.TabIndex = 105;
            this.labelControl27.Text = "病种名称";
            // 
            // gridControlDiseaseHistory
            // 
            this.gridControlDiseaseHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlDiseaseHistory.EmbeddedNavigator.Name = "";
            this.gridControlDiseaseHistory.Location = new System.Drawing.Point(16, 13);
            this.gridControlDiseaseHistory.MainView = this.gridViewDiseaseHistory;
            this.gridControlDiseaseHistory.Name = "gridControlDiseaseHistory";
            this.gridControlDiseaseHistory.Size = new System.Drawing.Size(793, 179);
            this.gridControlDiseaseHistory.TabIndex = 103;
            this.gridControlDiseaseHistory.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewDiseaseHistory,
            this.gridView7});
            // 
            // gridViewDiseaseHistory
            // 
            this.gridViewDiseaseHistory.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn61,
            this.gridColumn62,
            this.gridColumn63,
            this.gridColumn64,
            this.gridColumn1});
            this.gridViewDiseaseHistory.GridControl = this.gridControlDiseaseHistory;
            this.gridViewDiseaseHistory.Name = "gridViewDiseaseHistory";
            this.gridViewDiseaseHistory.OptionsBehavior.Editable = false;
            this.gridViewDiseaseHistory.OptionsCustomization.AllowFilter = false;
            this.gridViewDiseaseHistory.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewDiseaseHistory.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewDiseaseHistory.OptionsMenu.EnableFooterMenu = false;
            this.gridViewDiseaseHistory.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewDiseaseHistory.OptionsView.ShowFooter = true;
            this.gridViewDiseaseHistory.OptionsView.ShowGroupPanel = false;
            this.gridViewDiseaseHistory.OptionsView.ShowIndicator = false;
            this.gridViewDiseaseHistory.Click += new System.EventHandler(this.gridViewDiseaseHistory_Click);
            // 
            // gridColumn61
            // 
            this.gridColumn61.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn61.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn61.Caption = "病种名称";
            this.gridColumn61.FieldName = "DiagnosisName";
            this.gridColumn61.Name = "gridColumn61";
            this.gridColumn61.Visible = true;
            this.gridColumn61.VisibleIndex = 0;
            // 
            // gridColumn62
            // 
            this.gridColumn62.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn62.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn62.Caption = "发病时间";
            this.gridColumn62.FieldName = "DiseaseTime";
            this.gridColumn62.Name = "gridColumn62";
            this.gridColumn62.Visible = true;
            this.gridColumn62.VisibleIndex = 1;
            // 
            // gridColumn63
            // 
            this.gridColumn63.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn63.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn63.Caption = "是否治愈";
            this.gridColumn63.FieldName = "IsCure";
            this.gridColumn63.Name = "gridColumn63";
            this.gridColumn63.Visible = true;
            this.gridColumn63.VisibleIndex = 2;
            // 
            // gridColumn64
            // 
            this.gridColumn64.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn64.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn64.Caption = "疾病评论";
            this.gridColumn64.FieldName = "Discuss";
            this.gridColumn64.Name = "gridColumn64";
            this.gridColumn64.Visible = true;
            this.gridColumn64.VisibleIndex = 3;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "ID";
            this.gridColumn1.FieldName = "ID";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridView7
            // 
            this.gridView7.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn66,
            this.gridColumn67,
            this.gridColumn68,
            this.gridColumn69,
            this.gridColumn70,
            this.gridColumn71,
            this.gridColumn72,
            this.gridColumn73,
            this.gridColumn74,
            this.gridColumn75,
            this.gridColumn76,
            this.gridColumn77,
            this.gridColumn78,
            this.gridColumn79,
            this.gridColumn80});
            this.gridView7.GridControl = this.gridControlDiseaseHistory;
            this.gridView7.Name = "gridView7";
            this.gridView7.OptionsBehavior.Editable = false;
            this.gridView7.OptionsCustomization.AllowFilter = false;
            this.gridView7.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView7.OptionsFilter.AllowMRUFilterList = false;
            this.gridView7.OptionsMenu.EnableFooterMenu = false;
            this.gridView7.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView7.OptionsView.ShowFooter = true;
            this.gridView7.OptionsView.ShowGroupPanel = false;
            this.gridView7.OptionsView.ShowIndicator = false;
            // 
            // gridColumn66
            // 
            this.gridColumn66.Caption = "序号";
            this.gridColumn66.Name = "gridColumn66";
            // 
            // gridColumn67
            // 
            this.gridColumn67.Caption = "住院号";
            this.gridColumn67.FieldName = "PatID";
            this.gridColumn67.Name = "gridColumn67";
            this.gridColumn67.Visible = true;
            this.gridColumn67.VisibleIndex = 0;
            // 
            // gridColumn68
            // 
            this.gridColumn68.Caption = "病历号";
            this.gridColumn68.FieldName = "NoOfRecord";
            this.gridColumn68.Name = "gridColumn68";
            this.gridColumn68.Visible = true;
            this.gridColumn68.VisibleIndex = 1;
            // 
            // gridColumn69
            // 
            this.gridColumn69.Caption = "姓名";
            this.gridColumn69.FieldName = "Name";
            this.gridColumn69.Name = "gridColumn69";
            this.gridColumn69.Visible = true;
            this.gridColumn69.VisibleIndex = 2;
            // 
            // gridColumn70
            // 
            this.gridColumn70.Caption = "性别";
            this.gridColumn70.FieldName = "SexName";
            this.gridColumn70.Name = "gridColumn70";
            this.gridColumn70.Visible = true;
            this.gridColumn70.VisibleIndex = 3;
            // 
            // gridColumn71
            // 
            this.gridColumn71.Caption = "年龄";
            this.gridColumn71.FieldName = "AgeStr";
            this.gridColumn71.Name = "gridColumn71";
            this.gridColumn71.Visible = true;
            this.gridColumn71.VisibleIndex = 5;
            // 
            // gridColumn72
            // 
            this.gridColumn72.Caption = "住院次";
            this.gridColumn72.FieldName = "InCount";
            this.gridColumn72.Name = "gridColumn72";
            this.gridColumn72.Visible = true;
            this.gridColumn72.VisibleIndex = 4;
            // 
            // gridColumn73
            // 
            this.gridColumn73.Caption = "入院日期";
            this.gridColumn73.FieldName = "AdmitDate";
            this.gridColumn73.Name = "gridColumn73";
            this.gridColumn73.Visible = true;
            this.gridColumn73.VisibleIndex = 6;
            // 
            // gridColumn74
            // 
            this.gridColumn74.Caption = "病房号";
            this.gridColumn74.FieldName = "WardId";
            this.gridColumn74.Name = "gridColumn74";
            this.gridColumn74.Visible = true;
            this.gridColumn74.VisibleIndex = 7;
            // 
            // gridColumn75
            // 
            this.gridColumn75.Caption = "床号";
            this.gridColumn75.FieldName = "BedID";
            this.gridColumn75.Name = "gridColumn75";
            this.gridColumn75.Visible = true;
            this.gridColumn75.VisibleIndex = 8;
            // 
            // gridColumn76
            // 
            this.gridColumn76.Caption = "住院日";
            this.gridColumn76.Name = "gridColumn76";
            this.gridColumn76.Visible = true;
            this.gridColumn76.VisibleIndex = 9;
            // 
            // gridColumn77
            // 
            this.gridColumn77.Caption = "经治状态";
            this.gridColumn77.FieldName = "status";
            this.gridColumn77.Name = "gridColumn77";
            this.gridColumn77.Visible = true;
            this.gridColumn77.VisibleIndex = 10;
            // 
            // gridColumn78
            // 
            this.gridColumn78.Caption = "住院医生";
            this.gridColumn78.FieldName = "Resident";
            this.gridColumn78.Name = "gridColumn78";
            this.gridColumn78.Visible = true;
            this.gridColumn78.VisibleIndex = 11;
            // 
            // gridColumn79
            // 
            this.gridColumn79.Caption = "主治医师";
            this.gridColumn79.FieldName = "Attend";
            this.gridColumn79.Name = "gridColumn79";
            this.gridColumn79.Visible = true;
            this.gridColumn79.VisibleIndex = 12;
            // 
            // gridColumn80
            // 
            this.gridColumn80.Caption = "主任医生";
            this.gridColumn80.FieldName = "Chief";
            this.gridColumn80.Name = "gridColumn80";
            this.gridColumn80.Visible = true;
            this.gridColumn80.VisibleIndex = 13;
            // 
            // lookUpWindowDiagnosisID
            // 
            this.lookUpWindowDiagnosisID.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowDiagnosisID.GenShortCode = null;
            this.lookUpWindowDiagnosisID.MatchType = DrectSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowDiagnosisID.Owner = null;
            this.lookUpWindowDiagnosisID.SqlHelper = null;
            // 
            // lookUpEditorDiagnosisID
            // 
            this.lookUpEditorDiagnosisID.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDiagnosisID.ListWindow = this.lookUpWindowDiagnosisID;
            this.lookUpEditorDiagnosisID.ListWordbookName = "";
            this.lookUpEditorDiagnosisID.Location = new System.Drawing.Point(102, 277);
            this.lookUpEditorDiagnosisID.Name = "lookUpEditorDiagnosisID";
            this.lookUpEditorDiagnosisID.ShowFormImmediately = true;
            this.lookUpEditorDiagnosisID.Size = new System.Drawing.Size(209, 20);
            this.lookUpEditorDiagnosisID.TabIndex = 150;
            // 
            // UCDiseaseHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lookUpEditorDiagnosisID);
            this.Controls.Add(this.dateEditDate);
            this.Controls.Add(this.radioGroupCure);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.memoEditDiscuss);
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.labelControl22);
            this.Controls.Add(this.labelControl24);
            this.Controls.Add(this.labelControl25);
            this.Controls.Add(this.labelControl26);
            this.Controls.Add(this.labelControl27);
            this.Controls.Add(this.gridControlDiseaseHistory);
            this.Name = "UCDiseaseHistory";
            this.Size = new System.Drawing.Size(824, 391);
            this.Load += new System.EventHandler(this.UCOperationHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDate.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroupCure.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditDiscuss.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDiseaseHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewDiseaseHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowDiagnosisID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDiagnosisID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit dateEditDate;
        private DevExpress.XtraEditors.RadioGroup radioGroupCure;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnDel;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.SimpleButton btnModify;
        private DevExpress.XtraEditors.MemoEdit memoEditDiscuss;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.LabelControl labelControl24;
        private DevExpress.XtraEditors.LabelControl labelControl25;
        private DevExpress.XtraEditors.LabelControl labelControl26;
        private DevExpress.XtraEditors.LabelControl labelControl27;
        private DevExpress.XtraGrid.GridControl gridControlDiseaseHistory;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewDiseaseHistory;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn61;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn62;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn63;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn64;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn66;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn67;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn68;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn69;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn70;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn71;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn72;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn73;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn74;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn75;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn76;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn77;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn78;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn79;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn80;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DrectSoft.Common.Library.LookUpWindow lookUpWindowDiagnosisID;
        private DrectSoft.Common.Library.LookUpEditor lookUpEditorDiagnosisID;
    }
}
