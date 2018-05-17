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
using YiDanCommon.Ctrs.DLG;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class QC_LostScoreCat : UserControl, IStartPlugIn
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
        public QC_LostScoreCat(IYidanEmrHost app)
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
                //"ZKSFXTJZongFen"
                //xll 2013-02-26 临时 通过配置是否显示总分
                string conID = m_SqlManager.GetConfigValueByKey("ZKSFXTJZongFen");
                if (conID == string.Empty || conID == "0")
                {
                    gcZongFeng.Visible = false;
                }
                else
                {
                    gcZongFeng.Visible = true;
                }
                InitDepartment();//绑定科室下拉框 
                InitlookUpEditParents();
                InitlookUpEditChild();
                InitInTime();//绑定时间
                InitDoctor();

            }
            catch (Exception)
            {
                throw;
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
                //if (this.gridView1.FocusedRowHandle < 0)
                //{
                //    return;
                //}
                //return;
                //DataRow focuseRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                //if (focuseRow == null)
                //    return;
                //string noofpaint = focuseRow["NOOFINPAT"].ToString();
                //int sumpoint = int.Parse(focuseRow["sumpoint"].ToString());
                //EmrPainetScore emrpointScore = new EmrPainetScore(m_app, noofpaint, sumpoint);
                //SetWaitDialogCaption("正在加载病人评分表...");
                //emrpointScore.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
                //emrpointScore.ShowDialog();
                //HideWaitDialog();
                ////更改为调出报表页面
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("入院起始时间不能大于结束时间");
                    return;
                }

                if (this.dtZKStart.DateTime > this.dtZKEnd.DateTime)
                {
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("质控起始时间不能大于结束时间");
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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

                //InitlookUpEditorValue();
                lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 加载父类别下拉框数据
        /// </summary>
        private void InitlookUpEditParents()
        {
            try
            {
                DataTable dt = m_SqlManager.GetReductionParents();
                lookUpEditParents.Properties.DataSource = dt;
                lookUpEditParents.Properties.ValueMember = "ID";
                lookUpEditParents.Properties.DisplayMember = "NAME";
                lookUpEditParents.Properties.NullText = "";
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 加载子类别下拉框数据
        /// </summary>
        private void InitlookUpEditChild()
        {
            try
            {
                DataTable dt = m_SqlManager.GetReductionChild();

                if (lookUpEditParents.EditValue.ToString().Trim() != "")
                {
                    dt = ToDataTable(dt.Select("ccode='" + lookUpEditParents.EditValue.ToString() + "'"));
                }

                lookUpEditChild.Properties.DataSource = dt;
                lookUpEditChild.Properties.ValueMember = "ID";
                lookUpEditChild.Properties.DisplayMember = "NAME";
                lookUpEditChild.Properties.NullText = "";
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
                throw;
            }

        }
        /// <summary>
        /// 父类别选中项改变事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditParents_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string ID = this.lookUpEditParents.EditValue.ToString();
                InitlookUpEditChild();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }

        }

        private void InitInTime()
        {
            try
            {
                //默认为显示一月内的数据
                dateEditBeginInTime.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEditEndInTime.Text = DateTime.Now.ToShortDateString();
                //xll 2013-02-26
                dtZKStart.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dtZKEnd.Text = DateTime.Now.ToShortDateString();

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 绑定医生下拉列表
        /// </summary>
        private void InitDoctor()
        {
            try
            {
                lookUpWindowDoctor.SqlHelper = m_app.SqlHelper;
                string sql = string.Format(@"select u.id,u.name,u.py,u.wb from users u  ");
                DataTable Doc = m_app.SqlHelper.ExecuteDataTable(sql);

                Doc.Columns["ID"].Caption = "医生代码";
                Doc.Columns["NAME"].Caption = "医生姓名";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 70);
                cols.Add("NAME", 80);

                SqlWordbook doctorWordBook = new SqlWordbook("querybook", Doc, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDoctor.SqlWordbook = doctorWordBook;
                lookUpEditorDoctor.CodeValue = m_app.User.DoctorId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int SumPoint { get; set; }//满分值。通过配置中取得 ywk 2012年6月12日 14:43:29 
        /// <summary>
        /// 按科室查询病人信息 
        /// edit  by wangj 2013 1 30 查询条件出院时间改为入院时间
        /// </summary>
        /// <param name="p"></param>
        private void BindDataSouce()
        {
            try
            {
                //xll  r.create_time 添加质控时间
                string sql = @"select ip.patid ,
          dep.name deptname ,
          ip.name ipname ,
          ip.inwarddate  ,
          ep.createusername  ,
          ep.problem_desc  ,
          ep.doctorname  ,
          ep.noofinpat  ,
          ep.reducepoint  ,
          ep.recorddetailname  ,
          ec.childname  ,
          decode(r.qctype, 0, '环节质控', '终末质控')  qctype,
          to_char(r.create_time,'yyyy-MM-dd hh24:mi') create_time,
          di.cname
     from emr_point           ep,
          emr_configpoint     ec,
          emr_automark_record r,
          inpatient           ip,
          department          dep,
          dict_catalog  di
    where  (ep.emrpointid = ec.childcode or (ep.emrpointid like '-%' and ep.sortid=ec.ccode))
      and ep.emr_mark_record_id = r.id
      and ep.valid='1'
      and ip.noofinpat = ep.noofinpat
      and ec.valid='1'
      and dep.id = ip.outhosdept
      and di.ccode=ep.sortid
      and (dep.id = '{0}' or '{0}' is null or '{0}'='' )
      and (ip.name like '%{1}%' or '{1}' is null or '{1}'='')
      and (ip.patid like '%{2}%' or '{2}' is null or '{2}'='')
      and (r.qctype = '{3}' or '{3}' = '-1')
      and di.ccode=ep.sortid
      /*and to_char(to_date('{4}', 'yyyy-MM-dd'), 'yyyy-MM-dd') <= ip.outwarddate
      AND ip.outwarddate <=  to_char(to_date('{5}', 'yyyy-MM-dd'), 'yyyy-MM-dd')*/
      and to_char(to_date('{4}', 'yyyy-MM-dd'), 'yyyy-MM-dd') <= ip.ADMITDATE
      AND ip.ADMITDATE <=  to_char(to_date('{5}', 'yyyy-MM-dd'), 'yyyy-MM-dd')
      and (ep.sortid = '{6}' or '{6}' is null or '{6}'='')
      and (ep.emrpointid = '{7}' or '{7}' is null or '{7}'='')
      and (ep.doctorid = '{8}' or '{8}' is null or '{8}'='')
and to_date('{9}', 'yyyy-MM-dd') <= r.create_time
      AND r.create_time <=to_date('{10}', 'yyyy-MM-dd')
    order by dep.id, ip.OUTBED";
                string dept = lookUpEditorDepartment.CodeValue;
                string ipid = textEditPatID.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
                string name = textEditName.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
                string qctype = comboBox1.SelectedIndex.ToString();
                string parents = lookUpEditParents.EditValue.ToString();
                string children = lookUpEditChild.EditValue.ToString();
                string begindate = dateEditBeginInTime.Text;
                string enddate = dateEditEndInTime.DateTime.AddDays(1).ToString("yyyy-MM-dd");//.Text;
                string doctor = lookUpEditorDoctor.CodeValue;
                string zkbegindate = dtZKStart.DateTime.ToString("yyyy-MM-dd");
                string zkenddate = dtZKEnd.DateTime.AddDays(1).ToString("yyyy-MM-dd");
                sql = string.Format(sql, dept, name, ipid, qctype, begindate, enddate, parents, children, doctor, zkbegindate, zkenddate);
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 新增打印操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            gridControl1.Print();
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
                lookUpEditParents.EditValue = "";
                lookUpEditChild.EditValue = "";
                lookUpEditorDepartment.CodeValue = "";
                textEditPatID.Text = "";
                textEditName.Text = "";
                comboBox1.SelectedIndex = -1;
                lookUpEditorDoctor.CodeValue = "";
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }
    }
}
