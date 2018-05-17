using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraBars;
using DrectSoft.Wordbook;
using DrectSoft.Core.NurseDocument;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.NurseDocument.Controls
{
    public partial class NursingRecord : DevBaseForm
    {

        string[] m_DaysAfterSurgery;
        string m_currentInpatient;
        private string m_InTime = "";//入科时间
        public string m_outTime = "";

        public bool IsChagedPat = false;//用于判断是否从选择病人窗体跳转
        public string NoOfInpat = string.Empty;//用于存放病人首页序号
        public string InputDate = string.Empty;//用于存放跳转前的日期
        public string MyPatID = string.Empty;//用于存PATID
      
        /// <summary>
        /// NursingDocuments工程调用该构造函数
        /// </summary>
        public NursingRecord()
        {
            InitializeComponent();
        }

        public void RefreshForm(string curPat)
        {
            try
            {
                ucNursingRecordTable1.RefreshFormControls(curPat);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 非NursingDocuments工程调用该窗体，用该构造函数
        /// </summary>
        /// <param name="App">应用程序对象接口</param>
        public NursingRecord(IEmrHost App, string currentInpatient)
        {
            try
            {
                m_currentInpatient = currentInpatient;
                InitializeComponent();
                m_DaysAfterSurgery = null;
                MethodSet.App = App;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public DialogResult ShowNursingRecord()
        {
            try
            {
                return ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 模态调用窗口（）
        /// </summary>
        /// <param name="DaysAfterSurgery"></param>
        /// <returns></returns>
        public DialogResult ShowNursingRecord(string[] DaysAfterSurgery)
        {
            try
            {
                m_DaysAfterSurgery = DaysAfterSurgery;
                return ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化病人基本信息
        /// </summary>
        private void InitInpatInfo(string currentInpatient)
        {
            try
            {
                DataTable dt = null;
                if (IsChagedPat)//是跳转来的 
                {
                    dt = MethodSet.GetRedactPatientInfoFrm("14", "", NoOfInpat);
                }
                else
                {
                    NoOfInpat = currentInpatient;
                    dt = MethodSet.GetRedactPatientInfoFrm("14", "", currentInpatient);
                }
                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["inwarddate"].ToString() != "" && dt.Rows[0]["inwarddate"].ToString() != null)
                    {
                        m_InTime = dt.Rows[0]["inwarddate"].ToString();
                    }
                    else if (dt.Rows[0]["ADMITDATE"].ToString() != null && dt.Rows[0]["ADMITDATE"].ToString() != "")
                    {
                        m_InTime = dt.Rows[0]["ADMITDATE"].ToString();
                    }
                    else
                    {
                        throw new Exception("入院和入科时间不能同时为空");
                    }
                    m_outTime = dt.Rows[0]["OUTWARDDATE"].ToString().Trim();
                    MethodSet.AdmitDate = dt.Rows[0]["AdmitDate"].ToString().Trim();
                    MethodSet.PatName = dt.Rows[0]["Name"].ToString().Trim();//新增的
                    MethodSet.OutHosDate = dt.Rows[0]["OutHosDate"].ToString().Trim();
                    MethodSet.PatID = dt.Rows[0]["PatID"].ToString();
                    MyPatID = dt.Rows[0]["PatID"].ToString();
                    if (dt.Rows[0]["isbaby"].ToString().Equals("1"))
                    {
                        txtPatID.Text = new DataLoader().GetMotherPatid(dt.Rows[0]["mother"].ToString());
                    }
                    else
                    {
                        txtPatID.Text = MethodSet.PatID;
                    }
                    
                    MethodSet.OutHosDate = dt.Rows[0]["OutWardDate"].ToString().Trim();
                    MethodSet.BedID = dt.Rows[0]["OutBed"].ToString().Trim();//新增的
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NursingRecord_Load(object sender, EventArgs e)
        {
            RefreshDate(m_currentInpatient);
        }

        public void RefreshDate(string m_currentInpatient)
        {
            try
            {
                MethodSet.DaysAfterSurgery = m_DaysAfterSurgery;

                if (m_currentInpatient != null)
                {
                    InitInpatInfo(m_currentInpatient);
                    //txtPatID.Text = MethodSet.PatID;
                    //txtInpatName.Text = MethodSet.CurrentInPatient.Name;
                    txtInpatName.Text = MethodSet.PatName;
                    //新增的床号显示
                    txtBedID.Text = MethodSet.BedID;
                }
                else
                {
                    btnSave.Enabled = false;
                }

                if (IsChagedPat)//是跳转来的
                {
                    dateEdit.Text = InputDate;
                }
                else
                {
                    dateEdit.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                }
                //设置手术后天数
                //string inputdate = dateEdit.Text;
               // ucNursingRecordTable1.SetDaysAfterSurgery(inputdate, MethodSet.PatID);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
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
                if (DateTime.Parse(dateEdit.DateTime.Date.ToString("yyyy-MM-dd")) >= DateTime.Parse(DateTime.Parse(m_InTime).ToString("yyyy-MM-dd")))
                {
                    //if (m_outTime.Trim() != "")
                    //{
                    //    if (DateTime.Parse(dateEdit.DateTime.Date.ToString("yyyy-MM-dd")) >= DateTime.Parse(DateTime.Parse(m_outTime).ToString("yyyy-MM-dd")))
                    //    {

                    //        dateEdit.Focus();
                    //        MessageBox.Show("录入数据日期应该小于病人出区日期");
                    //    }
                    //    else 
                    //    {
                           
                    //        if (ucNursingRecordTable1.SaveUCNursingRecordTable(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat))
                    //        {
                    //            //this.Close();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                       
                        if (ucNursingRecordTable1.SaveUCNursingRecordTable(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat))
                        {
                            //this.Close();
                        }
                   // }
                }
                else
                {
                    dateEdit.Focus();
                    MessageBox.Show("录入数据日期应该大于病人入区日期");

                }

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        public void dateEdit_DateTimeChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_currentInpatient != null)
                {
                    //btnSave.Enabled = true;

                    string CurrDateTime = MethodSet.GetCurrServerDateTime.Date.ToString("yyyy-MM-dd 00:00:01");

                    //int Day = Convert.ToDateTime(CurrDateTime).
                    //Subtract(Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:01"))).Days + 1;

                    //btnSave.Enabled = (Day <= ucNursingRecordTable1.DayOfModify || ucNursingRecordTable1.DayOfModify == -1) ? true : false;

                    //判断服务器当前日期是否小于入院日期
                    if (!string.IsNullOrEmpty(MethodSet.AdmitDate))
                    {
                        DateTime m_AdmitDate = Convert.ToDateTime(MethodSet.AdmitDate);
                        if (Convert.ToDateTime(m_AdmitDate.Date.ToString("yyyy-MM-dd 00:00:00"))
                            > Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:00")))
                        {
                            btnSave.Enabled = false;
                        }
                        else
                        {
                            btnSave.Enabled = true;
                        }
                    }

                    //判断选择日期是否超出服务器当前日前
                    if (Convert.ToDateTime(CurrDateTime) < Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:00")))
                    {
                        btnSave.Enabled = false;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                    }

                    //此处为加载病人的体征录入信息（现在病人首页序号要判断下是否已经切换了病人）
                    ucNursingRecordTable1.GetNotesOfNursingInfo(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat);
                    //ucNursingRecordTable1.OperateLookUp(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"));
                    //住院天数也随时间改变 edit by ywk 2012年5月18日 10:16:03
                    string inputdate = string.Format("{0:yyyy-MM-dd}", dateEdit.DateTime.Date.ToString("yyyy-MM-dd"));

                    //ucNursingRecordTable1.SetDaysAfterSurgery(inputdate, m_currentInpatient.NoOfFirstPage.ToString());
                    //if (ConfigInfo.editable == 1)
                    //{
                    //    ucNursingRecordTable1.SetDaysAfterSurgery(dateEdit.Text, NoOfInpat);//zyx 解决手术后天数不显示问题
                    //}
                    //else 
                    //{
                    //    ucNursingRecordTable1.GetDaysAfterSurgery(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat);
                    //}
                    //ucNursingRecordTable1.SetDayInHospital(inputdate);
                }
            }
            catch (Exception ex)
            { 
                MyMessageBox.Show(1, ex); 
            }
        }

        private void barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string temp = e.Item.Caption.Trim();
            //ucNursingRecordTable1.ActivateTextEditFocus();
            try
            {
                SendKeys.Send(temp);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 患者状态信息维护
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientStateClicked(object sender, EventArgs e)
        {
            try
            {
                PaientStatus paientS = new PaientStatus(MyPatID, m_InTime,m_outTime, NoOfInpat);
                paientS.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间 
                paientS.ShowDialog();
                paientS.TopMost = true;
                this.dateEdit_DateTimeChanged(null, null);//刷新手术时间
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 切换病人
        /// edit by ywk 2012年5月29日 12:06:54
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChagePatientClicked(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 切换病人显示
        /// </summary>
        /// <param name="hasVisible"></param>
        public void SetQieHuanInpatVisible(bool hasVisible)
        {
            try
            {
                btnChangePatient.Visible = hasVisible;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}