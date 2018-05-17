using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using System.Runtime.InteropServices;
using DrectSoft.DSSqlHelper;
using DrectSoft.Common.Eop;

namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    public partial class DaLianYiBaoForm : DevBaseForm
    {
        Inpatient m_currentInpatient;
        public DaLianYiBaoForm(Inpatient currentInpatient)
        {
            InitializeComponent();
            m_currentInpatient = currentInpatient;
            
            DS_SqlHelper.CreateSqlHelperByDBName("HISDB");
            string sql = string.Format("select patnoofhis,yybh,bxbh,zlxh from YD_DLYBSC where patnoofhis='{0}'", m_currentInpatient.NoOfHisFirstPage);
         DataTable dt= DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);

        }


        [DllImport("OltpTransIc09.dll")]
        private static extern int OltpTransData(int msgType,string packageType,
            string packageLength, string str, string com);
    }
}
