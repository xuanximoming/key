using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Core.Consultation.Dal;
using YidanSoft.Wordbook;
using YidanSoft.Common.Library;
using YidanSoft.Common.Eop;

namespace YidanSoft.Core.Consultation
{
    public partial class UCConsultation : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        ConsultationEntity m_ConsultationEntity;

        private string m_ConsultationID = "";

        IYidanEmrHost m_app;
        string m_ConsultTypeID = "";
        /// <summary>
        /// 多科会诊页面
        /// </summary>
        UCConsultationMultiply m_UCConsultationMultiply;
        Inpatient CurrentInpatient;

        public UCConsultation(string noofinpat, string consultationID)
            : this(consultationID)
        {
            CurrentNoofinpat = noofinpat;
        }

        public UCConsultation(string ConsultationID)
        {
            InitializeComponent();
            m_ConsultationID = ConsultationID;

        }

        public UCConsultation()
        {
            InitializeComponent();
            m_ConsultationID = "";
        }

        private void GetConsultationEntity()
        {
            if (m_ConsultationID == "")
                return;
            else
            {
                DataTable dt = DataAccess.GetConsultationTable(m_ConsultationID);
                DataTable dtinp = Dal.DataAccess.GetRedactPatientInfoFrm("14", CurrentInpatient.NoOfFirstPage.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    m_ConsultationEntity = new ConsultationEntity();

                    m_ConsultationEntity.ConsultApplySn = m_ConsultationID;
                    m_ConsultationEntity.NoOfInpat = CurrentInpatient.NoOfFirstPage.ToString();


                    if (dtinp.Rows.Count > 0)
                    {
                        m_ConsultationEntity.Name = dtinp.Rows[0]["NAME"].ToString().Trim();
                        m_ConsultationEntity.PatNoOfHIS = dtinp.Rows[0]["PatID"].ToString().Trim();
                        m_ConsultationEntity.SexName = dtinp.Rows[0]["Gender"].ToString().Trim();
                        m_ConsultationEntity.Age = dtinp.Rows[0]["AgeStr"].ToString().Trim();
                        m_ConsultationEntity.Bed = dtinp.Rows[0]["OutBed"].ToString().Trim();
                        m_ConsultationEntity.DeptName = dtinp.Rows[0]["OutHosDeptName"].ToString().Trim();
                        m_ConsultationEntity.WardName = dtinp.Rows[0]["outhoswardname"].ToString().Trim();
                        m_ConsultationEntity.DeptID = dtinp.Rows[0]["OutHosDept"].ToString().Trim();
                        m_ConsultationEntity.WardID = dtinp.Rows[0]["outhosward"].ToString().Trim();
                        //textEditMarriage.Text = dtinp.Rows[0]["Marriage"].ToString().Trim();
                        //textEditJob.Text = dtinp.Rows[0]["JobName"].ToString().Trim();
                    }
                    m_ConsultTypeID = dr["consulttypeid"].ToString();

                    m_ConsultationEntity.UrgencyTypeID = dr["urgencytypeid"].ToString();
                    m_ConsultationEntity.UrgencyTypeName = dr["urgencytypeName"].ToString();
                    m_ConsultationEntity.ConsultTypeID = dr["consulttypeid"].ToString();
                    m_ConsultationEntity.ConsultTypeName = dr["consulttypeName"].ToString();
                    m_ConsultationEntity.Abstract = dr["abstract"].ToString();

                    m_ConsultationEntity.Purpose = dr["purpose"].ToString();
                    m_ConsultationEntity.ApplyDeptID = dr["ApplyDeptID"].ToString();
                    m_ConsultationEntity.ApplyDeptName = dr["ApplyDeptName"].ToString();
                    m_ConsultationEntity.ApplyUserID = dr["applyuserID"].ToString();
                    m_ConsultationEntity.ApplyUserName = dr["applyuserName"].ToString();

                    m_ConsultationEntity.ApplyTime = dr["applytime"].ToString();
                    m_ConsultationEntity.ConsultSuggestion = dr["consultsuggestion"].ToString();
                    m_ConsultationEntity.ConsultDeptID = dr["ConsultDeptID"].ToString();
                    m_ConsultationEntity.ConsultDeptName = dr["ConsultDeptName"].ToString();
                    m_ConsultationEntity.ConsultHospitalID = dr["hospitalcode"].ToString();

                    m_ConsultationEntity.ConsultHospitalName = dr["ConsultHospitalName"].ToString();
                    m_ConsultationEntity.ConsultDeptID2 = dr["ConsultDeptID2"].ToString();
                    m_ConsultationEntity.ConsultDeptName2 = dr["ConsultDeptName2"].ToString();
                    m_ConsultationEntity.ConsultUserID = dr["ConsultUserID"].ToString();
                    m_ConsultationEntity.ConsultUserName = dr["ConsultUserName"].ToString();

                    m_ConsultationEntity.ConsultTime = dr["ConsultTime"].ToString();
                    m_ConsultationEntity.StateID = dr["StateID"].ToString();
                    return;
                }
            }
        }

        private void FillUI()
        {
            if (m_ConsultationEntity == null || m_ConsultationEntity.ConsultApplySn == "")
            {
                memoConsultSuggestion.Properties.ReadOnly = true;
                lookUpEditorHospital.Enabled = false;
                lookUpEditorDepartment.Enabled = false;
                lookUpEditorEmployee.Enabled = false;

                dateEditConsultationDate.Enabled = false;
                timeEditConsultationTime.Enabled = false;
                return;
            }

            else
            {
                txtName.Text = m_ConsultationEntity.Name;
                txtSex.Text = m_ConsultationEntity.SexName;
                txtAge.Text = m_ConsultationEntity.Age;
                txtDept.Text = m_ConsultationEntity.DeptName;
                txtWard.Text = m_ConsultationEntity.WardName;

                txtBed.Text = m_ConsultationEntity.Bed;
                txtPatNoOfHis.Text = m_ConsultationEntity.PatNoOfHIS;

                memoAbstract.Text = m_ConsultationEntity.Abstract;
                memoPurpose.Text = m_ConsultationEntity.Purpose;


                txtConsultDept.Text = m_ConsultationEntity.ConsultDeptName;
                txtApplyDept.Text = m_ConsultationEntity.ApplyDeptName;
                txtApplyUser.Text = m_ConsultationEntity.ApplyUserName;
                txtApplyTime.Text = m_ConsultationEntity.ApplyTime;

                if (m_ConsultationEntity.StateID == "6741" || m_ConsultationEntity.StateID == "6740")
                {
                    memoConsultSuggestion.Text = m_ConsultationEntity.ConsultSuggestion;
                    //lookUpEditorHospital.Text = m_ConsultationEntity.ConsultHospitalName;
                    lookUpEditorHospital.CodeValue = m_ConsultationEntity.ConsultHospitalID;
                    //lookUpEditorDepartment.Text = m_ConsultationEntity.ConsultDeptName2;
                    lookUpEditorDepartment.CodeValue = m_ConsultationEntity.ConsultDeptID2;
                    //lookUpEditorEmployee.Text = m_ConsultationEntity.ConsultUserName;
                    lookUpEditorEmployee.CodeValue = m_ConsultationEntity.ConsultUserID;
                    //txtConsultTime.Text = m_ConsultationEntity.ConsultTime;

                    if (m_ConsultationEntity.ConsultTime.Trim().Split(' ').Length == 2)
                    {
                        dateEditConsultationDate.EditValue = m_ConsultationEntity.ConsultTime.Split(' ')[0];
                        timeEditConsultationTime.EditValue = m_ConsultationEntity.ConsultTime.Split(' ')[1];
                    }

                    if (m_ConsultationEntity.StateID == "6741")
                    {
                        memoConsultSuggestion.Properties.ReadOnly = true;
                        lookUpEditorHospital.Enabled = false;
                        lookUpEditorDepartment.Enabled = false;
                        lookUpEditorEmployee.Enabled = false;

                        dateEditConsultationDate.Enabled = false;
                        timeEditConsultationTime.Enabled = false;
                        return;
                    }
                }
                else
                {
                    m_ConsultationEntity.ConsultTime = "";
                    m_ConsultationEntity.ConsultSuggestion = "";
                    m_ConsultationEntity.ConsultHospitalName = "";
                    m_ConsultationEntity.ConsultDeptName2 = "";
                    m_ConsultationEntity.ConsultUserName = "";
                }


                //else
                //{
                memoConsultSuggestion.Properties.ReadOnly = false;
                lookUpEditorHospital.Enabled = true;
                lookUpEditorDepartment.Enabled = true;
                lookUpEditorEmployee.Enabled = true;

                dateEditConsultationDate.Enabled = true;
                timeEditConsultationTime.Enabled = true;

                InitBtnState(m_ConsultationEntity.ApplyDeptID, m_ConsultationEntity.ConsultDeptID);
                //}

            }
        }

        private void PanelControlInit()
        {
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
        }

        /// <summary>
        /// 多科会诊显示多科会诊信息
        /// </summary>
        private void ShowConsultationMultiply()
        {

            this.panelControl1.Visible = false;
            this.panelControl2.Visible = true;

            m_UCConsultationMultiply = new UCConsultationMultiply(m_ConsultationEntity, CurrentInpatient);
            panelControl2.Width = m_UCConsultationMultiply.Width;
            panelControl2.Height = m_UCConsultationMultiply.Height;
            this.panelControl2.Controls.Add(m_UCConsultationMultiply);

            m_UCConsultationMultiply.Load(m_app);
        }

        #region 初始化数据

        private void BindData()
        {
            BindHospitalData();
            BindDepartment();
            BindEmployee();
        }

        #region 绑定医院
        /// <summary>
        /// 绑定医院
        /// </summary>
        private void BindHospitalData()
        {
            lookUpEditorHospital.Kind = WordbookKind.Sql;
            lookUpEditorHospital.ListWindow = lookUpWindowHospital;
            BindHospitalWordBook(GetConsultationData("1"));
        }

        /// <summary>
        /// 绑定医院的数据源
        /// </summary>
        /// <param name="dataTableData"></param>
        private void BindHospitalWordBook(DataTable dataTableData)
        {
            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            colWidths.Add("ID", 60);
            colWidths.Add("NAME", 120);

            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
                {
                    dataTableData.Columns[i].Caption = "医院编码";
                }
                else if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                {
                    dataTableData.Columns[i].Caption = "医院名称";
                }
            }

            //得到数据字典，为LookUpWindow中的数据源
            SqlWordbook wordBook = new SqlWordbook("Hospital"/*字典的唯一标示*/, dataTableData/*数据集*/,
                "ID"/*每行记录的唯一标示*/, "NAME"/*显示在LookUpEditer中的值*/
               , colWidths/*设置每列的宽度*/, "ID//Name"/*根据医院编码，医院名称进行查询*/);

            lookUpEditorHospital.SqlWordbook = wordBook;
        }

        #endregion

        #region 绑定受邀科室
        /// <summary>
        /// 绑定受邀科室
        /// </summary>
        private void BindDepartment()
        {
            lookUpEditorDepartment.Kind = WordbookKind.Sql;
            lookUpEditorDepartment.ListWindow = lookUpWindowDepartment;
            BindDepartmentWordBook(GetConsultationData("2"));
        }

        private void BindDepartmentWordBook(DataTable dataTableData)
        {
            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
                {
                    dataTableData.Columns[i].Caption = "科室编码";
                }
                else if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                {
                    dataTableData.Columns[i].Caption = "科室名称";
                }
            }

            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            colWidths.Add("ID", 60);
            colWidths.Add("NAME", 120);
            SqlWordbook wordBook = new SqlWordbook("Department", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorDepartment.SqlWordbook = wordBook;
        }

        #endregion

        #region 绑定会诊医生
        /// <summary>
        /// 绑定受邀科室
        /// </summary>
        private void BindEmployee()
        {
            lookUpEditorEmployee.Kind = WordbookKind.Sql;
            lookUpEditorEmployee.ListWindow = lookUpWindowEmployee;
            BindEmployeeWordBook(GetConsultationData("3"));
        }

        private void BindEmployeeWordBook(DataTable dataTableData)
        {
            for (int i = 0; i < dataTableData.Columns.Count; i++)
            {
                if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
                {
                    dataTableData.Columns[i].Caption = "医师代码";
                }
                else if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "name")
                {
                    dataTableData.Columns[i].Caption = "医师名称";
                }
            }

            Dictionary<string, int> colWidths = new Dictionary<string, int>();
            colWidths.Add("ID", 60);
            colWidths.Add("NAME", 120);
            SqlWordbook wordBook = new SqlWordbook("Employee", dataTableData, "ID", "NAME", colWidths, "ID//NAME//PY//WB");
            lookUpEditorEmployee.SqlWordbook = wordBook;
        }

        #endregion



        /// <summary>
        /// 得到绑定需要的数据
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        private DataTable GetConsultationData(string typeID)
        {
            if (DataAccess.App == null)
            {
                DataAccess.App = m_app;
            }
            DataTable dataTableConsultationData = new DataTable();

            if (typeID == "1")//医院
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, "");
            }
            else if (typeID == "2")//受邀科室
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, lookUpEditorHospital.CodeValue.Trim());
            }
            else if (typeID == "3")//受邀医生
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, lookUpEditorDepartment.CodeValue.Trim());
            }
            else if (typeID == "4")//申请医师
            {
                string wardID = m_app.User.CurrentWardId.ToString();
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, wardID);
            }
            else if (typeID == "5")//申请医师科主任
            {
                string wardID = m_app.User.CurrentWardId.ToString();
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, wardID);
            }
            else if (typeID == "6")//CategoryDetail
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, "20");
            }
            else if (typeID == "8")//得到所有临床医师
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, "");
            }
            else if (typeID == "9")//医师级别
            {
                dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, "20");
            }

            return dataTableConsultationData;
        }

        #endregion

        #region IEMREditor
        public Control DesignUI
        {
            get { return this; }
        }

        public new void Load(IYidanEmrHost app)
        {
            DataAccess.App = app;
            m_app = app;
            if (!string.IsNullOrEmpty(CurrentNoofinpat))
            {
                CurrentInpatient = new Common.Eop.Inpatient(Convert.ToDecimal(CurrentNoofinpat));
            }
            else if (m_app.CurrentPatientInfo != null)
            {
                CurrentInpatient = m_app.CurrentPatientInfo;
            }
            else
            {
                return;
            }
            CurrentInpatient.ReInitializeAllProperties();

            GetConsultationEntity();
            if (m_ConsultationID == "")
                return;
            ////一般会诊
            //if (m_ConsultTypeID == "6501")
            //{
            //    BindData();
            //    FillUI();
            //}
            ////多科会诊
            //else
            //{
                ShowConsultationMultiply();
            //}
            PanelControlInit();
        }

        public void Print()
        {
            //打印单科会诊信息
            if (m_ConsultTypeID == "6501")
            {
                PrintForm form = new PrintForm(m_ConsultationEntity);
                form.StartPosition = FormStartPosition.CenterScreen;
                form.ShowDialog();
            }
            //保存多科会诊信息
            else
            {
                if (m_UCConsultationMultiply == null)
                    return;
                else
                    m_UCConsultationMultiply.Print();
            }

        }

        public void Save()
        {
            if (m_ConsultationID == "")
                return;
            if (!btnSave.Enabled)
                return;
            //保存单科会诊信息
            if (m_ConsultTypeID == "6501")
            {
                if (m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.WaitConsultation) || m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.RecordeSave))
                    SaveForOne(ConsultStatus.RecordeSave);
            }
            //保存多科会诊信息
            else
            {
                if (m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.WaitConsultation) || m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.RecordeSave))
                    SaveForMultiply(ConsultStatus.RecordeSave);
            }
        }

        public string Title
        {
            get { return "会诊记录单"; }
        }

        public string CurrentNoofinpat
        {
            get;
            set;
        }

        #endregion

        private void UCConsultation_Resize(object sender, EventArgs e)
        {
            if (panelControl2.Controls.Count == 0)
            {
                panelControl2.Visible = false;
            }

            if (m_ConsultTypeID == "6501")
                this.panelControl1.Location = new Point((this.Width - panelControl1.Width) / 2, 10);
            else
            {

                this.panelControl2.Location = new Point((this.Width - panelControl2.Width) / 2, 10);
            }
        }

        private void lookUpEditorHospital_CodeValueChanged(object sender, EventArgs e)
        {
            BindDepartment();
        }

        private void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            BindEmployee();
        }

        /// <summary>
        /// 检查控件的输入情况
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            if (memoConsultSuggestion.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请输入会诊医师意见!", CustomMessageBoxKind.WarningOk);
                memoConsultSuggestion.Text = "";
                memoConsultSuggestion.Focus();
                return false;
            }

            if (lookUpEditorHospital.CodeValue == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊医院!", CustomMessageBoxKind.WarningOk);
                lookUpEditorHospital.Text = "";
                lookUpEditorHospital.Focus();
                return false;
            }

            if (lookUpEditorDepartment.CodeValue == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊科室!", CustomMessageBoxKind.WarningOk);
                lookUpEditorDepartment.Text = "";
                lookUpEditorDepartment.Focus();
                return false;
            }

            if (lookUpEditorEmployee.CodeValue == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊医师!", CustomMessageBoxKind.WarningOk);
                lookUpEditorEmployee.Text = "";
                lookUpEditorEmployee.Focus();
                return false;
            }

            if (dateEditConsultationDate.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊日期!", CustomMessageBoxKind.WarningOk);
                dateEditConsultationDate.Focus();
                return false;
            }

            if (timeEditConsultationTime.Text.Trim() == "0:00:00")
            {
                m_app.CustomMessageBox.MessageShow("请选择会诊时间!", CustomMessageBoxKind.WarningOk);
                timeEditConsultationTime.Focus();
                return false;
            }



            return true;
        }

        /// <summary>
        /// 保存单科会诊信息
        /// </summary>
        /// <param name="status"></param>
        public void SaveForOne(ConsultStatus status)
        {

            if (CheckData())
            {
                try
                {
                    m_app.SqlHelper.BeginTransaction();
                    SaveConsultationApply(SaveType.RecordModify, m_ConsultationID, status);
                    SaveConsultationRecordDept(m_ConsultationID);
                    m_app.SqlHelper.CommitTransaction();

                    m_app.CustomMessageBox.MessageShow("保存成功!", CustomMessageBoxKind.InformationOk);
                    GetConsultationEntity();
                    FillUI();
                }
                catch (Exception ex)
                {
                    m_app.SqlHelper.RollbackTransaction();
                    m_app.CustomMessageBox.MessageShow("保存失败!" + ex.Message, CustomMessageBoxKind.InformationOk);
                }
            }
        }

        /// <summary>
        /// 保存多科会诊信息
        /// </summary>
        /// <param name="status"></param>
        public void SaveForMultiply(ConsultStatus status)
        {
            if (m_UCConsultationMultiply == null)
                return;
            else
                m_UCConsultationMultiply.Save();
        }

        private string SaveConsultationApply(SaveType saveType, string consultApplySN, ConsultStatus status)
        {

            string typeID = Convert.ToString((int)saveType);
            string consultSuggestion = memoConsultSuggestion.Text.Trim();
            string finishTime = dateEditConsultationDate.Text + " " + timeEditConsultationTime.Text;

            return Dal.DataAccess.InsertConsultationApply(typeID, consultApplySN, m_ConsultationEntity.NoOfInpat, consultSuggestion, finishTime, Convert.ToString((int)status));
        }

        private void SaveConsultationRecordDept(string consultApplySn)
        {
            string orderValue = "1";
            string hospitalCode = lookUpEditorHospital.CodeValue;
            string departmentCode = lookUpEditorDepartment.CodeValue;
            string departmentName = string.Empty;
            string employeeCode = lookUpEditorEmployee.CodeValue;
            string employeeName = string.Empty;
            string employeeLevelID = string.Empty;
            string createUser = m_app.User.Id;
            string createTime = System.DateTime.Now.ToString();

            Dal.DataAccess.InsertConsultationRecordDept(consultApplySn, orderValue, hospitalCode, departmentCode, departmentName,
               employeeCode, employeeName, employeeLevelID, createUser, createTime);
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            DrawUnderLine(e.Graphics);
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, panelControl1.Width - 1, panelControl1.Height - 1));
        }

        private void DrawUnderLine(Graphics g)
        {
            foreach (Control control in this.panelControl1.Controls)
            {
                if (control is TextEdit && !(control is MemoEdit) || control is DateEdit || control is LookUpEditor)
                {
                    g.DrawLine(Pens.Black,
                        new Point(control.Location.X, control.Location.Y + control.Height),
                        new Point(control.Location.X + control.Width, control.Location.Y + control.Height));
                }
            }
        }


        /// <summary>
        /// 判断会诊保存与会诊结束按钮状态
        /// </summary>
        /// <param name="applyDeptID">申请科室</param>
        /// <param name="consultationDeptID">受邀科室</param>
        private void InitBtnState(string applyDeptID, string consultationDeptID)
        {
            string config = m_app.SqlHelper.ExecuteScalar("select value from appcfg where configkey = 'ConsultationConfig'", CommandType.Text).ToString();

            string[] configs = config.Split(',');
            //if (m_ConsultationEntity.StateID != Convert.ToString((int)ConsultStatus.WaitConsultation) && m_ConsultationEntity.StateID != Convert.ToString((int)ConsultStatus.RecordeSave))
            //    return;

            //申请方有保存权限
            if (configs[0].ToString() == "1")
            {
                if (m_app.User.CurrentDeptId == applyDeptID)
                    this.btnSave.Enabled = true;
            }
            //受邀方有保存权限
            else if (configs[0].ToString() == "2")
            {
                if (m_app.User.CurrentDeptId == consultationDeptID)
                    this.btnSave.Enabled = true;
            }
            //双方都有保存权限
            else if (configs[0].ToString() == "3")
            {
                if (m_app.User.CurrentDeptId == applyDeptID || m_app.User.CurrentDeptId == consultationDeptID)
                    this.btnSave.Enabled = true;
            }

            //申请方有完成权限
            if (configs[1].ToString() == "1")
            {
                if (m_app.User.CurrentDeptId == applyDeptID)
                    this.btnOver.Enabled = true;
            }
            //受邀方有完成权限
            else if (configs[1].ToString() == "2")
            {
                if (m_app.User.CurrentDeptId == consultationDeptID)
                    this.btnOver.Enabled = true;
            }
            //双方都有完成权限
            else if (configs[1].ToString() == "3")
            {
                if (m_app.User.CurrentDeptId == applyDeptID || m_app.User.CurrentDeptId == consultationDeptID)
                    this.btnOver.Enabled = true;
            }

            //如果保存和修改按钮都不可用则控件不可用
            if (!btnOver.Enabled && !btnSave.Enabled)
            {
                memoConsultSuggestion.Properties.ReadOnly = true;
                lookUpEditorHospital.Enabled = false;
                lookUpEditorDepartment.Enabled = false;
                lookUpEditorEmployee.Enabled = false;

                dateEditConsultationDate.Enabled = false;
                timeEditConsultationTime.Enabled = false;
            }
        }

        /// <summary>
        /// 页面保存单科会诊信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //保存单科会诊信息
            if (m_ConsultTypeID == "6501")
            {
                if (m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.WaitConsultation) || m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.RecordeSave))
                    SaveForOne(ConsultStatus.RecordeSave);
            }
        }

        /// <summary>
        /// 页面完成单科会诊信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOver_Click(object sender, EventArgs e)
        {
            //保存单科会诊信息
            if (m_ConsultTypeID == "6501")
            {
                if (m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.WaitConsultation) || m_ConsultationEntity.StateID == Convert.ToString((int)ConsultStatus.RecordeSave))
                    SaveForOne(ConsultStatus.RecordeComplete);
            }
        }

        private void panelControl1_Click(object sender, EventArgs e)
        {
            int verticalScrollValue = this.VerticalScroll.Value;
            //Point pt = this.AutoScrollPosition;
            this.btn1.Focus();
            this.VerticalScroll.Value = verticalScrollValue;
        }

        private void UCConsultation_Click(object sender, EventArgs e)
        {
            int verticalScrollValue = this.VerticalScroll.Value;
            //Point pt = this.AutoScrollPosition;
            this.btn1.Focus();
            this.VerticalScroll.Value = verticalScrollValue;
        }
 
    }
}
