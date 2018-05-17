using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;


namespace DrectSoft.Core.IEMMainPage
{
    public partial class FormMainPage : DevExpress.XtraEditors.XtraForm
    {
        public FormMainPage(IEmrHost app)
        {
            InitializeComponent();
        }
 
    }
}