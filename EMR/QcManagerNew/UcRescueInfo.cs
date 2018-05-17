using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.Wordbook;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YindanSoft.Emr.QcManagerNew
{
    /// <summary>
    /// /抢救信息统计
    /// add by ywk 
    /// </summary>
    public partial class UcRescueInfo : DevExpress.XtraEditors.XtraUserControl
    {
        public UcRescueInfo()
        {
            InitializeComponent();
        }
        IYidanEmrHost m_app;
        public UcRescueInfo(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_query_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            try
            {
                string dept_id = this.lookUpEditorDepartment.CodeValue.Trim();

                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
                string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd");

                DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select ROW_NUMBER() OVER(ORDER BY a.outbed ASC) AS xh,
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
                                                                                    datediff('dd',
                                                                                            a.admitdate,
                                                                                            NVL(trim(a.outwarddate), TO_CHAR(SYSDATE, 'yyyy-mm-dd'))) INCOUNT,
                                                                                    '1' qjcs,
                                                                                    '1' CGCS,
                                                                                    '' lx,
                                                                                    '未结算' fee,
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
                                                                where (a.outhosdept = '{0}' or '{0}' is null) 
and to_date( a.admitdate,'yyyy-mm-dd hh24:mi:ss') between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd')", dept_id, begin_time, end_time));  //假数据修改   王冀 2013  1  4
                gridControl1.DataSource = dt;
                this.labPatCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }

        private void UcRescueInfo_Load(object sender, EventArgs e)
        {
            InitDepartment();
            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
        }

        //初始化科室
        private void InitDepartment()
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

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }

        private void UcRescueInfo_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(17, this.Height - 23);
                this.labPatCount.Location = new Point(68, this.Height - 23);
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
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
