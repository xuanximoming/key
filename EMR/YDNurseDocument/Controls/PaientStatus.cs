using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.NurseDocument.Controls
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
        private string m_InTime = "";
        private string m_outTime = "";
        public PaientStatus()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 病案首页序号
        /// </summary>
        public string NoPatID;
        public string NoOfFirstPage;
        public PaientStatus(string patid, string _m_InTime, string _m_outTime, string noOfFirstPage)
        {
            InitializeComponent();
            NoPatID = patid;
            m_InTime = _m_InTime;
            m_outTime = _m_outTime;
            NoOfFirstPage = noOfFirstPage;

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
            BtnState();
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
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
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
                    MethodSet.App.CustomMessageBox.MessageShow("请选择要删除的状态");
                    return;
                }
                MethodSet.SaveStateData(SetEntityByPage(), "3");
                MessageBox.Show("删除成功！");
                RefreshData();

            }
            catch (Exception)
            {
                MessageBox.Show("删除失败！");
            }
        }
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsSave())
                return;

            if (m_EditState == EditState.Add)
            {
                if (CheckDiticnt())
                { }
                else
                {
                    MessageBox.Show("不可重复增加！");
                    return;
                }
            }



            if (SaveData(SetEntityByPage()))
            {
                if (m_EditState == EditState.Add)
                {
                    MessageBox.Show("新增成功！");
                }
                else
                {
                    MessageBox.Show("修改成功！");
                }
                RefreshData();
            }
            else
            {
                if (m_EditState == EditState.Add)
                {
                    MessageBox.Show("新增失败！");
                }
                else
                {
                    MessageBox.Show("修改失败！");
                }
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
            patstate.PATID = focusrow["noofinpat"].ToString();
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
            InitSateData();
            m_EditState = EditState.View;
            BtnState();
            ClearPageValue();
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
            patStateEn.PATID = NoPatID;
            patStateEn.Noofpat = NoOfFirstPage;
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
        /// Moify by xlb 中心需求 从入科时间零点开始掐控新增状态的时间
        /// </summary>
        /// <returns></returns>
        private bool IsSave()
        {
            if (this.txtTime.Text.Trim() == "")
            {
                MessageBox.Show("请选择时间！");
                return false;
            }
            if (this.dateEdit.Text.ToString() == "")
            {
                MessageBox.Show("请选择日期！");
                return false;
            }
            if (DateTime.Parse(DateTime.Parse(m_InTime).ToString("yyyy-MM-dd 00:00:00")) > DateTime.Parse(dateEdit.DateTime.ToString("yyyy-MM-dd") + " " + txtTime.Text + ":59"))
            {
                MessageBox.Show("新增状态日期应该大于病人入区日期当天的零点");
                return false;
            }
            if (this.lookUpState.CodeValue == "")
            {
                MessageBox.Show("请选择状态！");
                return false;
            }
            else
            {
                return true;
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
            this.lookUpState.CodeValue = string.Empty;//zyx 2013-1-5
        }
        /// <summary>
        /// 绑定此患者的已经填写好的状态信息
        /// </summary>
        private void InitSateData()
        {
            DataTable m_StateData = new DataTable();
            m_StateData = MethodSet.GetStateData(NoOfFirstPage);
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
            else if (m_EditState == EditState.None)
            {
                btnADD.Enabled = true;
                btnDel.Enabled = true;

                btnSave.Enabled = false;
                BtnClear.Enabled = true;
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
        /// 序号
        /// <auth>zyx</auth>
        /// <date>2013-1-5</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gViewConfigStatus_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Header)
                {
                    e.Info.DisplayText = "序号";
                }
                else if (e.Info.Kind == DevExpress.Utils.Drawing.IndicatorKind.Row)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }




    }
}