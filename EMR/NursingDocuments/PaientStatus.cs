using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Core.NursingDocuments.PublicSet;
using DrectSoft.Wordbook;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Core.NursingDocuments.UserControls;
using DrectSoft.Common;
using DrectSoft.Common.Eop;
using DrectSoft.Service;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.NursingDocuments
{
    /// <summary>
    /// 用于维护病人的住院状态的操作
    /// add ywk
    /// </summary>
    public partial class PaientStatus : DevBaseForm
    {
        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;
        Inpatient m_currInpatient;
        /// <summary>
        /// 新增的病人的首页序号 add by ywk 二〇一三年五月二十八日 15:21:07  
        /// </summary>
        public string NoOfinPat { get; set; }
        /// <summary>
        /// 新增的病人的病案号码 add by ywk 二〇一三年五月二十八日 15:31:03 
        /// </summary>
        public string RecordNoofinpat { get; set; }

        public PaientStatus()
        {
            InitializeComponent();
        }

        //public PaientStatus(Inpatient currInpatient)
        //{
        //    m_currInpatient = currInpatient;
        //    InitializeComponent();
        //}

        public PaientStatus(string noofinpat, string recordNoofinpat)
        {
            NoOfinPat = noofinpat;
            RecordNoofinpat = recordNoofinpat;
            //m_currInpatient = currInpatient;
            InitializeComponent();
        }
        #region 事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaientStatus_Load(object sender, EventArgs e)
        {
            InitPainetState();
            InitSateData();
        }
        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnADD_Click(object sender, EventArgs e)
        {
            try
            {
                m_EditState = EditState.Add;
                ClearPageValue();
                BtnState();
                //新增时，选择状态的时间默认给当前时间 add by ywk  
                this.dateEdit.DateTime = DateTime.Now;
                this.txtTime.Time = Convert.ToDateTime(DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                this.dateEdit.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.dateEdit.Text) || string.IsNullOrEmpty(this.lookUpState.CodeValue))
                {
                    MethodSet.App.CustomMessageBox.MessageShow("请选择一条状态记录");
                    return;
                }
                //edit by cyq 2012-11-02 删除确认
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定删除状态 " + this.lookUpState.Text.Trim() + " 吗？", "", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                MethodSet.SaveStateData(SetEntityByPage(), "3");
                MethodSet.App.CustomMessageBox.MessageShow("删除成功");
                RefreshData();

            }
            catch (Exception)
            {
                MethodSet.App.CustomMessageBox.MessageShow("删除失败");
            }
        }
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }

                if (m_EditState == EditState.Add)
                {
                    if (CheckDiticnt())
                    { }
                    else
                    {
                        MethodSet.App.CustomMessageBox.MessageShow("不可重复增加");
                        return;
                    }
                }

                if (SaveData(SetEntityByPage()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show((m_EditState == EditState.Add ? "新增" : "修改") + "成功");
                    RefreshData();
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show((m_EditState == EditState.Add ? "新增" : "修改") + "失败");
                }
                this.btnADD.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 清空操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearPageValue();
            m_EditState = EditState.View;
            BtnState();
        }
        private void gridControlState_Click(object sender, EventArgs e)
        {
            if (gViewConfigStatus.FocusedRowHandle < 0)
                return;
            DataRow foucesRow = gViewConfigStatus.GetDataRow(gViewConfigStatus.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("id"))
                return;

            SetPageValue(SetEntityByDataRow(foucesRow));
            m_EditState = EditState.Edit;
            BtnState();
        }
        #endregion

        #region 方法
        private PatStateEntity SetEntityByDataRow(DataRow focusrow)
        {
            if (focusrow == null)
            {
                return null;
            }
            PatStateEntity patstate = new PatStateEntity();
            patstate.CCODE = focusrow["ccode"].ToString();
            patstate.DOTIME = focusrow["dotime"].ToString();
            patstate.ID = focusrow["id"].ToString();
            patstate.PATID = focusrow["patid"].ToString();
            patstate.NOOFINPAT = focusrow["noofinpat"].ToString();
            return patstate;

        }
        /// <summary>
        /// 将实体值赋给页面元素
        /// </summary>
        /// <param name="configEmrPoint"></param>
        private void SetPageValue(PatStateEntity patstateent)
        {
            if (patstateent == null)
                return;
            txtTime.Text = patstateent.DOTIME;
            //dateEdit.DateTime = patstateent.DOTIME;
            string m_time = patstateent.DOTIME;
            string date = m_time.Split(' ')[0].ToString();
            string time = m_time.Split(' ')[1].ToString();
            this.dateEdit.DateTime = Convert.ToDateTime(date.ToString());
            txtTime.Time = Convert.ToDateTime(time.ToString());
            lookUpState.CodeValue = patstateent.CCODE;
        }
        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshData()
        {
            try
            {
                InitSateData();
                m_EditState = EditState.View;
                BtnState();
                ClearPageValue();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        private bool SaveData(PatStateEntity patStateEnt)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";

                #region "段位順移(更新数据库的时间-后移一个段位) - 已弃用"
                /// <summary>
                /// 新增(或者修改)病人状态记录时，如果同时存在'入院'和'出院'记录且为同一时间段位时，
                /// 则将出院状态改至下一个时间段位
                /// </summary>
                //if (edittype == "1")
                //{
                //    if (patStateEnt.CCODE == "7003")
                //    {//出院
                //        if (CheckPatiStateDiv(null))
                //        {
                //            patStateEnt.DOTIME = GetNextPointDatetime(DateTime.Parse(patStateEnt.DOTIME)).ToString("yyyy-MM-dd HH:mm");
                //        }
                //    }
                //    else if (patStateEnt.CCODE == "7008")
                //    {//入院
                //        if (CheckPatiStateDiv(null))
                //        {
                //            UpdateToNextTimeDiv(null, patStateEnt.PATID, patStateEnt.DOTIME);
                //        }
                //    }
                //}
                //else
                //{
                //    if (patStateEnt.CCODE == "7003")
                //    {
                //        if (CheckPatiStateDiv(patStateEnt.ID))
                //        {
                //            patStateEnt.DOTIME = GetNextPointDatetime(DateTime.Parse(patStateEnt.DOTIME)).ToString("yyyy-MM-dd HH:mm");
                //        }
                //    }
                //    else if (patStateEnt.CCODE == "7008")
                //    {
                //        if (CheckPatiStateDiv(patStateEnt.ID))
                //        {
                //            UpdateToNextTimeDiv(patStateEnt.ID, patStateEnt.PATID, patStateEnt.DOTIME);
                //        }
                //    }
                //}
                #endregion

                MethodSet.SaveStateData(patStateEnt, edittype);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 取得窗体值赋给实体
        /// </summary>
        /// <returns></returns>
        private PatStateEntity SetEntityByPage()
        {
            PatStateEntity patStateEn = new PatStateEntity();

            patStateEn.CCODE = lookUpState.CodeValue;//状态编号
            patStateEn.DOTIME = this.dateEdit.DateTime.ToString("yyyy-MM-dd ") + this.txtTime.Text;
            //patStateEn.PATID = m_currInpatient.RecordNoOfHospital.ToString();
            //patStateEn.NOOFINPAT = m_currInpatient.NoOfFirstPage.ToString();
            patStateEn.PATID = RecordNoofinpat;
            patStateEn.NOOFINPAT = NoOfinPat;

            DataRow foucesRow = gViewConfigStatus.GetDataRow(gViewConfigStatus.FocusedRowHandle);
            if (foucesRow != null)
            {
                patStateEn.ID = foucesRow["id"].ToString();
            }
            return patStateEn;
        }
        /// <summary>
        /// 判断是否重复
        /// </summary>
        /// <returns></returns>
        private bool CheckDiticnt()
        {
            //            string sql = string.Format(@"select * from    PatientStatus where 
            //            noofinpat='{0}' and ccode='{1}' ", NoPatID, this.lookUpState.CodeValue);
            //            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            //            if (dt.Rows.Count > 0)
            //            {
            //                return false;
            //            }
            //            else
            //            {
            return true;
            //}
        }

        /// <summary>
        /// 保存验证
        /// </summary>
        /// <returns></returns>
        private bool IsSave()
        {
            if (this.txtTime.Text.Trim() == "")
            {
                MethodSet.App.CustomMessageBox.MessageShow("请选择时间");
                return false;
            }
            if (this.dateEdit.Text.ToString() == "")
            {
                MethodSet.App.CustomMessageBox.MessageShow("请选择日期");
                return false;
            }
            if (this.lookUpState.CodeValue == "")
            {
                MethodSet.App.CustomMessageBox.MessageShow("请选择状态");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 画面检查
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-02</date>
        /// </summary>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(this.dateEdit.Text.Trim()))
                {
                    this.dateEdit.Focus();
                    return "请选择日期";
                }
                else if (string.IsNullOrEmpty(this.txtTime.Text.Trim()))
                {
                    this.txtTime.Focus();
                    return "请选择时间";
                }
                else if (string.IsNullOrEmpty(this.lookUpState.CodeValue))
                {
                    this.lookUpState.Focus();
                    return "请选择状态";
                }
                else//add by cyq 2013-03-05 增加状态时间限制
                {
                    DateTime theTime = DateTime.Parse(this.dateEdit.DateTime.ToString("yyyy-MM-dd") + " " + this.txtTime.Time.ToString("HH:mm:ss"));
                    if (theTime > DateTime.Now)
                    {
                        this.dateEdit.Focus();
                        return "病人状态时间不能大于当前时间";
                    }
                    //获取入院记录
                    //DataTable inpatient = DS_SqlService.GetInpatientByID((int)m_currInpatient.NoOfFirstPage);
                    DataTable inpatient = DS_SqlService.GetInpatientByID(Int32.Parse(NoOfinPat));
                    if (null != inpatient && inpatient.Rows.Count > 0)
                    {//多次入院
                        if (theTime < DateTime.Parse(DateTime.Parse(inpatient.Rows[0]["inwarddate"].ToString()).ToString("yyyy-MM-dd 00:00:00")))
                        {
                            return "病人状态时间不能小于入院时间(" + DateTime.Parse(inpatient.Rows[0]["inwarddate"].ToString()).ToString("yyyy-MM-dd 00:00:00") + ")";
                        }
                        //if ((inpatient.Rows[0]["status"].ToString() == "1502" || inpatient.Rows[0]["status"].ToString() == "1503") && theTime > DateTime.Parse(inpatient.Rows[0]["outhosdate"].ToString()))
                        //{
                        //    return "病人状态时间不能大于出院时间(" + DateTime.Parse(inpatient.Rows[0]["outhosdate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + ")";
                        //}
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清空窗体所有输入的值
        /// </summary>
        private void ClearPageValue()
        {
            this.dateEdit.EditValue = "";
            this.txtTime.EditValue = "";
            this.lookUpState.EditValue = "";
        }
        /// <summary>
        /// 绑定此患者的已经填写好的状态信息
        /// </summary>
        private void InitSateData()
        {
            DataTable m_StateData = new DataTable();
            //ywk  增加入院和出院时间，筛选查询结果
            string outwarddate = string.Empty;//出院时间
            string admitdate = string.Empty;//入院时间
            DataTable dtInfo = PublicSet.MethodSet.GetPatData(NoOfinPat);
            //DataTable dtInfo = PublicSet.MethodSet.GetPatData(m_currInpatient.NoOfFirstPage.ToString());
            if (dtInfo.Rows.Count > 0)
            {
                outwarddate = dtInfo.Rows[0]["OUTWARDDATE"].ToString();
                admitdate = dtInfo.Rows[0]["INWARDDATE"].ToString();
            }
            if (string.IsNullOrEmpty(admitdate))
            {
                return;
            }
            //m_StateData = MethodSet.GetStateData(m_currInpatient.RecordNoOfHospital.ToString(), m_currInpatient.NoOfFirstPage.ToString(), outwarddate, admitdate);
            m_StateData = MethodSet.GetStateData(RecordNoofinpat, NoOfinPat, outwarddate, admitdate);
            gridControlState.DataSource = m_StateData;
        }

        /// <summary>
        /// 初始化病人的状态信息（手术，分娩，死亡等）
        /// add by ywk 2012年4月23日10:01:56
        /// </summary>
        private void InitPainetState()
        {
            lookUpWState.SqlHelper = MethodSet.App.SqlHelper;
            DataTable DTState = MethodSet.GetStates();
            DTState.Columns["ID"].Caption = "状态编码";
            DTState.Columns["NAME"].Caption = "状态名称";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("NAME", 120);
            SqlWordbook StateWordBook = new SqlWordbook("querybook", DTState, "ID", "NAME", cols);
            lookUpState.SqlWordbook = StateWordBook;

            this.txtTime.Enabled = false;
            this.dateEdit.Enabled = false;
            this.lookUpState.Enabled = false;
            this.btnSave.Enabled = false;
            this.BtnClear.Enabled = false;
        }
        /// <summary>
        ///  通过判断不同类型操作控件按钮状态
        /// </summary>
        private void BtnState()
        {
            //查看详细状态
            if (m_EditState == EditState.View)
            {
                this.btnADD.Enabled = true;
                this.btnDel.Enabled = true;
                // this.BtnEdit.Enabled = true;

                this.btnSave.Enabled = false;
                this.BtnClear.Enabled = false;

                this.txtTime.Enabled = false;
                this.dateEdit.Enabled = false;
                this.lookUpState.Enabled = false;


            }
            else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
            {
                this.btnADD.Enabled = false;
                this.btnDel.Enabled = false;
                //this.BtnEdit.Enabled = false;

                this.btnSave.Enabled = true;
                this.BtnClear.Enabled = true;

                this.txtTime.Enabled = true;
                this.lookUpState.Enabled = true;
                this.dateEdit.Enabled = true;

                this.btnADD.Enabled = true;
                this.btnDel.Enabled = true;
            }
        }

        /// <summary>
        /// 区分操作类型 
        /// </summary>
        public enum EditState
        {
            /// <summary>
            /// 无
            /// </summary>
            None = 1,
            /// <summary>
            /// 新增
            /// </summary>
            Add = 2,
            /// <summary>
            /// 编辑
            /// </summary>
            Edit = 4,
            /// <summary>
            /// 视图查看
            /// </summary>
            View = 8
        }
        #endregion

        /// <summary>
        /// 判断是否存在同一时间段位的'入院'和'出院'记录
        /// </summary>
        /// author:cyq
        /// date:2012-08-16
        /// <param name="id">id:null-新增；不为null:修改</param>
        /// <returns></returns>
        public bool CheckPatiStateDiv(string id)
        {
            bool boo = false;
            DataTable dtState = MethodSet.GetStatesByNoPatID(NoOfinPat);
            //DataTable dtState = MethodSet.GetStatesByNoPatID(m_currInpatient.Code.ToString());
            DateTime dtime = DateTime.Parse(this.dateEdit.DateTime.ToString("yyyy-MM-dd ") + this.txtTime.Text);
            DataTable timePoints = PublicSet.MethodSet.GetTimePoint();
            PatientInfo pinfo = new PatientInfo();
            string sqlWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(id))
            {
                sqlWhere += " and id not in(" + id + ") ";
            }
            if (this.lookUpState.CodeValue == "7003")
            {
                DataRow[] drState1 = dtState.Select(sqlWhere + " and ccode=7008 ");
                if (drState1.Length > 0)
                {
                    foreach (DataRow dr in drState1)
                    {
                        if (pinfo.CompareTimePoint(timePoints, DateTime.Parse(dr["dotime"].ToString()), dtime) == true)
                        {
                            boo = true;
                            break;
                        }
                    }
                }
            }
            else if (this.lookUpState.CodeValue == "7008")
            {
                DataRow[] drState2 = dtState.Select(sqlWhere + " and ccode=7003 ");
                if (drState2.Length > 0)
                {
                    foreach (DataRow dr in drState2)
                    {
                        if (pinfo.CompareTimePoint(timePoints, DateTime.Parse(dr["dotime"].ToString()), dtime) == true)
                        {
                            boo = true;
                            break;
                        }
                    }
                }
            }

            return boo;
        }

        /// <summary>
        /// 获取某一时间段位下一个段位的时间
        /// </summary>
        /// author:cyq
        /// date:2012-08-16
        /// <param name="dtime"></param>
        /// <returns></returns>
        public DateTime GetNextPointDatetime(DateTime dtime)
        {
            DataTable timePointDt = PublicSet.MethodSet.GetTimePoint();
            string newDateStr = dtime.ToString("yyyy-MM-dd");// 日期部分 yyyy-MM-dd
            string hour = dtime.Hour.ToString();

            string timePoint = string.Empty;
            int i = 0;
            foreach (DataRow dr in timePointDt.Rows)
            {
                i++;
                if (i == 1 && Convert.ToInt32(hour) <= Convert.ToInt32(dr[0]))
                {
                    //2  ----> 0 1 2
                    if (i < timePointDt.Rows.Count)
                    {
                        timePoint = timePointDt.Rows[i][0].ToString();
                    }
                    else
                    {
                        timePoint = timePointDt.Rows[0][0].ToString();
                        newDateStr = dtime.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    break;
                }
                else if (i == timePointDt.Rows.Count && Convert.ToInt32(hour) >= Convert.ToInt32(dr[0]))
                {
                    //22 ----> 22 23 24
                    if (i < timePointDt.Rows.Count)
                    {
                        timePoint = timePointDt.Rows[i][0].ToString();
                    }
                    else
                    {
                        timePoint = timePointDt.Rows[0][0].ToString();
                        newDateStr = dtime.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    break;
                }
                if (Convert.ToInt32(hour) >= (Convert.ToInt32(dr[0]) - 1) && Convert.ToInt32(hour) <= (Convert.ToInt32(dr[0]) + 2))
                {
                    //2  ----> 3 4
                    //6  ----> 5 6 7 8
                    //10 ----> 9 10 11 12
                    //14 ----> 13 14 15 16
                    //18 ----> 17 18 19 20
                    //22 ----> 21
                    if (i < timePointDt.Rows.Count)
                    {
                        timePoint = timePointDt.Rows[i][0].ToString();
                    }
                    else
                    {
                        timePoint = timePointDt.Rows[0][0].ToString();
                        newDateStr = dtime.AddDays(1).ToString("yyyy-MM-dd");
                    }
                    break;
                }
            }
            if (string.IsNullOrEmpty(timePoint))
            {
                timePoint = timePointDt.Rows[timePointDt.Rows.Count][0].ToString();
            }

            return DateTime.Parse(newDateStr + " " + (int.Parse(timePoint) < 10 ? ("0" + timePoint) : timePoint) + ":" + (dtime.Minute < 10 ? ("0" + dtime.Minute.ToString()) : dtime.Minute.ToString()));
        }

        /// <summary>
        /// 将数据表中与此对象同一段位的'出院'状态更新至下一个段位
        /// </summary>
        /// <param name="id">排除项</param>
        /// <param name="patID">首页序号</param>
        /// <param name="dtime">时间：yyyy-MM-dd HH:mm</param>
        public void UpdateToNextTimeDiv(string id, string patID, string dtime)
        {
            DataTable dtState = MethodSet.GetStatesByNoPatID(patID);

            string sqlWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(id))
            {
                sqlWhere += " and id not in(" + id + ") ";
            }
            DataRow[] drows = dtState.Select(sqlWhere + " and ccode = '7003' and dotime like '%" + DateTime.Parse(dtime).ToString("yyyy-MM-dd") + "%'");
            if (drows.Length > 0)
            {
                PatientInfo pinfo = new PatientInfo();
                foreach (DataRow dr in drows)
                {
                    PatStateEntity entity = new PatStateEntity();
                    entity.ID = dr["id"].ToString();
                    entity.NOOFINPAT = dr["noofinpat"].ToString();
                    entity.CCODE = dr["ccode"].ToString();
                    entity.PATID = dr["patid"].ToString();
                    //判断是否在同一段位
                    DataTable timePoints = PublicSet.MethodSet.GetTimePoint();
                    if (pinfo.CompareTimePoint(timePoints, DateTime.Parse(dr["dotime"].ToString()), DateTime.Parse(dtime)))
                    {
                        entity.DOTIME = GetNextPointDatetime(DateTime.Parse(dr["dotime"].ToString())).ToString("yyyy-MM-dd HH:mm");
                        MethodSet.SaveStateData(entity, "2");
                    }
                }
            }
        }

        private void dateEdit_DateTimeChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-02</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gViewConfigStatus_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-02</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}