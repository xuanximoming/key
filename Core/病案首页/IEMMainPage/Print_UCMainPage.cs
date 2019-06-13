using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using YidanSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;

using YidanSoft.FrameWork.BizBus;

using Convertmy = YidanSoft.Core.UtilsForExtension;
using YidanSoft.Common.Library;
using YidanSoft.Wordbook;
using DevExpress.XtraBars;
using YidanSoft.FrameWork.WinForm;
using System.Drawing.Printing;
using Microsoft.VisualBasic.PowerPacks;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class Print_UCMainPage : DevExpress.XtraEditors.XtraUserControl
    {
        #region fields & propertys
        private UCOthers ucOthers;
        private UCIemOperInfo ucIemOperInfo;
        private UCIemDiagnose ucIemDiagnose;
        private UCIemBasInfo ucIemBasInfo;

        private Print_UCFee print_UCFee;
        private Print_UCIemBasInfo print_UCIemBasInfo;
        private Print_UCIemDiagnose print_UCIemDiagnose;
        private Print_UCIemOperInfo print_UCIemOperInfo;
        private Print_UCObstetricsBaby print_UCObstetricsBaby;

        private IYidanEmrHost m_app;
        private IYidanSoftLog m_Logger;
        private IBizBus m_BizBus;
        private IemMainPageInfo m_IemInfo = new IemMainPageInfo();

        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get { return m_IemInfo; }
            set { m_IemInfo = value; }
        }
        #endregion

        public Print_UCMainPage()
            : this(null, null)
        {
        }
        public Print_UCMainPage(IYidanEmrHost app, IemMainPageInfo ieminfo)
        {
            InitializeComponent();
            m_app = app;

            m_IemInfo = ieminfo;
            //this.Load += new EventHandler(UCMainPage_Load);
            //simpleButtonSave.Click += new EventHandler(simpleButtonSave_Click);
            InitUserControl();
            //m_app.ChoosePatient(150);//切换病人
        }

        private void InitUserControl()
        {
            AddUserControl();
            //this.ucIemBasInfo.BackColor = Color.White;
            //this.ucIemDiagnose.BackColor = Color.White;
            //this.ucIemOperInfo.BackColor = Color.White;
            //this.ucOthers.BackColor = Color.White;

            this.print_UCFee.BackColor = Color.White;
            this.print_UCIemBasInfo.BackColor = Color.White;
            this.print_UCIemDiagnose.BackColor = Color.White;
            this.print_UCIemOperInfo.BackColor = Color.White;
            this.print_UCObstetricsBaby.BackColor = Color.White;

            //this.Height = ucOthers.Location.Y + ucOthers.Height + 20;
            this.Height = print_UCFee.Location.Y + print_UCFee.Height + 20;
            InitLocation();
        }

        //void simpleButtonSave_Click(object sender, EventArgs e)
        //{
        //    SaveData();
        //}

        /// <summary>
        /// 在界面中增加用户控件
        /// </summary>
        private void AddUserControl()
        {
            //ucOthers = new UCOthers();
            //ucIemOperInfo = new UCIemOperInfo();
            //ucIemDiagnose = new UCIemDiagnose();
            //ucIemBasInfo = new UCIemBasInfo();

            print_UCFee = new Print_UCFee(m_IemInfo, m_app);
            print_UCIemBasInfo = new Print_UCIemBasInfo(m_IemInfo, m_app);
            print_UCIemDiagnose = new Print_UCIemDiagnose(m_IemInfo, m_app);
            print_UCIemOperInfo = new Print_UCIemOperInfo(m_IemInfo, m_app);
            print_UCObstetricsBaby = new Print_UCObstetricsBaby(m_IemInfo, m_app);


            this.Controls.Add(print_UCFee);
            this.Controls.Add(print_UCIemBasInfo);
            this.Controls.Add(print_UCIemDiagnose);
            this.Controls.Add(print_UCIemOperInfo);
            this.Controls.Add(print_UCObstetricsBaby);

        }

        void UCMainPage_Load(object sender, EventArgs e)
        {

            InitLocation();

            if (m_app.CurrentPatientInfo == null)
                return;

            //delegateLoad deleteLoad = new delegateLoad(LoadForm);
            //deleteLoad.Invoke();
            LoadForm();

            Print_UCMainPage_Load(null, null);
        }

        private void LoadForm()
        {
            //m_BizBus = BusFactory.GetBus();
            //m_Logger = m_BizBus.BuildUp<IYidanSoftLog>(new string[] { "病案首页" });
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
            //GetIemInfo();
            //FillUI();
        }

        delegate void delegateLoad();

        /// <summary>
        /// 初始化控件位置
        /// </summary>
        private void InitLocation()
        {
            Int32 pointX = (this.Width - this.print_UCIemBasInfo.Width) / 2;
            Int32 pointY = 20;


            print_UCIemBasInfo.Location = new Point(pointX, pointY);
            print_UCIemDiagnose.Location = new Point(pointX, pointY + print_UCIemBasInfo.Height);
            print_UCIemOperInfo.Location = new Point(pointX, pointY + print_UCIemBasInfo.Height + print_UCIemDiagnose.Height);
            print_UCObstetricsBaby.Location = new Point(pointX, pointY + print_UCIemBasInfo.Height + print_UCIemDiagnose.Height + print_UCIemOperInfo.Height);
            print_UCFee.Location = new Point(pointX, pointY + print_UCIemBasInfo.Height + print_UCIemDiagnose.Height + print_UCIemOperInfo.Height + print_UCObstetricsBaby.Height);


        }

        #region 根据首页序号得到病案首页的信息，并给界面赋值

        private void FillUI()
        {
            //this.ucIemBasInfo.FillUI(IemInfo, m_app);
            //this.ucIemDiagnose.FillUI(IemInfo, m_app);
            //this.ucIemOperInfo.FillUI(IemInfo, m_app);
            //this.ucOthers.FillUI(IemInfo, m_app);

            //print_UCIemBasInfo.FillUI(IemInfo, m_app);
            //print_UCIemDiagnose.FillUI(IemInfo, m_app);
            //print_UCIemOperInfo.FillUI(IemInfo, m_app);
            //print_UCObstetricsBaby.FillUI(IemInfo, m_app);
            //print_UCFee.FillUI(IemInfo, m_app);
        }

        #endregion



        #region 初始化病人信息
        /// <summary>
        /// 初始化病人信息
        /// </summary>
        /// <param name="firstPageNo"></param>
        public void InitPatientInfo()
        {
            //GetIemInfo();
            FillUI();
        }
        #endregion

        #region IEMREditor 成员
        public Control DesignUI
        {
            get { return this; }
        }


        //public void Save()
        //{
        //    //SaveData();
        //}

        public string Title
        {
            get { return "病案首页"; }
        }
        #endregion

        private void UCMainPage_SizeChanged(object sender, EventArgs e)
        {
            ReSetUCLocaton();
        }

        /// <summary>
        /// 改变UserControl的位置
        /// </summary>
        private void ReSetUCLocaton()
        {
            Int32 pointX = (this.Width - this.print_UCIemBasInfo.Width) / 2;
            Int32 pointY = print_UCIemBasInfo.Location.Y;
            //this.ucIemBasInfo.Location = new Point(pointX, pointY);
            //this.ucIemDiagnose.Location = new Point(pointX, pointY + ucIemBasInfo.Height);
            //this.ucIemOperInfo.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height);
            //this.ucOthers.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height + ucIemOperInfo.Height);

            print_UCIemBasInfo.Location = new Point(pointX, pointY);
            print_UCIemDiagnose.Location = new Point(pointX, pointY + print_UCIemBasInfo.Height);
            print_UCIemOperInfo.Location = new Point(pointX, pointY + print_UCIemBasInfo.Height + print_UCIemDiagnose.Height);
            print_UCObstetricsBaby.Location = new Point(pointX, pointY + print_UCIemBasInfo.Height + print_UCIemDiagnose.Height + print_UCIemOperInfo.Height);
            print_UCFee.Location = new Point(pointX, pointY + print_UCIemBasInfo.Height + print_UCIemDiagnose.Height + print_UCIemOperInfo.Height + print_UCObstetricsBaby.Height);

        }

        private void Print_UCMainPage_Load(object sender, EventArgs e)
        {
            //PageSetupDialog m_PageSetupDialog = new PageSetupDialog();
            //m_PageSetupDialog.Document = printDocument1;
            //m_PageSetupDialog.ShowDialog();
            

            foreach (Control control in this.Controls)
            {
                GetControl(control);
            }


            printDocument1.Print();

            foreach (Control control in this.Controls)
            {
                control.Visible = false;
            }
        }

        int pageindex = -1;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            pageindex++;
            //e.Graphics.DrawString(
            //e.Graphics.DrawLine(
            PrintPage(e);


            if(pageindex==0)
                e.HasMorePages = true;
            else
            e.HasMorePages = false;

        }

        private void PrintPage(PrintPageEventArgs e)
        {
            foreach (Control control in m_listControl)
            {

                if (pageindex == 1)
                {
                    Point p = new Point(control.Location.X, control.Location.Y - m_LastPageLinePoint.Y);
                    control.Location = p;
                }
                if (control.Location.Y > m_LastPageLinePoint.Y)
                {
                    continue;
                }

                if (control is TextEdit || control is LookUpEdit)
                {
                    if (control.Visible == true)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                        e.Graphics.DrawString(((TextEdit)control).Text, control.Font, Brushes.Black, control.Location);
                    }
                }
                if (control is LabelControl)
                {
                    if (control.Visible == true)
                    {
                        //e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                        //    new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                        e.Graphics.DrawString(((LabelControl)control).Text, control.Font, Brushes.Black, control.Location);
                        if (((LabelControl)control).BorderStyle == DevExpress.XtraEditors.Controls.BorderStyles.Office2003)
                        {
                            Rectangle fram = Rectangle.FromLTRB(control.Left, control.Top, control.Right, control.Bottom);
                            e.Graphics.DrawRectangle(Pens.Black, fram);
                        }
                    }
                }

                if (control is CheckBox)
                {
                    if (control.Visible == true)
                    {
                        PrintOff(control, e);
                    }
                }

                if (control is HLineEx || control is VLineEx)
                {
                    if (control is HLineEx)
                    {
                        if (((HLineEx)control).IsBold == true)
                        {
                            e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y),
                                                new Point(control.Location.X + control.Width, control.Location.Y));

                            e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + 1),
                                                                        new Point(control.Location.X + control.Width, control.Location.Y + 1));
                        }
                        else
                        {
                            e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y),
                                                new Point(control.Location.X + control.Width, control.Location.Y));
                        }
                    }
                    else
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y),
                                            new Point(control.Location.X , control.Location.Y+control.Height));
                    }

                }
            }
        }

        private void HideSbutton()
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType() == typeof(LookUpEditor))
                    ((LookUpEditor)ctl).ShowSButton = false;
                else
                {
                    foreach (Control ct in ctl.Controls)
                    {
                        if (ct.GetType() == typeof(LookUpEditor))
                            ((LookUpEditor)ct).ShowSButton = false;
                    }
                }
            }
        }

        List<Control> m_listControl = new List<Control>();
        private void GetControl(Control control)
        {

            foreach (Control ct1 in control.Controls)
            {
                if (ct1.Controls.Count > 1)
                {
                    GetControl(ct1);
                    NewLocation(ct1);
                    m_listControl.Add(ct1);
                }
                else
                {
                    NewLocation(ct1);
                    m_listControl.Add(ct1);
                }
            }
        }

        Point m_LastPageLinePoint;
        private void NewLocation(Control control)
        {
            Control newControl = control;
            Point newPoint = control.Location;
            while (control.Parent != null && control.Name != "Print_UCMainPage")
            {
                newPoint = new Point(newPoint.X , newPoint.Y + control.Parent.Location.Y);
                control = control.Parent;
            }
            if (newControl.Name == "hLineExlast")
                m_LastPageLinePoint = newPoint;
            newControl.Location = newPoint;
        }

        /// <summary>
        /// 给出个起始坐标，长宽， 画个勾
        /// </summary>
        /// <param name="location"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        private void PrintOff(Control control, PrintPageEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height / 2),
                             new Point(control.Location.X + control.Width / 2, control.Location.Y + control.Height));

            e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X + control.Width / 2, control.Location.Y + control.Height),
                                new Point(control.Location.X + control.Width, control.Location.Y));
        }

        private void Print_UCMainPage_Load_1(object sender, EventArgs e)
        {
            printDocument1.Print();

            foreach (Control control in this.Controls)
            {
                control.Visible = false;
            }
        }


    }
}
