using DevExpress.Utils;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.Core.NursingDocuments;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.EMR.ThreeRecordAll
{
    public partial class ThreeRecordMainZT : DevBaseForm
    {
        IEmrHost m_app;
        NursingRecord nursingRecord;  //nursedocumentz中的
        Form form;

        DrectSoft.Core.NurseDocument.Controls.NursingRecord ydNursingRecord;  //ydnursedocuments中的
        string mName = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="app"></param>
        public ThreeRecordMainZT(IEmrHost app)
        {
            try
            {
                m_app = app;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// dategrid中的病人 xll 2012-11-23 把查询病人单独提取出来了
        /// </summary>
        public void ADDInpatientForm()
        {
            //    string departcode = m_app.User.CurrentDeptId;
            //    string wardcode = m_app.User.CurrentWardId;
            //    string patid = txtPatId.Text;
            //    string sql = @"select * from inpatient i where i.outhosdept=@departCode and i.outhosward=@wardcode and status in ('1501') order by outbed;";
            //    SqlParameter[] sqlParams = new SqlParameter[]
            //   {
            //        new SqlParameter("@departCode",SqlDbType.VarChar,50),
            //        new SqlParameter("@wardcode",SqlDbType.VarChar,50)
            //   };
            //    sqlParams[0].Value = departcode;
            //    sqlParams[1].Value = wardcode;
            //    DataTable dtInpatient = m_app.SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
            //    gcInpatient.DataSource = dtInpatient;
            InPatListForm inPatListForm = new InPatListForm(m_app);
            inPatListForm.gvInpatient.DoubleClick += new EventHandler(gvInpatient_DoubleClick);
            inPatListForm.gvInpatient.KeyUp += new KeyEventHandler(gvInpatient_KeyUp);
            inPatListForm.Dock = DockStyle.Fill;
            inPatListForm.TopLevel = false;
            inPatListForm.FormBorderStyle = FormBorderStyle.None;
            inPatListForm.Show();
            panelControl1.Controls.Clear();
            panelControl1.Controls.Add(inPatListForm);
            gvInpatient_DoubleClick(inPatListForm.gvInpatient, null);

        }

        void gvInpatient_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            gvInpatient_DoubleClick(sender, null);
        }

        /// <summary>
        /// 双击病人列表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvInpatient_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var gvInpatient = (sender as DevExpress.XtraGrid.Views.Grid.GridView);
                if (gvInpatient == null) { return; }
                DataRow drInpatient = gvInpatient.GetFocusedDataRow();
                if (drInpatient == null || drInpatient["NOOFINPAT"] == null) return;
                if (mName.Contains("DrectSoft.Core.YDNurseDocument"))
                {
                    LoadYDNurseDocument(drInpatient);
                }
                else
                {
                    LoadNurseDocument(drInpatient);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 最新版三测单界面
        /// Modify by xlb 2013-05-03
        /// </summary>
        /// <param name="drInpatient"></param>
        private void LoadYDNurseDocument(DataRow drInpatient)
        {
            try
            {
                WaitDialogForm waitDialog = new WaitDialogForm("正在绘制三测单", "请您稍后！");

                //获取病人对象
                Inpatient m_NewPat = new Inpatient();
                m_app.ChoosePatient(Convert.ToDecimal((drInpatient["NOOFINPAT"])).ToString(), out m_NewPat);
                //Inpatient m_NewPat = new Inpatient(Convert.ToDecimal(drInpatient["NOOFINPAT"]));
                DrectSoft.Core.NurseDocument.MainNursingMeasure mainNursingMeasure = new DrectSoft.Core.NurseDocument.MainNursingMeasure(drInpatient["NOOFINPAT"].ToString());
                string version = DrectSoft.Core.NurseDocument.ConfigInfo.GetNurseMeasureVersion(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                mainNursingMeasure.CurrentPat = drInpatient["NOOFINPAT"].ToString();

                mainNursingMeasure.eventHandlerXieRu += delegate(object sender1, EventArgs e1)
                {
                    #region 注销 by xlb 2013-05-06
                    //if (ydNursingRecord == null)
                    //{
                    //    ydNursingRecord = new DrectSoft.Core.YDNurseDocument.Controls.NursingRecord(m_app, drInpatient["NOOFINPAT"].ToString());
                    //    ydNursingRecord.SetQieHuanInpatVisible(false);
                    //    //ydNursingRecord.TopMost = true;
                    //    ydNursingRecord.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
                    //    {
                    //        ydNursingRecord = null;
                    //        mainNursingMeasure.LoadDataImage(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                    //    };
                    //    ydNursingRecord.Show(this);
                    //}
                    #endregion
                    if (form == null)
                    {
                        Assembly a = Assembly.Load("DrectSoft.Core.YDNurseDocument");
                        Type type = a.GetType(version);
                        form = (Form)Activator.CreateInstance(type, new object[] { m_app, drInpatient["NOOFINPAT"].ToString() });
                        form.Height = DrectSoft.Core.NurseDocument.ConfigInfo.GetNurseRecordSize(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                        form.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
                        {
                            form = null;
                            mainNursingMeasure.LoadDataImage(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                        };
                        form.Show(this);

                    }
                    mainNursingMeasure.LoadDataImage(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                };

                mainNursingMeasure.Load(m_app, m_NewPat);


                mainNursingMeasure.ReadOnlyControl = true;
                scrolThreeRecord.Controls.Clear();
                scrolThreeRecord.Controls.Add(mainNursingMeasure);
                mainNursingMeasure.Dock = DockStyle.Fill;
                waitDialog.Hide();
                #region 注销 by xlb 2013-05-06
                //if (ydNursingRecord != null)
                //{
                //    ydNursingRecord.RefreshForm(drInpatient["NOOFINPAT"].ToString());
                //    ydNursingRecord.RefreshDate(drInpatient["NOOFINPAT"].ToString());
                //    ydNursingRecord.dateEdit_DateTimeChanged(null, null);
                //    ydNursingRecord.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
                //    {
                //        mainNursingMeasure.LoadDataImage(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                //    };
                //}
                #endregion
                if (form != null)
                {
                    switch (version.Trim())
                    {
                        case "DrectSoft.Core.YDNurseDocument.Controls.NursingRecordNew":
                            (form as DrectSoft.Core.NurseDocument.Controls.NursingRecordNew).RefreshDate(drInpatient["NOOFINPAT"].ToString());
                            (form as DrectSoft.Core.NurseDocument.Controls.NursingRecordNew).dateEdit_DateTimeChanged(null, null);

                            break;
                        case "DrectSoft.Core.YDNurseDocument.Controls.NursingRecord":
                            (form as DrectSoft.Core.NurseDocument.Controls.NursingRecord).RefreshDate(drInpatient["NOOFINPAT"].ToString());
                            (form as DrectSoft.Core.NurseDocument.Controls.NursingRecord).dateEdit_DateTimeChanged(null, null);

                            break;
                    }
                    form.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
                    {
                        mainNursingMeasure.LoadDataImage(decimal.Parse(drInpatient["NOOFINPAT"].ToString()));
                    };
                }
                waitDialog.Hide();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// nursedocument界面
        /// </summary>
        /// <param name="drInpatient"></param>
        private void LoadNurseDocument(DataRow drInpatient)
        {
            WaitDialogForm waitDialog = new WaitDialogForm("正在绘制三测单", "请您稍后！");
            Inpatient m_NewPat = new Inpatient();
            m_app.ChoosePatient(Convert.ToDecimal((drInpatient["NOOFINPAT"])).ToString(), out m_NewPat);
            //Inpatient m_NewPat = new Inpatient(Convert.ToDecimal(drInpatient["NOOFINPAT"]));
            MainNursingMeasure mainNursingMeasure = new MainNursingMeasure(drInpatient["NOOFINPAT"].ToString());
            mainNursingMeasure.eventHandlerDaoRu += delegate(object sender1, EventArgs e1)
            {
                if (nursingRecord == null)
                {
                    nursingRecord = new NursingRecord(m_app, m_NewPat);
                    nursingRecord.SetQieHuanInpatVisible(false);
                    //nursingRecord.TopMost = true;
                    nursingRecord.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
                    {
                        nursingRecord = null;
                        mainNursingMeasure.ucThreeMeasureTable.LoadData();
                        mainNursingMeasure.ucThreeMeasureTable.Refresh();
                    };
                }
                nursingRecord.Show(this);
                mainNursingMeasure.ucThreeMeasureTable.LoadData();
                mainNursingMeasure.ucThreeMeasureTable.Refresh();
            };

            mainNursingMeasure.Load(m_app, m_NewPat);

            mainNursingMeasure.ReadOnlyControl = true;
            scrolThreeRecord.Controls.Clear();
            scrolThreeRecord.Controls.Add(mainNursingMeasure);
            mainNursingMeasure.Dock = DockStyle.Fill;
            waitDialog.Hide();
            if (nursingRecord != null)
            {
                nursingRecord.RefreshDate(m_NewPat);
                nursingRecord.dateEdit_DateTimeChanged(null, null);
                nursingRecord.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
                {
                    mainNursingMeasure.ucThreeMeasureTable.LoadData();
                    mainNursingMeasure.ucThreeMeasureTable.Refresh();
                };
            }
            waitDialog.Hide();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreeRecordMain_Load(object sender, EventArgs e)
        {
            try
            {
                GetNurseDocumentConfig();
                ADDInpatientForm();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 获取三测单
        /// </summary>
        private void GetNurseDocumentConfig()
        {
            try
            {
                string sql = @"select * from dict_catalog where ccode='13'";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    mName = dt.Rows[0]["MNAME"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}