namespace YindanSoft.Emr.QcManagerNew
{
    partial class QCScoreType
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
            this.lookUpWindowType = new YidanSoft.Common.Library.LookUpWindow(this.components);
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnQueryDetail = new DevExpress.XtraEditors.SimpleButton();
            this.btnQuery = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnDel = new DevExpress.XtraEditors.SimpleButton();
            this.btnEdit = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.gridviewScoreCoreType = new DevExpress.XtraGrid.GridControl();
            this.gridViewScoreType = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewScoreCoreType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewScoreType)).BeginInit();
            this.SuspendLayout();
            // 
            // lookUpWindowType
            // 
            this.lookUpWindowType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lookUpWindowType.GenShortCode = null;
            this.lookUpWindowType.MatchType = YidanSoft.Common.Library.ShowListMatchType.Any;
            this.lookUpWindowType.Owner = null;
            this.lookUpWindowType.SqlHelper = null;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnQueryDetail);
            this.panelControl1.Controls.Add(this.btnQuery);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(841, 50);
            this.panelControl1.TabIndex = 0;
            // 
            // btnQueryDetail
            // 
            this.btnQueryDetail.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.查看详细;
            this.btnQueryDetail.Location = new System.Drawing.Point(744, 12);
            this.btnQueryDetail.Name = "btnQueryDetail";
            this.btnQueryDetail.Size = new System.Drawing.Size(80, 27);
            this.btnQueryDetail.TabIndex = 1;
            this.btnQueryDetail.Text = "查询明细";
            this.btnQueryDetail.Click += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // btnQuery
            // 
            this.btnQuery.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.查询;
            this.btnQuery.Location = new System.Drawing.Point(649, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 27);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询(&Q)";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnDel);
            this.panelControl2.Controls.Add(this.btnEdit);
            this.panelControl2.Controls.Add(this.btnAdd);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 445);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(841, 44);
            this.panelControl2.TabIndex = 2;
            // 
            // btnDel
            // 
            this.btnDel.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.删除;
            this.btnDel.Location = new System.Drawing.Point(744, 12);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(80, 27);
            this.btnDel.TabIndex = 2;
            this.btnDel.Text = "删除";
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.编辑;
            this.btnEdit.Location = new System.Drawing.Point(648, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 27);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "编辑";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::YindanSoft.Emr.QcManagerNew.Properties.Resources.新增;
            this.btnAdd.Location = new System.Drawing.Point(555, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 27);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gridviewScoreCoreType
            // 
            this.gridviewScoreCoreType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridviewScoreCoreType.Location = new System.Drawing.Point(0, 50);
            this.gridviewScoreCoreType.MainView = this.gridViewScoreType;
            this.gridviewScoreCoreType.Name = "gridviewScoreCoreType";
            this.gridviewScoreCoreType.Size = new System.Drawing.Size(841, 395);
            this.gridviewScoreCoreType.TabIndex = 1;
            this.gridviewScoreCoreType.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewScoreType});
            this.gridviewScoreCoreType.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // gridViewScoreType
            // 
            this.gridViewScoreType.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6});
            this.gridViewScoreType.GridControl = this.gridviewScoreCoreType;
            this.gridViewScoreType.Name = "gridViewScoreType";
            this.gridViewScoreType.OptionsBehavior.Editable = false;
            this.gridViewScoreType.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewScoreType.OptionsMenu.EnableColumnMenu = false;
            this.gridViewScoreType.OptionsMenu.EnableFooterMenu = false;
            this.gridViewScoreType.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewScoreType.OptionsView.ShowFooter = true;
            this.gridViewScoreType.OptionsView.ShowGroupPanel = false;
            this.gridViewScoreType.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "项目代码";
            this.gridColumn1.FieldName = "TYPECODE";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 139;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "项目名称";
            this.gridColumn2.FieldName = "TYPENAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 139;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "项目说明";
            this.gridColumn3.FieldName = "TYPEINSTRUCTION";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 139;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "评分类型";
            this.gridColumn4.FieldName = "TYPECATEGORYNAME";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 113;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "排列顺序";
            this.gridColumn5.FieldName = "TYPEORDER";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 99;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "备注";
            this.gridColumn6.FieldName = "TYPEMEMO";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 210;
            // 
            // QCScoreType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 489);
            this.Controls.Add(this.gridviewScoreCoreType);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.MaximizeBox = false;
            this.Name = "QCScoreType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "标准评分大项维护";
            this.Load += new System.EventHandler(this.QCScoreType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpWindowType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridviewScoreCoreType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewScoreType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private YidanSoft.Common.Library.LookUpWindow lookUpWindowType;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnDel;
        private DevExpress.XtraEditors.SimpleButton btnEdit;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraGrid.GridControl gridviewScoreCoreType;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewScoreType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.SimpleButton btnQueryDetail;
        private DevExpress.XtraEditors.SimpleButton btnQuery;

    }
}