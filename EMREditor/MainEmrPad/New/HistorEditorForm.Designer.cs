namespace DrectSoft.Core.MainEmrPad.New
{
    partial class HistoryEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.zyEditorControl1 = new DrectSoft.Library.EmrEditor.Src.Gui.ZYEditorControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.btnItemSave = new DevExpress.XtraBars.BarButtonItem();
            this.btnItemDelete = new DevExpress.XtraBars.BarButtonItem();
            this.btnItemFind = new DevExpress.XtraBars.BarButtonItem();
            this.btnItemCopy = new DevExpress.XtraBars.BarButtonItem();
            this.btnItemPaste = new DevExpress.XtraBars.BarButtonItem();
            this.btnItemclean = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.btnItemReBack = new DevExpress.XtraBars.BarButtonItem();
            this.popupMenuRightMouse = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuRightMouse)).BeginInit();
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
            this.zyEditorControl1.InsertMode = true;
            this.zyEditorControl1.IsSingleClickAsDoubleClick = false;
            this.zyEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.zyEditorControl1.MouseDragScroll = true;
            this.zyEditorControl1.MoveCaretWithScroll = true;
            this.zyEditorControl1.Name = "zyEditorControl1";
            this.zyEditorControl1.PageBackColor = System.Drawing.Color.White;
            this.zyEditorControl1.PageIndex = 0;
            this.zyEditorControl1.Pages = ((DrectSoft.Library.EmrEditor.Src.Print.PrintPageCollection)(resources.GetObject("zyEditorControl1.Pages")));
            this.zyEditorControl1.PageSpacing = 20;
            this.zyEditorControl1.ScrollFlag = false;
            this.zyEditorControl1.Size = new System.Drawing.Size(530, 392);
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
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageCollection1;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnItemSave,
            this.btnItemDelete,
            this.btnItemFind,
            this.btnItemCopy,
            this.btnItemPaste,
            this.btnItemclean,
            this.barButtonItem1});
            this.barManager1.MaxItemId = 6;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(530, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 392);
            this.barDockControlBottom.Size = new System.Drawing.Size(530, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 392);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(530, 0);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 392);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "Save_16.png");
            this.imageCollection1.Images.SetKeyName(1, "Delete_24.png");
            this.imageCollection1.Images.SetKeyName(2, "Search_16.png");
            // 
            // btnItemSave
            // 
            this.btnItemSave.Caption = "保存病历";
            this.btnItemSave.Id = 0;
            this.btnItemSave.ImageIndex = 0;
            this.btnItemSave.Name = "btnItemSave";
            this.btnItemSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnItemSave_ItemClick);
            // 
            // btnItemDelete
            // 
            this.btnItemDelete.Caption = "删除病历";
            this.btnItemDelete.Id = 1;
            this.btnItemDelete.ImageIndex = 1;
            this.btnItemDelete.Name = "btnItemDelete";
            this.btnItemDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnItemDelete_ItemClick);
            // 
            // btnItemFind
            // 
            this.btnItemFind.Caption = "查找内容";
            this.btnItemFind.Id = 3;
            this.btnItemFind.ImageIndex = 2;
            this.btnItemFind.Name = "btnItemFind";
            this.btnItemFind.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnItemFind_ItemClick);
            // 
            // btnItemCopy
            // 
            this.btnItemCopy.Caption = "复制";
            this.btnItemCopy.Id = 4;
            this.btnItemCopy.Name = "btnItemCopy";
            this.btnItemCopy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnItemCopy_ItemClick);
            // 
            // btnItemPaste
            // 
            this.btnItemPaste.Caption = "粘贴";
            this.btnItemPaste.Id = 5;
            this.btnItemPaste.ImageIndex = 26;
            this.btnItemPaste.Name = "btnItemPaste";
            this.btnItemPaste.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnItemPaste_ItemClick);
            // 
            // btnItemclean
            // 
            this.btnItemclean.Caption = "清空剪切板";
            this.btnItemclean.Id = 0;
            this.btnItemclean.ImageIndex = 0;
            this.btnItemclean.Name = "btnItemclean";
            this.btnItemclean.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnItemclean_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "电子签章";
            this.barButtonItem1.Id = 5;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // btnItemReBack
            // 
            this.btnItemReBack.Caption = "申请开放";
            this.btnItemReBack.Id = 4;
            this.btnItemReBack.Name = "btnItemReBack";
            this.btnItemReBack.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnItemReBack_ItemClick);
            // 
            // popupMenuRightMouse
            // 
            this.popupMenuRightMouse.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnItemSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnItemDelete),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnItemFind, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnItemCopy, DevExpress.XtraBars.BarItemPaintStyle.Standard),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnItemPaste)});
            this.popupMenuRightMouse.Manager = this.barManager1;
            this.popupMenuRightMouse.Name = "popupMenuRightMouse";
            this.popupMenuRightMouse.BeforePopup += new System.ComponentModel.CancelEventHandler(this.popupMenuRightMouse_BeforePopup);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.zyEditorControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "EditorForm";
            this.Size = new System.Drawing.Size(530, 392);
            this.Load += new System.EventHandler(this.EditorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuRightMouse)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DrectSoft.Library.EmrEditor.Src.Gui.ZYEditorControl zyEditorControl1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.PopupMenu popupMenuRightMouse;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraBars.BarButtonItem btnItemSave;
        private DevExpress.XtraBars.BarButtonItem btnItemclean;
        private DevExpress.XtraBars.BarButtonItem btnItemDelete;
        private DevExpress.XtraBars.BarButtonItem btnItemFind;
        private DevExpress.XtraBars.BarButtonItem btnItemCopy;
        private DevExpress.XtraBars.BarButtonItem btnItemPaste;
        private DevExpress.XtraBars.BarButtonItem btnItemReBack;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
    }
}
