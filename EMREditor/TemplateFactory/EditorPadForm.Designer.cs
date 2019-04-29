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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorPadForm));
            this.zyEditorControl1 = new DrectSoft.Library.EmrEditor.Src.Gui.ZYEditorControl();
            this.popupMenuRightMouse = new DevExpress.XtraBars.PopupMenu(this.components);
            this.btn_CellBorderWidth = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuRightMouse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // zyEditorControl1
            // 
            this.zyEditorControl1.AcceptsTab = true;
            this.zyEditorControl1.AllowDrop = true;
            this.zyEditorControl1.AutoScroll = true;
            this.zyEditorControl1.AutoScrollMinSize = new System.Drawing.Size(798, 20);
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
            this.zyEditorControl1.ImeMode = System.Windows.Forms.ImeMode.On;
            this.zyEditorControl1.InsertMode = true;
            this.zyEditorControl1.IsSingleClickAsDoubleClick = false;
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
            this.zyEditorControl1.ViewAutoScrollMinSize = new System.Drawing.Size(2493, 62);
            this.zyEditorControl1.ViewAutoScrollPosition = new System.Drawing.Point(0, 0);
            this.zyEditorControl1.ViewBounds = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.zyEditorControl1.ViewMode = DrectSoft.Library.EmrEditor.Src.Gui.PageViewMode.Page;
            this.zyEditorControl1.WordWrap = false;
            this.zyEditorControl1.XZoomRate = 1F;
            this.zyEditorControl1.YZoomRate = 1F;
            // 
            // popupMenuRightMouse
            // 
            this.popupMenuRightMouse.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btn_CellBorderWidth)});
            this.popupMenuRightMouse.Manager = this.barManager1;
            this.popupMenuRightMouse.Name = "popupMenuRightMouse";
            // 
            // btn_CellBorderWidth
            // 
            this.btn_CellBorderWidth.Caption = "单元格属性";
            this.btn_CellBorderWidth.Id = 0;
            this.btn_CellBorderWidth.Name = "btn_CellBorderWidth";
            this.btn_CellBorderWidth.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btn_CellBorderWidth_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btn_CellBorderWidth});
            this.barManager1.MaxItemId = 1;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(602, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 451);
            this.barDockControlBottom.Size = new System.Drawing.Size(602, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 451);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(602, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 451);
            // 
            // EditorPadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zyEditorControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "EditorPadForm";
            this.Size = new System.Drawing.Size(602, 451);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuRightMouse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DrectSoft.Library.EmrEditor.Src.Gui.ZYEditorControl zyEditorControl1;
        private DevExpress.XtraBars.PopupMenu popupMenuRightMouse;
        private DevExpress.XtraBars.BarButtonItem btn_CellBorderWidth;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;


    }
}