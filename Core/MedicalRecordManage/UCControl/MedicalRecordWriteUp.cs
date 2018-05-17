using MedicalRecordManage.Object;
using MedicalRecordManage.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Service;
/// <summary>
/// <title>病例浏览自定义</title>
/// <auth>jy.zhu</auth>
/// <date>2013-06-07</date>
/// </summary>
namespace MedicalRecordManage.UCControl
{

    public partial class MedicalRecordWriteUp : DevExpress.XtraEditors.XtraUserControl
    {
        private string m_sUser;
        private int TypeNumber;//当前状态值
        private int TypeIndex;//当前索引
        private List<object> m_listObject = null;
        public DataTable User_Table = null;//当前数据源
        //到期提醒期限
        public int m_iRemindtime = 0;
        DevExpress.Utils.WaitDialogForm m_WaitDialog = null;
        public MedicalRecordWriteUp()
        {
            InitializeComponent();
            m_sUser = ComponentCommand.GetCurrentDoctor();//获取登录用户信息
            Init();
        }

        public MedicalRecordWriteUp(int index, int Type, DataTable tables)
        {
            InitializeComponent();
            TypeNumber = Type;
            TypeIndex = index;
            m_sUser = ComponentCommand.GetCurrentDoctor();//获取登录用户信息    
            User_Table = tables;
            Init();


        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            try
            {
                InitializeParameter();
                InitializeGrid();
                InitializaTime();
                GetInitDB(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializaTime()
        {
            try
            {
                m_iRemindtime = ComponentCommand.GetRemindTime();
            }
            catch (Exception)
            {
                m_iRemindtime = 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeParameter()
        {
            try
            {
                this.dateStart.DateTime = System.DateTime.Today.AddMonths(-6);
                this.dateEnd.DateTime = System.DateTime.Today;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeGrid()
        {
            try
            {
                for (int i = 0; i < dbGridView.Columns.Count; i++)
                {
                    dbGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public delegate void MyControlWriteupEventHandler(object sender, MyControlWriteupEventArgs args);
        public event MyControlWriteupEventHandler OnButtonClick;

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckDate())
                {
                    m_WaitDialog = new DevExpress.Utils.WaitDialogForm("正在查询数据...", "请稍等");
                    GetInitDB(true);
                    MyControlWriteupEventArgs retvals = new MyControlWriteupEventArgs(true, this.User_Table.Rows.Count.ToString(), this);
                    if (OnButtonClick != null)
                        OnButtonClick(this, retvals);
                    m_WaitDialog.Hide();

                    m_WaitDialog.Close();
                }
            }
            catch (Exception ex)
            {
                m_WaitDialog.Close();
                MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 校验日期方法
        /// </summary>
        /// <returns></returns>
        private bool CheckDate()
        {
            try
            {
                if (this.dateStart.DateTime.ToString().Trim() != "" && this.dateEnd.DateTime.ToString() != "")
                {
                    if (this.dateStart.DateTime > this.dateEnd.DateTime)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("申请起始日期不能大于结束日期", "信息提示");
                        dateStart.Focus();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刷新事件
        /// MOdify by xlb 2013-05-29
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.dateStart.DateTime = System.DateTime.Today.AddMonths(-1);
                this.dateEnd.DateTime = System.DateTime.Today;
                this.dbGrid.DataSource = null;
                this.dateStart.Focus();
                GetInitDB(true);
                MyControlWriteupEventArgs retvals = new MyControlWriteupEventArgs(true, this.User_Table.Rows.Count.ToString(), this);
                if (OnButtonClick != null)
                    OnButtonClick(this, retvals);

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                //申请
                OpenCommandDialog(-1);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        private void OpenCommandDialog(int flag)
        {
            try
            {
                int refresh = 0;
                int tabIndex = 0;
                if (flag < 0)
                {
                    MedicalRecordApplyBack medicalRecordApply = new MedicalRecordApplyBack();
                    medicalRecordApply.ShowDialog();
                    refresh = medicalRecordApply.m_iCommandFlag;
                    tabIndex = medicalRecordApply.m_iTabIndex;
                }
                else
                {
                    InitializeApplyList();
                    if (this.m_listObject.Count > 0)
                    {
                        CApplyObject obj = (CApplyObject)m_listObject[0];
                        MedicalRecordApplyBackEditAndDelay medicalRecordApply = new MedicalRecordApplyBackEditAndDelay(obj, flag);
                        medicalRecordApply.ShowDialog();
                        refresh = medicalRecordApply.m_iCommandFlag;
                        tabIndex = medicalRecordApply.m_iTabIndex;
                    }
                }
                //刷新数据
                if (refresh > 0)
                {
                    MedicalRecordWriteUpForm info = this.FindForm() as MedicalRecordWriteUpForm;
                    foreach (RecordWriteUpModel rd in info.listAll)
                    {
                        if (rd.Index == tabIndex)
                        {
                            if (rd.MyControl == null)
                            {
                                info.UserControlDB(tabIndex);
                            }
                            rd.MyControl.GetInitDB(true);
                            rd.MyDataTable = rd.MyControl.User_Table;
                            rd.Count = rd.MyControl.User_Table.Rows.Count;
                            info.RefreshInformation(rd.Index, rd.Count);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeApplyList()
        {
            try
            {
                m_listObject.Clear();
                int[] rows = dbGridView.GetSelectedRows();
                for (int i = 0; i < rows.Length; i++)
                {
                    CApplyObject m_applyObject = new CApplyObject();
                    m_applyObject.Clear();
                    m_applyObject.m_sNoOfInpat = dbGridView.GetRowCellValue(rows[i], "NOOFINPAT").ToString();
                    m_applyObject.m_sName = dbGridView.GetRowCellValue(rows[i], "NAME").ToString();
                    m_applyObject.m_sDepartmentName = dbGridView.GetRowCellValue(rows[i], "CYKS").ToString();

                    m_applyObject.m_iApplyTimes = int.Parse(dbGridView.GetRowCellValue(rows[i], "APPLYTIMES").ToString());
                    m_applyObject.m_sApplyContent = dbGridView.GetRowCellValue(rows[i], "APPLYCONTENT").ToString();
                    m_applyObject.m_sApply = dbGridView.GetRowCellValue(rows[i], "ID").ToString();
                    m_applyObject.m_sApplyDocId = this.m_sUser;

                    m_applyObject.m_sApproveContent = dbGridView.GetRowCellValue(rows[i], "APPROVECONTENT").ToString();
                    m_applyObject.m_sApproveDate = dbGridView.GetRowCellValue(rows[i], "APPROVEDATE").ToString();
                    m_applyObject.m_sApproveDocId = dbGridView.GetRowCellValue(rows[i], "APPROVEDOCID").ToString();
                    m_listObject.Add(m_applyObject);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
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
        /// 日期切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
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

        private void dbGrid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ShowEmrMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void GetInitDB(bool IsUpdate)
        {
            if (IsUpdate)
            {
                //add by zjy 2013-6-14
                string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                string sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_WriteUp_SQL;
                if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                {
                    sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_WriteUp_SQL_ZY;
                }

                //string sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_BORROW_SQL;
                List<DbParameter> sqlParams = new List<DbParameter>();

                sql = sql + " and i.applydocid = @doc";
                SqlParameter param1 = new SqlParameter("@doc", SqlDbType.VarChar);
                param1.Value = m_sUser;
                sqlParams.Add(param1);
                if (this.dateStart.Text.Trim() != null && this.dateStart.Text.Trim() != "")
                {
                    string ds = this.dateStart.DateTime.ToString("yyyy-MM-dd");
                    sql = sql + " and  to_char(i.applydate,'yyyy-mm-dd') >= @ds";
                    SqlParameter param2 = new SqlParameter("@ds", SqlDbType.VarChar);
                    param2.Value = ds;
                    sqlParams.Add(param2);
                }
                if (this.dateEnd.Text.Trim() != null && this.dateEnd.Text.Trim() != "")
                {
                    string de = this.dateEnd.DateTime.ToString("yyyy-MM-dd");
                    sql = sql + " and  to_char(i.applydate,'yyyy-mm-dd') <= @de";
                    SqlParameter param3 = new SqlParameter("@de", SqlDbType.VarChar);
                    param3.Value = de;
                    sqlParams.Add(param3);
                }
                if (TypeNumber >= 0)
                {
                    sql = sql + " and i.status = @st";
                    SqlParameter param4 = new SqlParameter("@st", SqlDbType.Int);
                    param4.Value = TypeNumber;
                    sqlParams.Add(param4);
                }
                DataTable dataTable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
                //压缩DateSet中的记录，将住院诊断信息合并
                ComponentCommand.ImpressDataSet(ref dataTable, "id", "cyzd");
                //填入状态值
                ComponentCommand.InitializeStatusInfo(ref dataTable);
                //控制状态显示和预警信息显示颜色显示


                User_Table = dataTable;
            }
            this.dbGrid.DataSource = User_Table;
            if (TypeIndex == 4)
            {
                this.dbGridView.Columns["STATUSDES"].Visible = true;
            }
            else
            {
                this.dbGridView.Columns["STATUSDES"].Visible = false;
            }



            //审核日期
            if (TypeIndex == 4 || TypeIndex == 1 || TypeIndex == 3)
            {
                this.dbGridView.Columns["SHSJ"].Visible = true;
            }
            else
            {
                this.dbGridView.Columns["SHSJ"].Visible = false;
            }
        }

        /// <summary>
        /// 显示病历信息方法
        /// </summary>
        private void ShowEmrMessage()
        {
            try
            {
                int[] list = dbGridView.GetSelectedRows();
                if (list.Length > 0)
                {
                    string status = dbGridView.GetRowCellValue(list[0], "STATUS").ToString();
                    //调用病历查看窗口,进行病历的查询
                    if (status.Equals("2"))
                    {
                        string noofinpat = dbGridView.GetRowCellValue(list[0], "NOOFINPAT").ToString();
                        // LoadEmrContent(noofinpat);

                        //  LoadEmrContent(noofinpat);
                        decimal syxh = decimal.Parse(noofinpat);
                        if (syxh < 0) return;
                        if (syxh < 0) return;

                        if (HasBaby(noofinpat))
                        {
                            ChoosePatOrBaby choosepat = new ChoosePatOrBaby(SqlUtil.App, noofinpat);
                            choosepat.StartPosition = FormStartPosition.CenterParent;
                            if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                SqlUtil.App.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                                SqlUtil.App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                            }
                        }
                        else
                        {
                            SqlUtil.App.ChoosePatient(syxh);
                            SqlUtil.App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool HasBaby(string noofinpat)
        {
            string sql = string.Format(@"select babycount from inpatient where noofinpat='{0}'", noofinpat);
            DataTable dt = SqlUtil.App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["babycount"].ToString() == "0")
                {
                    return false;
                }
                if (Int32.Parse(dt.Rows[0]["babycount"].ToString() == "" ?
                    "0" : dt.Rows[0]["babycount"].ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 加载病历
        /// </summary>
        /// <param name="noofinpat"></param>
        private void LoadEmrContent(string noofinpat)
        {
            try
            {
                EmrBrowerDlg frm = new EmrBrowerDlg(noofinpat, SqlUtil.App, FloderState.None);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void dbGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                int i = TypeNumber;
                if (e.CellValue == null)
                {
                    return;
                }
                DataRowView drv = dbGridView.GetRow(e.RowHandle) as DataRowView;
                if (i == 2 || i == 5)
                {
                    //取得病人名字
                    string val = drv["APPROVEDATE"].ToString().Trim();
                    string status = drv["STATUS"].ToString().Trim();
                    string times = drv["APPLYTIMES"].ToString().Trim();
                    if (val != null && val != "" && status.Equals("2"))
                    {
                        DateTime date = DateTime.Parse(val);
                        if (System.DateTime.Today.Date.CompareTo(date.AddDays(int.Parse(times) - this.m_iRemindtime).Date) >= 0)
                        {
                            e.Appearance.ForeColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dbGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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



    }
    /// <summary>
    /// 定义实体，保存委托参数
    /// </summary>
    public class MyControlWriteupEventArgs : EventArgs
    {
        private string _Result;
        private bool _IsOK;
        private MedicalRecordWriteUp _medicalRecordBrowse;
        public MyControlWriteupEventArgs(bool isOK, string res, MedicalRecordWriteUp medicalRecordBrowse)
        {
            _IsOK = isOK;
            _Result = res;
            _medicalRecordBrowse = medicalRecordBrowse;
        }

        public MedicalRecordWriteUp MyMedicalRecordBrowse
        {
            get { return _medicalRecordBrowse; }
            set { _medicalRecordBrowse = value; }
        }


        public string Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        public bool IsOK
        {
            get { return _IsOK; }
            set { _IsOK = value; }
        }
    }
}
