using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.DSSqlHelper;
using System.Configuration;

namespace DrectSoft.Emr.QCTimeLimit
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QCTimeLimitInnerService manager = new QCTimeLimitInnerService();
            manager.MainProcess();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
        }

        private void buttonCompleteRecord_Click(object sender, EventArgs e)
        {
            QCTimeExternal external = new QCTimeExternal();
            external.CompleteQCTimeLimit(textBoxRecordDetailID.Text.Trim());
        }
    }
}
