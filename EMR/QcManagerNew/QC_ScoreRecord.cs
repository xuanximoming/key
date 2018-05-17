using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.FrameWork;
using YidanSoft.Wordbook;
using DevExpress.Utils;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class QC_ScoreRecord : UserControl, IStartPlugIn
    {
        IYidanEmrHost m_app;
        SqlManger m_SqlManager;

        private string m_beginInTime = string.Empty;
        private string m_endInTime = string.Empty;
        private string m_patID = string.Empty;
        private string m_name = string.Empty;
        private string m_status = string.Empty;
        private string m_sortid = string.Empty;
        private string m_userid = string.Empty;
        private Authority m_qcAuth = Authority.DEPTQC;
        /// <summary>
        /// 患者病历评分列表页面
        /// </summary>
        public QC_ScoreRecord(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            SqlManger m_SqlManger = new SqlManger(app);
            m_SqlManager = new SqlManger(app);
            m_userid = app.User.DoctorId;
        }
        private WaitDialogForm m_WaitDialog;

        #region 实现接口部分
        public IPlugIn Run(IYidanEmrHost host)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutMedicalScore_Load(object sender, EventArgs e)
        {
            try
            {
                InitDepartment();//绑定科室下拉框 
                InitInTime();//绑定时间
                BindDataSouce();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }


        private void InitlookUpEditorValue()
        {
            //lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;
            //lookUpEditorDepartment.CodeValue = "2401";
        }

        /// <summary>
        /// 双击进入该病人的评分信息页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridView1.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow focuseRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (focuseRow == null)
                    return;
                string noofpaint = focuseRow["NOOFINPAT"].ToString();
                int sumpoint = int.Parse(focuseRow["sumpoint"].ToString());
                string recordid = focuseRow["id"].ToString();
                EmrPainetScoreNew emrpointScore = new EmrPainetScoreNew(m_app, noofpaint, sumpoint, recordid);
                SetWaitDialogCaption("正在加载病人评分详情表...");
                emrpointScore.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
                emrpointScore.ShowDialog();
                HideWaitDialog();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 弹出等待提示框
        /// </summary>
        /// <param name="caption"></param>
        public void SetWaitDialogCaption(string caption)
        {
            try
            {
                if (m_WaitDialog != null)
                {
                    if (!m_WaitDialog.Visible)
                        m_WaitDialog.Visible = true;
                    m_WaitDialog.Caption = caption;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void HideWaitDialog()
        {
            try
            {
                if (m_WaitDialog != null)
                    m_WaitDialog.Hide();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 按科室查询病人信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dateEditBeginInTime.DateTime > this.dateEditEndInTime.DateTime)
                {
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("起始时间不能大于结束时间");
                    return;
                }
                BindDataSouce();
                //if ((this.gridView1.DataSource as DataTable) == null || (this.gridView1.DataSource as DataTable).Rows.Count == 0)
                if (((DataView)this.gridView1.DataSource).ToTable() == null || ((DataView)this.gridView1.DataSource).ToTable().Rows.Count == 0)
                {
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("没有符合条件的记录");
                }
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }
        #endregion

        #region 方法


        /// <summary>
        /// 绑定科室下拉列表
        /// </summary>
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
                lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;        //王冀  2013  1  4  初始值设置为用户所在科室
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 数组DataRow[]转化成表DataTable
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);
                return tmp;
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
                throw;
            }
        }

        private void InitInTime()
        {
            //默认为显示一月内的数据
            dateEditBeginInTime.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEditEndInTime.Text = DateTime.Now.ToShortDateString();
        }

        private int SumPoint { get; set; }//满分值。通过配置中取得 ywk 2012年6月12日 14:43:29 
        /// <summary>
        /// 按科室查询病人信息 
        /// </summary>
        /// <param name="p"></param>
        private void BindDataSouce()
        {
            try
            {
                string sql = @" select dep.name deptname,
       ip.name ipname,
       ip.patid,
       rd.noofinpat,
       decode(rd.qctype, 0, '环节质控', '终末质控') qctype,
       ip.inwarddate,
       rd.name recordname,
       rd.score,
      u.name  CREATEUSERNAME,
       rd.id,
       decode(rd.qctype, 0, '85', '100') sumpoint
  from emr_automark_record rd, inpatient ip, department dep,users u
 where ip.noofinpat = rd.noofinpat
   and dep.id = ip.outhosdept
   and u.id=rd.create_user
      and (dep.id = '{0}' or '{0}' is null)
      and (ip.name like '%{1}%' or '{1}' is null or '{1}'='')
      and (ip.patid like '%{2}%' or '{2}' is null or '{2}'='')
      and (rd.qctype = '{3}' or '{3}' = '-1')
      /*and to_char(to_date('{4}', 'yyyy-MM-dd'), 'yyyy-MM-dd') <= ip.outwarddate
      AND ip.outwarddate <=  to_char(to_date('{5}', 'yyyy-MM-dd'), 'yyyy-MM-dd')*/
      and to_char(to_date('{4}', 'yyyy-MM-dd'), 'yyyy-MM-dd') <= ip.ADMITDATE
      AND ip.ADMITDATE <=  to_char(to_date('{5}', 'yyyy-MM-dd'), 'yyyy-MM-dd')
      and rd.isvalid='1' and rd.isauto='0'
    order by dep.id, ip.OUTBED";
                string dept = lookUpEditorDepartment.CodeValue;
                string ipid = textEditPatID.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]"); ;
                string name = textEditName.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]"); ;
                string qctype = comboBox1.SelectedIndex.ToString();
                string begindate = dateEditBeginInTime.Text;
                string enddate = dateEditEndInTime.DateTime.AddDays(1).ToString("yyyy-MM-dd");//.Text;
                sql = string.Format(sql, dept, name, ipid, qctype, begindate, enddate);
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);

                gridControl1.DataSource = dt;
                this.labPatCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        /// <summary>
        /// 病历评分列表增加导出功能
        /// add by ywk 2012年6月12日 14:32:28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
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
                    gridControl1.ExportToXls(saveFileDialog.FileName, true);

                    m_app.CustomMessageBox.MessageShow("导出成功！");
                }
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }
        /// <summary>
        /// 新增打印操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                gridControl1.Print();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
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
        /// <summary>
        /// /重置按钮清空搜索条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                InitInTime();
                this.textEditName.Text = "";
                this.textEditPatID.Text = "";
                lookUpEditorDepartment.CodeValue = "";
                textEditPatID.Text = "";
                textEditName.Text = "";
                comboBox1.SelectedIndex = -1;
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }

        private void OutMedicalScore_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(26, this.Height - 22);
                this.labPatCount.Location = new Point(77, this.Height - 22);
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }
    }
}
