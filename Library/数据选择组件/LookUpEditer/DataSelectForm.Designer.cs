namespace DrectSoft.Common.Library
{
   partial class DataSelectForm
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

      #region Windows 窗体设计器生成的代码

      /// <summary>
      /// 设计器支持所需的方法 - 不要
      /// 使用代码编辑器修改此方法的内容。
      /// </summary>
      private void InitializeComponent()
      {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSelectForm));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.panelTop = new DevExpress.XtraEditors.PanelControl();
            this.textInputor = new DevExpress.XtraEditors.ButtonEdit();
            this.panelSetting = new DevExpress.XtraEditors.PanelControl();
            this.ckEditBegin = new DevExpress.XtraEditors.CheckEdit();
            this.ckEditAny = new DevExpress.XtraEditors.CheckEdit();
            this.ckEditFull = new DevExpress.XtraEditors.CheckEdit();
            this.ckEditDynamic = new DevExpress.XtraEditors.CheckEdit();
            this.panelData = new System.Windows.Forms.Panel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridViewbook = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelMultiSelect = new DevExpress.XtraEditors.PanelControl();
            this.lbSelectedRecords = new DevExpress.XtraEditors.ListBoxControl();
            this.panelMoveRecord = new DevExpress.XtraEditors.PanelControl();
            this.btnMoveDown = new DevExpress.XtraEditors.SimpleButton();
            this.btnMoveUp = new DevExpress.XtraEditors.SimpleButton();
            this.panelSelRecord = new DevExpress.XtraEditors.PanelControl();
            this.btnDeleteAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnDeleteOne = new DevExpress.XtraEditors.SimpleButton();
            this.btnSelectOne = new DevExpress.XtraEditors.SimpleButton();
            this.btnSelectAll = new DevExpress.XtraEditors.SimpleButton();
            this.panelBottom = new DevExpress.XtraEditors.PanelControl();
            this.panelTopShadow = new System.Windows.Forms.Panel();
            this.panelDataShadow = new System.Windows.Forms.Panel();
            this.toolTipShowList = new System.Windows.Forms.ToolTip();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textInputor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelSetting)).BeginInit();
            this.panelSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckEditBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEditAny.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEditFull.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEditDynamic.Properties)).BeginInit();
            this.panelData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewbook)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMultiSelect)).BeginInit();
            this.panelMultiSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbSelectedRecords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMoveRecord)).BeginInit();
            this.panelMoveRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelSelRecord)).BeginInit();
            this.panelSelRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(87, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(31, 24);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "√";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(194, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(31, 24);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "X";
            this.btnCancel.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // panelTop
            // 
            this.panelTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelTop.Controls.Add(this.textInputor);
            this.panelTop.Controls.Add(this.panelSetting);
            this.panelTop.Location = new System.Drawing.Point(10, 10);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(351, 26);
            this.panelTop.TabIndex = 1;
            this.panelTop.LocationChanged += new System.EventHandler(this.panelTop_LocationChanged);
            this.panelTop.SizeChanged += new System.EventHandler(this.panelTop_SizeChanged);
            // 
            // textInputor
            // 
            this.textInputor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textInputor.Location = new System.Drawing.Point(0, 0);
            this.textInputor.Name = "textInputor";
            this.textInputor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "≡", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "<<", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, false),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, ">>", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, false)});
            this.textInputor.Size = new System.Drawing.Size(98, 20);
            this.textInputor.TabIndex = 6;
            this.textInputor.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.inputBox_ButtonClick);
            this.textInputor.TextChanged += new System.EventHandler(this.inputBox_TextChanged);
            this.textInputor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputBox_KeyDown);
            this.textInputor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputBox_KeyPress);
            this.textInputor.Leave += new System.EventHandler(this.inputBox_Leave);
            // 
            // panelSetting
            // 
            this.panelSetting.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelSetting.Controls.Add(this.ckEditBegin);
            this.panelSetting.Controls.Add(this.ckEditAny);
            this.panelSetting.Controls.Add(this.ckEditFull);
            this.panelSetting.Controls.Add(this.ckEditDynamic);
            this.panelSetting.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSetting.Location = new System.Drawing.Point(98, 0);
            this.panelSetting.Name = "panelSetting";
            this.panelSetting.Size = new System.Drawing.Size(253, 26);
            this.panelSetting.TabIndex = 5;
            // 
            // ckEditBegin
            // 
            this.ckEditBegin.Dock = System.Windows.Forms.DockStyle.Right;
            this.ckEditBegin.EditValue = true;
            this.ckEditBegin.Location = new System.Drawing.Point(1, 0);
            this.ckEditBegin.Name = "ckEditBegin";
            this.ckEditBegin.Properties.Caption = "Ｖ%";
            this.ckEditBegin.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ckEditBegin.Properties.RadioGroupIndex = 0;
            this.ckEditBegin.Size = new System.Drawing.Size(52, 19);
            this.ckEditBegin.TabIndex = 4;
            this.toolTipShowList.SetToolTip(this.ckEditBegin, "前导相似\r\n(以输入的代码开头)");
            this.ckEditBegin.CheckedChanged += new System.EventHandler(this.ckEditBegin_CheckedChanged);
            // 
            // ckEditAny
            // 
            this.ckEditAny.Dock = System.Windows.Forms.DockStyle.Right;
            this.ckEditAny.Location = new System.Drawing.Point(53, 0);
            this.ckEditAny.Name = "ckEditAny";
            this.ckEditAny.Properties.Caption = "%Ｖ%";
            this.ckEditAny.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ckEditAny.Properties.RadioGroupIndex = 0;
            this.ckEditAny.Size = new System.Drawing.Size(64, 19);
            this.ckEditAny.TabIndex = 3;
            this.ckEditAny.TabStop = false;
            this.toolTipShowList.SetToolTip(this.ckEditAny, "部分包含\r\n(包含输入的代码)");
            this.ckEditAny.CheckedChanged += new System.EventHandler(this.ckEditAny_CheckedChanged);
            // 
            // ckEditFull
            // 
            this.ckEditFull.Dock = System.Windows.Forms.DockStyle.Right;
            this.ckEditFull.Location = new System.Drawing.Point(117, 0);
            this.ckEditFull.Name = "ckEditFull";
            this.ckEditFull.Properties.Caption = "VCD";
            this.ckEditFull.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.ckEditFull.Properties.RadioGroupIndex = 0;
            this.ckEditFull.Size = new System.Drawing.Size(52, 19);
            this.ckEditFull.TabIndex = 2;
            this.ckEditFull.TabStop = false;
            this.toolTipShowList.SetToolTip(this.ckEditFull, "完全匹配\r\n(与输入的代码完全一样)");
            this.ckEditFull.CheckedChanged += new System.EventHandler(this.ckEditFull_CheckedChanged);
            // 
            // ckEditDynamic
            // 
            this.ckEditDynamic.Dock = System.Windows.Forms.DockStyle.Right;
            this.ckEditDynamic.EditValue = true;
            this.ckEditDynamic.Location = new System.Drawing.Point(169, 0);
            this.ckEditDynamic.Name = "ckEditDynamic";
            this.ckEditDynamic.Properties.Caption = "智能查询";
            this.ckEditDynamic.Size = new System.Drawing.Size(84, 19);
            this.ckEditDynamic.TabIndex = 1;
            this.toolTipShowList.SetToolTip(this.ckEditDynamic, "设置是否在输入框内容改变时立即进行查询。\r\n如果“动态查询”没有被选中，则在按回车时才进行查询");
            this.ckEditDynamic.CheckedChanged += new System.EventHandler(this.ckEditDynamic_CheckedChanged);
            // 
            // panelData
            // 
            this.panelData.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelData.Controls.Add(this.gridControl1);
            this.panelData.Controls.Add(this.panelMultiSelect);
            this.panelData.Controls.Add(this.panelBottom);
            this.panelData.ForeColor = System.Drawing.Color.Silver;
            this.panelData.Location = new System.Drawing.Point(0, 55);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(554, 283);
            this.panelData.TabIndex = 4;
            this.panelData.LocationChanged += new System.EventHandler(this.panelData_LocationChanged);
            this.panelData.SizeChanged += new System.EventHandler(this.panelData_SizeChanged);
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridViewbook;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(324, 253);
            this.gridControl1.TabIndex = 12;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewbook});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
            // 
            // gridViewbook
            // 
            this.gridViewbook.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridViewbook.GridControl = this.gridControl1;
            this.gridViewbook.Name = "gridViewbook";
            this.gridViewbook.OptionsBehavior.Editable = false;
            this.gridViewbook.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewbook.OptionsCustomization.AllowFilter = false;
            this.gridViewbook.OptionsCustomization.AllowGroup = false;
            this.gridViewbook.OptionsDetail.EnableMasterViewMode = false;
            this.gridViewbook.OptionsMenu.EnableColumnMenu = false;
            this.gridViewbook.OptionsMenu.EnableFooterMenu = false;
            this.gridViewbook.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewbook.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewbook.OptionsView.ColumnAutoWidth = false;
            this.gridViewbook.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewbook.OptionsView.ShowGroupPanel = false;
            this.gridViewbook.OptionsView.ShowIndicator = false;
            this.gridViewbook.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewbook_SelectionChanged);
            this.gridViewbook.FocusedColumnChanged += new DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventHandler(this.gridViewbook_FocusedColumnChanged);
            // 
            // panelMultiSelect
            // 
            this.panelMultiSelect.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelMultiSelect.Controls.Add(this.lbSelectedRecords);
            this.panelMultiSelect.Controls.Add(this.panelMoveRecord);
            this.panelMultiSelect.Controls.Add(this.panelSelRecord);
            this.panelMultiSelect.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelMultiSelect.Location = new System.Drawing.Point(324, 0);
            this.panelMultiSelect.Name = "panelMultiSelect";
            this.panelMultiSelect.Size = new System.Drawing.Size(230, 253);
            this.panelMultiSelect.TabIndex = 14;
            this.panelMultiSelect.Visible = false;
            // 
            // lbSelectedRecords
            // 
            this.lbSelectedRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSelectedRecords.Location = new System.Drawing.Point(37, 0);
            this.lbSelectedRecords.Name = "lbSelectedRecords";
            this.lbSelectedRecords.Size = new System.Drawing.Size(156, 253);
            this.lbSelectedRecords.TabIndex = 3;
            // 
            // panelMoveRecord
            // 
            this.panelMoveRecord.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelMoveRecord.Controls.Add(this.btnMoveDown);
            this.panelMoveRecord.Controls.Add(this.btnMoveUp);
            this.panelMoveRecord.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelMoveRecord.Location = new System.Drawing.Point(193, 0);
            this.panelMoveRecord.Name = "panelMoveRecord";
            this.panelMoveRecord.Size = new System.Drawing.Size(37, 253);
            this.panelMoveRecord.TabIndex = 2;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.ImageIndex = 7;
            this.btnMoveDown.ImageList = this.imageCollection1;
            this.btnMoveDown.Location = new System.Drawing.Point(2, 114);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(33, 23);
            this.btnMoveDown.TabIndex = 2;
            this.btnMoveDown.Text = "∨";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.ImageIndex = 6;
            this.btnMoveUp.ImageList = this.imageCollection1;
            this.btnMoveUp.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnMoveUp.Location = new System.Drawing.Point(2, 72);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(33, 23);
            this.btnMoveUp.TabIndex = 1;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // panelSelRecord
            // 
            this.panelSelRecord.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelSelRecord.Controls.Add(this.btnDeleteAll);
            this.panelSelRecord.Controls.Add(this.btnDeleteOne);
            this.panelSelRecord.Controls.Add(this.btnSelectOne);
            this.panelSelRecord.Controls.Add(this.btnSelectAll);
            this.panelSelRecord.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSelRecord.Location = new System.Drawing.Point(0, 0);
            this.panelSelRecord.Name = "panelSelRecord";
            this.panelSelRecord.Size = new System.Drawing.Size(37, 253);
            this.panelSelRecord.TabIndex = 0;
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.ImageIndex = 8;
            this.btnDeleteAll.ImageList = this.imageCollection1;
            this.btnDeleteAll.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDeleteAll.Location = new System.Drawing.Point(8, 156);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(19, 19);
            this.btnDeleteAll.TabIndex = 3;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnDeleteOne
            // 
            this.btnDeleteOne.ImageIndex = 4;
            this.btnDeleteOne.ImageList = this.imageCollection1;
            this.btnDeleteOne.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnDeleteOne.Location = new System.Drawing.Point(8, 114);
            this.btnDeleteOne.Name = "btnDeleteOne";
            this.btnDeleteOne.Size = new System.Drawing.Size(19, 19);
            this.btnDeleteOne.TabIndex = 2;
            this.btnDeleteOne.Click += new System.EventHandler(this.btnDeleteOne_Click);
            // 
            // btnSelectOne
            // 
            this.btnSelectOne.ImageIndex = 3;
            this.btnSelectOne.ImageList = this.imageCollection1;
            this.btnSelectOne.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSelectOne.Location = new System.Drawing.Point(8, 72);
            this.btnSelectOne.Name = "btnSelectOne";
            this.btnSelectOne.Size = new System.Drawing.Size(19, 19);
            this.btnSelectOne.TabIndex = 1;
            this.btnSelectOne.Text = ">";
            this.btnSelectOne.Click += new System.EventHandler(this.btnSelectOne_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.ImageIndex = 2;
            this.btnSelectAll.ImageList = this.imageCollection1;
            this.btnSelectAll.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSelectAll.Location = new System.Drawing.Point(8, 30);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(19, 19);
            this.btnSelectAll.TabIndex = 0;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.btnCancel);
            this.panelBottom.Controls.Add(this.btnOk);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 253);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(554, 30);
            this.panelBottom.TabIndex = 2;
            this.panelBottom.SizeChanged += new System.EventHandler(this.panelBottom_SizeChanged);
            // 
            // panelTopShadow
            // 
            this.panelTopShadow.BackColor = System.Drawing.Color.Purple;
            this.panelTopShadow.Location = new System.Drawing.Point(351, 26);
            this.panelTopShadow.Name = "panelTopShadow";
            this.panelTopShadow.Size = new System.Drawing.Size(42, 22);
            this.panelTopShadow.TabIndex = 3;
            // 
            // panelDataShadow
            // 
            this.panelDataShadow.BackColor = System.Drawing.Color.Purple;
            this.panelDataShadow.Location = new System.Drawing.Point(416, 24);
            this.panelDataShadow.Name = "panelDataShadow";
            this.panelDataShadow.Size = new System.Drawing.Size(48, 23);
            this.panelDataShadow.TabIndex = 2;
            // 
            // toolTipShowList
            // 
            this.toolTipShowList.IsBalloon = true;
            // 
            // DataSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(559, 343);
            this.ControlBox = false;
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelData);
            this.Controls.Add(this.panelTopShadow);
            this.Controls.Add(this.panelDataShadow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataSelectForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainForm_FormClosed);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.Shown += new System.EventHandler(this.mainForm_Shown);
            this.FontChanged += new System.EventHandler(this.DataSelectForm_FontChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.mainForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mainForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textInputor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelSetting)).EndInit();
            this.panelSetting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ckEditBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEditAny.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEditFull.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEditDynamic.Properties)).EndInit();
            this.panelData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewbook)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMultiSelect)).EndInit();
            this.panelMultiSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lbSelectedRecords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMoveRecord)).EndInit();
            this.panelMoveRecord.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelSelRecord)).EndInit();
            this.panelSelRecord.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelBottom)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton btnOk;
      private DevExpress.XtraEditors.SimpleButton btnCancel;
      private System.Windows.Forms.Panel panelData;
      private System.Windows.Forms.Panel panelTopShadow;
      private System.Windows.Forms.Panel panelDataShadow;
      private DevExpress.XtraEditors.PanelControl panelBottom;
      private DevExpress.XtraEditors.PanelControl panelTop;
      private DevExpress.XtraGrid.GridControl gridControl1;
      private DevExpress.XtraGrid.Views.Grid.GridView gridViewbook;
      private System.Windows.Forms.ToolTip toolTipShowList;
      private DevExpress.XtraEditors.PanelControl panelMultiSelect;
      private DevExpress.XtraEditors.PanelControl panelSelRecord;
      private DevExpress.XtraEditors.SimpleButton btnDeleteAll;
      private DevExpress.XtraEditors.SimpleButton btnDeleteOne;
      private DevExpress.XtraEditors.SimpleButton btnSelectOne;
      private DevExpress.XtraEditors.SimpleButton btnSelectAll;
      private DevExpress.XtraEditors.ButtonEdit textInputor;
      private DevExpress.XtraEditors.PanelControl panelSetting;
      private DevExpress.XtraEditors.CheckEdit ckEditDynamic;
      private DevExpress.XtraEditors.CheckEdit ckEditBegin;
      private DevExpress.XtraEditors.CheckEdit ckEditAny;
      private DevExpress.XtraEditors.CheckEdit ckEditFull;
      private DevExpress.XtraEditors.ListBoxControl lbSelectedRecords;
      private DevExpress.XtraEditors.PanelControl panelMoveRecord;
      private DevExpress.XtraEditors.SimpleButton btnMoveDown;
      private DevExpress.XtraEditors.SimpleButton btnMoveUp;
      //private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
      private DevExpress.Utils.ImageCollection imageCollection1;
   }
}
