using DrectSoft.FrameWork.WinForm;
namespace DrectSoft.MainFrame
{
   partial class DSMessageBox
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
          this.panelForm = new DevExpress.XtraEditors.PanelControl();
          this.panelMessageInfo = new DevExpress.XtraEditors.PanelControl();
          this.labelMessage = new System.Windows.Forms.Label();
          this.panelIconInfo = new DevExpress.XtraEditors.PanelControl();
          this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
          this.captionBarAnother1 = new DrectSoft.FrameWork.WinForm.CaptionBarAnother(this.components);
          this.panelButtonInfo = new DevExpress.XtraEditors.PanelControl();
          ((System.ComponentModel.ISupportInitialize)(this.panelForm)).BeginInit();
          this.panelForm.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.panelMessageInfo)).BeginInit();
          this.panelMessageInfo.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.panelIconInfo)).BeginInit();
          this.panelIconInfo.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.panelButtonInfo)).BeginInit();
          this.SuspendLayout();
          // 
          // panelForm
          // 
          this.panelForm.Controls.Add(this.panelMessageInfo);
          this.panelForm.Controls.Add(this.panelIconInfo);
          this.panelForm.Controls.Add(this.captionBarAnother1);
          this.panelForm.Controls.Add(this.panelButtonInfo);
          this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panelForm.Location = new System.Drawing.Point(0, 0);
          this.panelForm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
          this.panelForm.Name = "panelForm";
          this.panelForm.Size = new System.Drawing.Size(409, 178);
          this.panelForm.TabIndex = 2;
          // 
          // panelMessageInfo
          // 
          this.panelMessageInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
          this.panelMessageInfo.Controls.Add(this.labelMessage);
          this.panelMessageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panelMessageInfo.Location = new System.Drawing.Point(61, 27);
          this.panelMessageInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
          this.panelMessageInfo.Name = "panelMessageInfo";
          this.panelMessageInfo.Size = new System.Drawing.Size(346, 90);
          this.panelMessageInfo.TabIndex = 6;
          // 
          // labelMessage
          // 
          this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
          this.labelMessage.Location = new System.Drawing.Point(0, 0);
          this.labelMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
          this.labelMessage.Name = "labelMessage";
          this.labelMessage.Size = new System.Drawing.Size(346, 90);
          this.labelMessage.TabIndex = 0;
          this.labelMessage.Text = "LabelText";
          this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // panelIconInfo
          // 
          this.panelIconInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
          this.panelIconInfo.Controls.Add(this.pictureBoxIcon);
          this.panelIconInfo.Dock = System.Windows.Forms.DockStyle.Left;
          this.panelIconInfo.Location = new System.Drawing.Point(2, 27);
          this.panelIconInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
          this.panelIconInfo.Name = "panelIconInfo";
          this.panelIconInfo.Size = new System.Drawing.Size(59, 90);
          this.panelIconInfo.TabIndex = 5;
          this.panelIconInfo.Visible = false;
          // 
          // pictureBoxIcon
          // 
          this.pictureBoxIcon.Location = new System.Drawing.Point(9, 14);
          this.pictureBoxIcon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
          this.pictureBoxIcon.Name = "pictureBoxIcon";
          this.pictureBoxIcon.Size = new System.Drawing.Size(43, 45);
          this.pictureBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
          this.pictureBoxIcon.TabIndex = 0;
          this.pictureBoxIcon.TabStop = false;
          // 
          // captionBarAnother1
          // 
          this.captionBarAnother1.AllowMerge = false;
          this.captionBarAnother1.CanOverflow = false;
          this.captionBarAnother1.Caption = "ÌבÊ¾";
          this.captionBarAnother1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
          this.captionBarAnother1.Location = new System.Drawing.Point(2, 2);
          this.captionBarAnother1.MaxButtonVisible = false;
          this.captionBarAnother1.MinButtonVisible = false;
          this.captionBarAnother1.Name = "captionBarAnother1";
          this.captionBarAnother1.Size = new System.Drawing.Size(405, 25);
          this.captionBarAnother1.TabIndex = 4;
          this.captionBarAnother1.Text = "captionBarAnother1";
          // 
          // panelButtonInfo
          // 
          this.panelButtonInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
          this.panelButtonInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
          this.panelButtonInfo.Location = new System.Drawing.Point(2, 117);
          this.panelButtonInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
          this.panelButtonInfo.Name = "panelButtonInfo";
          this.panelButtonInfo.Size = new System.Drawing.Size(405, 59);
          this.panelButtonInfo.TabIndex = 0;
          // 
          // MyMessageBox
          // 
          this.Appearance.BackColor = System.Drawing.SystemColors.ControlLightLight;
          this.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.Appearance.Options.UseBackColor = true;
          this.Appearance.Options.UseFont = true;
          this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(409, 178);
          this.Controls.Add(this.panelForm);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
          this.KeyPreview = true;
          this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "MyMessageBox";
          this.ShowInTaskbar = false;
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
          this.Text = "FormMessageBox";
          this.TopMost = true;
          ((System.ComponentModel.ISupportInitialize)(this.panelForm)).EndInit();
          this.panelForm.ResumeLayout(false);
          this.panelForm.PerformLayout();
          ((System.ComponentModel.ISupportInitialize)(this.panelMessageInfo)).EndInit();
          this.panelMessageInfo.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.panelIconInfo)).EndInit();
          this.panelIconInfo.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.panelButtonInfo)).EndInit();
          this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelForm;
      private DevExpress.XtraEditors.PanelControl panelButtonInfo;
      private DevExpress.XtraEditors.PanelControl panelIconInfo;
      private System.Windows.Forms.PictureBox pictureBoxIcon;
      private CaptionBarAnother captionBarAnother1;
      private DevExpress.XtraEditors.PanelControl panelMessageInfo;
      private System.Windows.Forms.Label labelMessage;

   }
}