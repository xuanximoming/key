namespace DrectSoft.Core.TimeLimitQC
{
    partial class UserCtrlTimeQcInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserCtrlTimeQcInfo));
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButtonRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonZhanKai = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButtonShouSuo = new DevExpress.XtraEditors.SimpleButton();
            this.gridControlTimeLimitInfo = new DevExpress.XtraGrid.GridControl();
            this.advBandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTimeLimitInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.simpleButtonRefresh);
            this.panelControl3.Controls.Add(this.simpleButtonZhanKai);
            this.panelControl3.Controls.Add(this.simpleButtonShouSuo);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(260, 31);
            this.panelControl3.TabIndex = 0;
            // 
            // simpleButtonRefresh
            // 
            this.simpleButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonRefresh.Image")));
            this.simpleButtonRefresh.Location = new System.Drawing.Point(176, 4);
            this.simpleButtonRefresh.Name = "simpleButtonRefresh";
            this.simpleButtonRefresh.Size = new System.Drawing.Size(80, 23);
            this.simpleButtonRefresh.TabIndex = 2;
            this.simpleButtonRefresh.Text = "刷新 (&R)";
            this.simpleButtonRefresh.Click += new System.EventHandler(this.simpleButtonRefresh_Click);
            // 
            // simpleButtonZhanKai
            // 
            this.simpleButtonZhanKai.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonZhanKai.Image")));
            this.simpleButtonZhanKai.Location = new System.Drawing.Point(90, 4);
            this.simpleButtonZhanKai.Name = "simpleButtonZhanKai";
            this.simpleButtonZhanKai.Size = new System.Drawing.Size(80, 23);
            this.simpleButtonZhanKai.TabIndex = 1;
            this.simpleButtonZhanKai.Text = "展开 (&U)";
            this.simpleButtonZhanKai.Click += new System.EventHandler(this.simpleButtonZhanKai_Click);
            // 
            // simpleButtonShouSuo
            // 
            this.simpleButtonShouSuo.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonShouSuo.Image")));
            this.simpleButtonShouSuo.Location = new System.Drawing.Point(4, 4);
            this.simpleButtonShouSuo.Name = "simpleButtonShouSuo";
            this.simpleButtonShouSuo.Size = new System.Drawing.Size(80, 23);
            this.simpleButtonShouSuo.TabIndex = 0;
            this.simpleButtonShouSuo.Text = "收缩 (&V)";
            this.simpleButtonShouSuo.Click += new System.EventHandler(this.simpleButtonShouSuo_Click);
            // 
            // gridControlTimeLimitInfo
            // 
            this.gridControlTimeLimitInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlTimeLimitInfo.Location = new System.Drawing.Point(0, 31);
            this.gridControlTimeLimitInfo.MainView = this.advBandedGridView1;
            this.gridControlTimeLimitInfo.Name = "gridControlTimeLimitInfo";
            this.gridControlTimeLimitInfo.Size = new System.Drawing.Size(260, 431);
            this.gridControlTimeLimitInfo.TabIndex = 1;
            this.gridControlTimeLimitInfo.TabStop = false;
            this.gridControlTimeLimitInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.advBandedGridView1});
            // 
            // advBandedGridView1
            // 
            this.advBandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand2,
            this.gridBand1});
            this.advBandedGridView1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.advBandedGridView1.GridControl = this.gridControlTimeLimitInfo;
            this.advBandedGridView1.GroupFormat = "{0}:{1}";
            this.advBandedGridView1.GroupPanelText = "拖放指定列分组";
            this.advBandedGridView1.Name = "advBandedGridView1";
            this.advBandedGridView1.OptionsBehavior.AutoExpandAllGroups = true;
            this.advBandedGridView1.OptionsBehavior.Editable = false;
            this.advBandedGridView1.OptionsCustomization.AllowColumnMoving = false;
            this.advBandedGridView1.OptionsCustomization.AllowFilter = false;
            this.advBandedGridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.advBandedGridView1.OptionsFilter.AllowMRUFilterList = false;
            this.advBandedGridView1.OptionsMenu.EnableColumnMenu = false;
            this.advBandedGridView1.OptionsMenu.EnableFooterMenu = false;
            this.advBandedGridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.advBandedGridView1.OptionsView.ColumnAutoWidth = true;
            this.advBandedGridView1.OptionsView.ShowBands = false;
            this.advBandedGridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridBand2
            // 
            this.gridBand2.Caption = "gridBand2";
            this.gridBand2.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.OptionsBand.FixedWidth = true;
            this.gridBand2.Width = 12;
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.Width = 75;
            // 
            // UserCtrlTimeQcInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.gridControlTimeLimitInfo);
            this.Controls.Add(this.panelControl3);
            this.Name = "UserCtrlTimeQcInfo";
            this.Size = new System.Drawing.Size(260, 462);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTimeLimitInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButtonZhanKai;
        private DevExpress.XtraEditors.SimpleButton simpleButtonShouSuo;
        private DevExpress.XtraGrid.GridControl gridControlTimeLimitInfo;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView advBandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonRefresh;

    }
}
