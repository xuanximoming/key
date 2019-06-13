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
            if (info.IemBasicInfo.Iem_Mainpage_NO == "")
            {
                //to do 病患基本信息
            }
            else
            {
                //第一案例
                if (m_IemInfo.IemFeeInfo.IsFirstCase == "1")
                    chkIsFirstCase1.Checked = true;
                if (m_IemInfo.IemFeeInfo.IsFirstCase == "2")
                    chkIsFirstCase2.Checked = true;

                //随诊
                if (m_IemInfo.IemFeeInfo.IsFollowing == "1")
                    chkIsFollowing1.Checked = true;
                if (m_IemInfo.IemFeeInfo.IsFollowing == "2")
                    chkIsFollowing2.Checked = true;

                //示教
                if (m_IemInfo.IemFeeInfo.IsTeachingCase == "1")
                    chkIsTeachingCase1.Checked = true;
                if (m_IemInfo.IemFeeInfo.IsTeachingCase == "2")
                    chkIsTeachingCase2.Checked = true;

                //血型
                if (m_IemInfo.IemFeeInfo.BloodType == "1")
                    chkBlood1.Checked = true;
                if (m_IemInfo.IemFeeInfo.BloodType == "2")
                    chkBlood2.Checked = true;
                if (m_IemInfo.IemFeeInfo.BloodType == "3")
                    chkBlood3.Checked = true;
                if (m_IemInfo.IemFeeInfo.BloodType == "4")
                    chkBlood4.Checked = true;
                if (m_IemInfo.IemFeeInfo.BloodType == "5")
                    chkBlood5.Checked = true;


                //RH
                //if (m_IemInfo.IemFeeInfo.Rh == "0")
                //    chkRh1.Checked = true;
                if (m_IemInfo.IemFeeInfo.Rh == "1")
                    chkRh2.Checked = true;
                if (m_IemInfo.IemFeeInfo.Rh == "2")
                    chkRh3.Checked = true;


                //输血反应
                if (m_IemInfo.IemFeeInfo.BloodReaction == "0")
                    chkBloodReaction1.Checked = true;
                if (m_IemInfo.IemFeeInfo.BloodReaction == "1")
                    chkBloodReaction2.Checked = true;
                if (m_IemInfo.IemFeeInfo.BloodReaction == "2")
                    chkBloodReaction3.Checked = true;

                //输血
                seRbc.Value = Convertmy.ToDecimal(m_IemInfo.IemFeeInfo.Rbc);
                sePlt.Value = Convertmy.ToDecimal(m_IemInfo.IemFeeInfo.Plt);
                sePlasma.Value = Convertmy.ToDecimal(m_IemInfo.IemFeeInfo.Plasma);
                seWb.Value = Convertmy.ToDecimal(m_IemInfo.IemFeeInfo.Wb);
                txtOthers.Text = m_IemInfo.IemFeeInfo.Others;

                // 随诊
                if (!String.IsNullOrEmpty(m_IemInfo.IemFeeInfo.Following_Ending_Date))
                {
                    for (int i = 0; i < m_IemInfo.IemFeeInfo.Following_Ending_Date.Split('-').Length; i++)
                    {
                        if (i == 0)
                            txtIsFollowingDay.Text = m_IemInfo.IemFeeInfo.Following_Ending_Date.Split('-')[i];
                        else if (i == 1)
                            txtIsFollowingMon.Text = m_IemInfo.IemFeeInfo.Following_Ending_Date.Split('-')[i];
                        else if (i == 2)
                            txtIsFollowingYear.Text = m_IemInfo.IemFeeInfo.Following_Ending_Date.Split('-')[i];
                    }
                }

                //尸检
                if (m_IemInfo.IemFeeInfo.Ashes_Check == "1")
                    chkAshes_Check1.Checked = true;
                else if (m_IemInfo.IemFeeInfo.Ashes_Check == "2")
                    chkAshes_Check2.Checked = true;



                lblTotal.Text = m_IemInfo.IemFeeInfo.Total ;
                lblBed.Text = m_IemInfo.IemFeeInfo.Bed ;
                lblCare.Text = m_IemInfo.IemFeeInfo.Care ;
                lblWMedical.Text = m_IemInfo.IemFeeInfo.WMedical ;
                lblCPMedical.Text = m_IemInfo.IemFeeInfo.CPMedical;

                lblCMedical.Text = m_IemInfo.IemFeeInfo.CMedical;
                lblRadiate.Text = m_IemInfo.IemFeeInfo.Radiate;
                lblAssay.Text = m_IemInfo.IemFeeInfo.Assay;
                lblOx.Text = m_IemInfo.IemFeeInfo.Ox;
                lblBlood.Text = m_IemInfo.IemFeeInfo.Blood;

                lblMecical.Text = m_IemInfo.IemFeeInfo.Mecical;
                lblOperation.Text = m_IemInfo.IemFeeInfo.Operation;
                lblAccouche.Text = m_IemInfo.IemFeeInfo.Accouche;
                lblRis.Text = m_IemInfo.IemFeeInfo.Ris;
                lblAnaesthesia.Text = m_IemInfo.IemFeeInfo.Anaesthesia;

                lblBaby.Text = m_IemInfo.IemFeeInfo.Baby;
                lblFollwBed.Text = m_IemInfo.IemFeeInfo.FollwBed;
                lblOthers.Text = m_IemInfo.IemFeeInfo.Others1;
                labelZhiLiao.Text = m_IemInfo.IemFeeInfo.Others2;
                lblRate.Text = m_IemInfo.IemFeeInfo.Others3;

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
                m_IemInfo.IemFeeInfo.IsFirstCase = "1";
            if (chkIsFirstCase2.Checked == true)
                m_IemInfo.IemFeeInfo.IsFirstCase = "2";

            //随诊
            if (chkIsFollowing1.Checked == true)
                m_IemInfo.IemFeeInfo.IsFollowing = "1";
            if (chkIsFollowing2.Checked == true)
                m_IemInfo.IemFeeInfo.IsFollowing = "2";

            //示教
            if (chkIsTeachingCase1.Checked == true)
                m_IemInfo.IemFeeInfo.IsTeachingCase = "1";
            if (chkIsTeachingCase2.Checked == true)
                m_IemInfo.IemFeeInfo.IsTeachingCase = "2";

            //血型
            if (chkBlood1.Checked == true)
                m_IemInfo.IemFeeInfo.BloodType = "1";
            if (chkBlood2.Checked == true)
                m_IemInfo.IemFeeInfo.BloodType = "2";
            if (chkBlood3.Checked == true)
                m_IemInfo.IemFeeInfo.BloodType = "3";
            if (chkBlood4.Checked == true)
                m_IemInfo.IemFeeInfo.BloodType = "4";
            if (chkBlood5.Checked == true)
                m_IemInfo.IemFeeInfo.BloodType = "5";


            //RH
            //if (chkRh1.Checked == true)
            //    m_IemInfo.IemFeeInfo.Rh = "0";
            if (chkRh2.Checked == true)
                m_IemInfo.IemFeeInfo.Rh = "1";
            if (chkRh3.Checked == true)
                m_IemInfo.IemFeeInfo.Rh = "2";


            //输血反应
            if (chkBloodReaction1.Checked == true)
                m_IemInfo.IemFeeInfo.BloodReaction = "0";
            if (chkBloodReaction2.Checked == true)
                m_IemInfo.IemFeeInfo.BloodReaction = "1";
            if (chkBloodReaction3.Checked == true)
                m_IemInfo.IemFeeInfo.BloodReaction = "2";

            //输血
            m_IemInfo.IemFeeInfo.Rbc = seRbc.Value.ToString();
            m_IemInfo.IemFeeInfo.Plt = sePlt.Value.ToString();
            m_IemInfo.IemFeeInfo.Plasma = sePlasma.Value.ToString();
            m_IemInfo.IemFeeInfo.Wb = seWb.Value.ToString();
            m_IemInfo.IemFeeInfo.Others = txtOthers.Text.Trim();

            // 随诊
            m_IemInfo.IemFeeInfo.Following_Ending_Date = txtIsFollowingDay.Text.Trim() + "-" + txtIsFollowingMon.Text.Trim() + "-" + txtIsFollowingYear.Text;


            if (chkAshes_Check1.Checked == true)
            {
                m_IemInfo.IemFeeInfo.Ashes_Check = "1";
            }
            else if (chkAshes_Check2.Checked == true)
            {
                m_IemInfo.IemFeeInfo.Ashes_Check = "2";
            }

            
            m_IemInfo.IemFeeInfo.Total = lblTotal.Text;
            m_IemInfo.IemFeeInfo.Bed = lblBed.Text;
            m_IemInfo.IemFeeInfo.Care = lblCare.Text;
            m_IemInfo.IemFeeInfo.WMedical = lblWMedical.Text;
            m_IemInfo.IemFeeInfo.CPMedical = lblCPMedical.Text;

            m_IemInfo.IemFeeInfo.CMedical = lblCMedical.Text;
            m_IemInfo.IemFeeInfo.Radiate = lblRadiate.Text;
            m_IemInfo.IemFeeInfo.Assay = lblAssay.Text;
            m_IemInfo.IemFeeInfo.Ox = lblOx.Text;
            m_IemInfo.IemFeeInfo.Blood = lblBlood.Text;

            m_IemInfo.IemFeeInfo.Mecical = lblMecical.Text;
            m_IemInfo.IemFeeInfo.Operation = lblOperation.Text;
            m_IemInfo.IemFeeInfo.Accouche = lblAccouche.Text;
            m_IemInfo.IemFeeInfo.Ris = lblRis.Text;
            m_IemInfo.IemFeeInfo.Anaesthesia = lblAnaesthesia.Text;

            m_IemInfo.IemFeeInfo.Baby = lblBaby.Text;
            m_IemInfo.IemFeeInfo.FollwBed = lblFollwBed.Text;
            m_IemInfo.IemFeeInfo.Others1 = lblOthers.Text;
            m_IemInfo.IemFeeInfo.Others2 = labelZhiLiao.Text;
            m_IemInfo.IemFeeInfo.Others3 = lblRate.Text;
        }

        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFee_Click(object sender, EventArgs e)
        {
            return;

            this.SuspendLayout();
            if (m_App == null || m_App.CurrentPatientInfo == null)
                return;
            IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");

            if (sqlHelper == null)
            {
                m_App.CustomMessageBox.MessageShow("无法连接到HIS！", CustomMessageBoxKind.ErrorOk);
                return;
            }
            //to do  yxy 提取HIS数据库中病人费用信息

            string sql = string.Format(@"SELECT     CONVERT(varchar(12), PatCode) AS PatID,FeeCode,
                                             CONVERT(varchar(12), FeeName) AS FeeName, CONVERT(float, SUM(Amount)) AS amount
                                            FROM  root.InnerRecipeCount WITH (nolock)
                                            where PatCode = '{0}'
                                            GROUP BY PatCode, FeeName,FeeCode", m_App.CurrentPatientInfo.NoOfHisFirstPage);//m_App.CurrentPatientInfo.NoOfHisFirstPage);
            //SqlParameter[] paraColl = new SqlParameter[] { new SqlParameter("@syxh", m_App.CurrentPatientInfo.NoOfHisFirstPage ) };
            //DataTable dataTable = sqlHelper.ExecuteDataTable("usp_bq_fymxcx", paraColl, CommandType.StoredProcedure);

            DataTable dataTable = sqlHelper.ExecuteDataTable(sql, CommandType.Text);
            //to do 赋值
            //to do 赋值
            Double totalFee = 0;
            Double OtherFee = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                totalFee = totalFee + Convert.ToDouble(row["amount"].ToString());
                OtherFee = OtherFee + Convert.ToDouble(row["amount"].ToString());

                //床费
                if (row["FeeName"].ToString().Trim() == "床位费")
                {
                    this.lblBed.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //护理费
                else if (row["FeeName"].ToString().Trim() == "护理费")
                {
                    this.lblCare.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //西药费
                else if (row["FeeName"].ToString().Trim() == "西药费")
                {
                    lblWMedical.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }

                //中成药费
                else if (row["FeeName"].ToString().Trim() == "中成药费")
                {
                    lblCPMedical.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //中草药费
                else if (row["FeeName"].ToString().Trim() == "草药费")
                {
                    lblCMedical.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //放射费
                else if (row["FeeName"].ToString().Trim() == "放射费")
                {
                    lblRadiate.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //检验
                else if (row["FeeName"].ToString().Trim() == "其它")
                {
                    lblAssay.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //输氧费
                else if (row["FeeName"].ToString().Trim() == "输氧费")
                {
                    lblOx.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }

                //输血费
                else if (row["FeeName"].ToString().Trim() == "输血费")
                {
                    lblBlood.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //诊查费
                else if (row["FeeName"].ToString().Trim() == "诊查费")
                {
                    lblMecical.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //手术费
                else if (row["FeeName"].ToString().Trim() == "手术费")
                {
                    lblOperation.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //接生费
                else if (row["FeeName"].ToString().Trim() == "接生费")
                {
                    lblAccouche.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //检查费
                else if (row["FeeName"].ToString().Trim() == "检查费")
                {
                    lblRis.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }

                //麻醉费
                else if (row["FeeName"].ToString().Trim() == "麻醉费")
                {
                    lblAnaesthesia.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //治疗费
                else if (row["FeeName"].ToString().Trim() == "治疗费")
                {
                    labelZhiLiao.Text = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //婴儿费   陪床费  药占比  检验费 未匹配


            }

            this.lblTotal.Text = totalFee.ToString();

            lblOthers.Text = OtherFee.ToString();

            this.Refresh();
            //if (this.FindForm() == null)
            //{
            //    this.Refresh();
            //}
            //else
            //    this.FindForm().Refresh();

            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            foreach (Control control in this.Controls)
            {
                if (control is LabelControl)
                {
                    control.Visible = false;
                    e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

                }
                if (control is TextEdit)
                {
                    e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                        new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                }
            }
        }

        private void UCOthers_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is LabelControl)
                {
                    control.Visible = false;
                    e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

                }
                if (control is TextEdit)
                {
                    e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                        new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                }
            }

            //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
            //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
            //e.Graphics.DrawLine(Pens.Black, new Point(0, this.Height - 1), new Point(this.Width, this.Height - 1));
        }
 
        private void btn_OK_Click(object sender, EventArgs e)
        {
            GetUI();
            ((ShowUC)this.Parent).Close(true, m_IemInfo);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }
    }
}
