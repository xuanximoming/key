namespace DrectSoft.JobManager
{
    partial class ManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagerForm));
            this.panelControlTree = new DevExpress.XtraEditors.PanelControl();
            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControlLog = new DevExpress.XtraGrid.GridControl();
            this.gridViewLog = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSource = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColRecordCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColChangedCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColSuccess = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColMessage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColLogPath = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Excute = new DevExpress.XtraEditors.SimpleButton();
            this.btn_InitJob = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControlTools = new DevExpress.XtraEditors.PanelControl();
            this.btn_Quit = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTree)).BeginInit();
            this.panelControlTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTools)).BeginInit();
            this.panelControlTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControlTree
            // 
            this.panelControlTree.Controls.Add(this.splitterControl1);
            this.panelControlTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControlTree.Location = new System.Drawing.Point(0, 0);
            this.panelControlTree.Name = "panelControlTree";
            this.panelControlTree.Size = new System.Drawing.Size(213, 640);
            this.panelControlTree.TabIndex = 2;
            // 
            // splitterControl1
            // 
            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitterControl1.Location = new System.Drawing.Point(206, 2);
            this.splitterControl1.Name = "splitterControl1";
            this.splitterControl1.Size = new System.Drawing.Size(5, 636);
            this.splitterControl1.TabIndex = 0;
            this.splitterControl1.TabStop = false;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(213, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(658, 640);
            this.xtraTabControl1.TabIndex = 3;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gridControlLog);
            this.xtraTabPage1.Controls.Add(this.panelControl1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(652, 611);
            this.xtraTabPage1.Text = "任务执行";
            // 
            // gridControlLog
            // 
            this.gridControlLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlLog.Location = new System.Drawing.Point(0, 59);
            this.gridControlLog.MainView = this.gridViewLog;
            this.gridControlLog.Name = "gridControlLog";
            this.gridControlLog.Size = new System.Drawing.Size(652, 552);
            this.gridControlLog.TabIndex = 3;
            this.gridControlLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewLog});
            // 
            // gridViewLog
            // 
            this.gridViewLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColDate,
            this.gridColSource,
            this.gridColRecordCount,
            this.gridColChangedCount,
            this.gridColSuccess,
            this.gridColMessage,
            this.gridColLogPath});
            this.gridViewLog.GridControl = this.gridControlLog;
            this.gridViewLog.GroupPanelText = "如需按列筛选，请将指定列标题拖至此处";
            this.gridViewLog.Name = "gridViewLog";
            this.gridViewLog.OptionsBehavior.Editable = false;
            this.gridViewLog.OptionsCustomization.AllowFilter = false;
            this.gridViewLog.OptionsMenu.EnableColumnMenu = false;
            this.gridViewLog.OptionsMenu.EnableFooterMenu = false;
            this.gridViewLog.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewLog.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewLog.OptionsView.ShowGroupPanel = false;
            // 
            // gridColDate
            // 
            this.gridColDate.Caption = "日期";
            this.gridColDate.FieldName = "LOGTIME";
            this.gridColDate.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColDate.Name = "gridColDate";
            this.gridColDate.Visible = true;
            this.gridColDate.VisibleIndex = 0;
            this.gridColDate.Width = 138;
            // 
            // gridColSource
            // 
            this.gridColSource.Caption = "任务名称";
            this.gridColSource.FieldName = "JOBNAME";
            this.gridColSource.Name = "gridColSource";
            this.gridColSource.Visible = true;
            this.gridColSource.VisibleIndex = 1;
            this.gridColSource.Width = 103;
            // 
            // gridColRecordCount
            // 
            this.gridColRecordCount.Caption = "记录条数";
            this.gridColRecordCount.FieldName = "记录条数";
            this.gridColRecordCount.Name = "gridColRecordCount";
            this.gridColRecordCount.Width = 72;
            // 
            // gridColChangedCount
            // 
            this.gridColChangedCount.Caption = "改变条数";
            this.gridColChangedCount.FieldName = "改变条数";
            this.gridColChangedCount.Name = "gridColChangedCount";
            this.gridColChangedCount.Width = 72;
            // 
            // gridColSuccess
            // 
            this.gridColSuccess.Name = "gridColSuccess";
            // 
            // gridColMessage
            // 
            this.gridColMessage.Caption = "消息";
            this.gridColMessage.FieldName = "CONTENT";
            this.gridColMessage.Name = "gridColMessage";
            this.gridColMessage.Visible = true;
            this.gridColMessage.VisibleIndex = 2;
            this.gridColMessage.Width = 139;
            // 
            // gridColLogPath
            // 
            this.gridColLogPath.Caption = "备注";
            this.gridColLogPath.FieldName = "MEMO";
            this.gridColLogPath.Name = "gridColLogPath";
            this.gridColLogPath.Visible = true;
            this.gridColLogPath.VisibleIndex = 3;
            this.gridColLogPath.Width = 169;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_Excute);
            this.panelControl1.Controls.Add(this.btn_InitJob);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(652, 59);
            this.panelControl1.TabIndex = 2;
            // 
            // btn_Excute
            // 
            this.btn_Excute.Location = new System.Drawing.Point(124, 17);
            this.btn_Excute.Name = "btn_Excute";
            this.btn_Excute.Size = new System.Drawing.Size(75, 23);
            this.btn_Excute.TabIndex = 1;
            this.btn_Excute.Text = "自动同步";
            this.btn_Excute.Click += new System.EventHandler(this.btn_Excute_Click);
            // 
            // btn_InitJob
            // 
            this.btn_InitJob.Location = new System.Drawing.Point(15, 17);
            this.btn_InitJob.Name = "btn_InitJob";
            this.btn_InitJob.Size = new System.Drawing.Size(75, 23);
            this.btn_InitJob.TabIndex = 0;
            this.btn_InitJob.Text = "初始化";
            this.btn_InitJob.Click += new System.EventHandler(this.btn_InitJob_Click);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.panelControlTools);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(652, 611);
            this.xtraTabPage2.Text = "日程安排";
            // 
            // panelControlTools
            // 
            this.panelControlTools.Controls.Add(this.btn_Quit);
            this.panelControlTools.Controls.Add(this.btn_Save);
            this.panelControlTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControlTools.Location = new System.Drawing.Point(0, 574);
            this.panelControlTools.Name = "panelControlTools";
            this.panelControlTools.Size = new System.Drawing.Size(652, 37);
            this.panelControlTools.TabIndex = 1;
            // 
            // btn_Quit
            // 
            this.btn_Quit.Location = new System.Drawing.Point(516, 9);
            this.btn_Quit.Name = "btn_Quit";
            this.btn_Quit.Size = new System.Drawing.Size(75, 23);
            this.btn_Quit.TabIndex = 1;
            this.btn_Quit.Text = "取消";
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(425, 9);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 23);
            this.btn_Save.TabIndex = 0;
            this.btn_Save.Text = "确定";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // ManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 640);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panelControlTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动任务批处理器";
            this.Load += new System.EventHandler(this.ManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTree)).EndInit();
            this.panelControlTree.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTools)).EndInit();
            this.panelControlTools.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlTree;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl gridControlLog;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewLog;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SplitterControl splitterControl1;

        
        
        private DevExpress.XtraGrid.Columns.GridColumn gridColDate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSource;
        private DevExpress.XtraGrid.Columns.GridColumn gridColRecordCount;
        private DevExpress.XtraGrid.Columns.GridColumn gridColChangedCount;
        private DevExpress.XtraGrid.Columns.GridColumn gridColSuccess;
        private DevExpress.XtraGrid.Columns.GridColumn gridColMessage;
        private DevExpress.XtraGrid.Columns.GridColumn gridColLogPath;
        private DevExpress.XtraEditors.SimpleButton btn_Excute;
        private DevExpress.XtraEditors.SimpleButton btn_InitJob;
        private DevExpress.XtraEditors.PanelControl panelControlTools;
        private DevExpress.XtraEditors.SimpleButton btn_Quit;
        private DevExpress.XtraEditors.SimpleButton btn_Save;
    }
}

