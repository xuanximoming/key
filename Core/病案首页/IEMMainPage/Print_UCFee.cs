using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.Common.Library;
using YidanSoft.Wordbook;
using System.Data.SqlClient;
//

using Convertmy = YidanSoft.Core.UtilsForExtension;
using YidanSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraEditors;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class Print_UCFee : UserControl
    {
        IDataAccess m_SqlHelper;
        IYidanSoftLog m_Logger;
        private IemMainPageInfo m_IemInfo;
        /// <summary>
        /// 病案首页产科产妇婴儿信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                m_IemInfo = new IemMainPageInfo();
                return m_IemInfo;
            }
        }
    
        private IYidanEmrHost m_App;

        private DataTable m_DataTableDiag = null;

        public Print_UCFee(IemMainPageInfo info, IYidanEmrHost app)
        {
            InitializeComponent();
            //m_IemInfo = info;
            //m_App = app;
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
        }

        private void Print_UCIemOperInfo_Load(object sender, EventArgs e)
        {
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();
            //FillUI();
        }

        private void InitLookUpEditor()
        {
            InitLueDiagnose();
        }

        private void InitLueDiagnose()
        {
            //BindLueData(lueBefore, 12);
            //BindLueData(lueAfter, 12);
        }

        public void FillUI()
        {


            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner();


            //加载费用信息
            //btnFee_Click(null, null);
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
                //if (m_IemInfo.IemBasicInfo.Is_First_Case == 1)
                //    chkIsFirstCase1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Is_First_Case == 2)
                //    chkIsFirstCase2.Checked = true;
                labchkIsFirstCase.Text = m_IemInfo.IemBasicInfo.Is_First_Case.ToString();
                //随诊
                //if (m_IemInfo.IemBasicInfo.Is_Following == 1)
                //    chkIsFollowing1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Is_Following == 2)
                //    chkIsFollowing2.Checked = true;

                labchkIsFollowing.Text = m_IemInfo.IemBasicInfo.Is_Following.ToString();

                //示教
                //if (m_IemInfo.IemBasicInfo.Is_Teaching_Case == 1)
                //    chkIsTeachingCase1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Is_Teaching_Case == 2)
                //    chkIsTeachingCase2.Checked = true;

                labchkIsTeachingCase.Text = m_IemInfo.IemBasicInfo.Is_Teaching_Case.ToString();

                //血型
                //if (m_IemInfo.IemBasicInfo.Blood_Type_id == 1)
                //    chkBlood1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Blood_Type_id == 2)
                //    chkBlood2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Blood_Type_id == 3)
                //    chkBlood3.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Blood_Type_id == 4)
                //    chkBlood4.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Blood_Type_id == 5)
                //    chkBlood5.Checked = true;
                labchkBlood.Text = m_IemInfo.IemBasicInfo.Blood_Type_id.ToString();

                //RH
                //if (m_IemInfo.IemBasicInfo.Rh == 0)
                //    chkRh1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Blood_Type_id == 1)
                //    chkRh2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Blood_Type_id == 2)
                //    chkRh3.Checked = true;
                labchkRh.Text = m_IemInfo.IemBasicInfo.Rh.ToString();

                //输血反应
                //if (m_IemInfo.IemBasicInfo.Blood_Reaction_Id == 0)
                //    chkBloodReaction1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Blood_Reaction_Id == 1)
                //    chkBloodReaction2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Blood_Reaction_Id == 2)
                //    chkBloodReaction3.Checked = true;
                labchkBloodReaction.Text = m_IemInfo.IemBasicInfo.Blood_Reaction_Id.ToString();

                //输血
                seRbc.Text = m_IemInfo.IemBasicInfo.Blood_Rbc.ToString();
                sePlt.Text = m_IemInfo.IemBasicInfo.Blood_Plt.ToString();
                sePlasma.Text = m_IemInfo.IemBasicInfo.Blood_Plasma.ToString();
                seWb.Text = m_IemInfo.IemBasicInfo.Blood_Wb.ToString();
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
                //else if (row["项目名称"].ToString().Trim() == "治疗费")
                //{
                //    labelZhiLiao.Text = row["项目金额"].ToString();
                //}
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
                        //try
                        //{
                        //    lblRate.Text = Convertmy.ToString(Convertmy.ToDecimal(lblWMedical.Text) / Convertmy.ToDecimal(lblTotal.Text) * 100).Substring(0, 2) + "%";
                        //}
                        //catch
                        //{
                        //    lblRate.Text = "0";
                        //}
                    }
                }
                else if (row["项目名称"].ToString().Trim() == "押金累计")
                {
                    row["项目金额"].ToString();
                }
                //费比
            }

        }

        private void Print_UCFee_Paint(object sender, PaintEventArgs e)
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
        }
 }
}
