using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Core.NursingDocuments.PublicSet;
using System.Data.SqlClient;
using DrectSoft.Common.Library;
using DrectSoft.Wordbook;

namespace DrectSoft.Core.NursingDocuments.UserControls
{
    public partial class UCNursingRecordTable : DevExpress.XtraEditors.XtraUserControl
    {
        #region 定义字段
        //控件集
        List<LabelControl> m_lblTimeList = new List<LabelControl>();//时间段
        List<LookUpEdit> m_lookUpWayOfSurveyList = new List<LookUpEdit>();//体温测量部位
        List<TextEdit> m_txtTemperatureList = new List<TextEdit>();//体温
        List<TextEdit> m_txtPulseList = new List<TextEdit>();//脉搏
        List<TextEdit> m_txtBreatheList = new List<TextEdit>();//呼吸
        List<UCTextGroupBox> m_UcTimeOfShitList = new List<UCTextGroupBox>();//大便次数
        List<UCTextSeparateBox> m_UcTxtSeparateBoxList = new List<UCTextSeparateBox>();//血压
        List<TextEdit> m_txtCountInList = new List<TextEdit>();//入量
        List<TextEdit> m_txtCountOutList = new List<TextEdit>();//出量 作为尿量
        List<TextEdit> m_txtOther2List = new List<TextEdit>();//出量
        List<TextEdit> m_txtDrainageList = new List<TextEdit>();//引流量
        List<TextEdit> m_txtHeightList = new List<TextEdit>();//身高
        List<TextEdit> m_txtWeightList = new List<TextEdit>();//体重
        List<TextEdit> m_txtAllergyList = new List<TextEdit>();//过敏
        List<TextEdit> m_txtHeartRateList = new List<TextEdit>();//心率
        List<TextEdit> m_txtDaysAfterSurgeryList = new List<TextEdit>();//手术后日数
        List<TextEdit> m_txtDayOfHospitalList = new List<TextEdit>();//住院天数
        List<TextEdit> m_txtPhysicalCoolingList = new List<TextEdit>();//物理降温
        //新增的物理升温和疼痛 add by ywk
        List<TextEdit> m_txtPhysicalHottingList = new List<TextEdit>();//物理升温
        //List<TextEdit> m_txtPainInfoList = new List<TextEdit>();//疼痛
        //疼痛改为下拉框（数据从配置中读取）
        List<LookUpEditor> m_lookPainInfoList = new List<LookUpEditor>();

        //时间段数组
        string[] m_TimeSlot = null;

        //获取焦点的文本控件
        TextEdit m_ActivateTextEdit = new TextEdit();

        //护理信息允许修改的天数
        int m_DayOfModify = 0;

        #endregion

        #region 初始化窗体

        public UCNursingRecordTable()
        {
            InitializeComponent();
        }


        #region 初始化控件数组
        /// <summary>
        /// 初始化控件数组
        /// </summary>
        private void InitControlArray()
        {
            //添加时间段
            m_lblTimeList.Add(lblTime1);
            m_lblTimeList.Add(lblTime2);
            m_lblTimeList.Add(lblTime3);
            m_lblTimeList.Add(lblTime4);
            m_lblTimeList.Add(lblTime5);
            m_lblTimeList.Add(lblTime6);

            //添加体温测量部位
            m_lookUpWayOfSurveyList.Add(lookUpWayOfSurvey1);
            m_lookUpWayOfSurveyList.Add(lookUpWayOfSurvey2);
            m_lookUpWayOfSurveyList.Add(lookUpWayOfSurvey3);
            m_lookUpWayOfSurveyList.Add(lookUpWayOfSurvey4);
            m_lookUpWayOfSurveyList.Add(lookUpWayOfSurvey5);
            m_lookUpWayOfSurveyList.Add(lookUpWayOfSurvey6);
            for (int i = 0; i < 6; i++)
            {
                m_lookUpWayOfSurveyList[i].EditValue = "8801";
            }

            //添加体温
            m_txtTemperatureList.Add(txtTemperature1);
            m_txtTemperatureList.Add(txtTemperature2);
            m_txtTemperatureList.Add(txtTemperature3);
            m_txtTemperatureList.Add(txtTemperature4);
            m_txtTemperatureList.Add(txtTemperature5);
            m_txtTemperatureList.Add(txtTemperature6);

            //添加脉搏
            m_txtPulseList.Add(txtPulse1);
            m_txtPulseList.Add(txtPulse2);
            m_txtPulseList.Add(txtPulse3);
            m_txtPulseList.Add(txtPulse4);
            m_txtPulseList.Add(txtPulse5);
            m_txtPulseList.Add(txtPulse6);

            //添加呼吸
            m_txtBreatheList.Add(txtBreathe1);
            m_txtBreatheList.Add(txtBreathe2);
            m_txtBreatheList.Add(txtBreathe3);
            m_txtBreatheList.Add(txtBreathe4);
            m_txtBreatheList.Add(txtBreathe5);
            m_txtBreatheList.Add(txtBreathe6);

            //添加大便次数
            m_UcTimeOfShitList.Add(ucTxtTimeOfShit1);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit2);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit3);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit4);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit5);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit6);

            //添加血压
            m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure1);
            //m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure2);
            //m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure3);
            //m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure4);
            m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure2);
            //m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure6);

            //添加入量
            m_txtCountInList.Add(txtCountIn1);
            //m_txtCountInList.Add(txtCountIn2);
            //m_txtCountInList.Add(txtCountIn3);
            //m_txtCountInList.Add(txtCountIn4);
            //m_txtCountInList.Add(txtCountIn5);
            //m_txtCountInList.Add(txtCountIn6);

            //添加出量 作为尿量
            m_txtCountOutList.Add(txtCountOut1);
            //m_txtCountOutList.Add(txtCountOut2);
            //m_txtCountOutList.Add(txtCountOut3);
            //m_txtCountOutList.Add(txtCountOut4);
            //m_txtCountOutList.Add(txtCountOut5);
            //m_txtCountOutList.Add(txtCountOut6);

            //添加Others2 此处是出量
            m_txtOther2List.Add(txtOthers2);

            //添加引流量
            m_txtDrainageList.Add(txtDrainage1);
            //m_txtDrainageList.Add(txtDrainage2);
            //m_txtDrainageList.Add(txtDrainage3);
            //m_txtDrainageList.Add(txtDrainage4);
            //m_txtDrainageList.Add(txtDrainage5);
            //m_txtDrainageList.Add(txtDrainage6);

            //添加身高
            m_txtHeightList.Add(txtHeight1);
            //m_txtHeightList.Add(txtHeight2);
            //m_txtHeightList.Add(txtHeight3);
            //m_txtHeightList.Add(txtHeight4);
            //m_txtHeightList.Add(txtHeight5);
            //m_txtHeightList.Add(txtHeight6);

            //添加体重
            m_txtWeightList.Add(txtWeight1);
            //m_txtWeightList.Add(txtWeight2);
            //m_txtWeightList.Add(txtWeight3);
            //m_txtWeightList.Add(txtWeight4);
            //m_txtWeightList.Add(txtWeight5);
            //m_txtWeightList.Add(txtWeight6);

            //添加过敏
            m_txtAllergyList.Add(txtAllergy1);
            //m_txtAllergyList.Add(txtAllergy2);
            //m_txtAllergyList.Add(txtAllergy3);
            //m_txtAllergyList.Add(txtAllergy4);
            //m_txtAllergyList.Add(txtAllergy5);
            //m_txtAllergyList.Add(txtAllergy6);

            //添加心率
            m_txtHeartRateList.Add(txtHeartRate1);
            m_txtHeartRateList.Add(txtHeartRate2);
            m_txtHeartRateList.Add(txtHeartRate3);
            m_txtHeartRateList.Add(txtHeartRate4);
            m_txtHeartRateList.Add(txtHeartRate5);
            m_txtHeartRateList.Add(txtHeartRate6);

            //添加手术后日数
            m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery1);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery2);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery3);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery4);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery5);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery6);

            //添加住院天数
            m_txtDayOfHospitalList.Add(txtDayOfHospital1);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital2);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital3);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital4);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital5);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital6);

            //添加物理降温
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling1);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling2);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling3);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling4);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling5);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling6);

            //添加物理升温 add by ywk 
            m_txtPhysicalHottingList.Add(txtPhysicalHotting1);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting2);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting3);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting4);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting5);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting6);

            //添加疼痛 add by ywk
            //m_txtPainInfoList.Add(txtPainInfo1);
            //m_txtPainInfoList.Add(txtPainInfo2);
            //m_txtPainInfoList.Add(txtPainInfo3);
            //m_txtPainInfoList.Add(txtPainInfo4);
            //m_txtPainInfoList.Add(txtPainInfo5);
            //m_txtPainInfoList.Add(txtPainInfo6);

            //疼痛改为下拉框，里面的值从配置中读取
            //edit by ywk s二〇一二年四月二十五日 15:48:26
            m_lookPainInfoList.Add(lookPainInfo1);
            m_lookPainInfoList.Add(lookPainInfo2);
            m_lookPainInfoList.Add(lookPainInfo3);
            m_lookPainInfoList.Add(lookPainInfo4);
            m_lookPainInfoList.Add(lookPainInfo5);
            m_lookPainInfoList.Add(lookPainInfo6);


            //设置背景是否透明
            m_UcTimeOfShitList[0].IsTransparent = false;
            //m_UcTimeOfShitList[1].IsTransparent = true;
            //m_UcTimeOfShitList[2].IsTransparent = true;
            //m_UcTimeOfShitList[3].IsTransparent = true;
            //m_UcTimeOfShitList[4].IsTransparent = true;
            //m_UcTimeOfShitList[5].IsTransparent = true;

            //设置背景是否透明
            //m_UcTxtSeparateBoxList[1].IsTransparent = true;
            //m_UcTxtSeparateBoxList[2].IsTransparent = true;
            //m_UcTxtSeparateBoxList[4].IsTransparent = true;
            //m_UcTxtSeparateBoxList[5].IsTransparent = true;
        }
        #endregion

        #region 初始化时间段标签
        /// <summary>
        /// 初始化时间段标签
        /// </summary>
        private void InitLabelTime()
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar)
            };
            sqlParam[0].Value = "1";

            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_GetNursingRecordParam", sqlParam, CommandType.StoredProcedure);

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["Configkey"].ToString().Equals("VITALSIGNSRECORDTIME"))
                {
                    m_TimeSlot = dr["Value"].ToString().Split(new char[] { ',', '，' });
                }
                else if (dr["Configkey"].ToString().Equals("MODIFYNURSINGINFO"))
                {
                    m_DayOfModify = Convert.ToInt32(dr["Value"].ToString().Trim());
                }
            }

            if (m_TimeSlot.Length > 0)
            {

                for (int i = 0; i < m_TimeSlot.Length && i < 6; i++)
                {
                    m_lblTimeList[i].Text = m_TimeSlot[i];
                    if (Convert.ToInt32(m_TimeSlot[i]) >= 6 && Convert.ToInt32(m_TimeSlot[i]) < 18)
                    {
                        m_lblTimeList[i].ForeColor = Color.Black;
                    }
                    else
                    {
                        m_lblTimeList[i].ForeColor = Color.Red;
                    }

                }
            }
        }
        #endregion

        #region 初始化体温测量方式
        /// <summary>
        /// 初始化体温测量方式
        /// </summary>
        private void InitlookUpWayOfSurvey()
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = "13";
            sqlParam[1].Value = "0";
            sqlParam[2].Value = "88";

            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);

            foreach (LookUpEdit obj in m_lookUpWayOfSurveyList)
            {
                MethodSet.GetDictionarydetail(obj, dt);
                obj.EditValue = "8801";//默认选择腋温 edit by ywk
                //obj.Text = "腋温";
            }
        }
        #endregion

        public void InitForm()
        {
            //初始化控件数组
            InitControlArray();

            //初始化时间段标签
            InitLabelTime();

            //初始化体温测量方式
            InitlookUpWayOfSurvey();

            InitPainLookInfo();

        }
        /// <summary>
        /// 初始化疼痛指数下拉框
        /// </summary>
        private void InitPainLookInfo()
        {
            lookUpWindowPain.SqlHelper = MethodSet.App.SqlHelper;
            DataTable dtlookPain = new DataTable();
            string sql = string.Format(@"select id,name from categorydetail
                where categoryid='72'");
            dtlookPain = MethodSet.App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            dtlookPain.Columns["ID"].Caption = "代码";
            dtlookPain.Columns["NAME"].Caption = "疼痛指数";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            //cols.Add("ID", 65);
            cols.Add("NAME", 120);
            SqlWordbook painWordBook = new SqlWordbook("querybook", dtlookPain, "ID", "NAME", cols);
            foreach (LookUpEditor item in m_lookPainInfoList)
            {
                item.SqlWordbook = painWordBook;
            }
            //            DataTable dtlookPain = new DataTable();
            //            string sql = string.Format(@"select name from categorydetail
            //                where categoryid='72'");
            //            dtlookPain = MethodSet.App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            //            foreach (LookUpEditor item in m_lookPainInfoList)
            //            {
            //                item.EditValue = "";
            //                //item.Properties.DataSource = dtlookPain;
            //                //item.Properties.DisplayMember = "NAME";
            //                //item.Properties.ValueMember = "ID";

            //            }
        }

        private void UCNursingRecordTable_Load(object sender, EventArgs e)
        {
            //InitForm();
            //foreach (LookUpEdit item in m_lookPainInfoList)
            //{
            //    item.EditValue = "";
            //    item.Properties.DataSource = null;
            //    //item.Properties.DisplayMember = "NAME";
            //    //item.Properties.ValueMember = "ID";

            //}
            //add by ywk 2012年5月21日 11:34:48 
            string times = MethodSet.GetConfigValueByKey("VITALSIGNSRECORDTIME");
            string[] timeArray = times.Split(',');
            if (timeArray.Length >= 6)
            {
                lblTime1.Text = timeArray[0].ToString();
                lblTime2.Text = timeArray[1].ToString();
                lblTime3.Text = timeArray[2].ToString();
                lblTime4.Text = timeArray[3].ToString();
                lblTime5.Text = timeArray[4].ToString();
                lblTime6.Text = timeArray[5].ToString();
            }
        }
        #endregion

        #region 属性集
        /// <summary>
        /// 护理信息允许修改的天数
        /// </summary>
        public int DayOfModify
        {
            get { return m_DayOfModify; }
        }
        #endregion

        #region 当前文本控件重新获取焦点
        /// <summary>
        /// 当前文本控件重新获取焦点
        /// </summary>
        public void ActivateTextEditFocus()
        {
            if (m_ActivateTextEdit != null)
                m_ActivateTextEdit.Focus();

        }

        /// <summary>
        /// 获取有焦点的TextEdit
        /// </summary>
        private void TextBox_Enter(object sender, EventArgs e)
        {
            if (sender.GetType().Name.Equals("TextEdit"))
            {
                m_ActivateTextEdit = sender as TextEdit;
            }
            else
            {
                m_ActivateTextEdit = null;
            }
        }
        #endregion

        #region 保存护理信息数据
        /// <summary>
        /// 保存护理信息数据
        /// edit by ywk 2012年5月29日11:44:21
        /// </summary>
        /// <param name="DateOfSurvey">测量日期</param>
        ///  <param name="m_noofinpat">病人首页序号</param>
        public void SaveUCNursingRecordTable(string DateOfSurvey, string m_noofinpat)
        {
            try
            {
                ////判断是否有数据保存
                //if (CheckNullValue(-1))
                //{
                //    MethodSet.App.CustomMessageBox.MessageShow("没有需要保存的数据");
                //    return;
                //}

                string myNoOfInpat = string.Empty;//首页序号
                if (string.IsNullOrEmpty(m_noofinpat))//没切换患者
                {
                    myNoOfInpat = MethodSet.CurrentInPatient.NoOfFirstPage.ToString();
                }
                else
                {
                    myNoOfInpat = m_noofinpat;
                }
                //检查数据是否有效
                if (!IsDataValid()) return;

                for (int i = 0; i < 6; i++)
                {
                    //判断是否有数据保存
                    if (!CheckNullValue(i))
                    {
                        MethodSet.App.SqlHelper.ExecuteNoneQuery("usp_EditNotesOnNursingInfo", GetSqlParam(i, DateOfSurvey, myNoOfInpat), CommandType.StoredProcedure);
                    }
                }
                //6个一组的一起插入数据库，血压循环两次插入，其他做一次插入（更新）

                //更新血压的值
                if (m_UcTxtSeparateBoxList[0].IsInput)
                {
                    string updatesql = string.Format(@"update notesonnursing set bloodpressure='{0}'
            where dateofsurvey='{1}'and timeslot='" + lblTime1.Text + "' and noofinpat='{2}' ", m_UcTxtSeparateBoxList[0].BP, DateOfSurvey, myNoOfInpat);
                    MethodSet.App.SqlHelper.ExecuteNoneQuery(updatesql, CommandType.Text);
                }
                if (m_UcTxtSeparateBoxList[1].IsInput)
                {
                    string updatesql2 = string.Format(@"update notesonnursing set bloodpressure='{0}'
            where dateofsurvey='{1}' and timeslot='" + lblTime4.Text + "' and noofinpat='{2}' ", m_UcTxtSeparateBoxList[1].BP, DateOfSurvey, myNoOfInpat);

                    MethodSet.App.SqlHelper.ExecuteNoneQuery(updatesql2, CommandType.Text);
                }
                //更新其他栏位的值
                string updateother = string.Format(@"update notesonnursing set timeofshit='{0}',countin='{1}',countout='{2}',
                drainage='{3}',height='{4}',weight='{5}',allergy='{6}',daysaftersurgery='{7}',dayofhospital='{8}',param2='{11}'  
                where dateofsurvey='{9}' and timeslot='" + lblTime1.Text + "' and noofinpat='{10}' ", m_UcTimeOfShitList[0].Shit, txtCountIn1.Text.Trim(),
                txtCountOut1.Text.Trim(), txtDrainage1.Text.Trim(), txtHeight1.Text.Trim(), txtWeight1.Text.Trim(),
                txtAllergy1.Text.Trim(), txtDaysAfterSurgery1.Text.Trim(), txtDayOfHospital1.Text.Trim(), DateOfSurvey,
                myNoOfInpat,txtOthers2.Text.Trim());
                MethodSet.App.SqlHelper.ExecuteNoneQuery(updateother, CommandType.Text);

                MethodSet.App.CustomMessageBox.MessageShow("保存成功!");

            }
            catch (Exception ex)
            {
                MethodSet.App.CustomMessageBox.MessageShow("保存失败!\n详细错误：" + ex.Message);
            }
        }
        #endregion

        #region 检测控件数值是否为空

        /// <summary>
        /// 检测文本框是否为空
        /// </summary>
        /// <param name="TextEditList">文本框集合</param>
        private bool CheckTextEdit(List<TextEdit> TextEditList)
        {
            foreach (TextEdit obj in TextEditList)
            {
                if (!string.IsNullOrEmpty(obj.Text.Trim()))
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 检测疼痛的下拉框是否为空
        /// </summary>
        /// <param name="TextEditList"></param>
        /// <returns></returns>
        private bool CheckLookUpEdit(List<LookUpEditor> LookEditList)
        {
            foreach (LookUpEditor obj in LookEditList)
            {
                if (!string.IsNullOrEmpty(obj.Text.Trim()))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检测控件数值是否为空
        /// </summary>
        /// <param name="index">控件索引，-1为全部</param>
        /// <returns></returns>
        private bool CheckNullValue(int index)
        {
            if (index == -1) //全部控件数值
            {

                if (!CheckTextEdit(m_txtTemperatureList)) return false;
                if (!CheckTextEdit(m_txtPulseList)) return false;
                if (!CheckTextEdit(m_txtBreatheList)) return false;
                if (!CheckTextEdit(m_txtDrainageList)) return false;
                if (!CheckTextEdit(m_txtHeightList)) return false;
                if (!CheckTextEdit(m_txtWeightList)) return false;
                if (!CheckTextEdit(m_txtAllergyList)) return false;
                if (!CheckTextEdit(m_txtHeartRateList)) return false;
                if (!CheckTextEdit(m_txtDaysAfterSurgeryList)) return false;
                if (!CheckTextEdit(m_txtDayOfHospitalList)) return false;
                if (!CheckTextEdit(m_txtPhysicalCoolingList)) return false;
                if (!CheckTextEdit(m_txtPhysicalHottingList)) return false;//新增疼痛和升温
                //if (!CheckTextEdit(m_txtPainInfoList)) return false;
                //疼痛改为下拉框控件，数据从配置中读取
                if (!CheckLookUpEdit(m_lookPainInfoList)) return false;

                foreach (UCTextGroupBox obj in m_UcTimeOfShitList)
                {
                    if (obj.IsInput)
                    {
                        return false;
                    }
                }

                foreach (UCTextSeparateBox obj in m_UcTxtSeparateBoxList)
                {
                    if (obj.IsInput)
                    {
                        return false;
                    }
                }
            }
            else//指定index索引的控件数值
            {
                if (!(string.IsNullOrEmpty(m_txtTemperatureList[index].Text) &&
                  string.IsNullOrEmpty(m_txtPulseList[index].Text) &&
                  string.IsNullOrEmpty(m_txtBreatheList[index].Text) &&
                  !m_UcTimeOfShitList[0].IsInput &&
                  !m_UcTxtSeparateBoxList[0].IsInput &&
                  !m_UcTxtSeparateBoxList[1].IsInput &&
                  string.IsNullOrEmpty(m_txtDrainageList[0].Text) &&
                  string.IsNullOrEmpty(m_txtHeightList[0].Text) &&
                  string.IsNullOrEmpty(m_txtWeightList[0].Text) &&
                  string.IsNullOrEmpty(m_txtAllergyList[0].Text) &&
                  string.IsNullOrEmpty(m_txtHeartRateList[index].Text) &&
                  string.IsNullOrEmpty(m_txtDaysAfterSurgeryList[0].Text) &&
                  string.IsNullOrEmpty(m_txtDayOfHospitalList[0].Text) &&
                  string.IsNullOrEmpty(m_txtPhysicalCoolingList[index].Text) &&
                   string.IsNullOrEmpty(m_txtPhysicalHottingList[index].Text) &&
                    //string.IsNullOrEmpty(m_txtPainInfoList[index].Text)
                     string.IsNullOrEmpty(m_lookPainInfoList[index].Text)//疼痛栏位
                  ))
                {
                    return false;
                }

            }

            return true;
        }
        #endregion

        #region 检查数据是否有效
        /// <summary>
        /// 检查数据是否有效
        /// </summary>
        /// <returns></returns>
        private bool IsDataValid()
        {
            int intVart;

            for (int i = 0; i < 6; i++)
            {
                string strTimeSlot = "时间段为【" + m_lblTimeList[i].Text.Trim() + "】的记录：\n";

                //体温
                if (m_txtTemperatureList[i].Text.Trim().ToString() != "")
                {
                    if (!Dataprocessing.IsNumber(m_txtTemperatureList[i].Text.Trim().ToString(), 1))
                    {
                        MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "体温必须为数字，小数点保留一位!");
                        return false;
                    }
                    else
                    {
                        double dVar = Convert.ToDouble(m_txtTemperatureList[i].Text.Trim());
                        if (!(dVar >= 34 && dVar <= 43))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "体温超出范围：34℃至43℃!");
                            return false;
                        }
                    }
                }

                //脉搏
                if (m_txtPulseList[i].Text.Trim().ToString() != "")
                {
                    if (!Dataprocessing.IsNumber(m_txtPulseList[i].Text.Trim().ToString(), 0))
                    {
                        MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "脉搏必须为不带小数的数字!");
                        return false;
                    }
                    else
                    {
                        intVart = Convert.ToInt32(m_txtPulseList[i].Text.Trim().ToString());
                        if (!(intVart >= 20 && intVart <= 160))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "脉搏超出范围：20次/分至160/次/分!");
                            return false;
                        }
                    }
                }

                //心率
                if (m_txtHeartRateList[i].Text.Trim().ToString() != "")
                {
                    if (!Dataprocessing.IsNumber(m_txtHeartRateList[i].Text.Trim().ToString(), 0))
                    {
                        MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "心率必须为不带小数的数字!");
                        return false;
                    }
                    else
                    {
                        intVart = Convert.ToInt32(m_txtHeartRateList[i].Text.Trim().ToString());
                        if (!(intVart >= 20 && intVart <= 200))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "心率超出范围：20次/分至200/次/分!");
                            return false;
                        }
                    }
                }

                //呼吸
                if (m_txtBreatheList[i].Text.Trim().ToString() != "")
                {
                    if (!Dataprocessing.IsNumber(m_txtBreatheList[i].Text.Trim().ToString(), 0))
                    {
                        MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "呼吸必须为不带小数的数字!");
                        return false;
                    }
                    else
                    {
                        intVart = Convert.ToInt32(m_txtBreatheList[i].Text.Trim().ToString());
                        if (!(intVart >= 0 && intVart <= 90))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "呼吸超出范围：0次/分至90/次/分!");
                            return false;
                        }
                    }
                }

                ////血压
                //if (!m_UcTxtSeparateBoxList[i].CheckData())
                //{
                //    return false;
                //}

                //物理降温的限制验证 
                //add by ywk 2012年4月17日16:49:30
                if (m_txtPhysicalCoolingList[i].Text.Trim().ToString() != "")
                {
                    if (!Dataprocessing.IsNumber(m_txtPhysicalCoolingList[i].Text.Trim().ToString(), 1))
                    {
                        MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "物理降温必须为数字，小数点保留一位!");
                        return false;
                    }
                    else
                    {
                        double dVar = Convert.ToDouble(m_txtPhysicalCoolingList[i].Text.Trim());
                        if (!(dVar >= 34 && dVar <= 43))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "物理降温超出范围：34℃至43℃!");
                            return false;
                        }
                    }
                }

                //物理降温不能大于当前时间点的测试温度 edit by ywk 
                //体温为空，能填写物理降温？此处先限制住
                if (string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) && !string.IsNullOrEmpty(m_txtPhysicalCoolingList[i].Text.Trim()))
                {
                    MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "请先输入此时的体温");
                    return false;
                }
                //string temperature = m_txtTemperatureList[i].Text.Trim();
                if (Convert.ToDouble(string.IsNullOrEmpty(m_txtPhysicalCoolingList[i].Text.Trim()) ? "0" : m_txtPhysicalCoolingList[i].Text.Trim()) > Convert.ToDouble(string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) ? "0" : m_txtTemperatureList[i].Text.Trim()))
                {
                    MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "物理降温不能超出此时的体温");
                    return false;
                }

                //物理升温的限制验证 
                //add by ywk 2012年4月17日16:49:30
                if (m_txtPhysicalHottingList[i].Text.Trim().ToString() != "")
                {
                    if (!Dataprocessing.IsNumber(m_txtPhysicalHottingList[i].Text.Trim().ToString(), 1))
                    {
                        MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "物理升温必须为数字，小数点保留一位!");
                        return false;
                    }
                    else
                    {
                        double dVar = Convert.ToDouble(m_txtPhysicalHottingList[i].Text.Trim());
                        if (!(dVar >= 34 && dVar <= 43))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "物理升温超出范围：34℃至43℃!");
                            return false;
                        }
                    }
                }

                //物理升温不能小于于当前时间点的测试温度 edit by ywk 
                //体温为空，能填写物理升温？此处先限制住
                if (string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) && !string.IsNullOrEmpty(m_txtPhysicalHottingList[i].Text.Trim()))
                {
                    MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "请先输入此时的体温");
                    return false;
                }
                //可填写体温不填物理升温 
                //if (!string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) && string.IsNullOrEmpty(m_txtPhysicalHottingList[i].Text.Trim()))
                //{
                //    return true;
                //}
                //string temperature = m_txtTemperatureList[i].Text.Trim();
                //限制先取消 
                //if (Convert.ToDouble(string.IsNullOrEmpty(m_txtPhysicalHottingList[i].Text.Trim()) ? "0" : m_txtPhysicalHottingList[i].Text.Trim()) < Convert.ToDouble(string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) ? "0" : m_txtTemperatureList[i].Text.Trim()))
                //{
                //    MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "物理升温不能低于此时的体温");
                //    return false;
                //}
                //限制疼痛栏位只能输入数字 add by ywk 
                //if (m_txtPainInfoList[i].Text.Trim().ToString() != "")
                //{
                //    if (!Dataprocessing.IsNumber(m_txtPainInfoList[i].Text.Trim().ToString(), 1))
                //    {
                //        MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "疼痛指数必须为数字!");
                //        return false;
                //    }
                //else
                //{
                //    return false;
                //}
                //}
            }

            //血压
            for (int j = 0; j < 2; j++)
            {
                if (!m_UcTxtSeparateBoxList[j].CheckData())
                {
                    return false;
                }
            }


            return true;
        }
        #endregion

        #region 获取存储数据
        /// <summary>
        /// 获取存储数据
        /// </summary>
        /// <param name="index">控件数组索引</param>
        /// <param name="DateOfSurvey">测量日期</param>
        /// 新增noofinpat（首页序号）ywk 
        /// <returns></returns>
        private SqlParameter[] GetSqlParam(int index, string DateOfSurvey, string noofinpat)
        {
            SqlParameter[] sqlParam = new SqlParameter[]
            {
                new SqlParameter("@NoOfInpat",SqlDbType.VarChar),//病案号
                new SqlParameter("@DateOfSurvey",SqlDbType.VarChar),//测量日期
                new SqlParameter("@Temperature",SqlDbType.VarChar),//体温
                new SqlParameter("@WayOfSurvey",SqlDbType.Int),//体温类别

                new SqlParameter("@Pulse",SqlDbType.VarChar),//脉搏
                new SqlParameter("@HeartRate",SqlDbType.VarChar),//心率
                new SqlParameter("@Breathe",SqlDbType.VarChar),//呼吸
                //new SqlParameter("@BloodPressure",SqlDbType.VarChar),//血压

                //new SqlParameter("@TimeOfShit",SqlDbType.VarChar),//大便次数 
                //new SqlParameter("@CountIn",SqlDbType.VarChar),//总入量
                //new SqlParameter("@CountOut",SqlDbType.VarChar),//总出量
                //new SqlParameter("@Drainage",SqlDbType.VarChar),//引流量

                //new SqlParameter("@Height",SqlDbType.VarChar),// 身高
                //new SqlParameter("@Weight",SqlDbType.VarChar),//体重
                //new SqlParameter("@Allergy",SqlDbType.VarChar),//过敏药物
                //new SqlParameter("@DaysAfterSurgery",SqlDbType.VarChar),//手术后天数

                //new SqlParameter("@DayOfHospital",SqlDbType.VarChar),//住院天数 
                new SqlParameter("@PhysicalCooling",SqlDbType.VarChar),//物理降温
                new SqlParameter("@DoctorOfRecord",SqlDbType.VarChar),//记录人
                new SqlParameter("@TimeSlot",SqlDbType.VarChar),//时间点
                 //新增物理升温和疼痛两个字段 
                new SqlParameter("@PhysicalHotting",SqlDbType.VarChar),//温度变化
                new SqlParameter("@PainInfo",SqlDbType.VarChar)//疼痛

            };

            sqlParam[0].Value = noofinpat;// MethodSet.CurrentInPatient.NoOfFirstPage;
            sqlParam[1].Value = DateOfSurvey;
            sqlParam[2].Value = m_txtTemperatureList[index].Text.Trim();
            sqlParam[3].Value = int.Parse(m_lookUpWayOfSurveyList[index].EditValue.ToString());

            sqlParam[4].Value = m_txtPulseList[index].Text.Trim();
            sqlParam[5].Value = m_txtHeartRateList[index].Text.Trim();
            sqlParam[6].Value = m_txtBreatheList[index].Text.Trim();
            //sqlParam[7].Value = m_UcTxtSeparateBoxList[index].BP;

            //sqlParam[8].Value = m_UcTimeOfShitList[index].Shit;
            //sqlParam[9].Value = m_txtCountInList[index].Text.Trim();
            //sqlParam[10].Value = m_txtCountOutList[index].Text.Trim();
            //sqlParam[11].Value = m_txtDrainageList[index].Text.Trim();

            //sqlParam[12].Value = m_txtHeightList[index].Text.Trim();
            //sqlParam[13].Value = m_txtWeightList[index].Text.Trim();
            //sqlParam[7].Value = m_txtAllergyList[index].Text.Trim();
            //sqlParam[15].Value = m_txtDaysAfterSurgeryList[index].Text.Trim();

            //sqlParam[16].Value = m_txtDayOfHospitalList[index].Text.Trim();
            sqlParam[7].Value = m_txtPhysicalCoolingList[index].Text.Trim();
            sqlParam[8].Value = MethodSet.App.User.Id;
            sqlParam[9].Value = m_lblTimeList[index].Text;

            sqlParam[10].Value = m_txtPhysicalHottingList[index].Text.Trim();
            sqlParam[11].Value = m_lookPainInfoList[index].Text.ToString().Trim();

            return sqlParam;
        }
        #endregion

        #region 获取指定日期对应的护理信息数据
        /// <summary>
        /// 获取指定日期对应的护理信息数据
        /// edit by ywk 2012年5月29日11:17:24
        /// </summary>
        /// <param name="DateOfSurvey"></param>
        ///  <param name="m_oofinpat">新增病人首页序号字段</param>
        public void GetNotesOfNursingInfo(string DateOfSurvey, string m_oofinpat)
        {
            //清除控件内容
            ClearControlValue();

            //获取指定日期数据
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@DateOfSurvey", SqlDbType.VarChar)
            };
            string noofinpat = string.Empty;//病人首页序号
            if (string.IsNullOrEmpty(m_oofinpat))//如果为空，表示没切换患者
            {
                noofinpat = MethodSet.CurrentInPatient.NoOfFirstPage.ToString();
            }
            else
            {
                noofinpat = m_oofinpat;
            }
            //sqlParam[0].Value = MethodSet.CurrentInPatient.NoOfFirstPage;
            sqlParam[0].Value = noofinpat;
            sqlParam[1].Value = DateOfSurvey;

            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_GetNotesOnNursingInfo", sqlParam, CommandType.StoredProcedure);

            //控件赋值
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int index = GetIndex(dr["TimeSlot"].ToString());
                    if (index > -1)
                    {
                        m_txtTemperatureList[index].Text = dr["Temperature"].ToString();
                        m_lookUpWayOfSurveyList[index].EditValue = dr["WayOfSurvey"].ToString();

                        m_txtPulseList[index].Text = dr["Pulse"].ToString();
                        m_txtHeartRateList[index].Text = dr["HeartRate"].ToString();
                        m_txtBreatheList[index].Text = dr["Breathe"].ToString();
                        //m_UcTxtSeparateBoxList[index].BP = dr["BloodPressure"].ToString();

                        //m_UcTimeOfShitList[index].Shit = dr["TimeOfShit"].ToString();
                        //m_txtCountInList[index].Text = dr["CountIn"].ToString();
                        //m_txtCountOutList[index].Text = dr["CountOut"].ToString();
                        //m_txtDrainageList[index].Text = dr["Drainage"].ToString();//引流量

                        //m_txtHeightList[index].Text = dr["Height"].ToString();
                        //m_txtWeightList[index].Text = dr["Weight"].ToString();
                        //m_txtAllergyList[index].Text = dr["Allergy"].ToString();
                        //m_txtDaysAfterSurgeryList[index].Text = dr["DaysAfterSurgery"].ToString();

                        //m_txtDayOfHospitalList[index].Text = dr["DayOfHospital"].ToString();
                        m_txtPhysicalCoolingList[index].Text = dr["PhysicalCooling"].ToString();

                        //新增物理升温和疼痛两个栏位 edit by ywk  
                        m_txtPhysicalHottingList[index].Text = dr["PhysicalHotting"].ToString();
                        m_lookPainInfoList[index].Text = dr["PainInfo"].ToString();
                    }
                }
                //读取数据绑定到界面。绑定血压的值
                DataRow[] dr1 = dt.Select("timeslot='" + lblTime1.Text + "'");
                if (dr1.Length > 0)
                {
                    m_UcTxtSeparateBoxList[0].BP = dr1[0]["BloodPressure"].ToString();

                    m_UcTimeOfShitList[0].Shit = dr1[0]["TimeOfShit"].ToString();
                    m_txtCountInList[0].Text = dr1[0]["CountIn"].ToString();
                    m_txtCountOutList[0].Text = dr1[0]["CountOut"].ToString();
                    m_txtOther2List[0].Text = dr1[0]["param2"].ToString();
                    m_txtDrainageList[0].Text = dr1[0]["Drainage"].ToString();//引流量
                    m_txtHeightList[0].Text = dr1[0]["Height"].ToString();
                    m_txtWeightList[0].Text = dr1[0]["Weight"].ToString();
                    m_txtAllergyList[0].Text = dr1[0]["Allergy"].ToString();
                    m_txtDaysAfterSurgeryList[0].Text = dr1[0]["DaysAfterSurgery"].ToString();
                    m_txtDayOfHospitalList[0].Text = dr1[0]["DayOfHospital"].ToString();
                }
                DataRow[] dr2 = dt.Select("timeslot='" + lblTime4.Text + "'");
                if (dr2.Length > 0)
                {
                    m_UcTxtSeparateBoxList[1].BP = dr2[0]["BloodPressure"].ToString();
                }
            }
            else
            {
                foreach (LookUpEditor item in m_lookPainInfoList)
                {
                    item.EditValue = "";
                    item.SelectedText = "";
                }
                //只有录入里美数据，就给腋温值edit by ywk 
                for (int i = 0; i < 6; i++)
                {
                    m_lookUpWayOfSurveyList[i].EditValue = "8801";
                }

            }

            //住院天数
            if (m_txtDayOfHospitalList[0].Text.Trim() == ""
                && Convert.ToDateTime(DateOfSurvey) == Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")))
            {
                DateTime AdmitDate = Convert.ToDateTime(Convert.ToDateTime(MethodSet.AdmitDate).Date.ToString("yyyy-MM-dd 00:00:01"));
                DateTime CurrDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:01"));

                m_txtDayOfHospitalList[0].Text = (CurrDate.Subtract(AdmitDate).Days + 1).ToString();
            }

            //手术后日数
            if (m_txtDaysAfterSurgeryList[0].Text.Trim() == ""
                && Convert.ToDateTime(DateOfSurvey) == Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")))
            {
                //SetDaysAfterSurgery(MethodSet.DaysAfterSurgery);
            }

        }
        #endregion

        #region 获取时间段对应的下标索引
        /// <summary>
        /// 获取时间段对应的下标索引
        /// </summary>
        /// <param name="TimeSlot">时间段值</param>
        /// <returns></returns>
        private int GetIndex(string TimeSlot)
        {
            for (int i = 0; i < m_TimeSlot.Length; i++)
            {
                if (m_TimeSlot[i].Equals(TimeSlot))
                    return i;
            }

            return -1;
        }
        #endregion

        #region 清除控件内容
        /// <summary>
        /// 清除控件内容
        /// </summary>
        private void ClearControlValue()
        {
            for (int i = 0; i < 6; i++)
            {
                m_txtTemperatureList[i].Text = "";
                m_lookUpWayOfSurveyList[i].EditValue = "8801";

                m_txtPulseList[i].Text = "";
                m_txtHeartRateList[i].Text = "";
                m_txtBreatheList[i].Text = "";
                //m_UcTxtSeparateBoxList[i].BP = "";


                //m_txtCountInList[i].Text = "";
                //m_txtCountOutList[i].Text = "";
                //m_txtDrainageList[i].Text = "";

                //m_txtHeightList[i].Text = "";
                //m_txtWeightList[i].Text = "";
                //m_txtAllergyList[i].Text = "";
                //m_txtDaysAfterSurgeryList[i].Text = "";

                //m_txtDayOfHospitalList[i].Text = "";
                m_txtPhysicalCoolingList[i].Text = "";
                //新增疼痛和升温
                m_txtPhysicalHottingList[i].Text = "";
                //m_txtPainInfoList[i].Text = "";
                m_lookPainInfoList[i].Text = "";
            }
            m_UcTxtSeparateBoxList[0].BP = "";//血压
            m_UcTxtSeparateBoxList[1].BP = "";//血压
            m_UcTimeOfShitList[0].DateTextReset();//大便次数
            m_txtCountInList[0].Text = "";//总入量
            m_txtCountOutList[0].Text = "";//总出量 作为尿量
            m_txtOther2List[0].Text = "";//出量
            m_txtDrainageList[0].Text = "";//引流量
            m_txtHeightList[0].Text = "";//身高
            m_txtWeightList[0].Text = "";//体重
            m_txtAllergyList[0].Text = "";//过敏
            m_txtDaysAfterSurgeryList[0].Text = "";//手术后天数
            m_txtDayOfHospitalList[0].Text = "";//住院天数


        }
        #endregion

        #region 设置手术后天数
        /// <summary>
        /// 设置手术后天数
        /// </summary>
        /// <param name="DaysAfterSurgery">手术后天数数组</param>
        public void SetDaysAfterSurgery(string[] DaysAfterSurgery)
        {
            if (m_txtDaysAfterSurgeryList[0].Text.Trim() == "" && DaysAfterSurgery != null)
            {
                string strDaysAfterSurgery = "";

                DateTime dateTime = DateTime.Now.Date;

                for (int i = 0; i < DaysAfterSurgery.Length; i++)
                {
                    int days = dateTime.Subtract(Convert.ToDateTime(DaysAfterSurgery[i]).Date).Days;
                    if (days <= 14)
                    {
                        strDaysAfterSurgery = days.ToString() + "/" + strDaysAfterSurgery;
                    }
                }

                if (!string.IsNullOrEmpty(strDaysAfterSurgery))
                    strDaysAfterSurgery = strDaysAfterSurgery.Substring(0, strDaysAfterSurgery.Length - 1);
                m_txtDaysAfterSurgeryList[0].Text = strDaysAfterSurgery;
            }
        }
        #endregion



        /// <summary>
        /// 再加此方法，清掉疼痛指数下拉框的值
        /// </summary>
        /// <param name="DateOfSurvey"></param>
        public void OperateLookUp(string DateOfSurvey)
        {
            //清除控件内容
            ClearControlValue();

            //获取指定日期数据
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@DateOfSurvey", SqlDbType.VarChar)
            };
            sqlParam[0].Value = MethodSet.CurrentInPatient.NoOfFirstPage;
            sqlParam[1].Value = DateOfSurvey;

            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_GetNotesOnNursingInfo", sqlParam, CommandType.StoredProcedure);

            //控件赋值
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int index = GetIndex(dr["TimeSlot"].ToString());
                    if (index > -1)
                    {
                        m_txtTemperatureList[index].Text = dr["Temperature"].ToString();
                        m_lookUpWayOfSurveyList[index].EditValue = dr["WayOfSurvey"].ToString();

                        m_txtPulseList[index].Text = dr["Pulse"].ToString();
                        m_txtHeartRateList[index].Text = dr["HeartRate"].ToString();
                        m_txtBreatheList[index].Text = dr["Breathe"].ToString();
                        m_UcTxtSeparateBoxList[index].BP = dr["BloodPressure"].ToString();

                        m_UcTimeOfShitList[index].Shit = dr["TimeOfShit"].ToString();
                        m_txtCountInList[index].Text = dr["CountIn"].ToString();
                        m_txtCountOutList[index].Text = dr["CountOut"].ToString();
                        m_txtOther2List[index].Text = dr["Param2"].ToString();
                        m_txtDrainageList[index].Text = dr["Drainage"].ToString();

                        m_txtHeightList[index].Text = dr["Height"].ToString();
                        m_txtWeightList[index].Text = dr["Weight"].ToString();
                        m_txtAllergyList[index].Text = dr["Allergy"].ToString();
                        m_txtDaysAfterSurgeryList[index].Text = dr["DaysAfterSurgery"].ToString();

                        m_txtDayOfHospitalList[index].Text = dr["DayOfHospital"].ToString();
                        m_txtPhysicalCoolingList[index].Text = dr["PhysicalCooling"].ToString();

                        //新增物理升温和疼痛两个栏位 edit by ywk  
                        m_txtPhysicalHottingList[index].Text = dr["PhysicalHotting"].ToString();
                        //m_txtPainInfoList[index].Text = dr["PainInfo"].ToString();
                        m_lookPainInfoList[index].Text = dr["PainInfo"].ToString();
                    }
                }
            }
            else
            {
                foreach (LookUpEditor item in m_lookPainInfoList)
                {
                    item.EditValue = "";
                    item.SelectedText = "";
                }

            }
        }

        //internal void SetDaysAfterSurgery(string inputdate)
        //{
        //    throw new NotImplementedException();
        //}
        /// <summary>
        /// /根据选择的日期，改变住院天数的值
        /// edit by ywk 二〇一二年五月十八日 10:16:36
        /// </summary>
        /// <param name="inputdate"></param>
        internal void SetDayInHospital(string inputdate)
        {
            DateTime AdmitDate = Convert.ToDateTime(Convert.ToDateTime(MethodSet.AdmitDate).Date.ToString("yyyy-MM-dd 00:00:01"));
            DateTime CurrDate = Convert.ToDateTime(inputdate + "  00:00:01");

            m_txtDayOfHospitalList[0].Text = (CurrDate.Subtract(AdmitDate).Days + 1).ToString();
        }

        #region 对于手术后天数的处理 add  by ywk
        /// <summary>
        /// 设置手术后天数 edit by ywk
        /// </summary>
        internal void SetDaysAfterSurgery(string inputdate, string patid)
        {
            //DateOfSurvey 当前录入的日期时间 
            string serachsql = string.Format(@"    select * from patientstatus where noofinpat='{0}' and trunc(to_date(patientstatus.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 and dotime<'{1}'  and ccode='7000' order by dotime asc", patid, inputdate + " 23:59:59");
            //string serachsql = string.Format(@"    select * from patientstatus where noofinpat='{0}' and trunc(to_date(patientstatus.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 and dotime<'{1}'  and ccode='7000' order by dotime asc", MethodSet.CurrentInPatient.Code, inputdate + " 23:59:59");
            DataTable AllSurgeryData = MethodSet.App.SqlHelper.ExecuteDataTable(serachsql, CommandType.Text);

            if (AllSurgeryData.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(inputdate))
                {
                    DateTime nowInputDate = DateTime.Parse(inputdate + " 23:59:59");

                    if (AllSurgeryData.Rows.Count > 0)//有一次以上的手术 
                    {
                        string content = ReturnCotent(nowInputDate, AllSurgeryData);
                        m_txtDaysAfterSurgeryList[0].Text = content;
                    }

                }
            }
        }

        /// <summary>
        /// 对于一次以上的手术返回要显示的内容
        /// ywk
        /// </summary>
        /// <param name="count"></param>
        /// <param name="nowinputtime"></param>
        /// <returns></returns>
        private string ReturnCotent(DateTime nowinputtime, DataTable dtdata)
        {
            string mycontent = string.Empty;
            string outcontent = string.Empty;
            for (int i = 0; i < dtdata.Rows.Count; i++)
            {
                mycontent = (nowinputtime - DateTime.Parse(dtdata.Rows[i]["dotime"].ToString())).Days.ToString();
                if (mycontent == "0")
                {
                    outcontent += "/" + ReturnCount((i + 1).ToString());
                }
                else
                {
                    outcontent += "/" + mycontent;
                }

            }
            return outcontent.Trim('/');
        }
        /// <summary>
        /// 处理手术后第几次的显示内容
        /// </summary>
        /// <param name="showcount"></param>
        /// <returns></returns>
        private string ReturnCount(string showcount)
        {
            string show = string.Empty;
            switch (showcount)
            {
                case "1":
                    show = "";
                    break;
                case "2":
                    show = "II";
                    break;
                case "3":
                    show = "III";
                    break;
                case "4":
                    show = "IV";
                    break;
                case "5":
                    show = "V";
                    break;
                case "6":
                    show = "VI";
                    break;
                case "7":
                    show = "VII";
                    break;
                case "8":
                    show = "VIII";
                    break;
                case "9":
                    show = "IX";
                    break;
                case "10":
                    show = "X";
                    break;


                default:
                    break;
            }
            return show;
        }

        #endregion


        #region 处理每个文本框的Enter事件

        /// <summary>
        /// 获得下一个控件的名称Name
        /// </summary>
        /// <param name="currentControlName"></param>
        /// <returns></returns>
        public string GetNextControlName(string currentControlName)
        {
            string nextControlName = string.Empty;
            if (currentControlName.StartsWith("txtTemperature"))
            {
                nextControlName = "txtPulse" + currentControlName.Substring("txtTemperature".Length);
            }
            else if (currentControlName.StartsWith("txtPulse"))
            {
                nextControlName = "txtBreathe" + currentControlName.Substring("txtPulse".Length);
            }
            else if (currentControlName.StartsWith("txtBreathe"))
            {
                nextControlName = "txtHeartRate" + currentControlName.Substring("txtBreathe".Length);
            }
            else if (currentControlName.StartsWith("txtHeartRate"))
            {
                nextControlName = "txtPhysicalCooling" + currentControlName.Substring("txtHeartRate".Length);
            }
            else if (currentControlName.StartsWith("txtPhysicalCooling"))
            {
                nextControlName = "txtPhysicalHotting" + currentControlName.Substring("txtPhysicalCooling".Length);
            }
            //else if (currentControlName.StartsWith("txtPhysicalHotting"))
            //{
            //    nextControlName = "lookPainInfo" + currentControlName.Substring("txtPhysicalHotting".Length);
            //}
            else if (currentControlName.StartsWith("txtPhysicalHotting"))
            {
                nextControlName = "txtTemperature" + (Convert.ToInt32(currentControlName.Substring("txtPhysicalHotting".Length)) + 1);
            }
            return nextControlName;
        }

        private TextEdit GetNextTextEdit(string nextControlName)
        {
            foreach (Control control in this.Controls)
            {
                if (control.Name == nextControlName)
                {
                    return (TextEdit)control;
                }
            }
            return null;
        }
        /// <summary>
        /// 供文本框调用的Enter事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEdit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)13)
            {
                string name = GetNextControlName(((Control)sender).Name);
                TextEdit textEdit = GetNextTextEdit(name);
                if (textEdit != null)
                {
                    textEdit.Focus();
                }
            }
        }

        #endregion
    }
}
