using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class UnSubmitForm : UserControl
    {
        public UnSubmitForm()
        {
            InitializeComponent();
        }

        private void UnSubmitForm_Load(object sender, EventArgs e)
        {
            this.lookUpEditorDept.Focus();
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
