using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;

namespace DrectSoft.Core.OwnBedInfo
{
    /// <summary>
    /// ************对婴儿信息的维护功能**************
    /// **************add by ywk 2012年6月6日 14:26:47****************
    /// </summary>
    public partial class SetPatientsBaby : DevBaseForm
    {
        private UserControlAllListBedInfo m_UserControlAllListBedInfo;
        private DocCenter m_DocCenter;
        private IEmrHost m_app;
        public string NOOFINPAT;//上级页面传来的值（此婴儿的母亲的Noofinpat）
        public string PATNAME;//病人姓名
        public SetPatientsBaby()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 当前操作状态
        /// </summary>
        EditState m_EditState = EditState.None;

        /// <summary>
        /// 构造函数(用于医生工作站)
        /// </summary>
        /// <param name="NoOfInpat"></param>
        public SetPatientsBaby(string NoOfInpat, IEmrHost myapp, string mpatname, UserControlAllListBedInfo userControlAllListBedInfo)
        {
            NOOFINPAT = NoOfInpat;
            m_app = myapp;
            PATNAME = mpatname;
            m_UserControlAllListBedInfo = userControlAllListBedInfo;
            InitializeComponent();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="NoOfInpat"></param>
        public SetPatientsBaby(string NoOfInpat, IEmrHost myapp, string mpatname, DocCenter docCenter)
        {
            NOOFINPAT = NoOfInpat;
            m_app = myapp;
            PATNAME = mpatname;
            m_DocCenter = docCenter;
            InitializeComponent();
            txtPatName.Focus();
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
            /// 视图
            /// </summary>
            View = 8
        }

        #region 关于方法
        /// <summary>
        /// 加载婴儿信息，绑定到grid控件上
        /// edit by Yanqiao.Cai 2012-11-15
        /// 1、add try ... catch
        /// 2、加载性别图片
        /// </summary>
        private void LoadBabyInfo()
        {
            try
            {
                DataManager dtmanager = new DataManager(m_app, "", "");
                DataTable dt = dtmanager.GetBabyInfo(NOOFINPAT);
                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageXB1, imageListXB);
                gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        ///绑定性别
        /// </summary>
        private void InitLueSex()
        {
            if (lueSex.SqlWordbook == null)
                BindLueData(lueSex, 2);
        }
        /// <summary>
        /// 给lue的数据源，新增 名称 栏位
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private DataTable AddTableColumn(DataTable dataTable)
        {
            DataTable dataTableAdd = dataTable;
            if (!dataTableAdd.Columns.Contains("名称"))
                dataTableAdd.Columns.Add("名称");
            foreach (DataRow row in dataTableAdd.Rows)
                row["名称"] = row["Name"].ToString();
            return dataTableAdd;
        }
        /// <summary>
        /// 获取lue的数据源
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        private DataTable GetEditroData(Decimal queryType)
        {
            SqlParameter paraType = new SqlParameter("@QueryType", SqlDbType.Decimal);
            paraType.Value = queryType;
            SqlParameter[] paramCollection = new SqlParameter[] { paraType };
            DataTable dataTable = AddTableColumn(m_app.SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, CommandType.StoredProcedure));
            return dataTable;
        }
        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_app.SqlHelper;
            DataTable dataTable = GetEditroData(queryType);

            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueInfo.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            lueInfo.SqlWordbook = sqlWordBook;
            lueInfo.ListWindow = lupInfo;
        }
        /// <summary>
        /// 清空操作
        /// </summary>
        private void ClearPageValue()
        {
            txtName.Text = "";
            deBirth.EditValue = "";
            teAdmitDate.EditValue = string.Empty;
            txtAge.Text = "";
            lueSex.CodeValue = "";
        }
        /// <summary>
        /// 控制按钮状态
        /// edit by Yanqiao.Cai 2012-11-15
        /// add try ... catch
        /// </summary>
        private void BtnState()
        {
            try
            {
                //查看详细操作
                if (m_EditState == EditState.View)
                {
                    this.btnADD.Enabled = true;
                    this.btnDel.Enabled = true;
                    this.btnSave.Enabled = false;
                    this.btn_reset.Enabled = false;
                    this.BtnClear.Enabled = false;
                    txtName.Enabled = false;
                    deBirth.Properties.ReadOnly = true;
                    teAdmitDate.Properties.ReadOnly = true;
                    txtAge.Enabled = false;
                    lueSex.Enabled = false;
                }
                else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
                {
                    this.txtName.Focus();
                    this.btnADD.Enabled = false;
                    this.btnDel.Enabled = false;
                    this.btnSave.Enabled = true;
                    this.btn_reset.Enabled = true;
                    this.BtnClear.Enabled = true;

                    if (m_EditState == EditState.Edit)
                        btnDel.Enabled = true;

                    txtName.Enabled = true;
                    deBirth.Properties.ReadOnly = false;
                    teAdmitDate.Properties.ReadOnly = false;
                    //txtAge.Enabled = true;
                    lueSex.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 验证数据完整性
        /// </summary>
        /// <returns></returns>
        private bool IsSave()
        {
            if (lueSex.CodeValue == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择性别");
                this.lueSex.Focus();
                return false;
            }
            else if (this.txtName.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("姓名不能为空");
                this.txtName.Focus();
                return false;
            }
            else if (deBirth.EditValue.ToString() == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择出生日期");
                this.deBirth.Focus();
                return false;
            }
            else if (deBirth.DateTime > DateTime.Now)
            {
                m_app.CustomMessageBox.MessageShow("出生日期不能大于当前日期");
                this.deBirth.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证数据完整性
        /// </summary>
        /// <returns></returns>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(lueSex.CodeValue))
                {
                    this.lueSex.Focus();
                    return "请选择性别";
                }
                else if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
                {
                    this.txtName.Focus();
                    return "姓名不能为空";
                }
                else if (null == deBirth.DateTime || string.IsNullOrEmpty(deBirth.Text))
                {
                    this.deBirth.Focus();
                    return "请选择出生日期";
                }
                else if (deBirth.DateTime > DateTime.Now)
                {
                    this.deBirth.Focus();
                    return "出生日期不能大于当前日期";
                }
                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将页面的值加进实体里
        /// </summary>
        /// <returns></returns>
        private PatientEntity SetEntityByPage()
        {
            PatientEntity patEntity = new PatientEntity();
            //此处除了姓名，性别，出生日期，年龄，noofinpat，其他的都要与他的母亲保持一致
            //其余的取母亲的
            patEntity.Name = this.txtName.Text;
            patEntity.Birth = deBirth.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Time.ToString("HH:mm");
            //patEntity.Birth = deBirth.DateTime.ToString("yyyy-MM-dd");/*deBirth.EditValue.ToString("yyyy-MM-dd");*/
            //patEntity.Age = Int32.Parse(txtAge.Text);
            patEntity.Age = 0;
            patEntity.AgeStr = txtAge.Text;
            patEntity.SexID = lueSex.CodeValue;
            patEntity.ISBABY = 1;
            patEntity.MOTHER = Int32.Parse(NOOFINPAT);
            //patEntity
            string sql = string.Format("select * from inpatient where noofinpat='{0}'", NOOFINPAT);
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            string babyNO = GetBabyNo(NOOFINPAT);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //patEntity.PatNoOfHis = dt.Rows[0]["PATNOOFHIS"].ToString();
                //patnoofhis等四个地方随机生成 14位
                patEntity.PatNoOfHis = babyNO;
                //Guid.NewGuid().ToString().Substring(1, 14);
                patEntity.NoOfcClinic = babyNO;
                //Guid.NewGuid().ToString().Substring(1, 14);
                patEntity.NoOfRecord = babyNO;
                //Guid.NewGuid().ToString().Substring(1, 14);;
                patEntity.PatID = babyNO;
                //Guid.NewGuid().ToString().Substring(1, 14);
                //patEntity.NoOfcClinic = dt.Rows[0]["NOOFCLINIC"].ToString();
                //patEntity.NoOfRecord = dt.Rows[0]["NOOFRECORD"].ToString();
                //patEntity.PatID = dt.Rows[0]["PATID"].ToString();
                patEntity.INNERPIX = dt.Rows[0]["INNERPIX"].ToString();
                patEntity.OUTPIX = dt.Rows[0]["OUTPIX"].ToString();
                patEntity.PY = "";
                patEntity.WB = "";
                patEntity.PayID = dt.Rows[0]["PAYID"].ToString();
                patEntity.ORIGIN = dt.Rows[0]["ORIGIN"].ToString();
                patEntity.InCount = Int32.Parse(dt.Rows[0]["INCOUNT"].ToString());
                patEntity.IDNO = dt.Rows[0]["IDNO"].ToString();
                patEntity.Marital = "0";/*dt.Rows[0]["MARITAL"].ToString();*/
                patEntity.JobID = dt.Rows[0]["JOBID"].ToString();
                patEntity.CSD_ProvinceID = dt.Rows[0]["PROVINCEID"].ToString();
                patEntity.CSD_CityID = dt.Rows[0]["COUNTYID"].ToString();
                patEntity.CSD_DistrictID = dt.Rows[0]["DISTRICTID"].ToString();
                patEntity.NationID = dt.Rows[0]["NATIONID"].ToString();
                patEntity.NationalityID = dt.Rows[0]["NATIONALITYID"].ToString();
                patEntity.JG_ProvinceID = dt.Rows[0]["NATIVEPLACE_P"].ToString();
                patEntity.JG_CityID = dt.Rows[0]["NATIVEPLACE_C"].ToString();
                patEntity.Organization = dt.Rows[0]["ORGANIZATION"].ToString();
                patEntity.OfficePlace = dt.Rows[0]["OFFICEPLACE"].ToString();
                patEntity.OfficeTEL = dt.Rows[0]["OFFICETEL"].ToString();
                patEntity.OfficePost = dt.Rows[0]["OFFICEPOST"].ToString();
                patEntity.NATIVEADDRESS = dt.Rows[0]["NATIVEADDRESS"].ToString();
                patEntity.NATIVETEL = dt.Rows[0]["NATIVETEL"].ToString();
                patEntity.NATIVEPOST = dt.Rows[0]["NATIVEPOST"].ToString();
                patEntity.ADDRESS = dt.Rows[0]["ADDRESS"].ToString();
                patEntity.ContactPerson = dt.Rows[0]["CONTACTPERSON"].ToString();
                patEntity.RelationshipID = dt.Rows[0]["RELATIONSHIP"].ToString();
                patEntity.ContactAddress = dt.Rows[0]["CONTACTADDRESS"].ToString();
                patEntity.CONTACTOFFICE = dt.Rows[0]["CONTACTOFFICE"].ToString();
                patEntity.ContactTEL = dt.Rows[0]["CONTACTTEL"].ToString();
                patEntity.CONTACTPOST = dt.Rows[0]["CONTACTPOST"].ToString();
                patEntity.SocialCare = dt.Rows[0]["SOCIALCARE"].ToString();
                patEntity.INSURANCE = dt.Rows[0]["INSURANCE"].ToString();
                patEntity.CARDNO = dt.Rows[0]["CARDNO"].ToString();
                patEntity.ADMITINFO = dt.Rows[0]["ADMITINFO"].ToString();
                patEntity.AdmitDeptID = dt.Rows[0]["ADMITDEPT"].ToString();
                patEntity.AdmitWardID = dt.Rows[0]["ADMITWARD"].ToString();
                patEntity.ADMITBED = dt.Rows[0]["ADMITBED"].ToString();
                patEntity.AdmitDate = patEntity.Birth;
                patEntity.INWARDDATE = patEntity.Birth;
                patEntity.ADMITDIAGNOSIS = dt.Rows[0]["ADMITDIAGNOSIS"].ToString();
                patEntity.OutHosDeptID = dt.Rows[0]["OUTHOSDEPT"].ToString();
                patEntity.OutHosWardID = dt.Rows[0]["OUTHOSWARD"].ToString();
                patEntity.OutBed = dt.Rows[0]["OUTBED"].ToString();
                patEntity.OutWardDate = dt.Rows[0]["OUTWARDDATE"].ToString();
                patEntity.OUTHOSDATE = dt.Rows[0]["OUTHOSDATE"].ToString();
                patEntity.OUTDIAGNOSIS = dt.Rows[0]["OUTDIAGNOSIS"].ToString();
                patEntity.TOTALDAYS = Int32.Parse(dt.Rows[0]["TOTALDAYS"].ToString() == "" ? "0" : dt.Rows[0]["TOTALDAYS"].ToString());
                patEntity.CLINICDIAGNOSIS = dt.Rows[0]["CLINICDIAGNOSIS"].ToString();
                patEntity.SOLARTERMS = dt.Rows[0]["SOLARTERMS"].ToString();
                patEntity.ADMITWAY = dt.Rows[0]["ADMITWAY"].ToString();
                patEntity.OUTWAY = dt.Rows[0]["OUTWAY"].ToString();
                patEntity.CLINICDOCTOR = dt.Rows[0]["CLINICDOCTOR"].ToString();
                patEntity.RESIDENT = dt.Rows[0]["RESIDENT"].ToString();
                patEntity.ATTEND = dt.Rows[0]["ATTEND"].ToString();
                patEntity.CHIEF = dt.Rows[0]["CHIEF"].ToString();
                patEntity.EDU = dt.Rows[0]["EDU"].ToString();
                //Int32.Parse(dt.Rows[0]["TOTALDAYS"].ToString() == "" ? "0" : dt.Rows[0]["TOTALDAYS"].ToString());STATUS
                patEntity.EDUC = Int32.Parse(dt.Rows[0]["EDUC"].ToString() == "" ? "0" : dt.Rows[0]["EDUC"].ToString());
                patEntity.RELIGION = dt.Rows[0]["RELIGION"].ToString();
                patEntity.STATUS = Int32.Parse(dt.Rows[0]["STATUS"].ToString() == "" ? "0" : dt.Rows[0]["STATUS"].ToString());
                patEntity.CRITICALLEVEL = dt.Rows[0]["CRITICALLEVEL"].ToString();
                patEntity.ATTENDLEVEL = dt.Rows[0]["ATTENDLEVEL"].ToString();
                patEntity.EMPHASIS = Int32.Parse(dt.Rows[0]["EMPHASIS"].ToString() == "" ? "0" : dt.Rows[0]["EMPHASIS"].ToString());

                patEntity.MEDICAREID = dt.Rows[0]["MEDICAREID"].ToString();
                patEntity.MEDICAREQUOTA = Int32.Parse(dt.Rows[0]["MEDICAREQUOTA"].ToString() == "" ? "0" : dt.Rows[0]["MEDICAREQUOTA"].ToString());
                patEntity.VOUCHERSCODE = dt.Rows[0]["VOUCHERSCODE"].ToString();
                patEntity.STYLE = dt.Rows[0]["STYLE"].ToString();
                patEntity.OPERATOR = dt.Rows[0]["OPERATOR"].ToString();
                patEntity.MEMO = dt.Rows[0]["MEMO"].ToString();
                patEntity.CPSTATUS = Int32.Parse(dt.Rows[0]["CPSTATUS"].ToString() == "" ? "0" : dt.Rows[0]["CPSTATUS"].ToString());
                patEntity.OUTWARDBED = Int32.Parse(dt.Rows[0]["OUTWARDBED"].ToString() == "" ? "0" : dt.Rows[0]["OUTWARDBED"].ToString());

                patEntity.XZZ_ProvinceID = dt.Rows[0]["XZZPROVICEID"].ToString();
                patEntity.XZZ_CityID = dt.Rows[0]["XZZCITYID"].ToString();
                patEntity.XZZ_DistrictID = dt.Rows[0]["XZZDISTRICTID"].ToString();
                patEntity.XZZ_TEL = dt.Rows[0]["XZZTEL"].ToString();
                patEntity.HKDZ_ProvinceID = dt.Rows[0]["HKDZPROVICEID"].ToString();
                patEntity.HKDZ_CityID = dt.Rows[0]["HKZDCITYID"].ToString();
                patEntity.HKDZ_DistrictID = dt.Rows[0]["HKZDDISTRICTID"].ToString();
                patEntity.XZZ_Post = dt.Rows[0]["XZZPOST"].ToString();

            }
            return patEntity;
        }
        /// <summary>
        /// 获取婴儿住院号
        /// </summary>
        /// <param name="MotherNo"></param>
        /// <returns></returns>
        private string GetBabyNo(string MotherNo)
        {
            string babyNo = "";
            int MaxNO = 0;
            string sql_mother = string.Format("select * from inpatient where mother='{0}'", NOOFINPAT);
            DataTable dt_mother = m_app.SqlHelper.ExecuteDataTable(sql_mother, CommandType.Text);
            for (int i = 0; i < dt_mother.Rows.Count; i++)
            {

                if (dt_mother.Rows[i]["patnoofhis"] != null)
                {
                    string[] s = dt_mother.Rows[i]["patnoofhis"].ToString().Split('_');
                    if (s.Length > 1)
                    {
                        if (MaxNO < int.Parse(s[1]))
                        {
                            MaxNO = int.Parse(s[1]);
                        }

                    }
                }
            }
            return MotherNo + "_" + (MaxNO + 1).ToString();
        }
        int oldFocusRowHandle;//用来保存上部操作的焦点行
        string oldName = string.Empty;//用于保存上部操作的人的姓名
        /// <summary>
        /// 进行保存操作
        /// </summary>
        /// <param name="configEmrPoint"></param>
        /// <returns></returns>
        private bool SaveData(PatientEntity m_patientInfo)
        {
            try
            {

                DataManager dtmanager = new DataManager(m_app, "", "");

                DataRow foucesRow = gViewBabyInfo.GetDataRow(gViewBabyInfo.FocusedRowHandle);

                string noofinpat = string.Empty;
                if (foucesRow != null)
                {
                    noofinpat = foucesRow["NOOFINPAT"].ToString();
                }
                //string mnoofinpat = foucesRow["MNOOFINPAT"].ToString();
                string mnoofinpat = MNoOfPat;
                if (mnoofinpat == "")
                {
                    mnoofinpat = foucesRow["MNOOFINPAT"].ToString();
                    //m_app.CustomMessageBox.MessageShow("请先选择母亲");
                    //return false;
                }
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                //此处加进编辑过的病人数组中 add ywk 
                EditedPats.Add(mnoofinpat);
                dtmanager.SaveData(m_patientInfo, edittype, Int32.Parse(noofinpat == "" ? "0" : noofinpat));

                ////处理婴儿个数，和上面分开
                //int BabyCount=

                #region "插入入院状态(三测单显示) --- 已弃用（cyq 2012-08-16）"
                //PatStateEntity patStateEn = new PatStateEntity();
                //patStateEn.CCODE = "7008";//状态编号
                //patStateEn.DOTIME = m_patientInfo.Birth;
                //patStateEn.PATID = dtmanager.GetNoofInpatByPatID(m_patientInfo.PatID);
                //MethodSet.SaveStateData(patStateEn, "1");
                #endregion

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// 刷新页面数据
        /// </summary>
        /// <param name="p"></param>
        private void RefreshData()
        {
            LoadBabyInfo();
            m_EditState = EditState.View;
            BtnState();
            ClearPageValue();
            LoadPatInfo();//加了婴儿，上面婴儿个数相应变掉
            this.grdViewPat.FocusedRowHandle = oldFocusRowHandle;
            this.txtPatName.Text = oldName;
            SetGridDataBySea();//筛出符合条件的数据 2012年6月12日 08:38:37
        }

        /// <summary>
        /// 将grid中的值赋值给实体
        /// </summary>
        /// <param name="foucesRow"></param>
        /// <returns></returns>
        private PatientEntity SetEntityByDataRow(DataRow foucesRow)
        {
            PatientEntity m_patientInfo = new PatientEntity();
            if (foucesRow == null)
            {
                return null;
            }
            m_patientInfo.Name = foucesRow["Name"].ToString();
            m_patientInfo.Age = Int32.Parse(foucesRow["Age"].ToString());
            m_patientInfo.AgeStr = foucesRow["AgeStr"].ToString();
            m_patientInfo.Birth = foucesRow["birth"].ToString();
            m_patientInfo.SexID = foucesRow["SexID"].ToString();
            return m_patientInfo;
        }

        /// <summary>
        /// 将实体值赋给页面元素
        /// </summary>
        /// <param name="configEmrPoint"></param>
        private void SetPageValue(PatientEntity patientEntity)
        {
            if (patientEntity == null)
                return;
            txtAge.Text = patientEntity.AgeStr.ToString();
            txtName.Text = patientEntity.Name;
            lueSex.CodeValue = patientEntity.SexID;
            //deBirth.Text = patientEntity.Birth;
            deBirth.DateTime = Convert.ToDateTime(patientEntity.Birth);
            teAdmitDate.Time = Convert.ToDateTime(patientEntity.Birth);
        }
        public static string CalcDisplayAge(DateTime birthday, DateTime endDate)
        {
            string displayAge;
            int accAge;
            PatientBasicInfo.CalculateAge(birthday, DateTime.Now, out displayAge, out accAge);
            return displayAge;
        }
        /// <summary>
        /// TODO按照病案年龄处理逻辑计算病人截止到指定日期的年龄（显示用）
        /// </summary>
        /// <param name="birthday">病人出生日期</param>
        /// <param name="endDate">截止日期</param>
        /// <param name="displayAge">显示年龄</param>
        /// <param name="accurateAge">精确的年龄值</param>
        public static void CalculateAge(DateTime birthday, DateTime endDate, out string displayAge, out int accurateAge)
        {
            #region 屏蔽前台的运算逻辑，改成调用存储过程
            accurateAge = 0;
            displayAge = string.Empty;
            if (birthday > endDate)
                return;

            // 精确的年龄 = 年数 + 月数 + 天数 构成

            TimeSpan dateDiff;
            int yearPart;
            int monthPart;
            int dayPart;
            int decMonth = 0;
            int decYear = 0;

            // 比较两个日期天数部分（该月的第几天）。看是否足月。
            if (endDate.Day >= birthday.Day)
                dayPart = endDate.Day - birthday.Day;
            //if (dayPart == 0)//如果是今天，年龄应是1天 add by ywk 
            //{
            //    dayPart = 1;
            //}
            else
            {
                decMonth = 1; // 不足月，月数要扣除1
                dateDiff = (birthday.AddMonths(1) - birthday);
                dayPart = endDate.Day + dateDiff.Days - birthday.Day - 1;
            }

            // 比较两个日期的月份部分。看是否足年。
            if ((endDate.Month - decMonth) >= birthday.Month)
                monthPart = endDate.Month - birthday.Month - decMonth;
            else
            {
                decYear = 1; // 不足年，年数要扣除1
                monthPart = endDate.Month + 12 - birthday.Month - decMonth;
            }

            // 年数
            yearPart = endDate.Year - birthday.Year - decYear;

            if (yearPart > 1) // 2岁及以上的情况
            {
                // 对2岁上的若不是周岁，则加一，以符合日常习惯
                if ((monthPart == 0) && (dayPart == 0))
                {
                    displayAge = String.Format("{0}岁", yearPart);
                    accurateAge = yearPart;
                }
                else
                {
                    //accurateAge = yearPart + 1;
                    accurateAge = yearPart;//保存实际年龄（泗县修改 2012年5月9日12:16:37 ywk）
                    displayAge = String.Format("{0}岁", accurateAge);

                }
            }
            else if (yearPart == 1)
            {
                accurateAge = yearPart * 12 + monthPart;
                displayAge = String.Format("{0}个月", accurateAge);

            }
            else if ((monthPart > 1) || ((monthPart == 1) && (dayPart == 0)))
            {
                accurateAge = monthPart;
                displayAge = String.Format("{0}个月", monthPart);
            }
            else if (monthPart == 1)
            {
                displayAge = String.Format("{0}个月{1}天", monthPart, dayPart);
            }
            else
            { displayAge = String.Format("{0}天", dayPart); }
            #endregion
        }

        /// <summary>
        /// 加载当前科室的所有病人
        /// edit by Yanqiao.Cai 2012-11-15
        /// 1、add try ... catch
        /// 2、加载性别图片
        /// </summary>
        private void LoadPatInfo()
        {
            try
            {
                DataTable dt = DataManager.GetCurrentPatient(m_app.User.CurrentDeptId, m_app.User.CurrentWardId);
                string filter = string.Format(@"  yebz='0' ");
                //DataTable dt1 = gridControl2.DataSource as DataTable;
                if (dt != null)
                {
                    dt.DefaultView.RowFilter = filter;
                }
                //要进行处理的DataTAble，处理 病人姓名字段(***病人姓名【1个婴儿】***)
                DataTable newDt = dt.DefaultView.ToTable();
                string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
                for (int i = 0; i < newDt.Rows.Count; i++)
                {
                    ResultName = DataManager.GetPatsBabyContent(m_app, newDt.Rows[i]["noofinpat"].ToString());
                    newDt.Rows[i]["PatName"] = ResultName;
                }
                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageXB2, imageListXB);
                this.gridControl2.DataSource = newDt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 根据输入条件控制Grid绑定数据
        /// </summary>
        private void SetGridDataBySea()
        {
            //搜索条件(此处筛掉了婴儿)
            string filter = string.Format(@" PATNAME like '%{0}%' or SexName like '%{0}%' 
            or bedid like '%{0}%'  or PatId like '%{0}%' and yebz='0' ", txtPatName.Text.Trim());
            DataTable dt = gridControl2.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.RowFilter = filter;
            }
        }
        #endregion
        #region 关于事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetPatientsBaby_Load(object sender, EventArgs e)
        {
            InitLueSex();
            LoadBabyInfo();//加载婴儿信息
            LoadPatInfo();//加载病人信息
            //this.txtPatName.Text = PATNAME;
            //有婴儿的这边后处理下
            if (PATNAME.Contains("婴儿"))
            {
                this.txtPatName.Text = PATNAME.Substring(0, PATNAME.IndexOf('【'));
            }
            else
            {
                this.txtPatName.Text = PATNAME;
            }

            try
            {
                SetGridDataBySea();//默认显示上级页面选择的患者
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("错误信息:" + ex.Message);
                return;
            }

            //更改出生日期后，年龄算出来(ywk)
            deBirth.EditValueChanged += new EventHandler(deBirth_EditValueChanged);

            m_EditState = EditState.View;
            BtnState();
            this.ActiveControl = txtPatName;
        }
        /// <summary>
        /// 更改出生日期后，动态计算年龄//edit by wyt 2012-11-02 更新年龄计算方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deBirth_EditValueChanged(object sender, EventArgs e)
        {
            this.txtAge.Text = "0岁"; //PatientBasicInfo.CalcDisplayAge(deBirth.DateTime, DateTime.Now);
        }
        private string MNoOfPat = string.Empty;//母亲的病案首页序号
        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnADD_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdViewPat.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条病人记录");
                    return;
                }

                m_EditState = EditState.Add;
                oldFocusRowHandle = grdViewPat.FocusedRowHandle;
                DataRow foucesRow = grdViewPat.GetDataRow(grdViewPat.FocusedRowHandle);
                oldName = foucesRow["PatName"].ToString();
                MNoOfPat = foucesRow["NOOFINPAT"].ToString();
                if (oldName.Contains("婴儿"))
                {
                    oldName = oldName.Substring(0, oldName.IndexOf('【'));
                }
                //ClearPageValue();
                deBirth.DateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));//默认给今天出生日期精确的时分
                this.txtAge.Text = "0岁";//CalcDisplayAge(deBirth.DateTime, DateTime.Now);
                this.txtName.Text = oldName + "之";//仁和需求，婴儿姓名默认为“母亲姓名”+“之”
                lueSex.CodeValue = "";

                //oldName=grdViewPa
                BtnState();
                this.lueSex.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gViewBabyInfo.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条婴儿记录");
                    return;
                }
                DataRow foucesRow = gViewBabyInfo.GetDataRow(gViewBabyInfo.FocusedRowHandle);
                if (foucesRow == null)
                    return;
                oldFocusRowHandle = grdViewPat.FocusedRowHandle;
                oldName = foucesRow["MNAME"].ToString();
                if (oldName.Contains("婴儿"))
                {
                    oldName = oldName.Substring(0, oldName.IndexOf('【'));
                }

                string noofinpat = foucesRow["NOOFINPAT"].ToString();
                string mothernofinpat = foucesRow["MNOOFINPAT"].ToString(); ;//用于保存母亲的NoOfinpat

                if (string.IsNullOrEmpty(noofinpat))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条婴儿记录");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(string.Format("您确定要删除该婴儿吗？"), "删除婴儿", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    EditedPats.Add(mothernofinpat);
                    DataManager dtmanager = new DataManager(m_app, "", "");
                    dtmanager.SaveData(SetEntityByPage(), "3", Int32.Parse(noofinpat));
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");
                    RefreshData();
                }
            }
            catch (Exception)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除失败");
            }

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //edit by cyq 2012-10-23
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }
                if (SaveData(SetEntityByPage()))
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增成功");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改成功");
                    }
                    RefreshData();
                }
                else
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增失败");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改失败");
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPageValue();
                lueSex.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearPageValue();
            m_EditState = EditState.View;
            BtnState();
        }
        /// <summary>
        /// 点击列表看详细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Click(object sender, EventArgs e)
        {
            GridHitInfo hitInfo = gViewBabyInfo.CalcHitInfo(gridControl1.PointToClient(Cursor.Position));
            if (hitInfo.RowHandle < 0)
            {
                return;
            }
            if (gViewBabyInfo.FocusedRowHandle < 0)
                return;
            DataRow foucesRow = gViewBabyInfo.GetDataRow(gViewBabyInfo.FocusedRowHandle);
            if (foucesRow == null)
                return;

            SetPageValue(SetEntityByDataRow(foucesRow));
            m_EditState = EditState.Edit;


            BtnState();
        }
        /// <summary>
        /// 查询病人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPatName_TextChanged(object sender, EventArgs e)
        {
            SetGridDataBySea();
        }
        /// <summary>
        /// 点击上方患者列表，下方带出他的孩子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl2_Click(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = grdViewPat.CalcHitInfo(gridControl2.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                if (grdViewPat.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow foucesRow = grdViewPat.GetDataRow(grdViewPat.FocusedRowHandle);
                if (foucesRow == null)
                {
                    return;
                }
                string noofpat = foucesRow["NOOFINPAT"].ToString();
                DataManager dtmanager = new DataManager(m_app, "", "");
                DataTable dt = dtmanager.GetBabyInfo(noofpat);
                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageXB1, imageListXB);
                gridControl1.DataSource = dt;
                NOOFINPAT = noofpat;//此时全局的首页序号属性应更换为当前选择的人

                oldName = foucesRow["PATNAME"].ToString();
                if (oldName.Contains("婴儿"))
                {
                    oldName = oldName.Substring(0, oldName.IndexOf('【'));
                }
                txtPatName.Text = oldName;


                ClearPageValue();
                btnADD.Enabled = true;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        StringFormat s = new StringFormat();
        /// <summary>
        /// 控制病人列表中，有婴儿的数据的显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewPat_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            s.Alignment = StringAlignment.Center;
            s.LineAlignment = StringAlignment.Center;
            if (e.CellValue == null) return;
            DataRowView drv = grdViewPat.GetRow(e.RowHandle) as DataRowView;
            //取得病人名字
            string patname = drv["patname"].ToString().Trim();

            if (e.Column == gridColumn8)
            {
                if (patname.Contains("婴儿"))
                {
                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                    e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red, e.Bounds, s);
                    e.Handled = true;
                }

            }
            //e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
            //e.Appearance.ForeColor = Color.White;

        }
        #endregion

        private void txtPatName_KeyUp(object sender, KeyEventArgs e)
        {

        }
        public ArrayList EditedPats = new ArrayList();
        //public string[] EditedPats { get; set; }//用于保存在此界面进行修改操作后的患者（保存患者的Noofinpat）
        private void SetPatientsBaby_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_UserControlAllListBedInfo != null)
            {
                m_UserControlAllListBedInfo.RefreshPat(EditedPats);
            }
            else if (m_DocCenter != null)
            {
                m_DocCenter.ControlRefresh();
            }
        }

        /// <summary>
        /// 性别选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueSex_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataRow foucesRow = grdViewPat.GetDataRow(grdViewPat.FocusedRowHandle);
                if (foucesRow == null)//add by xlb 2012-12-25
                {
                    return;
                }
                string name = foucesRow["PatName"].ToString();
                if (name.Contains("婴儿"))
                {
                    name = name.Substring(0, name.IndexOf('【'));
                }
                name += "之";
                if (lueSex.CodeValue == "1")
                {
                    name += "子";
                }
                else if (lueSex.CodeValue == "2")
                {
                    name += "女";
                }
                this.txtName.Text = name;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 序号 --- 病人列表
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewPat_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 序号 --- 婴儿
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gViewBabyInfo_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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


    }
}