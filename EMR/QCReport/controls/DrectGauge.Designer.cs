namespace DrectSoft.Core.QCReport
{
    partial class Gauge
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
            this.SuspendLayout();
            // 
            // Gauge
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Name = "Gauge";
            this.Size = new System.Drawing.Size(541, 348);
            this.FontChanged += new System.EventHandler(this.Gauge_FontChanged);
            this.ForeColorChanged += new System.EventHandler(this.Gauge_ForeColorChanged);
            this.SizeChanged += new System.EventHandler(this.AquaGauge_SizeChanged);
            this.DoubleClick += new System.EventHandler(this.AquaGauge_DoubleClick);
            this.ResumeLayout(false);

        }

        #endregion




    }
}
