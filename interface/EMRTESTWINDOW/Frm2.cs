using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EmrInfirce;

namespace EMRTESTWINDOW
{
    public partial class Frm2 : Form
    {
        public Frm2()
        {
            InitializeComponent();
            UCEmr us = new UCEmr();
            us.Shuaxin("2024");
            us.Dock = DockStyle.Fill;
            this.Controls.Add(us);
        }
    }
}