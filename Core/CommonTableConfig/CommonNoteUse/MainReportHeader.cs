using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YiDanSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class MainReportHeader : UserControl
    {
        public MainReportHeader()
        {
            InitializeComponent();
        }

        public Dictionary<string,Control> GetDataControl()
        {
            try
            {
                Dictionary<string, Control> ctls = new Dictionary<string, Control>();
                foreach (Control ctl in this.Controls)
                {
                    if (ctl.Tag!=null)
                    {
                        ctls.Add(ctl.Tag.ToString(),ctl);
                    }
                }
                return ctls;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
