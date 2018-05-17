namespace DrectSoft.Core.QCDeptReport
{
    partial class DiagramForm
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.PointSeriesLabel pointSeriesLabel1 = new DevExpress.XtraCharts.PointSeriesLabel();
            DevExpress.XtraCharts.PointOptions pointOptions1 = new DevExpress.XtraCharts.PointOptions();
            DevExpress.XtraCharts.SplineSeriesView splineSeriesView1 = new DevExpress.XtraCharts.SplineSeriesView();
            DevExpress.XtraCharts.SplineSeriesView splineSeriesView2 = new DevExpress.XtraCharts.SplineSeriesView();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkedComboBoxEdit1 = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.lookUpEdit1 = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.checkEditStepLine = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditSpline = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditLine = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditPie = new DevExpress.XtraEditors.CheckEdit();
            this.checkEditBar = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditStepLine.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSpline.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditLine.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPie.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditBar.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkedComboBoxEdit1);
            this.panel1.Controls.Add(this.lookUpEdit1);
            this.panel1.Controls.Add(this.labelControl4);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.checkEditStepLine);
            this.panel1.Controls.Add(this.checkEditSpline);
            this.panel1.Controls.Add(this.checkEditLine);
            this.panel1.Controls.Add(this.checkEditPie);
            this.panel1.Controls.Add(this.checkEditBar);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1079, 39);
            this.panel1.TabIndex = 0;
            // 
            // checkedComboBoxEdit1
            // 
            this.checkedComboBoxEdit1.Location = new System.Drawing.Point(578, 8);
            this.checkedComboBoxEdit1.Name = "checkedComboBoxEdit1";
            this.checkedComboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.checkedComboBoxEdit1.Properties.ContextMenuStrip = this.contextMenuStrip;
            this.checkedComboBoxEdit1.Size = new System.Drawing.Size(323, 20);
            this.checkedComboBoxEdit1.TabIndex = 15;
            this.checkedComboBoxEdit1.EditValueChanged += new System.EventHandler(this.checkedComboBoxEdit1_EditValueChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // lookUpEdit1
            // 
            this.lookUpEdit1.Location = new System.Drawing.Point(578, 8);
            this.lookUpEdit1.Name = "lookUpEdit1";
            this.lookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit1.Size = new System.Drawing.Size(160, 20);
            this.lookUpEdit1.TabIndex = 14;
            this.lookUpEdit1.Visible = false;
            this.lookUpEdit1.EditValueChanged += new System.EventHandler(this.lookUpEdit1_EditValueChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(512, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 13;
            this.labelControl4.Text = "图标内容：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(79, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(15, 14);
            this.labelControl2.TabIndex = 11;
            this.labelControl2.Text = "2D";
            // 
            // checkEditStepLine
            // 
            this.checkEditStepLine.Location = new System.Drawing.Point(331, 10);
            this.checkEditStepLine.Name = "checkEditStepLine";
            this.checkEditStepLine.Properties.Caption = "梯线图";
            this.checkEditStepLine.Properties.RadioGroupIndex = 0;
            this.checkEditStepLine.Size = new System.Drawing.Size(60, 19);
            this.checkEditStepLine.TabIndex = 9;
            this.checkEditStepLine.TabStop = false;
            this.checkEditStepLine.Tag = "11";
            this.checkEditStepLine.CheckedChanged += new System.EventHandler(this.checkEditStepLine_CheckedChanged);
            // 
            // checkEditSpline
            // 
            this.checkEditSpline.Location = new System.Drawing.Point(412, 10);
            this.checkEditSpline.Name = "checkEditSpline";
            this.checkEditSpline.Properties.Caption = "曲线图";
            this.checkEditSpline.Properties.RadioGroupIndex = 0;
            this.checkEditSpline.Size = new System.Drawing.Size(58, 19);
            this.checkEditSpline.TabIndex = 7;
            this.checkEditSpline.TabStop = false;
            this.checkEditSpline.Tag = "12";
            this.checkEditSpline.CheckedChanged += new System.EventHandler(this.checkEditSpline_CheckedChanged);
            // 
            // checkEditLine
            // 
            this.checkEditLine.Location = new System.Drawing.Point(250, 10);
            this.checkEditLine.Name = "checkEditLine";
            this.checkEditLine.Properties.Caption = "折线图";
            this.checkEditLine.Properties.RadioGroupIndex = 0;
            this.checkEditLine.Size = new System.Drawing.Size(63, 19);
            this.checkEditLine.TabIndex = 5;
            this.checkEditLine.TabStop = false;
            this.checkEditLine.Tag = "10";
            this.checkEditLine.CheckedChanged += new System.EventHandler(this.checkEditLine_CheckedChanged);
            // 
            // checkEditPie
            // 
            this.checkEditPie.Location = new System.Drawing.Point(181, 10);
            this.checkEditPie.Name = "checkEditPie";
            this.checkEditPie.Properties.Caption = "饼图";
            this.checkEditPie.Properties.RadioGroupIndex = 0;
            this.checkEditPie.Size = new System.Drawing.Size(75, 19);
            this.checkEditPie.TabIndex = 3;
            this.checkEditPie.TabStop = false;
            this.checkEditPie.Tag = "5";
            this.checkEditPie.CheckedChanged += new System.EventHandler(this.checkEditPie_CheckedChanged);
            // 
            // checkEditBar
            // 
            this.checkEditBar.Location = new System.Drawing.Point(100, 10);
            this.checkEditBar.Name = "checkEditBar";
            this.checkEditBar.Properties.Caption = "柱状图";
            this.checkEditBar.Properties.RadioGroupIndex = 0;
            this.checkEditBar.Size = new System.Drawing.Size(75, 19);
            this.checkEditBar.TabIndex = 1;
            this.checkEditBar.TabStop = false;
            this.checkEditBar.Tag = "0";
            this.checkEditBar.CheckedChanged += new System.EventHandler(this.checkEditBar_CheckedChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "图形样式：";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chartControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1079, 504);
            this.panel2.TabIndex = 1;
            // 
            // chartControl1
            // 
            xyDiagram1.AxisX.Label.Angle = -50;
            xyDiagram1.AxisX.Label.EnableAntialiasing = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisX.VisualRange.Auto = false;
            xyDiagram1.AxisX.VisualRange.AutoSideMargins = true;
            xyDiagram1.AxisX.VisualRange.MaxValueSerializable = "4.26666666666667";
            xyDiagram1.AxisX.VisualRange.MinValueSerializable = "-0.266666666666667";
            xyDiagram1.AxisX.WholeRange.Auto = false;
            xyDiagram1.AxisX.WholeRange.AutoSideMargins = true;
            xyDiagram1.AxisX.WholeRange.MaxValueSerializable = "4.26666666666667";
            xyDiagram1.AxisX.WholeRange.MinValueSerializable = "-0.266666666666667";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.WholeRange.AutoSideMargins = true;
            this.chartControl1.Diagram = xyDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Location = new System.Drawing.Point(0, 0);
            this.chartControl1.Name = "chartControl1";
            this.chartControl1.RuntimeSelection = true;
            pointSeriesLabel1.LineLength = 40;
            pointOptions1.PointView = DevExpress.XtraCharts.PointView.ArgumentAndValues;
            pointSeriesLabel1.PointOptions = pointOptions1;
            pointSeriesLabel1.ResolveOverlappingMode = DevExpress.XtraCharts.ResolveOverlappingMode.JustifyAllAroundPoint;
            pointSeriesLabel1.Shadow.Visible = true;
            series1.Label = pointSeriesLabel1;
            series1.Name = "Series 1";
            splineSeriesView1.LineMarkerOptions.Size = 8;
            series1.View = splineSeriesView1;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControl1.SeriesTemplate.View = splineSeriesView2;
            this.chartControl1.Size = new System.Drawing.Size(1079, 504);
            this.chartControl1.TabIndex = 1;
            this.chartControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.chartControl1_MouseDown);
            // 
            // DiagramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 543);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DiagramForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图表分析";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DiagramForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkedComboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditStepLine.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditSpline.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditLine.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditPie.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditBar.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pointSeriesLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(splineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.CheckEdit checkEditSpline;
        private DevExpress.XtraEditors.CheckEdit checkEditLine;
        private DevExpress.XtraEditors.CheckEdit checkEditPie;
        private DevExpress.XtraEditors.CheckEdit checkEditBar;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.XtraEditors.CheckEdit checkEditStepLine;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckedComboBoxEdit checkedComboBoxEdit1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;

    }
}