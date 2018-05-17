using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Common.SystemAppManager
{
    public partial class CaptionEditBox : UserControl
    {
        string _configValue;

        public CaptionEditBox()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public string Config
        {
            get { return _configValue; }
            set 
            {
                if (_configValue != value)
                {
                    SetConfigValue(value);
                    OnConfigChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void SetConfigValue(string value)
        {
            _configValue = value;
        }

        #region Event ConfigChanged
        private static readonly object EventKey_ConfigChanged = new object();

        protected virtual void OnConfigChanged(EventArgs e)
        {
            EventHandler handler =
                base.Events[EventKey_ConfigChanged] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler ConfigChanged
        {
            add { base.Events.AddHandler(EventKey_ConfigChanged, value); }
            remove { base.Events.RemoveHandler(EventKey_ConfigChanged, value); }
        }
        #endregion

    }

    public class CaptionColorBox : CaptionEditBox
    {
        private DevExpress.XtraEditors.ColorEdit colorEdit1 = new DevExpress.XtraEditors.ColorEdit();

        public CaptionColorBox()
            : base()
        {
            // 
            // colorEdit1
            // 
            this.colorEdit1.EditValue = System.Drawing.Color.Empty;
            this.colorEdit1.Location = new System.Drawing.Point(0, 28);
            this.colorEdit1.Name = "colorEdit1";
            this.colorEdit1.Size = new System.Drawing.Size(371, 23);
            colorEdit1.ColorChanged += new EventHandler(colorEdit1_ColorChanged);
            this.Controls.Add(colorEdit1);
        }

        void colorEdit1_ColorChanged(object sender, EventArgs e)
        {
            Config = colorEdit1.Color.ToArgb().ToString();
        }

        protected override void SetConfigValue(string value)
        {
            base.SetConfigValue(value);
            colorEdit1.Color = Color.FromArgb(int.Parse(value));
        }
    }

    public class CaptionTextBox : CaptionEditBox
    {
        private DevExpress.XtraEditors.TextEdit textEdit1 = new DevExpress.XtraEditors.TextEdit();

        public CaptionTextBox():base()
        {
            // 
            // textEdit1
            // 
            textEdit1.Location = new System.Drawing.Point(0, 28);
            textEdit1.Name = "textEdit1";
            textEdit1.Size = new System.Drawing.Size(371, 23);
            textEdit1.TabIndex = 2;
            textEdit1.TextChanged += new EventHandler(textEdit1_TextChanged);
            this.Controls.Add(textEdit1);
        }

        public DevExpress.XtraEditors.Mask.MaskProperties MaskInfo
        {
            get { return textEdit1.Properties.Mask; }
        }

        void textEdit1_TextChanged(object sender, EventArgs e)
        {
            Config = textEdit1.Text;
        }

        protected override void SetConfigValue(string value)
        {
            base.SetConfigValue(value);
            if (textEdit1.Text != value) textEdit1.Text = value;
        }
    }

    public class CaptionIntBox : CaptionTextBox
    {
        public CaptionIntBox():base()
        {
            this.MaskInfo.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.MaskInfo.EditMask = "d";
        }
    }

    public class CaptionDoubleBox : CaptionTextBox
    {
        public CaptionDoubleBox():base()
        {
            this.MaskInfo.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.MaskInfo.EditMask = "n";
        }
    }
}
