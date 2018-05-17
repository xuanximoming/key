using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Wordbook;
using YiDanCommon.Ctrs.FORM;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class QC_Die_Info : DevBaseForm
    {
        IYidanEmrHost m_app;
        public QC_Die_Info(IYidanEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_app = app;

            }
            catch (Exception)
            {
                throw;
            }

        }

        private void QC_Doctor_Query_Load(object sender, EventArgs e)
        {
            try
            {
                InitDepartment();
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
                this.lookUpEditorDepartment.Focus();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        //初始化科室
        private void InitDepartment()
        {

            try
            {
                lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
                DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
            }
            catch (Exception)
            {
                throw;
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
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }

        }

        private void BindData()
        {
            try
            {
                string dept_id = this.lookUpEditorDepartment.CodeValue.Trim();
                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
                string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd");
                string sql = string.Format(@"select ROW_NUMBER() OVER(ORDER BY a.outbed ASC) AS xh,
       a.patid,
       a.name,
       (case
         when a.SexID = 1 then
          '男'
         when a.SexID = 2 then
          '女'
         else
          '未知'
       end) as PATSEX,
       a.agestr,
       diag.name diag_name,
       a.admitdate,
       a.address,
      sysdate-to_date( a.outhosdate,'yyyy-mm-dd')  die_day,
     decode( feeinfo.totalfee,'','未结算',feeinfo.totalfee) fee,
       dept.name dept_name,
       u.name doc_name
  from inpatient a
  left join diagnosis diag
    on a.AdmitDiagnosis = diag.markid
  left join department dept
    on a.outhosdept = dept.id
  left join doctor_assignpatient doc
    on doc.noofinpat = a.noofinpat
  left join users u
    on doc.id = u.id
  left join iem_mainpage_basicinfo_2012 mainpage
    on mainpage.noofinpat = a.noofinpat
    left join iem_mainpage_feeinfo feeinfo
    on feeinfo.iem_mainpage_no=mainpage.iem_mainpage_no
 where mainpage.outhostype = '5' 
   and ( a.outhosdept = '{0}'
    or '{0}' is null)
and to_date( a.outhosdate,'yyyy-mm-dd hh24:mi:ss') between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd')", dept_id, begin_time, end_time);
                #region
                //            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select ROW_NUMBER() OVER(ORDER BY a.outbed ASC) AS xh,
                //                                                                    a.patid,
                //                                                                    a.name,
                //                                                                    (case
                //                                                                        when a.SexID = 1 then
                //                                                                        '男'
                //                                                                        when a.SexID = 2 then
                //                                                                        '女'
                //                                                                        else
                //                                                                        '未知'
                //                                                                    end) as PATSEX,
                //                                                                    a.agestr,
                //                                                                    diag.name diag_name,
                //                                                                    a.admitdate,
                //                                                                    to_date(substr(a.admitdate, 1, 10), 'yyyy-mm-dd') + 10 die_time,
                //                                                                    a.address,
                //                                                                    10 die_day,
                //                                                                    '未结算' fee,
                //                                                                    dept.name dept_name,
                //                                                                    u.name doc_name
                //                                                                from inpatient a
                //                                                                left join diagnosis diag
                //                                                                on a.AdmitDiagnosis = diag.markid
                //                                                                left join department dept
                //                                                                on a.outhosdept = dept.id
                //                                                                left join doctor_assignpatient doc
                //                                                                on doc.noofinpat = a.noofinpat
                //                                                                left join users u
                //                                                                on doc.id = u.id
                //                                                                where a.outhosdept = '{0}' or '{0}' is null and rownum <8", dept_id));
                #endregion
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                gridControl1.DataSource = dt;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "导出";
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
                DialogResult dialogResult = saveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    //gridViewCardList.ExportToXls(saveFileDialog.FileName);
                    gridControl1.ExportToXls(saveFileDialog.FileName, true);

                    m_app.CustomMessageBox.MessageShow("导出成功！");
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.lookUpEditorDepartment.Text = "";
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
    }
}