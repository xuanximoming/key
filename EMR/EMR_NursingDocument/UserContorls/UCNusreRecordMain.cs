using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Core.EMR_NursingDocument.PublicMethod;
using DrectSoft.Core.EMR_NursingDocument.PublicSet;

namespace DrectSoft.Core.EMR_NursingDocument.UserContorls
{
    public partial class UCNusreRecordMain : DevExpress.XtraEditors.XtraUserControl
    {
        //记录单id
        string recordId = "";

        public UCNusreRecordMain()
        {
            InitializeComponent();
            SetMainToopStripFalse();
        }
 


        public UCNusreRecordMain(string recordID)
        {
            this.recordId = recordID;
            InitializeComponent();
            SetMainToopStripFalse();
        }

        /// <summary>
        /// 对记录单id进行赋值 判断调用哪个记录单 new构造函数后调用
        /// </summary>
        /// <param name="recordId"></param>
        public void InitData(string recordId)
        {
            this.recordId = recordId;
        }

        //根据recordId进行判断调用哪个打印模板
        private void LoadPrint(PrintRecordModel model)
        {
            XDesigner.Report.DataBaseReportBuilder builder = new XDesigner.Report.DataBaseReportBuilder();
            builder.SetVariable("PrintRecordModel", new object[] { model });
            string path = Application.StartupPath.ToString();
            if (this.recordId == "2")
            {
                path += @"\Report\OutAndInNurseRecord.xrp";
            }
            else if (this.recordId == "3")
            {
                path += @"\Report\ChildOperNurseRecord.xrp";
            }
            else if (this.recordId == "4")
            {
                path += @"\Report\NoOperNurseRecord.xrp";
            }
            else if (this.recordId == "5")
            {
                path += @"\Report\OperNurseRecord.xrp";
            }
            if (path == null) return;
            builder.Load(path);
            builder.Refresh();
            xPrint.Documents.Add(builder.ReportDocument);
        }

        /// <summary>
        /// 从数据库中查询相关记录并显示
        /// </summary>
        public void SetOperNurseRecord()
        {
            int everpagecount = 15;
            NurseRecordBiz nurseRecordBiz = new PublicMethod.NurseRecordBiz();
            List<NurseRecordEntity> nurseRecordEntityList = nurseRecordBiz.GetNurseRecord(MethodSet.CurrentInPatient.NoOfFirstPage.ToString(), recordId);
            xPrint.Documents.Clear();
            //显示数据
            if (this.recordId == "2")
            {
                SetPrintInAndOut(everpagecount, nurseRecordEntityList);
            }
            else
            {
                SetPrintOthers(everpagecount, nurseRecordEntityList);
            }
            xPrint.RefreshView();
        }

        private void SetPrintInAndOut(int everpagecount, List<NurseRecordEntity> nurseRecordEntityList)
        {
            List<NurseRecordEntity> nurseRecordEntityInOut1 = new List<NurseRecordEntity>();
            List<NurseRecordEntity> nurseRecordEntityInOut2= new List<NurseRecordEntity>();
            List<NurseRecordEntity> nurseRecordEntityInOut3 = new List<NurseRecordEntity>();

            //对list进行处理 使其是everpagecount的倍数
            for (int i = 0; i < nurseRecordEntityList.Count; i++)
            {
                if (i % 3 == 0)
                {
                    nurseRecordEntityInOut1.Add(nurseRecordEntityList[i]);
                }

                if (i % 3 == 1)
                {
                    nurseRecordEntityInOut2.Add(nurseRecordEntityList[i]);
                }

                if (i % 3 == 2)
                {
                    nurseRecordEntityInOut3.Add(nurseRecordEntityList[i]);
                }
            }

            PrintRecordModel printRecordModel = new PrintRecordModel();
            printRecordModel.PrintHeaderModel = new PrintHeaderModel();
            printRecordModel.PrintHeaderModel.PersonName = MethodSet.CurrentInPatient.Name;
            printRecordModel.PrintHeaderModel.InNo = MethodSet.CurrentInPatient.RecordNoOfHospital;
            DataTable dt = MethodSet.GetPatientInfoForThreeMeasureTable(MethodSet.CurrentInPatient.NoOfFirstPage);
            if (dt != null && dt.Rows.Count > 0)
            {
                printRecordModel.PrintHeaderModel.InBedNo = dt.Rows[0]["outbed"].ToString();
                printRecordModel.PrintHeaderModel.DepartName = dt.Rows[0]["dept_name"].ToString();
            }

            if (nurseRecordEntityInOut1.Count == 0)
            {
                for (int i = 0; i < everpagecount; i++)
                {
                    nurseRecordEntityInOut1.Add(new NurseRecordEntity());
                }
            }
            if (nurseRecordEntityInOut2.Count == 0)
            {
                for (int i = 0; i < everpagecount; i++)
                {
                    nurseRecordEntityInOut2.Add(new NurseRecordEntity());
                }
            }
            if (nurseRecordEntityInOut3.Count == 0)
            {
                for (int i = 0; i < everpagecount; i++)
                {
                    nurseRecordEntityInOut3.Add(new NurseRecordEntity());
                }
            }

            int yushu1 = nurseRecordEntityInOut1.Count % everpagecount;
            int zheshu1 = nurseRecordEntityInOut1.Count / everpagecount;
            if (yushu1 > 0)
            {
                zheshu1 = zheshu1 + 1;
                int lastint = everpagecount - yushu1;
                for (int i = 0; i < lastint; i++)
                {
                    nurseRecordEntityInOut1.Add(new NurseRecordEntity());
                }
            }


            int yushu2 = nurseRecordEntityInOut2.Count % everpagecount;
            int zheshu2 = nurseRecordEntityInOut2.Count / everpagecount;
            if (yushu2 > 0)
            {
                zheshu2 = zheshu2 + 1;
                int lastint = everpagecount - yushu2;
                for (int i = 0; i < lastint; i++)
                {
                    nurseRecordEntityInOut2.Add(new NurseRecordEntity());
                }
            }

            int yushu3 = nurseRecordEntityInOut3.Count % everpagecount;
            int zheshu3 = nurseRecordEntityInOut3.Count / everpagecount;
            if (yushu3 > 0)
            {
                zheshu3 = zheshu3 + 1;
                int lastint = everpagecount - yushu3;
                for (int i = 0; i < lastint; i++)
                {
                    nurseRecordEntityInOut3.Add(new NurseRecordEntity());
                }
            }

          

            int n = 1;
            for (int i = 0; i < zheshu1; i++)
            {

                List<NurseRecordEntity> nurseRecordEntityListEve1 = new List<NurseRecordEntity>();
                List<NurseRecordEntity> nurseRecordEntityListEve2 = new List<NurseRecordEntity>();
                List<NurseRecordEntity> nurseRecordEntityListEve3 = new List<NurseRecordEntity>();
                for (int j = (n - 1) * everpagecount; j < everpagecount * n; j++)
                {
                    if (j < nurseRecordEntityInOut1.Count)
                    {
                        nurseRecordEntityListEve1.Add(nurseRecordEntityInOut1[j]);
                    }
                    if (j < nurseRecordEntityInOut2.Count)
                    {
                        nurseRecordEntityListEve2.Add(nurseRecordEntityInOut2[j]);
                    }
                    if (j < nurseRecordEntityInOut3.Count)
                    {
                        nurseRecordEntityListEve3.Add(nurseRecordEntityInOut3[j]);
                    }

                    else
                        break;
                }
                printRecordModel.NurseRecordEntityListInOut1 = nurseRecordEntityListEve1;
                printRecordModel.NurseRecordEntityListInOut2 = nurseRecordEntityListEve2;
                printRecordModel.NurseRecordEntityListInOut3 = nurseRecordEntityListEve3;
                LoadPrint(printRecordModel);
                n++;
            }



        }

        private void SetPrintOthers(int everpagecount, List<NurseRecordEntity> nurseRecordEntityList)
        {
            //对list进行处理 使其是everpagecount的倍数
            if (nurseRecordEntityList.Count == 0)
            {
                for (int i = 0; i < everpagecount; i++)
                {
                    nurseRecordEntityList.Add(new NurseRecordEntity());
                }
            }
            int yushu = nurseRecordEntityList.Count % everpagecount;
            int zheshu = nurseRecordEntityList.Count / everpagecount;
            if (yushu > 0)
            {
                zheshu = zheshu + 1;
                int lastint = everpagecount - yushu;
                for (int i = 0; i < lastint; i++)
                {
                    nurseRecordEntityList.Add(new NurseRecordEntity());
                }
            }
            int n = 1;
            for (int i = 0; i < zheshu; i++)
            {

                List<NurseRecordEntity> nurseRecordEntityListEve = new List<NurseRecordEntity>();
                for (int j = (n - 1) * everpagecount; j < everpagecount * n; j++)
                {
                    if (j < nurseRecordEntityList.Count)
                    {
                        nurseRecordEntityListEve.Add(nurseRecordEntityList[j]);
                    }
                    else
                        break;
                }
                PrintRecordModel printRecordModel = new PrintRecordModel();
                printRecordModel.PrintHeaderModel = new PrintHeaderModel();
                printRecordModel.PrintHeaderModel.PersonName = MethodSet.CurrentInPatient.Name;
                printRecordModel.PrintHeaderModel.InNo = MethodSet.CurrentInPatient.RecordNoOfHospital;
                DataTable dt = MethodSet.GetPatientInfoForThreeMeasureTable(MethodSet.CurrentInPatient.NoOfFirstPage);
                if (dt != null && dt.Rows.Count > 0)
                {
                    printRecordModel.PrintHeaderModel.InBedNo = dt.Rows[0]["outbed"].ToString();
                    printRecordModel.PrintHeaderModel.DepartName = dt.Rows[0]["dept_name"].ToString();
                }
                printRecordModel.NurseRecordEntityList = nurseRecordEntityListEve;
                LoadPrint(printRecordModel);
                n++;
            }
        }

        /// <summary>
        /// 录入按钮事件 通过recordId判断打开不同录入界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLuRu_Click(object sender, EventArgs e)
        {
            XtraForm xform = new XtraForm();
            xform.FormClosed += new FormClosedEventHandler(FormClosed);
            xform.StartPosition = FormStartPosition.CenterParent;
            xform.FormBorderStyle = FormBorderStyle.FixedDialog;
            if (this.recordId == "2")
            {
                UCOutAndInpRecord oai = new UCOutAndInpRecord();
                xform.Width = 850;
                xform.Height = 620;
                xform.Controls.Add(oai);
            }
            else if (this.recordId == "3")
            {
                UCChildNurseRecord cnr = new UCChildNurseRecord();
                xform.Width = 1170;
                xform.Height = 615;
                xform.Controls.Add(cnr);
            }
            else if (this.recordId == "4")
            {
                NoOperNurseRecordForm noOperNurseRecordForm = new NoOperNurseRecordForm();
                noOperNurseRecordForm.InitDate(this.recordId);
                noOperNurseRecordForm.FormClosed += new FormClosedEventHandler(FormClosed);
                //noOperNurseRecordForm.Width = 1250;
                //noOperNurseRecordForm.Height = 700;
                noOperNurseRecordForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                noOperNurseRecordForm.ShowDialog();
            }
            else if (this.recordId == "5")
            {
                UCDevOperNurseRecord uCDevOperNurseRecord = new UCDevOperNurseRecord();
                uCDevOperNurseRecord.InitDate(this.recordId);
                xform.Width = 1150;
                xform.Height = 750;
                xform.MaximizeBox = false;
                xform.Controls.Add(uCDevOperNurseRecord);
            }
            if (this.recordId != "4")
            {
                xform.ShowDialog();
            }
        }

        public void FormClosed(object sender, FormClosedEventArgs e)
        {
            SetOperNurseRecord();
        }

        /// <summary>
        /// 打印控件按钮进行控制
        /// </summary>
        private void SetMainToopStripFalse()
        {
            XDesigner.Report.XPrintControlExt.ControlButtons butts = xPrint.Buttons;
            //butts.MainToolStrip.Visible = false;
            butts.cmdExport.Visible = false;
            butts.cmdRefresh.Visible = false;
            butts.lblVersion2.Visible = false;

            butts.btnPageSettings.Visible = false;
            butts.cmdJumpPrint.Visible = true;
            butts.cmdPrint.Visible = true;
        
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            xPrint.PrintDocument(true);
        }
    }
}
