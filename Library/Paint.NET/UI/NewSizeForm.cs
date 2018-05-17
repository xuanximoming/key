using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace DrectSoft.Basic.Paint.NET
{
    public partial class NewSizeForm :Form
    {

        public NewSizeForm()
        {
            InitializeComponent();
        }

        public Size ImageSize
        {
            get
            {
                int width, height;
                if (int.TryParse(txtWidth.Text, out width) &&
                    int.TryParse(txtHeight.Text, out height))
                    return new Size(width, height);
                else
                    return Size.Empty;
            }
            set
            {
                txtWidth.Text = value.Width.ToString();
                txtHeight.Text = value.Height.ToString();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}