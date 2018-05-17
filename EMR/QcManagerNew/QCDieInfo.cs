using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Wordbook;

namespace YindanSoft.Emr.QcManagerNew
{
    /// <summary>
    /// /加到用户控件中
    /// add by ywk 2012年10月23日 09:10:52 
    /// </summary>
    public partial class QCDieInfo : DevExpress.XtraEditors.XtraUserControl
    {
        IYidanEmrHost m_app;
        /// <summary>
        /// /
        /// </summary>
        public QCDieInfo()
        {
            InitializeComponent();
        }
        public QCDieInfo(IYidanEmrHost app)
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
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("起始时间不能大于结束时间");
                    return;
                }
                BindData();
                //if ((this.gridView1.DataSource as DataTable) == null || (this.gridView1.DataSource as DataTable).Rows.Count == 0)
                if (((DataView)this.gridView1.DataSource).ToTable() == null || ((DataView)this.gridView1.DataSource).ToTable().Rows.Count == 0)
                {
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("没有符合条件的记录");
                }
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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

                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
                string end_time = this.dateEdit_end.DateTime.AddDays(1).ToString("yyyy-MM-dd");
                #region 注释 edit by wyt 2012-11-06
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
                //王冀 2012 12 6
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
a.outhosdate,
       a.address,
     round(sysdate - to_date(a.outhosdate, 'yyyy-mm-dd hh24:mi:ss'),0) die_day,
      feeinfo.totalfee fee,
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
   and (a.outhosdept = '{0}'
    or '{0}' is null)
and to_date( a.outhosdate,'yyyy-mm-dd hh24:mi:ss') between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd')";
                sql = string.Format(sql, dept_id, begin_time, end_time);
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
                //                                                                where a.outhosdept = '{0}' or '{0}' is null", dept_id));
                #endregion
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
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
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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
                //lookUpEditorDepartment.SelectedText = "妇科";
                lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;        //王冀  2013  1  4  初始值设置为用户所在科室

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 打印操作
        /// add by ywk 2012年10月23日 09:12:49 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_print_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
                lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;//wangj 2013 3 5 重置为默认科室
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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
            catch (Exception ex)
            {

            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                YiDanCommon.YD_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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

            }
        }
    }
}
