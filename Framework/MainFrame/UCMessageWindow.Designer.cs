namespace DrectSoft.MainFrame
{
    partial class UCMessageWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMessageWindow));
            this.groupControlMessageForm = new DevExpress.XtraEditors.GroupControl();
            this.gridControlMessage = new DevExpress.XtraGrid.GridControl();
            this.gridViewMessage = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gc_typeid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gc_typename = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gc_content = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pictureBoxExit = new System.Windows.Forms.PictureBox();
            this.timerMoving = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMessageForm)).BeginInit();
            this.groupControlMessageForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExit)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControlMessageForm
            // 
            this.groupControlMessageForm.AppearanceCaption.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupControlMessageForm.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupControlMessageForm.AppearanceCaption.Options.UseFont = true;
            this.groupControlMessageForm.AppearanceCaption.Options.UseForeColor = true;
            this.groupControlMessageForm.Controls.Add(this.gridControlMessage);
            this.groupControlMessageForm.Controls.Add(this.pictureBoxExit);
            this.groupControlMessageForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlMessageForm.Location = new System.Drawing.Point(0, 0);
            this.groupControlMessageForm.Name = "groupControlMessageForm";
            this.groupControlMessageForm.Size = new System.Drawing.Size(340, 184);
            this.groupControlMessageForm.TabIndex = 1;
            this.groupControlMessageForm.Text = "系统提示";
            this.groupControlMessageForm.Paint += new System.Windows.Forms.PaintEventHandler(this.groupControlMessageForm_Paint);
            this.groupControlMessageForm.MouseEnter += new System.EventHandler(this.groupControlMessageForm_MouseEnter);
            this.groupControlMessageForm.MouseLeave += new System.EventHandler(this.groupControlMessageForm_MouseLeave);
            // 
            // gridControlMessage
            // 
            this.gridControlMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlMessage.Location = new System.Drawing.Point(2, 22);
            this.gridControlMessage.MainView = this.gridViewMessage;
            this.gridControlMessage.Name = "gridControlMessage";
            this.gridControlMessage.Size = new System.Drawing.Size(336, 160);
            this.gridControlMessage.TabIndex = 1;
            this.gridControlMessage.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMessage});
            // 
            // gridViewMessage
            // 
            this.gridViewMessage.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gc_typeid,
            this.gc_typename,
            this.gc_content});
            this.gridViewMessage.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewMessage.GridControl = this.gridControlMessage;
            this.gridViewMessage.Name = "gridViewMessage";
            this.gridViewMessage.OptionsBehavior.Editable = false;
            this.gridViewMessage.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewMessage.OptionsFilter.AllowFilterEditor = false;
            this.gridViewMessage.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewMessage.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewMessage.OptionsView.ShowColumnHeaders = false;
            this.gridViewMessage.OptionsView.ShowGroupPanel = false;
            this.gridViewMessage.OptionsView.ShowIndicator = false;
            this.gridViewMessage.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewMessage_CustomDrawCell);
            // 
            // gc_typeid
            // 
            this.gc_typeid.Caption = "类型ID";
            this.gc_typeid.FieldName = "TYPEID";
            this.gc_typeid.Name = "gc_typeid";
            // 
            // gc_typename
            // 
            this.gc_typename.AppearanceCell.Options.UseTextOptions = true;
            this.gc_typename.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gc_typename.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gc_typename.Caption = "类型名称";
            this.gc_typename.FieldName = "TYPENAME";
            this.gc_typename.Name = "gc_typename";
            this.gc_typename.Visible = true;
            this.gc_typename.VisibleIndex = 0;
            this.gc_typename.Width = 90;
            // 
            // gc_content
            // 
            this.gc_content.Caption = "内容";
            this.gc_content.FieldName = "CONTENT";
            this.gc_content.Name = "gc_content";
            this.gc_content.Visible = true;
            this.gc_content.VisibleIndex = 1;
            this.gc_content.Width = 246;
            // 
            // pictureBoxExit
            // 
            this.pictureBoxExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxExit.BackgroundImage")));
            this.pictureBoxExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxExit.Location = new System.Drawing.Point(316, 3);
            this.pictureBoxExit.Name = "pictureBoxExit";
            this.pictureBoxExit.Size = new System.Drawing.Size(18, 18);
            this.pictureBoxExit.TabIndex = 0;
            this.pictureBoxExit.TabStop = false;
            this.pictureBoxExit.Click += new System.EventHandler(this.pictureBoxExit_Click);
            // 
            // timerMoving
            // 
            this.timerMoving.Interval = 10;
            this.timerMoving.Tick += new System.EventHandler(this.timerMoving_Tick);
            // 
            // UCMessageWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControlMessageForm);
            this.Name = "UCMessageWindow";
            this.Size = new System.Drawing.Size(340, 184);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlMessageForm)).EndInit();
            this.groupControlMessageForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxExit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControlMessageForm;
        private System.Windows.Forms.PictureBox pictureBoxExit;
        private System.Windows.Forms.Timer timerMoving;
        private DevExpress.XtraGrid.GridControl gridControlMessage;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewMessage;
        private DevExpress.XtraGrid.Columns.GridColumn gc_typeid;
        private DevExpress.XtraGrid.Columns.GridColumn gc_typename;
        private DevExpress.XtraGrid.Columns.GridColumn gc_content;
    }
}
