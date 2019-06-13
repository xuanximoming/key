using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Library;
using DrectSoft.Wordbook;
using System.Data.SqlClient;
//

using Convertmy = DrectSoft.Core.UtilsForExtension;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraEditors;
using DrectSoft.Common.Eop;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCObstetricsBaby : UserControl
    {
        IDataAccess m_SqlHelper;
        IDrectSoftLog m_Logger;
        private IemMainPageInfo m_IemInfo;
        DataTable m_showTable;
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

        private IEmrHost m_App;

        public UCObstetricsBaby()
        {
            InitializeComponent();

            m_SqlHelper = DataAccessFactory.DefaultDataAccess;
        }

        private void UCIemOperInfo_Load(object sender, EventArgs e)
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
            #region
            //胎次
            if (chkTC1.Checked == true)
                m_IemInfo.IemObstetricsBaby.TC = "1";
            else if (chkTC2.Checked == true)
                m_IemInfo.IemObstetricsBaby.TC = "2";
            else if (chkTC3.Checked == true)
                m_IemInfo.IemObstetricsBaby.TC = "3";
            else
                m_IemInfo.IemObstetricsBaby.TC = "";

            //胎别
            if (chkTB1.Checked == true)
                m_IemInfo.IemObstetricsBaby.TB = "1";
            else if (chkTB2.Checked == true)
                m_IemInfo.IemObstetricsBaby.TB = "2";
            else if (chkTB3.Checked == true)
                m_IemInfo.IemObstetricsBaby.TB = "3";
            else
                m_IemInfo.IemObstetricsBaby.TB = "";

            //产次
            if (chkCC1.Checked == true)
                m_IemInfo.IemObstetricsBaby.CC = "1";
            else if (chkCC2.Checked == true)
                m_IemInfo.IemObstetricsBaby.CC = "2";
            else if (chkCC3.Checked == true)
                m_IemInfo.IemObstetricsBaby.CC = "3";
            else
                m_IemInfo.IemObstetricsBaby.CC = "";

            //产妇会阴破裂度
            if (chkCFHYPLD1.Checked == true)
                m_IemInfo.IemObstetricsBaby.CFHYPLD = "1";
            else if (chkCFHYPLD2.Checked == true)
                m_IemInfo.IemObstetricsBaby.CFHYPLD = "2";
            else if (chkCFHYPLD3.Checked == true)
                m_IemInfo.IemObstetricsBaby.CFHYPLD = "3";
            else
                m_IemInfo.IemObstetricsBaby.CFHYPLD = "";

            //性别
            if (chkSex1.Checked == true)
                m_IemInfo.IemObstetricsBaby.Sex = "1";
            else if (chkSex2.Checked == true)
                m_IemInfo.IemObstetricsBaby.Sex = "2";
            else
                m_IemInfo.IemObstetricsBaby.Sex = "";

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
            else
                m_IemInfo.IemObstetricsBaby.CCQK = "";

            //出院情况
            if (chkCYQK1.Checked == true)
                m_IemInfo.IemObstetricsBaby.CYQK = "1";
            else if (chkCYQK2.Checked == true)
                m_IemInfo.IemObstetricsBaby.CYQK = "2";
            else if (chkCYQK3.Checked == true)
                m_IemInfo.IemObstetricsBaby.CYQK = "3";
            else
                m_IemInfo.IemObstetricsBaby.CYQK = "";

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
            else
                m_IemInfo.IemObstetricsBaby.FMFS = "";
            #endregion
        }

        public void FillUI(IemMainPageInfo info, IEmrHost app)
        {
            m_App = app;
            m_IemInfo = info;

            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner(0);
            this.UpdateShowTable();
            this.gridControlBabyInfo.DataSource = m_showTable;
            gridControlBabyInfo.EndUpdate();
            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewBabyInfo);
        }

        delegate void FillUIDelegate();
        private void FillUIInner(int rowindex)
        {
            #region
            if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
            {
                //to do 病患基本信息
            }
            else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.Count <= rowindex)
            {
                chkTC1.Checked = false;
                chkTC2.Checked = false;
                chkTC3.Checked = false;
                chkTB1.Checked = false;
                chkTB2.Checked = false;
                chkTB3.Checked = false;
                chkCC1.Checked = false;
                chkCC2.Checked = false;
                chkCC3.Checked = false;
                chkCFHYPLD1.Checked = false;
                chkCFHYPLD2.Checked = false;
                chkCFHYPLD3.Checked = false;
                chkSex1.Checked = false;
                chkSex2.Checked = false;
                txtMidwifery.Text = "";
                txtAPJPF.Text = "";
                txtheigh.Text = "";
                txtweight.Text = "";
                BithDayDate.Text = "";
                BithDayTime.Text = "";
                chkCCQK1.Checked = false;
                chkCCQK2.Checked = false;
                chkCCQK3.Checked = false;
                chkCCQK4.Checked = false;
                chkCYQK1.Checked = false;
                chkCYQK2.Checked = false;
                chkCYQK3.Checked = false;
                chkFMFS1.Checked = false;
                chkFMFS2.Checked = false;
                chkFMFS3.Checked = false;
                chkFMFS4.Checked = false;
                chkFMFS5.Checked = false;
                chkFMFS6.Checked = false;
                //return;
            }
            else
            {
                //胎次
                if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TC"].ToString() == "1")
                    chkTC1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TC"].ToString() == "2")
                    chkTC2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TC"].ToString() == "3")
                    chkTC3.Checked = true;
                else
                {
                    chkTC1.Checked = false;
                    chkTC2.Checked = false;
                    chkTC3.Checked = false;
                }



                //胎别
                if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TB"].ToString() == "1")
                    chkTB1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TB"].ToString() == "2")
                    chkTB2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["TB"].ToString() == "3")
                    chkTB3.Checked = true;
                else
                {
                    chkTB1.Checked = false;
                    chkTB2.Checked = false;
                    chkTB3.Checked = false;
                }


                //产次
                if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CC"].ToString() == "1")
                    chkCC1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CC"].ToString() == "2")
                    chkCC2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CC"].ToString() == "3")
                    chkCC3.Checked = true;
                else
                {
                    chkCC1.Checked = false;
                    chkCC2.Checked = false;
                    chkCC3.Checked = false;
                }


                //产妇会阴破裂度
                if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CFHYPLD"].ToString() == "1")
                    chkCFHYPLD1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CFHYPLD"].ToString() == "2")
                    chkCFHYPLD2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CFHYPLD"].ToString() == "3")
                    chkCFHYPLD3.Checked = true;
                else
                {
                    chkCFHYPLD1.Checked = false;
                    chkCFHYPLD2.Checked = false;
                    chkCFHYPLD3.Checked = false;
                }


                //性别
                if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["SEX"].ToString() == "1")
                    chkSex1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["SEX"].ToString() == "2")
                    chkSex2.Checked = true;
                else
                {
                    chkSex1.Checked = false;
                    chkSex2.Checked = false;
                }

                //接产者 add by ywk 2012年5月14日 14:42:04
                txtMidwifery.Text = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["MIDWIFERY"].ToString();

                //阿帕加评分
                txtAPJPF.Text = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["APJ"].ToString();

                //身长
                txtheigh.Text = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["HEIGH"].ToString();

                //体重
                txtweight.Text = m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["WEIGHT"].ToString();

                //出生日期
                //m_IemInfo.IemObstetricsBaby.BithDay = BithDayDate.DateTime.ToString("yyyy-MM-dd") + " " + BithDayTime.Time.ToString("HH:mm:ss");
                if (!String.IsNullOrEmpty(m_IemInfo.IemObstetricsBaby.BithDay))
                {
                    try
                    {
                        BithDayDate.Text = (DateTime.Parse(m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["BITHDAY"].ToString())).ToString("yyyy-MM-dd");
                        BithDayTime.Text = (DateTime.Parse(m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["BITHDAY"].ToString())).ToString("HH:mm:ss");
                    }
                    catch
                    { }
                }


                //产出情况
                if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CCQK"].ToString() == "1")
                    chkCCQK1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CCQK"].ToString() == "2")
                    chkCCQK2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CCQK"].ToString() == "3")
                    chkCCQK3.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CCQK"].ToString() == "4")
                    chkCCQK4.Checked = true;
                else
                {
                    chkCCQK1.Checked = false;
                    chkCCQK2.Checked = false;
                    chkCCQK3.Checked = false;
                    chkCCQK4.Checked = false;
                }


                //出院情况
                if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CYQK"].ToString() == "1")
                    chkCYQK1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CYQK"].ToString() == "2")
                    chkCYQK2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["CYQK"].ToString() == "3")
                    chkCYQK3.Checked = true;
                else
                {
                    chkCYQK1.Checked = false;
                    chkCYQK2.Checked = false;
                    chkCYQK3.Checked = false;
                }


                //分娩方式
                if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "1")
                    chkFMFS1.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "2")
                    chkFMFS2.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "3")
                    chkFMFS3.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "4")
                    chkFMFS4.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "5")
                    chkFMFS5.Checked = true;
                else if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows[rowindex]["FMFS"].ToString() == "6")
                    chkFMFS6.Checked = true;
                else
                {
                    chkFMFS1.Checked = false;
                    chkFMFS2.Checked = false;
                    chkFMFS3.Checked = false;
                    chkFMFS4.Checked = false;
                    chkFMFS5.Checked = false;
                    chkFMFS6.Checked = false;
                }

            }
            #endregion
            
        }
        private void UpdateShowTable()
        {
            #region
            m_showTable = new DataTable();
            m_showTable.Columns.Add("SEX");
            m_showTable.Columns.Add("HEIGH");
            m_showTable.Columns.Add("WEIGHT");
            m_showTable.Columns.Add("BITHDAY");
            m_showTable.Columns.Add("FMFS");
            m_showTable.Columns.Add("CCQK");
            m_showTable.Columns.Add("APJ");
            m_showTable.Columns.Add("MIDWIFERY");
            m_showTable.Columns.Add("CYQK");
            foreach (DataRow dr in m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows)
            {
                DataRow row = m_showTable.NewRow();
                switch (dr["SEX"].ToString())
                {
                    case "1":
                        row["SEX"] = "男";
                        break;
                    case "2":
                        row["SEX"] = "女";
                        break;
                    default:
                        row["SEX"] = "";
                        break;
                }
                switch (dr["FMFS"].ToString())
                {
                    case "1":
                        row["FMFS"] = "自然";
                        break;
                    case "2":
                        row["FMFS"] = "侧+吸";
                        break;
                    case "3":
                        row["FMFS"] = "产钳";
                        break;
                    case "4":
                        row["FMFS"] = "臂牵引";
                        break;
                    case "5":
                        row["FMFS"] = "剖宫";
                        break;
                    case "6":
                        row["FMFS"] = "其他";
                        break;
                    default:
                        row["FMFS"] = "";
                        break;
                }
                switch (dr["CCQK"].ToString())
                {
                    case "1":
                        row["CCQK"] = "活产";
                        break;
                    case "2":
                        row["CCQK"] = "死产";
                        break;
                    case "3":
                        row["CCQK"] = "死胎";
                        break;
                    case "4":
                        row["CCQK"] = "畸形";
                        break;
                    default:
                        row["CCQK"] = "";
                        break;
                }
                switch (dr["CYQK"].ToString())
                {
                    case "1":
                        row["CYQK"] = "正常";
                        break;
                    case "2":
                        row["CYQK"] = "有病";
                        break;
                    case "3":
                        row["CYQK"] = "交叉感染";
                        break;
                    default:
                        row["CYQK"] = "";
                        break;
                }
                row["HEIGH"] = dr["HEIGH"];
                row["WEIGHT"] = dr["WEIGHT"];
                row["BITHDAY"] = dr["BITHDAY"];
                row["APJ"] = dr["APJ"];
                row["MIDWIFERY"] = dr["MIDWIFERY"];
                m_showTable.Rows.Add(row);
            }
            #endregion
        }
        private void UCObstetricsBaby_Paint(object sender, PaintEventArgs e)
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
            ////e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(this.Width, 0));
            //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        }

        /// <summary>
        /// 获取打印板块的产妇婴儿信息
        /// </summary>
        /// <returns></returns>
        public Iem_MainPage_ObstetricsBaby GetPrintObsBaby()
        {
            Iem_MainPage_ObstetricsBaby _Iem_MainPage_Baby = new Iem_MainPage_ObstetricsBaby();
            if (m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.Count == 0)
            {
                return _Iem_MainPage_Baby;
            }
            _Iem_MainPage_Baby.IEM_MainPage_NO = m_IemInfo.IemObstetricsBaby.IEM_MainPage_NO;
            _Iem_MainPage_Baby.IEM_MainPage_ObstetricsBabyID = m_IemInfo.IemObstetricsBaby.IEM_MainPage_ObstetricsBabyID;
            _Iem_MainPage_Baby.TC = m_IemInfo.IemObstetricsBaby.TC;
            _Iem_MainPage_Baby.CC = m_IemInfo.IemObstetricsBaby.CC;
            _Iem_MainPage_Baby.TB = m_IemInfo.IemObstetricsBaby.TB;

            _Iem_MainPage_Baby.CFHYPLD = m_IemInfo.IemObstetricsBaby.CFHYPLD;
            _Iem_MainPage_Baby.Midwifery = m_IemInfo.IemObstetricsBaby.Midwifery;
            _Iem_MainPage_Baby.Sex = m_IemInfo.IemObstetricsBaby.Sex;
            _Iem_MainPage_Baby.APJ = m_IemInfo.IemObstetricsBaby.APJ;
            _Iem_MainPage_Baby.Heigh = m_IemInfo.IemObstetricsBaby.Heigh;

            _Iem_MainPage_Baby.Weight = m_IemInfo.IemObstetricsBaby.Weight;
            _Iem_MainPage_Baby.CCQK = m_IemInfo.IemObstetricsBaby.CCQK;
            _Iem_MainPage_Baby.BithDay = m_IemInfo.IemObstetricsBaby.BithDay;
            _Iem_MainPage_Baby.FMFS = m_IemInfo.IemObstetricsBaby.FMFS;
            _Iem_MainPage_Baby.CYQK = m_IemInfo.IemObstetricsBaby.CYQK;



            return _Iem_MainPage_Baby;
        }
        private Inpatient CurrentInpatient;//add by ywk 
        private void btn_OK_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(true, m_IemInfo);
            //点击确认按钮就将数据更新到数据库
            CurrentInpatient = m_App.CurrentPatientInfo;
            CurrentInpatient.ReInitializeAllProperties();
            IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
            manger.SaveData(m_IemInfo);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }

        private void simpleButtonAddInfo_Click(object sender, EventArgs e)
        {
            DataRow dr = m_IemInfo.IemObstetricsBaby.OutBabyTable.NewRow();
            
            #region
            //胎次
            if (chkTC1.Checked == true)
                dr["TC"] = "1";
            else if (chkTC2.Checked == true)
                dr["TC"] = "2";
            else if (chkTC3.Checked == true)
                dr["TC"] = "3";
            else
                dr["TC"] = "";
            //胎别
            if (chkTB1.Checked == true)
                dr["TB"] = "1";
            else if (chkTB2.Checked == true)
                dr["TB"] = "2";
            else if (chkTB3.Checked == true)
                dr["TB"] = "3";
            else
                dr["TB"] = "";

            //产次
            if (chkCC1.Checked == true)
                dr["CC"] = "1";
            else if (chkCC2.Checked == true)
                dr["CC"] = "2";
            else if (chkCC3.Checked == true)
                dr["CC"] = "3";
            else
                dr["CC"] = "";

            //产妇会阴破裂度
            if (chkCFHYPLD1.Checked == true)
                dr["CFHYPLD"] = "1";
            else if (chkCFHYPLD2.Checked == true)
                dr["CFHYPLD"] = "2";
            else if (chkCFHYPLD3.Checked == true)
                dr["CFHYPLD"] = "3";
            else
                dr["CFHYPLD"] = "";

            //性别
            if (chkSex1.Checked == true)
                dr["SEX"] = "1";
            else if (chkSex2.Checked == true)
                dr["SEX"] = "2";
            else
            {
                m_App.CustomMessageBox.MessageShow("性别不能为空!");
                return;
            }

            //阿帕加评分
            dr["APJ"] = txtAPJPF.Text;


            //身长
            dr["HEIGH"] = txtheigh.Text;
            if (txtheigh.Text.Trim() == "")
            {
                m_App.CustomMessageBox.MessageShow("身高不能为空!");
                return;
            }
            //体重
            dr["WEIGHT"] = txtweight.Text;
            if (txtweight.Text.Trim() == "")
            {
                m_App.CustomMessageBox.MessageShow("体重不能为空!");
                return;
            }

            //出生日期
            dr["BITHDAY"] = BithDayDate.DateTime.ToString("yyyy-MM-dd") + " " + BithDayTime.Time.ToString("HH:mm:ss");
            if (BithDayDate.Text.Trim() == "" || BithDayTime.Text.Trim() == "")
            {
                m_App.CustomMessageBox.MessageShow("出生日期不能为空!");
                return;
            }
            //接产者
            dr["MIDWIFERY"] = txtMidwifery.Text;
            if (txtMidwifery.Text.Trim() == "")
            {
                m_App.CustomMessageBox.MessageShow("接产者不能为空!");
                return;
            }

            //产出情况
            if (chkCCQK1.Checked == true)
                dr["CCQK"] = "1";
            else if (chkCCQK2.Checked == true)
                dr["CCQK"] = "2";
            else if (chkCCQK3.Checked == true)
                dr["CCQK"] = "3";
            else if (chkCCQK4.Checked == true)
                dr["CCQK"] = "4";
            else
            {
                m_App.CustomMessageBox.MessageShow("产出情况不能为空!");
                return;
            }

            //出院情况
            if (chkCYQK1.Checked == true)
                dr["CYQK"] = "1";
            else if (chkCYQK2.Checked == true)
                dr["CYQK"] = "2";
            else if (chkCYQK3.Checked == true)
                dr["CYQK"] = "3";
            else
            {
                m_App.CustomMessageBox.MessageShow("出院情况不能为空!");
                return;
            }

            //分娩方式
            if (chkFMFS1.Checked == true)
                dr["FMFS"] = "1";
            else if (chkFMFS2.Checked == true)
                dr["FMFS"] = "2";
            else if (chkFMFS3.Checked == true)
                dr["FMFS"] = "3";
            else if (chkFMFS4.Checked == true)
                dr["FMFS"] = "4";
            else if (chkFMFS5.Checked == true)
                dr["FMFS"] = "5";
            else if (chkFMFS6.Checked == true)
                dr["FMFS"] = "6";
            else
            {
                m_App.CustomMessageBox.MessageShow("分娩方式不能为空!");
                return;
            }
            int id = 0;
            foreach (DataRow d in m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows)
            {
                if (d["IBSBABYID"].ToString().Trim() == "")
                {
                    id = 1;
                }
                else if (int.Parse(d["IBSBABYID"].ToString()) > id)
                {
                    id = int.Parse(d["IBSBABYID"].ToString());
                }
            }
            dr["IBSBABYID"] = (id + 1).ToString();
            #endregion
            m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.Add(dr);
            this.GetUI();

            //DataTable dataTable = m_IemInfo.IemObstetricsBaby.OutBabyTable;
            //DataTable dataTableBaby;
            //if (this.gridControlBabyInfo.DataSource != null)
            //    dataTableBaby = this.gridControlBabyInfo.DataSource as DataTable;
            //if (dataTableBaby.Rows.Count == 0)
            //    dataTableBaby = dataTable.Clone();
            //foreach (DataRow row in dataTable.Rows)
            //{
            //    dataTableOper.ImportRow(row);
            //}
            //gridControlBabyInfo.BeginUpdate();
            this.UpdateShowTable();
            this.gridControlBabyInfo.DataSource = m_showTable;
            gridControlBabyInfo.EndUpdate();
            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewBabyInfo);
        }


        private void simpleButtonDeleteInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewBabyInfo.FocusedRowHandle < 0)
                    return;
                else
                {
                    m_IemInfo.IemObstetricsBaby.OutBabyTable.Rows.RemoveAt(gridViewBabyInfo.FocusedRowHandle);
                    this.FillUIInner(0);
                    this.UpdateShowTable();
                    this.gridControlBabyInfo.DataSource = m_showTable;
                    gridControlBabyInfo.EndUpdate();
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewBabyInfo);
                    this.GetUI();
                }
            }
            catch
            { }
        }

        private void gridControlBabyInfo_DoubleClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 更改，选中后可消除选择
        /// add by wyt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit chkEdit = GetCheckEdit(((Control)sender).Name);
            if (chkEdit.Checked)
            {
                chkEdit.Checked = false;
            }
        }
        /// <summary>
        /// 根据名称返回控件
        /// </summary>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        private CheckEdit GetCheckEdit(string ControlName)
        {
            foreach (Control control in this.Controls)
            {
                if (control.Name == ControlName)
                {
                    return (CheckEdit)control;
                }
            }
            foreach (Control control in this.panelControl1.Controls)
            {
                if (control.Name == ControlName)
                {
                    return (CheckEdit)control;
                }
            }
            return null;
        }

        private void gridControlBabyInfo_Click(object sender, EventArgs e)
        {
            if (gridViewBabyInfo.FocusedRowHandle < 0)
                return;
            else
            {
                DataRow dataRow = gridViewBabyInfo.GetDataRow(gridViewBabyInfo.FocusedRowHandle);
                if (dataRow == null)
                    return;
                else
                {
                    this.FillUIInner(gridViewBabyInfo.FocusedRowHandle);
                    this.GetUI();
                }
            }
        }

        /// <summary>
        /// <title>出生日期范围验证，不能大于当前日期</title>
        /// <auth>wyt</auth>
        /// <date>2012-11-02</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BithDayDate_Validating(object sender, CancelEventArgs e)
        {
            if (BithDayDate.DateTime > DateTime.Now)
            {
                this.errorProvider.SetError(BithDayDate, "超出范围");
                e.Cancel = true;
            }
            else
            {
                this.errorProvider.Clear();
            }
        }
    }
}
