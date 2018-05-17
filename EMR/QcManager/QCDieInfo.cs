using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// /加到用户控件中
    /// add by ywk 2012年10月23日 09:10:52 
    /// </summary>
    public partial class QCDieInfo : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_app;

        public QCDieInfo()
        {
            InitializeComponent();
        }

        public QCDieInfo(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_app = app;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    MessageBox.Show("起始时间不能大于结束时间");
                    return;
                }
                BindData();
                if (((DataView)this.gridView1.DataSource).ToTable() == null || ((DataView)this.gridView1.DataSource).ToTable().Rows.Count == 0)
                {
                    MessageBox.Show("没有符合条件的记录");
                    gridControl1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void BindData()
        {
            try
            {
                string dept_id = this.lookUpEditorDepartment.CodeValue.Trim();

                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd") + " " + "00:00:00";
                string end_time = this.dateEdit_end.DateTime.AddDays(1).ToString("yyyy-MM-dd") + " " + "23:59:59";


                string tablename = DataAccess.GetConfigValueByKey("AutoScoreMainpage");
                string[] mainpage = tablename.Split(',');
                StringBuilder sqlstr = new StringBuilder();
                sqlstr.Append(@"select ROW_NUMBER() OVER(ORDER BY a.outbed ASC) AS xh,
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
                                 mainpage.XZZ_PROVINCENAME || mainpage.XZZ_CITYNAME || mainpage.XZZ_DISTRICTNAME as address,
                                 round((sysdate - to_date(a.outhosdate, 'yyyy-mm-dd hh24:mi:ss')+1),0) die_day,
                                 feeinfo.totalfee fee,
                                 dept.name dept_name,
                                 u.name doc_name
                                 from inpatient a
                                 left join diagnosis diag
                                 on a.AdmitDiagnosis = diag.markid
                                 left join department dept
                                 on a.outhosdept = dept.id
                                 
                                 left join users u
                                 on u.id = a.resident 
                                 left join {0} mainpage
                                 on mainpage.noofinpat = a.noofinpat
                                 left join iem_mainpage_feeinfo feeinfo
                                 on feeinfo.iem_mainpage_no=mainpage.iem_mainpage_no
                                 where mainpage.outhostype = '5'");

                if (!string.IsNullOrEmpty(dept_id.Trim()))
                {
                    sqlstr.Append(@" and a.outhosdept=@outHosDept");
                }
                sqlstr.Append(@" and trunc(to_date( a.outhosdate,'yyyy-mm-dd hh24:mi:ss')) between to_date(@begin_time,'yyyy-mm-dd hh24:mi:ss') and to_date(@end_time,'yyyy-mm-dd hh24:mi:ss')");
                //sql = string.Format(sql, mainpage[0], dept_id, begin_time, end_time);
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
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(sqlstr.ToString(), mainpage[0]),
                new SqlParameter[] 
                { 
                 new SqlParameter("@outHosDept",dept_id),
                 new SqlParameter("@begin_time", begin_time),
                 new SqlParameter("@end_time",end_time)},
                 CommandType.Text);
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
                MyMessageBox.Show(1, ex);
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
            catch (Exception ex)
            {
                throw ex;
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;//wangj 2013 3 5 重置为默认科室
                ResetControl();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        public void ResetControl()
        {
            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();

            this.gridControl1.DataSource = null;
        }

        /// <summary>
        /// 键盘事件切换(Tab)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 加序号列
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体大小改变触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QCDieInfo_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(21, this.Height - 22);
                this.labPatCount.Location = new Point(72, this.Height - 22);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MyMessageBox.Show(1, ex);
            }
        }
    }
}
