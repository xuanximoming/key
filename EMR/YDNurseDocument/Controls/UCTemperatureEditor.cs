using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.NurseDocument.Controls
{
    /// <summary>
    /// 体温编辑用户控件
    /// </summary>
    public partial class UCTemperatureEditor : DevExpress.XtraEditors.XtraUserControl
    {
        public LookUpEdit LookUpWayOfSurvey
        {
            get { return this.lookUpWayOfSurvey; }
        }

        public TextEdit TxtTemperature
        {
            get { return this.txtTemperature1; }
        }

        public UCTemperatureEditor()
        {
            InitializeComponent();
        }

        private object _value;

        public object Value
        {
            get 
            {
                if (_value == null)
                    return "";
                else
                {
                    GetValue();
                    return _value;
                }
            }
            set { _value = value; SetValue(value); }
        }

        private void SetValue(object val)
        {
            if (val == null || val.ToString().Equals(""))
            {
                lookUpWayOfSurvey.EditValue = "8801"; 
                return;
            }
            string[] vals = val.ToString().Split(':');
            txtTemperature1.Text = vals[0];
            lookUpWayOfSurvey.Text = vals[1];
            //lookUpWayOfSurvey.EditValue = vals[1];
        }

        private void GetValue()
        {
            if (txtTemperature1.EditValue != null)
                _value = txtTemperature1.EditValue.ToString() + ":" + lookUpWayOfSurvey.Text.ToString();
            else _value = "";

        }

        private void txtTemperature1_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void lookUpWayOfSurvey_Enter(object sender, EventArgs e)
        {
            if (sender.GetType().Name.Equals("TextEdit"))
            {
                //m_ActivateTextEdit = sender as TextEdit;
            }
            else
            {
                
                //m_ActivateTextEdit = null;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (this.lookUpWayOfSurvey.Focused && keyData == Keys.Down || keyData == Keys.Up)
                {
                    return false;
                }
                else if (this.txtTemperature1.Focused && keyData == Keys.Right) 
                {
                    return lookUpWayOfSurvey.Focus();
                    
                }
                else
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            catch (Exception ex)
            {                               
                
                throw ex;
            }
        }

        public override bool Focused
        {
            get
            {
                return txtTemperature1.Focused;
            }
        }



    }
}
