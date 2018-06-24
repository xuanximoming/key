using DevExpress.Utils;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace DrectSoft.EMR.ThreeRecordAll
{
    public partial class ThreeRecordMainZT : DevBaseForm
    {
        IEmrHost m_app;
        //NursingRecord nursingRecord;  //nursedocumentz中的


        //DrectSoft.Core.NurseDocument.Controls.NursingRecord ydNursingRecord;  //ydnursedocuments中的
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
                if (mName.Contains("DrectSoft.Core.NurseDocument"))
                {
                    LoadDCNurseDocument(drInpatient);
                }
                else
                {
                    //LoadNurseDocument(drInpatient);
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
        private void LoadDCNurseDocument(DataRow drInpatient)
        {

            try
            {
                //修改调用方式，防止项目互相调用
                WaitDialogForm waitDialog = new WaitDialogForm("正在绘制三测单", "请您稍后！");
                //加载dll文件
                Assembly AmainNursingMeasure = Assembly.Load("DrectSoft.Core.NurseDocument");
                //获取类
                Type TypeM = AmainNursingMeasure.GetType("DrectSoft.Core.NurseDocument.LoadNurseDocument");
                //实例化一个类
                object obj = Activator.CreateInstance(TypeM);
                //创建方法
                MethodInfo m = TypeM.GetMethod("MyLoadNurseDocument");
                //参数对象  
                object[] p = new object[] { this, m_app, drInpatient, null };
                m.Invoke(obj, p);

                if (p[3] != null)
                {
                    XtraUserControl XtraUc = p[3] as XtraUserControl;
                    scrolThreeRecord.Controls.Clear();
                    scrolThreeRecord.Controls.Add(XtraUc);
                    XtraUc.Dock = DockStyle.Fill;
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
        //private void LoadNurseDocument(DataRow drInpatient)
        //{
        //    WaitDialogForm waitDialog = new WaitDialogForm("正在绘制三测单", "请您稍后！");
        //    Inpatient m_NewPat = new Inpatient();
        //    m_app.ChoosePatient(Convert.ToDecimal((drInpatient["NOOFINPAT"])).ToString(), out m_NewPat);
        //    MainNursingMeasure mainNursingMeasure = new MainNursingMeasure(drInpatient["NOOFINPAT"].ToString());
        //    mainNursingMeasure.eventHandlerDaoRu += delegate(object sender1, EventArgs e1)
        //    {
        //        if (nursingRecord == null)
        //        {
        //            nursingRecord = new NursingRecord(m_app, m_NewPat);
        //            nursingRecord.SetQieHuanInpatVisible(false);
        //            nursingRecord.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
        //            {
        //                nursingRecord = null;
        //                mainNursingMeasure.ucThreeMeasureTable.LoadData();
        //                mainNursingMeasure.ucThreeMeasureTable.Refresh();
        //            };
        //        }
        //        nursingRecord.Show(this);
        //        mainNursingMeasure.ucThreeMeasureTable.LoadData();
        //        mainNursingMeasure.ucThreeMeasureTable.Refresh();
        //    };

        //    mainNursingMeasure.Load(m_app, m_NewPat);

        //    mainNursingMeasure.ReadOnlyControl = true;
        //    scrolThreeRecord.Controls.Clear();
        //    scrolThreeRecord.Controls.Add(mainNursingMeasure);
        //    mainNursingMeasure.Dock = DockStyle.Fill;
        //    waitDialog.Hide();
        //    if (nursingRecord != null)
        //    {
        //        nursingRecord.RefreshDate(m_NewPat);
        //        nursingRecord.dateEdit_DateTimeChanged(null, null);
        //        nursingRecord.FormClosed += delegate(object sender2, FormClosedEventArgs e2)
        //        {
        //            mainNursingMeasure.ucThreeMeasureTable.LoadData();
        //            mainNursingMeasure.ucThreeMeasureTable.Refresh();
        //        };
        //    }
        //    waitDialog.Hide();
        //}

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