namespace DrectSoft.Emr.TemplateFactory
{
    partial class EditorPadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorPadForm));
            this.zyEditorControl1 = new DrectSoft.Library.EmrEditor.Src.Gui.ZYEditorControl();
            this.SuspendLayout();
            // 
            // zyEditorControl1
            // 
            this.zyEditorControl1.AcceptsTab = true;
            this.zyEditorControl1.AllowDrop = true;
            this.zyEditorControl1.AutoScroll = true;
            this.zyEditorControl1.AutoScrollMinSize = new System.Drawing.Size(833, 20);
            this.zyEditorControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
            this.zyEditorControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.zyEditorControl1.CaptureMouse = false;
            this.zyEditorControl1.CurrentPage = null;
            this.zyEditorControl1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.zyEditorControl1.DefaultCursor = System.Windows.Forms.Cursors.Default;
            this.zyEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zyEditorControl1.Document = null;
            this.zyEditorControl1.DrawBottomMargin = false;
            this.zyEditorControl1.DrawTopMargin = false;
            this.zyEditorControl1.EMRDoc = null;
            this.zyEditorControl1.EnableInsertMode = true;
            this.zyEditorControl1.ForceShowCaret = true;
            this.zyEditorControl1.GraphicsUnit = System.Drawing.GraphicsUnit.Document;
            this.zyEditorControl1.ImeMode = System.Windows.Forms.ImeMode.OnHalf;
            this.zyEditorControl1.InsertMode = true;
            this.zyEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.zyEditorControl1.MouseDragScroll = false;
            this.zyEditorControl1.MoveCaretWithScroll = true;
            this.zyEditorControl1.Name = "zyEditorControl1";
            this.zyEditorControl1.PageBackColor = System.Drawing.Color.White;
            this.zyEditorControl1.PageIndex = 0;
            this.zyEditorControl1.Pages = ((DrectSoft.Library.EmrEditor.Src.Print.PrintPageCollection)(resources.GetObject("zyEditorControl1.Pages")));
            this.zyEditorControl1.PageSpacing = 20;
            this.zyEditorControl1.ScrollFlag = false;
            this.zyEditorControl1.Size = new System.Drawing.Size(602, 451);
            this.zyEditorControl1.TabIndex = 0;
            this.zyEditorControl1.Text = "zyEditorControl1";
            this.zyEditorControl1.UseAbsTransformPoint = false;
            this.zyEditorControl1.ViewAutoScrollMinSize = new System.Drawing.Size(2603, 62);
            this.zyEditorControl1.ViewAutoScrollPosition = new System.Drawing.Point(0, 0);
            this.zyEditorControl1.ViewBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.zyEditorControl1.ViewMode = DrectSoft.Library.EmrEditor.Src.Gui.PageViewMode.Page;
            this.zyEditorControl1.WordWrap = false;
            this.zyEditorControl1.XZoomRate = 1F;
            this.zyEditorControl1.YZoomRate = 1F;
            // 
            // EditorPadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zyEditorControl1);
            this.ImeMode = System.Windows.Forms.ImeMode.OnHalf;
            this.Name = "EditorPadForm";
            this.Size = new System.Drawing.Size(602, 451);
            this.ResumeLayout(false);

        }

        #endregion

        public DrectSoft.Library.EmrEditor.Src.Gui.ZYEditorControl zyEditorControl1;


    }
}