using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.NurseDocument.Controls
{
	public partial class DlgBloodPressure: DevBaseForm,IDlg
	{
        private string _editValue = "";//保存编辑的值

        public DlgBloodPressure(string val)
        {
            try
            {
                InitializeComponent();
                this.splitTextBox1.Text = val;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           

        }

        public DlgBloodPressure()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                
                throw ex;
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
                _editValue = value;
            }
        }

        private void DevButtonOK1_Click(object sender, EventArgs e)
        {
            try
            {
                _editValue = this.splitTextBox1.Text;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        
        }
        public override bool Focused { get { return this.splitTextBox1.bTextBox.Focused; } }


        private void DlgBloodPressure_Shown(object sender, EventArgs e)
        {
            try
            {
                this.ActiveControl = splitTextBox1;
                splitTextBox1.aTextBox.Focus();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
         
        }
    }
}
