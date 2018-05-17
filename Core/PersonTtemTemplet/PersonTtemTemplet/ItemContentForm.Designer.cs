namespace DrectSoft.Core.PersonTtemTemplet
{
    partial class ItemContentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemContentForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.Cancel = new DrectSoft.Common.Ctrs.OTHER.DevButtonCancel(this.components);
            this.btnSave = new DrectSoft.Common.Ctrs.OTHER.DevButtonOK(this.components);
            this.itemDisplayContent1 = new DrectSoft.Core.PersonTtemTemplet.ItemDisplayContent();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.Cancel);
            this.panelControl1.Controls.Add(this.btnSave);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 538);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(533, 42);
            this.panelControl1.TabIndex = 0;
            // 
            // Cancel
            // 
            this.Cancel.Image = ((System.Drawing.Image)(resources.GetObject("Cancel.Image")));
            this.Cancel.Location = new System.Drawing.Point(425, 8);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(80, 27);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "取消(&C)";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(339, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 27);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "确定(&Y)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // itemDisplayContent1
            // 
            this.itemDisplayContent1.CatalogName = "";
            this.itemDisplayContent1.Code = null;
            this.itemDisplayContent1.Content = "";
            this.itemDisplayContent1.CreateUser = "";
            this.itemDisplayContent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemDisplayContent1.ISPserson = "";
            this.itemDisplayContent1.ItemName = "";
            this.itemDisplayContent1.Location = new System.Drawing.Point(0, 0);
            this.itemDisplayContent1.Name = "itemDisplayContent1";
            this.itemDisplayContent1.ParentID = null;
            this.itemDisplayContent1.Size = new System.Drawing.Size(533, 538);
            this.itemDisplayContent1.TabIndex = 1;
            // 
            // ItemContentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 580);
            this.Controls.Add(this.itemDisplayContent1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemContentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "子模板";
            this.Load += new System.EventHandler(this.ItemContentForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private ItemDisplayContent itemDisplayContent1;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonCancel Cancel;
        private DrectSoft.Common.Ctrs.OTHER.DevButtonOK btnSave;
    }
}