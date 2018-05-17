using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;

namespace YidanSoft.Core.QCReport.controls
{
    public partial class MyTextBox : TextEdit, IControlDataInit
    {
        public MyTextBox()
        {
            InitializeComponent();
        }

        public MyTextBox(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        public void InitControlBindData()
        { }
    }
}
