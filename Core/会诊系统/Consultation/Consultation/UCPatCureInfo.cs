using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.Consultation
{
    /// <summary>
    /// 展示患者诊疗时间轴的界面
    /// add by ywk 2013年7月23日 18:32:19
    /// </summary>
    public partial class UCPatCureInfo : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        #region 属性
        private string m_Noofinpat = "";
        Inpatient CurrentInpatient;
        IEmrHost m_app;
        #endregion

        #region 事件 add by 杨伟康 2013年7月23日 21:40:06
        private void UCPatCureInfo_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        ///  住院志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picZYZ_MouseEnter(object sender, EventArgs e)
        {
            this.picZYZ.ToolTip = "住院志共" + GetRecordBySortID(m_Noofinpat, "AB");
        }
        /// <summary>
        /// 病程记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picBCJL_MouseEnter(object sender, EventArgs e)
        {
            this.picBCJL.ToolTip = "病程记录共" + GetRecordBySortID(m_Noofinpat, "AC");
        }
        /// <summary>
        /// 知情文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picZQWJ_MouseEnter(object sender, EventArgs e)
        {
            this.picZQWJ.ToolTip = "知情文件共" + GetRecordBySortID(m_Noofinpat, "AD");
        }
        /// <summary>
        /// 
        /// 其他记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picQTJL_MouseEnter(object sender, EventArgs e)
        {
            this.picQTJL.ToolTip = "其他记录共" + GetRecordBySortID(m_Noofinpat, "AE");
        }
        /// <summary>
        /// 会诊记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picHZJL_MouseEnter(object sender, EventArgs e)
        {
            this.picHZJL.ToolTip = "会诊记录共" + GetRecordBySortID(m_Noofinpat, "AL");
        }
        /// <summary>
        /// 手术记录 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picSSJL_MouseEnter(object sender, EventArgs e)
        {
            this.picSSJL.ToolTip = "手术记录共" + GetRecordBySortID(m_Noofinpat, "AH");
        }
        /// <summary>
        /// 入院
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picInHop_MouseEnter(object sender, EventArgs e)
        {
            this.picInHop.ToolTip = "入院时间:" + CurrentInpatient.PersonalInformation.InHosDate;
        }
        /// <summary>
        /// 出院 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picOutHop_MouseEnter(object sender, EventArgs e)
        {
            DataTable dtPatInfo = DrectSoft.Service.DS_SqlService.GetInpatientByID(Int32.Parse(m_Noofinpat), 2);
            this.picOutHop.ToolTip = "出院时间:" + dtPatInfo.Rows[0]["OUTHOSDATE"].ToString();
        }
        /// <summary>
        /// /根据首页序号和病历分类获取共有多少病历文件信息 
        /// </summary>
        /// <param name="m_Noofinpat"></param>
        /// <param name="sortid"></param>
        /// <returns></returns>
        private string GetRecordBySortID(string m_Noofinpat, string sortid)
        {
            string tooltipcontent = "条:";
            string sqlserach = string.Format(@"select name from recorddetail where 
noofinpat='{0}' and sortid ='{1}' and valid=1 ", m_Noofinpat, sortid);
            int recordcount = 0;//此类型病历共几条
            switch (sortid)
            {
                case "AL":
                    //会诊单独处理
                    recordcount = DS_SqlService.GetConsultRecrod(Int32.Parse(m_Noofinpat)).Rows.Count;
                    break;
                default:
                    break;
            }
            DataTable dtPatRecordData = m_app.SqlHelper.ExecuteDataTable(sqlserach);
            if (dtPatRecordData != null && dtPatRecordData.Rows.Count > 0)
            {
                recordcount = dtPatRecordData.Rows.Count;
                tooltipcontent = recordcount.ToString() + tooltipcontent;
                for (int i = 0; i < dtPatRecordData.Rows.Count; i++)
                {
                    tooltipcontent += dtPatRecordData.Rows[i]["name"].ToString() + "\r\n";
                }
            }
            if (dtPatRecordData.Rows.Count == 0)
            {
                tooltipcontent = "0条记录";
            }
            return tooltipcontent;
        }
        #endregion

        #region 方法
        public UCPatCureInfo()
        {
            InitializeComponent();
        }
        public UCPatCureInfo(string noofinpat)
        {
            InitializeComponent();
            m_Noofinpat = noofinpat;
        }

        private void PanelControlInit()
        {
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
        }
        #endregion

        #region 实现接口
        public string CurrentNoofinpat
        {
            get;
            set;
        }

        public Control DesignUI
        {
            get { return this; }
        }

        public new void Load(IEmrHost app)
        {
            //DataAccess.App = app;
            m_app = app;
            if (!string.IsNullOrEmpty(CurrentNoofinpat))
            {
                CurrentInpatient = new DrectSoft.Common.Eop.Inpatient(Convert.ToDecimal(CurrentNoofinpat));
            }
            else if (m_app.CurrentPatientInfo != null)
            {
                CurrentInpatient = m_app.CurrentPatientInfo;
            }
            else
            {
                return;
            }
            CurrentInpatient.ReInitializeAllProperties();
            PanelControlInit();
            InitControl();
        }
        /// <summary>
        /// 初始化各基本信息栏位的值
        /// </summary>
        private void InitControl()
        {

            DataTable dtPatInfo = DrectSoft.Service.DS_SqlService.GetInpatientByID(Int32.Parse(m_Noofinpat), 2);
            if (dtPatInfo != null && dtPatInfo.Rows.Count > 0)
            {
                textEditName.Text = dtPatInfo.Rows[0]["name"].ToString();
                textEditPatientSN.Text = dtPatInfo.Rows[0]["patid"].ToString();
                textEditGender.Text = dtPatInfo.Rows[0]["sexname"].ToString();
                textEditAge.Text = dtPatInfo.Rows[0]["agestr"].ToString();
                textEditBedCode.Text = dtPatInfo.Rows[0]["outbed"].ToString();
                textEditDepartment.Text = dtPatInfo.Rows[0]["deptname"].ToString();
                textEdit1.Text = dtPatInfo.Rows[0]["wardname"].ToString();
                //textEditBedCode.Text=CurrentInpatient.g
            }
            //CurrentInpatient = Service.DS_SqlService.GetPatientInfo(CurrentInpatient.Code.ToString());
            //if (CurrentInpatient!=null)
            //{
            //    textEditName.Text = CurrentInpatient.Name;
            //    textEditPatientSN.Text = CurrentInpatient.PersonalInformation.IdentificationNo;
            //    textEditGender.Text = CurrentInpatient.PersonalInformation.Sex.ToString();
            //    textEditAge.Text = CurrentInpatient.PersonalInformation.CurrentDisplayAge;
            //    textEditBedCode.Text=CurrentInpatient.g

            //}
        }

        public void Print()
        {
            throw new NotImplementedException();
        }
        private bool m_ReadOnlyControl = false;
        public bool ReadOnlyControl
        {
            get { return m_ReadOnlyControl; }
            set { m_ReadOnlyControl = value; }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public string Title
        {
            get { return "患者诊疗时间轴"; }
        }
        #endregion












    }
}
