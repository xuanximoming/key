using DevExpress.XtraEditors;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Library;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.Core.OwnBedInfo
{
    public partial class XtraFormInHosLogin : DevBaseForm
    {
        private IEmrHost m_app;
        private string m_noofInpat;
        /// <summary>
        /// 父页面刷新标识
        /// </summary>
        public bool refreashFlag = false;
        private PatientEntity patEntity;
        public XtraFormInHosLogin(IEmrHost app, string noofInpat)
        {
            InitializeComponent();
            m_noofInpat = noofInpat;
            m_app = app;
        }
        List<TextEdit> m_TextEditControlList = new List<TextEdit>();
        ///// <summary>
        ///// 科室
        ///// </summary>
        //private DataTable Ward
        //{
        //    get { return _ward; }
        //}
        //private DataTable _ward;
        ///// <summary>
        ///// 科室
        ///// </summary>
        //private DataTable Dept
        //{
        //    get { return _dept; }
        //}
        //private DataTable _dept;

        private DataTable Province
        {
            get { return _province; }
        }
        private DataTable _province;

        private DataTable City
        {
            get { return _city; }
        }
        private DataTable _city;

        private DataTable QXian
        {
            get { return _qxian; }
        }
        private DataTable _qxian;
        //private DataTable Depts
        //{
        //    get { return _depts; }
        //}
        //private DataTable _depts;
        //private DataTable Wards
        //{
        //    get { return _wards; }
        //}
        //private DataTable _wards;

        private DataTable Nations
        {
            get { return _nations; }
        }
        private DataTable _nations;

        private DataTable ReleationShip
        {
            get { return _releationShip; }
        }
        private DataTable _releationShip;

        private void XtraFormInHosLogin_Load(object sender, EventArgs e)
        {
            InitData();
            InitLuePayId();
            InitLueSex();
            InitMarital();
            InitJob();
            InitNation();
            InitNationality();
            InitRelationship();
            InitPatientSource();
            BindAllProvince();
            InitAdmitDiag();
            this.InitUser();
            this.InitUS();
            this.dateBirthDay.Text = DateTime.Now.ToShortDateString();
            InitDateShow();
        }
        /// <summary>
        /// 增加入院诊断栏位
        /// add by ywk 2013年9月16日 14:34:30
        /// </summary>
        private void InitAdmitDiag()
        {
            LookUpWindow lookUpWindowAdmitDiag = new LookUpWindow();
            lookUpWindowAdmitDiag.SqlHelper = m_app.SqlHelper;
            this.lueAdmitDiag.ListWindow = lookUpWindowAdmitDiag;
            string sql = string.Empty;
            //防止需要此功能的是中医院
            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            if (!valueStr.Contains("sx"))
            {
                sql = "select py, wb, name, icd id from diagnosis  where valid='1' union select py, wb, name, icdid id from diagnosisothername where valid='1'";
            }
            else
            {
                sql = "select  py, wb, name, id   from diagnosisofchinese where valid='1' union select py, wb, name, id from diagnosischiothername where valid='1'";
            }
            DataTable DTdiag = m_app.SqlHelper.ExecuteDataTable(sql);
            Dictionary<string, int> colDiag = new Dictionary<string, int>();
            colDiag.Add("ID", 80);
            colDiag.Add("NAME", 160);
            SqlWordbook inWordBook = new SqlWordbook("inDiag", DTdiag, "ID", "NAME", colDiag, "ID//NAME//PY//WB");
            this.lueAdmitDiag.SqlWordbook = inWordBook;
        }

        /// <summary>
        /// 添加更新信息至ui  xll
        /// </summary>
        private void InitDateShow()
        {
            if (string.IsNullOrEmpty(this.m_noofInpat))
            {
                return; //新增时不用
            }
            string sql = string.Format(@"select * from inpatient where noofinpat='{0}';", this.m_noofInpat);
            DataTable dtInpatient = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            if (dtInpatient != null && dtInpatient.Rows.Count > 0)
            {
                DataRow dtrow = dtInpatient.Rows[0];
                patEntity = new PatientEntity();
                ConvertDTtoEntity(dtrow);
                InitUI();
            }
        }

        private void InitUI()
        {
            try
            {
                //by cyq 2012-09-13 
                // textEditNoOfRecord.Text = patEntity.NoOfRecord;
                textEditNoOfRecord.Text = patEntity.NoOfRecord;
                textEditPatID.Text = patEntity.PatID;
                txtBedID.Text = patEntity.OutBed;

                luePayId.CodeValue = patEntity.PayID;
                txtName.Text = patEntity.Name;
                lueSex.CodeValue = patEntity.SexID;
                dateBirthDay.Text = patEntity.Birth;
                txtAge.Text = patEntity.AgeStr.ToString();
                lueMarital.CodeValue = patEntity.Marital;
                lueCSD_ProvinceID.CodeValue = patEntity.CSD_ProvinceID;
                lueCSD_CityID.CodeValue = patEntity.CSD_CityID;
                lueCSD_DistrictID.CodeValue = patEntity.CSD_DistrictID;
                lueNationality.CodeValue = patEntity.NationalityID;
                lueNation.CodeValue = patEntity.NationID;
                txtIDCard.Text = patEntity.IDNO;
                lueJob.CodeValue = patEntity.JobID;
                txtOrganization.Text = patEntity.Organization;
                txtOfficeTel.Text = patEntity.OfficeTEL;
                txtOfficePostalCode.Text = patEntity.OfficePost;
                txtHousehold.Text = patEntity.NATIVEADDRESS;
                txtTel.Text = patEntity.NATIVETEL;
                txtPostalCode.Text = patEntity.NATIVEPOST;
                txtAddress.Text = patEntity.ADDRESS;
                textEditContactPerson.Text = patEntity.ContactPerson;
                lueRelationship.CodeValue = patEntity.RelationshipID;
                textEditContactAddress.Text = patEntity.ContactAddress;
                textEditContactTEL.Text = patEntity.ContactTEL;
                luePatientSource.CodeValue = patEntity.ORIGIN;
                switch (patEntity.ADMITWAY)
                {
                    case "1":
                        chkInHosType1.Checked = true;
                        break;
                    case "2":
                        chkInHosType2.Checked = true;
                        break;
                    case "3":
                        chkInHosType3.Checked = true;
                        break;
                    case "9":
                        chkInHosType4.Checked = true;
                        break;
                    default:
                        break;
                }

                switch (patEntity.ADMITINFO)
                {
                    case "1":
                        chkAdmitInfo1.Checked = true;
                        break;
                    case "2":
                        chkAdmitInfo2.Checked = true;
                        break;
                    case "3":
                        chkAdmitInfo3.Checked = true;
                        break;
                    case "4":
                        chkAdmitInfo4.Checked = true;
                        break;
                    default:
                        break;
                }
                lookUpEditorUser1.CodeValue = patEntity.CLINICDOCTOR;
                this.lookUpEditorUser2.CodeValue = patEntity.RESIDENT;
                this.lookUpEditorUser3.CodeValue = patEntity.ATTEND;
                this.lookUpEditorUser4.CodeValue = patEntity.CHIEF;
                textEdit_csd.Text = patEntity.CSD_ProvinceID;
                textEdit_JG.Text = patEntity.JG_ProvinceID;
                textEdit_admitdiagnosis.Text = patEntity.ADMITDIAGNOSIS;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ConvertDTtoEntity(DataRow dtrow)
        {
            patEntity.ADDRESS = dtrow["ADDRESS"].ToString();
            patEntity.ADMITBED = dtrow["ADMITBED"].ToString();
            patEntity.AdmitDate = dtrow["ADMITDATE"].ToString();
            patEntity.AdmitDeptID = dtrow["ADMITDEPT"].ToString();
            patEntity.ADMITDIAGNOSIS = dtrow["ADMITDIAGNOSIS"].ToString();
            patEntity.ADMITINFO = dtrow["ADMITINFO"].ToString();
            patEntity.AdmitWardID = dtrow["ADMITWARD"].ToString();
            patEntity.ADMITWAY = dtrow["ADMITWAY"].ToString();
            if (!string.IsNullOrEmpty(dtrow["AGE"].ToString()))
                patEntity.Age = Convert.ToInt32(dtrow["AGE"]);
            patEntity.AgeStr = dtrow["AGESTR"].ToString();
            patEntity.ATTEND = dtrow["ATTEND"].ToString();
            patEntity.ATTENDLEVEL = dtrow["ATTENDLEVEL"].ToString();
            patEntity.Birth = dtrow["BIRTH"].ToString();
            patEntity.CARDNO = dtrow["CARDNO"].ToString();
            patEntity.CHIEF = dtrow["CHIEF"].ToString();
            patEntity.CLINICDIAGNOSIS = dtrow["CLINICDIAGNOSIS"].ToString();
            patEntity.CLINICDOCTOR = dtrow["CLINICDOCTOR"].ToString();
            patEntity.ContactAddress = dtrow["CONTACTADDRESS"].ToString();
            patEntity.CONTACTOFFICE = dtrow["CONTACTOFFICE"].ToString();
            patEntity.ContactPerson = dtrow["CONTACTPERSON"].ToString();
            patEntity.CONTACTPOST = dtrow["CONTACTPOST"].ToString();
            patEntity.ContactTEL = dtrow["CONTACTTEL"].ToString();
            if (!string.IsNullOrEmpty(dtrow["CPSTATUS"].ToString()))
                patEntity.CPSTATUS = Convert.ToInt32(dtrow["CPSTATUS"]);
            patEntity.CRITICALLEVEL = dtrow["CRITICALLEVEL"].ToString();
            patEntity.CSD_CityID = dtrow["COUNTYID"].ToString();
            patEntity.CSD_DistrictID = dtrow["DISTRICTID"].ToString();
            patEntity.CSD_ProvinceID = dtrow["PROVINCEID"].ToString();
            patEntity.EDU = dtrow["EDU"].ToString();
            if (!string.IsNullOrEmpty(dtrow["EDUC"].ToString()))
                patEntity.EDUC = Convert.ToInt32(dtrow["EDUC"]);
            if (!string.IsNullOrEmpty(dtrow["EMPHASIS"].ToString()))
                patEntity.EMPHASIS = Convert.ToInt32(dtrow["EMPHASIS"]);
            patEntity.HKDZ_CityID = dtrow["HKZDCITYID"].ToString();
            patEntity.HKDZ_DistrictID = dtrow["HKZDDISTRICTID"].ToString();
            patEntity.HKDZ_ProvinceID = dtrow["HKDZPROVICEID"].ToString();
            patEntity.IDNO = dtrow["IDNO"].ToString();
            //patEntity.Iem_Mainpage_NO = dtrow[""].ToString();
            if (!string.IsNullOrEmpty(dtrow["INCOUNT"].ToString()))
                patEntity.InCount = Convert.ToInt32(dtrow["INCOUNT"]);
            patEntity.INNERPIX = dtrow["INNERPIX"].ToString();
            patEntity.INSURANCE = dtrow["INSURANCE"].ToString();
            patEntity.INWARDDATE = dtrow["INWARDDATE"].ToString();
            if (!string.IsNullOrEmpty(dtrow["ISBABY"].ToString()))
                patEntity.ISBABY = Convert.ToInt32(dtrow["ISBABY"]);
            patEntity.JG_CityID = dtrow["NATIVEPLACE_C"].ToString();
            patEntity.JG_ProvinceID = dtrow["NATIVEPLACE_P"].ToString();
            patEntity.JobID = dtrow["JOBID"].ToString();
            //patEntity.JobName = dtrow[""].ToString();
            patEntity.Marital = dtrow["MARITAL"].ToString();
            patEntity.MEDICAREID = dtrow["MEDICAREID"].ToString();
            if (!string.IsNullOrEmpty(dtrow["MEDICAREQUOTA"].ToString()))
                patEntity.MEDICAREQUOTA = Convert.ToInt32(dtrow["MEDICAREQUOTA"]);
            patEntity.MEMO = dtrow["MEMO"].ToString();
            if (!string.IsNullOrEmpty(dtrow["MOTHER"].ToString()))
                patEntity.MOTHER = Convert.ToInt32(dtrow["MOTHER"]);
            patEntity.Name = dtrow["NAME"].ToString();
            patEntity.NationalityID = dtrow["NATIONALITYID"].ToString();
            // patEntity.NationalityName = dtrow[""].ToString();
            patEntity.NationID = dtrow["NATIONID"].ToString();
            // patEntity.NationName = dtrow[""].ToString();
            patEntity.NATIVEADDRESS = dtrow["NATIVEADDRESS"].ToString();
            patEntity.NATIVEPOST = dtrow["NATIVEPOST"].ToString();
            patEntity.NATIVETEL = dtrow["NATIVETEL"].ToString();
            patEntity.NoOfcClinic = dtrow["NOOFCLINIC"].ToString();
            if (!string.IsNullOrEmpty(dtrow["NOOFINPAT"].ToString()))
                patEntity.NoOfInpat = Convert.ToInt32(dtrow["NOOFINPAT"]);
            patEntity.NoOfRecord = dtrow["NOOFRECORD"].ToString();
            patEntity.OFFERER = dtrow["OFFERER"].ToString();
            patEntity.OfficePlace = dtrow["OFFICEPLACE"].ToString();
            patEntity.OfficePost = dtrow["OFFICEPOST"].ToString();
            patEntity.OfficeTEL = dtrow["OFFICETEL"].ToString();
            patEntity.OPERATOR = dtrow["OPERATOR"].ToString();
            patEntity.Organization = dtrow["ORGANIZATION"].ToString();
            patEntity.ORIGIN = dtrow["ORIGIN"].ToString();
            patEntity.OutBed = dtrow["OutBED"].ToString();
            patEntity.OUTDIAGNOSIS = dtrow["OUTDIAGNOSIS"].ToString();
            patEntity.OUTHOSDATE = dtrow["OUTHOSDATE"].ToString();
            patEntity.OutHosDeptID = dtrow["OUTHOSDEPT"].ToString();
            patEntity.OutHosWardID = dtrow["OUTHOSWARD"].ToString();
            patEntity.OUTPIX = dtrow["OUTPIX"].ToString();
            if (!string.IsNullOrEmpty(dtrow["OUTWARDBED"].ToString()))
                patEntity.OUTWARDBED = Convert.ToInt32(dtrow["OUTWARDBED"]);
            patEntity.OutWardDate = dtrow["OUTWARDDATE"].ToString();
            patEntity.OUTWAY = dtrow["OUTWAY"].ToString();
            patEntity.PatID = dtrow["PATID"].ToString();
            patEntity.PatNoOfHis = dtrow["PATNOOFHIS"].ToString();
            patEntity.PayID = dtrow["PAYID"].ToString();
            //patEntity.PayName = dtrow[""].ToString();
            patEntity.PY = dtrow["PY"].ToString();
            patEntity.RelationshipID = dtrow["RELATIONSHIP"].ToString();
            //patEntity.RelationshipName = dtrow[""].ToString();
            patEntity.RELIGION = dtrow["RELIGION"].ToString();
            patEntity.RESIDENT = dtrow["RESIDENT"].ToString();
            patEntity.SexID = dtrow["SEXID"].ToString();
            patEntity.SocialCare = dtrow["SOCIALCARE"].ToString();
            patEntity.SOLARTERMS = dtrow["SOLARTERMS"].ToString();
            if (!string.IsNullOrEmpty(dtrow["STATUS"].ToString()))
                patEntity.STATUS = Convert.ToInt32(dtrow["STATUS"]);
            patEntity.STYLE = dtrow["STYLE"].ToString();
            if (!string.IsNullOrEmpty(dtrow["STYLE"].ToString()))
                patEntity.TOTALDAYS = Convert.ToInt32(dtrow["TOTALDAYS"]);
            patEntity.VOUCHERSCODE = dtrow["VOUCHERSCODE"].ToString();
            patEntity.WB = dtrow["WB"].ToString();
            patEntity.XZZ_CityID = dtrow["XZZCITYID"].ToString();
            patEntity.XZZ_DistrictID = dtrow["XZZDISTRICTID"].ToString();
            patEntity.XZZ_Post = dtrow["XZZPOST"].ToString();
            patEntity.XZZ_ProvinceID = dtrow["XZZPROVICEID"].ToString();
            patEntity.XZZ_TEL = dtrow["XZZTEL"].ToString();
            //  textEdit_csd.Text = dtrow["provinceid"].ToString(); ;
        }

        //初始化医师数据
        private void InitUser()
        {
            this.lookUpWindowUser1.SqlHelper = m_app.SqlHelper;
            this.lookUpWindowUser2.SqlHelper = m_app.SqlHelper;
            this.lookUpWindowUser3.SqlHelper = m_app.SqlHelper;
            this.lookUpWindowUser4.SqlHelper = m_app.SqlHelper;

            string sql = "select ID,NAME,PY,WB from users where valid = 1 and deptid='" + m_app.User.CurrentDeptId + "'";
            DataTable Name = m_app.SqlHelper.ExecuteDataTable(sql);
            Name.Columns["ID"].Caption = "编号";
            Name.Columns["NAME"].Caption = "医师姓名";
            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 50);
            cols.Add("NAME", 70);

            SqlWordbook nameWordBook = new SqlWordbook("queryname", Name, "ID", "NAME", cols, "ID//NAME//PY//WB");
            this.lookUpEditorUser1.SqlWordbook = nameWordBook;
            this.lookUpEditorUser2.SqlWordbook = nameWordBook;
            this.lookUpEditorUser3.SqlWordbook = nameWordBook;
            this.lookUpEditorUser4.SqlWordbook = nameWordBook;
        }

        private void InitData()
        {
            //省份、城市、区县、部门、病区
            _province = m_app.SqlHelper.ExecuteDataTable("select a.provinceid ID,a.provincename Name,a.py,a.wb from s_province a");
            _city = m_app.SqlHelper.ExecuteDataTable("select a.cityid ID,a.cityname Name,a.provinceid,a.py,a.wb  from s_city a ");
            _qxian = m_app.SqlHelper.ExecuteDataTable("select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a");
            //_dept = m_app.SqlHelper.ExecuteDataTable("select a.ID,a.NAME,a.py,a.wb from department a  where a.valid='1'");
            //_ward = m_app.SqlHelper.ExecuteDataTable(@"select a.ID,a.NAME,a.py,a.wb,b.deptid from ward a ,dept2ward b where     a.id=b.wardid  and a.valid='1' ");
        }

        #region UI上lue的数据源赋值
        /// <summary>
        /// 医疗付款方式
        /// </summary>
        private void InitLuePayId()
        {
            if (luePayId.SqlWordbook == null)
                BindLueData(luePayId, 1);
        }

        /// <summary>
        /// 病人性别
        /// </summary>
        private void InitLueSex()
        {
            if (lueSex.SqlWordbook == null)
                BindLueData(lueSex, 2);
        }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        private void InitMarital()
        {
            if (lueMarital.SqlWordbook == null)
                BindLueData(lueMarital, 3);
        }

        /// <summary>
        /// 职业代码
        /// </summary>
        private void InitJob()
        {
            if (lueJob.SqlWordbook == null)
                BindLueData(lueJob, 4);
        }

        /// <summary>
        /// 病人来源
        /// </summary>
        private void InitPatientSource()
        {
            if (luePatientSource.SqlWordbook == null)
                BindLueData(luePatientSource, 30);
        }

        ///// <summary>
        ///// 市代码
        ///// </summary>
        //private void InitProvAndCity()
        //{
        //    BindLueData(lueProvice, 4);
        //}

        //private DataTable m_DataTableCountry;
        ///// <summary>
        ///// 区县代码
        ///// </summary>
        ///// <param name="lueInfo"></param>
        ///// <param name="queryType"></param>
        //private void BindLueCountryData(LookUpEditor lueInfo, Decimal queryType)
        //{
        //    LookUpWindow lupInfo = new LookUpWindow();
        //    lupInfo.SqlHelper = m_app.SqlHelper;
        //    if (m_DataTableCountry == null)
        //        m_DataTableCountry = GetEditroData(queryType);

        //    Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
        //    columnwidth.Add("名称", lueInfo.Width);
        //    SqlWordbook sqlWordBook = new SqlWordbook("ID", m_DataTableCountry, "ID", "Name", columnwidth, true);

        //    lueInfo.SqlWordbook = sqlWordBook;
        //    lueInfo.ListWindow = lupInfo;
        //}
        ///// <summary>
        ///// 区县代码
        ///// </summary>
        ///// <param name="lueInfo"></param>
        ///// <param name="queryType"></param>
        //private void BindLueCountryData(LookUpEditor lueInfo, DataTable dataTable)
        //{
        //    LookUpWindow lupInfo = new LookUpWindow();
        //    lupInfo.SqlHelper = m_app.SqlHelper;

        //    Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
        //    columnwidth.Add("名称", lueInfo.Width);
        //    SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

        //    lueInfo.SqlWordbook = sqlWordBook;
        //    lueInfo.ListWindow = lupInfo;
        //}

        /// <summary>
        /// 民族代码
        /// </summary>
        private void InitNation()
        {
            if (lueNation.SqlWordbook == null)
                BindLueData(lueNation, 6);
        }

        /// <summary>
        /// 国籍代码
        /// </summary>
        private void InitNationality()
        {
            if (lueNationality.SqlWordbook == null)
                BindLueData(lueNationality, 7);
        }

        /// <summary>
        /// 联系关系
        /// </summary>
        private void InitRelationship()
        {
            if (lueRelationship.SqlWordbook == null)
                BindLueData(lueRelationship, 8);
        }

        /// <summary>
        /// 绑定页面所有省区信息
        /// </summary>
        private void BindAllProvince()
        {
            //string sql = @"select a.provinceid ID,a.provincename Name,a.py,a.wb from s_province a";
            //DataTable province = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            //BindProvince(lueCSD_ProvinceID, Province);

            //BindCity(lueCSD_CityID, "");
            if (lueCSD_DistrictID.SqlWordbook == null)
                BindDistrict(lueCSD_DistrictID, QXian);
            if (lueCSD_CityID.SqlWordbook == null)
                BindCity(lueCSD_CityID, City);
            if (lueCSD_ProvinceID.SqlWordbook == null)
                BindProvince(lueCSD_ProvinceID, Province);
        }

        #endregion

        #region 绑定LUE

        /// <summary>
        /// 绑定省级地区
        /// </summary>
        /// <param name="lueinfo"></param>
        private void BindProvince(LookUpEditor lueInfo, DataTable dataTable)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_app.SqlHelper;
            dataTable.Columns["NAME"].Caption = "省（区、市）";
            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("NAME", lueInfo.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            lueInfo.SqlWordbook = sqlWordBook;
            lueInfo.ListWindow = lupInfo;

        }
        //DataTable m_GetCityData;//市级数据
        ///// <summary>
        ///// 绑定市级地区
        ///// </summary>
        ///// <param name="lueinfo"></param>
        private void BindCity(LookUpEditor lueinfo, string provinceid)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_app.SqlHelper;


            string sql = string.Format(@"select a.cityid ID,a.cityname Name,a.provinceid,a.py,a.wb  from s_city a where a.provinceid = '{0}' order by a.cityid asc", provinceid);
            //if (m_GetCityData==null)
            //    m_GetCityData = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            DataTable province = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            province.Columns["NAME"].Caption = "市";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("NAME", 120);
            SqlWordbook deptWordBook = new SqlWordbook("querybook", province, "ID", "NAME", cols, true);//province
            lueinfo.SqlWordbook = deptWordBook;
            lueinfo.ListWindow = lupInfo;


            //原来的选择省份后，要选择下拉列表，联动的市才改变
            //现在改为选第一个市 add by ywk 2012年5月4日 11:53:37
            if (province.Rows.Count > 0)
            {
                lueinfo.CodeValue = province.DefaultView[0]["ID"].ToString();
            }
        }
        /// <summary>
        /// 绑定市级地区
        /// </summary>
        /// <param name="lueinfo"></param>
        private void BindCity(LookUpEditor lueinfo, DataTable city)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_app.SqlHelper;
            //string sql = string.Format(@"select a.cityid ID,a.cityname Name,a.provinceid,a.py,a.wb  from s_city a where a.provinceid = '{0}'", provinceid);
            //DataTable province = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            city.Columns["NAME"].Caption = "市";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NAME", 120);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", city, "ID", "NAME", cols, true);
            lueinfo.SqlWordbook = deptWordBook;
            lueinfo.ListWindow = lupInfo;

            //原来的选择省份后，要选择下拉列表，联动的市才改变
            //现在改为选第一个市 add by ywk 2012年5月4日 11:53:37
            if (city.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(lueCSD_ProvinceID.CodeValue))
                {
                    lueinfo.CodeValue = city.Rows[0]["ID"].ToString();
                }
                else
                {
                    lueinfo.CodeValue = string.Empty;
                }
            }
        }

        /// <summary>
        /// 绑定县级地区
        /// </summary>
        /// <param name="lueinfo"></param>
        private void BindDistrict(LookUpEditor lueinfo, DataTable qxian)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_app.SqlHelper;
            //string sql = string.Format(@"select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a where a.cityid = '{0}'", cityid);
            //DataTable province = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            qxian.Columns["NAME"].Caption = "县";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NAME", 120);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", qxian, "ID", "NAME", cols);
            lueinfo.SqlWordbook = deptWordBook;
            lueinfo.ListWindow = lupInfo;

            //原来的选择市后，要选择下拉列表，联动的县才改变
            //现在改为选第一个县 add by ywk 2012年5月4日 11:55:39
            if (qxian.Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(lueCSD_CityID.CodeValue))
                {
                    lueinfo.CodeValue = qxian.Rows[0]["ID"].ToString();
                }
                else
                {
                    lueinfo.CodeValue = string.Empty;
                }
            }
        }
        ///// <summary>
        ///// 绑定县级地区
        ///// </summary>
        ///// <param name="lueinfo"></param>
        private void BindDistrict(LookUpEditor lueinfo, string cityid)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_app.SqlHelper;
            string sql = string.Format(@"select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a where a.cityid = '{0}'", cityid);
            DataTable province = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            province.Columns["NAME"].Caption = "县";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NAME", 120);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", province, "ID", "NAME", cols);
            lueinfo.SqlWordbook = deptWordBook;
            lueinfo.ListWindow = lupInfo;

            //原来的选择市后，要选择下拉列表，联动的县才改变
            //现在改为选第一个县 add by ywk 2012年5月4日 11:55:39
            if (province.Rows.Count > 0)
            {
                lueinfo.CodeValue = province.DefaultView[0]["ID"].ToString();
            }
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
        #endregion
        /// <summary>
        /// 下拉框联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueProvice_CodeValueChanged(object sender, EventArgs e)
        {
            //if (lueCSD_ProvinceID.CodeValue != null)
            //{
            LookUpEditor lue = ((LookUpEditor)sender);
            string val = lue.CodeValue;
            if (string.IsNullOrEmpty(val)) return;

            if (lue.Name == "lueCSD_ProvinceID")
            {
                lueCSD_CityID.SqlWordbook.ExtraCondition = "provinceid = '" + val + "'";
                DataRow[] m_row = lueCSD_CityID.SqlWordbook.BookData.Select(" provinceid='" + val + "'");
                if (m_row.Length > 0)
                {
                    if (!string.IsNullOrEmpty(lueCSD_ProvinceID.CodeValue))
                    {
                        lueCSD_CityID.CodeValue = m_row[0]["ID"].ToString();
                    }
                    else
                    {
                        lueCSD_CityID.CodeValue = string.Empty;
                    }
                }
                //lueCSD_CityID.CodeValue = lueCSD_CityID.SqlWordbook.BookData.Rows[0]["ID"].ToString();
                //BindCity(lueCSD_CityID, provice);
            }

            else if (lue.Name == "lueCSD_CityID")
            {
                lueCSD_DistrictID.SqlWordbook.ExtraCondition = "cityid = '" + val + "'";
                //BindDistrict(lueCSD_DistrictID, provice);
                DataRow[] m_row = lueCSD_DistrictID.SqlWordbook.BookData.Select(" cityid='" + val + "'");
                if (m_row.Length > 0)
                {
                    if (!string.IsNullOrEmpty(lueCSD_CityID.CodeValue))
                    {
                        lueCSD_DistrictID.CodeValue = m_row[0]["ID"].ToString();
                    }
                    else
                    {
                        lueCSD_DistrictID.CodeValue = "";
                    }
                }

                //lueCSD_DistrictID.CodeValue = lueCSD_DistrictID.SqlWordbook.BookData.Select(" cityid='" + provice + "'")[0]["ID"].ToString();
            }
        }

        //将界面txtedit控件添加到集合
        private void InitUS()
        {
            try
            {
                m_TextEditControlList.Add(this.textEditNoOfRecord);
                m_TextEditControlList.Add(this.textEditPatID);
                m_TextEditControlList.Add(this.txtName);
                //m_TextEditControlList.Add(this.textEditSex);
                m_TextEditControlList.Add(this.txtAge);
                //m_TextEditControlList.Add(this.textEditProvince);
                //m_TextEditControlList.Add(this.textEditCounty);
                //m_TextEditControlList.Add(this.textEditNationality);
                m_TextEditControlList.Add(this.txtIDCard);
                m_TextEditControlList.Add(this.txtOrganization);
                m_TextEditControlList.Add(this.txtOfficeTel);
                m_TextEditControlList.Add(this.txtOfficePostalCode);
                m_TextEditControlList.Add(this.txtHousehold);
                m_TextEditControlList.Add(this.txtTel);
                m_TextEditControlList.Add(this.txtPostalCode);
                m_TextEditControlList.Add(this.txtAddress);
                m_TextEditControlList.Add(this.textEditContactPerson);
                //m_TextEditControlList.Add(this.textEditRelationShip);
                m_TextEditControlList.Add(this.textEditContactAddress);
                m_TextEditControlList.Add(this.textEditContactTEL);
                //m_TextEditControlList.Add(this.textEditPay);
                //m_TextEditControlList.Add(this.textEditOrigin);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Modify  by xlb 2013-04-07
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (patEntity == null)
            {
                patEntity = new PatientEntity();
            }
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                #region "弃用 by cyq 2012-10-15"
                //foreach (TextEdit te in m_TextEditControlList)
                //{
                //    if (te.Text.Trim() == "")
                //    {
                //        m_app.CustomMessageBox.MessageShow("输入值不能有空！");
                //        return;
                //    }
                //}
                //if (lueCSD_CityID.CodeValue == "" || lueCSD_DistrictID.CodeValue == "" ||
                //    this.lueCSD_ProvinceID.CodeValue == "" ||
                //    this.lueJob.CodeValue == "" ||
                //    this.lueMarital.CodeValue == "" ||
                //    this.lueNation.CodeValue == "" ||
                //    this.lueNationality.CodeValue == "" ||
                //    this.luePatientSource.CodeValue == "" ||
                //    this.luePayId.CodeValue == "" ||
                //    this.lueRelationship.CodeValue == "" ||
                //    this.lueSex.CodeValue == "")
                //{
                //    m_app.CustomMessageBox.MessageShow("输入值不能有空！");
                //    return;
                //}
                #endregion
                //画面输入项验证 add by cyq 2012-10-15
                string errorStr = checkItems();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    m_app.CustomMessageBox.MessageShow(errorStr);
                    return;
                }
                //病号号
                string noOfRecord = this.textEditNoOfRecord.Text;
                string patid = this.textEditPatID.Text;
                //新增时需要校验
                if (textEditNoOfRecord.Enabled)
                {
                    DataTable dt = DS_SqlHelper.ExecuteDataTable(@"select * from inpatient where noofrecord=@nOofRecord",
                                                 new SqlParameter[] { new SqlParameter("@nOofRecord", noOfRecord) }, CommandType.Text);//Modify by xlb 2013-06-08
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        m_app.CustomMessageBox.MessageShow("该病案编号已经存在");
                        return;
                    }

                    //string sql1 = "select * from inpatient where patid = '" + patid + "'";
                    DataTable dt1 = DS_SqlHelper.ExecuteDataTable("select * from inpatient where patid=@patId", new SqlParameter[] { new SqlParameter("@patId", patid) }, CommandType.Text);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        m_app.CustomMessageBox.MessageShow("该住院编号已经存在");
                        return;
                    }
                }
                patEntity.Name = this.txtName.Text;
                patEntity.Birth = dateBirthDay.DateTime.ToString("yyyy-MM-dd hh:mm:ss");
                patEntity.Age = DateTime.Now.Year - dateBirthDay.DateTime.Year;
                //patEntity.SexID = m_TextEditControlList[2].Text == "男" ? "1" : "0";
                patEntity.SexID = this.lueSex.CodeValue;
                patEntity.Marital = this.lueMarital.CodeValue;
                patEntity.ADMITDIAGNOSIS = textEdit_admitdiagnosis.Text.Trim();
                //if (this.checkEditMarry1.Checked)
                //{
                //    patEntity.Marital = "1";
                //}
                //else
                //{
                //    patEntity.Marital = "0";
                //}
                if (patEntity.Age > 1) //儿科如果接收小于一岁的 edit by ukey zhang 2017-10-17
                {
                    patEntity.AgeStr = DrectSoft.Common.Eop.PatientBasicInfo.CalcDisplayAge(dateBirthDay.DateTime, Convert.ToDateTime(patEntity.AdmitDate));
                }
                else
                {
                    patEntity.AgeStr = this.txtAge.Text;
                }
                patEntity.ISBABY = 0;
                if (patEntity.PatNoOfHis == null || patEntity.PatNoOfHis.ToString() == "")
                {
                    patEntity.PatNoOfHis = "";
                    patEntity.PatNoOfHis = textEditNoOfRecord.Text;
                }
                if (patEntity.NoOfcClinic == null || patEntity.NoOfcClinic.ToString() == "")
                {
                    patEntity.NoOfcClinic = "";
                    patEntity.NoOfcClinic = textEditNoOfRecord.Text;
                }

                patEntity.NoOfRecord = textEditNoOfRecord.Text;
                //  patEntity.NoOfcClinic = Guid.NewGuid().ToString().Substring(1, 14);
                //patEntity.NoOfRecord = Guid.NewGuid().ToString().Substring(1, 14);
                //patEntity.PatID = Guid.NewGuid().ToString().Substring(1, 14);
                patEntity.NoOfRecord = noOfRecord;
                patEntity.PatID = patid;

                patEntity.PY = "";
                patEntity.WB = "";
                //patEntity.CSD_ProvinceID = textEditProvince.Text;
                //  patEntity.CSD_ProvinceID = this.lueCSD_ProvinceID.CodeValue;
                patEntity.JG_ProvinceID = textEdit_JG.Text.Trim();
                patEntity.CSD_ProvinceID = textEdit_csd.Text.Trim();
                patEntity.CSD_CityID = this.lueCSD_CityID.CodeValue;
                patEntity.CSD_DistrictID = this.lueCSD_DistrictID.CodeValue;
                patEntity.NationID = this.lueNation.CodeValue;
                patEntity.NationalityID = this.lueNationality.CodeValue;

                //patEntity.CSD_DistrictID = textEditCounty.Text;
                //patEntity.NationID = textEditNation.Text;
                //patEntity.NationalityID = textEditNationality.Text;

                patEntity.Organization = txtOrganization.Text;
                patEntity.OfficePlace = txtOrganization.Text;
                patEntity.OfficeTEL = txtOfficeTel.Text;
                patEntity.OfficePost = txtOfficePostalCode.Text;
                patEntity.NATIVEADDRESS = txtHousehold.Text;
                patEntity.NATIVETEL = txtTel.Text;
                patEntity.NATIVEPOST = txtPostalCode.Text;
                patEntity.ADDRESS = txtAddress.Text;

                patEntity.ContactPerson = textEditContactPerson.Text;
                //patEntity.RelationshipID = textEditRelationShip.Text;
                patEntity.RelationshipID = this.lueRelationship.CodeValue;
                patEntity.ContactAddress = textEditContactAddress.Text;
                patEntity.ContactTEL = textEditContactTEL.Text;

                patEntity.CLINICDOCTOR = this.lookUpEditorUser1.CodeValue;
                patEntity.RESIDENT = this.lookUpEditorUser2.CodeValue;
                patEntity.ATTEND = this.lookUpEditorUser3.CodeValue;
                patEntity.CHIEF = this.lookUpEditorUser4.CodeValue;
                patEntity.JobID = lueJob.CodeValue;
                //patEntity.InCount = int.Parse(this.textEditInCount.Text);
                patEntity.STATUS = 1501;

                //patEntity.CRITICALLEVEL = this.textEditCriticalLevel.Text;
                //patEntity.ATTENDLEVEL = this.textEditAttendLevel.Text;
                //patEntity.PayID = this.textEditPay.Text;
                patEntity.PayID = this.luePayId.CodeValue;
                patEntity.ORIGIN = this.luePatientSource.CodeValue;
                //patEntity.ORIGIN = this.textEditOrigin.Text;
                //patEntity.ADMITWAY = this.textEditAdmitWay.Text;
                //patEntity.OUTWAY = this.textEditOutWay.Text;

                //入院途径
                if (chkInHosType1.Checked)
                    patEntity.ADMITWAY = "1";
                else if (chkInHosType2.Checked)
                    patEntity.ADMITWAY = "2";
                else if (chkInHosType3.Checked)
                    patEntity.ADMITWAY = "3";
                else
                    patEntity.ADMITWAY = "9";

                //新增的入院病情赋给实体  add by ywk 时间
                if (chkAdmitInfo1.Checked)
                    patEntity.ADMITINFO = "1";
                else if (chkAdmitInfo2.Checked)
                    patEntity.ADMITINFO = "2";
                else if (chkAdmitInfo3.Checked)
                    patEntity.ADMITINFO = "3";
                else if (chkAdmitInfo4.Checked)
                    patEntity.ADMITINFO = "4";
                else
                    patEntity.ADMITINFO = "";
                patEntity.OutHosDeptID = m_app.User.CurrentDeptId;
                patEntity.OutHosWardID = m_app.User.CurrentWardId;//出院科室和病区add by ywk 2012年9月3日 17:53:38
                patEntity.AdmitDeptID = m_app.User.CurrentDeptId;
                patEntity.AdmitWardID = m_app.User.CurrentWardId;
                patEntity.AdmitDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//时间有误 edit by ywk 2012年9月3日 17:24:47
                patEntity.INWARDDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                patEntity.ADMITBED = txtBedID.Text.ToString();//新增床位号!!.//add by ywk 2012年9月3日 17:53:38
                patEntity.OutBed = txtBedID.Text.ToString();//新增床位号!!.//add by ywk 2012年9月3日 17:56:01
                patEntity.IDNO = txtIDCard.Text.ToString();
                patEntity.ADMITDIAGNOSIS = textEdit_admitdiagnosis.Text.Trim();//新增入院诊断 add by ywk 2013年9月16日 14:43:32
                this.InsertInpatient(patEntity);
                refreashFlag = true;
                if (textEditNoOfRecord.Enabled)
                {
                    ClearPage();
                    this.textEditNoOfRecord.Focus();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 插入一条病人信息到数据库
        /// </summary>
        /// <param name="pat"></param>
        private void InsertInpatient(PatientEntity pat)
        {
            SqlParameter parNoofinpat = new SqlParameter("@noofinpat", SqlDbType.VarChar, 14);
            parNoofinpat.Value = pat.NoOfInpat;
            SqlParameter parPatNoOfHis = new SqlParameter("@patnoofhis", SqlDbType.VarChar, 14);
            parPatNoOfHis.Value = pat.PatNoOfHis;
            SqlParameter parNoofclinic = new SqlParameter("@Noofclinic", SqlDbType.VarChar, 32);
            parNoofclinic.Value = pat.NoOfcClinic;
            SqlParameter parNoofrecord = new SqlParameter("@Noofrecord", SqlDbType.VarChar, 32);
            parNoofrecord.Value = pat.NoOfRecord;
            SqlParameter parpatid = new SqlParameter("@patid", SqlDbType.VarChar, 32);
            parpatid.Value = pat.PatID;
            SqlParameter parInnerpix = new SqlParameter("@Innerpix", SqlDbType.VarChar, 32);
            parInnerpix.Value = pat.INNERPIX;
            SqlParameter paroutpix = new SqlParameter("@outpix", SqlDbType.VarChar, 32);
            paroutpix.Value = pat.OUTPIX;
            SqlParameter parName = new SqlParameter("@Name", SqlDbType.VarChar, 32);
            parName.Value = pat.Name;
            SqlParameter parpy = new SqlParameter("@py", SqlDbType.VarChar, 8);
            parpy.Value = pat.PY;
            SqlParameter parwb = new SqlParameter("@wb", SqlDbType.VarChar, 8);
            parwb.Value = pat.WB;
            SqlParameter parPayid = new SqlParameter("@payid", SqlDbType.VarChar, 4);
            parPayid.Value = pat.PayID;
            SqlParameter parORIGIN = new SqlParameter("@ORIGIN", SqlDbType.VarChar, 4);
            parORIGIN.Value = pat.ORIGIN;
            SqlParameter parInCount = new SqlParameter("@InCount", SqlDbType.Int);
            parInCount.Value = pat.InCount;
            SqlParameter parSexID = new SqlParameter("@sexid", SqlDbType.VarChar, 4);
            parSexID.Value = pat.SexID;
            SqlParameter parBirth = new SqlParameter("@Birth", SqlDbType.VarChar, 19);
            parBirth.Value = pat.Birth;
            SqlParameter parAge = new SqlParameter("@Age", SqlDbType.Int);
            parAge.Value = pat.Age;
            SqlParameter parAgeStr = new SqlParameter("@AgeStr", SqlDbType.VarChar, 16);
            parAgeStr.Value = pat.AgeStr;
            SqlParameter parIDNO = new SqlParameter("@IDNO", SqlDbType.VarChar, 18);
            parIDNO.Value = pat.IDNO;
            SqlParameter parMarital = new SqlParameter("@Marital", SqlDbType.VarChar, 12);
            parMarital.Value = pat.Marital;
            SqlParameter parJobID = new SqlParameter("@JobID", SqlDbType.VarChar, 4);
            parJobID.Value = pat.JobID;
            SqlParameter parCSDProvinceID = new SqlParameter("@CSDProvinceID", SqlDbType.VarChar, 14);
            parCSDProvinceID.Value = pat.CSD_ProvinceID;
            SqlParameter parCSDCityID = new SqlParameter("@CSDCityID", SqlDbType.VarChar, 14);
            parCSDCityID.Value = pat.CSD_CityID;
            SqlParameter parCSDDistrictID = new SqlParameter("@CSDDistrictID", SqlDbType.VarChar, 14);
            parCSDDistrictID.Value = pat.CSD_DistrictID;
            SqlParameter parNationID = new SqlParameter("@NationID", SqlDbType.VarChar, 14);
            parNationID.Value = pat.NationID;
            SqlParameter parNationalityID = new SqlParameter("@NationalityID", SqlDbType.VarChar, 14);
            parNationalityID.Value = pat.NationalityID;
            SqlParameter parJGProvinceID = new SqlParameter("@JGProvinceID", SqlDbType.VarChar, 14);
            parJGProvinceID.Value = pat.JG_ProvinceID;
            SqlParameter parJGCityID = new SqlParameter("@JGCityID", SqlDbType.VarChar, 14);
            parJGCityID.Value = pat.JG_CityID;
            SqlParameter parOrganization = new SqlParameter("@Organization", SqlDbType.VarChar, 64);
            parOrganization.Value = pat.Organization;
            SqlParameter parOfficePlace = new SqlParameter("@OfficePlace", SqlDbType.VarChar, 64);
            parOfficePlace.Value = pat.OfficePlace;
            SqlParameter parOfficeTEL = new SqlParameter("@OfficeTEL", SqlDbType.VarChar, 16);
            parOfficeTEL.Value = pat.OfficeTEL;
            SqlParameter parOfficePost = new SqlParameter("@OfficePost", SqlDbType.VarChar, 16);
            parOfficePost.Value = pat.OfficePost;
            SqlParameter parHKDZProvinceID = new SqlParameter("@HKDZProvinceID", SqlDbType.VarChar, 14);
            parHKDZProvinceID.Value = pat.HKDZ_ProvinceID;
            SqlParameter parHKDZCityID = new SqlParameter("@HKDZCityID", SqlDbType.VarChar, 14);
            parHKDZCityID.Value = pat.HKDZ_CityID;
            SqlParameter parHKDZDistrictID = new SqlParameter("@HKDZDistrictID", SqlDbType.VarChar, 14);
            parHKDZDistrictID.Value = pat.HKDZ_DistrictID;
            SqlParameter parNATIVEPOST = new SqlParameter("@NATIVEPOST", SqlDbType.VarChar, 16);
            parNATIVEPOST.Value = pat.NATIVEPOST;
            SqlParameter parNATIVETEL = new SqlParameter("@NATIVETEL", SqlDbType.VarChar, 16);
            parNATIVETEL.Value = pat.NATIVETEL;
            SqlParameter parNATIVEADDRESS = new SqlParameter("@NATIVEADDRESS", SqlDbType.VarChar, 64);
            parNATIVEADDRESS.Value = pat.NATIVEADDRESS;
            SqlParameter parADDRESS = new SqlParameter("@ADDRESS", SqlDbType.VarChar, 255);
            parADDRESS.Value = pat.ADDRESS;
            SqlParameter parContactPerson = new SqlParameter("@ContactPerson", SqlDbType.VarChar, 32);
            parContactPerson.Value = pat.ContactPerson;
            SqlParameter parRelationshipID = new SqlParameter("@RelationshipID", SqlDbType.VarChar, 14);
            parRelationshipID.Value = pat.RelationshipID;
            SqlParameter parContactAddress = new SqlParameter("@ContactAddress", SqlDbType.VarChar, 255);
            parContactAddress.Value = pat.ContactAddress;
            SqlParameter parContactTEL = new SqlParameter("@ContactTEL", SqlDbType.VarChar, 16);
            parContactTEL.Value = pat.ContactTEL;
            SqlParameter parCONTACTOFFICE = new SqlParameter("@CONTACTOFFICE", SqlDbType.VarChar, 64);
            parCONTACTOFFICE.Value = pat.CONTACTOFFICE;
            SqlParameter parCONTACTPOST = new SqlParameter("@CONTACTPOST", SqlDbType.VarChar, 16);
            parCONTACTPOST.Value = pat.CONTACTPOST;
            SqlParameter parOFFERER = new SqlParameter("@OFFERER", SqlDbType.VarChar, 64);
            parOFFERER.Value = pat.OFFERER;
            SqlParameter parSocialCare = new SqlParameter("@SocialCare", SqlDbType.VarChar, 14);
            parSocialCare.Value = pat.SocialCare;
            SqlParameter parINSURANCE = new SqlParameter("@INSURANCE", SqlDbType.VarChar, 32);
            parINSURANCE.Value = pat.INSURANCE;
            SqlParameter parCARDNO = new SqlParameter("@CARDNO", SqlDbType.VarChar, 32);
            parCARDNO.Value = pat.CARDNO;
            SqlParameter parADMITINFO = new SqlParameter("@ADMITINFO", SqlDbType.VarChar, 14);
            parADMITINFO.Value = pat.ADMITINFO;
            SqlParameter parAdmitDeptID = new SqlParameter("@AdmitDeptID", SqlDbType.VarChar, 14);
            parAdmitDeptID.Value = pat.AdmitDeptID;
            SqlParameter parAdmitWardID = new SqlParameter("@AdmitWardID", SqlDbType.VarChar, 14);
            parAdmitWardID.Value = pat.AdmitWardID;
            SqlParameter parADMITBED = new SqlParameter("@ADMITBED", SqlDbType.VarChar, 14);
            parADMITBED.Value = pat.ADMITBED;
            SqlParameter parAdmitDate = new SqlParameter("@AdmitDate", SqlDbType.VarChar, 19);
            parAdmitDate.Value = pat.AdmitDate;
            SqlParameter parINWARDDATE = new SqlParameter("@INWARDDATE", SqlDbType.VarChar, 19);
            parINWARDDATE.Value = pat.INWARDDATE;
            SqlParameter parADMITDIAGNOSIS = new SqlParameter("@ADMITDIAGNOSIS", SqlDbType.VarChar, 14);
            parADMITDIAGNOSIS.Value = pat.ADMITDIAGNOSIS;
            SqlParameter parOutWardDate = new SqlParameter("@OutWardDate", SqlDbType.VarChar, 19);
            parOutWardDate.Value = pat.OutWardDate;
            SqlParameter parOutHosDeptID = new SqlParameter("@OutHosDeptID", SqlDbType.VarChar, 14);
            parOutHosDeptID.Value = pat.OutHosDeptID;
            SqlParameter parOutHosWardID = new SqlParameter("@OutHosWardID", SqlDbType.VarChar, 14);
            parOutHosWardID.Value = pat.OutHosWardID;
            SqlParameter parOutBed = new SqlParameter("@OutBed", SqlDbType.VarChar, 14);
            parOutBed.Value = pat.OutBed;
            SqlParameter parOUTHOSDATE = new SqlParameter("@OUTHOSDATE", SqlDbType.VarChar, 19);
            parOUTHOSDATE.Value = pat.OUTHOSDATE;
            SqlParameter parOUTDIAGNOSIS = new SqlParameter("@OUTDIAGNOSIS", SqlDbType.VarChar, 14);
            parOUTDIAGNOSIS.Value = pat.OUTDIAGNOSIS;

            SqlParameter parTOTALDAYS = new SqlParameter("@TOTALDAYS", SqlDbType.Int);
            parTOTALDAYS.Value = pat.TOTALDAYS;
            SqlParameter parCLINICDIAGNOSIS = new SqlParameter("@CLINICDIAGNOSIS", SqlDbType.VarChar, 14);
            parCLINICDIAGNOSIS.Value = pat.CLINICDIAGNOSIS;
            SqlParameter parSOLARTERMS = new SqlParameter("@SOLARTERMS", SqlDbType.VarChar, 16);
            parSOLARTERMS.Value = pat.SOLARTERMS;
            SqlParameter parADMITWAY = new SqlParameter("@ADMITWAY", SqlDbType.VarChar, 14);
            parADMITWAY.Value = pat.ADMITWAY;
            SqlParameter parOUTWAY = new SqlParameter("@OUTWAY", SqlDbType.VarChar, 14);
            parOUTWAY.Value = pat.OUTWAY;
            SqlParameter parCLINICDOCTOR = new SqlParameter("@CLINICDOCTOR", SqlDbType.VarChar, 14);
            parCLINICDOCTOR.Value = pat.CLINICDOCTOR;
            SqlParameter parRESIDENT = new SqlParameter("@RESIDENT", SqlDbType.VarChar, 14);
            parRESIDENT.Value = pat.RESIDENT;
            SqlParameter parATTEND = new SqlParameter("@ATTEND", SqlDbType.VarChar, 14);
            parATTEND.Value = pat.ATTEND;
            SqlParameter parCHIEF = new SqlParameter("@CHIEF", SqlDbType.VarChar, 14);
            parCHIEF.Value = pat.CHIEF;
            SqlParameter parEDU = new SqlParameter("@EDU", SqlDbType.VarChar, 14);
            parEDU.Value = pat.EDU;
            SqlParameter parEDUC = new SqlParameter("@EDUC", SqlDbType.Int);
            parEDUC.Value = pat.EDUC;
            SqlParameter parRELIGION = new SqlParameter("@RELIGION", SqlDbType.VarChar, 32);
            parRELIGION.Value = pat.RELIGION;
            SqlParameter parSTATUS = new SqlParameter("@STATUS", SqlDbType.Int);
            parSTATUS.Value = pat.STATUS;
            SqlParameter parCRITICALLEVEL = new SqlParameter("@CRITICALLEVEL", SqlDbType.VarChar, 14);
            parCRITICALLEVEL.Value = pat.CRITICALLEVEL;
            SqlParameter parATTENDLEVEL = new SqlParameter("@ATTENDLEVEL", SqlDbType.VarChar, 14);
            parATTENDLEVEL.Value = pat.ATTENDLEVEL;
            SqlParameter parEMPHASIS = new SqlParameter("@EMPHASIS", SqlDbType.Int);
            parEMPHASIS.Value = pat.EMPHASIS;
            SqlParameter parISBABY = new SqlParameter("@ISBABY", SqlDbType.Int);
            parISBABY.Value = pat.ISBABY;
            SqlParameter parMOTHER = new SqlParameter("@MOTHER", SqlDbType.Int);
            parMOTHER.Value = pat.MOTHER;
            SqlParameter parMEDICAREID = new SqlParameter("@MEDICAREID", SqlDbType.VarChar, 14);
            parMEDICAREID.Value = pat.MEDICAREID;
            SqlParameter parMEDICAREQUOTA = new SqlParameter("@MEDICAREQUOTA", SqlDbType.Int);
            parMEDICAREQUOTA.Value = pat.MEDICAREQUOTA;
            SqlParameter parVOUCHERSCODE = new SqlParameter("@VOUCHERSCODE", SqlDbType.VarChar, 14);
            parVOUCHERSCODE.Value = pat.VOUCHERSCODE;
            SqlParameter parSTYLE = new SqlParameter("@STYLE", SqlDbType.VarChar, 14);
            parSTYLE.Value = pat.STYLE;
            SqlParameter parOPERATOR = new SqlParameter("@OPERATOR", SqlDbType.VarChar, 14);
            parOPERATOR.Value = pat.OPERATOR;
            SqlParameter parMEMO = new SqlParameter("@MEMO", SqlDbType.VarChar, 64);
            parMEMO.Value = pat.MEMO;
            SqlParameter parCPSTATUS = new SqlParameter("@CPSTATUS", SqlDbType.Int);
            parCPSTATUS.Value = pat.CPSTATUS;
            SqlParameter parOUTWARDBED = new SqlParameter("@OUTWARDBED", SqlDbType.Int);
            parOUTWARDBED.Value = pat.OUTWARDBED;
            SqlParameter parXZZProvinceID = new SqlParameter("@XZZProvinceID", SqlDbType.VarChar, 14);
            parXZZProvinceID.Value = pat.XZZ_ProvinceID;
            SqlParameter parXZZCityID = new SqlParameter("@XZZCityID", SqlDbType.VarChar, 14);
            parXZZCityID.Value = pat.XZZ_CityID;
            SqlParameter parXZZDistrictID = new SqlParameter("@XZZDistrictID", SqlDbType.VarChar, 14);
            parXZZDistrictID.Value = pat.XZZ_DistrictID;
            SqlParameter parXZZTEL = new SqlParameter("@XZZTEL", SqlDbType.VarChar, 14);
            parXZZTEL.Value = pat.XZZ_TEL;
            SqlParameter parXZZPost = new SqlParameter("@XZZPost", SqlDbType.VarChar, 16);
            parXZZPost.Value = pat.XZZ_Post;

            SqlParameter[] paraColl = new SqlParameter[] {parNoofinpat, parPatNoOfHis,parNoofclinic,parNoofrecord,parpatid,
                 parInnerpix, paroutpix ,parName,  parpy,  parwb,  parPayid,  parORIGIN , parInCount ,parSexID , parBirth , 
                 parAge , parAgeStr,  parIDNO , parMarital, parJobID,  parCSDProvinceID,  parCSDCityID,  parCSDDistrictID , 
                 parNationID,   parNationalityID,  parJGProvinceID,  parJGCityID , parOrganization , parOfficePlace, parOfficeTEL,  
                 parOfficePost ,parHKDZProvinceID , parHKDZCityID,  parHKDZDistrictID, parNATIVEPOST , parNATIVETEL , parNATIVEADDRESS, 
                 parADDRESS,  parContactPerson,  parRelationshipID,  parContactAddress,  parContactTEL, parCONTACTOFFICE,  parCONTACTPOST ,
                parOFFERER,  parSocialCare , parINSURANCE , parCARDNO,  parADMITINFO , parAdmitDeptID ,parAdmitWardID ,
                parADMITBED,  parAdmitDate , parINWARDDATE,  parADMITDIAGNOSIS , parOutWardDate, parOutHosDeptID , parOutHosWardID ,
                parOutBed,parOUTHOSDATE,  parOUTDIAGNOSIS,  parTOTALDAYS,  parCLINICDIAGNOSIS, parSOLARTERMS ,
                parADMITWAY ,parOUTWAY, parCLINICDOCTOR,  parRESIDENT,  parATTEND,  parCHIEF, parEDU,  parEDUC,
                 parRELIGION,  parSTATUS , parCRITICALLEVEL, parATTENDLEVEL,  parEMPHASIS , parISBABY, parMOTHER,
                parMEDICAREID , parMEDICAREQUOTA , parVOUCHERSCODE, parSTYLE,  parOPERATOR, parMEMO, parCPSTATUS, parOUTWARDBED ,
                parXZZProvinceID , parXZZCityID,  parXZZDistrictID, parXZZTEL,  parXZZPost
            };

            try
            {
                m_app.SqlHelper.ExecuteNoneQuery("iem_main_page.usp_insertpatientinfo", paraColl, CommandType.StoredProcedure);
                m_app.CustomMessageBox.MessageShow("保存成功");
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 出生日期时间改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateBirthDay_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //edit by wyt 2012-11-02 更新年龄计算方法 
                if (patEntity != null)  //已经入院的患者按入院时间计算年龄
                {
                    DateTime AdmitDate = Convert.ToDateTime(patEntity.AdmitDate);
                    string age = DrectSoft.Common.Eop.PatientBasicInfo.CalcDisplayAge(dateBirthDay.DateTime, AdmitDate);
                    if (age.IndexOf("小时") != -1 || age.IndexOf("分") != -1)
                    {
                        this.lbc_PlaTime.Visible = true;
                        this.tmE_PlaTime.Visible = true;
                    }
                    this.txtAge.Text = age;
                }
                else //没有入院，即新入病人按当前时间
                {
                    string age = DrectSoft.Common.Eop.PatientBasicInfo.CalcDisplayAge(dateBirthDay.DateTime, DateTime.Now);
                    if (age.IndexOf("小时") != -1 || age.IndexOf("分") != -1)
                    {
                        this.lbc_PlaTime.Visible = true;
                        this.tmE_PlaTime.Visible = true;
                    }
                    this.txtAge.Text = age;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 计划入院时间改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmE_PlaTime_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //edit by ukey 2016-11-15 年龄计算方法
                if (patEntity != null)
                {
                    string age = DrectSoft.Common.Eop.PatientBasicInfo.CalcDisplayAge(dateBirthDay.DateTime, tmE_PlaTime.DateTime);
                    this.txtAge.Text = age;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 复选框选中后可右键取消选中
        /// add by ywk 2012年7月30日 08:43:05 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkInHosType_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit chkEdit = GetCheckEdit(((Control)sender).Name);
            if (chkEdit.Checked)
            {
                chkEdit.Checked = false;
            }
        }
        /// <summary>
        /// 根据名称返回控件
        /// </summary>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        private CheckEdit GetCheckEdit(string ControlName)
        {
            foreach (Control control in this.Controls)
            {
                if (control.Name == ControlName)
                {
                    return (CheckEdit)control;
                }
            }
            return null;
        }
        /// <summary>
        /// /更新病人的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-22</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_KeyPress(object sender, KeyPressEventArgs e)
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
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 关闭事件
        /// 
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 重置
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-24</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                if (null != patEntity)
                {
                    InitUI();
                }
                else
                {
                    ClearPage();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 清空事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-24</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPage();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 画面项目check
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-24</date>
        /// xll 2013-05-28 无需做所有项目的验证
        /// </summary>
        /// <returns></returns>
        private string checkItems()
        {
            try
            {
                if (string.IsNullOrEmpty(textEditNoOfRecord.Text.Trim()))
                {
                    textEditNoOfRecord.Focus();
                    return "病案号码不能为空";
                }
                else if (string.IsNullOrEmpty(textEditPatID.Text.Trim()))
                {
                    textEditPatID.Focus();
                    return "住院号码不能为空";
                }
                //else if (string.IsNullOrEmpty(luePayId.CodeValue.Trim()))
                //{
                //    luePayId.Focus();
                //    return "请选择医疗付款方式";
                //}
                else if (string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    txtName.Focus();
                    return "姓名不能为空";
                }
                else if (string.IsNullOrEmpty(lueSex.CodeValue.Trim()))
                {
                    lueSex.Focus();
                    return "请选择性别";
                }
                else if (string.IsNullOrEmpty(dateBirthDay.Text.Trim()))
                {
                    dateBirthDay.Focus();
                    return "请选择出生日期";
                }
                //else if (string.IsNullOrEmpty(txtAge.Text.Trim()))
                //{
                //    txtAge.Focus();
                //    return "年龄不能为空";
                //}
                //else if (string.IsNullOrEmpty(lueCSD_ProvinceID.CodeValue.Trim()))
                //{
                //    lueCSD_ProvinceID.Focus();
                //    return "请选择出生省份";
                //}
                //else if (string.IsNullOrEmpty(lueCSD_CityID.CodeValue.Trim()))
                //{
                //    lueCSD_CityID.Focus();
                //    return "请选择出生城市";
                //}
                //else if (string.IsNullOrEmpty(lueCSD_DistrictID.CodeValue.Trim()))
                //{
                //    lueCSD_DistrictID.Focus();
                //    return "请选择出生区县";
                //}
                //else if (string.IsNullOrEmpty(lueMarital.CodeValue.Trim()))
                //{
                //    lueMarital.Focus();
                //    return "请选择婚姻状况";
                //}
                //else if (string.IsNullOrEmpty(lueNationality.CodeValue.Trim()))
                //{
                //    lueNationality.Focus();
                //    return "请选择国籍";
                //}
                //else if (string.IsNullOrEmpty(lueNation.CodeValue.Trim()))
                //{
                //    lueNation.Focus();
                //    return "请选择民族";
                //}
                //else if (string.IsNullOrEmpty(txtIDCard.Text.Trim()))
                //{
                //    txtIDCard.Focus();
                //    return "身份证号不能为空";
                //}
                //else if (!Tool.IsDigitalOrLetter(txtIDCard.Text.Trim()))
                //{
                //    txtIDCard.Focus();
                //    return "身份证号只能包含数字或者英文字母";
                //}
                //else if (string.IsNullOrEmpty(lueJob.CodeValue.Trim()))
                //{
                //    lueJob.Focus();
                //    return "请选择职业";
                //}
                //else if (string.IsNullOrEmpty(txtOrganization.Text.Trim()))
                //{
                //    txtOrganization.Focus();
                //    return "工作单位不能为空";
                //}
                //else if (string.IsNullOrEmpty(txtOfficeTel.Text.Trim()))
                //{
                //    txtOfficeTel.Focus();
                //    return "单位电话不能为空";
                //}
                //else if (Tool.IfContainsChinese(txtOfficeTel.Text.Trim()))
                //{
                //    txtOfficeTel.Focus();
                //    return "单位电话不能包含中文字符";
                //}
                //else if (string.IsNullOrEmpty(txtOfficePostalCode.Text.Trim()))
                //{
                //    txtOfficePostalCode.Focus();
                //    return "单位邮编不能为空";
                //}
                //else if (!Tool.IsNumeric(txtOfficePostalCode.Text.Trim()))
                //{
                //    txtOfficePostalCode.Focus();
                //    return "单位邮编只能是数字";
                //}
                //else if (string.IsNullOrEmpty(txtHousehold.Text.Trim()))
                //{
                //    txtHousehold.Focus();
                //    return "户口地址不能为空";
                //}
                //else if (string.IsNullOrEmpty(txtTel.Text.Trim()))
                //{
                //    txtTel.Focus();
                //    return "住宅电话不能为空";
                //}
                //else if (Tool.IfContainsChinese(txtTel.Text.Trim()))
                //{
                //    txtTel.Focus();
                //    return "住宅电话不能包含中文字符";
                //}
                //else if (string.IsNullOrEmpty(txtPostalCode.Text.Trim()))
                //{
                //    txtPostalCode.Focus();
                //    return "住宅邮编不能为空";
                //}
                //else if (!Tool.IsNumeric(txtPostalCode.Text.Trim()))
                //{
                //    txtPostalCode.Focus();
                //    return "住宅邮编只能是数字";
                //}
                //else if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
                //{
                //    txtAddress.Focus();
                //    return "当前地址不能为空";
                //}
                //else if (string.IsNullOrEmpty(textEditContactPerson.Text.Trim()))
                //{
                //    textEditContactPerson.Focus();
                //    return "联系人名不能为空";
                //}
                //else if (string.IsNullOrEmpty(lueRelationship.CodeValue.Trim()))
                //{
                //    lueRelationship.Focus();
                //    return "请选择联系关系";
                //}
                //else if (string.IsNullOrEmpty(textEditContactAddress.Text.Trim()))
                //{
                //    textEditContactAddress.Focus();
                //    return "联系地址不能为空";
                //}
                //else if (string.IsNullOrEmpty(textEditContactTEL.Text.Trim()))
                //{
                //    textEditContactTEL.Focus();
                //    return "联系电话不能为空";
                //}
                //else if (Tool.IfContainsChinese(textEditContactTEL.Text.Trim()))
                //{
                //    textEditContactTEL.Focus();
                //    return "联系电话不能包含中文字符";
                //}
                //else if (string.IsNullOrEmpty(luePatientSource.CodeValue.Trim()))
                //{
                //    luePatientSource.Focus();
                //    return "请选择病人来源";
                //}
                else if (string.IsNullOrEmpty(txtBedID.Text.Trim()))
                {
                    txtBedID.Focus();
                    return "床位号不能为空";
                }
                //else if (string.IsNullOrEmpty(lookUpEditorUser1.CodeValue.Trim()))
                //{
                //    lookUpEditorUser1.Focus();
                //    return "请选择门诊医师";
                //}
                else if (string.IsNullOrEmpty(lookUpEditorUser2.CodeValue.Trim()))
                {
                    lookUpEditorUser2.Focus();
                    return "请选择住院医师";
                }
                //else if (string.IsNullOrEmpty(lookUpEditorUser3.CodeValue.Trim()))
                //{
                //    lookUpEditorUser3.Focus();
                //    return "请选择主治医师";
                //}
                //else if (string.IsNullOrEmpty(lookUpEditorUser4.CodeValue.Trim()))
                //{
                //    lookUpEditorUser4.Focus();
                //    return "请选择主任医师";
                //}

                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 清空
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-24</date>
        /// </summary>
        public void ClearPage()
        {
            try
            {
                luePayId.CodeValue = string.Empty;
                textEditNoOfRecord.Text = string.Empty;
                textEditPatID.Text = string.Empty;
                txtName.Text = string.Empty;
                lueSex.CodeValue = string.Empty;
                dateBirthDay.DateTime = DateTime.Now;
                txtAge.Text = "1";
                lueCSD_ProvinceID.CodeValue = string.Empty;
                lueCSD_CityID.CodeValue = string.Empty;
                lueCSD_DistrictID.CodeValue = string.Empty;
                lueMarital.CodeValue = string.Empty;
                lueNationality.CodeValue = string.Empty;
                lueNation.CodeValue = string.Empty;
                txtIDCard.Text = string.Empty;
                lueJob.Text = string.Empty;
                txtOrganization.Text = string.Empty;
                txtOfficeTel.Text = string.Empty;
                txtOfficePostalCode.Text = string.Empty;
                txtHousehold.Text = string.Empty;
                txtTel.Text = string.Empty;
                txtPostalCode.Text = string.Empty;
                txtAddress.Text = string.Empty;
                textEditContactPerson.Text = string.Empty;
                lueRelationship.CodeValue = string.Empty;
                textEditContactAddress.Text = string.Empty;
                textEditContactTEL.Text = string.Empty;
                luePatientSource.CodeValue = string.Empty;
                txtBedID.Text = string.Empty;
                ResetChkInHosType();
                ResetChkAdmitInfo();
                lookUpEditorUser1.CodeValue = string.Empty;
                lookUpEditorUser2.CodeValue = string.Empty;
                lookUpEditorUser3.CodeValue = string.Empty;
                lookUpEditorUser4.CodeValue = string.Empty;
                lueAdmitDiag.CodeValue = string.Empty;

                if (textEditNoOfRecord.Enabled)
                {
                    textEditNoOfRecord.Focus();
                }
                else
                {
                    //luePayId.Focus();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重置入院途径
        /// </summary>
        private void ResetChkInHosType()
        {
            chkInHosType1.Checked = false;
            chkInHosType2.Checked = false;
            chkInHosType3.Checked = false;
            chkInHosType4.Checked = false;
        }

        /// <summary>
        /// 重置入院病情
        /// </summary>
        private void ResetChkAdmitInfo()
        {
            chkAdmitInfo1.Checked = false;
            chkAdmitInfo2.Checked = false;
            chkAdmitInfo3.Checked = false;
            chkAdmitInfo4.Checked = false;
        }

        /// <summary>
        /// 设置病案号和住院号的编辑状态
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-24</date>
        /// </summary>
        /// <param name="boo"></param>
        public void SetEnable(bool boo)
        {
            this.textEditNoOfRecord.Enabled = boo;
            this.textEditPatID.Enabled = boo;
        }

        /// <summary>
        /// <title>出生日期范围验证，不能大于当前日期</title>
        /// <auth>wyt</auth>
        /// <date>2012-11-02</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateBirthDay_Validating(object sender, CancelEventArgs e)
        {
            if (dateBirthDay.DateTime > DateTime.Now)
            {
                this.errorProvider.SetError(dateBirthDay, "超出范围");
                e.Cancel = true;
            }
            else
            {
                this.errorProvider.Clear();
            }
        }
    }
}
