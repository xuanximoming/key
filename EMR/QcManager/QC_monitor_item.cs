﻿using System;
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
    public partial class QC_monitor_item : DevBaseForm
    {
        IEmrHost m_app;
        public QC_monitor_item(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        private void QC_Doctor_Query_Load(object sender, EventArgs e)
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



        private void btn_query_Click(object sender, EventArgs e)
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
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select c_name,
                                                                            a.i_code,
                                                                            a.i_name,
                                                                            a.limit,
                                                                            a.start_point,
                                                                            a.e_code as e_ode
                                                                            from dict_monitor_item a,dict_point b
                                                                            where (a.start_point = b.code(+))
                                                                            order by c_code,i_code;
                                                                            "));
                gridControl1.DataSource = dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_print_Click(object sender, EventArgs e)
        {

        }
    }
}