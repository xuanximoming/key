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
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.QcManager
{
    public partial class QC_Disease_Level : DevBaseForm
    {
        IEmrHost m_app;
        public QC_Disease_Level(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        private void QC_Disease_Level_Load(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }


        private void BindData()
        {
            try
            {
                DataTable dt = m_app.SqlHelper.ExecuteDataTable("select ROW_NUMBER() OVER(ORDER BY a.icd ASC) AS xh,a.* from dict_std  a");
                gridControl1.DataSource = dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}