using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork;
using DrectSoft.Wordbook;
using DevExpress.Utils;

namespace DrectSoft.Emr.QcManager
{
    public partial class OutMedicalScore : UserControl, IStartPlugIn
    {
        IEmrHost m_app;
        SqlManger m_SqlManager;

        private string m_beginInTime = string.Empty;
        private string m_endInTime = string.Empty;
        private string m_patID = string.Empty;
        private string m_name = string.Empty;
        private string m_status = string.Empty;
        /// <summary>
        /// 患者病历评分列表页面
        /// </summary>
        public OutMedicalScore(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_SqlManager = new SqlManger(app);
        }
        private WaitDialogForm m_WaitDialog;

        #region 实现接口部分
        public IPlugIn Run(IEmrHost host)
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
            InitDepartment();
            InitStatus();
            InitInTime();
            SetlookUpEditorDepartmentEable();

        }

        //设置科室的禁用与否
        private void SetlookUpEditorDepartmentEable()
        {
            string Uidentity = m_SqlManager.JudgeIdentity(m_app.User.Id, m_SqlManager);//判断当前登录的人是科室质控员还是质控科的
            if (Uidentity == "QCDepart")
            {
                lookUpEditorDepartment.Enabled = true;
                gridColumnRYZD.Visible = false;
                gridColumnKouFenRE.Visible = true;

            }
            else
            {
                lookUpEditorDepartment.Enabled = false;
                gridColumnRYZD.Visible = true;
                gridColumnKouFenRE.Visible = false;
            }

        }

        private void InitlookUpEditorValue()
        {
            lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;
        }

        /// <summary>
        /// 双击进入该病人的评分信息页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
           string shenfeng= m_SqlManager.JudgeIdentity(m_app.User.Id, m_SqlManager);
           if (shenfeng == "QCDepart")
               return;

            DataRow focuseRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (focuseRow == null)
                return;
            EmrPainetScore emrpointScore = new EmrPainetScore(m_app, focuseRow);
            SetWaitDialogCaption("正在加载病人评分表...");
            emrpointScore.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
            emrpointScore.ShowDialog();
            HideWaitDialog();
            //更改为调出报表页面

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
            dateEditBeginInTime.EditValue = System.DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            dateEditEndInTime.EditValue = System.DateTime.Now.ToString("yyyy-MM-dd");
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

            cols.Add("ID", 65);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name");
            lookUpEditorStatus.SqlWordbook = deptWordBook;
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

            cols.Add("ID", 65);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDepartment.SqlWordbook = deptWordBook;

            //lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;

            InitlookUpEditorValue();
        }
        private int SumPoint { get; set; }//满分值。通过配置中取得 ywk 2012年6月12日 14:43:29 
        /// <summary>
        /// 按科室查询病人信息 
        /// </summary>
        /// <param name="p"></param>
        private void BindDataSouce(string deptid)
        {
            //新加个参数，计算总分
            //SumPoint = Int32.Parse(m_SqlManager.GetConfigValueByKey("EmrPointConfig"));
            //SumPoint = m_SqlManager.GetSumPoint(m_NoOfInpat, m_app);

            m_patID = textEditPatID.Text.Trim();
            m_name = textEditName.Text.Trim();
            m_status = lookUpEditorStatus.CodeValue.Trim();

            m_beginInTime = Convert.ToDateTime(dateEditBeginInTime.EditValue).ToString("yyyy-MM-dd HH:mm:ss").Trim();
            m_endInTime = Convert.ToDateTime(dateEditEndInTime.EditValue).ToString("yyyy-MM-dd HH:mm:ss").Trim();
            DataTable dt = m_SqlManager.GetPatientPinFen(deptid, m_patID, m_name, m_status, m_beginInTime, m_endInTime);

            DataColumn colREDPOINT = new DataColumn();
            colREDPOINT.ColumnName = "REDPOINT";
            DataColumn colKOUFENLIYOU = new DataColumn();
            colKOUFENLIYOU.ColumnName = "KOUFENLIYOU";
            dt.Columns.Add(colREDPOINT);
            dt.Columns.Add(colKOUFENLIYOU);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //先在此处给空值，后面就要根据病人的Noofinpat来进行向相应的位置填值
                dt.Rows[i]["REDPOINT"] = "";
                dt.Rows[i]["KOUFENLIYOU"] = "";
            }

            DataTable newtable = OperatePatPoint(dt);
            gridControl1.DataSource = newtable;
        }

        private DataTable OperatePatPoint(DataTable Oldtable)
        {
            DataTable table = Oldtable.Copy();//最终要处理此Table
            for (int i = 0; i < table.Rows.Count; i++)
            {
                string status = table.Rows[i]["status"].ToString(); //病人状态
                string rhqcId = table.Rows[i]["id"].ToString();   //评分表id
                int points = 0;
                try
                {
                    points = Convert.ToInt32(table.Rows[i]["totalscore"].ToString());
                }
                catch (Exception)
                {
                }
                if (status == "1501")
                {
                    table.Rows[i]["REDPOINT"] = (85 - points).ToString();
                }
                else
                { table.Rows[i]["REDPOINT"] = (100 - points).ToString(); }
                string strSql = string.Format(@"select * from emr_rhpoint where rhqc_table_id='{0}' and valid='1'", rhqcId);
               DataTable dtPoint= m_app.SqlHelper.ExecuteDataTable(strSql);
               string strKouFen = "";
               for (int j = 0; j < dtPoint.Rows.Count; j++)
               {
                  strKouFen+= dtPoint.Rows[j]["problem_desc"].ToString()+"\r\n";
               }
               table.Rows[i]["KOUFENLIYOU"] = strKouFen;
            }
            return table;
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
            saveFileDialog.Title = "导出Excel";
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

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

    }
}
