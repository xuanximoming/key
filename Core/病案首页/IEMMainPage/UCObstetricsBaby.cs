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
    public partial class UCObstetricsBaby : UserControl
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
                GetUI();
                return m_IemInfo;
            }
        }

        private IYidanEmrHost m_App;

        public UCObstetricsBaby()
        {
            InitializeComponent();

            m_SqlHelper = DataAccessFactory.DefaultDataAccess;
        }

        private void Print_UCIemOperInfo_Load(object sender, EventArgs e)
        {
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();

            //FillUI();
        }


        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            //胎次
            if (chkTC1.Checked == true)
                m_IemInfo.IemObstetricsBaby.TC = "1";
            else if (chkTC2.Checked == true)
                m_IemInfo.IemObstetricsBaby.TC = "2";
            else if (chkTC3.Checked == true)
                m_IemInfo.IemObstetricsBaby.TC = "3";

            //胎别
            if (chkTB1.Checked == true)
                m_IemInfo.IemObstetricsBaby.TB = "1";
            else if (chkTB2.Checked == true)
                m_IemInfo.IemObstetricsBaby.TB = "2";
            else if (chkTB3.Checked == true)
                m_IemInfo.IemObstetricsBaby.TB = "3";

            //产次
            if (chkCC1.Checked == true)
                m_IemInfo.IemObstetricsBaby.CC = "1";
            else if (chkCC2.Checked == true)
                m_IemInfo.IemObstetricsBaby.CC = "2";
            else if (chkCC3.Checked == true)
                m_IemInfo.IemObstetricsBaby.CC = "3";

            //产妇会阴破裂度
            if (chkCFHYPLD1.Checked == true)
                m_IemInfo.IemObstetricsBaby.CFHYPLD = "1";
            else if (chkCFHYPLD2.Checked == true)
                m_IemInfo.IemObstetricsBaby.CFHYPLD = "2";
            else if (chkCFHYPLD3.Checked == true)
                m_IemInfo.IemObstetricsBaby.CFHYPLD = "3";

            //性别
            if (chkSex1.Checked == true)
                m_IemInfo.IemObstetricsBaby.Sex = "1";
            else if (chkSex2.Checked == true)
                m_IemInfo.IemObstetricsBaby.Sex = "2";

            //阿帕加评分
            m_IemInfo.IemObstetricsBaby.APJ = txtAPJPF.Text;

            //身长
            m_IemInfo.IemObstetricsBaby.Heigh = txtheigh.Text;

            //体重
            m_IemInfo.IemObstetricsBaby.Weight = txtweight.Text;

            //出生日期
            m_IemInfo.IemObstetricsBaby.BithDay = BithDayDate.DateTime.ToString("yyyy-MM-dd") + " " + BithDayTime.Time.ToString("HH:mm:ss");

            m_IemInfo.IemObstetricsBaby.Midwifery = txtMidwifery.Text;

            //产出情况
            if (chkCCQK1.Checked == true)
                m_IemInfo.IemObstetricsBaby.CCQK = "1";
            else if (chkCCQK2.Checked == true)
                m_IemInfo.IemObstetricsBaby.CCQK = "2";
            else if (chkCCQK3.Checked == true)
                m_IemInfo.IemObstetricsBaby.CCQK = "3";
            else if (chkCCQK4.Checked == true)
                m_IemInfo.IemObstetricsBaby.CCQK = "4";

            //出院情况
            if (chkCYQK1.Checked == true)
                m_IemInfo.IemObstetricsBaby.CYQK = "1";
            else if (chkCYQK2.Checked == true)
                m_IemInfo.IemObstetricsBaby.CYQK = "2";
            else if (chkCYQK3.Checked == true)
                m_IemInfo.IemObstetricsBaby.CYQK = "3";

            //分娩方式
            if (chkFMFS1.Checked == true)
                m_IemInfo.IemObstetricsBaby.FMFS = "1";
            else if (chkFMFS2.Checked == true)
                m_IemInfo.IemObstetricsBaby.FMFS = "2";
            else if (chkFMFS3.Checked == true)
                m_IemInfo.IemObstetricsBaby.FMFS = "3";
            else if (chkFMFS4.Checked == true)
                m_IemInfo.IemObstetricsBaby.FMFS = "4";
            else if (chkFMFS5.Checked == true)
                m_IemInfo.IemObstetricsBaby.FMFS = "5";
            else if (chkFMFS6.Checked == true)
                m_IemInfo.IemObstetricsBaby.FMFS = "6";


        }

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
            #region
            if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == 0)
            {
                //to do 病患基本信息
            }
            else
            {
                //胎次
                if (m_IemInfo.IemObstetricsBaby.TC == "1")
                    chkTC1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.TC == "2")
                    chkTC2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.TC == "3")
                    chkTC3.Checked = true;



                //胎别
                if (m_IemInfo.IemObstetricsBaby.TB == "1")
                    chkTB1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.TB == "2")
                    chkTB2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.TB == "3")
                    chkTB3.Checked = true;


                //产次
                if (m_IemInfo.IemObstetricsBaby.CC == "1")
                    chkCC1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.CC == "2")
                    chkCC2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.CC == "3")
                    chkCC3.Checked = true;


                //产妇会阴破裂度
                if (m_IemInfo.IemObstetricsBaby.CFHYPLD == "1")
                    chkCFHYPLD1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.CFHYPLD == "2")
                    chkCFHYPLD2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.CFHYPLD == "3")
                    chkCFHYPLD3.Checked = true;


                //性别
                if (m_IemInfo.IemObstetricsBaby.Sex == "1")
                    chkSex1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.Sex == "2")
                    chkSex2.Checked = true;


                //阿帕加评分
                txtAPJPF.Text = m_IemInfo.IemObstetricsBaby.APJ;

                //身长
                txtheigh.Text = m_IemInfo.IemObstetricsBaby.Heigh;

                //体重
                txtweight.Text = m_IemInfo.IemObstetricsBaby.Weight;

                //出生日期
                //m_IemInfo.IemObstetricsBaby.BithDay = BithDayDate.DateTime.ToString("yyyy-MM-dd") + " " + BithDayTime.Time.ToString("HH:mm:ss");
                if (!String.IsNullOrEmpty(m_IemInfo.IemObstetricsBaby.BithDay))
                {
                    BithDayDate.DateTime = Convert.ToDateTime(m_IemInfo.IemObstetricsBaby.BithDay);
                    BithDayTime.Time = Convert.ToDateTime(m_IemInfo.IemObstetricsBaby.BithDay);
                }


                //产出情况
                if (m_IemInfo.IemObstetricsBaby.CCQK == "1")
                    chkCCQK1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.CCQK == "2")
                    chkCCQK2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.CCQK == "3")
                    chkCCQK3.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.CCQK == "4")
                    chkCCQK4.Checked = true;


                //出院情况
                if (m_IemInfo.IemObstetricsBaby.CYQK == "1")
                    chkCYQK1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.CYQK == "2")
                    chkCYQK2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.CYQK == "3")
                    chkCYQK3.Checked = true;


                //分娩方式
                if (m_IemInfo.IemObstetricsBaby.FMFS == "1")
                    chkFMFS1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.FMFS == "2")
                    chkFMFS2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.FMFS == "3")
                    chkFMFS3.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.FMFS == "4")
                    chkCCQK4.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.FMFS == "5")
                    chkFMFS5.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.FMFS == "6")
                    chkFMFS6.Checked = true;

            }
            #endregion
        }

        private void UCObstetricsBaby_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextEdit)
                {
                    if (control.Visible == true)
                        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                }
            }

            e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
            //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(this.Width, 0));
            e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        }

        /// <summary>
        /// 获取打印板块的产妇婴儿信息
        /// </summary>
        /// <returns></returns>
        public Print_Iem_MainPage_ObstetricsBaby GetPrintObsBaby()
        {
            Print_Iem_MainPage_ObstetricsBaby _Print_Iem_MainPage_Baby = new Print_Iem_MainPage_ObstetricsBaby();

            _Print_Iem_MainPage_Baby.IEM_MainPage_NO = m_IemInfo.IemObstetricsBaby.IEM_MainPage_NO;
            _Print_Iem_MainPage_Baby.IEM_MainPage_ObstetricsBabyID = m_IemInfo.IemObstetricsBaby.IEM_MainPage_ObstetricsBabyID;
            _Print_Iem_MainPage_Baby.TC = m_IemInfo.IemObstetricsBaby.TC;
            _Print_Iem_MainPage_Baby.CC = m_IemInfo.IemObstetricsBaby.CC;
            _Print_Iem_MainPage_Baby.TB = m_IemInfo.IemObstetricsBaby.TB;

            _Print_Iem_MainPage_Baby.CFHYPLD = m_IemInfo.IemObstetricsBaby.CFHYPLD;
            _Print_Iem_MainPage_Baby.Midwifery = m_IemInfo.IemObstetricsBaby.Midwifery;
            _Print_Iem_MainPage_Baby.Sex = m_IemInfo.IemObstetricsBaby.Sex;
            _Print_Iem_MainPage_Baby.APJ = m_IemInfo.IemObstetricsBaby.APJ;
            _Print_Iem_MainPage_Baby.Heigh = m_IemInfo.IemObstetricsBaby.Heigh;

            _Print_Iem_MainPage_Baby.Weight = m_IemInfo.IemObstetricsBaby.Weight;
            _Print_Iem_MainPage_Baby.CCQK = m_IemInfo.IemObstetricsBaby.CCQK;
            _Print_Iem_MainPage_Baby.BithDay = m_IemInfo.IemObstetricsBaby.BithDay;
            _Print_Iem_MainPage_Baby.FMFS = m_IemInfo.IemObstetricsBaby.FMFS;
            _Print_Iem_MainPage_Baby.CYQK = m_IemInfo.IemObstetricsBaby.CYQK;



            return _Print_Iem_MainPage_Baby;
        }

    }
}
