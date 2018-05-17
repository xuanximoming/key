using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.QCReport.controls
{
	public class MyLabel:LabelControl,IControlDataInit
	{

        public string Value
        {
            get { return ""; }
        }

        public void InitControlBindData()
        {
        }


        public void Reset()
        {
           
        }
    }
}
