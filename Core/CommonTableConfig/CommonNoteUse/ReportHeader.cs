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
    public partial class ReportHeader : UserControl
    {
        public ReportHeader()
        {
            InitializeComponent();
        }

        public Dictionary<string,Control> GetDataControl()
        {
            try
            {
                Dictionary<string, Control> ctls = new Dictionary<string, Control>();
                int i = 0;
                foreach (Control ctl in this.Controls)
                {
                    if (ctl.Tag!=null && ctl.Tag.ToString().Equals("1"))
                    {
                        ctls.Add((i++).ToString(),ctl);
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
