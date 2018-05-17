using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.NurseDocument.Controls
{
    public partial class CombItems : DevBaseForm, IDlg
    {
        private string _editValue;

        public CombItems(string val)
        {
            InitializeComponent();
            EditValue = val;
        }

        private void DevButtonCancel1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void DevButtonOK1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this._editValue = this.comboBox1.Text;
        }

        public string EditValue
        {
            get { return _editValue; }
            set
            {
                this.comboBox1.Text = value == null ? "" : value;
                _editValue = this.comboBox1.Text;
            }
        }
    }
}