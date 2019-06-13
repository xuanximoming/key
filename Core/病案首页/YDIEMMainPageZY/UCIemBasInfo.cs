using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DrectSoft.Wordbook;
using DrectSoft.Common.Library;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;

using Convertmy = DrectSoft.Core.UtilsForExtension;
using DevExpress.XtraEditors;
using DrectSoft.Common.Eop;
using DrectSoft.Common;
using System.Reflection;
using DrectSoft.DSSqlHelper;
using System.Xml;
using DrectSoft.Service;

namespace DrectSoft.Core.IEMMainPageZY
{
    /// <summary>
    /// 中医病案首页基本信息编辑界面
    /// </summary>
    public partial class UCIemBasInfo : UserControl
    {
        private IDataAccess m_SqlHelper;
        private IEmrHost m_App;
        private IemMainPageInfo m_IemInfo;
        private DataHelper m_DataHelper = new DataHelper();

        private Inpatient CurrentInpatient;//add by ywk 
        private UCMainForm m_UCMainForm;
        private string m_GOTYPE;//传来此页面的操作类型
        private string m_MZTYPE;//门诊类型，是中医还是西医
        private string m_MZZDNAME;//传来的诊断名称
        private string m_MZZDCODE;//传来诊断名称相对应的诊断编码
        private string valueStr = DrectSoft.Service.YD_SqlService.GetConfigValueByKey("MainPageDiagnosisType");

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

        private string MZDiagType = string.Empty;//诊断类型
        private string GoType = string.Empty;//表明大类别的Type
        private string inputText = string.Empty;//获取文本里面的内容

        //private string m_GOTYPE;//传来此页面的操作类型
        //private string m_MZTYPE;//门诊类型，是中医还是西医
        //private string m_MZZDNAME;//传来的诊断名称
        //private string m_MZZDCODE;//传来诊断名称相对应的诊断编码

        private DataTable dtZY = new DataTable();
        private DataTable dtXY = new DataTable();

        #region 基本信息编辑界面相关方法
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

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCIemBasInfo()
        {
            try
            {
                InitializeComponent();
                m_SqlHelper = DataAccessFactory.DefaultDataAccess;
                InitControl();
                InitData();
                InitLookUpEditor();
                ControlBaseInfoEdit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重载构造
        /// </summary>
        /// <param name="ucMainForm"></param>
        public UCIemBasInfo(UCMainForm ucMainForm)
            : this()
        {
            try
            {
                m_UCMainForm = ucMainForm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重载构造
        /// </summary>
        /// <param name="app"></param>
        /// <param name="operatertype"></param>
        /// <param name="mzzycode"></param>
        /// <param name="mztype"></param>
        public UCIemBasInfo(IEmrHost app, string operatertype, string mzzycode, string mztype):this()
        {
            try
            {
                m_App = app;
                m_GOTYPE = operatertype;
                m_MZTYPE = mztype;
                m_MZZDCODE = mzzycode;
                lueMZZYZD_CODE.Text = "";
                lueMZXYZD_CODE.Text = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // edit by jxh 根据配置控制两种不同的控件显示及位置
        private void InitControl()
        {
            try
            {
                //string valueStr = Service.YD_SqlService.GetConfigValueByKey("MainPageDiagnosisType");
                if (valueStr == "0")
                {
                    lueMZZYZD_CODE.Visible = true;
                    lueMZXYZD_CODE.Visible = true;
                    lueMZZYZD_CODE1.Visible = false;
                    lueMZXYZD_CODE1.Visible = false;
                }
                else
                {
                    lueMZZYZD_CODE.Visible = false;
                    lueMZXYZD_CODE.Visible = false;
                    lueMZZYZD_CODE1.Visible = true;
                    lueMZXYZD_CODE1.Visible = true;
                    lueMZZYZD_CODE1.Left = 175;
                    lueMZZYZD_CODE1.Width = 360;
                    lueMZXYZD_CODE1.Left = 175;
                    lueMZXYZD_CODE1.Width = 360;
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得数据(省市县)
        /// </summary>
        private void InitData()
        {
            try
            {
                _province = m_SqlHelper.ExecuteDataTable("select a.provinceid ID,a.provincename Name,a.py,a.wb from s_province a");
                _city = m_SqlHelper.ExecuteDataTable("select a.cityid ID,a.cityname Name,a.provinceid,a.py,a.wb  from s_city a ");
                _qxian = m_SqlHelper.ExecuteDataTable("select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a");

                _dept = m_SqlHelper.ExecuteDataTable("select a.ID,a.NAME,a.py,a.wb from department a  where a.valid='1'");

                _ward = m_SqlHelper.ExecuteDataTable(@"select a.ID,a.NAME,a.py,a.wb,b.deptid from ward a ,dept2ward b where     a.id=b.wardid  and a.valid='1' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据系统配置控制患者基本信息是否可编辑
        /// 对应Appcfg表 IsOpenSetPaientBaseInfo
        /// </summary>
        private void ControlBaseInfoEdit()
        {
            try
            {
                string canedit = m_DataHelper.GetConfigValueByKey("IsOpenSetPaientBaseInfo");
                switch (canedit)
                {
                    case "1":/*1为可编辑,控件可使用*/
                        SetIemBaseInfoEditable(false);
                        break;
                    case "0":/*其余则只读*/
                        SetIemBaseInfoEditable(true);
                        break;
                    default:
                        SetIemBaseInfoEditable(true);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置患者基本信息是否可编辑
        /// </summary>
        private void SetIemBaseInfoEditable(bool isCanEdit)
        {
            try
            {
                luePayId.ReadOnly = isCanEdit;//医疗付款方式
                txtName.Properties.ReadOnly = isCanEdit;//姓名
                lueSex.ReadOnly = isCanEdit;//性别
                deBirth.Properties.ReadOnly = isCanEdit;//出生日期
                txtAge.Properties.ReadOnly = isCanEdit;//年龄
                txtMonthAge.Properties.ReadOnly = isCanEdit;//年龄
                txtIDNO.Properties.ReadOnly = isCanEdit;//身份证号码
                lueMarital.ReadOnly = isCanEdit;//婚姻
                lueJob.ReadOnly = isCanEdit;//职业
                lueCSD_ProvinceID.ReadOnly = isCanEdit;//出生地省
                lueCSD_CityID.ReadOnly = isCanEdit;//出生地市
                lueCSD_DistrictID.ReadOnly = isCanEdit;//出生地先
                lueNation.ReadOnly = isCanEdit;//民族
                lueNationality.ReadOnly = isCanEdit;//国籍
                lueJG_ProvinceID.ReadOnly = isCanEdit;//籍贯省
                lueJG_CityID.ReadOnly = isCanEdit;//籍贯市
                txtOfficePlace.Properties.ReadOnly = isCanEdit;//工作单位地址
                txtOfficeTEL.Properties.ReadOnly = isCanEdit;//单位电话
                txtOfficePost.Properties.ReadOnly = isCanEdit;//工作单位邮编
                txtHKDZ_Post.Properties.ReadOnly = isCanEdit;//户口住址邮编
                txtContactPerson.Properties.ReadOnly = isCanEdit;//联系人姓名
                lueRelationship.ReadOnly = isCanEdit;//联系人关系
                txtContactAddress.Properties.ReadOnly = isCanEdit;//联系人地址
                txtContactTEL.Properties.ReadOnly = isCanEdit;//联系人电话
                lueAdmitDept.ReadOnly = isCanEdit;//入院科别
                lueAdmitWard.ReadOnly = isCanEdit;//入院病区
                deAdmitDate.Properties.ReadOnly = isCanEdit;//入院日期
                teAdmitDate.Properties.ReadOnly = isCanEdit;//入院日期
                deOutWardDate.Properties.ReadOnly = isCanEdit;//出院日期
                teOutWardDate.Properties.ReadOnly = isCanEdit;//出院日期
                lueOutHosDept.ReadOnly = isCanEdit;//出院科别
                lueOutHosWard.ReadOnly = isCanEdit;//出院病区
                seActualDays.Properties.ReadOnly = isCanEdit;//住院天数
                txtCardNumber.Properties.ReadOnly = isCanEdit;//健康卡号
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

                string SqlAllDiag = @"select py, wb, name, icd from diagnosis where valid='1' union select py, wb, name, icdid from diagnosisothername where valid='1";
                dtXY = m_SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 初始化lookupeditor
        /// </summary>
        private void InitLookUpEditor()
        {
            try
            {
                InitLuePayId();
                InitLueSex();
                InitMarital();
                InitJob();
                InitNation();
                InitNationality();
                InitRelationship();
                InitDept();
                BindAllProvince();
                BindDiagnosisOfChinese();
                BindDiagnosis();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            //BindLueData(lueMZXYZD_CODE1, 12); 
            //edit by jxh  绑定西医数据
            LookUpWindow lupInfo1 = new LookUpWindow();
            lupInfo1.SqlHelper = m_SqlHelper;

            string sql = string.Format(@"select py, wb, name, icd from diagnosis where valid='1' union select py, wb, name, icdid from diagnosisothername where valid='1");
            DataTable Dept = m_SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["NAME"].Caption = "名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("ICD", 80);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ICD", "NAME", cols, "ICD//NAME//PY//WB");
            lueMZXYZD_CODE1.SqlWordbook = deptWordBook;

            lueMZXYZD_CODE1.ListWindow = lupInfo1;

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

            cols.Add("ID", 80);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
            lueMZZYZD_CODE1.SqlWordbook = deptWordBook;

            lueMZZYZD_CODE1.ListWindow = lupInfo1;

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
            string sql = string.Format(@"select a.cityid ID,a.cityname Name,a.provinceid,py,wb  from s_city a where a.provinceid = '{0}'", 

provinceid);
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
            //string sql = string.Format(@"select a.cityid ID,a.cityname Name,a.provinceid,a.py,a.wb  from s_city a where a.provinceid = '{0}'",provinceid);
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
            string sql = string.Format(@"select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a where a.cityid = '{0}'", 

cityid);
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
            //string sql = string.Format(@"select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a where a.cityid = '{0}'",cityid);
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
            DataTable dataTable = AddTableColumn(m_SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, 

CommandType.StoredProcedure));
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
        /// 公开方法调用更新信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="app"></param>
        public void FillUI(IemMainPageInfo info, IEmrHost app)
        {
            try
            {
                m_App = app;
                m_IemInfo = info;
                InitForm();
                //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
                FillUIInner();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        delegate void FillUIDelegate();

        /// <summary>
        /// 展示基本信息实体类信息
        /// Modify by xlb 2013-05-17 病案号用noofrecord字段
        /// </summary>
        private void FillUIInner()
        {
            try
            {
                IemMainPageInfo info = m_IemInfo;
                IEmrHost app = m_App;

                //表头
                luePayId.CodeValue = info.IemBasicInfo.PayID;
                txtCardNumber.Text = info.IemBasicInfo.CardNumber;
                txtSocialCare.Text = info.IemBasicInfo.SocialCare;

                if (info.IemBasicInfo.IsBaby == "1")//如果是婴儿，取母亲的病案号
                {
                    IemMainPageManger iem = new IemMainPageManger(m_App, m_App.CurrentPatientInfo);
                    string babypat = iem.GetPatData(info.IemBasicInfo.Mother).Rows[0]["noofrecord"].ToString();
                    txtPatNoOfHis.Text = babypat;
                }
                else
                {
                    txtPatNoOfHis.Text = info.IemBasicInfo.NoofRecord.ToString();
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

                //第十行 治疗类别
                if (m_IemInfo.IemBasicInfo.CURE_TYPE == "1")
                    chkCURE_TYPE1.Checked = true;
                else if (m_IemInfo.IemBasicInfo.CURE_TYPE == "1.1")
                    chkCURE_TYPE2.Checked = true;
                else if (m_IemInfo.IemBasicInfo.CURE_TYPE == "1.2")
                    chkCURE_TYPE3.Checked = true;
                else if (m_IemInfo.IemBasicInfo.CURE_TYPE == "2")
                    chkCURE_TYPE4.Checked = true;
                else if (m_IemInfo.IemBasicInfo.CURE_TYPE == "3")
                    chkCURE_TYPE5.Checked = true;

                //入院信息
                if (!String.IsNullOrEmpty(info.IemBasicInfo.AdmitDate))
                {
                    deAdmitDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
                    teAdmitDate.Time = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
                }

                lueAdmitDept.CodeValue = info.IemBasicInfo.AdmitDeptID;
                lueAdmitWard.CodeValue = info.IemBasicInfo.AdmitWardID;

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
                        this.seActualDays.EditValue = datediff;
                    }

                }
                else
                {
                    seActualDays.Value = Convertmy.ToDecimal(info.IemBasicInfo.ActualDays);
                }
                lueOutHosDept.CodeValue = info.IemBasicInfo.OutHosDeptID;
                lueOutHosWard.CodeValue = info.IemBasicInfo.OutHosWardID;


                //门急诊诊断 

                if (valueStr =="0")
                {
                    
                    lueMZZYZD_CODE.DiaCode = m_IemInfo.IemBasicInfo.MZZYZD_CODE;//.CodeValue 
                    lueMZZYZD_CODE.DiaValue = m_IemInfo.IemBasicInfo.MZZYZD_NAME;
                    lueMZZYZD_CODE.Text = m_IemInfo.IemBasicInfo.MZZYZD_NAME;
                    lueMZXYZD_CODE.DiaCode = m_IemInfo.IemBasicInfo.MZXYZD_CODE;//.CodeValue 
                    lueMZXYZD_CODE.DiaValue = m_IemInfo.IemBasicInfo.MZXYZD_NAME;
                    lueMZXYZD_CODE.Text = m_IemInfo.IemBasicInfo.MZXYZD_NAME;
                }
                else
                {                   
                    lueMZZYZD_CODE1.CodeValue = m_IemInfo.IemBasicInfo.MZZYZD_CODE;
                    lueMZXYZD_CODE1.CodeValue = m_IemInfo.IemBasicInfo.MZXYZD_CODE;//.CodeValue 
                    //lueMZZYZD_CODE1.Text = info.IemBasicInfo.MZZYZD_NAME;
                    //lueMZXYZD_CODE1.Text = info.IemBasicInfo.MZXYZD_NAME;

                }
                //lueMZXYZD_CODE1.CodeValue = m_IemInfo.IemBasicInfo.MZXYZD_CODE;


                //第十三行 实施临床路径
                if (m_IemInfo.IemBasicInfo.SSLCLJ == "1")
                    chkSSLCLJ1.Checked = true;
                else if (m_IemInfo.IemBasicInfo.SSLCLJ == "2")
                    chkSSLCLJ2.Checked = true;
                else if (m_IemInfo.IemBasicInfo.SSLCLJ == "3")
                    chkSSLCLJ3.Checked = true;


                if (m_IemInfo.IemBasicInfo.ZYZJ == "1")
                    chkZYZJ1.Checked = true;
                else if (m_IemInfo.IemBasicInfo.ZYZJ == "2")
                    chkZYZJ2.Checked = true;

                //第十四行 使用中医诊疗设备
                if (m_IemInfo.IemBasicInfo.ZYZLSB == "1")
                    chkZYZLSB1.Checked = true;
                else if (m_IemInfo.IemBasicInfo.ZYZLSB == "2")
                    chkZYZLSB2.Checked = true;

                if (m_IemInfo.IemBasicInfo.ZYZLJS == "1")
                    chkZYZLJS1.Checked = true;
                else if (m_IemInfo.IemBasicInfo.ZYZLJS == "2")
                    chkZYZLJS2.Checked = true;

                //第十五行 辨证施护
                if (m_IemInfo.IemBasicInfo.BZSH == "1")
                    chkBZSH1.Checked = true;
                else if (m_IemInfo.IemBasicInfo.BZSH == "2")
                    chkBZSH2.Checked = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新实体信息
        /// </summary>
        private void GetUI()
        {
            try
            {
                //表头
                m_IemInfo.IemBasicInfo.PayID = luePayId.CodeValue;
                m_IemInfo.IemBasicInfo.PayName = luePayId.Text;
                m_IemInfo.IemBasicInfo.CardNumber = txtCardNumber.Text;
                m_IemInfo.IemBasicInfo.SocialCare = txtSocialCare.Text;
                m_IemInfo.IemBasicInfo.PatNoOfHis = txtPatNoOfHis.Text;
                m_IemInfo.IemBasicInfo.InCount = seInCount.Value.ToString();
                m_IemInfo.IemBasicInfo.NoofRecord = txtPatNoOfHis.Text;
                //第一行
                m_IemInfo.IemBasicInfo.Name = txtName.Text;
                m_IemInfo.IemBasicInfo.SexID = lueSex.CodeValue;
                if (!(deBirth.DateTime.CompareTo(DateTime.MinValue) == 0))
                {
                    m_IemInfo.IemBasicInfo.Birth = deBirth.DateTime.ToString("yyyy-MM-dd");
                    m_IemInfo.IemBasicInfo.BirthYear = deBirth.DateTime.ToString("yyyy");
                    m_IemInfo.IemBasicInfo.BirthMonth = deBirth.DateTime.ToString("MM");
                    m_IemInfo.IemBasicInfo.BirthDay = deBirth.DateTime.ToString("dd");
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
                m_IemInfo.IemBasicInfo.CSD_ProvinceName = lueCSD_ProvinceID.Text;
                m_IemInfo.IemBasicInfo.CSD_CityID = lueCSD_CityID.CodeValue;
                m_IemInfo.IemBasicInfo.CSD_CityName = lueCSD_CityID.Text;
                m_IemInfo.IemBasicInfo.CSD_DistrictID = lueCSD_DistrictID.CodeValue;
                m_IemInfo.IemBasicInfo.CSD_DistrictName = lueCSD_DistrictID.Text;
                m_IemInfo.IemBasicInfo.JG_ProvinceID = lueJG_ProvinceID.CodeValue;
                m_IemInfo.IemBasicInfo.JG_ProvinceName = lueJG_ProvinceID.Text;
                m_IemInfo.IemBasicInfo.JG_CityID = lueJG_CityID.CodeValue;
                m_IemInfo.IemBasicInfo.JG_CityName = lueJG_CityID.Text;

                //第四行
                m_IemInfo.IemBasicInfo.IDNO = txtIDNO.Text.Trim();
                m_IemInfo.IemBasicInfo.JobID = lueJob.CodeValue;
                m_IemInfo.IemBasicInfo.JobName = lueJob.Text;
                m_IemInfo.IemBasicInfo.Marital = lueMarital.CodeValue;
                m_IemInfo.IemBasicInfo.NationID = lueNation.CodeValue;
                m_IemInfo.IemBasicInfo.NationName = lueNation.Text;

                //第五行
                m_IemInfo.IemBasicInfo.XZZ_ProvinceID = lueXZZ_ProvinceID.CodeValue;
                m_IemInfo.IemBasicInfo.XZZ_ProvinceName = lueXZZ_ProvinceID.Text;
                m_IemInfo.IemBasicInfo.XZZ_CityID = lueXZZ_CityID.CodeValue;
                m_IemInfo.IemBasicInfo.XZZ_CityName = lueXZZ_CityID.Text;
                m_IemInfo.IemBasicInfo.XZZ_DistrictID = lueXZZ_DistrictID.CodeValue;
                m_IemInfo.IemBasicInfo.XZZ_DistrictName = lueXZZ_DistrictID.Text;
                m_IemInfo.IemBasicInfo.XZZ_TEL = txtXZZ_TEL.Text;
                m_IemInfo.IemBasicInfo.XZZ_Post = txtXZZ_Post.Text;

                //第六行
                m_IemInfo.IemBasicInfo.HKDZ_ProvinceID = lueHKDZ_ProvinceID.CodeValue;
                m_IemInfo.IemBasicInfo.HKDZ_ProvinceName = lueHKDZ_ProvinceID.Text;
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

                //第十行 治疗类别
                if (chkCURE_TYPE1.Checked)
                    m_IemInfo.IemBasicInfo.CURE_TYPE = "1";
                else if (chkCURE_TYPE2.Checked)
                    m_IemInfo.IemBasicInfo.CURE_TYPE = "1.1";
                else if (chkCURE_TYPE3.Checked)
                    m_IemInfo.IemBasicInfo.CURE_TYPE = "1.2";
                else if (chkCURE_TYPE4.Checked)
                    m_IemInfo.IemBasicInfo.CURE_TYPE = "2";
                else if (chkCURE_TYPE5.Checked)
                    m_IemInfo.IemBasicInfo.CURE_TYPE = "3";

                //入院信息

                if (!(deAdmitDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                {
                    m_IemInfo.IemBasicInfo.AdmitDate = deAdmitDate.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Time.ToString("HH:mm:ss");
                    m_IemInfo.IemBasicInfo.AdmitDateYear = deAdmitDate.DateTime.ToString("yyyy");
                    m_IemInfo.IemBasicInfo.AdmitDateMonth = deAdmitDate.DateTime.ToString("MM");
                    m_IemInfo.IemBasicInfo.AdmitDateDay = deAdmitDate.DateTime.ToString("dd");
                    m_IemInfo.IemBasicInfo.AdmitHour = teAdmitDate.Time.ToString("HH");
                }
                m_IemInfo.IemBasicInfo.AdmitDeptID = lueAdmitDept.CodeValue;
                m_IemInfo.IemBasicInfo.AdmitDeptName = lueAdmitDept.Text;
                m_IemInfo.IemBasicInfo.AdmitWardID = lueAdmitWard.CodeValue;
                m_IemInfo.IemBasicInfo.AdmitWardName = lueAdmitWard.Text;

                m_IemInfo.IemBasicInfo.Trans_AdmitDeptID = lueTransAdmitDept.CodeValue;
                m_IemInfo.IemBasicInfo.Trans_AdmitDeptName = lueTransAdmitDept.Text;

                if (!(deOutWardDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                {
                    m_IemInfo.IemBasicInfo.OutWardDate = deOutWardDate.DateTime.ToString("yyyy-MM-dd") + " " + teOutWardDate.Time.ToString("HH:mm:ss");
                    m_IemInfo.IemBasicInfo.OutWardDateYear = deOutWardDate.DateTime.ToString("yyyy");
                    m_IemInfo.IemBasicInfo.OutWardMonth = deOutWardDate.DateTime.ToString("MM");
                    m_IemInfo.IemBasicInfo.OutWardDay = deOutWardDate.DateTime.ToString("dd");
                    m_IemInfo.IemBasicInfo.OutWardHour = teOutWardDate.Time.ToString("HH");
                }
                m_IemInfo.IemBasicInfo.OutHosDeptID = lueOutHosDept.CodeValue;
                m_IemInfo.IemBasicInfo.OutHosDeptName = lueOutHosDept.Text;
                m_IemInfo.IemBasicInfo.OutHosWardID = lueOutHosWard.CodeValue;
                m_IemInfo.IemBasicInfo.OutHosWardName = lueOutHosWard.Text;
                m_IemInfo.IemBasicInfo.ActualDays = seActualDays.Value.ToString();

                if (valueStr == "0")
                {
                    m_IemInfo.IemBasicInfo.MZZYZD_CODE = lueMZZYZD_CODE.DiaCode;//.CodeValue;
                    m_IemInfo.IemBasicInfo.MZZYZD_NAME = lueMZZYZD_CODE.DiaValue;//.Text;
                    //m_IemInfo.IemBasicInfo.MZXYZD_NAME

                    m_IemInfo.IemBasicInfo.MZXYZD_CODE = lueMZXYZD_CODE.DiaCode;//.CodeValue;
                    m_IemInfo.IemBasicInfo.MZXYZD_NAME = lueMZXYZD_CODE.DiaValue;//.Text;
                }
                else  
                {
                    m_IemInfo.IemBasicInfo.MZZYZD_NAME = lueMZZYZD_CODE1.DisplayValue;
                    m_IemInfo.IemBasicInfo.MZZYZD_CODE = lueMZZYZD_CODE1.CodeValue;
                    m_IemInfo.IemBasicInfo.MZXYZD_NAME = lueMZXYZD_CODE1.DisplayValue;
                    m_IemInfo.IemBasicInfo.MZXYZD_CODE = lueMZXYZD_CODE1.CodeValue;
                }

                //第十三行 实施临床路径
                if (chkSSLCLJ1.Checked)
                    m_IemInfo.IemBasicInfo.SSLCLJ = "1";
                else if (chkSSLCLJ2.Checked)
                    m_IemInfo.IemBasicInfo.SSLCLJ = "2";
                else if (chkSSLCLJ3.Checked)
                    m_IemInfo.IemBasicInfo.SSLCLJ = "3";
                else
                    m_IemInfo.IemBasicInfo.SSLCLJ = "";

                if (chkZYZJ1.Checked)
                    m_IemInfo.IemBasicInfo.ZYZJ = "1";
                else if (chkZYZJ2.Checked)
                    m_IemInfo.IemBasicInfo.ZYZJ = "2";
                else
                {
                    m_IemInfo.IemBasicInfo.ZYZJ = "";
                }

                //第十四行 使用中医诊疗设备
                if (chkZYZLSB1.Checked)
                    m_IemInfo.IemBasicInfo.ZYZLSB = "1";
                else if (chkZYZLSB2.Checked)
                    m_IemInfo.IemBasicInfo.ZYZLSB = "2";
                else
                {
                    m_IemInfo.IemBasicInfo.ZYZLSB = "";
                }
                if (chkZYZLJS1.Checked)
                    m_IemInfo.IemBasicInfo.ZYZLJS = "1";
                else if (chkZYZLJS2.Checked)
                    m_IemInfo.IemBasicInfo.ZYZLJS = "2";
                else
                {
                    m_IemInfo.IemBasicInfo.ZYZLJS = "";
                }
                //第十五行 辨证施护
                if (chkBZSH1.Checked)
                    m_IemInfo.IemBasicInfo.BZSH = "1";
                else if (chkBZSH2.Checked)
                    m_IemInfo.IemBasicInfo.BZSH = "2";
                else
                {
                    m_IemInfo.IemBasicInfo.BZSH = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        private void InitForm()
        {
            //设置医院名称和位置
            if (m_App != null)
            {
                labelHospitalName.Text = m_DataHelper.GetHospitalName();
                labelHospitalName.Location = new Point((this.Width - TextRenderer.MeasureText(labelHospitalName.Text, labelHospitalName.Font).Width)/ 2, labelHospitalName.Location.Y);
            }
        }

        /// <summary>
        /// 病案首页基本信息
        /// </summary>
        /// <returns></returns>
        public Iem_Mainpage_Basicinfo GetPrintBasicInfo()
        {
            Iem_Mainpage_Basicinfo _BasicInfo = new Iem_Mainpage_Basicinfo();


            //_BasicInfo.HospitalName = labelHospitalName.Text;
            //_BasicInfo.PayName = luePayId.Text;
            //_BasicInfo.InCount = seInCount.Value.ToString();
            //_BasicInfo.PatNoOfHis = txtPatNoOfHis.Text;
            //_BasicInfo.Name = txtName.Text;

            //_BasicInfo.SexID = lueSex.CodeValue;
            //_BasicInfo.Birth = deBirth.DateTime.ToShortDateString();
            //_BasicInfo.Age = txtAge.Text;
            //_BasicInfo.Marital = lueMarital.CodeValue;
            //_BasicInfo.JobName = lueJob.Text;

            //_BasicInfo.CountyName = lueCounty.Text;
            //_BasicInfo.NationName = lueNation.Text;
            //_BasicInfo.NationalityName = lueNationality.Text;
            //_BasicInfo.IDNO = txtIDNO.Text;
            //_BasicInfo.OfficePlace = txtOfficePlace.Text;

            //_BasicInfo.OfficeTEL = txtOfficeTEL.Text;
            //_BasicInfo.OfficePost = txtOfficePost.Text;
            //_BasicInfo.NativeAddress = txtNativeAddress.Text;
            //_BasicInfo.NativeTEL = txtNativeTEL.Text;
            //_BasicInfo.NativePost = txtNativePost.Text;

            //_BasicInfo.ContactPerson = txtContactPerson.Text;
            //_BasicInfo.Relationship = lueRelationship.Text;
            //_BasicInfo.ContactAddress = txtContactAddress.Text;
            //_BasicInfo.ContactTEL = txtContactTEL.Text;
            //_BasicInfo.AdmitDate = deAdmitDate.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Time.ToString("HH:mm:ss");

            //_BasicInfo.AdmitDeptName = lueAdmitDept.Text;
            //_BasicInfo.AdmitWardName = lueAdmitWard.Text;
            //_BasicInfo.Trans_AdmitDept = lueTransAdmitDept.Text;
            //_BasicInfo.OutWardDate = deOutWardDate.DateTime.ToString("yyyy-MM-dd") + " " + teOutWardDate.Time.ToString("HH:mm:ss");
            //_BasicInfo.OutHosDeptName = lueOutHosDept.Text;

            //_BasicInfo.OutHosWardName = lueOutHosWard.Text;
            //_BasicInfo.ActualDays = seActualDays.Value.ToString();

            return _BasicInfo;
        }

        public Iem_Mainpage_Diagnosis GetPrintDiagnosis(Iem_Mainpage_Diagnosis _Iem_Mainpage_Diagnosis)
        {

            //_Iem_Mainpage_Diagnosis.Section_Director = lueKszr.Text;
            //_Iem_Mainpage_Diagnosis.Director = lueZrys.Text;
            //_Iem_Mainpage_Diagnosis.Vs_Employee_Code = lueZzys.Text;
            //_Iem_Mainpage_Diagnosis.Resident_Employee_Code = lueZyys.Text;
            //_Iem_Mainpage_Diagnosis.Refresh_Employee_Code = lueJxys.Text;


            //_Iem_Mainpage_Diagnosis.Master_Interne = lueYjs.Text;
            //_Iem_Mainpage_Diagnosis.Interne = lueSxys.Text;
            //_Iem_Mainpage_Diagnosis.Coding_User = lueBmy.Text;
            //_Iem_Mainpage_Diagnosis.Medical_Quality_Id = m_IemInfo.IemBasicInfo.Medical_Quality_Id.ToString();
            //_Iem_Mainpage_Diagnosis.Quality_Control_Doctor = lueZkys.Text;

            //_Iem_Mainpage_Diagnosis.Quality_Control_Nurse = lueZkhs.Text;
            //_Iem_Mainpage_Diagnosis.Quality_Control_Date = m_IemInfo.IemBasicInfo.Quality_Control_Date;

            return _Iem_Mainpage_Diagnosis;
        }

        /// <summary>
        /// 得到门诊的数据库数据
        /// </summary>
        private void getMZResult()
        {
            try
            {
                if (!string.IsNullOrEmpty(lueMZZYZD_CODE.Text.Trim()) == true)
                {
                    //GetFormLoadData("ZHONGYI");
                    string filter = string.Empty;

                    string NameFilter = " name= '{0}'";
                    filter += string.Format(NameFilter, lueMZZYZD_CODE.Text.Trim());
                    dtZY.DefaultView.RowFilter = filter;

                    int dataResult = dtZY.DefaultView.ToTable().Rows.Count;

                    if (dataResult > 0)
                    {
                        lueMZZYZD_CODE.DiaValue = lueMZZYZD_CODE.Text.Trim();
                        lueMZZYZD_CODE.DiaCode = dtZY.DefaultView.ToTable().Rows[0][0].ToString();    //dtZY.row["icd"].ToString();

                    }
                    if (dataResult == 0)
                    {
                        lueMZZYZD_CODE.DiaValue = lueMZZYZD_CODE.Text.Trim();
                        lueMZZYZD_CODE.DiaCode = "";
                    }

                }
                if (string.IsNullOrEmpty(lueMZZYZD_CODE.Text.Trim()) == true)
                {
                    //GetFormLoadData("ZHONGYI");
                    string filter = string.Empty;

                    string NameFilter = " name= '{0}'";
                    filter += string.Format(NameFilter, lueMZZYZD_CODE.Text.Trim());
                    dtZY.DefaultView.RowFilter = filter;

                    int dataResult = dtZY.DefaultView.ToTable().Rows.Count;

                    if (dataResult > 0)
                    {
                        lueMZZYZD_CODE.DiaValue = lueMZZYZD_CODE.Text.Trim();
                        lueMZZYZD_CODE.DiaCode = dtZY.DefaultView.ToTable().Rows[0][0].ToString();    //dtZY.row["icd"].ToString();

                    }
                    if (dataResult == 0)
                    {
                        lueMZZYZD_CODE.DiaValue = lueMZZYZD_CODE.Text.Trim();
                        lueMZZYZD_CODE.DiaCode = "";
                    }

                }
                if (!string.IsNullOrEmpty(lueMZXYZD_CODE.Text.Trim()) == true)
                {
                    //GetFormLoadData("XIYI");
                    string filter = string.Empty;

                    string NameFilter = " name= '{0}'";
                    filter += string.Format(NameFilter, lueMZXYZD_CODE.Text.Trim());
                    dtXY.DefaultView.RowFilter = filter;

                    int dataResult = dtXY.DefaultView.ToTable().Rows.Count;

                    if (dataResult > 0)
                    {
                        lueMZXYZD_CODE.DiaValue = lueMZXYZD_CODE.Text.Trim();
                        lueMZXYZD_CODE.DiaCode = dtXY.DefaultView.ToTable().Rows[0]["icd"].ToString();
                        //lueMZXYZD_CODE.DiaCode = dtXY.DefaultView.ToTable().Rows[0][3].ToString();    //dtZY.row["icd"].ToString();

                    }
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

                    if (dataResult > 0)
                    {
                        lueMZXYZD_CODE.DiaValue = lueMZXYZD_CODE.Text.Trim();
                        lueMZXYZD_CODE.DiaCode = dtXY.DefaultView.ToTable().Rows[0]["icd"].ToString();
                        //lueMZXYZD_CODE.DiaCode = dtXY.DefaultView.ToTable().Rows[0][3].ToString();    //dtZY.row["icd"].ToString();

                    }
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

        #endregion

        #region 基本信息编辑界面基本事件

        /// <summary>
        /// 键盘事件
        /// enter键修改勾选状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar ==13)
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
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCIemBasInfo_Load(object sender, EventArgs e)
        {
            try
            {
                GetFormLoadData();
                //更改出生日期后，年龄算出来(ywk 泗县修改)
                deBirth.EditValueChanged += new EventHandler(deBirth_EditValueChanged);
                //更改出院日期，算出住院天数 ywk 2012年6月19日 14:17:07
                deOutWardDate.EditValueChanged += new EventHandler(deOutWardDate_EditValueChanged);
                teOutWardDate.EditValueChanged += new EventHandler(deOutWardDate_EditValueChanged);
                deAdmitDate.EditValueChanged+=new EventHandler(deAdmitDate_EditValueChanged);
                teAdmitDate.EditValueChanged += new EventHandler(deAdmitDate_EditValueChanged);
                this.ActiveControl = luePayId;
                if (CheckIsNeedSetDefault())
                {
                    DrectSoft.Common.DS_Common.SetDefaultValue(this);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 联动事件选择选择可别联动病区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueAdmitDept_CodeValueChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 判断是否已存在病案基本信息存在则无需设默认值
        /// </summary>
        /// <returns></returns>
        private bool CheckIsNeedSetDefault()
        {
            try
            {
                if (string.IsNullOrEmpty(YD_SqlHelper.ConnectionString))
                {
                    YD_SqlHelper.CreateSqlHelper();
                }
                object count=YD_SqlHelper.ExecuteScalar(@"select count(*) from iem_mainpage_basicinfo_sx 
                where iem_mainpage_no=@iemMainpapgeNo", new SqlParameter[]{new SqlParameter("@iemMainpapgeNo",
                m_IemInfo == null || m_IemInfo.IemBasicInfo == null || m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == null ? "" : 

m_IemInfo.IemBasicInfo.Iem_Mainpage_NO)}, CommandType.Text);
                bool result = int.Parse(count.ToString()) > 0 ? false : true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存事件
        /// 保存后不关闭窗体
        /// <auth>xlb</auth>
        /// <modifydate>2013-05-27</modifydate>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                getMZResult();
                if (CheckConfig())
                {
                    StringBuilder sb = Verify();
                    if (sb.ToString() != "")
                    {
                        ShowVerify frm = new ShowVerify(sb);
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog(this);
                        return;
                    }
                }
                GetUI();
                //((ShowUC)this.Parent).Close(true, m_IemInfo);

                //点击确认按钮就将数据更新到数据库
                CurrentInpatient = m_App.CurrentPatientInfo;
                CurrentInpatient.ReInitializeAllProperties();
                IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
                manger.SaveData(m_IemInfo);
                RefreshMacroData();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 校验是否需要开启验证窗体
        /// </summary>
        /// <returns></returns>
        private bool CheckConfig()
        {
            try
            {
                string configValue=YD_SqlService.GetConfigValueByKey("EmrInputConfig");
                if (string.IsNullOrEmpty(configValue))
                {
                    return false;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(configValue);
                XmlNode xmlNode = xmlDoc.SelectSingleNode("/Setting/IsVerifyBasicInfo");
                if (xmlNode == null)
                {
                    return false;
                }
                bool result = xmlNode.InnerText.Trim()=="1"?true:false;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private StringBuilder Verify()
        {
            try
            {
                StringBuilder msg = new StringBuilder();
                //医疗付款方式
                if (luePayId.CodeValue == "")
                {
                    msg.Append("付款方式不能为空！\n");
                    //luePayId.Focus();
                }
                //姓名
                if (txtName.Text.Trim() == "")
                {
                    msg.Append("姓名不能为空!\n\n");
                    txtName.Focus();
                }
                if (lueSex.CodeValue == "")
                {
                    msg.Append("性别不能为空!\n\n");
                    //lueSex.Focus();
                }
                if (deBirth.Text.Trim() == "")
                {
                    msg.Append("出生日期不能为空!\n\n");
                    deBirth.Focus();
                }
                if (txtAge.Text.Trim() == "")
                {
                    msg.Append("年龄不能为空!\n\n");
                    txtAge.Focus();
                }
                if (txtIDNO.Text.Trim() == "")
                {
                    msg.Append("身份证号不能为空!\n\n");
                    txtIDNO.Focus();
                }
                if (lueMarital.CodeValue == "")
                {
                    msg.Append("婚姻状况不能为空!\n\n");
                    //lueMarital.Focus();
                }
                if (lueJob.CodeValue == "")
                {
                    msg.Append("职业不能为空!\n\n");
                    //lueJob.Focus();
                }

                if (lueCSD_ProvinceID.CodeValue == "")
                {
                    msg.Append("出生地省份不能为空!\n\n");
                    //lueCSD_ProvinceID.Focus();
                }
                if (lueCSD_CityID.CodeValue == "")
                {
                    msg.Append("出生地城市不能为空!\n\n");
                    //lueCSD_CityID.Focus();
                }
                if (lueCSD_DistrictID.CodeValue == "")
                {
                    msg.Append("出生地县不能为空!\n\n");
                    //lueCSD_DistrictID.Focus();
                }
                if (this.lueJG_ProvinceID.CodeValue == "")
                {
                    msg.Append("籍贯省份不能为空!\n\n");
                    //lueJG_ProvinceID.Focus();
                }
                if (this.lueJG_CityID.CodeValue == "")
                {
                    msg.Append("籍贯市县不能为空!\n\n");
                    //lueCSD_DistrictID.Focus();
                }
                if (this.lueXZZ_ProvinceID.CodeValue == "")
                {
                    msg.Append("现住址省份不能为空!\n\n");
                    //lueXZZ_ProvinceID.Focus();
                }
                if (this.lueXZZ_CityID.CodeValue == "")
                {
                    msg.Append("现住址市不能为空!\n\n");
                    //lueXZZ_CityID.Focus();
                }
                if (this.lueXZZ_DistrictID.CodeValue == "")
                {
                    msg.Append("现住址区县不能为空!\n\n");
                    //lueXZZ_DistrictID.Focus();
                }
                if (this.lueHKDZ_ProvinceID.CodeValue == "")
                {
                    msg.Append("户口省份不能为空!\n\n");
                    //lueHKDZ_ProvinceID.Focus();
                }
                if (this.lueHKDZ_CityID.CodeValue == "")
                {
                    msg.Append("户口市不能为空!\n\n");
                    //lueHKDZ_CityID.Focus();
                }
                if (this.lueHKDZ_DistrictID.CodeValue == "")
                {
                    msg.Append("户口区县不能为空!\n\n");
                    //lueHKDZ_DistrictID.Focus();
                }

                if (lueNation.CodeValue == "")
                {
                    msg.Append("民族不能为空!\n\n");
                    //lueNation.Focus();
                }
                if (lueNationality.CodeValue == "")
                {
                    msg.Append("国籍不能为空!\n\n");
                    //lueNationality.Focus();
                }
                if (this.txtXZZ_TEL.Text.Trim() == "")
                {
                    msg.Append("现住址电话不能为空!\n\n");
                    txtXZZ_TEL.Focus();
                }
                if (this.txtXZZ_Post.Text.Trim() == "")
                {
                    msg.Append("现住址邮编不能为空!\n\n");
                    txtXZZ_Post.Focus();
                }
                if (txtOfficePlace.Text.Trim() == "")
                {
                    msg.Append("工作单位地址不能为空!\n\n");
                    txtOfficePlace.Focus();
                }
                if (txtOfficeTEL.Text.Trim() == "")
                {
                    msg.Append("单位电话不能为空!\n\n");
                    txtOfficeTEL.Focus();
                }
                if (txtOfficePost.Text.Trim() == "")
                {
                    msg.Append("工作单位邮编不能为空!\n\n");
                    txtOfficePost.Focus();
                }
                if (txtHKDZ_Post.Text.Trim() == "")
                {
                    msg.Append("户口地址邮编不能为空!\n\n");
                    txtHKDZ_Post.Focus();
                }
                if (txtContactPerson.Text.Trim() == "")
                {
                    msg.Append("联系人姓名不能为空!\n\n");
                    txtContactPerson.Focus();
                }
                if (lueRelationship.CodeValue == "")
                {
                    msg.Append("联系人关系不能为空!\n\n");
                    //lueRelationship.Focus();
                }
                if (txtContactAddress.Text.Trim() == "")
                {
                    msg.Append("联系人地址不能为空!\n\n");
                    txtContactAddress.Focus();
                }
                if (txtContactTEL.Text.Trim() == "")
                {
                    msg.Append("联系人电话不能为空!\n\n");
                    txtContactTEL.Focus();
                }
                if (lueAdmitDept.CodeValue == "")
                {
                    msg.Append("入院科别不能为空!\n\n");
                    //lueAdmitDept.Focus();
                }
                if (lueAdmitWard.CodeValue == "")
                {
                    msg.Append("入院病区不能为空!\n\n");
                    //lueAdmitWard.Focus();
                }
                //if (this.lueTransAdmitDept.CodeValue == "")
                //{
                //    msg.Append("转科科别不能为空!\n\n");
                //    lueTransAdmitDept.Focus();
                //}
                if (deAdmitDate.Text.Trim() == "")
                {
                    msg.Append("入院日期不能为空!\n\n");
                    deAdmitDate.Focus();
                }
                if (teAdmitDate.Text.Trim() == "")
                {
                    msg.Append("入院时间不能为空!\n\n");
                    teAdmitDate.Focus();
                }
                if (deOutWardDate.Text.Trim() == "")
                {
                    msg.Append("出院日期不能为空!\n\n");
                    deOutWardDate.Focus();
                }
                if (teOutWardDate.Text.Trim() == "")
                {
                    msg.Append("出院时间不能为空!\n\n");
                    teOutWardDate.Focus();
                }
                if (lueOutHosDept.CodeValue == "")
                {
                    msg.Append("出院科别不能为空!\n\n");
                    //lueOutHosDept.Focus();
                }
                if (lueOutHosWard.CodeValue == "")
                {
                    msg.Append("出院病区不能为空!\n\n");
                    //lueOutHosWard.Focus();
                }
                if (seActualDays.Text.Trim() == "")
                {
                    msg.Append("住院天数不能为空!\n\n");
                    seActualDays.Focus();
                }
                if (txtCardNumber.Text.Trim() == "")
                {
                    msg.Append("健康卡号不能为空!\n\n");
                    txtCardNumber.Focus();
                }
                return msg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更改出生日期后，动态计算年龄 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deBirth_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtAge.Text = PatientBasicInfo.CalcDisplayAge(deBirth.DateTime, DateTime.Now);
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, EventArgs e)
        {
            try
            {
                ((ShowUC)this.Parent).Close(false, null);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 下拉框联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueProvice_CodeValueChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 更改出院日期算出住院天数
        /// 2012年6月19日 14:17:26
        /// Modify by xlb 2013-05-23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deOutWardDate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dtAdmitDate = DateTime.Parse(deAdmitDate.Text + " " + teAdmitDate.Text);
                DateTime dtOutDate = DateTime.Parse(deOutWardDate.Text + " " + teOutWardDate.Text);//出院日期
                this.seActualDays.EditValue = CalcInHosDay(dtAdmitDate,dtOutDate);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 计算住院天数
        /// </summary>
        /// <param name="dateBegin"></param>
        /// <param name="End"></param>
        /// <returns></returns>
        private int CalcInHosTime(DateTime dateBegin, DateTime End)
        {
            try
            {
                TimeSpan timesLost = End - dateBegin;
                int dayPart = timesLost.Days;//时间差的天数
                int minutes = timesLost.Minutes;//时间差分钟
                int hours = timesLost.Hours;//时间差小时部分
                if (dayPart < 0)//足天且小于0则默认住院天数0天
                {
                    return 0;
                }
                else
                {
                    if (minutes >= 0)//出院时间和入院时间差若分钟大于等于0则表示满一天
                    {
                        if (hours >= 0 && hours < 24)//不足24小时算一天
                        {
                            return dayPart + 1;
                        }
                        else
                        {
                           return 0;//此情况出现于不足天时间部分为负数小时
                        }
                    }
                    else
                    {
                       return dayPart;//此情况只存在不足天且时间差小于0
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <purpose>该方法用于计算住院天数</purpose>
        /// <date>2013-06-18</date>
        /// <auth>XLB</auth>
        /// </summary>
        /// <param name="dateBegin">开始日期</param>
        /// <param name="dateEnd">结束日期</param>
        /// <returns>天数</returns>
        int CalcInHosDay(DateTime dateBegin, DateTime dateEnd)
        {
            try
            {
                /*时间差天部分*/
                int daysPart = (dateEnd - dateBegin).Days;
                if (dateEnd < dateBegin)
                {
                    return 0;/*结束日期小于开始日期则返回0*/
                }
                else if (daysPart < 0)
                {
                    return 0;
                }
                else if (daysPart == 0)
                {
                    return daysPart + 1;/*不足一天则记为一天*/
                }
                else
                {
                    return daysPart;/*超过一天则只计算天数*/
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更改入院时间计算住院天数
        /// Add by xlb 2013-05-23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deAdmitDate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dtAdmitDate = DateTime.Parse(deAdmitDate.Text + " " + teAdmitDate.Text);
                DateTime dtOutDate = DateTime.Parse(deOutWardDate.Text + " " + teOutWardDate.Text);//出院日期
                this.seActualDays.EditValue = CalcInHosDay(dtAdmitDate,dtOutDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 西医诊断框键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MessageBox.Show(ex.Message);
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
                if (m_MZTYPE == "ZHONGYI")
                {
                    if (mzdiagInfo.IsClosed)
                    {
                        lueMZZYZD_CODE.Text = mzdiagInfo.inText;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 中医诊断框键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bwj1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Button btn = new Button();
                if (e.KeyChar == 13)//lueMZZYZD_CODE.Text.Trim() != null && 
                {
                    GoType = "MZDIAG";
                    MZDiagType = "ZHONGYI";
                    inputText = lueMZZYZD_CODE.Text.Trim();

                    IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtZY, GoType, MZDiagType, inputText);
                    if (diagInfo.GetFormResult())
                    {
                        diagInfo.ShowDialog();
                        if (diagInfo.IsClosed)
                        {
                            lueMZZYZD_CODE.Text = diagInfo.inText;
                            lueMZZYZD_CODE.DiaCode = diagInfo.inCode;
                            lueMZZYZD_CODE.DiaValue = diagInfo.inText;
                        }
                    }
                    else
                    {
                        lueMZZYZD_CODE.DiaCode = diagInfo.inCode;
                        lueMZZYZD_CODE.DiaValue = diagInfo.inText;
                        lueMZZYZD_CODE.Multiline = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void labelLogoName_Click(object sender, EventArgs e)
        {

        }

        private void labelControl47_Click(object sender, EventArgs e)
        {

        }

        private void labelControl48_Click(object sender, EventArgs e)
        {

        }

        private void txtInWeight_EditValueChanged(object sender, EventArgs e)
        {

        }

        #region =======注销===========

        ///// <summary>
        ///// <Auth>XLB</Auth>
        ///// <date>2013-07-01</date>
        ///// 设置控件属性默认值
        ///// </summary>
        //private void SetDefaultValue()
        //{
        //    try
        //    {
        //        Type type = this.GetType();
        //        IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
        //        DataTable dataControls = manger.GetControlsByForm(type.ToString());
        //        object obj;
        //        if (dataControls == null || dataControls.Rows.Count <= 0)
        //        {
        //            return;
        //        }
        //        foreach (DataRow item in dataControls.Rows)
        //        {
        //            obj = Assembly.Load("Common.Ctrs").CreateInstance(item["CONTROLSTYLE"].ToString().Trim(), true);
        //            //匹配指定名属性
        //            Control[] control = this.Controls.Find(item["Controlname"].ToString(), true);
        //            if (control != null && control[0] != null)
        //            {
        //                //设置指定属性默认值
        //                ReflectionSetProperty(control[0] as object, item["PROPERTY"].ToString().Trim(), (object)item["CONTROLVALUE"].ToString());
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 修改指定控件指定属性值
        ///// </summary>
        ///// <param name="objclass">当前控件</param>
        ///// <param name="propertyName">属性值</param>
        ///// <param name="value">属性值</param>
        //private void ReflectionSetProperty(object objclass, string propertyName, object value)
        //{
        //    try
        //    {
        //        //获取当前对象的所有属性
        //        PropertyInfo[] infos = objclass.GetType().GetProperties();
        //        foreach (PropertyInfo item in infos)
        //        {
        //            //遍历属性找到指定属性
        //            if (item.Name == propertyName && item.CanWrite/*属性是否可写*/)
        //            {
        //                if (!item.PropertyType.IsGenericType)
        //                {
        //                    //获取当前属性的类型若为非泛型则转换成属性类型的对象
        //                    if (item.PropertyType.IsEnum)
        //                    {
        //                        //枚举类型转换
        //                        object objValue = Enum.Parse(item.PropertyType, value.ToString(),true);
        //                        item.SetValue(objclass, string.IsNullOrEmpty(value.ToString()) ? null : Convert.ChangeType(objValue,item.PropertyType), null);
        //                    }
        //                    else if (item.PropertyType == typeof(System.Drawing.Color))
        //                    {
        //                        /*设置值作为颜色结构体一个属性并返回*/
        //                        item.SetValue(objclass, CreateColor(value), null);
        //                    }
        //                }
        //                else
        //                {
        //                    //泛型
        //                    Type type = item.PropertyType.GetGenericTypeDefinition();
        //                    if (type == typeof(Nullable<>))
        //                    {
        //                        item.SetValue(objclass, string.IsNullOrEmpty(value.ToString()) ? null : Convert.ChangeType(value,Nullable.GetUnderlyingType(item.PropertyType)), null);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// 根据字符生成对应的颜色
        ///// <auth>XLB</auth>
        ///// <date>2013-07-01</date>
        ///// </summary>
        ///// <param name="colorName"></param>
        ///// <returns></returns>
        //private Color CreateColor(object colorName)
        //{
        //    try
        //    {
        //        //获取结构体类型
        //        Type colorType = typeof(Color);

        //        //匹配字符串指定的属性
        //        PropertyInfo property = colorType.GetProperty
        //        (colorName.ToString(),BindingFlags.Public | BindingFlags.NonPublic 
        //        | BindingFlags.Instance| BindingFlags.Static 
        //        | BindingFlags.DeclaredOnly );

        //        if (property == null)
        //        {
        //            throw new Exception("属性配置错误");
        //        }
        //        //返回字符串指定的颜色
        //        return (Color)property.GetValue(null,null);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #endregion

    }
}