namespace DrectSoft.Core.MainEmrPad
{
    partial class PadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PadForm));
            this.zyEditorControl1 = new DrectSoft.Library.EmrEditor.Src.Gui.ZYEditorControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItemSave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSubmit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemAudit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemCancelAudit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemLocate = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenuRightMouse = new DevExpress.XtraBars.PopupMenu();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuRightMouse)).BeginInit();
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
            this.zyEditorControl1.InsertMode = true;
            this.zyEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.zyEditorControl1.MouseDragScroll = true;
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
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemSave,
            this.barButtonItemDelete,
            this.barButtonItemSubmit,
            this.barButtonItemAudit,
            this.barButtonItem1,
            this.barButtonItemCancelAudit,
            this.barButtonItem2,
            this.barButtonItemLocate});
            this.barManager1.MaxItemId = 8;
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
            // barButtonItemSave
            // 
            this.barButtonItemSave.Caption = "保存病历";
            this.barButtonItemSave.Id = 0;
            this.barButtonItemSave.Name = "barButtonItemSave";
            this.barButtonItemSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSave_ItemClick);
            // 
            // barButtonItemDelete
            // 
            this.barButtonItemDelete.Caption = "删除当前病历";
            this.barButtonItemDelete.Id = 1;
            this.barButtonItemDelete.Name = "barButtonItemDelete";
            this.barButtonItemDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDelete_ItemClick);
            // 
            // barButtonItemSubmit
            // 
            this.barButtonItemSubmit.Caption = "提交至上级医师";
            this.barButtonItemSubmit.Id = 2;
            this.barButtonItemSubmit.Name = "barButtonItemSubmit";
            this.barButtonItemSubmit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSubmit_ItemClick);
            // 
            // barButtonItemAudit
            // 
            this.barButtonItemAudit.Caption = "审核当前病历";
            this.barButtonItemAudit.Id = 3;
            this.barButtonItemAudit.Name = "barButtonItemAudit";
            this.barButtonItemAudit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAudit_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 4;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItemCancelAudit
            // 
            this.barButtonItemCancelAudit.Caption = "取消审核当前病历";
            this.barButtonItemCancelAudit.Id = 5;
            this.barButtonItemCancelAudit.Name = "barButtonItemCancelAudit";
            this.barButtonItemCancelAudit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemCancelAudit_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 6;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonItemLocate
            // 
            this.barButtonItemLocate.Caption = "定位左侧导航";
            this.barButtonItemLocate.Id = 7;
            this.barButtonItemLocate.Name = "barButtonItemLocate";
            this.barButtonItemLocate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemLocate_ItemClick);
            // 
            // popupMenuRightMouse
            // 
            this.popupMenuRightMouse.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemLocate),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDelete),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSubmit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemAudit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemCancelAudit)});
            this.popupMenuRightMouse.Manager = this.barManager1;
            this.popupMenuRightMouse.Name = "popupMenuRightMouse";
            // 
            // PadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zyEditorControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "PadForm";
            this.Size = new System.Drawing.Size(602, 451);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuRightMouse)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DrectSoft.Library.EmrEditor.Src.Gui.ZYEditorControl zyEditorControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSave;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDelete;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSubmit;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAudit;
        private DevExpress.XtraBars.PopupMenu popupMenuRightMouse;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemCancelAudit;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItemLocate;


    }
}