using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Library.EmrEditor.Src.Common;
using DrectSoft.Library.EmrEditor.Src.Document;

namespace DrectSoft.Library.EmrEditor.Src.Gui
{
    public partial class TextBoxFrm : Form
    {
        public TextBoxFrm(object o )
        {
            InitializeComponent();
            this.richTextBox1.SelectionChanged += new EventHandler(richTextBox1_SelectionChanged);

            if (o is ZYReplace)
            {
                ele = o as ZYReplace;
                this.richTextBox1.Text = ele.Text;
            }
        }
        ZYTextBlock ele = null;
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ele != null)
            {
                ele.Text =  this.richTextBox1.Text.Length >0 ? this.richTextBox1.Text:" ";
                ele.OwnerDocument.RefreshSize();
                ele.OwnerDocument.ContentChanged();
                ele.OwnerDocument.OwnerControl.Refresh();
                ele.OwnerDocument.UpdateCaret();
            }
            this.Close();
        }

        private void TextBoxFrm_Load(object sender, EventArgs e)
        {
            //设置字体大小
            //Font f = new Font(ZYEditorControl.GetDefaultSettings("fontname"), FontCommon.GetFontSizeByName(ZYEditorControl.GetDefaultSettings("fontsize")));
            //this.richTextBox1.Font = f;
            this.Text = ele.Name;
        }

        void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            if(this.richTextBox1.SelectionLength ==0)
                this.richTextBox1.SelectionColor = Color.Red;
        }


        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.richTextBox1.SelectionLength = 0;
            this.richTextBox1.SelectionColor = Color.Red;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
