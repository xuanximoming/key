using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Convertmy = YidanSoft.Core.UtilsForExtension;
using YidanSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class UCOthers : UserControl
    {
        private IemMainPageInfo m_IemInfo;
        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                m_IemInfo = new IemMainPageInfo();
                GetUI();
                return m_IemInfo;
            }
        }
        private IYidanEmrHost m_App;


        public UCOthers()
        {
            InitializeComponent();
        }

        private void GetFeeData()
        {
            if (m_IemInfo != null)
            {
                //lblTotal.Text
            }
        }


        /// <summary>
        /// FILL UI
        /// </summary>
        /// <param name="info"></param>
        /// <param name="app"></param>
        public void FillUI(IemMainPageInfo info, IYidanEmrHost app)
        {
            m_App = app;
            m_IemInfo = info;

            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner();
        }

        delegate void FillUIDelegate();
        private void FillUIInner()
        {
            IemMainPageInfo info = m_IemInfo;
            #region
            if (info.IemBasicInfo.Iem_Mainpage_NO == 0)
            {
                //to do 病患基本信息
            }
            else
            {
                //第一案例
                if (m_IemInfo.IemBasicInfo.Is_First_Case == 1)
                    chkIsFirstCase1.Checked = true;
                if (m_IemInfo.IemBasicInfo.Is_First_Case == 2)
                    chkIsFirstCase2.Checked = true;

                //随诊
                if (m_IemInfo.IemBasicInfo.Is_Following == 1)
                    chkIsFollowing1.Checked = true;
                if (m_IemInfo.IemBasicInfo.Is_Following == 2)
                    chkIsFollowing2.Checked = true;

                //示教
                if (m_IemInfo.IemBasicInfo.Is_Teaching_Case == 1)
                    chkIsTeachingCase1.Checked = true;
                if (m_IemInfo.IemBasicInfo.Is_Teaching_Case == 2)
                    chkIsTeachingCase2.Checked = true;

                //血型
                if (m_IemInfo.IemBasicInfo.Blood_Type_id == 1)
                    chkBlood1.Checked = true;
                if (m_IemInfo.IemBasicInfo.Blood_Type_id == 2)
                    chkBlood2.Checked = true;
                if (m_IemInfo.IemBasicInfo.Blood_Type_id == 3)
                    chkBlood3.Checked = true;
                if (m_IemInfo.IemBasicInfo.Blood_Type_id == 4)
                    chkBlood4.Checked = true;
                if (m_IemInfo.IemBasicInfo.Blood_Type_id == 5)
                    chkBlood5.Checked = true;


                //RH
                if (m_IemInfo.IemBasicInfo.Rh == 0)
                    chkRh1.Checked = true;
                if (m_IemInfo.IemBasicInfo.Rh == 1)
                    chkRh2.Checked = true;
                if (m_IemInfo.IemBasicInfo.Rh == 2)
                    chkRh3.Checked = true;


                //输血反应
                if (m_IemInfo.IemBasicInfo.Blood_Reaction_Id == 0)
                    chkBloodReaction1.Checked = true;
                if (m_IemInfo.IemBasicInfo.Blood_Reaction_Id == 1)
                    chkBloodReaction2.Checked = true;
                if (m_IemInfo.IemBasicInfo.Blood_Reaction_Id == 2)
                    chkBloodReaction3.Checked = true;

                //输血
                seRbc.Value = Convertmy.ToDecimal(m_IemInfo.IemBasicInfo.Blood_Rbc);
                sePlt.Value = Convertmy.ToDecimal(m_IemInfo.IemBasicInfo.Blood_Plt);
                sePlasma.Value = Convertmy.ToDecimal(m_IemInfo.IemBasicInfo.Blood_Plasma);
                seWb.Value = Convertmy.ToDecimal(m_IemInfo.IemBasicInfo.Blood_Wb);
                txtOthers.Text = m_IemInfo.IemBasicInfo.Blood_Others;

                // 随诊
                if (!String.IsNullOrEmpty(m_IemInfo.IemBasicInfo.Following_Ending_Date))
                {
                    for (int i = 0; i < m_IemInfo.IemBasicInfo.Following_Ending_Date.Split('-').Length; i++)
                    {
                        if (i == 0)
                            txtIsFollowingDay.Text = m_IemInfo.IemBasicInfo.Following_Ending_Date.Split('-')[i];
                        else if (i == 1)
                            txtIsFollowingMon.Text = m_IemInfo.IemBasicInfo.Following_Ending_Date.Split('-')[i];
                        else if (i == 2)
                            txtIsFollowingYear.Text = m_IemInfo.IemBasicInfo.Following_Ending_Date.Split('-')[i];
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            //第一案例
            if (chkIsFirstCase1.Checked == true)
                m_IemInfo.IemBasicInfo.Is_First_Case = 1;
            if (chkIsFirstCase2.Checked == true)
                m_IemInfo.IemBasicInfo.Is_First_Case = 2;

            //随诊
            if (chkIsFollowing1.Checked == true)
                m_IemInfo.IemBasicInfo.Is_Following = 1;
            if (chkIsFollowing2.Checked == true)
                m_IemInfo.IemBasicInfo.Is_Following = 2;

            //示教
            if (chkIsTeachingCase1.Checked == true)
                m_IemInfo.IemBasicInfo.Is_Teaching_Case = 1;
            if (chkIsTeachingCase2.Checked == true)
                m_IemInfo.IemBasicInfo.Is_Teaching_Case = 2;

            //血型
            if (chkBlood1.Checked == true)
                m_IemInfo.IemBasicInfo.Blood_Type_id = 1;
            if (chkBlood2.Checked == true)
                m_IemInfo.IemBasicInfo.Blood_Type_id = 2;
            if (chkBlood3.Checked == true)
                m_IemInfo.IemBasicInfo.Blood_Type_id = 3;
            if (chkBlood4.Checked == true)
                m_IemInfo.IemBasicInfo.Blood_Type_id = 4;
            if (chkBlood5.Checked == true)
                m_IemInfo.IemBasicInfo.Blood_Type_id = 5;


            //RH
            if (chkRh1.Checked == true)
                m_IemInfo.IemBasicInfo.Rh = 0;
            if (chkRh2.Checked == true)
                m_IemInfo.IemBasicInfo.Rh = 1;
            if (chkRh3.Checked == true)
                m_IemInfo.IemBasicInfo.Rh = 2;


            //输血反应
            if (chkBloodReaction1.Checked == true)
                m_IemInfo.IemBasicInfo.Blood_Reaction_Id = 0;
            if (chkBloodReaction2.Checked == true)
                m_IemInfo.IemBasicInfo.Blood_Reaction_Id = 1;
            if (chkBloodReaction3.Checked == true)
                m_IemInfo.IemBasicInfo.Blood_Reaction_Id = 2;

            //输血
            m_IemInfo.IemBasicInfo.Blood_Rbc = seRbc.Value;
            m_IemInfo.IemBasicInfo.Blood_Plt = sePlt.Value;
            m_IemInfo.IemBasicInfo.Blood_Plasma = sePlasma.Value;
            m_IemInfo.IemBasicInfo.Blood_Wb = seWb.Value;
            m_IemInfo.IemBasicInfo.Blood_Others = txtOthers.Text.Trim();

            // 随诊
            m_IemInfo.IemBasicInfo.Following_Ending_Date = txtIsFollowingDay.Text.Trim() + "-" + txtIsFollowingMon.Text.Trim() + "-" + txtIsFollowingYear.Text;

        }

        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFee_Click(object sender, EventArgs e)
        {
            if (m_App == null || m_App.CurrentPatientInfo == null)
                return;
            IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");

            if (sqlHelper == null)
            {
                m_App.CustomMessageBox.MessageShow("无法连接到HIS！", CustomMessageBoxKind.ErrorOk);
                return;
            }

            SqlParameter[] paraColl = new SqlParameter[] { new SqlParameter("@syxh", 26) };
            DataTable dataTable = sqlHelper.ExecuteDataTable("usp_bq_fymxcx", paraColl, CommandType.StoredProcedure);

            //to do 赋值
            //to do 赋值
            foreach (DataRow row in dataTable.Rows)
            {
                if (row[0].ToString() == "F")
                    return;
                if (row["项目名称"].ToString().Trim() == "其它")
                {
                    lblOthers.Text = row["项目金额"].ToString();
                }
                else if (row["项目名称"].ToString().Trim() == "西药费")
                {
                    lblWMedical.Text = row["项目金额"].ToString();
                }
                else if (row["项目名称"].ToString().Trim() == "治疗费")
                {
                    labelZhiLiao.Text = row["项目金额"].ToString();
                }
                else if (row["项目名称"].ToString().Trim() == "检查费")
                {
                    lblRis.Text = row["项目金额"].ToString();
                }
                else if (row["项目名称"].ToString().Trim() == "检验费")
                {
                    lblAssay.Text = row["项目金额"].ToString();
                }
                else if (row["项目名称"].ToString().Trim() == "诊疗费")
                {
                    lblMecical.Text = row["项目金额"].ToString();
                }
                else if (row["项目名称"].ToString().Trim() == "床位费")
                {
                    lblBed.Text = row["项目金额"].ToString();
                }
                else if (row["项目名称"].ToString().Trim() == "护士护理费")
                {
                    lblCare.Text = row["项目金额"].ToString();
                }
                else if (row["项目名称"].ToString().Trim() == "总计")
                {
                    lblTotal.Text = row["项目金额"].ToString();
                    if (Convertmy.ToDecimal(lblTotal.Text) != 0)
                    {
                        try
                        {
                            lblRate.Text = Convertmy.ToString(Convertmy.ToDecimal(lblWMedical.Text) / Convertmy.ToDecimal(lblTotal.Text) * 100).Substring(0, 2) + "%";
                        }
                        catch
                        {
                            lblRate.Text = "0";
                        }
                    }
                }
                else if (row["项目名称"].ToString().Trim() == "押金累计")
                {
                    row["项目金额"].ToString();
                }
                //费比
            }

        }

        private void UCOthers_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextEdit)
                {
                    e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                        new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                }
            }

            e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
            e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
            e.Graphics.DrawLine(Pens.Black, new Point(0, this.Height - 1), new Point(this.Width, this.Height - 1));
        }

        #region 获取打印病案首页 费用信息板块信息

        public Print_Iem_MainPage_Fee GetPrintFee()
        {
            Print_Iem_MainPage_Fee _Print_Iem_MainPage_Fee = new Print_Iem_MainPage_Fee();

            _Print_Iem_MainPage_Fee.Total = lblTotal.Text;
            _Print_Iem_MainPage_Fee.Bed = lblBed.Text;
            _Print_Iem_MainPage_Fee.Care = lblCare.Text;
            _Print_Iem_MainPage_Fee.WMedical = lblWMedical.Text;
            _Print_Iem_MainPage_Fee.CPMedical = lblCPMedical.Text;

            _Print_Iem_MainPage_Fee.CMedical = lblCMedical.Text;
            _Print_Iem_MainPage_Fee.Radiate = lblRadiate.Text;
            _Print_Iem_MainPage_Fee.Assay = lblAssay.Text;
            _Print_Iem_MainPage_Fee.Ox = lblOx.Text;
            _Print_Iem_MainPage_Fee.Blood = lblBlood.Text;

            _Print_Iem_MainPage_Fee.Mecical = lblMecical.Text;
            _Print_Iem_MainPage_Fee.Operation = lblOperation.Text;
            _Print_Iem_MainPage_Fee.Accouche = lblAccouche.Text;
            _Print_Iem_MainPage_Fee.Ris = lblRis.Text;
            _Print_Iem_MainPage_Fee.Anaesthesia = lblAnaesthesia.Text;

            _Print_Iem_MainPage_Fee.Baby = lblBaby.Text;
            _Print_Iem_MainPage_Fee.FollwBed = lblFollwBed.Text;
            _Print_Iem_MainPage_Fee.Others1 = lblOthers.Text;
            _Print_Iem_MainPage_Fee.Others2 = labelZhiLiao.Text;
            _Print_Iem_MainPage_Fee.Others3 = lblRate.Text;

            _Print_Iem_MainPage_Fee.Ashes_Check = m_IemInfo.IemBasicInfo.Ashes_Diagnosis_Name != "" ? "1" : "2";

            _Print_Iem_MainPage_Fee.IsFirstCase = m_IemInfo.IemBasicInfo.Is_First_Case.ToString();
            _Print_Iem_MainPage_Fee.IsFollowing = m_IemInfo.IemBasicInfo.Is_Following.ToString();
            _Print_Iem_MainPage_Fee.IsFollowingDay = txtIsFollowingDay.Text;
            _Print_Iem_MainPage_Fee.IsFollowingMon = txtIsFollowingMon.Text;
            _Print_Iem_MainPage_Fee.IsFollowingYear = txtIsFollowingYear.Text;

            _Print_Iem_MainPage_Fee.IsTeachingCase = m_IemInfo.IemBasicInfo.Is_Teaching_Case.ToString();
            _Print_Iem_MainPage_Fee.BloodType = m_IemInfo.IemBasicInfo.Blood_Type_id.ToString();
            _Print_Iem_MainPage_Fee.Rh = m_IemInfo.IemBasicInfo.Rh.ToString();
            _Print_Iem_MainPage_Fee.BloodReaction = m_IemInfo.IemBasicInfo.Blood_Reaction_Id.ToString();
            _Print_Iem_MainPage_Fee.Rbc = m_IemInfo.IemBasicInfo.Blood_Rbc.ToString();

            _Print_Iem_MainPage_Fee.Plt = m_IemInfo.IemBasicInfo.Blood_Plt.ToString();
            _Print_Iem_MainPage_Fee.Plasma = m_IemInfo.IemBasicInfo.Blood_Plasma.ToString();
            _Print_Iem_MainPage_Fee.Wb = m_IemInfo.IemBasicInfo.Blood_Wb.ToString();
            _Print_Iem_MainPage_Fee.Others = m_IemInfo.IemBasicInfo.Blood_Others.ToString();


            return _Print_Iem_MainPage_Fee;
        }

        #endregion
    }
}
