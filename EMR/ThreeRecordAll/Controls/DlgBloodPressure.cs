using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.EMR.ThreeRecordAll.Controls
{
    public partial class DlgBloodPressure : DevBaseForm, IDlg
    {
        private string _editValue = "";//保存编辑的值

        public DlgBloodPressure(string val)
        {
            InitializeComponent();
            this.splitTextBox1.Text = val;
        }

        public DlgBloodPressure() 
        {
            InitializeComponent();
        }

        public string EditValue
        {
            get
            {
                return _editValue;
            }
            set
            {
                _editValue = value;
            }
        }

        private void DevButtonOK1_Click(object sender, EventArgs e)
        {
            _editValue = this.splitTextBox1.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}