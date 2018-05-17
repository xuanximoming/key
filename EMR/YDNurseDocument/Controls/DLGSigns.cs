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
    public partial class DLGSigns : DevBaseForm, IDlg
    {
        public string _editValue = "";//保存编辑的值
        public DLGSigns(string val)
        {
            InitializeComponent();
            this.ucTextGroupBox1.Shit = val;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _editValue = this.ucTextGroupBox1.Shit;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


        private void barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string temp = e.Item.Caption.Trim();
            try
            {
                SendKeys.Send(temp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public string EditValue
        {
            get
            {
                return _editValue;
            }
            set
            {
                _editValue=value;
            }
        }
    }

}