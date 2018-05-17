using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;

namespace DrectSoft.Emr.QcManager
{
    public partial class QC_Disease_Level : DevExpress.XtraEditors.XtraForm
    {
        IEmrHost m_app;
        public QC_Disease_Level(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        private void QC_Disease_Level_Load(object sender, EventArgs e)
        {
            BindData();
          
        }

      
        private void BindData()
        {
            DataTable dt = m_app.SqlHelper.ExecuteDataTable("select ROW_NUMBER() OVER(ORDER BY a.icd ASC) AS xh,a.* from dict_std  a");
            gridControl1.DataSource = dt;
            
        }
    }
}