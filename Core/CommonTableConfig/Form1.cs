using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YiDanSoft.Core.CommonTableConfig.CommonNoteUse;

namespace YiDanSoft.Core.CommonTableConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ReportHeader reportHeader = new ReportHeader();
            this.Controls.Add(reportHeader);
           
            Bitmap bmp = new Bitmap(1100, 800);
            reportHeader.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            reportHeader.Hide();
            this.BackgroundImage = bmp;
        }

        
    }
}
