using DrectSoft.Core.EMR_NursingDocument.NursingDocuments;
using DrectSoft.Core.EMR_NursingDocument.PublicSet;
using DrectSoft.Core.EMR_NursingDocument.UserControls;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.EMR_NursingDocument
{
    public partial class NursingRecord : DevExpress.XtraEditors.XtraForm
    {

        string[] m_DaysAfterSurgery;


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
        public NursingRecord(IEmrHost App)
        {
            InitializeComponent();
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
        private void InitInpatInfo()
        {
            DataTable dt = null;
            //此处的DataTable数据的提取，就区分开是否是切换病人后的数据
            if (IsChagedPat)//是跳转来的 
            {
                dt = MethodSet.GetRedactPatientInfoFrm("14", "", NoOfInpat);
            }
            else
            {
                dt = MethodSet.GetRedactPatientInfoFrm("14", "", MethodSet.CurrentInPatient.NoOfFirstPage.ToString());
            }
            //DataTable dt = MethodSet.GetRedactPatientInfoFrm("14", "", MethodSet.CurrentInPatient.NoOfFirstPage.ToString());
            if (dt.Rows.Count == 1)
            {
                //如果入科时间为空，则读取入院时间，否则以入科时间为准
                MethodSet.AdmitDate = string.IsNullOrEmpty(dt.Rows[0]["inwarddate"].ToString().Trim()) ?
                    dt.Rows[0]["AdmitDate"].ToString().Trim() : dt.Rows[0]["inwarddate"].ToString().Trim();

                MethodSet.OutHosDate = dt.Rows[0]["status"].ToString().Trim() == "1503" ? dt.Rows[0]["OutHosDate"].ToString().Trim() : "";
                MethodSet.PatID = dt.Rows[0]["PatID"].ToString();
                MethodSet.OutWardDate = dt.Rows[0]["status"].ToString().Trim() == "1502" ? dt.Rows[0]["OutWardDate"].ToString().Trim() : "";
                MethodSet.PatName = dt.Rows[0]["Name"].ToString().Trim();//新增的
                MethodSet.BedID = dt.Rows[0]["OutBed"].ToString().Trim();//新增的
            }
        }

        private void NursingRecord_Load(object sender, EventArgs e)
        {
            MethodSet.DaysAfterSurgery = m_DaysAfterSurgery;

            if (MethodSet.CurrentInPatient != null)
            {
                InitInpatInfo();
                txtPatID.Text = MethodSet.PatID;
                txtInpatName.Text = MethodSet.PatName;
                //新增的床号显示
                txtBedID.Text = MethodSet.BedID;
                //txtInpatName.Text = MethodSet.CurrentInPatient.Name;
                txtAge.Text = PatientInfo.Age;
            }
            else
            {
                btnSave.Enabled = false;
            }

            ucNursingRecordTable1.InitForm();

            if (IsChagedPat)//是跳转来的
            {
                dateEdit.Text = InputDate;
            }
            else
            {
                dateEdit.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
            }

            //dateEdit.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
            //体征录入界面刚进入时，进入自定义控件进行计算要显示的手术后天数  2012年5月15日 09:44:02
            //设置手术后天数
            string inputdate = dateEdit.Text;
            ucNursingRecordTable1.SetDaysAfterSurgery(inputdate, MethodSet.PatID);
            //ucNursingRecordTable1.SetDaysAfterSurgery(m_DaysAfterSurgery);
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
            ucNursingRecordTable1.SaveUCNursingRecordTable(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat);
        }


        private void dateEdit_DateTimeChanged(object sender, EventArgs e)
        {

            if (MethodSet.CurrentInPatient != null)
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
                        MethodSet.App.CustomMessageBox.MessageShow("选中日期不能小于入院日期，请重新选择日期！", CustomMessageBoxKind.InformationOk);
                        //btnSave.Enabled = false;
                        dateEdit.DateTime = m_AdmitDate;
                        dateEdit.Focus();
                        return;
                    }

                    if (!string.IsNullOrEmpty(MethodSet.OutHosDate))
                    {
                        DateTime outHosDate = Convert.ToDateTime(MethodSet.OutHosDate);
                        if (Convert.ToDateTime(outHosDate.Date.ToString("yyyy-MM-dd 00:00:00"))
                             < Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:00")))
                        {
                            MethodSet.App.CustomMessageBox.MessageShow("选中日期不能大于出院日期，请重新选择日期！", CustomMessageBoxKind.InformationOk);
                            dateEdit.DateTime = outHosDate;
                            dateEdit.Focus();
                            return;
                        }
                    }
                }

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

                //判断选择日期是否超出服务器当前日前
                if (Convert.ToDateTime(CurrDateTime) < Convert.ToDateTime(dateEdit.DateTime.Date.ToString("yyyy-MM-dd 00:00:00")))
                {
                    MethodSet.App.CustomMessageBox.MessageShow("不能提前填写，请重新选择日期！", CustomMessageBoxKind.InformationOk);
                    //btnSave.Enabled = false;
                    dateEdit.DateTime = Convert.ToDateTime(CurrDateTime);
                    dateEdit.Focus();
                    return;
                }


                ucNursingRecordTable1.GetNotesOfNursingInfo(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat);
                //ucNursingRecordTable1.OperateLookUp(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"));


                //根据选择的日期设置手术后天数 edit by ywk 2012年5月15日 09:43:56
                string inputdate = string.Format("{0:yyyy-MM-dd}", dateEdit.DateTime.Date.ToString("yyyy-MM-dd"));

                ucNursingRecordTable1.SetDaysAfterSurgery(inputdate, MethodSet.PatID);
                //住院天数也随时间改变 edit by ywk 二〇一二年五月十五日 20:48:22
                ucNursingRecordTable1.SetDayInHospital(inputdate);
            }
        }

        private void barButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string temp = e.Item.Caption.Trim();
            ucNursingRecordTable1.ActivateTextEditFocus();
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
            PaientStatus paientS = new PaientStatus(MethodSet.PatID);
            paientS.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间 
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
            if (chageP.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IsChagedPat = true;
                NoOfInpat = chageP.NOOfINPAT;
                //MessageBox.Show(chageP.NOOfINPAT);
                NursingRecord_Load(null, null);
                dateEdit_DateTimeChanged(null, null);
            }
        }




    }
}