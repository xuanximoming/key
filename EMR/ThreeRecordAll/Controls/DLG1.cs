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
    /// <summary>
    /// 另一版本大便次数
    /// add by ywk 2013年4月28日9:12:11 
    /// </summary>
    public partial class DLG1 : DevBaseForm, IDlg
    {
        public DLG1()
        {
            InitializeComponent();
        }

        public DLG1(string val)
        {
            InitializeComponent();
            this.ucTextGroupBoxNew1.Shit = val;
        }

        private string _editValue = "";//保存编辑的值

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

            _editValue = this.ucTextGroupBoxNew1.Shit;
            this.DialogResult = DialogResult.OK;
        }

        private void DevButtonCancel1_Click(object sender, EventArgs e)
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

        private void DLG1_Load(object sender, EventArgs e)
        {
            this.ActiveControl = ucTextGroupBoxNew1;
        }
    }
}