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
    /// 手术信息统计
    /// add by ywk 2012年10月23日 09:23:22 
    /// </summary>
    public partial class UcOperatInfo : DevExpress.XtraEditors.XtraUserControl
    {
        IYidanEmrHost m_app;
        public UcOperatInfo(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }
        public UcOperatInfo()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 查询
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
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
        /// <summary>
        /// 打印
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }
        private void BindData()
        {
            try
            {
                string dept_id = this.lookUpEditorDepartment.CodeValue.Trim();

                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
                string end_time = this.dateEdit_end.DateTime.AddDays(1).ToString("yyyy-MM-dd");
                string sql = @" select  distinct b.patid,
                            b.OperName,
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
                           '1' qjcs,
                           '1' CGCS,
                           '' lx,
                           '未结算' fee,
                           dept.name dept_name,
                           u.name doc_name
                           from   (select distinct inp.patid,
                                          wmsys.wm_concat(ope.operation_name || '(' || ope.operation_date || ')') OperName
                                          from inpatient inp
                                         left join iem_mainpage_basicinfo_2012 bas
                                         on inp.noofinpat=bas.noofinpat
                                           left join iem_mainpage_operation_2012 ope
                                         on ope.iem_mainpage_no = bas.iem_mainpage_no and ope.valide = 1 
                                          group by inp.patid) b
                           inner join inpatient a
                           on a.patid=b.patid
                           inner join iem_mainpage_basicinfo_2012 basic
                           on basic.noofinpat = a.noofinpat
                           inner join iem_mainpage_operation_2012 oper
                           on oper.iem_mainpage_no = basic.iem_mainpage_no and oper.valide = 1 
                           left join diagnosis diag
                           on a.AdmitDiagnosis = diag.markid
                           inner join department dept
                           on a.outhosdept = dept.id
                           and (dept.id='{0}' or '{0}' is null)
                           left join doctor_assignpatient doc
                           on doc.noofinpat = a.noofinpat
                           left join users u  on doc.id = u.id
                           where  to_date(substr(nvl(trim(a.admitdate), '1990-01-01'), 1, 10), 'yyyy-mm-dd') >= to_date('{1}', 'yyyy-mm-dd')
                           and to_date(substr(nvl(trim(a.admitdate), '1990-01-01'), 1, 10), 'yyyy-mm-dd') <= to_date('{2}', 'yyyy-mm-dd') order by a.admitdate desc ";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(sql, dept_id, begin_time, end_time), CommandType.Text);//此DataTable要处理
                ////                           and a.admitdate >= '{0}' || ' 00:00:00 ' AND a.admitdate < '{1}' || ' 24:00:00 '";
                #region 注释掉的
                //            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select ROW_NUMBER() OVER(ORDER BY a.outbed ASC) AS xh,
                //                                                                                       a.patid,
                //                                                                                       a.name,
                //                                                                                       (case
                //                                                                                         when a.SexID = 1 then
                //                                                                                          '男'
                //                                                                                         when a.SexID = 2 then
                //                                                                                          '女'
                //                                                                                         else
                //                                                                                          '未知'
                //                                                                                       end) as PATSEX,
                //                                                                                       a.agestr,
                //                                                                                       diag.name diag_name,
                //                                                                                       oper.operation_name OperName,
                //                                                                                       oper.operation_date operdate,
                //                                                                                       a.admitdate,
                //                                                                                       a.outhosdate,
                //                                                                                       a.address,
                //                                                                                       datediff('dd',
                //                                                                                                a.admitdate,
                //                                                                                                NVL(trim(a.outwarddate), TO_CHAR(SYSDATE, 'yyyy-mm-dd'))) INCOUNT,
                //                                                                                       '1' qjcs,
                //                                                                                       '1' CGCS,
                //                                                                                       '' lx,
                //                                                                                       '未结算' fee,
                //                                                                                       dept.name dept_name,
                //                                                                                       u.name doc_name
                //                                                                                  from inpatient a
                //                                                                                  left join diagnosis diag
                //                                                                                    on a.AdmitDiagnosis = diag.markid
                //                                                                                  left join department dept
                //                                                                                    on a.outhosdept = dept.id
                //                                                                                  left join doctor_assignpatient doc
                //                                                                                    on doc.noofinpat = a.noofinpat
                //                                                                                  left join users u
                //                                                                                    on doc.id = u.id
                //                                                                                    left join iem_mainpage_operation oper
                //                                                                                    on oper.iem_mainpage_no = a.noofinpat and oper.valide = 1
                //                                                                                 /*where oper.operation_name is not null*/"));
                #endregion
                gridControl1.DataSource = dt;
                this.labPatCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                throw;
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 窗体加载事件
        /// add by ywk  2012年10月23日16:42:03
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcOperatInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitDepartment();
                this.dateEdit_begin.DateTime = DateTime.Now.AddYears(-1);
                this.dateEdit_end.DateTime = DateTime.Now;
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
                //lookUpEditorDepartment.SelectedText = "妇科";   王冀 2012 12 12 注释
                lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;        //王冀  2013  2  22  初始值设置为用户所在科室
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
                InitDepartment();
                this.dateEdit_begin.DateTime = DateTime.Now.AddYears(-1);
                this.dateEdit_end.DateTime = DateTime.Now;
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

        private void UcOperatInfo_SizeChanged(object sender, EventArgs e)
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
