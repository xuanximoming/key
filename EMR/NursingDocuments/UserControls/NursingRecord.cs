using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using DrectSoft.Core.NursingDocuments.UserControls;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Core.NursingDocuments.PublicSet;
using DevExpress.XtraBars;
using DrectSoft.Wordbook;
using DrectSoft.Common.Eop;
using DrectSoft.Service;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.NursingDocuments
{
    public partial class NursingRecord : DevBaseForm
    {

        string[] m_DaysAfterSurgery;
        Inpatient m_currInpatient;
        /// <summary>
        /// NursingDocuments工程调用该构造函数
        /// </summary>
        public NursingRecord()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 非NursingDocuments工程调用该窗体，用该构造函数
        /// </summary>
        /// <param name="App">应用程序对象接口</param>
        public NursingRecord(IEmrHost App, Inpatient currInpatient )
        {
            m_currInpatient = currInpatient;
            InitializeComponent();
            ucNursingRecordTable1.currInpatient = m_currInpatient;
            m_DaysAfterSurgery = null;
            MethodSet.App = App;
        }

        public DialogResult ShowNursingRecord()
        {
            return ShowDialog();

        }

        /// <summary>
        /// 模态调用窗口（）
        /// </summary>
        /// <param name="DaysAfterSurgery"></param>
        /// <returns></returns>
        public DialogResult ShowNursingRecord(string[] DaysAfterSurgery)
        {
            m_DaysAfterSurgery = DaysAfterSurgery;
            return ShowDialog();

        }

        /// <summary>
        /// 初始化病人基本信息
        /// </summary>
        private void InitInpatInfo(Inpatient currInpatient)
        {
            DataTable dt = null;
            //此处的DataTable数据的提取，就区分开是否是切换病人后的数据
            if (IsChagedPat)//是跳转来的 
            {
                dt = MethodSet.GetRedactPatientInfoFrm("14", "", NoOfInpat);
            }
            else
            {
                NoOfInpat = currInpatient.NoOfFirstPage.ToString();
                dt = MethodSet.GetRedactPatientInfoFrm("14", "", currInpatient.NoOfFirstPage.ToString());
            }
            //DataTable dt = MethodSet.GetRedactPatientInfoFrm("14", "", MethodSet.CurrentInPatient.NoOfFirstPage.ToString());
            if (dt.Rows.Count == 1)
            {
                //如果入科时间为空，则读取入院时间，否则以入科时间为准
                MethodSet.AdmitDate = string.IsNullOrEmpty(dt.Rows[0]["inwarddate"].ToString().Trim()) ?
                    dt.Rows[0]["AdmitDate"].ToString().Trim() : dt.Rows[0]["inwarddate"].ToString().Trim();
                //PatientInfo.IsBaby
                //if (PatientInfo.IsBaby == "1")//如果是婴儿 add by ywk 2012年11月22日19:59:18 
                //{
                //    MethodSet.PatID = PublicSet.MethodSet.GetPatData(PatientInfo.Mother).Rows[0]["Patid"].ToString();
                //}
                //else
                //{
                    MethodSet.PatID = dt.Rows[0]["PatID"].ToString();
                //}

                MethodSet.OutHosDate = dt.Rows[0]["status"].ToString().Trim() == "1503" ? dt.Rows[0]["OutHosDate"].ToString().Trim() : "";
                //MethodSet.PatID = dt.Rows[0]["PatID"].ToString();
                MethodSet.OutWardDate = dt.Rows[0]["status"].ToString().Trim() == "1502" ? dt.Rows[0]["OutWardDate"].ToString().Trim() : "";
                MethodSet.PatName = dt.Rows[0]["Name"].ToString().Trim();//新增的
                MethodSet.BedID = dt.Rows[0]["OutBed"].ToString().Trim();//新增的
                MethodSet.Age = dt.Rows[0]["AGESTR"].ToString().Trim();//新增的
                //add by ywk 二〇一三年五月二十八日 15:21:51  
                MethodSet.NoOfinPat = dt.Rows[0]["NOOFINPAT"].ToString().Trim();//新增的病人的首页序号

                MethodSet.RecordNoofinpat = dt.Rows[0]["NOOFRECORD"].ToString().Trim();//新增的病人的首页序号
            }
        }

        public void NursingRecord_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshDate(m_currInpatient);
                dateEdit_DateTimeChanged(null, null);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        public void RefreshDate(Inpatient currInpatient)
        {
            try
            {
                DataTable dt = MethodSet.GetRedactPatientInfoFrm("14", "", m_currInpatient.NoOfFirstPage.ToString());
                MethodSet.DaysAfterSurgery = m_DaysAfterSurgery;
                if (currInpatient != null)
                {
                    InitInpatInfo(currInpatient);
                    if (dt.Rows[0]["isbaby"].ToString().Equals("1"))//如果是婴儿
                    {
                        txtPatID.Text = PublicSet.MethodSet.GetPatData(dt.Rows[0]["mother"].ToString()).Rows[0]["Patid"].ToString();
                    }
                    else
                    {
                        txtPatID.Text = MethodSet.PatID;
                    }

                    //txtPatID.Text = MethodSet.PatID;
                    txtInpatName.Text = MethodSet.PatName;
                    //新增的床号显示
                    txtBedID.Text = MethodSet.BedID;
                    //txtInpatName.Text = MethodSet.CurrentInPatient.Name;
                    //txtAge.Text = PatientInfo.Age;
                    txtAge.Text = MethodSet.Age;//wyt

                    #region 已注释
                //MethodSet.DaysAfterSurgery = m_DaysAfterSurgery;
                //if (currInpatient != null)
                //{
                //    InitInpatInfo(currInpatient);
                //    if (patientInfo.IsBaby == "1")//如果是婴儿 add by ywk 2012年11月22日19:59:18 
                //    {
                //        txtPatID.Text = PublicSet.MethodSet.GetPatData(patientInfo.Mother).Rows[0]["Patid"].ToString();
                //    }
                //    else
                //    {
                //        txtPatID.Text = MethodSet.PatID;
                //    }

                //    //txtPatID.Text = MethodSet.PatID;
                //    txtInpatName.Text = MethodSet.PatName;
                //    //新增的床号显示
                //    txtBedID.Text = MethodSet.BedID;
                //    //txtInpatName.Text = MethodSet.CurrentInPatient.Name;
                //    //txtAge.Text = PatientInfo.Age;
                    //    txtAge.Text = MethodSet.Age;//wyt
                    #endregion
                }
                else
                {
                    btnSave.Enabled = false;
                }

                ucNursingRecordTable1.InitForm();

                //add by cyq 2013-03-05
                dateEdit.DateTimeChanged -= new EventHandler(dateEdit_DateTimeChanged);
                if (IsChagedPat)//是跳转来的
                {
                    dateEdit.Text = InputDate;
                }
                else
                {//add by cyq 2013-03-05
                    DataTable inps = DS_SqlService.GetInpatientByID((int)currInpatient.NoOfFirstPage);
                    if (null != inps && inps.Rows.Count == 1)
                    {
                        if (inps.Rows[0]["status"].ToString() == "1502" || inps.Rows[0]["status"].ToString() == "1503")
                        {
                            dateEdit.Text = DateTime.Parse(inps.Rows[0]["outhosdate"].ToString()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            dateEdit.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        dateEdit.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    }
                }
                //add by cyq 2013-03-05
                dateEdit.DateTimeChanged += new EventHandler(dateEdit_DateTimeChanged);

                //dateEdit.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                //体征录入界面刚进入时，进入自定义控件进行计算要显示的手术后天数  2012年5月15日 09:44:02
                //设置手术后天数
                string inputdate = dateEdit.Text;
                //现新表已经取得的真实的首页序号 add by ywk 2013-4-8 16:10:04 
                ucNursingRecordTable1.SetDaysAfterSurgery(inputdate, currInpatient.NoOfFirstPage.ToString());
                ucNursingRecordTable1.CurrentOperTime = DateTime.Parse(inputdate);//add by wyt

                //ucNursingRecordTable1.SetDaysAfterSurgery(m_DaysAfterSurgery);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                if (string.IsNullOrEmpty(this.dateEdit.Text))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("录入日期不能为空");
                    this.dateEdit.Focus();
                    return;
                }
                if (ucNursingRecordTable1.SaveUCNursingRecordTable(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat))
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 时间变化事件
        /// edit by Yanqiao.Cai 2012-11-14
        /// 1、add try ... catch
        /// 2、优化提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dateEdit_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_currInpatient != null)
                {
                    //btnSave.Enabled = true;

                    string CurrDateTime = MethodSet.GetCurrServerDateTime.Date.ToString("yyyy-MM-dd 00:00:01");

                    int Day = Convert.ToDateTime(CurrDateTime).
                        Subtract(Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:01"))).Days + 1;

                    btnSave.Enabled = (Day <= ucNursingRecordTable1.DayOfModify || ucNursingRecordTable1.DayOfModify == -1) ? true : false;

                    //判断服务器当前日期是否小于入院日期
                    if (!string.IsNullOrEmpty(MethodSet.AdmitDate))
                    {
                        DateTime m_AdmitDate = Convert.ToDateTime(MethodSet.AdmitDate);
                        if (Convert.ToDateTime(m_AdmitDate.Date.ToString("yyyy-MM-dd 00:00:00"))
                            > Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:00")))
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("录入日期不能小于入院日期");
                            //btnSave.Enabled = false;
                            //add by cyq 2013-03-05
                            dateEdit.DateTimeChanged -= new EventHandler(dateEdit_DateTimeChanged);
                            dateEdit.DateTime = m_AdmitDate;
                            //add by cyq 2013-03-05
                            dateEdit.DateTimeChanged += new EventHandler(dateEdit_DateTimeChanged);
                            dateEdit.Focus();
                            return;
                        }

                        if (!string.IsNullOrEmpty(MethodSet.OutHosDate))
                        {
                            DateTime outHosDate = Convert.ToDateTime(MethodSet.OutHosDate);
                            if (Convert.ToDateTime(outHosDate.Date.ToString("yyyy-MM-dd 00:00:00"))
                                 < Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:00")))
                            {
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("录入日期不能大于出院日期");
                                //add by cyq 2013-03-05
                                dateEdit.DateTimeChanged -= new EventHandler(dateEdit_DateTimeChanged);
                                dateEdit.DateTime = outHosDate;
                                //add by cyq 2013-03-05
                                dateEdit.DateTimeChanged += new EventHandler(dateEdit_DateTimeChanged);
                                dateEdit.Focus();
                                return;
                            }
                        }
                    }

                    #region 已注释
                    ////判断选择日期是否超出出区日期2012年5月9日22:50:00 泗县修改
                    //if (!string.IsNullOrEmpty(MethodSet.OutHosDate))
                    //{
                    //    DateTime m_OutWardDate = Convert.ToDateTime(MethodSet.OutWardDate);
                    //    if (Convert.ToDateTime(m_OutWardDate.Date.ToString("yyyy-MM-dd 00:00:00"))
                    //        < Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:00")))
                    //    {
                    //        btnSave.Enabled = false;
                    //    }
                    //}
                    #endregion

                    //判断选择日期是否超出服务器当前日前
                    if (Convert.ToDateTime(CurrDateTime) < Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:00")))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("录入日期不能大于当前日期");
                        //btnSave.Enabled = false;
                        //add by cyq 2013-03-05
                        dateEdit.DateTimeChanged -= new EventHandler(dateEdit_DateTimeChanged);
                        dateEdit.DateTime = Convert.ToDateTime(CurrDateTime);
                        //add by cyq 2013-03-05
                        dateEdit.DateTimeChanged += new EventHandler(dateEdit_DateTimeChanged);
                        dateEdit.Focus();
                        return;
                    }


                    ucNursingRecordTable1.GetNotesOfNursingInfo(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat,m_currInpatient);
                    //ucNursingRecordTable1.OperateLookUp(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"));


                    //根据选择的日期设置手术后天数 edit by ywk 2012年5月15日 09:43:56
                    string inputdate = string.Format("{0:yyyy-MM-dd}", dateEdit.DateTime.Date.ToString("yyyy-MM-dd"));
                    //现新表已经取得的真实的首页序号 add by ywk 2013-4-8 16:10:04 
                    ucNursingRecordTable1.SetDaysAfterSurgery(inputdate, m_currInpatient.NoOfFirstPage.ToString());
                    ucNursingRecordTable1.CurrentOperTime = DateTime.Parse(inputdate);  //add by wyt
                    //住院天数也随时间改变 edit by ywk 二〇一二年五月十五日 20:48:22
                    ucNursingRecordTable1.SetDayInHospital(inputdate);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string temp = e.Item.Caption.Trim();
            //add by ywk 2012年11月8日10:48:36
            UCTextGroupBox ucText = new UCTextGroupBox();
            ucText.ActivateTextEditFocus();
            if (!ucText.ISFocused)
            {
                ucNursingRecordTable1.ActivateTextEditFocus();
            }


            try
            {
                SendKeys.Send(temp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //PaientStatus paientS = new PaientStatus(m_currInpatient);
            PaientStatus paientS = new PaientStatus(MethodSet.NoOfinPat,MethodSet.RecordNoofinpat);
            paientS.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间 
            paientS.TopMost = true;
            paientS.ShowDialog();
        }

        public bool IsChagedPat = false;//用于判断是否从选择病人窗体跳转
        public string NoOfInpat = string.Empty;//用于存放病人首页序号
        public string InputDate = string.Empty;//用于存放跳转前的日期
        public string MyPatID = string.Empty;//用于存PATID
        /// <summary>
        /// 切换病人
        /// edit by ywk 2012年5月29日 13:22:03
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkchangePat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePatient chageP = new ChangePatient(MethodSet.App, MethodSet.App.User);
            chageP.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间 
            InputDate = dateEdit.DateTime.Date.ToString("yyyy-MM-dd");
            chageP.TopMost = true;
            if (chageP.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IsChagedPat = true;
                NoOfInpat = chageP.NOOfINPAT;
                //MessageBox.Show(chageP.NOOfINPAT);
                NursingRecord_Load(null, null);
                dateEdit_DateTimeChanged(null, null);
            }
        }

        /// <summary>
        /// 切换病人显示
        /// </summary>
        /// <param name="hasVisible"></param>
        public void SetQieHuanInpatVisible(bool hasVisible)
        {
            linkchangePat.Visible = hasVisible;
        }
    }
}