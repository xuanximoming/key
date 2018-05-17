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

namespace DrectSoft.Core.NursingDocuments
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
            if (IsChagedPat)//是跳转来的 
            {
                dt = MethodSet.GetRedactPatientInfoFrm("14", "", NoOfInpat);
            }
            else
            {
                dt = MethodSet.GetRedactPatientInfoFrm("14", "", MethodSet.CurrentInPatient.NoOfFirstPage.ToString());
            }
            if (dt.Rows.Count == 1)
            {
                MethodSet.AdmitDate = dt.Rows[0]["AdmitDate"].ToString().Trim();
                MethodSet.PatName = dt.Rows[0]["Name"].ToString().Trim();//新增的
                MethodSet.OutHosDate = dt.Rows[0]["OutHosDate"].ToString().Trim();
                MethodSet.PatID = dt.Rows[0]["PatID"].ToString();
                MethodSet.OutHosDate = dt.Rows[0]["OutWardDate"].ToString().Trim();
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
                //txtInpatName.Text = MethodSet.CurrentInPatient.Name;
                txtInpatName.Text = MethodSet.PatName;
                //新增的床号显示
                txtBedID.Text = MethodSet.BedID;
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
            //设置手术后天数
            string inputdate = dateEdit.Text;
            ucNursingRecordTable1.SetDaysAfterSurgery(inputdate, MethodSet.PatID);
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
            ucNursingRecordTable1.SaveUCNursingRecordTable(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"),NoOfInpat);
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
                        btnSave.Enabled = false;
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
                    btnSave.Enabled = false;
                }

                //此处为加载病人的体征录入信息（现在病人首页序号要判断下是否已经切换了病人）
                ucNursingRecordTable1.GetNotesOfNursingInfo(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"), NoOfInpat);
                //ucNursingRecordTable1.OperateLookUp(dateEdit.DateTime.Date.ToString("yyyy-MM-dd"));
                //住院天数也随时间改变 edit by ywk 2012年5月18日 10:16:03
                string inputdate = string.Format("{0:yyyy-MM-dd}", dateEdit.DateTime.Date.ToString("yyyy-MM-dd"));

                ucNursingRecordTable1.SetDaysAfterSurgery(inputdate,MethodSet.PatID);
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
        /// <summary>
        /// 患者状态信息维护
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// edit by ywk 2012年5月29日 12:06:54
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