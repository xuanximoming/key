namespace DrectSoft.Core.MedicalRecordQuery
{
    partial class UCSetAttendDoctor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCSetAttendDoctor));
            this.scAttendDoctor = new DevExpress.XtraEditors.SplitContainerControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcmDepartment = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcmUserId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcmUserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pcSelectUser = new DevExpress.XtraEditors.PanelControl();
            this.lookUpEditorUserName = new DrectSoft.Common.Library.LookUpEditor();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnQuery = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey(this.components);
            this.lblUserName = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditorDepartment = new DrectSoft.Common.Library.LookUpEditor();
            this.lblDepartMent = new DevExpress.XtraEditors.LabelControl();
            this.lblFirstStep = new DevExpress.XtraEditors.PanelControl();
            this.pcFill = new DevExpress.XtraEditors.PanelControl();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcmGroupAuthority = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gcmGroupName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcmGroupEntity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.pcAttendTop = new DevExpress.XtraEditors.PanelControl();
            this.lblTwoStep = new DevExpress.XtraEditors.LabelControl();
            this.txtGroupName = new DevExpress.XtraEditors.TextEdit();
            this.lblGroupName = new DevExpress.XtraEditors.LabelControl();
            this.lblSearchKey = new DevExpress.XtraEditors.LabelControl();
            this.pcThere = new DevExpress.XtraEditors.PanelControl();
            this.lblThreeStep = new DevExpress.XtraEditors.LabelControl();
            this.DevButtonSave1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonSave(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.scAttendDoctor)).BeginInit();
            this.scAttendDoctor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcSelectUser)).BeginInit();
            this.pcSelectUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFirstStep)).BeginInit();
            this.lblFirstStep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcFill)).BeginInit();
            this.pcFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcAttendTop)).BeginInit();
            this.pcAttendTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcThere)).BeginInit();
            this.pcThere.SuspendLayout();
            this.SuspendLayout();
            // 
            // scAttendDoctor
            // 
            this.scAttendDoctor.Dock = System.Windows.Forms.DockStyle.Top;
            this.scAttendDoctor.Location = new System.Drawing.Point(0, 0);
            this.scAttendDoctor.Name = "scAttendDoctor";
            this.scAttendDoctor.Panel1.Controls.Add(this.panelControl1);
            this.scAttendDoctor.Panel1.Controls.Add(this.pcSelectUser);
            this.scAttendDoctor.Panel1.Text = "Panel1";
            this.scAttendDoctor.Panel2.Controls.Add(this.lblFirstStep);
            this.scAttendDoctor.Panel2.Text = "Panel2";
            this.scAttendDoctor.Size = new System.Drawing.Size(1213, 405);
            this.scAttendDoctor.SplitterPosition = 611;
            this.scAttendDoctor.TabIndex = 0;
            this.scAttendDoctor.Text = "splitContainerControl1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 84);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(611, 321);
            this.panelControl1.TabIndex = 1;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(607, 317);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcmDepartment,
            this.gcmUserId,
            this.gcmUserName});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 50;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gcmDepartment
            // 
            this.gcmDepartment.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmDepartment.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmDepartment.Caption = "科室";
            this.gcmDepartment.Name = "gcmDepartment";
            this.gcmDepartment.Visible = true;
            this.gcmDepartment.VisibleIndex = 0;
            // 
            // gcmUserId
            // 
            this.gcmUserId.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmUserId.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmUserId.Caption = "员工工号";
            this.gcmUserId.Name = "gcmUserId";
            this.gcmUserId.Visible = true;
            this.gcmUserId.VisibleIndex = 1;
            // 
            // gcmUserName
            // 
            this.gcmUserName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmUserName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmUserName.Caption = "员工姓名";
            this.gcmUserName.Name = "gcmUserName";
            this.gcmUserName.Visible = true;
            this.gcmUserName.VisibleIndex = 2;
            // 
            // pcSelectUser
            // 
            this.pcSelectUser.Controls.Add(this.lookUpEditorUserName);
            this.pcSelectUser.Controls.Add(this.labelControl1);
            this.pcSelectUser.Controls.Add(this.btnQuery);
            this.pcSelectUser.Controls.Add(this.lblUserName);
            this.pcSelectUser.Controls.Add(this.lookUpEditorDepartment);
            this.pcSelectUser.Controls.Add(this.lblDepartMent);
            this.pcSelectUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcSelectUser.Location = new System.Drawing.Point(0, 0);
            this.pcSelectUser.Name = "pcSelectUser";
            this.pcSelectUser.Size = new System.Drawing.Size(611, 84);
            this.pcSelectUser.TabIndex = 1;
            // 
            // lookUpEditorUserName
            // 
            this.lookUpEditorUserName.EnterMoveNextControl = true;
            this.lookUpEditorUserName.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorUserName.ListWindow = null;
            this.lookUpEditorUserName.Location = new System.Drawing.Point(351, 32);
            this.lookUpEditorUserName.Name = "lookUpEditorUserName";
            this.lookUpEditorUserName.ShowSButton = true;
            this.lookUpEditorUserName.Size = new System.Drawing.Size(150, 20);
            this.lookUpEditorUserName.TabIndex = 2;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Green;
            this.labelControl1.Location = new System.Drawing.Point(52, 63);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(108, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "第一步：请选择员工";
            // 
            // btnQuery
            // 
            this.btnQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnQuery.Image")));
            this.btnQuery.Location = new System.Drawing.Point(511, 30);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 23);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "查询(&Q)";
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(288, 34);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(60, 14);
            this.lblUserName.TabIndex = 3;
            this.lblUserName.Text = "员工姓名：";
            // 
            // lookUpEditorDepartment
            // 
            this.lookUpEditorDepartment.EnterMoveNextControl = true;
            this.lookUpEditorDepartment.Kind = DrectSoft.Wordbook.WordbookKind.Sql;
            this.lookUpEditorDepartment.ListWindow = null;
            this.lookUpEditorDepartment.ListWordbookName = "";
            this.lookUpEditorDepartment.Location = new System.Drawing.Point(119, 31);
            this.lookUpEditorDepartment.Name = "lookUpEditorDepartment";
            this.lookUpEditorDepartment.ShowSButton = true;
            this.lookUpEditorDepartment.Size = new System.Drawing.Size(150, 20);
            this.lookUpEditorDepartment.TabIndex = 1;
            this.lookUpEditorDepartment.ToolTip = "支持代码、名称(汉字/拼音/五笔)检索";
            this.lookUpEditorDepartment.ToolTipTitle = "科室";
            // 
            // lblDepartMent
            // 
            this.lblDepartMent.Location = new System.Drawing.Point(56, 33);
            this.lblDepartMent.Name = "lblDepartMent";
            this.lblDepartMent.Size = new System.Drawing.Size(60, 14);
            this.lblDepartMent.TabIndex = 1;
            this.lblDepartMent.Text = "所属科室：";
            // 
            // lblFirstStep
            // 
            this.lblFirstStep.Controls.Add(this.pcFill);
            this.lblFirstStep.Controls.Add(this.pcAttendTop);
            this.lblFirstStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFirstStep.Location = new System.Drawing.Point(0, 0);
            this.lblFirstStep.Name = "lblFirstStep";
            this.lblFirstStep.Size = new System.Drawing.Size(597, 405);
            this.lblFirstStep.TabIndex = 1;
            // 
            // pcFill
            // 
            this.pcFill.Controls.Add(this.gridControl2);
            this.pcFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcFill.Location = new System.Drawing.Point(2, 84);
            this.pcFill.Name = "pcFill";
            this.pcFill.Size = new System.Drawing.Size(593, 319);
            this.pcFill.TabIndex = 1;
            // 
            // gridControl2
            // 
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(2, 2);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemCheckEdit1});
            this.gridControl2.Size = new System.Drawing.Size(589, 315);
            this.gridControl2.TabIndex = 1;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcmGroupAuthority,
            this.gcmGroupName,
            this.gcmGroupEntity});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.IndicatorWidth = 50;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // gcmGroupAuthority
            // 
            this.gcmGroupAuthority.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmGroupAuthority.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmGroupAuthority.Caption = "组合权限";
            this.gcmGroupAuthority.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gcmGroupAuthority.FieldName = "gridColumn1";
            this.gcmGroupAuthority.Name = "gcmGroupAuthority";
            this.gcmGroupAuthority.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
            this.gcmGroupAuthority.Visible = true;
            this.gcmGroupAuthority.VisibleIndex = 0;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // gcmGroupName
            // 
            this.gcmGroupName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmGroupName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmGroupName.Caption = "组合名称";
            this.gcmGroupName.Name = "gcmGroupName";
            this.gcmGroupName.Visible = true;
            this.gcmGroupName.VisibleIndex = 1;
            // 
            // gcmGroupEntity
            // 
            this.gcmGroupEntity.AppearanceHeader.Options.UseTextOptions = true;
            this.gcmGroupEntity.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcmGroupEntity.Caption = "组合病种";
            this.gcmGroupEntity.Name = "gcmGroupEntity";
            this.gcmGroupEntity.Visible = true;
            this.gcmGroupEntity.VisibleIndex = 2;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // pcAttendTop
            // 
            this.pcAttendTop.Controls.Add(this.lblTwoStep);
            this.pcAttendTop.Controls.Add(this.txtGroupName);
            this.pcAttendTop.Controls.Add(this.lblGroupName);
            this.pcAttendTop.Controls.Add(this.lblSearchKey);
            this.pcAttendTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcAttendTop.Location = new System.Drawing.Point(2, 2);
            this.pcAttendTop.Name = "pcAttendTop";
            this.pcAttendTop.Size = new System.Drawing.Size(593, 82);
            this.pcAttendTop.TabIndex = 0;
            // 
            // lblTwoStep
            // 
            this.lblTwoStep.Appearance.ForeColor = System.Drawing.Color.Green;
            this.lblTwoStep.Location = new System.Drawing.Point(52, 62);
            this.lblTwoStep.Name = "lblTwoStep";
            this.lblTwoStep.Size = new System.Drawing.Size(204, 14);
            this.lblTwoStep.TabIndex = 6;
            this.lblTwoStep.Text = "第二步：请勾选要赋予员工的组合权限";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(223, 30);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(100, 21);
            this.txtGroupName.TabIndex = 4;
            // 
            // lblGroupName
            // 
            this.lblGroupName.Location = new System.Drawing.Point(153, 33);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(60, 14);
            this.lblGroupName.TabIndex = 2;
            this.lblGroupName.Text = "组合名称：";
            // 
            // lblSearchKey
            // 
            this.lblSearchKey.Location = new System.Drawing.Point(49, 33);
            this.lblSearchKey.Name = "lblSearchKey";
            this.lblSearchKey.Size = new System.Drawing.Size(75, 14);
            this.lblSearchKey.TabIndex = 1;
            this.lblSearchKey.Text = "检索条件>>>";
            // 
            // pcThere
            // 
            this.pcThere.Controls.Add(this.DevButtonSave1);
            this.pcThere.Controls.Add(this.lblThreeStep);
            this.pcThere.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcThere.Location = new System.Drawing.Point(0, 405);
            this.pcThere.Name = "pcThere";
            this.pcThere.Size = new System.Drawing.Size(1213, 69);
            this.pcThere.TabIndex = 1;
            // 
            // lblThreeStep
            // 
            this.lblThreeStep.Appearance.ForeColor = System.Drawing.Color.Green;
            this.lblThreeStep.Location = new System.Drawing.Point(52, 26);
            this.lblThreeStep.Name = "lblThreeStep";
            this.lblThreeStep.Size = new System.Drawing.Size(168, 14);
            this.lblThreeStep.TabIndex = 6;
            this.lblThreeStep.Text = "第三步：请保存设置的组合权限";
            // 
            // DevButtonSave1
            // 
            this.DevButtonSave1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonSave1.Image")));
            this.DevButtonSave1.Location = new System.Drawing.Point(1051, 28);
            this.DevButtonSave1.Name = "DevButtonSave1";
            this.DevButtonSave1.Size = new System.Drawing.Size(80, 23);
            this.DevButtonSave1.TabIndex = 8;
            this.DevButtonSave1.Text = "保存(&S)";
            // 
            // UCSetAttendDoctor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pcThere);
            this.Controls.Add(this.scAttendDoctor);
            this.Name = "UCSetAttendDoctor";
            this.Size = new System.Drawing.Size(1213, 474);
            ((System.ComponentModel.ISupportInitialize)(this.scAttendDoctor)).EndInit();
            this.scAttendDoctor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcSelectUser)).EndInit();
            this.pcSelectUser.ResumeLayout(false);
            this.pcSelectUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditorDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFirstStep)).EndInit();
            this.lblFirstStep.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcFill)).EndInit();
            this.pcFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcAttendTop)).EndInit();
            this.pcAttendTop.ResumeLayout(false);
            this.pcAttendTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGroupName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcThere)).EndInit();
            this.pcThere.ResumeLayout(false);
            this.pcThere.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl scAttendDoctor;
        private DevExpress.XtraEditors.PanelControl pcSelectUser;
        private DevExpress.XtraEditors.PanelControl lblFirstStep;
        private DevExpress.XtraEditors.LabelControl lblDepartMent;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey btnQuery;
        private DevExpress.XtraEditors.LabelControl lblUserName;
        private Common.Library.LookUpEditor lookUpEditorDepartment;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl pcThere;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gcmDepartment;
        private DevExpress.XtraGrid.Columns.GridColumn gcmUserId;
        private DevExpress.XtraGrid.Columns.GridColumn gcmUserName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl pcFill;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gcmGroupAuthority;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gcmGroupName;
        private DevExpress.XtraGrid.Columns.GridColumn gcmGroupEntity;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.PanelControl pcAttendTop;
        private DevExpress.XtraEditors.LabelControl lblTwoStep;
        private DevExpress.XtraEditors.TextEdit txtGroupName;
        private DevExpress.XtraEditors.LabelControl lblGroupName;
        private DevExpress.XtraEditors.LabelControl lblSearchKey;
        private DevExpress.XtraEditors.LabelControl lblThreeStep;
        private Common.Library.LookUpEditor lookUpEditorUserName;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonSave DevButtonSave1;
    }
}
