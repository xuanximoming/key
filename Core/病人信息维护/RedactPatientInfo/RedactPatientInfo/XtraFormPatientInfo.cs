using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.RedactPatientInfo
{
    public partial class XtraFormPatientInfo : DevExpress.XtraEditors.XtraForm
    {
        IEmrHost m_app;

        string m_NoOfInpat;
        public XtraFormPatientInfo()
        {
            InitializeComponent();
        }

        public XtraFormPatientInfo(IEmrHost app, string noOfInpat)
            : this()
        {
            m_app = app;
            m_NoOfInpat = noOfInpat;
        }

        private void XtraFormPatientInfo_Load(object sender, EventArgs e)
        {
            try
            {
                string sqlEmr = " select patnoofhis from inpatient where noofinpat = '{0}' ";
                string patnoofhis = m_app.SqlHelper.ExecuteScalar(string.Format(sqlEmr, m_NoOfInpat)).ToString();
                DataTable dt = new DataTable();
                //从his查病人信息功能
                if (DS_SqlService.GetConfigValueByKey("GetInpatientForHis") == "1")
                {
                    IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");

                    if (sqlHelper == null)
                    {
                        m_app.CustomMessageBox.MessageShow("无法连接到HIS", CustomMessageBoxKind.ErrorOk);
                        return;
                    }
                    string sqlHis = " select * from zc_inpatient where zc_inpatient.patnoofhis = '{0}' ";
                    dt = sqlHelper.ExecuteDataTable(string.Format(sqlHis, patnoofhis), CommandType.Text);
                }
                else
                {
                    dt = m_app.SqlHelper.ExecuteDataTable(string.Format(" select * from inpatient where noofinpat = '{0}' ", m_NoOfInpat), CommandType.Text);
                }

                InitPatientInfo(dt);
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message;
                m_app.CustomMessageBox.MessageShow("调用HIS时出错，请联系管理员！", CustomMessageBoxKind.ErrorOk);
            }
        }

        private void InitPatientInfo(DataTable dt)
        {
            if (dt.Rows.Count == 1)
            {
                txtName.Text = dt.Rows[0]["name"].ToString();
                textEditSex.Text = dt.Rows[0]["sexname"].ToString();
                textEditBirthDate.Text = dt.Rows[0]["birth"].ToString();
                txtAge.Text = dt.Rows[0]["agestr"].ToString();
                textEditMariage.Text = dt.Rows[0]["mari"].ToString();
                textEditProvince.Text = dt.Rows[0]["provincename"].ToString();
                textEditCounty.Text = dt.Rows[0]["countyname"].ToString();
                textEditNation.Text = dt.Rows[0]["nationname"].ToString();
                textEditNationality.Text = dt.Rows[0]["nationalityname"].ToString();
                txtIDCard.Text = dt.Rows[0]["idno"].ToString();
                textEditJobName.Text = dt.Rows[0]["jobname"].ToString();
                txtOrganization.Text = dt.Rows[0]["organization"].ToString();
                txtOfficeTel.Text = dt.Rows[0]["officetel"].ToString();
                txtOfficePostalCode.Text = dt.Rows[0]["officepost"].ToString();
                txtHousehold.Text = dt.Rows[0]["nativeaddress"].ToString();
                txtTel.Text = dt.Rows[0]["nativetel"].ToString();
                txtPostalCode.Text = dt.Rows[0]["nativepost"].ToString();
                txtAddress.Text = dt.Rows[0]["address"].ToString();
                textEditContactPerson.Text = dt.Rows[0]["contactperson"].ToString();
                textEditRelationShip.Text = dt.Rows[0]["rela_name"].ToString();
                textEditContactAddress.Text = dt.Rows[0]["contactaddress"].ToString();
                textEditContactTEL.Text = dt.Rows[0]["contacttel"].ToString();
                textEditNoOfClinic.Text = dt.Rows[0]["noofclinic"].ToString();
                textEditNoOfRecord.Text = dt.Rows[0]["noofrecord"].ToString();
                textEditInCount.Text = dt.Rows[0]["incount"].ToString();
                textEditPay.Text = dt.Rows[0]["pact_name"].ToString();
                textEditOrigin.Text = dt.Rows[0]["source_name"].ToString();
                textEditAdmitWay.Text = dt.Rows[0]["admitwayname"].ToString();
                textEditOutWay.Text = dt.Rows[0]["outwayname"].ToString();
                textEditClinicDoctor.Text = dt.Rows[0]["clinicdoctorname"].ToString();
                textEditResident.Text = dt.Rows[0]["residentname"].ToString();
                textEditAttend.Text = dt.Rows[0]["attendname"].ToString();
                textEditChief.Text = dt.Rows[0]["chiefname"].ToString();
                textEditStatus.Text = dt.Rows[0]["statusname"].ToString();
                textEditCriticalLevel.Text = dt.Rows[0]["criticallevelname"].ToString();
                textEditAttendLevel.Text = dt.Rows[0]["attendlevelname"].ToString();
            }
        }

        /// <summary>
        /// 设置首页序号
        /// </summary>
        public DialogResult ShowCurrentPatInfo()
        {
            m_NoOfInpat = m_app.CurrentPatientInfo.NoOfFirstPage.ToString();
            return ShowDialog();
        }

        /// <summary>
        /// 设置首页序号
        /// </summary>
        /// <param name="NoOfInpat">首页序号</param>
        public DialogResult ShowCurrentPatInfo(string NoOfInpat)
        {
            m_NoOfInpat = NoOfInpat;
            return ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 回车切换焦点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
    }
}