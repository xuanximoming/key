using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public partial class UCReportBrowse : DevExpress.XtraEditors.XtraUserControl
    {
        SqlHelper m_SqlHelper;
        IEmrHost m_app;
        private UCReportCard m_ucReportCard;

        UCReportCard UCReportCard
        {
            get
            {
                if (m_ucReportCard == null)
                    m_ucReportCard = new UCReportCard(m_app);
                return m_ucReportCard;
            }
        }

        public UCReportBrowse(IEmrHost _app)
        {
            InitializeComponent();
            m_app = _app;
            AddDiseaseAnalyse();
            AddJobDisease();
        }

        private void AddDiseaseAnalyse()
        {
            UCDiseaseAnalyse ucDiseaseAnalyse = new UCDiseaseAnalyse(m_app);
            ucDiseaseAnalyse.Dock = DockStyle.Fill;
            xtraTabPageAnalyse.Controls.Add(ucDiseaseAnalyse);
        }

        private void AddJobDisease()
        {
            UCJobDisease ucJobDisease = new UCJobDisease(m_app);
            ucJobDisease.Dock = DockStyle.Fill;
            xtraTabPageJobDisease.Controls.Add(ucJobDisease);
        }

        private void UCReportBrowse_Load(object sender, EventArgs e)
        {
            InitDepartment();
            InitBzlb();

            Reset();

        }

        #region 初始化控件值

        /// <summary>
        /// 初始化科室
        /// </summary>
        private void InitDepartment()
        {

            lookUpWindowDept.SqlHelper = m_app.SqlHelper;

            string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["ID"].Caption = "科室代码";
            Dept.Columns["NAME"].Caption = "科室名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 60);
            cols.Add("NAME", 90);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDept.SqlWordbook = deptWordBook;

        }

        /// <summary>
        /// 初始传染病病种类别
        /// </summary>
        private void InitBzlb()
        {

            lookUpWindowZymosis.SqlHelper = m_app.SqlHelper;

            string sql = string.Format(@"select (case when a.level_id = 1 then '甲类传染病' when a.level_id = 2 then
                                                '乙类传染病' when a.level_id = 3 then '并类传染病'
                                                else '其他传染病' end) level_Name,a.icd,a.name,a.py,a.wb
                                          from Zymosis_Diagnosis a;");
            DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

            Bzlb.Columns["ICD"].Caption = "疾病代码";
            Bzlb.Columns["NAME"].Caption = "疾病名称";
            Bzlb.Columns["LEVEL_NAME"].Caption = "传染等级";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ICD", 60);
            cols.Add("NAME", 150);
            cols.Add("LEVEL_NAME", 90);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ICD", "NAME", cols, "ICD//Name//PY//WB//LEVEL_NAME");
            lookUpEditorZymosis.SqlWordbook = deptWordBook;

        }


        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        /// <summary>
        /// 查询事件
        /// edit by Yanqiao.Cai 2012-11-16
        /// 1、add try ... catch
        /// 2、加载性别图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search()
        {
            try
            {
                m_SqlHelper = new SqlHelper(m_app.SqlHelper);

                //report_type1,string report_type2,string recordtype1,string beginDate,string EndDate,string deptid,string diagICD,string status
                string report_type1 = chkEditFirst.Checked ? "1" : ""; //1、初次报告  
                string report_type2 = chkEditModify.Checked ? "2" : ""; //2、订正报告
                string recordtype1 = "";
                string beginDate = datebegin.DateTime.ToString("yyyy-MM-dd");
                string EndDate = dateend.DateTime.ToString("yyyy-MM-dd");
                string deptid = lookUpEditorDept.CodeValue;
                string diagICD = lookUpEditorZymosis.CodeValue;
                string status = "";
                if (chkNoAudit.Checked)
                {
                    status = "1,2,3";
                }
                else if (chkAll.Checked)
                {
                    status = "1,2,3,4,5,6,7";
                }
                else if (chkAudit.Checked)
                {
                    status = "4,5,6";
                }
                else if (chkCanel.Checked)
                {
                    status = "7";
                }

                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListXB);
                DataTable dt = m_SqlHelper.GetReportList(report_type1, report_type2, recordtype1, beginDate, EndDate, deptid, diagICD, status);
                gridControlCardList.DataSource = dt;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void xtraTabPage2Load(string reportID)
        {
            tabReportList.SelectedTabPage = tabReportInfo;

            panelReportInfo.Controls.Add(UCReportCard);

            UCReportCard.LoadPage(reportID, "1", "2");
        }

        /// <summary>
        /// 详细事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowInfo_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = gridViewCardList.FocusedRowHandle;

                if (rowIndex < 0)
                {
                    MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRowView dr = gridViewCardList.GetRow(rowIndex) as DataRowView;
                if (dr != null)
                {
                    string reportID = dr["report_id"].ToString();
                    xtraTabPage2Load(reportID);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void tabReportInfo_Resize(object sender, EventArgs e)
        {
            UCReportCard.Location = new Point((this.panelReportInfo.Width - UCReportCard.Width) / 2, this.AutoScrollPosition.Y + 15);
        }

        /// <summary>
        /// 详细页面返回报告卡列表页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnreturn_Click(object sender, EventArgs e)
        {
            tabReportList.SelectedTabPage = xtraTabPageRepList;
        }

        /// <summary>
        /// 列表页面中双击报告卡行跳转到报告卡详细页面
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、双击小标题应无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlCardList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewCardList.CalcHitInfo(gridControlCardList.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                int rowIndex = gridViewCardList.FocusedRowHandle;
                if (rowIndex >= 0)
                {
                    DataRowView dr = gridViewCardList.GetRow(rowIndex) as DataRowView;
                    if (dr != null)
                    {
                        string reportID = dr["report_id"].ToString();
                        xtraTabPage2Load(reportID);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 表格加载事件
        /// edit by Yanqiao.Cai 2012-11-13
        /// 1、add try ... catch
        /// 2、更改颜色设置，去掉背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCardList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                DataRow dr = gridViewCardList.GetDataRow(e.RowHandle) as DataRow;
                if (dr == null)
                    return;
                if (dr["STATENAME"].ToString() == "作废")
                {
                    //e.Appearance.BackColor = Color.Red; //Color.FromArgb(192, 192, 255);//设置背景色
                    //e.Appearance.BackColor = Color.LightBlue;
                    e.Appearance.ForeColor = Color.Red;
                }
                else if (dr["STATENAME"].ToString() == "审核通过")
                {
                    //e.Appearance.BackColor = Color.LightGreen;
                    //e.Appearance.BackColor = Color.LightGreen;
                    e.Appearance.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "导出Excel";
            saveFileDialog.FileName = "传染病报告统计报表";
            saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                //gridViewCardList.ExportToXls(saveFileDialog.FileName);
                gridControlCardList.ExportToXls(saveFileDialog.FileName, true);

                m_app.CustomMessageBox.MessageShow("导出成功");
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-30</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-30</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCardList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// tab页切换事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-30</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReportList_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (this.tabReportList.SelectedTabPage == xtraTabPageRepList)
                {
                    this.datebegin.Focus();
                }
                else if (this.tabReportList.SelectedTabPage == tabReportInfo)
                {
                    this.btnreturn.Focus();
                }
                else
                {
                    this.tabReportList.SelectedTabPage.Controls[0].Focus();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-01</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-01</date>
        /// <Modifyer>XLB 2013-07-02</Modifyer>
        /// </summary>
        private void Reset()
        {
            try
            {
                this.datebegin.DateTime = DateTime.Now.AddMonths(-1);
                this.dateend.DateTime = DateTime.Now;
                this.lookUpEditorDept.CodeValue = string.Empty;
                this.lookUpEditorZymosis.CodeValue = string.Empty;
                chkAll.Checked = true;
                chkEditFirst.Checked = chkEditModify.Checked = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
