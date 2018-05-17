using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;

namespace DrectSoft.Common.Ctrs.OTHER
{
    [ToolboxBitmap(typeof(TextBox))]
    [Description("功能描述：封装一个复合控件\r\n------\r\nbwj  20130104")]
    public partial class DevTextboxAndButton : Control
    {
        private Color colBColor;
        private Color colFColor;

        public Color TextBoxAndButtonBackColor
        {
            get { return colBColor; }
            set 
            { 
                colBColor = value;
                textBox1.BackColor = colBColor;
            }
        }

        public Color TextBoxAndButtonForeColor
        {
            get { return colFColor; }
            set
            {
                colFColor = value;
                textBox1.BackColor = colFColor;
            }
        }

        public DevTextboxAndButton()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
