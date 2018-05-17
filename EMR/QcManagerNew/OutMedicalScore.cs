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
    public partial class OutMedicalScore : UserControl, IStartPlugIn
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
        public OutMedicalScore(IYidanEmrHost app)
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
            InitDepartment();//绑定科室下拉框 
            InitStatus();//绑定病人状态
            InitInTime();//绑定时间

            InitPointClass();//初始化评分类别下拉框 add by ywk 2012年10月25日 11:19:33
            InitQCManager();//add by wyt 2012-12-12
            //this.lookUpEditorDepartment.Focus();
        }
        /// <summary>
        /// 初始化评分类别下拉框 
        /// add by ywk 2012年10月25日 11:19:51 
        /// </summary>
        private void InitPointClass()
        {
            lookUpWindowCCNAME.SqlHelper = m_app.SqlHelper;

            SqlManger sqlManager = new SqlManger(m_app);
            DataTable DtParent = sqlManager.GetParentClass();
            DtParent.Columns["CCODE"].Caption = "分类代码";
            DtParent.Columns["CNAME"].Caption = "分类名称";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("CCODE", 50);
            cols.Add("CNAME", 100);
            SqlWordbook deptWordBook = new SqlWordbook("querybook", DtParent, "CCODE", "CNAME", cols);
            lookUpCCNAME.SqlWordbook = deptWordBook;
        }

        private void InitlookUpEditorValue()
        {
            lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;
            //lookUpEditorDepartment.CodeValue = "2401";
        }

        /// <summary>
        /// 根据用户质控权限初始化窗口 add by wyt 2012-12-12
        /// </summary>
        private void InitQCManager()
        {
            try
            {
                bool haveRole = false;
                m_qcAuth = Authority.DEPTQC;  //默认科室质控员
                lookUpEditorDepartment.Enabled = false;
                lookUpEditorStatus.Enabled = false;
                string deptid = m_app.User.CurrentDeptId;
                string userid = m_app.User.DoctorId;
                //质控科质控员
                string configvalue = m_SqlManager.GetConfigValueByKey("ShowAllDeptQuality");
                string c_UserJobId = m_app.User.GWCodes;                //当前登录人的jobid标识
                string[] userJobid = c_UserJobId.Split(',');
                if (!string.IsNullOrEmpty(configvalue))
                {
                    if (configvalue.Contains(","))                      //配置了多个角色可查看
                    {
                        string[] configjobid = configvalue.Split(',');  //配置里的多个角色jobid
                        for (int i = 0; i < configjobid.Length; i++)    //先循环配置里所有jobid
                        {
                            if (haveRole == true)
                            {
                                break;
                            }
                            for (int j = 0; j < userJobid.Length; j++)  //再循环登录人的多个jobid
                            {
                                if (configjobid[i] == userJobid[j])
                                {
                                    m_qcAuth = Authority.QC;
                                    haveRole = true;
                                    lookUpEditorDepartment.Enabled = true;
                                    lookUpEditorStatus.Enabled = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (string item in userJobid)//取出
                        {
                            if (item == configvalue)//当前登录人的jobid在系统配置中,可以查看全院质控
                            {
                                m_qcAuth = Authority.QC;
                                haveRole = true;
                                lookUpEditorDepartment.Enabled = true;
                                lookUpEditorStatus.Enabled = true;
                                break;
                            }
                        }
                    }
                }
                if (haveRole == false)
                {
                    DataTable deptmanager = m_SqlManager.GetDirectorDoc(deptid);
                    foreach (DataRow dr in deptmanager.Rows)
                    {
                        if (dr["ID"].ToString() == userid)
                        {
                            m_qcAuth = Authority.DEPTMANAGER;
                            lookUpEditorDepartment.Enabled = false;
                            lookUpEditorStatus.Enabled = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
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
                EmrPainetScore emrpointScore = new EmrPainetScore(m_app, noofpaint, sumpoint);
                SetWaitDialogCaption("正在加载病人评分表...");
                emrpointScore.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
                emrpointScore.ShowDialog();
                HideWaitDialog();
                //更改为调出报表页面
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
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }

        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }

        /// <summary>
        /// 按科室查询病人信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            BindDataSouce(this.lookUpEditorDepartment.CodeValue);
        }
        #endregion

        #region 方法
        /// <summary>
        /// 绑定时间
        /// </summary>
        private void InitInTime()
        {
            //默认为显示一月内的数据
            dateEditBeginInTime.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEditEndInTime.Text = DateTime.Now.ToShortDateString();
        }

        /// <summary>
        /// 绑定病人状态下拉框
        /// </summary>
        private void InitStatus()
        {
            lookUpWindowStatus.SqlHelper = m_app.SqlHelper;

            string sql = string.Format(@"select c.id, c.name from categorydetail c 
                                         where c.categoryid = '15' and c.id in 
                                         (select distinct status from inpatient)
                                         ");
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["ID"].Caption = "状态代码";
            Dept.Columns["NAME"].Caption = "状态名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 70);
            cols.Add("NAME", 80);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name");
            lookUpEditorStatus.SqlWordbook = deptWordBook;

            lookUpEditorStatus.CodeValue = "1503";
        }
        /// <summary>
        /// 绑定科室下拉列表
        /// </summary>
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

            InitlookUpEditorValue();
            lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;
        }
        private int SumPoint { get; set; }//满分值。通过配置中取得 ywk 2012年6月12日 14:43:29 
        /// <summary>
        /// 按科室查询病人信息 
        /// </summary>
        /// <param name="p"></param>
        private void BindDataSouce(string deptid)
        {
            try
            {
                //新加个参数，计算总分
                //SumPoint = Int32.Parse(m_SqlManager.GetConfigValueByKey("EmrPointConfig"));
                string point = m_SqlManager.GetConfigValueByKey("EmrPointConfig");
                int sumpoint1 = 85;
                int sumpoint2 = 100;

                if (point.Contains(","))
                {
                    string[] points = point.Split(',');
                    sumpoint1 = Int32.Parse(points[0]);
                    sumpoint2 = Int32.Parse(points[1]);
                }
                m_patID = textEditPatID.Text.Trim();
                m_name = textEditName.Text.Trim();
                m_status = lookUpEditorStatus.CodeValue.Trim();
                m_sortid = lookUpCCNAME.CodeValue;
                m_beginInTime = Convert.ToDateTime(dateEditBeginInTime.EditValue).ToString("yyyy-MM-dd HH:mm:ss").Trim();
                m_endInTime = Convert.ToDateTime(dateEditEndInTime.EditValue).ToString("yyyy-MM-dd HH:mm:ss").Trim();

                DataTable dt = m_SqlManager.GetDepartmentPatStatInfo(deptid, m_patID, m_name, m_status, m_beginInTime, m_endInTime, m_sortid, sumpoint1, sumpoint2, "point", m_userid, m_qcAuth);

                //DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_QueryWardDetailInfo, deptid));
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
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        /// <summary>
        /// /重置按钮清空搜索条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            InitInTime();
            InitlookUpEditorValue();
            this.lookUpEditorStatus.CodeValue = "";
            this.textEditName.Text = "";
            this.textEditPatID.Text = "";
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

        private void gridView1_Click(object sender, EventArgs e)
        {
            int index = this.gridView1.FocusedRowHandle;
            //MessageBox.Show(index.ToString());
        }
    }
}
