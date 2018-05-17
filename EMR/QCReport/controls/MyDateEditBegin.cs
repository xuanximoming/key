using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.QCReport.controls
{
    public class MyDateEditBegin : DateEdit, IControlDataInit
	{
        public string Value
        {
            get { return this.Text; }
        }

        public void InitControlBindData()
        {
            this.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        }

        public void Reset()
        {
            this.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        }
    }
}
