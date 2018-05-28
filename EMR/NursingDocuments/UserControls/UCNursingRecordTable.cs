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
using System.Xml;
using DrectSoft.Common;
using DrectSoft.Common.Eop;
using DrectSoft.Service;

namespace DrectSoft.Core.NursingDocuments.UserControls
{
    public partial class UCNursingRecordTable : DevExpress.XtraEditors.XtraUserControl
    {
        #region �����ֶ�
        //�ؼ���
        List<LabelControl> m_lblTimeList = new List<LabelControl>();//ʱ���
        List<LookUpEdit> m_lookUpWayOfSurveyList = new List<LookUpEdit>();//���²�����λ
        List<TextEdit> m_txtTemperatureList = new List<TextEdit>();//����
        List<TextEdit> m_txtPulseList = new List<TextEdit>();//����
        List<TextEdit> m_txtBreatheList = new List<TextEdit>();//����
        List<UCTextGroupBox> m_UcTimeOfShitList = new List<UCTextGroupBox>();//������
        List<UCTextSeparateBox> m_UcTxtSeparateBoxList = new List<UCTextSeparateBox>();//Ѫѹ
        List<TextEdit> m_txtCountInList = new List<TextEdit>();//����
        List<TextEdit> m_txtCountOutList = new List<TextEdit>();//����
        List<TextEdit> m_txtDrainageList = new List<TextEdit>();//������
        List<TextEdit> m_txtHeightList = new List<TextEdit>();//����
        List<TextEdit> m_txtWeightList = new List<TextEdit>();//����
        List<TextEdit> m_txtAllergyList = new List<TextEdit>();//����
        List<TextEdit> m_txtHeartRateList = new List<TextEdit>();//����
        List<TextEdit> m_txtDaysAfterSurgeryList = new List<TextEdit>();//����������
        List<TextEdit> m_txtDayOfHospitalList = new List<TextEdit>();//סԺ����
        List<TextEdit> m_txtPhysicalCoolingList = new List<TextEdit>();//��������
        //�������������º���ʹ add by ywk
        List<TextEdit> m_txtPhysicalHottingList = new List<TextEdit>();//��������
        //List<TextEdit> m_txtPainInfoList = new List<TextEdit>();//��ʹ
        //��ʹ��Ϊ���������ݴ������ж�ȡ��
        List<LookUpEditor> m_lookPainInfoList = new List<LookUpEditor>();//��ʹָ��

        //ʱ�������
        string[] m_TimeSlot = null;



        //��ȡ������ı��ؼ�
        TextEdit m_ActivateTextEdit = new TextEdit();

        //������Ϣ�����޸ĵ�����
        int m_DayOfModify = 0;

        public Inpatient currInpatient;
        #endregion

        #region ��ʼ������

        public UCNursingRecordTable()
        {
            InitializeComponent();
        }


        #region ��ʼ���ؼ�����
        /// <summary>
        /// ��ʼ���ؼ�����
        /// </summary>
        private void InitControlArray()
        {
            //����ʱ���
            m_lblTimeList.Add(lblTime1);
            m_lblTimeList.Add(lblTime2);
            m_lblTimeList.Add(lblTime3);
            m_lblTimeList.Add(lblTime4);
            m_lblTimeList.Add(lblTime5);
            m_lblTimeList.Add(lblTime6);

            //�������²�����λ
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

            //��������
            m_txtTemperatureList.Add(txtTemperature1);
            m_txtTemperatureList.Add(txtTemperature2);
            m_txtTemperatureList.Add(txtTemperature3);
            m_txtTemperatureList.Add(txtTemperature4);
            m_txtTemperatureList.Add(txtTemperature5);
            m_txtTemperatureList.Add(txtTemperature6);

            //��������
            m_txtPulseList.Add(txtPulse1);
            m_txtPulseList.Add(txtPulse2);
            m_txtPulseList.Add(txtPulse3);
            m_txtPulseList.Add(txtPulse4);
            m_txtPulseList.Add(txtPulse5);
            m_txtPulseList.Add(txtPulse6);

            //���Ӻ���
            m_txtBreatheList.Add(txtBreathe1);
            m_txtBreatheList.Add(txtBreathe2);
            m_txtBreatheList.Add(txtBreathe3);
            m_txtBreatheList.Add(txtBreathe4);
            m_txtBreatheList.Add(txtBreathe5);
            m_txtBreatheList.Add(txtBreathe6);

            //���Ӵ�����
            m_UcTimeOfShitList.Add(ucTxtTimeOfShit1);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit2);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit3);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit4);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit5);
            //m_UcTimeOfShitList.Add(ucTxtTimeOfShit6);

            //����Ѫѹ
            m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure1);
            //m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure2);
            //m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure3);
            //m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure4);
            m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure5);
            //m_UcTxtSeparateBoxList.Add(ucTxtBloodPressure6);

            //��������
            m_txtCountInList.Add(txtCountIn1);
            //m_txtCountInList.Add(txtCountIn2);
            //m_txtCountInList.Add(txtCountIn3);
            //m_txtCountInList.Add(txtCountIn4);
            //m_txtCountInList.Add(txtCountIn5);
            //m_txtCountInList.Add(txtCountIn6);

            //���ӳ���
            m_txtCountOutList.Add(txtCountOut1);
            //m_txtCountOutList.Add(txtCountOut2);
            //m_txtCountOutList.Add(txtCountOut3);
            //m_txtCountOutList.Add(txtCountOut4);
            //m_txtCountOutList.Add(txtCountOut5);
            //m_txtCountOutList.Add(txtCountOut6);

            //����������
            m_txtDrainageList.Add(txtDrainage1);
            //m_txtDrainageList.Add(txtDrainage2);
            //m_txtDrainageList.Add(txtDrainage3);
            //m_txtDrainageList.Add(txtDrainage4);
            //m_txtDrainageList.Add(txtDrainage5);
            //m_txtDrainageList.Add(txtDrainage6);

            //��������
            m_txtHeightList.Add(txtHeight1);
            //m_txtHeightList.Add(txtHeight2);
            //m_txtHeightList.Add(txtHeight3);
            //m_txtHeightList.Add(txtHeight4);
            //m_txtHeightList.Add(txtHeight5);
            //m_txtHeightList.Add(txtHeight6);

            //��������
            m_txtWeightList.Add(txtWeight1);
            //m_txtWeightList.Add(txtWeight2);
            //m_txtWeightList.Add(txtWeight3);
            //m_txtWeightList.Add(txtWeight4);
            //m_txtWeightList.Add(txtWeight5);
            //m_txtWeightList.Add(txtWeight6);

            //���ӹ���
            m_txtAllergyList.Add(txtAllergy1);
            //m_txtAllergyList.Add(txtAllergy2);
            //m_txtAllergyList.Add(txtAllergy3);
            //m_txtAllergyList.Add(txtAllergy4);
            //m_txtAllergyList.Add(txtAllergy5);
            //m_txtAllergyList.Add(txtAllergy6);

            //��������
            m_txtHeartRateList.Add(txtHeartRate1);
            m_txtHeartRateList.Add(txtHeartRate2);
            m_txtHeartRateList.Add(txtHeartRate3);
            m_txtHeartRateList.Add(txtHeartRate4);
            m_txtHeartRateList.Add(txtHeartRate5);
            m_txtHeartRateList.Add(txtHeartRate6);

            //��������������
            m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery1);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery2);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery3);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery4);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery5);
            //m_txtDaysAfterSurgeryList.Add(txtDaysAfterSurgery6);

            //����סԺ����
            m_txtDayOfHospitalList.Add(txtDayOfHospital1);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital2);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital3);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital4);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital5);
            //m_txtDayOfHospitalList.Add(txtDayOfHospital6);

            //������������
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling1);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling2);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling3);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling4);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling5);
            m_txtPhysicalCoolingList.Add(txtPhysicalCooling6);

            //������������ add by ywk 
            m_txtPhysicalHottingList.Add(txtPhysicalHotting1);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting2);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting3);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting4);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting5);
            m_txtPhysicalHottingList.Add(txtPhysicalHotting6);

            //������ʹ add by ywk
            //m_txtPainInfoList.Add(txtPainInfo1);
            //m_txtPainInfoList.Add(txtPainInfo2);
            //m_txtPainInfoList.Add(txtPainInfo3);
            //m_txtPainInfoList.Add(txtPainInfo4);
            //m_txtPainInfoList.Add(txtPainInfo5);
            //m_txtPainInfoList.Add(txtPainInfo6);

            //��ʹ��Ϊ�����������ֵ�������ж�ȡ
            //edit by ywk s����һ�������¶�ʮ���� 15:48:26
            m_lookPainInfoList.Add(lookPainInfo1);
            m_lookPainInfoList.Add(lookPainInfo2);
            m_lookPainInfoList.Add(lookPainInfo3);
            m_lookPainInfoList.Add(lookPainInfo4);
            m_lookPainInfoList.Add(lookPainInfo5);
            m_lookPainInfoList.Add(lookPainInfo6);


            //���ñ����Ƿ�͸��
            m_UcTimeOfShitList[0].IsTransparent = false;
            //m_UcTimeOfShitList[1].IsTransparent = true;
            //m_UcTimeOfShitList[2].IsTransparent = true;
            //m_UcTimeOfShitList[3].IsTransparent = true;
            //m_UcTimeOfShitList[4].IsTransparent = true;
            //m_UcTimeOfShitList[5].IsTransparent = true;

            //���ñ����Ƿ�͸��
            //m_UcTxtSeparateBoxList[1].IsTransparent = true;
            //m_UcTxtSeparateBoxList[2].IsTransparent = true;
            //m_UcTxtSeparateBoxList[4].IsTransparent = true;
            //m_UcTxtSeparateBoxList[5].IsTransparent = true;
        }
        #endregion

        #region ��ʼ��ʱ��α�ǩ
        /// <summary>
        /// ��ʼ��ʱ��α�ǩ
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
                    m_TimeSlot = dr["Value"].ToString().Split(new char[] { ',', '��' });
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

                lblTime4.ForeColor = Color.Blue;
                lblTime6.ForeColor = Color.Black;
            }
        }
        #endregion

        #region ��ʼ�����²�����ʽ
        /// <summary>
        /// ��ʼ�����²�����ʽ
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
                obj.EditValue = "8801";//Ĭ��ѡ��Ҹ�� edit by ywk
                //obj.Text = "Ҹ��";
            }
        }
        #endregion

        public void InitForm()
        {
            //��ʼ���ؼ�����
            InitControlArray();

            //��ʼ��ʱ��α�ǩ
            InitLabelTime();

            //��ʼ�����²�����ʽ
            InitlookUpWayOfSurvey();

            InitPainLookInfo();

        }
        /// <summary>
        /// ��ʼ����ʹָ��������
        /// </summary>
        private void InitPainLookInfo()
        {
            lookUpWindowPain.SqlHelper = MethodSet.App.SqlHelper;
            DataTable dtlookPain = new DataTable();
            string sql = string.Format(@"select id,name from categorydetail
                where categoryid='72'");
            dtlookPain = MethodSet.App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            dtlookPain.Columns["ID"].Caption = "����";
            dtlookPain.Columns["NAME"].Caption = "��ʹָ��";
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
            try
            {
                //InitForm();
                //foreach (LookUpEdit item in m_lookPainInfoList)
                //{
                //    item.EditValue = "";
                //    item.Properties.DataSource = null;
                //    //item.Properties.DisplayMember = "NAME";
                //    //item.Properties.ValueMember = "ID";

                //}

                //45,67

                //string times= GetConfigValueByKey("VITALSIGNSRECORDTIME"); ywk 
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


                //���������ж���ʾ��������������2012��6��20�� 10:25:46
                string showtext = MethodSet.GetConfigValueByKey("ThreeMeasureTableSetting");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(showtext);
                textEdit35.Text = doc.GetElementsByTagName("������OR����")[0].InnerText.Trim();

                //add by wyt 2012-08-07
                string myparam = doc.GetElementsByTagName("param")[0].InnerText.Trim();
                string[] myparams = myparam.Split(',');
                if (myparams.Length == 3)
                {
                    if (myparams[2] == "1")
                    {
                        this.textEditMy1.Visible = true;
                        this.textEditMy1Val.Visible = true;
                        this.textEditMy2.Visible = true;
                        this.textEditMy2Val.Visible = true;
                    }
                    else
                    {
                        this.textEditMy1.Visible = false;
                        this.textEditMy1Val.Visible = false;
                        this.textEditMy2.Visible = false;
                        this.textEditMy2Val.Visible = false;
                    }
                    this.textEditMy1.Text = myparams[0];
                    this.textEditMy2.Text = myparams[1];
                }

                string isShowTengTongZhiShu = doc.GetElementsByTagName("TengTongZhiShuShow")[0].InnerText.Trim();
                string isShowShenGao = doc.GetElementsByTagName("ShenGao")[0].InnerText.Trim();
                if (isShowTengTongZhiShu == "0")
                {
                    textEdit16.Hide();
                    lookPainInfo1.Hide();
                    lookPainInfo2.Hide();
                    lookPainInfo3.Hide();
                    lookPainInfo4.Hide();
                    lookPainInfo5.Hide();
                    lookPainInfo6.Hide();
                }
                else
                {
                    textEdit16.Show();
                    lookPainInfo1.Show();
                    lookPainInfo2.Show();
                    lookPainInfo3.Show();
                    lookPainInfo4.Show();
                    lookPainInfo5.Show();
                    lookPainInfo6.Show();
                }
                if (isShowShenGao == "0")
                {
                    textEdit112.Hide();
                    txtHeight1.Hide();
                }
                else
                {
                    textEdit112.Show();
                    txtHeight1.Show();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region ���Լ�
        /// <summary>
        /// ������Ϣ�����޸ĵ�����
        /// </summary>
        public int DayOfModify
        {
            get { return m_DayOfModify; }
        }
        //add by wyt
        /// <summary>
        /// ��ǰ����ʱ��
        /// </summary>
        public DateTime CurrentOperTime
        {
            get;
            set;
        }
        #endregion

        #region ��ǰ�ı��ؼ����»�ȡ����
        /// <summary>
        /// ��ǰ�ı��ؼ����»�ȡ����
        /// </summary>
        public void ActivateTextEditFocus()
        {
            if (m_ActivateTextEdit != null)
                m_ActivateTextEdit.Focus();

        }

        /// <summary>
        /// ��ȡ�н����TextEdit
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

        #region ���滤����Ϣ����
        /// <summary>
        /// ���滤����Ϣ����
        /// edit by ywk 2012��5��29�� 13:34:01
        /// </summary>
        /// <param name="DateOfSurvey">��������</param>
        /// <param name="m_noofinpat">������ҳ���</param>
        public bool SaveUCNursingRecordTable(string DateOfSurvey, string m_noofinpat)
        {
            try
            {
                ////�ж��Ƿ������ݱ���
                //if (CheckNullValue(-1))
                //{
                //    MethodSet.App.CustomMessageBox.MessageShow("û����Ҫ���������");
                //    return;
                //}

                string myNoOfInpat = string.Empty;//��ҳ���
                if (string.IsNullOrEmpty(m_noofinpat))//û�л�����
                {

                    myNoOfInpat = currInpatient.NoOfFirstPage.ToString();
                }
                else
                {
                    myNoOfInpat = m_noofinpat;
                }
                //��������Ƿ���Ч
                if (!IsDataValid()) return false;

                for (int i = 0; i < 6; i++)
                {
                    //�ж��Ƿ������ݱ���
                    if (!CheckNullValue(i))
                    {
                        MethodSet.App.SqlHelper.ExecuteNoneQuery("usp_EditNotesOnNursingInfo", GetSqlParam(i, DateOfSurvey, myNoOfInpat), CommandType.StoredProcedure);
                    }
                }
                //6��һ���һ��������ݿ⣬Ѫѹѭ�����β��룬������һ�β��루���£�

                //����Ѫѹ��ֵ
                if (m_UcTxtSeparateBoxList[0].IsInput)
                {
                    //ywk 2012��5��21�� 11:30:02
                    string updatesql = string.Format(@"update notesonnursing set bloodpressure='{0}'
            where dateofsurvey='{1}' and timeslot='" + lblTime1.Text + "' and noofinpat='{2}' ", m_UcTxtSeparateBoxList[0].BP, DateOfSurvey, myNoOfInpat);
                    MethodSet.App.SqlHelper.ExecuteNoneQuery(updatesql, CommandType.Text);
                }
                if (m_UcTxtSeparateBoxList[1].IsInput)
                {
                    string updatesql2 = string.Format(@"update notesonnursing set bloodpressure='{0}'
            where dateofsurvey='{1}' and timeslot='" + lblTime4.Text + "' and noofinpat='{2}' ", m_UcTxtSeparateBoxList[1].BP, DateOfSurvey, myNoOfInpat);

                    MethodSet.App.SqlHelper.ExecuteNoneQuery(updatesql2, CommandType.Text);
                }
                //����������λ��ֵ//update by wyt 20120807
                string updateother = string.Format(@"update notesonnursing set timeofshit='{0}',countin='{1}',countout='{2}',
                drainage='{3}',height='{4}',weight='{5}',allergy='{6}',daysaftersurgery='{7}',dayofhospital='{8}',param1='{9}',param2='{10}'  
                where dateofsurvey='{11}' and  timeslot='" + lblTime1.Text + "'  and noofinpat='{12}'", m_UcTimeOfShitList[0].Shit, txtCountIn1.Text.Trim(),
                txtCountOut1.Text.Trim(), txtDrainage1.Text.Trim(), txtHeight1.Text.Trim(), txtWeight1.Text.Trim(),
                txtAllergy1.Text.Trim(), txtDaysAfterSurgery1.Text.Trim(), txtDayOfHospital1.Text.Trim(), this.textEditMy1Val.Text.Trim(), this.textEditMy2Val.Text.Trim(), DateOfSurvey,
                myNoOfInpat);//������λ�Ĵ��һ��ʱ���
                MethodSet.App.SqlHelper.ExecuteNoneQuery(updateother, CommandType.Text);

                MethodSet.App.CustomMessageBox.MessageShow("����ɹ�");
                return true;

            }
            catch (Exception ex)
            {
                MethodSet.App.CustomMessageBox.MessageShow("����ʧ�ܣ�\n��ϸ����" + ex.Message);
                return false;
            }
        }
        #endregion

        #region ���ؼ���ֵ�Ƿ�Ϊ��

        /// <summary>
        /// ����ı����Ƿ�Ϊ��
        /// </summary>
        /// <param name="TextEditList">�ı��򼯺�</param>
        private bool CheckTextEdit(List<TextEdit> TextEditList)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// �����ʹ���������Ƿ�Ϊ��
        /// </summary>
        /// <param name="TextEditList"></param>
        /// <returns></returns>
        private bool CheckLookUpEdit(List<LookUpEditor> LookEditList)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// ���ؼ���ֵ�Ƿ�Ϊ��
        /// </summary>
        /// <param name="index">�ؼ�������-1Ϊȫ��</param>
        /// <returns></returns>
        private bool CheckNullValue(int index)
        {
            try
            {
                string IsHidePainHeight = DS_SqlService.GetConfigValueByKey("IsHidePainHeight_NursingRecord");
                if (index == -1) //ȫ���ؼ���ֵ
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
                    if (!CheckTextEdit(m_txtPhysicalHottingList)) return false;//������ʹ������
                    //if (!CheckTextEdit(m_txtPainInfoList)) return false;
                    //��ʹ��Ϊ������ؼ������ݴ������ж�ȡ
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
                else//ָ��index�����Ŀؼ���ֵ
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
                         string.IsNullOrEmpty(m_lookPainInfoList[index].Text)//��ʹ��λ
                      ))
                    {
                        return false;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// �õ���ʱ���֡�24Сʱ�ơ�add by wyt
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public int GetHour(DateTime time)
        {
            int hour = Convert.ToInt32(time.ToString("yyyy-HH").Split('-')[1]);
            return hour;
        }

        #region ��������Ƿ���Ч
        /// <summary>
        /// ��������Ƿ���Ч
        /// </summary>
        /// <returns></returns>
        private bool IsDataValid()
        {
            try
            {
                int intVart;

                //��������ʾ��ʽ(2:������)
                string config = DS_SqlService.GetConfigValueByKey("ThreeMeasureTableSetting");
                XmlDocument doc = new XmlDocument();
                if (!string.IsNullOrEmpty(config))
                {
                    doc.LoadXml(config);
                }
                XmlNodeList nodeList = doc.GetElementsByTagName("HuXiShowType");
                string huXiShowType = string.Empty;
                if (null != nodeList && nodeList.Count > 0)
                {
                    huXiShowType = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                }

                for (int i = 0; i < 6; i++)
                {
                    string strTimeSlot = "ʱ���Ϊ��" + m_lblTimeList[i].Text.Trim() + "���ļ�¼��\n";

                    //add by wyt
                    double inHosHour = GetHour(DateTime.Parse(MethodSet.AdmitDate));//��Ժʱ���
                    double timeSlot = double.Parse(m_lblTimeList[i].Text.Trim());   //��ǰ����ʱ���
                    if ((timeSlot < inHosHour - 2) && (DateTime.Parse(MethodSet.AdmitDate)).ToShortDateString().Equals(CurrentOperTime.ToShortDateString()))
                    {
                        //MessageBox.Show(m_txtTemperatureList[i].Text.Trim() + "," + m_txtBreatheList[i].Text.Trim() + "," + m_txtPhysicalCoolingList[i].Text.Trim() +
                        //    "," + m_txtPhysicalHottingList[i].Text.Trim() + "," + m_txtHeartRateList[i].Text.Trim() + "," + m_txtPulseList[i].Text.Trim() +
                        //    "," + m_lookPainInfoList[i].Text.Trim());
                        if (m_txtTemperatureList[i].Text.Trim() != "" || m_txtBreatheList[i].Text.Trim() != "" || m_txtPhysicalCoolingList[i].Text.Trim() != ""
                            || m_txtPhysicalHottingList[i].Text.Trim() != "" || m_txtHeartRateList[i].Text.Trim() != "" || m_txtPulseList[i].Text.Trim() != ""
                            || m_lookPainInfoList[i].Text.Trim() != "")
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "ʱ��㳬����Ժʱ�䷶Χ");
                            return false;
                        }
                        //if (m_UcTxtSeparateBoxList[0].IsInput && inHosHour<12) Ѫѹ��ע�͵�  addby ywk 2012��8��14�� 11:40:19
                        //{
                        //    MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "ʱ��㳬����Ժʱ�䷶Χ!");
                        //    return false;
                        //}
                    }

                    //����
                    if (m_txtTemperatureList[i].Text.Trim().ToString() != "")
                    {
                        if (!Dataprocessing.IsNumber(m_txtTemperatureList[i].Text.Trim().ToString(), 1))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "���±���Ϊ���֣�С���㱣��һλ��");
                            return false;
                        }
                        else
                        {
                            double dVar = Convert.ToDouble(m_txtTemperatureList[i].Text.Trim());
                            if (!(dVar >= 34 && dVar <= 43))
                            {
                                MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "���³�����Χ��34����43�档");
                                return false;
                            }
                        }
                    }

                    //����
                    if (m_txtPulseList[i].Text.Trim().ToString() != "")
                    {
                        if (!Dataprocessing.IsNumber(m_txtPulseList[i].Text.Trim().ToString(), 0))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "��������Ϊ������");
                            return false;
                        }
                        else
                        {
                            intVart = Convert.ToInt32(m_txtPulseList[i].Text.Trim().ToString());
                            if (!(intVart >= 20 && intVart <= 200))
                            {
                                MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "����������Χ��20��/����200��/�֡�");
                                return false;
                            }
                        }
                    }

                    //����
                    if (m_txtHeartRateList[i].Text.Trim().ToString() != "")
                    {
                        if (!Dataprocessing.IsNumber(m_txtHeartRateList[i].Text.Trim().ToString(), 0))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "���ʱ���Ϊ������");
                            return false;
                        }
                        else
                        {
                            intVart = Convert.ToInt32(m_txtHeartRateList[i].Text.Trim().ToString());
                            if (!(intVart >= 20 && intVart <= 200))
                            {
                                MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "���ʳ�����Χ��20��/����200��/�֡�");
                                return false;
                            }
                        }
                    }

                    //����
                    if (m_txtBreatheList[i].Text.Trim().ToString() != "")
                    {
                        if (!(Dataprocessing.IsNumber(m_txtBreatheList[i].Text.Trim(), 0)))
                        {//��������
                            if (huXiShowType == "2")
                            {//������
                                if (m_txtBreatheList[i].Text.Trim().ToUpper() != "R")
                                {
                                    MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "����ֻ��Ϊ����������R(������)");
                                    return false;
                                }
                            }
                            else
                            {
                                MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "����ֻ��Ϊ������");
                                return false;
                            }
                        }
                        else
                        {//������ 
                            intVart = Convert.ToInt32(m_txtBreatheList[i].Text.Trim().ToString());
                            if (!(intVart >= 0 && intVart <= 90))
                            {
                                MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "����������Χ��0��/����90��/�֡�");
                                return false;
                            }
                        }
                    }

                    //�������µ�������֤ 
                    //add by ywk 2012��4��17��16:49:30
                    if (m_txtPhysicalCoolingList[i].Text.Trim().ToString() != "")
                    {
                        if (!Dataprocessing.IsNumber(m_txtPhysicalCoolingList[i].Text.Trim().ToString(), 1))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "�������±���Ϊ���֣�С���㱣��һλ��");
                            return false;
                        }
                        else
                        {
                            double dVar = Convert.ToDouble(m_txtPhysicalCoolingList[i].Text.Trim());
                            if (!(dVar >= 34 && dVar <= 43))
                            {
                                MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "�������³�����Χ��34����43�档");
                                return false;
                            }
                        }
                    }

                    //�������²��ܴ��ڵ�ǰʱ���Ĳ����¶� edit by ywk 
                    //����Ϊ�գ�����д�������£��˴�������ס
                    //edit by wyt 2012-11-05 ȥ�����¡��������¡��¶ȱ仯���߼���߼���ϵ
                    string IsHidePainHeight = MethodSet.GetConfigValueByKey("IsHidePainHeight_NursingRecord");
                    if (IsHidePainHeight.Substring(13, 1) == "0")
                    {
                        if (string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) && !string.IsNullOrEmpty(m_txtPhysicalCoolingList[i].Text.Trim()))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "���������ʱ������");
                            return false;
                        }
                        string temperature = m_txtTemperatureList[i].Text.Trim();
                        if (Convert.ToDouble(string.IsNullOrEmpty(m_txtPhysicalCoolingList[i].Text.Trim()) ? "0" : m_txtPhysicalCoolingList[i].Text.Trim()) > Convert.ToDouble(string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) ? "0" : m_txtTemperatureList[i].Text.Trim()))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "�������²��ܳ�����ʱ������");
                            return false;
                        }
                    }


                    //�������µ�������֤ 
                    //add by ywk 2012��4��17��16:49:30
                    if (m_txtPhysicalHottingList[i].Text.Trim().ToString() != "")
                    {
                        if (!Dataprocessing.IsNumber(m_txtPhysicalHottingList[i].Text.Trim().ToString(), 1))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "�¶ȱ仯����Ϊ���֣�С���㱣��һλ��");
                            return false;
                        }
                        else
                        {
                            double dVar = Convert.ToDouble(m_txtPhysicalHottingList[i].Text.Trim());
                            if (!(dVar >= 34 && dVar <= 43))
                            {
                                MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "�¶ȱ仯������Χ��34����43�档");
                                return false;
                            }
                        }
                    }

                    //�������²���С���ڵ�ǰʱ���Ĳ����¶� edit by ywk 
                    //����Ϊ�գ�����д�������£��˴�������ס
                    //edit by wyt 2012-11-05 ȥ�����¡��������¡��¶ȱ仯���߼���߼���ϵ
                    if (IsHidePainHeight.Substring(13, 1) == "0")
                    {
                        if (string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) && !string.IsNullOrEmpty(m_txtPhysicalHottingList[i].Text.Trim()))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "���������ʱ������");
                            return false;
                        }
                    }
                    //����д���²����������� 
                    //if (!string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) && string.IsNullOrEmpty(m_txtPhysicalHottingList[i].Text.Trim()))
                    //{
                    //    return true;
                    //}
                    //string temperature = m_txtTemperatureList[i].Text.Trim();
                    //������ȡ�� 
                    //if (Convert.ToDouble(string.IsNullOrEmpty(m_txtPhysicalHottingList[i].Text.Trim()) ? "0" : m_txtPhysicalHottingList[i].Text.Trim()) < Convert.ToDouble(string.IsNullOrEmpty(m_txtTemperatureList[i].Text.Trim()) ? "0" : m_txtTemperatureList[i].Text.Trim()))
                    //{
                    //    MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "�¶ȱ仯���ܵ��ڴ�ʱ������");
                    //    return false;
                    //}
                    //������ʹ��λֻ���������� add by ywk 
                    //if (m_txtPainInfoList[i].Text.Trim().ToString() != "")
                    //{
                    //    if (!Dataprocessing.IsNumber(m_txtPainInfoList[i].Text.Trim().ToString(), 1))
                    //    {
                    //        MethodSet.App.CustomMessageBox.MessageShow(strTimeSlot + "��ʹָ������Ϊ���֡�");
                    //        return false;
                    //    }
                    //else
                    //{
                    //    return false;
                    //}
                    //}
                    //ucTxtTimeOfShit1
                }

                //Ѫѹ
                for (int j = 0; j < 2; j++)
                {
                    if (!m_UcTxtSeparateBoxList[j].CheckData())
                    {
                        return false;
                    }
                }
                //������
                if (Tool.GetByteLength(ucTxtTimeOfShit1.Shit) > 50)
                {
                    MethodSet.App.CustomMessageBox.MessageShow("�������������ű�������೦���ű�������೦�����ַ����ȼӺͲ��ܴ���48");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                MethodSet.App.CustomMessageBox.MessageShow(ex.Message);
                return false;
            }
        }
        #endregion

        #region ��ȡ�洢����
        /// <summary>
        /// ��ȡ�洢����
        /// </summary>
        /// <param name="index">�ؼ���������</param>
        /// <param name="DateOfSurvey">��������</param>
        /// <returns></returns>
        private SqlParameter[] GetSqlParam(int index, string DateOfSurvey, string noofinpat)
        {
            SqlParameter[] sqlParam = new SqlParameter[]
            {
                new SqlParameter("@NoOfInpat",SqlDbType.VarChar),//������
                new SqlParameter("@DateOfSurvey",SqlDbType.VarChar),//��������
                new SqlParameter("@Temperature",SqlDbType.VarChar),//����
                new SqlParameter("@WayOfSurvey",SqlDbType.Int),//�������

                new SqlParameter("@Pulse",SqlDbType.VarChar),//����
                new SqlParameter("@HeartRate",SqlDbType.VarChar),//����
                new SqlParameter("@Breathe",SqlDbType.VarChar),//����
                //new SqlParameter("@BloodPressure",SqlDbType.VarChar),//Ѫѹ

                //new SqlParameter("@TimeOfShit",SqlDbType.VarChar),//������ 
                //new SqlParameter("@CountIn",SqlDbType.VarChar),//������
                //new SqlParameter("@CountOut",SqlDbType.VarChar),//�ܳ���
                //new SqlParameter("@Drainage",SqlDbType.VarChar),//������

                //new SqlParameter("@Height",SqlDbType.VarChar),// ����
                //new SqlParameter("@Weight",SqlDbType.VarChar),//����
                //new SqlParameter("@Allergy",SqlDbType.VarChar),//����ҩ��
                //new SqlParameter("@DaysAfterSurgery",SqlDbType.VarChar),//����������

                //new SqlParameter("@DayOfHospital",SqlDbType.VarChar),//סԺ���� 
                new SqlParameter("@PhysicalCooling",SqlDbType.VarChar),//��������
                new SqlParameter("@DoctorOfRecord",SqlDbType.VarChar),//��¼��
                //new SqlParameter("@indx",SqlDbType.VarChar),//���
                new SqlParameter("@TimeSlot",SqlDbType.VarChar),//ʱ���
                 //�����������º���ʹ�����ֶ� 
                new SqlParameter("@PhysicalHotting",SqlDbType.VarChar),//�¶ȱ仯
                new SqlParameter("@PainInfo",SqlDbType.VarChar),//��ʹ

                //new SqlParameter("@Param1",SqlDbType.VarChar),//����1
                //new SqlParameter("@Param2",SqlDbType.VarChar)//����2

            };

            sqlParam[0].Value = noofinpat;//MethodSet.CurrentInPatient.NoOfFirstPage;
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
            //sqlParam[9].Value = index.ToString();
            sqlParam[9].Value = m_lblTimeList[index].Text;

            sqlParam[10].Value = m_txtPhysicalHottingList[index].Text.Trim();
            sqlParam[11].Value = m_lookPainInfoList[index].Text.ToString().Trim();

            return sqlParam;
        }
        #endregion

        #region ��ȡָ�����ڶ�Ӧ�Ļ�����Ϣ����
        /// <summary>
        /// ��ȡָ�����ڶ�Ӧ�Ļ�����Ϣ����
        /// </summary>
        /// <param name="DateOfSurvey"></param>
        /// <param name="m_oofinpat">����������ҳ����ֶ�</param>
        public void GetNotesOfNursingInfo(string DateOfSurvey, string m_oofinpat, Inpatient currInpatient)
        {
            //����ؼ�����
            ClearControlValue();

            //��ȡָ����������
            SqlParameter[] sqlParam = new SqlParameter[]
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@DateOfSurvey", SqlDbType.VarChar)
            };
            string noofinpat = string.Empty;//������ҳ���
            if (string.IsNullOrEmpty(m_oofinpat))//���Ϊ�գ���ʾû�л�����
            {
                noofinpat = currInpatient.NoOfFirstPage.ToString();
            }
            else
            {
                noofinpat = m_oofinpat;
            }
            //sqlParam[0].Value = MethodSet.CurrentInPatient.NoOfFirstPage;
            sqlParam[0].Value = noofinpat;
            //sqlParam[0].Value = MethodSet.CurrentInPatient.NoOfFirstPage;
            sqlParam[1].Value = DateOfSurvey;

            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_GetNotesOnNursingInfo", sqlParam, CommandType.StoredProcedure);

            //�ؼ���ֵ
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //int index = -1;
                    //if (dr["Indx"] != null && dr["Indx"].ToString() != "")
                    //{
                    //    index = int.Parse(dr["Indx"].ToString());
                    //}
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
                        //m_txtDrainageList[index].Text = dr["Drainage"].ToString();//������

                        //m_txtHeightList[index].Text = dr["Height"].ToString();
                        //m_txtWeightList[index].Text = dr["Weight"].ToString();
                        //m_txtAllergyList[index].Text = dr["Allergy"].ToString();
                        //m_txtDaysAfterSurgeryList[index].Text = dr["DaysAfterSurgery"].ToString();

                        //m_txtDayOfHospitalList[index].Text = dr["DayOfHospital"].ToString();
                        m_txtPhysicalCoolingList[index].Text = dr["PhysicalCooling"].ToString();

                        //�����������º���ʹ������λ edit by ywk  
                        m_txtPhysicalHottingList[index].Text = dr["PhysicalHotting"].ToString();
                        m_lookPainInfoList[index].Text = dr["PainInfo"].ToString();
                    }
                }
                //��ȡ���ݰ󶨵����档��Ѫѹ��ֵ
                //DataRow[] dr1 = dt.Select("timeslot='3'");
                //DataRow[] dr1 = dt.Select("indx='0'");
                DataRow[] dr1 = dt.Select("timeslot='" + lblTime1.Text + "'");
                if (dr1.Length > 0)
                {
                    m_UcTxtSeparateBoxList[0].BP = dr1[0]["BloodPressure"].ToString();

                    m_UcTimeOfShitList[0].Shit = dr1[0]["TimeOfShit"].ToString();
                    m_txtCountInList[0].Text = dr1[0]["CountIn"].ToString();
                    m_txtCountOutList[0].Text = dr1[0]["CountOut"].ToString();
                    m_txtDrainageList[0].Text = dr1[0]["Drainage"].ToString();//������
                    m_txtHeightList[0].Text = dr1[0]["Height"].ToString();
                    m_txtWeightList[0].Text = dr1[0]["Weight"].ToString();
                    m_txtAllergyList[0].Text = dr1[0]["Allergy"].ToString();
                    m_txtDaysAfterSurgeryList[0].Text = dr1[0]["DaysAfterSurgery"].ToString();
                    m_txtDayOfHospitalList[0].Text = dr1[0]["DayOfHospital"].ToString();
                    //add by wyt
                    this.textEditMy1Val.Text = dr1[0]["Param1"].ToString();
                    this.textEditMy2Val.Text = dr1[0]["Param2"].ToString();
                }
                //DataRow[] dr2 = dt.Select("timeslot='15'");
                //DataRow[] dr2 = dt.Select("indx='3'");
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
                //ֻ��¼���������ݣ��͸�Ҹ��ֵedit by ywk 
                for (int i = 0; i < 6; i++)
                {
                    m_lookUpWayOfSurveyList[i].EditValue = "8801";
                }

            }

            //סԺ����
            if (m_txtDayOfHospitalList[0].Text.Trim() == ""
                && Convert.ToDateTime(DateOfSurvey) == Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")))
            {
                DateTime AdmitDate = Convert.ToDateTime(Convert.ToDateTime(MethodSet.AdmitDate).Date.ToString("yyyy-MM-dd 00:00:01"));
                DateTime CurrDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:01"));

                m_txtDayOfHospitalList[0].Text = (CurrDate.Subtract(AdmitDate).Days + 1).ToString();
            }

            //����������
            if (m_txtDaysAfterSurgeryList[0].Text.Trim() == ""
                && Convert.ToDateTime(DateOfSurvey) == Convert.ToDateTime(DateTime.Now.Date.ToString("yyyy-MM-dd")))
            {
                //SetDaysAfterSurgery(MethodSet.DaysAfterSurgery);------old

                //ShowAfterSurgery showafter = new ShowAfterSurgery();
                string[] surgerydays;
                string searchsql = string.Format(@" select * from patientstatus  p left join THERE_CHECK_EVENT t on p.ccode=t.id  where noofinpat='{0}' and
                (p.ccode='7000' or t.name ='���䡤����')", currInpatient.Code);
                DataTable AllSurgeryData = MethodSet.App.SqlHelper.ExecuteDataTable(searchsql, CommandType.Text);
                if (AllSurgeryData.Rows.Count > 0)
                {
                    surgerydays = new string[AllSurgeryData.Rows.Count];
                    for (int i = 0; i < AllSurgeryData.Rows.Count; i++)
                    {
                        surgerydays[i] = AllSurgeryData.Rows[i]["dotime"].ToString();
                    }
                    //SetDaysAfterSurgery(surgerydays);
                }
                //��������������ֵ(edit by ywk 2012��5��14�� 10:25:16)
                //ComputeSurgeryDay();
            }

        }
        #endregion

        #region ��������������
        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="DaysAfterSurgery">��������������</param>
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

        /// <summary>
        /// �������������� edit by ywk
        /// </summary>
        /// <param name="DaysAfterSurgery">��ǰ����¼�����ʱ��ʱ��</param>
        public void SetDaysAfterSurgery(string DaysAfterSurgery, string patid)
        {

            //DateOfSurvey ��ǰ¼�������ʱ�� 
            string serachsql = string.Format
                (@"    select * from patientstatus  p left join THERE_CHECK_EVENT t on p.ccode=t.id  where noofinpat='{0}' 
and trunc(to_date(p.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 
and dotime<'{1}'  and (p.ccode='7000' or t.name ='���䡤����') order by dotime asc", patid, DaysAfterSurgery + " 23:59:59");
            //string serachsql = string.Format(@"    select * from patientstatus where noofinpat='{0}' and trunc(to_date(patientstatus.dotime,'yyyy-mm-dd hh24:mi:ss'))>=trunc(to_date('{1}','yyyy-mm-dd hh24:mi:ss'))-14 and dotime<'{1}'  and ccode='7000'   order by dotime asc", MethodSet.CurrentInPatient.Code, DaysAfterSurgery + " 23:59:59");
            DataTable AllSurgeryData = MethodSet.App.SqlHelper.ExecuteDataTable(serachsql, CommandType.Text);

            if (AllSurgeryData.Rows.Count > 0)
            {

                if (!string.IsNullOrEmpty(DaysAfterSurgery))
                {
                    DateTime nowInputDate = DateTime.Parse(DaysAfterSurgery + " 23:59:59");

                    if (AllSurgeryData.Rows.Count > 0)//��һ�����ϵ����� 
                    {
                        string content = ReturnCotent(nowInputDate, AllSurgeryData);
                        m_txtDaysAfterSurgeryList[0].Text = content;
                    }

                }
            }

            //for (int i = 0; i < 6; i++)
            //{
            //    m_lookUpWayOfSurveyList[i].EditValue = "8801";
            //}
        }
        /// <summary>
        /// ����һ�����ϵ���������Ҫ��ʾ������
        /// ywk
        /// �����������޸� xll 20120731
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
                    outcontent = ReturnCount((i + 1).ToString());
                }
                else if (i == (dtdata.Rows.Count - 1))
                {
                    outcontent = (nowinputtime - DateTime.Parse(dtdata.Rows[i]["dotime"].ToString())).Days.ToString();
                }

            }
            return outcontent;
        }
        /// <summary>
        /// ����������ڼ��ε���ʾ����
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
                    show = "II-0";
                    break;
                case "3":
                    show = "III-0";
                    break;
                case "4":
                    show = "IV-0";
                    break;
                case "5":
                    show = "V-0";
                    break;
                case "6":
                    show = "VI-0";
                    break;
                case "7":
                    show = "VII-0";
                    break;
                case "8":
                    show = "VIII-0";
                    break;
                case "9":
                    show = "IX-0";
                    break;
                case "10":
                    show = "X-0";
                    break;


                default:
                    break;
            }
            return show;
        }
        #endregion

        #region ��ȡʱ��ζ�Ӧ���±�����
        /// <summary>
        /// ��ȡʱ��ζ�Ӧ���±�����
        /// </summary>
        /// <param name="TimeSlot">ʱ���ֵ</param>
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

        #region ����ؼ�����
        /// <summary>
        /// ����ؼ�����
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
                //������ʹ������
                m_txtPhysicalHottingList[i].Text = "";
                //m_txtPainInfoList[i].Text = "";
                m_lookPainInfoList[i].Text = "";
            }
            m_UcTxtSeparateBoxList[0].BP = "";//Ѫѹ
            m_UcTxtSeparateBoxList[1].BP = "";//Ѫѹ
            m_UcTimeOfShitList[0].DateTextReset();//������
            m_txtCountInList[0].Text = "";//������
            m_txtCountOutList[0].Text = "";//�ܳ���
            m_txtDrainageList[0].Text = "";//������
            m_txtHeightList[0].Text = "";//����
            m_txtWeightList[0].Text = "";//����
            m_txtAllergyList[0].Text = "";//����
            m_txtDaysAfterSurgeryList[0].Text = "";//����������
            m_txtDayOfHospitalList[0].Text = "";//סԺ����

            //add by wyt 2012-08-07
            this.textEditMy1Val.Text = "";
            this.textEditMy2Val.Text = "";
        }
        #endregion

        /// <summary>
        /// �ټӴ˷����������ʹָ���������ֵ(�˷�������Ч)
        /// </summary>
        /// <param name="DateOfSurvey"></param>
        public void OperateLookUp(string DateOfSurvey)
        {
            //����ؼ�����
            ClearControlValue();

            //��ȡָ����������
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@DateOfSurvey", SqlDbType.VarChar)
            };
            sqlParam[0].Value = currInpatient.NoOfFirstPage;
            sqlParam[1].Value = DateOfSurvey;

            DataTable dt = MethodSet.App.SqlHelper.ExecuteDataTable("usp_GetNotesOnNursingInfo", sqlParam, CommandType.StoredProcedure);

            //�ؼ���ֵ
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //int index = -1;
                    int index = GetIndex(dr["TimeSlot"].ToString());
                    //if (dr["indx"] != null && dr["indx"].ToString() != "")
                    //{
                    //    index = int.Parse(dr["indx"].ToString());
                    //}
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
                        m_txtDrainageList[index].Text = dr["Drainage"].ToString();

                        m_txtHeightList[index].Text = dr["Height"].ToString();
                        m_txtWeightList[index].Text = dr["Weight"].ToString();
                        m_txtAllergyList[index].Text = dr["Allergy"].ToString();
                        m_txtDaysAfterSurgeryList[index].Text = dr["DaysAfterSurgery"].ToString();

                        m_txtDayOfHospitalList[index].Text = dr["DayOfHospital"].ToString();
                        m_txtPhysicalCoolingList[index].Text = dr["PhysicalCooling"].ToString();

                        //�����������º���ʹ������λ edit by ywk  
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
        /// <summary>
        /// ����ѡ������ڣ��ı�סԺ������ֵ
        /// edit by ywk ����һ��������ʮ���� 20:48:06
        /// </summary>
        /// <param name="inputdate"></param>
        internal void SetDayInHospital(string inputdate)
        {

            DateTime AdmitDate = Convert.ToDateTime(Convert.ToDateTime(MethodSet.AdmitDate).Date.ToString("yyyy-MM-dd 00:00:01"));
            DateTime CurrDate = Convert.ToDateTime(inputdate + "  00:00:01");

            m_txtDayOfHospitalList[0].Text = (CurrDate.Subtract(AdmitDate).Days + 1).ToString();
        }

        #region ����ÿ���ı����Enter�¼�

        /// <summary>
        /// �����һ���ؼ�������Name
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
            else if (currentControlName.StartsWith("txtPhysicalHotting"))
            {
                nextControlName = "lookPainInfo" + currentControlName.Substring("txtPhysicalHotting".Length);
            }
            else if (currentControlName.StartsWith("lookPainInfo"))
            {
                nextControlName = "txtTemperature" + (Convert.ToInt32(currentControlName.Substring("lookPainInfo".Length)) + 1);
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
        /// ���ı�����õ�Enter�¼�
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