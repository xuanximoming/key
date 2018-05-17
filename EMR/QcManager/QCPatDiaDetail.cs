using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// /加到用户控件中
    /// add by 王冀 2012年12月21日 
    /// </summary>
    public partial class QCPatDiaDetail : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_app;
        /// <summary>
        /// /
        /// </summary>
        public QCPatDiaDetail()
        {
            InitializeComponent();
        }
        public QCPatDiaDetail(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }
        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_query_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dateEdit_begin.DateTime > this.dateEdit_end.DateTime)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("起始时间不能大于结束时间");

                    return;
                }
                BindData();
                //if ((this.gridView1.DataSource as DataTable) == null || (this.gridView1.DataSource as DataTable).Rows.Count == 0)
                if (((DataView)this.gridView1.DataSource).ToTable() == null || ((DataView)this.gridView1.DataSource).ToTable().Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有符合条件的记录");
                    gridControl1.DataSource = null;
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void QCDieInfo_LocationChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void BindData()
        {
            try
            {
                string dept_id = this.lookUpEditorDepartment.CodeValue.Trim();
                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                string end_time = this.dateEdit_end.DateTime.AddDays(1).ToString("yyyy-MM-dd") + " 23:59:59";
                string dia = this.lookUpdia.CodeValue.Trim();
                string tablename = DataAccess.GetConfigValueByKey("AutoScoreMainpage");
                string[] mainpage = tablename.Split(',');
                //edit by ywk 2013年7月16日 19:53:07
                string sql = @"select ROW_NUMBER() OVER(ORDER BY a.outbed ASC) AS xh,
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
                        imb.xzzaddress as address,
                      /*sysdate-to_date( a.outhosdate,'yyyy-mm-dd')'' die_day,
                      feeinfo.totalfee fee,*/
                       dept.name dept_name,
                       
                       u.name doc_name
                  from inpatient a
                  left join {4} imb 
                    on imb.noofinpat = a.noofinpat
                  left join {5} imd
                    on imd.iem_mainpage_no = imb.iem_mainpage_no
                  left join diagnosis diag
                    on imd.diagnosis_code=diag.markid
                  left join department dept
                    on a.admitdept = dept.id
               
                  left join users u
                    on a.resident = u.id                  
                 where (a.admitdept = '{0}' or '{0}' is null) and imd.diagnosis_type_id='13' and imd.valide='1' and imb.valide='1'
                   and to_date( a.inwarddate,'yyyy-mm-dd hh24:mi:ss') between to_date('{1}','yyyy-mm-dd hh24:mi:ss') and to_date('{2}','yyyy-mm-dd hh24:mi:ss') and  ( imd.diagnosis_code='{3}' or '{3}' is null )";

                string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                {

                    sql = @"select ROW_NUMBER() OVER(ORDER BY a.outbed ASC) AS xh,
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
                       (imb.xzz_provincename||imb.xzz_cityname||imb.xzz_districtname) as address,
                      /*sysdate-to_date( a.outhosdate,'yyyy-mm-dd')'' die_day,
                      feeinfo.totalfee fee,-- zjy 2013-7-9*/
                       dept.name dept_name,
                       imb.mzzyzd_name,
                       imb.mzxyzd_name,                    
                       u.name doc_name
                  from inpatient a
                  left join {4} imb 
                    on imb.noofinpat = a.noofinpat                
                  left join diagnosis diag
                   on  (imb.mzxyzd_code=diag.markid or imb.mzzyzd_code=diag.markid)
                  left join department dept
                    on a.admitdept = dept.id
                  /*left join doctor_assignpatient doc
                    on doc.noofinpat = a.noofinpat*/
                  left join users u
                    on a.resident = u.id                  
                 where (a.admitdept = '{0}' or '{0}' is null) and (imb.valide='1')
                   and to_date( a.inwarddate,'yyyy-mm-dd hh24:mi:ss') between to_date('{1}','yyyy-mm-dd hh24:mi:ss') and to_date('{2}','yyyy-mm-dd hh24:mi:ss') 
                  and  (( imb.MZXYZD_CODE='{3}' or '{3}' is null ) OR (imb.mzzyzd_code='{3}' or '{3}' is null))";
                }

                sql = string.Format(sql, dept_id, begin_time, end_time, dia, mainpage[0], mainpage[2]);
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(dr["mzzyzd_name"].ToString()))
                        {
                            dr["diag_name"] = dr["mzxyzd_name"].ToString();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(dr["mzxyzd_name"].ToString()))
                            {
                                dr["diag_name"] = dr["mzzyzd_name"].ToString();
                            }
                            else
                            {
                                dr["diag_name"] = dr["mzzyzd_name"].ToString() + "," + dr["mzxyzd_name"].ToString();
                            }

                        }
                    }
                }
                gridControl1.DataSource = dt;
                this.labPatCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QCDieInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitDepartment();
                InitDia();
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
                lookUpEditorDepartment.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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

                cols.Add("ID", 70);
                cols.Add("NAME", 80);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;        //王冀  2013  2  22  初始值设置为用户所在科室
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitDia()
        {
            try
            {

                lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select  icd,name,py,wb from diagnosis where valid='1' ");
                DataTable Dia = m_app.SqlHelper.ExecuteDataTable(sql);

                Dia.Columns["ICD"].Caption = "诊断代码";
                Dia.Columns["NAME"].Caption = "诊断名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ICD", 70);
                cols.Add("NAME", 80);

                SqlWordbook diaWordBook = new SqlWordbook("querybook", Dia, "ICD", "NAME", cols, "ICD//NAME//PY//WB");
                lookUpdia.SqlWordbook = diaWordBook;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 打印操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;
                lookUpdia.CodeValue = "";
                ResetControl();

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        public void ResetControl()
        {
            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
           
            this.gridControl1.DataSource = null;
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
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 列序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void QCDieInfo_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(21, this.Height - 22);
                this.labPatCount.Location = new Point(72, this.Height - 22);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridView1.FocusedRowHandle == -1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
