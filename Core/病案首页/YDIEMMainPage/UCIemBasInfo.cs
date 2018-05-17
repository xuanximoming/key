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
using DrectSoft.FrameWork.WinForm.Plugin;
using Convertmy = DrectSoft.Core.UtilsForExtension;
using DevExpress.XtraEditors;
using DrectSoft.Common.Eop;
using System.Xml;
using DrectSoft.Service;
using System.Data.Common;
using DrectSoft.DSSqlHelper;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.IEMMainPage
{
    /// <summary>
    /// 病案首页基本信息编辑界面
    /// </summary>
    public partial class UCIemBasInfo : UserControl
    {
        private IDataAccess m_SqlHelper;
        private IEmrHost m_App;
        private IemMainPageInfo m_IemInfo;
        private DataHelper m_DataHelper = new DataHelper();
        private Inpatient CurrentInpatient;//add by ywk 
        private UCMainForm m_UCMainForm;
        public bool editFlag = false;  //add by cyq 2012-12-06 病案室人员编辑首页(状态改为归档)

        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                if (m_IemInfo == null)
                {
                    m_IemInfo = new IemMainPageInfo();
                }
                GetUI();
                return m_IemInfo;
            }
        }

        //声明委托(异步加载数据)
        //delegate void InitLookUpEd();

        #region 病案首页基本信息编辑界面相关方法

        public UCIemBasInfo()
        {
            try
            {
                InitializeComponent();

                m_SqlHelper = DataAccessFactory.DefaultDataAccess;
                InitData();
                //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;//解决第三方控件异步报错的问题
                //(new InitLookUpEd(InitLookUpEditor)).BeginInvoke(null, null);//异步处理模板列表
                InitLookUpEditor();
                //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = false;//解决第三方控件异步报错的问题
                //根据系统配置中的设置，进行控制患者基本信息的是否可编辑
                ControlBaseInfoEdit();
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

        public UCIemBasInfo(UCMainForm ucMainForm)
            : this()
        {
            m_UCMainForm = ucMainForm;

        }
        public string ReadAddress = string.Empty;//标识读取地址的方式 

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

                GetControlVisble();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取病案首页配置项设置界面
        /// </summary>
        private void GetControlVisble()
        {
            try
            {
                string cansee = m_DataHelper.GetConfigValueByKey("EmrInputConfig");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cansee);
                //控制界面部分控件是否显示节点
                string pageControlVisble = GetElementByTagName("IemPageContorlVisable", doc);
                //0表示不显示1表示显示
                if (pageControlVisble == "0")
                {
                    labelControl23.Text = "";
                    labelControl23.Visible = false;
                    chkAdmitInfo1.Visible = false;
                    chkAdmitInfo2.Visible = false;
                    chkAdmitInfo3.Visible = false;
                    chkAdmitInfo4.Visible = false;

                    labelControl37.Text = "";
                    labelControl36.Text = "";
                    labelControl34.Text = "";
                    labelControl33.Text = "";
                    labelControl32.Text = "";
                    labelControl24.Text = "";

                    labelControl37.Visible = false;
                    chkMandZ0.Visible = false;
                    chkMandZ1.Visible = false;
                    chkMandZ1.Visible = false;
                    chkMandZ3.Visible = false;
                    labelControl36.Visible = false;
                    chkRandC0.Visible = false;
                    chkRandC1.Visible = false;
                    chkRandC2.Visible = false;
                    chkRandC3.Visible = false;
                    labelControl34.Visible = false;
                    labelControl33.Visible = false;
                    labelControl32.Visible = false;
                    labelControl24.Visible = false;
                    chkSqAndSh0.Visible = false;
                    chkSqAndSh1.Visible = false;
                    chkSqAndSh2.Visible = false;
                    chkSqAndSh3.Visible = false;
                    chkLandB0.Visible = false;
                    chkLandB1.Visible = false;
                    chkLandB2.Visible = false;
                    chkLandB3.Visible = false;
                    chkRThree0.Visible = false;
                    chkRThree1.Visible = false;
                    chkRThree2.Visible = false;
                    chkRThree3.Visible = false;
                    chkFandB0.Visible = false;
                    chkFandB1.Visible = false;
                    chkFandB2.Visible = false;
                    chkFandB3.Visible = false;
                    this.Height -= 60;//隐藏checkBox调节窗体大小
                    Point M = new Point(370, 420);
                    Point M1 = new Point(480, 420);
                    btn_OK.Location = M;
                    btn_Close.Location = M1;
                }
                else
                {
                    labelControl23.Text = "入院病情：";
                    labelControl23.Visible = true;
                    chkAdmitInfo1.Visible = true;
                    chkAdmitInfo2.Visible = true;
                    chkAdmitInfo3.Visible = true;
                    chkAdmitInfo4.Visible = true;

                    labelControl37.Text = "门诊与住院：";
                    labelControl36.Text = "入院与出院：";
                    labelControl34.Text = "术前与术后：";
                    labelControl33.Text = "临床与病理：";
                    labelControl32.Text = "入院三日内：";
                    labelControl24.Text = "放射与病理：";

                    labelControl36.Visible = true;
                    chkMandZ0.Visible = true;
                    chkMandZ1.Visible = true;
                    chkMandZ2.Visible = true;
                    chkMandZ3.Visible = true;
                    labelControl37.Visible = true;
                    chkRandC0.Visible = true;
                    chkRandC1.Visible = true;
                    chkRandC2.Visible = true;
                    chkRandC3.Visible = true;
                    labelControl34.Visible = true;
                    labelControl33.Visible = true;
                    labelControl32.Visible = true;
                    labelControl24.Visible = true;
                    chkSqAndSh0.Visible = true;
                    chkSqAndSh1.Visible = true;
                    chkSqAndSh2.Visible = true;
                    chkSqAndSh3.Visible = true;
                    chkLandB0.Visible = true;
                    chkLandB1.Visible = true;
                    chkLandB2.Visible = true;
                    chkLandB3.Visible = true;
                    chkRThree0.Visible = true;
                    chkRThree1.Visible = true;
                    chkRThree2.Visible = true;
                    chkRThree3.Visible = true;
                    chkFandB0.Visible = true;
                    chkFandB1.Visible = true;
                    chkFandB2.Visible = true;
                    chkFandB3.Visible = true;
                }
                //地址节点
                string readAddress = GetElementByTagName("IsReadAddressInfo", doc);
                if (readAddress == "0")//0则显示详细的输入框分省市区下拉框
                {
                    txtCSDAddress.Visible = false;
                    txtJGAddress.Visible = false;
                    txtHKZZAddress.Visible = false;
                    txtXZZAddress.Visible = false;
                }
                else
                {
                    //出生地的几个
                    lueCSD_ProvinceID.Visible = false;
                    lueCSD_CityID.Visible = false;
                    labelControl50.Visible = false;
                    lueCSD_DistrictID.Visible = false;
                    labelControl51.Visible = false;
                    //籍贯的几个
                    lueJG_ProvinceID.Visible = false;
                    lueJG_CityID.Visible = false;
                    labelControl53.Visible = false;
                    labelControl54.Visible = false;
                    //现住址的几个
                    lueXZZ_ProvinceID.Visible = false;
                    lueXZZ_CityID.Visible = false;
                    lueXZZ_DistrictID.Visible = false;
                    labelControl56.Visible = false;
                    labelControl57.Visible = false;
                    labelControl58.Visible = false;
                    //户口住址的几个
                    //labelControl22.Visible = false;
                    lueHKDZ_ProvinceID.Visible = false;
                    lueHKDZ_CityID.Visible = false;
                    lueHKDZ_DistrictID.Visible = false;
                    labelControl61.Visible = false;
                    labelControl59.Visible = false;
                    labelControl60.Visible = false;
                }
                //组织代码节点
                string OrganizationCode = GetElementByTagName("ShowOrganizationCode", doc);
                if (OrganizationCode == "")
                {
                    label1.Visible = false;
                    textEditOrganizationCode.EditValue = "";
                }
                else
                {
                    label1.Visible = true;
                    textEditOrganizationCode.EditValue = OrganizationCode;
                    textEditOrganizationCode.Properties.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 通过节点名抓取节点信息
        /// Add by xlb 2013-04-22
        /// </summary>
        /// <param name="elementNodeName"></param>
        /// <param name="xDoc"></param>
        /// <returns></returns>
        private string GetElementByTagName(string elementNodeName, XmlDocument xDoc)
        {
            try
            {
                //通过节点名读取某个节点
                XmlNodeList xmlNodeList = xDoc.GetElementsByTagName(elementNodeName);
                XmlNode xmlNode = null;
                if (xmlNodeList.Count == 0)
                {
                    xmlNode = null;
                }
                else
                {
                    xmlNode = xmlNodeList[0];
                }
                //返回节点串值
                return xmlNode.InnerText;
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

        /// <summary>
        /// 设置基本信息的编辑状态(所有项目)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// </summary>
        /// <param name="flag"></param>
        private void RefreashBaesInfoEnableState(bool flag)
        {
            try
            {
                #region  设置ReadOnly
                luePayId.ReadOnly = !flag;//医疗付款方式
                txtName.Properties.ReadOnly = !flag;//姓名
                lueSex.ReadOnly = !flag;//性别
                deBirth.Properties.ReadOnly = !flag;//出生日期
                txtAge.Properties.ReadOnly = !flag;//年龄
                txtMonthAge.Properties.ReadOnly = !flag;//年龄
                txtIDNO.Properties.ReadOnly = !flag;//身份证号码
                lueMarital.ReadOnly = !flag;//婚姻
                lueJob.ReadOnly = !flag;//职业
                lueCSD_ProvinceID.ReadOnly = !flag;//出生地省
                lueCSD_CityID.ReadOnly = !flag;//出生地市
                lueCSD_DistrictID.ReadOnly = !flag;//出生地先
                lueNation.ReadOnly = !flag;//民族
                lueNationality.ReadOnly = !flag;//国籍
                lueJG_ProvinceID.ReadOnly = !flag;//籍贯省
                lueJG_CityID.ReadOnly = !flag;//籍贯市
                txtOfficePlace.Properties.ReadOnly = !flag;//工作单位地址
                txtOfficeTEL.Properties.ReadOnly = !flag;//单位电话
                txtOfficePost.Properties.ReadOnly = !flag;//工作单位邮编
                txtHKDZ_Post.Properties.ReadOnly = !flag;//户口住址邮编
                txtContactPerson.Properties.ReadOnly = !flag;//联系人姓名
                lueRelationship.ReadOnly = !flag;//联系人关系
                txtContactAddress.Properties.ReadOnly = !flag;//联系人地址
                txtContactTEL.Properties.ReadOnly = !flag;//联系人电话
                lueAdmitDept.ReadOnly = !flag;//入院科别
                lueAdmitWard.ReadOnly = !flag;//入院病区
                deAdmitDate.Properties.ReadOnly = !flag;//入院日期
                teAdmitDate.Properties.ReadOnly = !flag;//入院日期
                deOutWardDate.Properties.ReadOnly = !flag;//出院日期
                teOutWardDate.Properties.ReadOnly = !flag;//出院日期
                lueOutHosDept.ReadOnly = !flag;//出院科别
                lueOutHosWard.ReadOnly = !flag;//出院病区
                seActualDays.Properties.ReadOnly = !flag;//住院天数
                txtCardNumber.Properties.ReadOnly = !flag;//健康卡号
                seInCount.Properties.ReadOnly = !flag;//第几次出院
                txtWeight.Properties.ReadOnly = !flag;//新生儿出生体重
                txtInWeight.Properties.ReadOnly = !flag;//新生儿入院体重
                txtHKZZAddress.Properties.ReadOnly = !flag;//现住址
                txtXZZ_TEL.Properties.ReadOnly = !flag;//电话
                txtXZZ_Post.Properties.ReadOnly = !flag;//电话
                txtXZZAddress.Properties.ReadOnly = !flag;//户口地址
                lueTransAdmitDept.ReadOnly = !flag;//转入科别
                #endregion

                #region  设置Enabled
                luePayId.Enabled = flag;//医疗付款方式
                txtName.Enabled = flag;//姓名
                lueSex.Enabled = flag;//性别
                deBirth.Enabled = flag;//出生日期
                txtAge.Enabled = flag;//年龄
                txtMonthAge.Enabled = flag;//年龄 月
                txtIDNO.Enabled = flag;//身份证号码
                lueMarital.Enabled = flag;//婚姻
                lueJob.Enabled = flag;//职业
                lueCSD_ProvinceID.Enabled = flag;//出生地省
                lueCSD_CityID.Enabled = flag;//出生地市
                lueCSD_DistrictID.Enabled = flag;//出生地先
                lueNation.Enabled = flag;//民族
                lueNationality.Enabled = flag;//国籍
                lueJG_ProvinceID.Enabled = flag;//籍贯省
                lueJG_CityID.Enabled = flag;//籍贯市
                txtOfficePlace.Enabled = flag;//工作单位地址
                txtOfficeTEL.Enabled = flag;//单位电话
                txtOfficePost.Enabled = flag;//工作单位邮编
                txtHKDZ_Post.Enabled = flag;//户口住址邮编
                txtContactPerson.Enabled = flag;//联系人姓名
                lueRelationship.Enabled = flag;//联系人关系
                txtContactAddress.Enabled = flag;//联系人地址
                txtContactTEL.Enabled = flag;//联系人电话
                lueAdmitDept.Enabled = flag;//入院科别
                lueAdmitWard.Enabled = flag;//入院病区
                deAdmitDate.Enabled = flag;//入院日期
                teAdmitDate.Enabled = flag;//入院日期
                deOutWardDate.Enabled = flag;//出院日期
                teOutWardDate.Enabled = flag;//出院日期
                lueOutHosDept.Enabled = flag;//出院科别
                lueOutHosWard.Enabled = flag;//出院病区
                seActualDays.Enabled = flag;//住院天数
                txtCardNumber.Enabled = flag;//健康卡号

                seInCount.Enabled = flag;//第几次出院
                txtWeight.Enabled = flag;//新生儿出生体重
                txtInWeight.Enabled = flag;//新生儿入院体重
                txtHKZZAddress.Enabled = flag;//现住址
                txtXZZ_TEL.Enabled = flag;//电话
                txtXZZ_Post.Enabled = flag;//电话
                txtXZZAddress.Enabled = flag;//户口地址
                lueTransAdmitDept.Enabled = flag;//转入科别
                #endregion

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            InitJob();
            InitNation();
            InitNationality();
            InitRelationship();
            InitDept();
            BindAllProvince();
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
        /// 绑定页面所有省区信息
        /// </summary>
        private void BindAllProvince()
        {
            //string sql = @"select a.provinceid ID,a.provincename Name,a.py,a.wb from s_province a";
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
            lupInfo.SqlHelper = m_SqlHelper;


            string sql = string.Format(@"select a.cityid ID,a.cityname Name,a.provinceid,a.py,a.wb  from s_city a where a.provinceid = '{0}' order by a.cityid asc", provinceid);
            //if (m_GetCityData==null)
            //    m_GetCityData = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            DataTable province = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            province.Columns["NAME"].Caption = "市";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("NAME", 160);
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
        ///// <summary>
        ///// 绑定县级地区
        ///// </summary>
        ///// <param name="lueinfo"></param>
        private void BindDistrict(LookUpEditor lueinfo, string cityid)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            string sql = string.Format(@"select a.districtid ID,a.districtname NAME,a.cityid,a.py,a.wb from s_district a where a.cityid = '{0}'", cityid);
            DataTable province = m_SqlHelper.ExecuteDataTable(sql, CommandType.Text);

            province.Columns["NAME"].Caption = "县";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NAME", 160);

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

        private void FillUIInner()
        {
            try
            {
                #region
                IemMainPageInfo info = m_IemInfo;
                //设置当前病人(修复m_App病人丢失问题)
                Inpatient currentPatientInfo = null;
                if (null == m_App || null == m_App.CurrentPatientInfo)
                {
                    currentPatientInfo = DS_SqlService.GetPatientInfo(info.IemBasicInfo.NoOfInpat);
                }
                else
                {
                    currentPatientInfo = m_App.CurrentPatientInfo;
                }

                //表头
                luePayId.CodeValue = info.IemBasicInfo.PayID;
                txtCardNumber.Text = info.IemBasicInfo.CardNumber;
                txtSocialCare.Text = info.IemBasicInfo.SocialCare;

                if (info.IemBasicInfo.IsBaby == "1")//如果是婴儿，取母亲的病案号
                {
                    IemMainPageManger iem = new IemMainPageManger(m_App, null == m_App.CurrentPatientInfo ? currentPatientInfo : m_App.CurrentPatientInfo);
                    string babypat = iem.GetPatData(info.IemBasicInfo.Mother).Rows[0]["noofrecord"].ToString();
                    txtPatNoOfHis.Text = babypat;
                }
                else
                {
                    txtPatNoOfHis.Text = info.IemBasicInfo.NoofRecord.ToString();
                }


                seInCount.Value = info.IemBasicInfo.InCount == "" ? 0 : Convert.ToInt32(info.IemBasicInfo.InCount);

                //第一行
                txtName.Text = info.IemBasicInfo.Name;
                lueSex.CodeValue = info.IemBasicInfo.SexID;
                if (!String.IsNullOrEmpty(info.IemBasicInfo.Birth))
                {
                    deBirth.DateTime = Convert.ToDateTime(info.IemBasicInfo.Birth);
                }
                // txtAge.Text = app.CurrentPatientInfo.PersonalInformation.CurrentDisplayAge;
                //此处的Age显示的也要跟前面显示的一致 （泗县 ywk 修改 2012年5月9日10:21:39）
                bool isNeedAuto = true;//是否需要计算年龄，针对新生儿
                //如果病案首页里没有此病人的数据，则年龄字段是计算得到的，否则的话就是输入的编辑的值
                string sqlserach = string.Format(@" select name from  iem_mainpage_basicinfo_2012
                where noofinpat='{0}'", info.IemBasicInfo.NoOfInpat);
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(sqlserach, CommandType.Text);
                if (dt.Rows.Count > 0)//病案首页里存在该病人
                {
                    txtAge.Text = info.IemBasicInfo.Age;
                    isNeedAuto = false;
                }
                else
                {
                    txtAge.Text = (null == m_App.CurrentPatientInfo ? currentPatientInfo : m_App.CurrentPatientInfo).PersonalInformation.CurrentDisplayAge;
                }
                lueNationality.CodeValue = info.IemBasicInfo.NationalityID;

                if ((!string.IsNullOrEmpty(info.IemBasicInfo.MonthAge)) && info.IemBasicInfo.MonthAge.Split(',').Length == 3)
                {
                    txtMonthAge.Text = info.IemBasicInfo.MonthAge.Split(',')[0];
                    txtXseDay.Text = info.IemBasicInfo.MonthAge.Split(',')[1];
                }
                else
                {
                    txtMonthAge.Text = info.IemBasicInfo.MonthAge;
                }
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

                //入院病情add by ywk 2012年6月26日10:02:16 
                if (info.IemBasicInfo.AdmitInfo == "1")
                {
                    chkAdmitInfo1.Checked = true;
                }
                else if (info.IemBasicInfo.AdmitInfo == "2")
                {
                    chkAdmitInfo2.Checked = true;
                }
                else if (info.IemBasicInfo.AdmitInfo == "3")
                {
                    chkAdmitInfo3.Checked = true;
                }
                else if (info.IemBasicInfo.AdmitInfo == "4")
                {
                    chkAdmitInfo4.Checked = true;
                }

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
                //seActualDays.Value = Convertmy.ToDecimal(info.IemBasicInfo.ActualDays);

                #endregion

                #region 有关几个地址栏位的赋值
                txtCSDAddress.Text = info.IemBasicInfo.CSDAddress;
                txtJGAddress.Text = info.IemBasicInfo.JGAddress;
                txtHKZZAddress.Text = info.IemBasicInfo.HKZZAddress;
                txtXZZAddress.Text = info.IemBasicInfo.XZZAddress;
                #endregion

                #region 关于诊断符合情况的赋值
                //门诊和住院
                if (info.IemBasicInfo.MenAndInHop == "0")
                {
                    chkMandZ0.Checked = true;
                }
                if (info.IemBasicInfo.MenAndInHop == "1")
                {
                    chkMandZ1.Checked = true;
                }
                if (info.IemBasicInfo.MenAndInHop == "2")
                {
                    chkMandZ2.Checked = true;
                }
                if (info.IemBasicInfo.MenAndInHop == "3")
                {
                    chkMandZ3.Checked = true;
                }
                //入院和出院
                if (info.IemBasicInfo.InHopAndOutHop == "0")
                {
                    chkRandC0.Checked = true;
                }
                if (info.IemBasicInfo.InHopAndOutHop == "1")
                {
                    chkRandC1.Checked = true;
                }
                if (info.IemBasicInfo.InHopAndOutHop == "2")
                {
                    chkRandC2.Checked = true;
                }
                if (info.IemBasicInfo.InHopAndOutHop == "3")
                {
                    chkRandC3.Checked = true;
                }
                //术前和术后
                if (info.IemBasicInfo.BeforeOpeAndAfterOper == "0")
                {
                    chkSqAndSh0.Checked = true;
                }
                if (info.IemBasicInfo.BeforeOpeAndAfterOper == "1")
                {
                    chkSqAndSh1.Checked = true;
                }
                if (info.IemBasicInfo.BeforeOpeAndAfterOper == "2")
                {
                    chkSqAndSh2.Checked = true;
                }
                if (info.IemBasicInfo.BeforeOpeAndAfterOper == "3")
                {
                    chkSqAndSh3.Checked = true;
                }
                //临床和病理
                if (info.IemBasicInfo.LinAndBingLi == "0")
                {
                    chkLandB0.Checked = true;
                }
                if (info.IemBasicInfo.LinAndBingLi == "1")
                {
                    chkLandB1.Checked = true;
                }
                if (info.IemBasicInfo.LinAndBingLi == "2")
                {
                    chkLandB2.Checked = true;
                }
                if (info.IemBasicInfo.LinAndBingLi == "3")
                {
                    chkLandB3.Checked = true;
                }
                //入院三日内
                if (info.IemBasicInfo.InHopThree == "0")
                {
                    chkRThree0.Checked = true;
                }
                if (info.IemBasicInfo.InHopThree == "1")
                {
                    chkRThree1.Checked = true;
                }
                if (info.IemBasicInfo.InHopThree == "2")
                {
                    chkRThree2.Checked = true;
                }
                if (info.IemBasicInfo.InHopThree == "3")
                {
                    chkRThree3.Checked = true;
                }
                //放射和病理
                if (info.IemBasicInfo.FangAndBingLi == "0")
                {
                    chkFandB0.Checked = true;
                }
                if (info.IemBasicInfo.FangAndBingLi == "1")
                {
                    chkFandB1.Checked = true;
                }
                if (info.IemBasicInfo.FangAndBingLi == "2")
                {
                    chkFandB2.Checked = true;
                }
                if (info.IemBasicInfo.FangAndBingLi == "3")
                {
                    chkFandB3.Checked = true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// GET UI
        /// 抓取数据
        /// Modify by xlb 2013-03-22
        /// </summary>
        private void GetUI()
        {
            try
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
                //Modify by xlb 2013-03-22
                if (string.IsNullOrEmpty(txtMonthAge.Text) && !string.IsNullOrEmpty(txtXseDay.Text))
                {
                    m_IemInfo.IemBasicInfo.MonthAge = txtXseDay.Text + "天";
                }
                else if (string.IsNullOrEmpty(txtXseDay.Text) && !string.IsNullOrEmpty(txtMonthAge.Text))
                {
                    m_IemInfo.IemBasicInfo.MonthAge = txtMonthAge.Text + "个月";
                }
                else if (string.IsNullOrEmpty(txtMonthAge.Text) && string.IsNullOrEmpty(txtXseDay.Text))
                {
                    m_IemInfo.IemBasicInfo.MonthAge = string.Empty;
                }
                else
                {
                    m_IemInfo.IemBasicInfo.MonthAge = txtMonthAge.Text + "个月" + txtXseDay.Text + "天";
                }
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
                m_IemInfo.IemBasicInfo.IDNO = txtIDNO.Text;
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

                //第九行
                if (chkInHosType1.Checked)
                    m_IemInfo.IemBasicInfo.InHosType = "1";
                else if (chkInHosType2.Checked)
                    m_IemInfo.IemBasicInfo.InHosType = "2";
                else if (chkInHosType3.Checked)
                    m_IemInfo.IemBasicInfo.InHosType = "3";
                else
                    m_IemInfo.IemBasicInfo.InHosType = "9";

                //新增的入院病情赋给实体  add by ywk 时间
                if (chkAdmitInfo1.Checked)
                    m_IemInfo.IemBasicInfo.AdmitInfo = "1";
                else if (chkAdmitInfo2.Checked)
                    m_IemInfo.IemBasicInfo.AdmitInfo = "2";
                else if (chkAdmitInfo3.Checked)
                    m_IemInfo.IemBasicInfo.AdmitInfo = "3";
                else if (chkAdmitInfo4.Checked)
                    m_IemInfo.IemBasicInfo.AdmitInfo = "4";
                else
                    m_IemInfo.IemBasicInfo.AdmitInfo = "";

                //入院信息
                if (!(deAdmitDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                    m_IemInfo.IemBasicInfo.AdmitDate = deAdmitDate.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Time.ToString("HH:mm:ss");

                m_IemInfo.IemBasicInfo.AdmitDeptID = lueAdmitDept.CodeValue;
                m_IemInfo.IemBasicInfo.AdmitDeptName = lueAdmitDept.Text;
                m_IemInfo.IemBasicInfo.AdmitWardID = lueAdmitWard.CodeValue;
                m_IemInfo.IemBasicInfo.AdmitWardName = lueAdmitWard.Text;

                m_IemInfo.IemBasicInfo.Trans_AdmitDeptID = lueTransAdmitDept.CodeValue;
                m_IemInfo.IemBasicInfo.Trans_AdmitDeptName = lueTransAdmitDept.Text;

                if (!(deOutWardDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                    m_IemInfo.IemBasicInfo.OutWardDate = deOutWardDate.DateTime.ToString("yyyy-MM-dd") + " " + teOutWardDate.Time.ToString("HH:mm:ss");
                m_IemInfo.IemBasicInfo.OutHosDeptID = lueOutHosDept.CodeValue;
                m_IemInfo.IemBasicInfo.OutHosDeptName = lueOutHosDept.Text;
                m_IemInfo.IemBasicInfo.OutHosWardID = lueOutHosWard.CodeValue;
                m_IemInfo.IemBasicInfo.OutHosWardName = lueOutHosWard.Text;
                m_IemInfo.IemBasicInfo.ActualDays = seActualDays.Value.ToString();

                #endregion
                #region 处理的新增的几个地址的具体名称的情况
                m_IemInfo.IemBasicInfo.CSDAddress = txtCSDAddress.Text.ToString();//出生地地址
                m_IemInfo.IemBasicInfo.JGAddress = txtJGAddress.Text.ToString();//籍贯地址
                m_IemInfo.IemBasicInfo.XZZAddress = txtXZZAddress.Text.ToString();//现住址
                m_IemInfo.IemBasicInfo.HKZZAddress = txtHKZZAddress.Text.ToString();//户口住址
                #endregion


                #region 新增的几个诊断符合情况 add by ywk 2012年7月20日 10:08:22
                //门诊和住院
                if (chkMandZ0.Checked == true)
                {
                    m_IemInfo.IemBasicInfo.MenAndInHop = "0";
                }
                else if (chkMandZ1.Checked)
                {
                    m_IemInfo.IemBasicInfo.MenAndInHop = "1";
                }
                else if (chkMandZ2.Checked)
                {
                    m_IemInfo.IemBasicInfo.MenAndInHop = "2";
                }
                else if (chkMandZ3.Checked)
                {
                    m_IemInfo.IemBasicInfo.MenAndInHop = "3";
                }
                else
                {
                    m_IemInfo.IemBasicInfo.MenAndInHop = "";
                }
                //入院和出院
                if (chkRandC0.Checked)
                {
                    m_IemInfo.IemBasicInfo.InHopAndOutHop = "0";
                }
                else if (chkRandC1.Checked)
                {
                    m_IemInfo.IemBasicInfo.InHopAndOutHop = "1";
                }
                else if (chkRandC2.Checked)
                {
                    m_IemInfo.IemBasicInfo.InHopAndOutHop = "2";
                }
                else if (chkRandC3.Checked)
                {
                    m_IemInfo.IemBasicInfo.InHopAndOutHop = "3";
                }
                else
                {
                    m_IemInfo.IemBasicInfo.InHopAndOutHop = "";
                }
                //术前和术后
                if (chkSqAndSh0.Checked)
                {
                    m_IemInfo.IemBasicInfo.BeforeOpeAndAfterOper = "0";
                }
                else if (chkSqAndSh1.Checked)
                {
                    m_IemInfo.IemBasicInfo.BeforeOpeAndAfterOper = "1";
                }
                else if (chkSqAndSh2.Checked)
                {
                    m_IemInfo.IemBasicInfo.BeforeOpeAndAfterOper = "2";
                }
                else if (chkSqAndSh3.Checked)
                {
                    m_IemInfo.IemBasicInfo.BeforeOpeAndAfterOper = "3";
                }
                else
                {
                    m_IemInfo.IemBasicInfo.BeforeOpeAndAfterOper = "";
                }
                //临床和病理
                if (chkLandB0.Checked)
                {
                    m_IemInfo.IemBasicInfo.LinAndBingLi = "0";
                }
                else if (chkLandB1.Checked)
                {
                    m_IemInfo.IemBasicInfo.LinAndBingLi = "1";
                }
                else if (chkLandB2.Checked)
                {
                    m_IemInfo.IemBasicInfo.LinAndBingLi = "2";
                }
                else if (chkLandB3.Checked)
                {
                    m_IemInfo.IemBasicInfo.LinAndBingLi = "3";
                }
                else
                {
                    m_IemInfo.IemBasicInfo.LinAndBingLi = "";
                }
                //入院三日内
                if (chkRThree0.Checked)
                {
                    m_IemInfo.IemBasicInfo.InHopThree = "0";
                }
                else if (chkRThree1.Checked)
                {
                    m_IemInfo.IemBasicInfo.InHopThree = "1";
                }
                else if (chkRThree2.Checked)
                {
                    m_IemInfo.IemBasicInfo.InHopThree = "2";
                }
                else if (chkRThree3.Checked)
                {
                    m_IemInfo.IemBasicInfo.InHopThree = "3";
                }
                else
                {
                    m_IemInfo.IemBasicInfo.InHopThree = "";
                }
                //放射和病理
                if (chkFandB0.Checked)
                {
                    m_IemInfo.IemBasicInfo.FangAndBingLi = "0";
                }
                else if (chkFandB1.Checked)
                {
                    m_IemInfo.IemBasicInfo.FangAndBingLi = "1";
                }
                else if (chkFandB2.Checked)
                {
                    m_IemInfo.IemBasicInfo.FangAndBingLi = "2";
                }
                else if (chkFandB3.Checked)
                {
                    m_IemInfo.IemBasicInfo.FangAndBingLi = "3";
                }
                else
                {
                    m_IemInfo.IemBasicInfo.FangAndBingLi = "";
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        #region private events
        /// <summary>
        /// 根据科别带出相应的病区 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lueAdmitDept_CodeValueChanged(object sender, EventArgs e)
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
        /// 下拉框联动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueProvice_CodeValueChanged(object sender, EventArgs e)
        {
            //if (lueCSD_ProvinceID.CodeValue != null)
            //{
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

            #region old
            //}
            //if (lueCSD_ProvinceID.CodeValue != null)
            //{
            //    LookUpEditor lue = ((LookUpEditor)sender);
            //    string provice = lue.CodeValue;
            //    if (lue.Name == "lueCSD_ProvinceID")
            //    {
            //        BindCity(lueCSD_CityID, provice);
            //    }
            //    else if (lue.Name == "lueJG_ProvinceID")
            //    {
            //        BindCity(lueJG_CityID, provice);
            //    }
            //    else if (lue.Name == "lueXZZ_ProvinceID")
            //    {
            //        BindCity(lueXZZ_CityID, provice);
            //    }
            //    else if (lue.Name == "lueHKDZ_ProvinceID")
            //    {
            //        BindCity(lueHKDZ_CityID, provice);
            //    }

            //    else if (lue.Name == "lueCSD_CityID")
            //    {
            //        BindDistrict(lueCSD_DistrictID, provice);
            //    }
            //    else if (lue.Name == "lueXZZ_CityID")
            //    {
            //        BindDistrict(lueXZZ_DistrictID, provice);
            //    }
            //    else if (lue.Name == "lueHKDZ_CityID")
            //    {
            //        BindDistrict(lueHKDZ_DistrictID, provice);
            //    }

            //}
            #endregion
        }
        #endregion

        private void HideSbutton()
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType() == typeof(LookUpEditor))
                {
                    ((LookUpEditor)ctl).ShowSButton = false;
                }
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

        private void InitForm()
        {
            //设置医院名称和位置
            if (m_App != null)
            {
                labelHospitalName.Text = m_DataHelper.GetHospitalName();
                labelHospitalName.Location = new Point((this.Width - TextRenderer.MeasureText(labelHospitalName.Text, labelHospitalName.Font).Width) / 2, labelHospitalName.Location.Y);
            }
        }

        #region 病案首页基本信息
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

        #endregion

        //非空验证 add by wyt 2012-09-10
        private StringBuilder Verify()
        {
            string sql = "select * from appcfg where CONFIGKEY = 'EmrInputConfig'";
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
            string config = "0";
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0][2].ToString();
                int index = config.IndexOf("<IsReadAddressInfo>");
                int paramIndex = index + 19;
                config = config.Substring(paramIndex, 1);
            }

            //"<Setting><BtnPacsImageVisable>1</BtnPacsImageVisable><IemPageContorlVisable>1</IemPageContorlVisable><IsReadAddressInfo>0</IsReadAddressInfo><IsShowCheckBtn>1</IsShowCheckBtn></Setting>"
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
            if (config == "0")
            {
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
            }
            if (config == "1")
            {
                if (this.txtCSDAddress.Text.Trim() == "")
                {
                    msg.Append("出生地地址不能为空!\n\n");
                    txtCSDAddress.Focus();
                }
                if (this.txtJGAddress.Text.Trim() == "")
                {
                    msg.Append("籍贯地址不能为空!\n\n");
                    txtJGAddress.Focus();
                }
                if (this.txtHKZZAddress.Text.Trim() == "")
                {
                    msg.Append("户口地址不能为空!\n\n");
                    txtHKZZAddress.Focus();
                }
                if (this.txtXZZAddress.Text.Trim() == "")
                {
                    msg.Append("现住址不能为空!\n\n");
                    txtXZZAddress.Focus();
                }
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
            if (!this.chkAdmitInfo1.Checked && !this.chkAdmitInfo2.Checked && !this.chkAdmitInfo3.Checked && !this.chkAdmitInfo4.Checked)
            {
                msg.Append("入院病情不能为空!\n\n");
            }
            if (!this.chkInHosType1.Checked && !this.chkInHosType2.Checked && !this.chkInHosType3.Checked)
            {
                msg.Append("入院途径不能为空!\n\n");
            }
            if (!this.chkMandZ0.Checked && !this.chkMandZ1.Checked && !this.chkMandZ2.Checked && !this.chkMandZ3.Checked)
            {
                msg.Append("门诊与住院不能为空!\n\n");
            }
            if (!this.chkRandC0.Checked && !this.chkRandC1.Checked && !this.chkRandC2.Checked && !this.chkRandC3.Checked)
            {
                msg.Append("入院与出院不能为空!\n\n");
            }
            if (!this.chkSqAndSh0.Checked && !this.chkSqAndSh1.Checked && !this.chkSqAndSh2.Checked && !this.chkSqAndSh3.Checked)
            {
                msg.Append("术前与术后不能为空!\n\n");
            }
            if (!this.chkLandB0.Checked && !this.chkLandB1.Checked && !this.chkLandB2.Checked && !this.chkLandB3.Checked)
            {
                msg.Append("临床与病理不能为空!\n\n");
            }
            if (!this.chkRThree0.Checked && !this.chkRThree1.Checked && !this.chkRThree2.Checked && !this.chkRThree3.Checked)
            {
                msg.Append("入院三日内不能为空!\n\n");
            }
            if (!this.chkFandB0.Checked && !this.chkFandB1.Checked && !this.chkFandB2.Checked && !this.chkFandB3.Checked)
            {
                msg.Append("放射与病理不能为空!\n\n");
            }
            return msg;
        }

        #endregion


        #region 病案首页基本信息编辑界面相关事件

        /// <summary>
        /// 更改出生日期后，动态计算年龄  2012年5月14日 13:21:55
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
        /// 更改出院日期算出住院天数
        /// 2012年6月19日 14:17:26
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void deOutWardDate_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime dtAdmitDate = DateTime.Parse(deAdmitDate.Text + " " + teAdmitDate.Text);
                DateTime dtOutDate = DateTime.Parse(deOutWardDate.Text + " " + teOutWardDate.Text);//出院日期
                this.seActualDays.EditValue = CalcInHosDay(dtAdmitDate, dtOutDate);
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
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(this.lueSex.CodeValue))
                {
                    m_App.CustomMessageBox.MessageShow("请选择性别");
                    return;
                }                
                //设置当前病人(修复m_App病人丢失问题)
                if (null == m_App || null == m_App.CurrentPatientInfo || m_App.CurrentPatientInfo.NoOfFirstPage.ToString() != m_IemInfo.IemBasicInfo.NoOfInpat)
                {
                    CurrentInpatient = DS_SqlService.GetPatientInfo(m_IemInfo.IemBasicInfo.NoOfInpat);
                }
                else
                {
                    CurrentInpatient = m_App.CurrentPatientInfo;
                }

                string sql = "select * from appcfg where CONFIGKEY = 'EmrInputConfig'";
                DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql);
                string config = "0";
                if (dt!=null||dt.Rows.Count > 0)//原先采用截字符串
                {
                    config = dt.Rows[0][2].ToString();
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(config);
                    XmlNode xmlNode = xDoc.SelectSingleNode("/Setting/IsVerifyBasicInfo");
                    if (xmlNode != null)
                    {
                        config = xmlNode.InnerText;
                    }
                }
                if (config == "1")
                {
                    StringBuilder sb = Verify();
                    if (sb.ToString() != "")
                    {
                        ShowVerify frm = new ShowVerify(sb);
                        frm.ShowDialog();
                        return;
                    }
                }
                GetUI();
                //edit by 2012-12-20 张业兴 关闭弹出框只关闭提示框
                //((ShowUC)this.Parent).Close(true, m_IemInfo);

                //点击确认按钮就将数据更新到数据库二〇一二年五月七日 11:33:57 

                //CurrentInpatient = m_App.CurrentPatientInfo;
                if (null != CurrentInpatient)
                {
                    CurrentInpatient.ReInitializeAllProperties();
                }
                IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
                manger.SaveData(m_IemInfo);

                //add by cyq 2012-12-05 病案室人员编辑后状态改为已归档
                if (editFlag)
                {
                    DS_BaseService.SetRecordsRebacked(int.Parse(CurrentInpatient.NoOfFirstPage.ToString().Trim()));
                    //int num = DS_SqlService.SetRecordsRebacked(CurrentInpatient.NoOfFirstPage.ToString());
                    //if (num == 0)
                    //{
                    //    Common.Ctrs.DLG.MessageBox.Show("该病人没有病历，无法归档。");
                    //}
                }

                RefreshMacroData();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭窗体事件
        /// 调用容器关闭方法
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

        #region 注销 by xlb  重绘导致刷屏等问题  重写控件

        ///// <summary>
        ///// 动态在空间下方画横线
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void UCIemBasInfo_Paint(object sender, PaintEventArgs e)
        //{
        //    #region 注销
        //    //foreach (Control control in this.Controls)
        //    //{
        //    //    if (control is LabelControl)
        //    //    {
        //    //        //if (control.Visible == true)
        //    //        //{
        //    //        //e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
        //    //        //    new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
        //    //        control.Visible = false;
        //    //        e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

        //    //        //}
        //    //    }
        //    //if (control is TextEdit)
        //    //{
        //    //    if (control.Visible == true)
        //    //    {
        //    //        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
        //    //            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));


        //    //    }
        //    //}
        //    //}

        //    //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
        //    //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(this.Width, 0));
        //    //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        //    #endregion
        //    try
        //    {
        //        foreach (Control control in this.Controls)
        //        {
        //            Type type=control.GetType();
        //            if (type==typeof(DateEdit)||type==typeof(LookUpEditor))
        //            {
        //                e.Graphics.DrawLine(Pens.Black,new Point(control.Location.X,control.Location.Y+control.Height),
        //                    new Point(control.Location.X+control.Width,control.Location.Y+control.Height));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyMessageBox.Show(1, ex);
        //    }
        //}

        #endregion

        /// <summary>
        /// 复选框选中后可右键取消选中
        /// add by ywk 2012年7月30日 08:43:05 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkInHosType2_CheckedChanged(object sender, EventArgs e)
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
                //更改出生日期后，年龄算出来(ywk 和泗县保持一致)2012年5月14日 13:21:44
                deBirth.EditValueChanged += new EventHandler(deBirth_EditValueChanged);
                //更改出院日期，算出住院天数 ywk 2012年6月19日 14:17:07
                deOutWardDate.EditValueChanged += new EventHandler(deOutWardDate_EditValueChanged);
                //lueAdmitDept.CodeValueChanged += new EventHandler(lueAdmitDept_CodeValueChanged);
                if (editFlag)
                {
                    RefreashBaesInfoEnableState(true);
                }
                this.ActiveControl = luePayId;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <title>出生日期范围验证，不能大于当前日期</title>
        /// <auth>wyt</auth>
        /// <date>2012-11-02</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deBirth_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (deBirth.DateTime > DateTime.Now)
                {
                    this.errorProvider.SetError(deBirth, "超出范围");
                    e.Cancel = true;
                }
                else
                {
                    this.errorProvider.Clear();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 键盘事件
        /// 控制电话文本框职能输入数字
        /// Add by xlb 2013-04-25
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //13对应的键盘是Enter,8对应的键盘是BackSpace键
                if (e.KeyChar == 13|| Char.IsDigit(e.KeyChar) || e.KeyChar == 8)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
