using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Convertmy = DrectSoft.Core.UtilsForExtension;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCIemBasInfoEn : UserControl
    {
        private IDataAccess m_SqlHelper;
        private IEmrHost m_App;
        private IemMainPageInfo m_IemInfo;
        private DataHelper m_DataHelper = new DataHelper();

        private Inpatient CurrentInpatient;//add by ywk 
        private UCMainForm m_UCMainForm;


        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                if (m_IemInfo == null)
                    m_IemInfo = new IemMainPageInfo();
                GetUI();
                return m_IemInfo;
            }
        }

        public UCIemBasInfoEn()
        {
            InitializeComponent();

            m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            InitData();
            InitLookUpEditor();

            //根据系统配置中的设置，进行控制患者基本信息的是否可编辑
            ControlBaseInfoEdit();
        }

        private string m_GOTYPE;//传来此页面的操作类型
        private string m_MZTYPE;//门诊类型，是中医还是西医
        private string m_MZZDNAME;//传来的诊断名称
        private string m_MZZDCODE;//传来诊断名称相对应的诊断编码

        public UCIemBasInfoEn(IEmrHost app, string operatertype, string mzzycode, string mztype)
        {
            m_App = app;
            m_GOTYPE = operatertype;
            m_MZTYPE = mztype;
            m_MZZDCODE = mzzycode;
            //lueMZZYZD_CODE.Text = "";
            lueMZXYZD_CODE.Text = "";


        }

        /// <summary>
        /// 取得数据(省市县)
        /// </summary>
        private void InitData()
        {
            _province = m_SqlHelper.ExecuteDataTable("select a.provinceid ID,a.provincename Name,a.py,a.wb from s_province a");
            _city = m_SqlHelper.ExecuteDataTable("select a.cityid ID,a.cityname Name,a.provinceid,a.py,a.wb  from s_city a ");
            _qxian = m_SqlHelper.ExecuteDataTable("select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a");

            _dept = m_SqlHelper.ExecuteDataTable("select a.ID,a.NAME,a.py,a.wb from department a  where a.valid='1'");

            _ward = m_SqlHelper.ExecuteDataTable(@"select a.ID,a.NAME,a.py,a.wb,b.deptid from ward a ,dept2ward b where     a.id=b.wardid  and a.valid='1' ");
        }
        #region 各个下拉框的值，预先取出数据
        /// <summary>
        /// 科室
        /// </summary>
        private DataTable Ward
        {
            get { return _ward; }
        }
        private DataTable _ward;
        /// <summary>
        /// 科室
        /// </summary>
        private DataTable Dept
        {
            get { return _dept; }
        }
        private DataTable _dept;

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


        private DataTable Depts
        {
            get { return _depts; }
        }
        private DataTable _depts;

        private DataTable Wards
        {
            get { return _wards; }
        }
        private DataTable _wards;

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
        #endregion
        public UCIemBasInfoEn(UCMainForm ucMainForm)
            : this()
        {
            m_UCMainForm = ucMainForm;
        }

        /// <summary>
        /// 根据系统配置中的设置，进行控制患者基本信息的是否可编辑
        /// add by ywk 二〇一二年五月四日 11:35:06
        /// </summary>
        private void ControlBaseInfoEdit()
        {
            string canedit = m_DataHelper.GetConfigValueByKey("IsOpenSetPaientBaseInfo");
            if (canedit == "1")//可以编辑
            {
                luePayId.ReadOnly = false;//医疗付款方式
                txtName.Properties.ReadOnly = false;//姓名
                lueSex.ReadOnly = false;//性别
                deBirth.Properties.ReadOnly = false;//出生日期
                txtAge.Properties.ReadOnly = false;//年龄
                txtIDNO.Properties.ReadOnly = false;//身份证号码
                lueMarital.ReadOnly = false;//婚姻
                lueJob.ReadOnly = false;//职业
                lueCSD_ProvinceID.ReadOnly = false;//出生地省
                lueCSD_CityID.ReadOnly = false;//出生地市
                lueCSD_DistrictID.ReadOnly = false;//出生地先
                lueNation.ReadOnly = false;//民族
                lueNationality.ReadOnly = false;//国籍
                lueJG_ProvinceID.ReadOnly = false;//籍贯省
                lueJG_CityID.ReadOnly = false;//籍贯市
                txtOfficePlace.Properties.ReadOnly = false;//工作单位地址
                textEditotherhospital.Properties.ReadOnly = false;//其他医疗机构转入
                txtOfficeTEL.Properties.ReadOnly = false;//单位电话
                txtOfficePost.Properties.ReadOnly = false;//工作单位邮编
                txtHKDZ_Post.Properties.ReadOnly = false;//户口住址邮编
                txtContactPerson.Properties.ReadOnly = false;//联系人姓名
                lueRelationship.ReadOnly = false;//联系人关系
                txtContactAddress.Properties.ReadOnly = false;//联系人地址
                txtContactTEL.Properties.ReadOnly = false;//联系人电话
                lueAdmitDept.ReadOnly = false;//入院科别
                lueAdmitWard.ReadOnly = false;//入院病区
                deAdmitDate.Properties.ReadOnly = false;//入院日期
                teAdmitDate.Properties.ReadOnly = false;//入院日期
                deOutWardDate.Properties.ReadOnly = false;//出院日期
                teOutWardDate.Properties.ReadOnly = false;//出院日期
                lueOutHosDept.ReadOnly = false;//出院科别
                lueOutHosWard.ReadOnly = false;//出院病区
                seActualDays.Properties.ReadOnly = false;//住院天数
                txtCardNumber.Properties.ReadOnly = false;//健康卡号
            }
            else
            {
                luePayId.ReadOnly = true;//医疗付款方式
                txtName.Properties.ReadOnly = true;//姓名
                lueSex.ReadOnly = true;//性别
                deBirth.Properties.ReadOnly = true;//出生日期
                txtAge.Properties.ReadOnly = true;//年龄
                txtMonthAge.Properties.ReadOnly = true;//年龄
                txtIDNO.Properties.ReadOnly = true;//身份证号码
                lueMarital.ReadOnly = true;//婚姻
                lueJob.ReadOnly = true;//职业
                lueCSD_ProvinceID.ReadOnly = true;//出生地省
                lueCSD_CityID.ReadOnly = true;//出生地市
                lueCSD_DistrictID.ReadOnly = true;//出生地先
                lueNation.ReadOnly = true;//民族
                lueNationality.ReadOnly = true;//国籍
                lueJG_ProvinceID.ReadOnly = true;//籍贯省
                lueJG_CityID.ReadOnly = true;//籍贯市
                txtOfficePlace.Properties.ReadOnly = true;//工作单位地址
                textEditotherhospital.Properties.ReadOnly = true;//其他医疗机构转入
                txtOfficeTEL.Properties.ReadOnly = true;//单位电话
                txtOfficePost.Properties.ReadOnly = true;//工作单位邮编
                txtHKDZ_Post.Properties.ReadOnly = true;//户口住址邮编
                txtContactPerson.Properties.ReadOnly = true;//联系人姓名
                lueRelationship.ReadOnly = true;//联系人关系
                txtContactAddress.Properties.ReadOnly = true;//联系人地址
                txtContactTEL.Properties.ReadOnly = true;//联系人电话
                lueAdmitDept.ReadOnly = true;//入院科别
                lueAdmitWard.ReadOnly = true;//入院病区
                deAdmitDate.Properties.ReadOnly = true;//入院日期
                teAdmitDate.Properties.ReadOnly = true;//入院日期
                deOutWardDate.Properties.ReadOnly = true;//出院日期
                teOutWardDate.Properties.ReadOnly = true;//出院日期
                lueOutHosDept.ReadOnly = true;//出院科别
                lueOutHosWard.ReadOnly = true;//出院病区
                seActualDays.Properties.ReadOnly = true;//住院天数

                txtCardNumber.Properties.ReadOnly = true;//健康卡号edit by ywk 2012年5月14日 16:32:03
            }
        }



        private void UCIemBasInfoEn_Load(object sender, EventArgs e)
        {
            GetFormLoadData();

            //更改出生日期后，年龄算出来(ywk 泗县修改)
            deBirth.EditValueChanged += new EventHandler(deBirth_EditValueChanged);

            //更改出院日期，算出住院天数 ywk 2012年6月19日 14:17:07
            deOutWardDate.EditValueChanged += new EventHandler(deOutWardDate_EditValueChanged);
            deAdmitDate.EditValueChanged += new EventHandler(deOutWardDate_EditValueChanged);
            teAdmitDate.EditValueChanged += new EventHandler(deOutWardDate_EditValueChanged);
            teOutWardDate.EditValueChanged += new EventHandler(deOutWardDate_EditValueChanged);
        }

        private string MZDiagType = string.Empty;//诊断类型
        private string GoType = string.Empty;//表明大类别的Type
        private string inputText = string.Empty;//获取文本里面的内容

        //private string m_GOTYPE;//传来此页面的操作类型
        //private string m_MZTYPE;//门诊类型，是中医还是西医
        //private string m_MZZDNAME;//传来的诊断名称
        //private string m_MZZDCODE;//传来诊断名称相对应的诊断编码


        private DataTable dtZY = new DataTable();
        private DataTable dtXY = new DataTable();
        /// <summary>
        /// 得到整个界面的绑定的出诊的中西医库的值
        /// </summary>
        /// <returns></returns>
        public void GetFormLoadData()
        {
            try
            {
                string SqlAllDiagChinese = @"select  py, wb, name, id icd from diagnosisofchinese where valid='1' union select py, wb, name, icdid icd from diagnosischiothername where valid='1'";
                dtZY = m_SqlHelper.ExecuteDataTable(SqlAllDiagChinese, CommandType.Text);

                string SqlAllDiag = @"select py, wb, name, icd from diagnosis  where valid='1' union select py, wb, name, icdid icd from diagnosisothername where valid='1';
";
                dtXY = m_SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void bwj2_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Button btn = new Button();
                if (e.KeyChar == 13)//lueMZXYZD_CODE.Text.Trim() != null &&
                {
                    GoType = "MZDIAG";
                    MZDiagType = "XIYI";
                    inputText = lueMZXYZD_CODE.Text.Trim();

                    IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtXY, GoType, MZDiagType, inputText);
                    if (diagInfo.GetFormResult())
                    {
                        diagInfo.ShowDialog();
                        if (diagInfo.IsClosed)
                        {
                            lueMZXYZD_CODE.Text = diagInfo.inText;
                            lueMZXYZD_CODE.DiaCode = diagInfo.inCode;
                            lueMZXYZD_CODE.DiaValue = diagInfo.inText;
                        }
                    }
                    else
                    {
                        lueMZXYZD_CODE.DiaCode = diagInfo.inCode;
                        lueMZXYZD_CODE.DiaValue = diagInfo.inText;
                        lueMZXYZD_CODE.Multiline = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }


        private void lueRYZD_CODE_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Button btn = new Button();
                if (e.KeyChar == 13)
                {
                    GoType = "RYZDIAG";
                    MZDiagType = "XIYI";
                    inputText = lueRYZD_CODE.Text.Trim();

                    IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtXY, GoType, MZDiagType, inputText);
                    if (diagInfo.GetFormResult())
                    {
                        diagInfo.ShowDialog();
                        if (diagInfo.IsClosed)
                        {
                            lueRYZD_CODE.Text = diagInfo.inText;
                            lueRYZD_CODE.DiaCode = diagInfo.inCode;
                            lueRYZD_CODE.DiaValue = diagInfo.inText;
                        }
                    }
                    else
                    {
                        lueRYZD_CODE.DiaCode = diagInfo.inCode;
                        lueRYZD_CODE.DiaValue = diagInfo.inText;
                        lueRYZD_CODE.Multiline = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        public void mzdiagInfo_FormClosed(object sender, EventArgs e)
        {
            try
            {
                IemNewDiagInfo mzdiagInfo = (IemNewDiagInfo)sender;
                if (m_MZTYPE == "XIYI")
                {
                    if (mzdiagInfo.IsClosed)
                    {
                        lueMZXYZD_CODE.Text = mzdiagInfo.inText;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 更改出生日期后，动态计算年龄 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deBirth_EditValueChanged(object sender, EventArgs e)
        {
            this.txtAge.Text = PatientBasicInfo.CalcDisplayAge(deBirth.DateTime, DateTime.Now);

        }

        /// <summary>
        /// 更改出院日期算出住院天数
        /// 2012年6月19日 14:17:26
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deOutWardDate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dtAdmitDate = DateTime.Parse(deAdmitDate.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Text);//入院日期
                DateTime dtOutDate = DateTime.Parse(deOutWardDate.DateTime.ToString("yyyy-MM-dd") + " " + teOutWardDate.Text);//出院日期
                this.seActualDays.EditValue = CalcInHosDays(dtAdmitDate, dtOutDate);
                //int datediff = (dtOutDate - dtAdmitDate).Days;
                //if (datediff < 0)
                //{
                //    this.seActualDays.EditValue = "";
                //}
                //else
                //{
                //    this.seActualDays.EditValue = datediff;
                //}
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region private methods

        /// <summary>
        /// 初始化lookupeditor
        /// </summary>
        private void InitLookUpEditor()
        {
            InitLuePayId();
            InitLueSex();
            InitMarital();
            InitInHosInfo();
            InitInHosCall();
            InitJob();
            InitNation();
            InitNationality();
            InitRelationship();
            InitDept();
            BindAllProvince();
            BindDiagnosisOfChinese();
            BindDiagnosis();
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
        /// 初始化入院情况
        /// </summary>
        private void InitInHosInfo()
        {
            if (lueINHOSINFO.SqlWordbook == null)
                BindLueData(lueINHOSINFO, 34);
        }
        /// <summary>
        /// 初始化住院期间是否告病危或病重
        /// </summary>
        private void InitInHosCall()
        {
            if (lueINHOSCALL.SqlWordbook == null)
                BindLueData(lueINHOSCALL, 35);
        }


        /// <summary>
        /// 职业代码
        /// </summary>
        private void InitJob()
        {
            if (lueJob.SqlWordbook == null)
                BindLueData(lueJob, 4);
        }

        ///// <summary>
        ///// 市代码
        ///// </summary>
        //private void InitProvAndCity()
        //{
        //    BindLueData(lueProvice, 4);
        //}

        private DataTable m_DataTableCountry;
        /// <summary>
        /// 区县代码
        /// </summary>
        /// <param name="lueInfo"></param>
        /// <param name="queryType"></param>
        private void BindLueCountryData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            if (m_DataTableCountry == null)
                m_DataTableCountry = GetEditroData(queryType);

            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueInfo.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", m_DataTableCountry, "ID", "Name", columnwidth, true);

            lueInfo.SqlWordbook = sqlWordBook;
            lueInfo.ListWindow = lupInfo;
        }
        /// <summary>
        /// 区县代码
        /// </summary>
        /// <param name="lueInfo"></param>
        /// <param name="queryType"></param>
        private void BindLueCountryData(LookUpEditor lueInfo, DataTable dataTable)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;

            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueInfo.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            lueInfo.SqlWordbook = sqlWordBook;
            lueInfo.ListWindow = lupInfo;
        }

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


        private DataTable m_DataTableWard = null;
        /// <summary>
        /// 科室和病区
        /// </summary>
        private void InitDept()
        {
            if (lueAdmitDept.SqlWordbook == null)
                BindDeptData();
            if (lueOutHosWard.SqlWordbook == null)
                BindWardData();
        }

        /// <summary>
        /// 绑定西医门急诊诊断
        /// </summary>
        private void BindDiagnosis()
        {
            //if (lueMZXYZD_CODE == null)//----------****************
            BindLueData(lueMZXYZD_CODE1, 12);
        }

        /// <summary>
        /// 所有科室
        /// </summary>
        private void BindDeptData()
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            DataTable dataTable = GetEditroData(9);
            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueAdmitDept.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            lueAdmitDept.SqlWordbook = sqlWordBook;
            lueAdmitDept.ListWindow = lupInfo;

            lueTransAdmitDept.SqlWordbook = sqlWordBook;
            lueTransAdmitDept.ListWindow = lupInfo;

            //lueAdmitDeptAgain.SqlWordbook = sqlWordBook;
            //lueAdmitDeptAgain.ListWindow = lupInfo;

            lueOutHosDept.SqlWordbook = sqlWordBook;
            lueOutHosDept.ListWindow = lupInfo;

        }

        private void BindWardData()
        {
            LookUpWindow lupInfo1 = new LookUpWindow();
            lupInfo1.SqlHelper = m_SqlHelper;
            if (m_DataTableWard == null)
                m_DataTableWard = GetEditroData(10);
            Dictionary<string, int> columnwidth1 = new Dictionary<String, Int32>();
            columnwidth1.Add("名称", lueAdmitWard.Width);
            SqlWordbook sqlWordBook1 = new SqlWordbook("ID", m_DataTableWard, "ID", "Name", columnwidth1, true);

            lueAdmitWard.SqlWordbook = sqlWordBook1;
            lueAdmitWard.ListWindow = lupInfo1;

            //lueTransAdmitWard.SqlWordbook = sqlWordBook1;
            //lueTransAdmitWard.ListWindow = lupInfo1;

            lueOutHosWard.SqlWordbook = sqlWordBook1;
            lueOutHosWard.ListWindow = lupInfo1;
        }

        /// <summary>
        /// 绑定中医诊断
        /// </summary>
        private void BindDiagnosisOfChinese()
        {
            LookUpWindow lupInfo1 = new LookUpWindow();
            lupInfo1.SqlHelper = m_SqlHelper;

            string sql = string.Format(@"select  py, wb, name, id  from diagnosisofchinese where valid='1' union select py, wb, name, icdid id from diagnosischiothername where valid='1'");
            DataTable Dept = m_SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["NAME"].Caption = "名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
            lueMZXYZD_CODE1.SqlWordbook = deptWordBook;

            lueMZXYZD_CODE1.ListWindow = lupInfo1;

        }

        /// <summary>
        /// 绑定页面所有省区信息
        /// </summary>
        private void BindAllProvince()
        {
            //string sql = @"select a.provinceid ID,a.provincename Name,py,wb from s_province a";
            //DataTable province = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            //BindProvince(lueCSD_ProvinceID, province);
            //BindProvince(lueJG_ProvinceID, province);
            //BindProvince(lueHKDZ_ProvinceID, province);
            //BindProvince(lueXZZ_ProvinceID, province);

            //BindCity(lueCSD_CityID, "");
            //BindCity(lueJG_CityID, "");
            //BindCity(lueXZZ_CityID, "");
            //BindCity(lueHKDZ_CityID, "");

            //BindDistrict(lueCSD_DistrictID, "");
            //BindDistrict(lueXZZ_DistrictID, "");
            //BindDistrict(lueHKDZ_DistrictID, "");
            if (lueCSD_DistrictID.SqlWordbook == null)
                BindDistrict(lueCSD_DistrictID, QXian);
            if (lueXZZ_DistrictID.SqlWordbook == null)
                BindDistrict(lueXZZ_DistrictID, QXian);
            if (lueHKDZ_DistrictID.SqlWordbook == null)
                BindDistrict(lueHKDZ_DistrictID, QXian);

            if (lueCSD_CityID.SqlWordbook == null)
                BindCity(lueCSD_CityID, City);
            if (lueJG_CityID.SqlWordbook == null)
                BindCity(lueJG_CityID, City);
            if (lueXZZ_CityID.SqlWordbook == null)
                BindCity(lueXZZ_CityID, City);
            if (lueHKDZ_CityID.SqlWordbook == null)
                BindCity(lueHKDZ_CityID, City);

            if (lueCSD_ProvinceID.SqlWordbook == null)
            {
                BindProvince(lueCSD_ProvinceID, Province);
                BindProvince(lueJG_ProvinceID, Province);
                BindProvince(lueHKDZ_ProvinceID, Province);
                BindProvince(lueXZZ_ProvinceID, Province);
            }


            if (lueAdmitDept.SqlWordbook == null)
                BindDept(lueAdmitDept, Dept);
            if (lueOutHosDept.SqlWordbook == null)
                BindDept(lueOutHosDept, Dept);

            if (lueAdmitWard.SqlWordbook == null)
                BindWard(lueAdmitWard, Ward);
            if (lueOutHosWard.SqlWordbook == null)
                BindWard(lueOutHosWard, Ward);
        }
        /// <summary>
        /// 绑定科室
        /// </summary>
        /// <param name="lueinfo"></param>
        private void BindDept(LookUpEditor lueInfo, DataTable dataTable)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;

            Dictionary<string, int> columnwidth1 = new Dictionary<String, Int32>();
            columnwidth1.Add("名称", lueInfo.Width);
            SqlWordbook sqlWordBook1 = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth1, true);

            lueInfo.SqlWordbook = sqlWordBook1;
            lueInfo.ListWindow = lupInfo;
        }
        /// <summary>
        /// 绑定病区
        /// </summary>
        /// <param name="lueinfo"></param>
        private void BindWard(LookUpEditor lueInfo, DataTable dataTable)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;

            Dictionary<string, int> columnwidth1 = new Dictionary<String, Int32>();
            columnwidth1.Add("名称", lueInfo.Width);
            SqlWordbook sqlWordBook1 = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth1, true);

            lueInfo.SqlWordbook = sqlWordBook1;
            lueInfo.ListWindow = lupInfo;
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
            lupInfo.SqlHelper = m_SqlHelper;
            dataTable.Columns["NAME"].Caption = "省（区、市）";
            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("NAME", lueInfo.Width);

            //new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            lueInfo.SqlWordbook = sqlWordBook;
            lueInfo.ListWindow = lupInfo;

        }

        /// <summary>
        /// 绑定市级地区
        /// </summary>
        /// <param name="lueinfo"></param>
        private void BindCity(LookUpEditor lueinfo, string provinceid)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            string sql = string.Format(@"select a.cityid ID,a.cityname Name,a.provinceid,py,wb  from s_city a where a.provinceid = '{0}'", provinceid);
            DataTable province = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            province.Columns["NAME"].Caption = "市";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", province, "ID", "NAME", cols, true);
            lueinfo.SqlWordbook = deptWordBook;
            lueinfo.ListWindow = lupInfo;

            //原来的选择省份后，要选择下拉列表，联动的市才改变
            //现在改为选第一个市 add by ywk 2012年5月4日 11:53:37
            if (province.Rows.Count > 0)
            {
                lueinfo.CodeValue = province.Rows[0]["ID"].ToString();
            }

        }
        /// <summary>
        /// 绑定市级地区
        /// </summary>
        /// <param name="lueinfo"></param>
        private void BindCity(LookUpEditor lueinfo, DataTable city)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            //string sql = string.Format(@"select a.cityid ID,a.cityname Name,a.provinceid,a.py,a.wb  from s_city a where a.provinceid = '{0}'", provinceid);
            //DataTable province = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            city.Columns["NAME"].Caption = "市";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", city, "ID", "NAME", cols, true);
            lueinfo.SqlWordbook = deptWordBook;
            lueinfo.ListWindow = lupInfo;

            //原来的选择省份后，要选择下拉列表，联动的市才改变
            //现在改为选第一个市 add by ywk 2012年5月4日 11:53:37
            if (city.Rows.Count > 0)
            {
                lueinfo.CodeValue = city.Rows[0]["ID"].ToString();
            }
        }
        /// <summary>
        /// 绑定县级地区
        /// </summary>
        /// <param name="lueinfo"></param>
        private void BindDistrict(LookUpEditor lueinfo, string cityid)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            string sql = string.Format(@"select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a where a.cityid = '{0}'", cityid);
            DataTable province = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            province.Columns["NAME"].Caption = "县";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", province, "ID", "NAME", cols, true);
            lueinfo.SqlWordbook = deptWordBook;
            lueinfo.ListWindow = lupInfo;

            //原来的选择市后，要选择下拉列表，联动的县才改变
            //现在改为选第一个县 add by ywk 2012年5月4日 11:55:39
            if (province.Rows.Count > 0)
            {
                lueinfo.CodeValue = province.Rows[0]["ID"].ToString();
            }
        }
        /// <summary>
        /// 绑定县级地区
        /// </summary>
        /// <param name="lueinfo"></param>
        private void BindDistrict(LookUpEditor lueinfo, DataTable qxian)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            //string sql = string.Format(@"select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a where a.cityid = '{0}'", cityid);
            //DataTable province = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            qxian.Columns["NAME"].Caption = "县";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", qxian, "ID", "NAME", cols);
            lueinfo.SqlWordbook = deptWordBook;
            lueinfo.ListWindow = lupInfo;

            //原来的选择市后，要选择下拉列表，联动的县才改变
            //现在改为选第一个县 add by ywk 2012年5月4日 11:55:39
            if (qxian.Rows.Count > 0)
            {
                lueinfo.CodeValue = qxian.Rows[0]["ID"].ToString();
            }
        }
        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
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
            DataTable dataTable = AddTableColumn(m_SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, CommandType.StoredProcedure));
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

        public void FillUI(IemMainPageInfo info, IEmrHost app)
        {
            m_App = app;
            m_IemInfo = info;

            InitForm();

            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner();
        }

        /// <summary>
        /// 计算住院天数
        /// </summary>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        int CalcInHosDays(DateTime dateBegin, DateTime dateEnd)
        {
            try
            {
                int daysPart = (dateEnd.Date - dateBegin.Date).Days;
                if (dateEnd < dateBegin)
                {
                    return 0;
                }
                else if (daysPart < 0)
                {
                    return 0;
                }
                else if (daysPart == 0)
                {
                    return daysPart + 1;
                }
                else
                {

                    return daysPart;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        delegate void FillUIDelegate();
        private void FillUIInner()
        {
            #region
            IemMainPageInfo info = m_IemInfo;
            IEmrHost app = m_App;

            //表头
            luePayId.CodeValue = info.IemBasicInfo.PayID;
            txtCardNumber.Text = info.IemBasicInfo.CardNumber;
            txtSocialCare.Text = info.IemBasicInfo.SocialCare;

            if (info.IemBasicInfo.IsBaby == "1")//如果是婴儿，取母亲的病案号
            {
                IemMainPageManger iem = new IemMainPageManger(m_App, m_App.CurrentPatientInfo);
                string babypat = iem.GetPatData(info.IemBasicInfo.Mother).Rows[0]["noofclinic"].ToString();
                txtPatNoOfHis.Text = babypat;
            }
            else
            {
                //  txtPatNoOfHis.Text = info.IemBasicInfo.PatNoOfHis.ToString();
                #region 将his首页序号 修改为显示his中病人的id号
                txtPatNoOfHis.Text = info.IemBasicInfo.NOOFRECORD.ToString();
                #endregion
            }

            //txtPatNoOfHis.Text = info.IemBasicInfo.PatNoOfHis.ToString();
            //入院次数如果没有，应该算第一次住院  add by ywk 2013年3月25日11:50:15 
            seInCount.Value = info.IemBasicInfo.InCount == "" ? 1 : Convert.ToInt32(info.IemBasicInfo.InCount);

            //第一行
            txtName.Text = info.IemBasicInfo.Name;
            lueSex.CodeValue = info.IemBasicInfo.SexID;
            if (!String.IsNullOrEmpty(info.IemBasicInfo.Birth))
            {
                deBirth.DateTime = Convert.ToDateTime(info.IemBasicInfo.Birth);
            }
            //txtAge.Text = app.CurrentPatientInfo.PersonalInformation.CurrentDisplayAge;
            //此处的Age显示的也要跟前面显示的一致 （泗县 ywk 修改 2012年5月9日10:21:39）

            //如果病案首页里没有此病人的数据，则年龄字段是计算得到的，否则的话就是输入的编辑的值
            string sqlserach = string.Format(@" select name from  iem_mainpage_basicinfo_sx
                where noofinpat='{0}'", info.IemBasicInfo.NoOfInpat);
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(sqlserach, CommandType.Text);
            if (dt.Rows.Count > 0)//病案首页里存在该病人
            {
                txtAge.Text = info.IemBasicInfo.Age;
            }
            else
            {
                txtAge.Text = app.CurrentPatientInfo.PersonalInformation.CurrentDisplayAge;
            }
            lueNationality.CodeValue = info.IemBasicInfo.NationalityID;

            //第二行
            txtMonthAge.Text = info.IemBasicInfo.MonthAge;
            txtWeight.Text = info.IemBasicInfo.Weight;
            txtInWeight.Text = info.IemBasicInfo.InWeight;

            //第三行
            lueCSD_ProvinceID.CodeValue = info.IemBasicInfo.CSD_ProvinceID;
            lueCSD_CityID.CodeValue = info.IemBasicInfo.CSD_CityID;
            lueCSD_DistrictID.CodeValue = info.IemBasicInfo.CSD_DistrictID;
            lueJG_ProvinceID.CodeValue = info.IemBasicInfo.JG_ProvinceID;
            lueJG_CityID.CodeValue = info.IemBasicInfo.JG_CityID;
            #region 0622添加
            textEdit_hkdz.Text = info.IemBasicInfo.HKDZ_ProvinceName;
            textEdit_xzz.Text = info.IemBasicInfo.XZZ_ProvinceName;
            textEdit_jg.Text = info.IemBasicInfo.JG_ProvinceName;
            textEdit_csd.Text = info.IemBasicInfo.CSD_ProvinceName;
            //   txt_job.Text = info.IemBasicInfo.JobID;
            #endregion
            //第四行
            txtIDNO.Text = info.IemBasicInfo.IDNO;
            lueJob.CodeValue = info.IemBasicInfo.JobID;
            lueMarital.CodeValue = info.IemBasicInfo.Marital;
            lueNation.CodeValue = info.IemBasicInfo.NationID;

            //第五行
            lueXZZ_ProvinceID.CodeValue = info.IemBasicInfo.XZZ_ProvinceID;
            lueXZZ_CityID.CodeValue = info.IemBasicInfo.XZZ_CityID;
            lueXZZ_DistrictID.CodeValue = info.IemBasicInfo.XZZ_DistrictID;
            txtXZZ_TEL.Text = info.IemBasicInfo.XZZ_TEL;
            txtXZZ_Post.Text = info.IemBasicInfo.XZZ_Post;

            //第六行
            lueHKDZ_ProvinceID.CodeValue = info.IemBasicInfo.HKDZ_ProvinceID;
            lueHKDZ_CityID.CodeValue = info.IemBasicInfo.HKDZ_CityID;
            lueHKDZ_DistrictID.CodeValue = info.IemBasicInfo.HKDZ_DistrictID;
            txtHKDZ_Post.Text = info.IemBasicInfo.HKDZ_Post;

            //第七行
            txtOfficePlace.Text = info.IemBasicInfo.OfficePlace;
            textEditotherhospital.Text = info.IemBasicInfo.TypeHos;
            txtOfficeTEL.Text = info.IemBasicInfo.OfficeTEL;
            txtOfficePost.Text = info.IemBasicInfo.OfficePost;

            //第八行
            txtContactPerson.Text = info.IemBasicInfo.ContactPerson;
            lueRelationship.CodeValue = info.IemBasicInfo.RelationshipID;
            txtContactAddress.Text = info.IemBasicInfo.ContactAddress;
            txtContactTEL.Text = info.IemBasicInfo.ContactTEL;

            //第九行
            if (info.IemBasicInfo.InHosType == "1")
                chkInHosType1.Checked = true;
            else if (info.IemBasicInfo.InHosType == "2")
                chkInHosType2.Checked = true;
            else if (info.IemBasicInfo.InHosType == "3")
                chkInHosType3.Checked = true;
            else
                chkInHosType4.Checked = true;
            textEditotherhospital.Text = info.IemBasicInfo.TypeHos;
            lueINHOSINFO.CodeValue = info.IemBasicInfo.InHosInfo;
            lueINHOSCALL.CodeValue = info.IemBasicInfo.InHosCall;

            //入院信息

            //入院日期
            if (!String.IsNullOrEmpty(info.IemBasicInfo.AdmitDate))
            {
                deAdmitDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
                teAdmitDate.Time = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
            }
            //主要诊断确诊日期
            if (!String.IsNullOrEmpty(info.IemBasicInfo.MainDiagDate))
            {
                deMAINDIAGDATE.DateTime = Convert.ToDateTime(info.IemBasicInfo.MainDiagDate);
                teMAINDIAGDATE.Time = Convert.ToDateTime(info.IemBasicInfo.MainDiagDate);
            }

            lueAdmitDept.CodeValue = info.IemBasicInfo.AdmitDeptID;
            // lueAdmitWard.CodeValue = info.IemBasicInfo.AdmitWardID;
            textEdit_rybg.Text = info.IemBasicInfo.AdmitWardID;

            lueTransAdmitDept.CodeValue = info.IemBasicInfo.Trans_AdmitDeptID;

            if (!String.IsNullOrEmpty(info.IemBasicInfo.OutWardDate))
            {
                deOutWardDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.OutWardDate);
                teOutWardDate.Time = Convert.ToDateTime(info.IemBasicInfo.OutWardDate);


                //如果出院日期不为空。自动算出住院天数
                int datediff = (deOutWardDate.DateTime - deAdmitDate.DateTime).Days;
                if (datediff < 0)
                {
                    this.seActualDays.EditValue = "";
                }
                else
                {
                    //this.seActualDays.EditValue = datediff;
                    this.seActualDays.EditValue = CalcInHosDays(deAdmitDate.DateTime, deOutWardDate.DateTime);
                }

            }
            else
            {
                seActualDays.Value = Convertmy.ToDecimal(info.IemBasicInfo.ActualDays);
            }
            lueOutHosDept.CodeValue = info.IemBasicInfo.OutHosDeptID;
            lueOutHosWard.CodeValue = info.IemBasicInfo.OutHosWardID;
            textEdit_cybf.Text = info.IemBasicInfo.OutHosWardID; ;


            lueMZXYZD_CODE.DiaCode = m_IemInfo.IemBasicInfo.MZXYZD_CODE;//.CodeValue 
            lueMZXYZD_CODE.DiaValue = m_IemInfo.IemBasicInfo.MZXYZD_NAME;
            lueMZXYZD_CODE.Text = m_IemInfo.IemBasicInfo.MZXYZD_NAME;

            lueRYZD_CODE.DiaCode = m_IemInfo.IemBasicInfo.RYXYZD_CODE;
            lueRYZD_CODE.DiaValue = m_IemInfo.IemBasicInfo.RYXYZD_NAME;
            lueRYZD_CODE.Text = m_IemInfo.IemBasicInfo.RYXYZD_NAME;

            #endregion
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            #region

            //表头
            m_IemInfo.IemBasicInfo.PayID = luePayId.CodeValue;
            m_IemInfo.IemBasicInfo.PayName = luePayId.Text;
            m_IemInfo.IemBasicInfo.CardNumber = txtCardNumber.Text;
            m_IemInfo.IemBasicInfo.SocialCare = txtSocialCare.Text;
            m_IemInfo.IemBasicInfo.PatNoOfHis = txtPatNoOfHis.Text;
            m_IemInfo.IemBasicInfo.InCount = seInCount.Value.ToString();

            //第一行
            m_IemInfo.IemBasicInfo.Name = txtName.Text;
            m_IemInfo.IemBasicInfo.SexID = lueSex.CodeValue;
            if (!(deBirth.DateTime.CompareTo(DateTime.MinValue) == 0))
            {
                m_IemInfo.IemBasicInfo.Birth = deBirth.DateTime.ToString("yyyy-MM-dd");
            }
            m_IemInfo.IemBasicInfo.Age = txtAge.Text;
            m_IemInfo.IemBasicInfo.NationalityID = lueNationality.CodeValue;
            m_IemInfo.IemBasicInfo.NationalityName = lueNationality.Text;

            //第二行
            m_IemInfo.IemBasicInfo.MonthAge = txtMonthAge.Text;
            m_IemInfo.IemBasicInfo.Weight = txtWeight.Text;
            m_IemInfo.IemBasicInfo.InWeight = txtInWeight.Text;

            //第三行
            m_IemInfo.IemBasicInfo.CSD_ProvinceID = lueCSD_ProvinceID.CodeValue;
            m_IemInfo.IemBasicInfo.CSD_ProvinceName = textEdit_csd.Text.Trim();
            m_IemInfo.IemBasicInfo.CSD_CityID = lueCSD_CityID.CodeValue;
            m_IemInfo.IemBasicInfo.CSD_CityName = lueCSD_CityID.Text;
            m_IemInfo.IemBasicInfo.CSD_DistrictID = lueCSD_DistrictID.CodeValue;
            m_IemInfo.IemBasicInfo.CSD_DistrictName = lueCSD_DistrictID.Text;
            m_IemInfo.IemBasicInfo.JG_ProvinceID = lueJG_ProvinceID.CodeValue;
            m_IemInfo.IemBasicInfo.JG_ProvinceName = textEdit_jg.Text.Trim();
            m_IemInfo.IemBasicInfo.JG_CityID = lueJG_CityID.CodeValue;
            m_IemInfo.IemBasicInfo.JG_CityName = lueJG_CityID.Text;

            //第四行
            m_IemInfo.IemBasicInfo.IDNO = txtIDNO.Text;
            m_IemInfo.IemBasicInfo.JobID = lueJob.CodeValue;
            m_IemInfo.IemBasicInfo.JobName = lueJob.Text;
            m_IemInfo.IemBasicInfo.Marital = lueMarital.CodeValue;
            m_IemInfo.IemBasicInfo.NationID = lueNation.CodeValue;
            m_IemInfo.IemBasicInfo.NationName = lueNation.Text;

            //第五行
            m_IemInfo.IemBasicInfo.XZZ_ProvinceID = lueXZZ_ProvinceID.CodeValue;
            m_IemInfo.IemBasicInfo.XZZ_ProvinceName = textEdit_xzz.Text.Trim();
            m_IemInfo.IemBasicInfo.XZZ_CityID = lueXZZ_CityID.CodeValue;
            m_IemInfo.IemBasicInfo.XZZ_CityName = lueXZZ_CityID.Text;
            m_IemInfo.IemBasicInfo.XZZ_DistrictID = lueXZZ_DistrictID.CodeValue;
            m_IemInfo.IemBasicInfo.XZZ_DistrictName = lueXZZ_DistrictID.Text;
            m_IemInfo.IemBasicInfo.XZZ_TEL = txtXZZ_TEL.Text;
            m_IemInfo.IemBasicInfo.XZZ_Post = txtXZZ_Post.Text;

            //第六行
            m_IemInfo.IemBasicInfo.HKDZ_ProvinceID = lueHKDZ_ProvinceID.CodeValue;
            m_IemInfo.IemBasicInfo.HKDZ_ProvinceName = textEdit_hkdz.Text.Trim();
            m_IemInfo.IemBasicInfo.HKDZ_CityID = lueHKDZ_CityID.CodeValue;
            m_IemInfo.IemBasicInfo.HKDZ_CityName = lueHKDZ_CityID.Text;
            m_IemInfo.IemBasicInfo.HKDZ_DistrictID = lueHKDZ_DistrictID.CodeValue;
            m_IemInfo.IemBasicInfo.HKDZ_DistrictName = lueHKDZ_DistrictID.Text;
            m_IemInfo.IemBasicInfo.HKDZ_Post = txtHKDZ_Post.Text;

            //第七行
            m_IemInfo.IemBasicInfo.OfficePlace = txtOfficePlace.Text;
            m_IemInfo.IemBasicInfo.OfficeTEL = txtOfficeTEL.Text;
            m_IemInfo.IemBasicInfo.OfficePost = txtOfficePost.Text;

            //第八行
            m_IemInfo.IemBasicInfo.ContactPerson = txtContactPerson.Text;
            m_IemInfo.IemBasicInfo.RelationshipID = lueRelationship.CodeValue;
            m_IemInfo.IemBasicInfo.RelationshipName = lueRelationship.Text;
            m_IemInfo.IemBasicInfo.ContactAddress = txtContactAddress.Text;
            m_IemInfo.IemBasicInfo.ContactTEL = txtContactTEL.Text;

            //第九行 入院途径
            if (chkInHosType1.Checked)
                m_IemInfo.IemBasicInfo.InHosType = "1";
            else if (chkInHosType2.Checked)
                m_IemInfo.IemBasicInfo.InHosType = "2";
            else if (chkInHosType3.Checked)
                m_IemInfo.IemBasicInfo.InHosType = "3";
            else
                m_IemInfo.IemBasicInfo.InHosType = "9";
            m_IemInfo.IemBasicInfo.TypeHos = textEditotherhospital.Text;

            m_IemInfo.IemBasicInfo.InHosInfo = lueINHOSINFO.CodeValue;
            m_IemInfo.IemBasicInfo.InHosCall = lueINHOSCALL.CodeValue;

            //入院信息

            if (!(deAdmitDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.AdmitDate = deAdmitDate.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Time.ToString("HH:mm:ss");

            if (!(deMAINDIAGDATE.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.MainDiagDate = deMAINDIAGDATE.DateTime.ToString("yyyy-MM-dd") + " " + teMAINDIAGDATE.Time.ToString("HH:mm:ss");

            m_IemInfo.IemBasicInfo.AdmitDeptID = lueAdmitDept.CodeValue;
            m_IemInfo.IemBasicInfo.AdmitDeptName = lueAdmitDept.Text;
            m_IemInfo.IemBasicInfo.AdmitWardID = textEdit_rybg.Text.Trim();
            //lueAdmitWard.CodeValue;
            m_IemInfo.IemBasicInfo.AdmitWardName = textEdit_rybg.Text.Trim();
            ///lueAdmitWard.Text;

            m_IemInfo.IemBasicInfo.Trans_AdmitDeptID = lueTransAdmitDept.CodeValue;
            m_IemInfo.IemBasicInfo.Trans_AdmitDeptName = lueTransAdmitDept.Text;

            if (!(deOutWardDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.OutWardDate = deOutWardDate.DateTime.ToString("yyyy-MM-dd") + " " + teOutWardDate.Time.ToString("HH:mm:ss");
            m_IemInfo.IemBasicInfo.OutHosDeptID = lueOutHosDept.CodeValue;
            m_IemInfo.IemBasicInfo.OutHosDeptName = lueOutHosDept.Text;
            m_IemInfo.IemBasicInfo.OutHosWardID = textEdit_cybf.Text.Trim();
            //lueOutHosWard.CodeValue;
            m_IemInfo.IemBasicInfo.OutHosWardName = textEdit_cybf.Text.Trim();
            //lueOutHosWard.Text;
            m_IemInfo.IemBasicInfo.ActualDays = seActualDays.Value.ToString();

            //门诊诊断
            m_IemInfo.IemBasicInfo.MZXYZD_CODE = lueMZXYZD_CODE.DiaCode;//.CodeValue;
            m_IemInfo.IemBasicInfo.MZXYZD_NAME = lueMZXYZD_CODE.DiaValue;//.Text;

            //入院诊断
            m_IemInfo.IemBasicInfo.RYXYZD_CODE = lueRYZD_CODE.DiaCode;
            m_IemInfo.IemBasicInfo.RYXYZD_NAME = lueRYZD_CODE.DiaValue;

            m_IemInfo.IemBasicInfo.SSLCLJ = "";

            m_IemInfo.IemBasicInfo.ZYZJ = "";
            //}

            //第十四行 使用中医诊疗设备

            m_IemInfo.IemBasicInfo.ZYZLSB = "";
            m_IemInfo.IemBasicInfo.ZYZLJS = "";
            //第十五行 辨证施护
            m_IemInfo.IemBasicInfo.BZSH = "";

            #endregion


        }
        #endregion

        #region private events
        /// <summary>
        /// 下拉框联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueProvice_CodeValueChanged(object sender, EventArgs e)
        {
            ////if (lueCSD_ProvinceID.CodeValue != null)
            ////{
            //LookUpEditor lue = ((LookUpEditor)sender);
            //string provice = lue.CodeValue;
            //if (string.IsNullOrEmpty(provice)) return;

            //if (lue.Name == "lueCSD_ProvinceID")
            //{
            //    lueCSD_CityID.SqlWordbook.ExtraCondition = "provinceid = '" + provice + "'";
            //    //lueCSD_CityID.SqlWordbook = lueCSD_CityID.SqlWordbook;
            //    //lueCSD_CityID.CodeValue = "";
            //    //lueCSD_CityID.IsModified = true;
            //    //lueCSD_CityID.CodeValue = lueCSD_CityID.SqlWordbook.BookData.Rows[0]["ID"].ToString();
            //    BindCity(lueCSD_CityID, provice);
            //    //BindCity(lueCSD_CityID, City);
            //    //lueCSD_CityID.Focus();
            //}
            //else if (lue.Name == "lueJG_ProvinceID")
            //{
            //    lueJG_CityID.SqlWordbook.ExtraCondition = "provinceid = '" + provice + "'";
            //    BindCity(lueJG_CityID, provice);
            //}
            //else if (lue.Name == "lueXZZ_ProvinceID")
            //{
            //    lueXZZ_CityID.SqlWordbook.ExtraCondition = "provinceid = '" + provice + "'";
            //    BindCity(lueXZZ_CityID, provice);
            //}
            //else if (lue.Name == "lueHKDZ_ProvinceID")
            //{
            //    lueHKDZ_CityID.SqlWordbook.ExtraCondition = "provinceid = '" + provice + "'";
            //    BindCity(lueHKDZ_CityID, provice);
            //}

            //else if (lue.Name == "lueCSD_CityID")
            //{
            //    lueCSD_DistrictID.SqlWordbook.ExtraCondition = "cityid = '" + provice + "'";
            //    BindDistrict(lueCSD_DistrictID, provice);
            //}
            //else if (lue.Name == "lueXZZ_CityID")
            //{
            //    lueXZZ_DistrictID.SqlWordbook.ExtraCondition = "cityid = '" + provice + "'";
            //    BindDistrict(lueXZZ_DistrictID, provice);
            //}
            //else if (lue.Name == "lueHKDZ_CityID")
            //{
            //    lueHKDZ_DistrictID.SqlWordbook.ExtraCondition = "cityid = '" + provice + "'";
            //    BindDistrict(lueHKDZ_DistrictID, provice);
            //}
            LookUpEditor lue = ((LookUpEditor)sender);
            string provice = lue.CodeValue;
            if (string.IsNullOrEmpty(provice)) return;

            if (lue.Name == "lueCSD_ProvinceID")
            {
                lueCSD_CityID.SqlWordbook.ExtraCondition = "provinceid = '" + provice + "'";
                DataRow[] m_row = lueCSD_CityID.SqlWordbook.BookData.Select(" provinceid='" + provice + "'");
                if (m_row.Length > 0)
                {
                    lueCSD_CityID.CodeValue = m_row[0]["ID"].ToString();
                }
                //lueCSD_CityID.CodeValue = lueCSD_CityID.SqlWordbook.BookData.Rows[0]["ID"].ToString();
                //BindCity(lueCSD_CityID, provice);
            }
            else if (lue.Name == "lueJG_ProvinceID")
            {
                lueJG_CityID.SqlWordbook.ExtraCondition = "provinceid = '" + provice + "'";
                //BindCity(lueJG_CityID, provice);
                DataRow[] m_row = lueJG_CityID.SqlWordbook.BookData.Select(" provinceid='" + provice + "'");
                if (m_row.Length > 0)
                {
                    lueJG_CityID.CodeValue = m_row[0]["ID"].ToString();
                }
                //lueJG_CityID.CodeValue = lueJG_CityID.SqlWordbook.BookData.Select(" provinceid='" + provice + "'")[0]["ID"].ToString();
            }
            else if (lue.Name == "lueXZZ_ProvinceID")
            {
                lueXZZ_CityID.SqlWordbook.ExtraCondition = "provinceid = '" + provice + "'";
                DataRow[] m_row = lueXZZ_CityID.SqlWordbook.BookData.Select(" provinceid='" + provice + "'");
                if (m_row.Length > 0)
                {
                    lueXZZ_CityID.CodeValue = m_row[0]["ID"].ToString();
                }
                //lueXZZ_CityID.CodeValue = lueXZZ_CityID.SqlWordbook.BookData.Select(" provinceid='" + provice + "'")[0]["ID"].ToString();
            }
            else if (lue.Name == "lueHKDZ_ProvinceID")
            {
                lueHKDZ_CityID.SqlWordbook.ExtraCondition = "provinceid = '" + provice + "'";
                //BindCity(lueHKDZ_CityID, provice);
                DataRow[] m_row = lueHKDZ_CityID.SqlWordbook.BookData.Select(" provinceid='" + provice + "'");
                if (m_row.Length > 0)
                {
                    lueHKDZ_CityID.CodeValue = m_row[0]["ID"].ToString();
                }
                //lueHKDZ_CityID.CodeValue = lueHKDZ_CityID.SqlWordbook.BookData.Select(" provinceid='" + provice + "'")[0]["ID"].ToString();
            }

            else if (lue.Name == "lueCSD_CityID")
            {
                lueCSD_DistrictID.SqlWordbook.ExtraCondition = "cityid = '" + provice + "'";
                //BindDistrict(lueCSD_DistrictID, provice);
                DataRow[] m_row = lueCSD_DistrictID.SqlWordbook.BookData.Select(" cityid='" + provice + "'");
                if (m_row.Length > 0)
                {
                    lueCSD_DistrictID.CodeValue = m_row[0]["ID"].ToString();
                }
                else
                {
                    lueCSD_DistrictID.CodeValue = "";
                }
                //lueCSD_DistrictID.CodeValue = lueCSD_DistrictID.SqlWordbook.BookData.Select(" cityid='" + provice + "'")[0]["ID"].ToString();
            }
            else if (lue.Name == "lueXZZ_CityID")
            {
                lueXZZ_DistrictID.SqlWordbook.ExtraCondition = "cityid = '" + provice + "'";
                DataRow[] m_row = lueXZZ_DistrictID.SqlWordbook.BookData.Select(" cityid='" + provice + "'");
                if (m_row.Length > 0)
                {
                    lueXZZ_DistrictID.CodeValue = m_row[0]["ID"].ToString();
                }
                else
                {
                    lueXZZ_DistrictID.CodeValue = "";
                }
                //lueXZZ_DistrictID.CodeValue = lueXZZ_DistrictID.SqlWordbook.BookData.Select(" cityid='" + provice + "'")[0]["ID"].ToString();
            }
            else if (lue.Name == "lueHKDZ_CityID")
            {
                lueHKDZ_DistrictID.SqlWordbook.ExtraCondition = "cityid = '" + provice + "'";
                DataRow[] m_row = lueHKDZ_DistrictID.SqlWordbook.BookData.Select(" cityid='" + provice + "'");
                if (m_row.Length > 0)
                {
                    lueHKDZ_DistrictID.CodeValue = m_row[0]["ID"].ToString();
                }
                else
                {
                    lueHKDZ_DistrictID.CodeValue = "";
                }
                //lueHKDZ_DistrictID.CodeValue = lueHKDZ_DistrictID.SqlWordbook.BookData.Select(" cityid='" + provice + "'")[0]["ID"].ToString();
            }


        }
        #endregion

        private void HideSbutton()
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType() == typeof(LookUpEditor))
                    ((LookUpEditor)ctl).ShowSButton = false;
                else
                {
                    foreach (Control ct in ctl.Controls)
                    {
                        if (ct.GetType() == typeof(LookUpEditor))
                            ((LookUpEditor)ct).ShowSButton = false;
                    }
                }
            }
        }

        /// <summary>
        /// 动态在空间下方画横线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCIemBasInfoEn_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is LabelControl)
                {
                    control.Visible = false;
                    e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

                    //}
                }
                if (control is TextEdit)
                {
                    if (control.Visible == true)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));


                    }
                }
            }

        }

        private void InitForm()
        {
            //设置医院名称和位置
            if (m_App != null)
            {
                labelHospitalName.Text = m_DataHelper.GetHospitalName();
                labelHospitalName.Location = new Point((this.Width - TextRenderer.MeasureText(labelHospitalName.Text, labelHospitalName.Font).Width) / 2, labelHospitalName.Location.Y);
            }
        }

        /// <summary>
        /// 得到门诊的数据库数据
        /// </summary>
        private void getMZResult()
        {
            try
            {
                if (!string.IsNullOrEmpty(lueMZXYZD_CODE.Text.Trim()) == true)
                {
                    //GetFormLoadData("XIYI");
                    string filter = string.Empty;

                    string NameFilter = " name= '{0}'";
                    filter += string.Format(NameFilter, lueMZXYZD_CODE.Text.Trim());
                    dtXY.DefaultView.RowFilter = filter;

                    int dataResult = dtXY.DefaultView.ToTable().Rows.Count;

                    if (dataResult == 0)
                    {
                        lueMZXYZD_CODE.DiaValue = lueMZXYZD_CODE.Text.Trim();
                        lueMZXYZD_CODE.DiaCode = "";
                    }
                }
                if (string.IsNullOrEmpty(lueMZXYZD_CODE.Text.Trim()) == true)
                {
                    //GetFormLoadData("XIYI");
                    string filter = string.Empty;

                    string NameFilter = " name= '{0}'";
                    filter += string.Format(NameFilter, lueMZXYZD_CODE.Text.Trim());
                    dtXY.DefaultView.RowFilter = filter;

                    int dataResult = dtXY.DefaultView.ToTable().Rows.Count;


                    if (dataResult == 0)
                    {
                        lueMZXYZD_CODE.DiaValue = lueMZXYZD_CODE.Text.Trim();
                        lueMZXYZD_CODE.DiaCode = "";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            getMZResult();
            GetUI();

            //点击确认按钮就将数据更新到数据库
            CurrentInpatient = m_App.CurrentPatientInfo;
            CurrentInpatient.ReInitializeAllProperties();
            IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
            manger.SaveData(m_IemInfo);
            RefreshMacroData();
            btn_Close_Click(sender, e);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, m_IemInfo);
        }

        /// <summary>
        /// 刷新文书录入中的宏元素
        /// </summary>
        private void RefreshMacroData()
        {
            Control myObj = m_UCMainForm;
            while (true)
            {
                if (myObj.Name.ToUpper() == "UCEmrInput".ToUpper())
                {
                    Type t = myObj.GetType();
                    t.InvokeMember("RefreshMacroData", System.Reflection.BindingFlags.InvokeMethod, null, myObj, null);
                    return;
                }
                if (myObj.Parent != null)
                {
                    myObj = myObj.Parent;
                }
                else
                {
                    return;
                }
            }
        }

        private void lueAdmitDept_CodeValueChanged(object sender, EventArgs e)
        {
            LookUpEditor lue = ((LookUpEditor)sender);
            string deptid = lue.CodeValue;
            if (string.IsNullOrEmpty(deptid)) return;

            if (lue.Name == "lueAdmitDept")//入院科别
            {
                lueAdmitWard.SqlWordbook.ExtraCondition = "deptid='" + deptid + "'";
                DataRow[] m_row = lueAdmitWard.SqlWordbook.BookData.Select(" deptid='" + deptid + "'");
                if (m_row.Length > 0)
                {
                    lueAdmitWard.CodeValue = m_row[0]["ID"].ToString();
                }
            }
            if (lue.Name == "lueOutHosDept")//出院科别
            {
                lueOutHosWard.SqlWordbook.ExtraCondition = "deptid='" + deptid + "'";
                DataRow[] m_row = lueOutHosWard.SqlWordbook.BookData.Select(" deptid='" + deptid + "'");
                if (m_row.Length > 0)
                {
                    lueOutHosWard.CodeValue = m_row[0]["ID"].ToString();
                }
            }
        }
        /// <summary>
        /// /此事件，右键针对复选框消除选择
        /// add by ywk 2012年7月30日 09:07:38 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBZSH2_Click(object sender, EventArgs e)
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
            textEditotherhospital.Enabled = false;
            if (ControlName == "chkInHosType3")
                textEditotherhospital.Enabled = true;
            foreach (Control control in this.Controls)
            {
                if (control.Name == ControlName)
                {
                    return (CheckEdit)control;
                }
            }
            return null;
        }
    }
}
