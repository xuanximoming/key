namespace DrectSoft.Basic.Paint.NET
{
    partial class PaintDesignPanel
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
            DrectSoft.Basic.Paint.NET.ShapeSource shapeSource1 = new DrectSoft.Basic.Paint.NET.ShapeSource();
            this.controlSurface1 = new DrectSoft.Basic.Paint.NET.ControlSurface();
            this.brushStrip1 = new DrectSoft.Basic.Paint.NET.BrushStrip();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.shapeStrip1 = new DrectSoft.Basic.Paint.NET.ShapeStrip();
            this.lineWidthStrip1 = new DrectSoft.Basic.Paint.NET.LineWidthStrip();
            this.colorStrip1 = new DrectSoft.Basic.Paint.NET.ColorStrip();
            this.newPictureStrip1 = new DrectSoft.Basic.Paint.NET.NewPictureStrip();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.LeftToolStripPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlSurface1
            // 
            this.controlSurface1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.controlSurface1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlSurface1.Location = new System.Drawing.Point(0, 0);
            this.controlSurface1.Name = "controlSurface1";
            this.controlSurface1.ReadOnly = false;
            this.controlSurface1.Size = new System.Drawing.Size(445, 441);
            shapeSource1.Dirty = false;
            shapeSource1.Surface = this.controlSurface1;
            shapeSource1.Tag = null;
            this.controlSurface1.Source = shapeSource1;
            this.controlSurface1.SurfaceDrawColor = System.Drawing.Color.Black;
            this.controlSurface1.SurfaceFillColor = System.Drawing.Color.Black;
            this.controlSurface1.SurfaceHatch = System.Drawing.Drawing2D.HatchStyle.Percent50;
            this.controlSurface1.SurfaceIsFillColor = false;
            this.controlSurface1.SurfaceIsHatch = false;
            this.controlSurface1.SurfaceIsTexture = false;
            this.controlSurface1.SurfaceLineWidth = 1F;
            this.controlSurface1.SurfaceTexture = null;
            this.controlSurface1.TabIndex = 3;
            // 
            // brushStrip1
            // 
            this.brushStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.brushStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.brushStrip1.Location = new System.Drawing.Point(3, 25);
            this.brushStrip1.Name = "brushStrip1";
            this.brushStrip1.Size = new System.Drawing.Size(107, 25);
            this.brushStrip1.Surface = this.controlSurface1;
            this.brushStrip1.TabIndex = 2;
            this.brushStrip1.Text = "brushStrip1";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(260, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(356, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.controlSurface1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(445, 441);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add(this.shapeStrip1);
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add(this.lineWidthStrip1);
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add(this.colorStrip1);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(495, 491);
            this.toolStripContainer1.TabIndex = 6;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.newPictureStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.brushStrip1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 404);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 37);
            this.panel1.TabIndex = 6;
            // 
            // shapeStrip1
            // 
            this.shapeStrip1.AutoSize = false;
            this.shapeStrip1.BackColor = System.Drawing.Color.White;
            this.shapeStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.shapeStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.shapeStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.shapeStrip1.Location = new System.Drawing.Point(0, 3);
            this.shapeStrip1.Name = "shapeStrip1";
            this.shapeStrip1.Size = new System.Drawing.Size(50, 98);
            this.shapeStrip1.Surface = this.controlSurface1;
            this.shapeStrip1.TabIndex = 0;
            // 
            // lineWidthStrip1
            // 
            this.lineWidthStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.lineWidthStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.lineWidthStrip1.ImageScalingSize = new System.Drawing.Size(45, 12);
            this.lineWidthStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.lineWidthStrip1.Location = new System.Drawing.Point(0, 101);
            this.lineWidthStrip1.Name = "lineWidthStrip1";
            this.lineWidthStrip1.Size = new System.Drawing.Size(50, 97);
            this.lineWidthStrip1.Surface = this.controlSurface1;
            this.lineWidthStrip1.TabIndex = 1;
            // 
            // colorStrip1
            // 
            this.colorStrip1.AutoSize = false;
            this.colorStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.colorStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.colorStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.colorStrip1.Location = new System.Drawing.Point(0, 198);
            this.colorStrip1.Name = "colorStrip1";
            this.colorStrip1.Size = new System.Drawing.Size(50, 50);
            this.colorStrip1.Surface = this.controlSurface1;
            this.colorStrip1.TabIndex = 2;
            // 
            // newPictureStrip1
            // 
            this.newPictureStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.newPictureStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.newPictureStrip1.Location = new System.Drawing.Point(3, 0);
            this.newPictureStrip1.Name = "newPictureStrip1";
            this.newPictureStrip1.Size = new System.Drawing.Size(241, 25);
            this.newPictureStrip1.Surface = this.controlSurface1;
            this.newPictureStrip1.TabIndex = 3;
            this.newPictureStrip1.FontSizeChanged += new System.EventHandler<DrectSoft.Basic.Paint.NET.SizeEventArgs>(this.newPictureStrip1_FontSizeChanged);
            // 
            // PaintDesignPanel
            // 
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "PaintDesignPanel";
            this.Size = new System.Drawing.Size(495, 491);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.LeftToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.LeftToolStripPanel.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ControlSurface controlSurface1;
        private BrushStrip brushStrip1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private NewPictureStrip newPictureStrip1;
        private ShapeStrip shapeStrip1;
        private DrectSoft.Basic.Paint.NET.LineWidthStrip lineWidthStrip1;
        private DrectSoft.Basic.Paint.NET.ColorStrip colorStrip1;
        private System.Windows.Forms.Panel panel1;
        
    }
}
