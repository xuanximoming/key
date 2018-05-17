namespace DrectSoft.Core.QCDeptReport
{
    partial class RepDeductPointPie
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
            DevExpress.XtraCharts.SimpleDiagram simpleDiagram1 = new DevExpress.XtraCharts.SimpleDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PieSeriesLabel pieSeriesLabel1 = new DevExpress.XtraCharts.PieSeriesLabel();
            DevExpress.XtraCharts.PiePointOptions piePointOptions1 = new DevExpress.XtraCharts.PiePointOptions();
            DevExpress.XtraCharts.PieSeriesView pieSeriesView1 = new DevExpress.XtraCharts.PieSeriesView();
            DevExpress.XtraCharts.PieSeriesLabel pieSeriesLabel2 = new DevExpress.XtraCharts.PieSeriesLabel();
            DevExpress.XtraCharts.PiePointOptions piePointOptions2 = new DevExpress.XtraCharts.PiePointOptions();
            DevExpress.XtraCharts.PieSeriesView pieSeriesView2 = new DevExpress.XtraCharts.PieSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepDeductPointPie));
            this.PieChart = new DevExpress.XtraCharts.ChartControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.PieChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(simpleDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PieChart
            // 
            this.PieChart.AppearanceNameSerializable = "In A Fog";
            simpleDiagram1.EqualPieSize = false;
            this.PieChart.Diagram = simpleDiagram1;
            this.PieChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PieChart.Legend.AlignmentHorizontal = DevExpress.XtraCharts.LegendAlignmentHorizontal.LeftOutside;
            this.PieChart.Legend.Border.Color = System.Drawing.Color.Gray;
            this.PieChart.Legend.EquallySpacedItems = false;
            this.PieChart.Location = new System.Drawing.Point(2, 2);
            this.PieChart.Name = "PieChart";
            this.PieChart.PaletteName = "Chameleon";
            pieSeriesLabel1.LineLength = 5;
            piePointOptions1.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
            pieSeriesLabel1.PointOptions = piePointOptions1;
            series1.Label = pieSeriesLabel1;
            series1.Name = "Pieview";
            pieSeriesView1.RuntimeExploding = false;
            series1.View = pieSeriesView1;
            this.PieChart.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.PieChart.SeriesTemplate.ArgumentDataMember = "DeductPointResult";
            piePointOptions2.ValueNumericOptions.Format = DevExpress.XtraCharts.NumericFormat.General;
            pieSeriesLabel2.PointOptions = piePointOptions2;
            this.PieChart.SeriesTemplate.Label = pieSeriesLabel2;
            this.PieChart.SeriesTemplate.SummaryFunction = "COUNT()";
            pieSeriesView2.RuntimeExploding = false;
            this.PieChart.SeriesTemplate.View = pieSeriesView2;
            this.PieChart.Size = new System.Drawing.Size(1366, 746);
            this.PieChart.TabIndex = 0;
            chartTitle1.Indent = 6;
            chartTitle1.Text = "科室病历失分统计";
            chartTitle1.TextColor = System.Drawing.Color.Teal;
            this.PieChart.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // panelControl1
            // 
            this.panelControl1.AutoSize = true;
            this.panelControl1.Controls.Add(this.PieChart);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1370, 750);
            this.panelControl1.TabIndex = 0;
            // 
            // RepDeductPointPie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RepDeductPointPie";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "科室病历失分统计";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.RepDeductPointPie_Load);
            ((System.ComponentModel.ISupportInitialize)(simpleDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pieSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PieChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl PieChart;

        private DevExpress.XtraEditors.PanelControl panelControl1;

    }
}