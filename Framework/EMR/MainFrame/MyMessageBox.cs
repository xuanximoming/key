using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Resources;

namespace DrectSoft.MainFrame
{
    public class MyMessageBox : XtraForm
    {
        private const int InitTop = 10;

        private const int InitLeft = 10;

        private const int InitGap = 5;

        private const string s_OkCaption = "确定(&O)";

        private const string s_CancelCaption = "取消(&C)";

        private const string s_YesCaption = "是(&Y)";

        private const string s_NoCaption = "否(&N)";

        private const int s_MinBoxWidth = 260;

        private const int s_MinBoxHeight = 150;

        private const int s_DefaultPad = 3;

        private int m_MaxWidth;

        private int m_MaxHeight;

        private int m_MaxLayoutWidth;

        private int m_MaxLayoutHeight;

        private int m_MinLayoutWidth;

        private int m_MinLayoutHeight;

        private IContainer components = null;

        private PanelControl panelForm;

        private PanelControl panelButtonInfo;

        private PanelControl panelIconInfo;

        private PictureBox pictureBoxIcon;

        private CaptionBarAnother captionBarAnother1;

        private PanelControl panelMessageInfo;

        private Label labelMessage;

        public MyMessageBox()
        {
            this.InitializeComponent();
            this.Text = "信息";
            base.KeyDown += new KeyEventHandler(this.FormMessageBox_KeyDown);
            this.SetContainer();
        }

        public MyMessageBox(Control parent)
            : this()
        {
            base.Parent = parent;
            this.SetContainer();
        }

        private void FormMessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                Clipboard.SetText(this.labelMessage.Text);
            }
        }

        public void SetMessageInfo(Collection<string> messages)
        {
            if (messages == null)
            {
                this.labelMessage.Text = string.Empty;
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (string current in messages)
                {
                    stringBuilder.Append(current);
                }
                this.labelMessage.Text = stringBuilder.ToString();
            }
            this.ResetBoxMaxSize();
            this.ResetMessageSizeRange();
            Font font = new Font("SimSun", 11f);
            Graphics graphics = Graphics.FromHwnd(this.panelMessageInfo.Handle);
            SizeF stringSize = graphics.MeasureString(this.labelMessage.Text, font, this.m_MaxLayoutWidth);
            this.ResetBoxSize(stringSize);
            this.labelMessage.Text = this.labelMessage.Text.Trim();
            SizeF sizeF = graphics.MeasureString("中", font);
            if (stringSize.Height <= sizeF.Height)
            {
                this.labelMessage.TextAlign = ContentAlignment.MiddleCenter;
            }
            else
            {
                this.labelMessage.TextAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void ResetBoxSize(SizeF stringSize)
        {
            int num = this.EnsureSizeInRange((int)stringSize.Height, this.m_MinLayoutHeight, this.m_MaxLayoutHeight);
            int num2 = this.EnsureSizeInRange((int)stringSize.Width + 30, this.m_MinLayoutWidth, this.m_MaxLayoutWidth);
            base.Height = num + this.captionBarAnother1.Height + this.panelButtonInfo.Height + 3;
            base.Width = num2 + this.panelIconInfo.Width + 3;
        }

        private int EnsureSizeInRange(int value, int minValue, int maxValue)
        {
            int result;
            if (value < minValue)
            {
                result = minValue;
            }
            else if (value > maxValue)
            {
                result = maxValue;
            }
            else
            {
                result = value;
            }
            return result;
        }

        private void ResetMessageSizeRange()
        {
            int num = this.captionBarAnother1.Height + this.panelButtonInfo.Height + 3;
            int num2 = this.panelIconInfo.Width + 3;
            this.m_MinLayoutHeight = 150 - num;
            this.m_MinLayoutWidth = 260 - num2;
            this.m_MaxLayoutHeight = this.m_MaxHeight - num;
            this.m_MaxLayoutWidth = this.m_MaxWidth - num2;
        }

        private void ResetBoxMaxSize()
        {
            Rectangle workingArea;
            if (base.Parent == null)
            {
                workingArea = SystemInformation.WorkingArea;
            }
            else
            {
                workingArea = Screen.GetWorkingArea(base.Parent);
            }
            this.m_MaxHeight = (int)((double)workingArea.Height * 0.9);
            this.m_MaxWidth = (int)((double)workingArea.Width * 0.6);
        }

        public void SetButtonInfo(Collection<SimpleButton> controls)
        {
            base.SuspendLayout();
            this.panelButtonInfo.Controls.Clear();
            if (controls != null)
            {
                int count = controls.Count;
                if (count >= 0)
                {
                    int num = 0;
                    int num2 = 0;
                    for (int i = 0; i < count; i++)
                    {
                        num += controls[i].Width;
                    }
                    num += (count - 1) * 5;
                    if (num < this.panelButtonInfo.Width - 20)
                    {
                        num2 = (this.panelButtonInfo.Width - 20 - num) / 2;
                    }
                    int num3 = 10 + num2;
                    for (int i = 0; i < count; i++)
                    {
                        controls[i].AutoSize = true;
                        this.panelButtonInfo.Controls.Add(controls[i]);
                        controls[i].Top = 10;
                        controls[i].Left = num3;
                        num3 = num3 + controls[i].Width + 5;
                    }
                    base.ResumeLayout();
                }
            }
        }

        public void SetButtonInfo(CustomMessageBoxKind kind)
        {
            Collection<SimpleButton> collection = new Collection<SimpleButton>();
            switch (kind)
            {
                case CustomMessageBoxKind.WarningOk:
                case CustomMessageBoxKind.ErrorOk:
                case CustomMessageBoxKind.InformationOk:
                    collection.Add(this.CreateButton("确定(&O)", DialogResult.OK));
                    break;
                case CustomMessageBoxKind.WarningOkCancel:
                case CustomMessageBoxKind.QuestionOkCancel:
                case CustomMessageBoxKind.ErrorOkCancel:
                    collection.Add(this.CreateButton("确定(&O)", DialogResult.OK));
                    collection.Add(this.CreateButton("取消(&C)", DialogResult.Cancel));
                    break;
                case CustomMessageBoxKind.QuestionYesNo:
                case CustomMessageBoxKind.ErrorYesNo:
                    collection.Add(this.CreateButton("是(&Y)", DialogResult.Yes));
                    collection.Add(this.CreateButton("否(&N)", DialogResult.No));
                    break;
                case CustomMessageBoxKind.QuestionYesNoCancel:
                case CustomMessageBoxKind.ErrorYesNoCancel:
                    collection.Add(this.CreateButton("是(&Y)", DialogResult.Yes));
                    collection.Add(this.CreateButton("否(&N)", DialogResult.No, false));
                    collection.Add(this.CreateButton("取消(&C)", DialogResult.Cancel));
                    break;
                case CustomMessageBoxKind.ErrorYes:
                    collection.Add(this.CreateButton("是(&Y)", DialogResult.Yes));
                    break;
            }
            this.SetButtonInfo(collection);
        }

        private SimpleButton CreateButton(string text, DialogResult dialogResult)
        {
            return this.CreateButton(text, dialogResult, true);
        }

        private SimpleButton CreateButton(string text, DialogResult dialogResult, bool setAcceptCancelButton)
        {
            SimpleButton simpleButton = new SimpleButton();
            simpleButton.Text = text;
            simpleButton.DialogResult = dialogResult;
            if (setAcceptCancelButton)
            {
                if (dialogResult == DialogResult.Yes || dialogResult == DialogResult.OK)
                {
                    base.AcceptButton = simpleButton;
                }
                if (dialogResult == DialogResult.No || dialogResult == DialogResult.Cancel)
                {
                    base.CancelButton = simpleButton;
                }
            }
            return simpleButton;
        }

        public void SetIconInfo(CustomMessageBoxIconType iconType)
        {
            Image image = null;
            switch (iconType)
            {
                case CustomMessageBoxIconType.WarningIcon:
                    image = ResourceManager.GetImage("MessageBoxWarning.png");
                    break;
                case CustomMessageBoxIconType.ErrorIcon:
                    image = ResourceManager.GetImage("MessageBoxError.png");
                    break;
                case CustomMessageBoxIconType.QuestionIcon:
                    image = ResourceManager.GetImage("MessageBoxPrompt.png");
                    break;
                case CustomMessageBoxIconType.InformationIcon:
                    image = ResourceManager.GetImage("MessageBoxMessage.png");
                    break;
            }
            if (image != null)
            {
                Bitmap bitmap = new Bitmap(image);
                if (bitmap != null)
                {
                    bitmap.MakeTransparent(Color.Magenta);
                    this.pictureBoxIcon.BackgroundImage = bitmap;
                    this.pictureBoxIcon.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    this.pictureBoxIcon.Image = image;
                }
            }
        }

        public void SetIconInfo(CustomMessageBoxKind kind)
        {
            CustomMessageBoxIconType iconInfo = CustomMessageBoxIconType.None;
            switch (kind)
            {
                case CustomMessageBoxKind.WarningOk:
                case CustomMessageBoxKind.WarningOkCancel:
                    iconInfo = CustomMessageBoxIconType.WarningIcon;
                    break;
                case CustomMessageBoxKind.QuestionYesNo:
                case CustomMessageBoxKind.QuestionYesNoCancel:
                case CustomMessageBoxKind.QuestionOkCancel:
                    iconInfo = CustomMessageBoxIconType.QuestionIcon;
                    break;
                case CustomMessageBoxKind.ErrorOk:
                case CustomMessageBoxKind.ErrorOkCancel:
                case CustomMessageBoxKind.ErrorYes:
                case CustomMessageBoxKind.ErrorYesNo:
                case CustomMessageBoxKind.ErrorYesNoCancel:
                    iconInfo = CustomMessageBoxIconType.ErrorIcon;
                    break;
                case CustomMessageBoxKind.InformationOk:
                    iconInfo = CustomMessageBoxIconType.InformationIcon;
                    break;
            }
            this.SetIconInfo(iconInfo);
        }

        private void SetContainer()
        {
            GroupControl groupControl = new GroupControl();
            groupControl.Location = new Point(0, 0);
            groupControl.Name = "groupControlContainer";
            groupControl.Text = "提示";
            groupControl.Dock = DockStyle.Fill;
            groupControl.MouseDown += new MouseEventHandler(this.groupControlContainer_MouseDown);
            base.Controls.Add(groupControl);
            foreach (Control value in base.Controls)
            {
                groupControl.Controls.Add(value);
            }
            this.captionBarAnother1.Visible = false;
        }

        private void groupControlContainer_MouseDown(object sender, MouseEventArgs e)
        {
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(base.FindForm().Handle, 161u, (IntPtr)2, IntPtr.Zero);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.panelForm = new PanelControl();
            this.panelMessageInfo = new PanelControl();
            this.labelMessage = new Label();
            this.panelIconInfo = new PanelControl();
            this.pictureBoxIcon = new PictureBox();
            this.captionBarAnother1 = new CaptionBarAnother(this.components);
            this.panelButtonInfo = new PanelControl();
            ((ISupportInitialize)this.panelForm).BeginInit();
            this.panelForm.SuspendLayout();
            ((ISupportInitialize)this.panelMessageInfo).BeginInit();
            this.panelMessageInfo.SuspendLayout();
            ((ISupportInitialize)this.panelIconInfo).BeginInit();
            this.panelIconInfo.SuspendLayout();
            ((ISupportInitialize)this.pictureBoxIcon).BeginInit();
            ((ISupportInitialize)this.panelButtonInfo).BeginInit();
            base.SuspendLayout();
            this.panelForm.Controls.Add(this.panelMessageInfo);
            this.panelForm.Controls.Add(this.panelIconInfo);
            this.panelForm.Controls.Add(this.captionBarAnother1);
            this.panelForm.Controls.Add(this.panelButtonInfo);
            this.panelForm.Dock = DockStyle.Fill;
            this.panelForm.Location = new Point(0, 0);
            this.panelForm.Margin = new Padding(4, 4, 4, 4);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new Size(409, 178);
            this.panelForm.TabIndex = 2;
            this.panelMessageInfo.BorderStyle = BorderStyles.NoBorder;
            this.panelMessageInfo.Controls.Add(this.labelMessage);
            this.panelMessageInfo.Dock = DockStyle.Fill;
            this.panelMessageInfo.Location = new Point(61, 27);
            this.panelMessageInfo.Margin = new Padding(4, 4, 4, 4);
            this.panelMessageInfo.Name = "panelMessageInfo";
            this.panelMessageInfo.Size = new Size(346, 90);
            this.panelMessageInfo.TabIndex = 6;
            this.labelMessage.Dock = DockStyle.Fill;
            this.labelMessage.Location = new Point(0, 0);
            this.labelMessage.Margin = new Padding(4, 0, 4, 0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new Size(346, 90);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "LabelText";
            this.labelMessage.TextAlign = ContentAlignment.MiddleLeft;
            this.panelIconInfo.BorderStyle = BorderStyles.NoBorder;
            this.panelIconInfo.Controls.Add(this.pictureBoxIcon);
            this.panelIconInfo.Dock = DockStyle.Left;
            this.panelIconInfo.Location = new Point(2, 27);
            this.panelIconInfo.Margin = new Padding(4, 4, 4, 4);
            this.panelIconInfo.Name = "panelIconInfo";
            this.panelIconInfo.Size = new Size(59, 90);
            this.panelIconInfo.TabIndex = 5;
            this.panelIconInfo.Visible = false;
            this.pictureBoxIcon.Location = new Point(9, 14);
            this.pictureBoxIcon.Margin = new Padding(4, 4, 4, 4);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new Size(43, 45);
            this.pictureBoxIcon.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBoxIcon.TabIndex = 0;
            this.pictureBoxIcon.TabStop = false;
            this.captionBarAnother1.AllowMerge = false;
            this.captionBarAnother1.CanOverflow = false;
            this.captionBarAnother1.Caption = "提示";
            this.captionBarAnother1.GripStyle = ToolStripGripStyle.Hidden;
            this.captionBarAnother1.Location = new Point(2, 2);
            this.captionBarAnother1.MaxButtonVisible = false;
            this.captionBarAnother1.MinButtonVisible = false;
            this.captionBarAnother1.Name = "captionBarAnother1";
            this.captionBarAnother1.Size = new Size(405, 25);
            this.captionBarAnother1.TabIndex = 4;
            this.captionBarAnother1.Text = "captionBarAnother1";
            this.panelButtonInfo.BorderStyle = BorderStyles.NoBorder;
            this.panelButtonInfo.Dock = DockStyle.Bottom;
            this.panelButtonInfo.Location = new Point(2, 117);
            this.panelButtonInfo.Margin = new Padding(4, 4, 4, 4);
            this.panelButtonInfo.Name = "panelButtonInfo";
            this.panelButtonInfo.Size = new Size(405, 59);
            this.panelButtonInfo.TabIndex = 0;
            base.Appearance.BackColor = SystemColors.ControlLightLight;
            base.Appearance.Font = new Font("Tahoma", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.Appearance.Options.UseBackColor = true;
            base.Appearance.Options.UseFont = true;
            base.AutoScaleDimensions = new SizeF(8f, 17f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(409, 178);
            base.Controls.Add(this.panelForm);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.KeyPreview = true;
            base.Margin = new Padding(4, 4, 4, 4);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "MyMessageBox";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "FormMessageBox";
            base.TopMost = true;
            ((ISupportInitialize)this.panelForm).EndInit();
            this.panelForm.ResumeLayout(false);
            this.panelForm.PerformLayout();
            ((ISupportInitialize)this.panelMessageInfo).EndInit();
            this.panelMessageInfo.ResumeLayout(false);
            ((ISupportInitialize)this.panelIconInfo).EndInit();
            this.panelIconInfo.ResumeLayout(false);
            ((ISupportInitialize)this.pictureBoxIcon).EndInit();
            ((ISupportInitialize)this.panelButtonInfo).EndInit();
            base.ResumeLayout(false);
        }
    }
}
