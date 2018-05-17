namespace DrectSoft.Core.QCReport
{
    partial class Reportboard
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reportboard));
            this.panelControlSearch = new DevExpress.XtraEditors.PanelControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelControlCommand = new DevExpress.XtraEditors.PanelControl();
            this.DevButtonReset1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonReset(this.components);
            this.DevButtonImportExcel1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonImportExcel(this.components);
            this.DevButtonPrint1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonPrint(this.components);
            this.DevButtonQurey1 = new DrectSoft.Common.Ctrs.OTHER.DevButtonQurey(this.components);
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lblCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlSearch)).BeginInit();
            this.panelControlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCommand)).BeginInit();
            this.panelControlCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControlSearch
            // 
            this.panelControlSearch.Controls.Add(this.flowLayoutPanel1);
            this.panelControlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlSearch.Location = new System.Drawing.Point(0, 0);
            this.panelControlSearch.Name = "panelControlSearch";
            this.panelControlSearch.Size = new System.Drawing.Size(857, 45);
            this.panelControlSearch.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(853, 41);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // panelControlCommand
            // 
            this.panelControlCommand.Appearance.BackColor = System.Drawing.Color.White;
            this.panelControlCommand.Appearance.Options.UseBackColor = true;
            this.panelControlCommand.Controls.Add(this.DevButtonReset1);
            this.panelControlCommand.Controls.Add(this.DevButtonImportExcel1);
            this.panelControlCommand.Controls.Add(this.DevButtonPrint1);
            this.panelControlCommand.Controls.Add(this.DevButtonQurey1);
            this.panelControlCommand.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlCommand.Location = new System.Drawing.Point(0, 45);
            this.panelControlCommand.Name = "panelControlCommand";
            this.panelControlCommand.Size = new System.Drawing.Size(857, 33);
            this.panelControlCommand.TabIndex = 1;
            // 
            // DevButtonReset1
            // 
            this.DevButtonReset1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonReset1.Image")));
            this.DevButtonReset1.Location = new System.Drawing.Point(263, 4);
            this.DevButtonReset1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.DevButtonReset1.Name = "DevButtonReset1";
            this.DevButtonReset1.Size = new System.Drawing.Size(70, 25);
            this.DevButtonReset1.TabIndex = 3;
            this.DevButtonReset1.Text = "重置(&B)";
            this.DevButtonReset1.Click += new System.EventHandler(this.DevButtonReset1_Click);
            // 
            // DevButtonImportExcel1
            // 
            this.DevButtonImportExcel1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonImportExcel1.Image")));
            this.DevButtonImportExcel1.ImageSelect = DrectSoft.Common.Ctrs.OTHER.DevButtonImportExcel.ImageTypes.ImportExcel;
            this.DevButtonImportExcel1.Location = new System.Drawing.Point(180, 4);
            this.DevButtonImportExcel1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.DevButtonImportExcel1.Name = "DevButtonImportExcel1";
            this.DevButtonImportExcel1.Size = new System.Drawing.Size(70, 25);
            this.DevButtonImportExcel1.TabIndex = 2;
            this.DevButtonImportExcel1.Text = "导出(&I)";
            this.DevButtonImportExcel1.Click += new System.EventHandler(this.DevButtonImportExcel1_Click);
            // 
            // DevButtonPrint1
            // 
            this.DevButtonPrint1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonPrint1.Image")));
            this.DevButtonPrint1.Location = new System.Drawing.Point(97, 4);
            this.DevButtonPrint1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.DevButtonPrint1.Name = "DevButtonPrint1";
            this.DevButtonPrint1.Size = new System.Drawing.Size(70, 25);
            this.DevButtonPrint1.TabIndex = 1;
            this.DevButtonPrint1.Text = "打印(&P)";
            this.DevButtonPrint1.Click += new System.EventHandler(this.DevButtonPrint1_Click);
            // 
            // DevButtonQurey1
            // 
            this.DevButtonQurey1.Image = ((System.Drawing.Image)(resources.GetObject("DevButtonQurey1.Image")));
            this.DevButtonQurey1.Location = new System.Drawing.Point(12, 4);
            this.DevButtonQurey1.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.DevButtonQurey1.Name = "DevButtonQurey1";
            this.DevButtonQurey1.Size = new System.Drawing.Size(70, 25);
            this.DevButtonQurey1.TabIndex = 0;
            this.DevButtonQurey1.Text = "查询(&Q)";
            this.DevButtonQurey1.Click += new System.EventHandler(this.DevButtonQurey1_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 78);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(857, 370);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.IndicatorWidth = 30;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            this.gridView1.OptionsBehavior.ReadOnly = true;
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.RowAutoHeight = true;
            this.gridView1.OptionsView.ShowDetailButtons = false;
            this.gridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridView1.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblCount);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 448);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(857, 33);
            this.panelControl1.TabIndex = 3;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCount.Location = new System.Drawing.Point(23, 9);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 12);
            this.lblCount.TabIndex = 0;
            // 
            // Reportboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControlCommand);
            this.Controls.Add(this.panelControlSearch);
            this.Name = "Reportboard";
            this.Size = new System.Drawing.Size(857, 481);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlSearch)).EndInit();
            this.panelControlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlCommand)).EndInit();
            this.panelControlCommand.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlSearch;
        private DevExpress.XtraEditors.PanelControl panelControlCommand;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonImportExcel DevButtonImportExcel1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonPrint DevButtonPrint1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonQurey DevButtonQurey1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonReset DevButtonReset1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.Label lblCount;
    }
}
