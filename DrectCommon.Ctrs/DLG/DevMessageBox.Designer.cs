using DevExpress.XtraEditors;
using System.Windows.Forms;
namespace DrectSoft.Common.Ctrs.DLG
{
    partial class DevMessageBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevMessageBox));
            this.panelIconInfo = new DevExpress.XtraEditors.PanelControl();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.labelMessage = new System.Windows.Forms.Label();
            this.panelButtonInfo = new DevExpress.XtraEditors.PanelControl();
            this.panelMessageInfo = new DevExpress.XtraEditors.PanelControl();
            this.groupControlContainer = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelIconInfo)).BeginInit();
            this.panelIconInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelButtonInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMessageInfo)).BeginInit();
            this.panelMessageInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlContainer)).BeginInit();
            this.groupControlContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelIconInfo
            // 
            this.panelIconInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelIconInfo.Controls.Add(this.pictureBoxIcon);
            this.panelIconInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIconInfo.Location = new System.Drawing.Point(2, 21);
            this.panelIconInfo.Margin = new System.Windows.Forms.Padding(4);
            this.panelIconInfo.Name = "panelIconInfo";
            this.panelIconInfo.Size = new System.Drawing.Size(95, 85);
            this.panelIconInfo.TabIndex = 8;
            this.panelIconInfo.Visible = false;
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.Location = new System.Drawing.Point(42, 13);
            this.pictureBoxIcon.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(43, 47);
            this.pictureBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxIcon.TabIndex = 0;
            this.pictureBoxIcon.TabStop = false;
            this.pictureBoxIcon.Click += new System.EventHandler(this.pictureBoxIcon_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.Location = new System.Drawing.Point(0, 0);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(313, 85);
            this.labelMessage.TabIndex = 7;
            this.labelMessage.Text = "LabelText";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelButtonInfo
            // 
            this.panelButtonInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelButtonInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtonInfo.Location = new System.Drawing.Point(2, 106);
            this.panelButtonInfo.Margin = new System.Windows.Forms.Padding(4);
            this.panelButtonInfo.Name = "panelButtonInfo";
            this.panelButtonInfo.Size = new System.Drawing.Size(408, 56);
            this.panelButtonInfo.TabIndex = 6;
            // 
            // panelMessageInfo
            // 
            this.panelMessageInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelMessageInfo.Controls.Add(this.labelMessage);
            this.panelMessageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMessageInfo.Location = new System.Drawing.Point(97, 21);
            this.panelMessageInfo.Name = "panelMessageInfo";
            this.panelMessageInfo.Size = new System.Drawing.Size(313, 85);
            this.panelMessageInfo.TabIndex = 9;
            // 
            // groupControlContainer
            // 
            this.groupControlContainer.AppearanceCaption.Font = new System.Drawing.Font("宋体", 10F);
            this.groupControlContainer.AppearanceCaption.Options.UseFont = true;
            this.groupControlContainer.Controls.Add(this.panelMessageInfo);
            this.groupControlContainer.Controls.Add(this.panelIconInfo);
            this.groupControlContainer.Controls.Add(this.panelButtonInfo);
            this.groupControlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlContainer.Location = new System.Drawing.Point(0, 0);
            this.groupControlContainer.Name = "groupControlContainer";
            this.groupControlContainer.Size = new System.Drawing.Size(412, 164);
            this.groupControlContainer.TabIndex = 0;
            this.groupControlContainer.Text = this.Text;
            this.groupControlContainer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.groupControlContainer_MouseDown);
            // 
            // DevMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 164);
            this.Controls.Add(this.groupControlContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevMessageBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "提示";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DevMessageBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelIconInfo)).EndInit();
            this.panelIconInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelButtonInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelMessageInfo)).EndInit();
            this.panelMessageInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlContainer)).EndInit();
            this.groupControlContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelIconInfo;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.Label labelMessage;
        private DevExpress.XtraEditors.PanelControl panelButtonInfo;
        private DevExpress.XtraEditors.PanelControl panelMessageInfo;
        private GroupControl groupControlContainer;

    }
}