using Consultation.NEW;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Core.RedactPatientInfo;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.OwnBedInfo
{
    public partial class UserControlAllListBedInfo : UserControl
    {
        #region private members
        /// <summary>
        /// 科室代码
        /// </summary>
        private string m_DeptId;
        /// <summary>
        /// 病区代码
        /// </summary>
        private string m_WardId;

        //跟产科相关
        private bool _flagObstetricWard;
        internal bool FlagObstetricWard
        {
            get
            {
                //产科标志不做考虑直接传FALSE
                _flagObstetricWard = false;// CheckObstetricWard(m_WardId);
                return _flagObstetricWard;
            }
        }

        /// <summary>
        /// 欠费病人姓名颜色为红
        /// </summary>
        private static Color BackPayColor = Color.Red;
        /// <summary>
        /// 非欠费病人姓名颜色为黑
        /// </summary>
        private static Color NotBackPayColor = Color.Black;

        /// <summary>
        /// 是否允许设置我的患者
        /// </summary>
        public bool NeedUndoMyInpatient { get; set; }

        #endregion

        #region Layout setting members
        private ImageList m_ImageListzifei;

        /// <summary>
        /// 连接数据库的SqlHelper
        /// </summary>
        private IDataAccess m_DataAccessEmrly;
        private DataManager m_DataManager;



        #endregion

        #region properties
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        private IEmrHost _app;
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        public IEmrHost App
        {
            get { return _app; }
            set { _app = value; }
        }

        private bool _forceRefresh = false;
        /// <summary>
        /// 是否强制刷新数据源
        /// </summary>
        internal bool ForceRefresh
        {
            get
            {
                return _forceRefresh;
            }
            set
            {
                _forceRefresh = value;
            }
        }

        /// <summary>
        /// 当前选中病患首页序号
        /// </summary>
        public Decimal CurrNoOfInpat
        {
            get
            {
                return GetCurrentPat();
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// 病区一览全部病患LIST模式
        /// </summary>
        /// <param name="application"></param>
        public UserControlAllListBedInfo(IEmrHost application)
        {
            InitializeComponent();
            App = application;
            gridViewGridWardPat.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(gridViewGridWardPat_CustomDrawRowIndicator);
            DS_SqlHelper.CreateSqlHelper();
            //edit by Yanqiao.Cai 2012-11-08
            //CopntrolModPatient();
            //加在UserControlAllListBedInfo_Load窗体Load事件中SetButtonState
        }

        #region "已弃用 -- 改用SetButtonState方法 by cyq 2012-11-08 方法统一封装"
        //private void CopntrolModPatient()
        //{

        //    if (GetConfigValueByKey("ManualMaintainBasicInfo") == "1")
        //    {
        //        barButtonItem1.Visibility = BarItemVisibility.Never;
        //        btnChangePatient.Visibility = BarItemVisibility.Always;
        //        barButtonItem8.Visibility = BarItemVisibility.Always;
        //    }
        //    else
        //    {
        //        barButtonItem1.Visibility = BarItemVisibility.Always;
        //        btnChangePatient.Visibility = BarItemVisibility.Never;
        //        barButtonItem8.Visibility = BarItemVisibility.Never;
        //    }

        //    //从his查病人信息功能
        //    if (GetConfigValueByKey("GetInpatientForHis") == "1")
        //    {
        //        barButtonItem1.Visibility = BarItemVisibility.Always;
        //    }
        //    else
        //    {

        //        barButtonItem1.Visibility = BarItemVisibility.Never;
        //    }

        //    string simpledoctor = GetConfigValueByKey("SimpleDoctorCentor");
        //    if (simpledoctor == "1")
        //    {
        //        //右键会诊申请和临床路径控制
        //        barButtonItem3.Visibility = barButtonItemConsultationApply.Visibility = BarItemVisibility.Never;
        //    }
        //    else
        //    {
        //        barButtonItem3.Visibility = barButtonItemConsultationApply.Visibility = BarItemVisibility.Always;
        //    }
        //}
        #endregion

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-08
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UserControlAllListBedInfo_Load(object sender, EventArgs e)
        {
            try
            {
                this.SuspendLayout();

                InitApp(App);
                this.ResumeLayout();
                //add by Yanqiao.Cai 2012-11-08
                SetButtonState();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设置入院日期为入院时间还是入科日期（依据配置）
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-14</date>
        public void SetInHosOrInWardDate()
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                if (!string.IsNullOrEmpty(config))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(config);
                    XmlNodeList nodeList = doc.GetElementsByTagName("InHosTimeType");
                    if (null != nodeList && nodeList.Count > 0)
                    {
                        string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                        if (cfgValue == "1")
                        {//入科
                            gridViewGridWardPat.Columns[9].FieldName = "INWARDDATE";
                        }
                        else
                        {//入院
                            gridViewGridWardPat.Columns[9].FieldName = "ADMITDATE";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 得到配置信息  wyt 2012年8月27日
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string key)
        {
            try
            {
                string sql1 = " select * from appcfg where configkey = '" + key + "'  ";
                DataTable dt = App.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
                string config = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    config = dt.Rows[0]["value"].ToString();
                }
                return config;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 显示序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridViewGridWardPat_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            DS_Common.AutoIndex(e);
        }
        #endregion


        public delegate void del_ShowVigals(string noofinpat);
        public event del_ShowVigals ShowVigalsHandle;
        public void OnShowVigals(string noofinpat)
        {
            if (ShowVigalsHandle != null)
            {
                ShowVigalsHandle(noofinpat);
            }
        }
        NursingRecordForm nursingRecordForm;

        private string m_DefaultRowFilter = string.Empty;

        /// <summary>
        /// 获取焦点
        /// edit by Yanqiao.Cai 2012-11-08
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FocusFirstControl()
        {
            try
            {
                textEditBedNo.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重置查询条件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-22</date>
        /// </summary>
        public void Reset()
        {
            try
            {
                textEditPatientSN.Text = "";
                textEditPatientName.Text = "";
                textEditBedNo.Text = "";
                textEditInwDia.Text = "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GetDefaultRowFilter()
        {
            DataView dv = gridMain.DataSource as DataView;
            if (dv != null)
            {
                m_DefaultRowFilter = dv.RowFilter;
            }
        }

        #region events
        StringFormat s = new StringFormat();
        public void gridViewGridWardPat_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle < 0)
                return;
            if (e.Column.FieldName.ToUpper() == PatientWardField.Fieldhzxm.ToUpper())
            {
                DataRow dr = gridViewGridWardPat.GetDataRow(e.RowHandle);
                if ((!string.IsNullOrEmpty(dr[PatientWardField.Fieldfee].ToString())) && (Convert.ToSingle(dr[PatientWardField.Fieldfee]) < ConstResource.OweLine))
                    e.Appearance.ForeColor = BackPayColor;
                else
                    e.Appearance.ForeColor = NotBackPayColor;
            }
            s.Alignment = StringAlignment.Near;
            s.LineAlignment = StringAlignment.Center;
            if (e.CellValue == null) return;
            DataRowView drv = gridViewGridWardPat.GetRow(e.RowHandle) as DataRowView;
            //取得病人名字
            string patname = drv["patname"].ToString().Trim();

            if (e.Column == colname)
            {
                if (patname.Contains("婴儿"))
                {
                    Region oldRegion = e.Graphics.Clip;
                    e.Graphics.Clip = new Region(e.Bounds);

                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                    e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red,
                        new RectangleF(e.Bounds.Location, new SizeF(300, e.Bounds.Height)), s);

                    e.Graphics.Clip = oldRegion;
                    e.Handled = true;
                }
            }
        }


        #endregion

        #region methods
        protected virtual void InitApp(IEmrHost application)
        {
            try
            {
                App = application;
                m_DataAccessEmrly = App.SqlHelper;
                InitializeImage();

                m_DataManager = new DataManager(application, App.User.CurrentDeptId, App.User.CurrentWardId);
                AddEventHander();
                this.Text = ConstResource.AppName;
                InitAppThread();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitAppThread()
        {
            panelControlPatientList.Visible = true;
            Thread thread = new Thread(new ThreadStart(InitAppInner));
            thread.Start();
        }

        public void InitAppInner()
        {
            try
            {
                InitAppInvoke(GetAllPats());
            }
            catch (Exception)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("捞取病人出错！");
            }
        }

        public void InitAppInvoke(DataTable dt)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<DataTable>(InitAppInvoke), dt);
                }
                else
                {
                    this.gridMain.DataSource = dt.Copy();
                    GetDefaultRowFilter();
                    panelControlPatientList.Visible = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region "edit by cyq 2012-11-08 方法统一封装 现使用SetButtonState "
        /// <summary>
        /// 判断“撤销我的病人”功能的显示与否
        /// </summary>
        //private void InitUndoMyInpatient()
        //{
        //    btn_ownInp.Visibility = NeedUndoMyInpatient.Equals(true) ? BarItemVisibility.Always : BarItemVisibility.Never;
        //}
        #endregion

        /// <summary>
        /// 注册事件
        /// </summary>
        private void AddEventHander()
        {
            gridViewGridWardPat.CustomDrawCell += new RowCellCustomDrawEventHandler(gridViewGridWardPat_CustomDrawCell);
        }

        /// <summary>
        /// 
        /// </summary>
        public DataTable GetAllPats()
        {
            try
            {
                //设置入院日期为入院时间还是入科日期（依据配置）add by cyq 2013-03-14
                SetInHosOrInWardDate();
                DataTable dtSource = m_DataManager.TableWard;
                //要进行处理的DataTAble，处理 病人姓名字段(***病人姓名【1个婴儿】***)
                string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
                for (int i = 0; i < dtSource.Rows.Count; i++)
                {
                    ResultName = DataManager.GetPatsBabyContent(App, dtSource.Rows[i]["noofinpat"].ToString());
                    dtSource.Rows[i]["PatName"] = ResultName;
                }
                return dtSource;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RefreshInpatientList()
        {
            try
            {
                //连续两次撤销我的病人就会报错的bug
                //edit by ywk 2012年9月20日 13:43:40 
                if (m_DataManager == null)
                {
                    m_DataManager = new DataManager(App, App.User.CurrentDeptId, App.User.CurrentWardId);
                }
                RefreshInpatientListThread();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RefreshInpatientListThread()
        {
            try
            {
                panelControlPatientList.Visible = true;
                Thread thread = new Thread(new ThreadStart(RefreshInpatientListInner));
                thread.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RefreshInpatientListInner()
        {
            try
            {
                m_DataManager.RefreshWardData();
                RefreshInpatientListInvoke(GetAllPats());
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RefreshInpatientListInvoke(DataTable dt)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<DataTable>(RefreshInpatientListInvoke), dt);
                }
                else
                {
                    this.gridMain.DataSource = dt.Copy();
                    panelControlPatientList.Visible = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 初始化图片
        /// </summary>
        private void InitializeImage()
        {
            try
            {
                ImageHelper.GetImageListIllness();
                imageListCustomwzjb = ImageHelper.GetImageListIllness();
                repItemImageComboBoxwzjb.SmallImages = imageListCustomwzjb;
                DataTable dt = null;
                if ((dt == null) || (dt.Rows.Count <= 0))
                {
                    ImageComboBoxItem item1 = new ImageComboBoxItem("一般病人", "0", 0);
                    ImageComboBoxItem item2 = new ImageComboBoxItem("危重病人", "1", 1);
                    ImageComboBoxItem item3 = new ImageComboBoxItem("病重病人", "2", 2);
                    repItemImageComboBoxwzjb.Items.AddRange(new ImageComboBoxItem[] { item1, item2, item3 });
                }
                else
                {
                    ImageComboBoxItem[] imageCombo = new ImageComboBoxItem[dt.Rows.Count];
                    for (int index = 0; index < imageCombo.Length; index++)
                    {
                        ImageComboBoxItem item = new ImageComboBoxItem(dt.Rows[index]["name"].ToString().Trim(), dt.Rows[index]["mxdm"].ToString().Trim(), Convert.ToInt16(dt.Rows[index]["mxdm"].ToString().Trim()));
                        imageCombo[index] = item;
                    }
                    repItemImageComboBoxwzjb.Items.AddRange(imageCombo);
                }
                imageListcwdm = ImageHelper.GetImageListBedNum();
                m_ImageListzifei = ImageHelper.GetImageListPay();
                imageListBrxb = ImageHelper.GetImageListBrxb();
                repItemImageComboBoxBrxb.SmallImages = imageListBrxb;
                ImageComboBoxItem ImageComboItemMale = new ImageComboBoxItem("男", "1", 1);
                ImageComboBoxItem ImageComboItemFemale = new ImageComboBoxItem("女", "2", 0);
                ImageComboBoxItem ImageComboItemUnknow = new ImageComboBoxItem("未知", "3", 1);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemMale);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemFemale);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemUnknow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刷新
        /// edit by Yanqiao.Cai 2012-11-08
        /// add try ... catch
        /// </summary>
        public void RefreshControl()
        {
            try
            {
                this.SuspendLayout();
                InitApp(App);
                this.ResumeLayout();
                //add by Yanqiao.Cai 2012-11-08
                SetButtonState();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        /// <summary>
        /// 
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、双击小标题无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridMain_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewGridWardPat.CalcHitInfo(gridMain.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0) { return; }

                LoadRecordInput();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 进入文书录入
        /// </summary>
        public void LoadRecordInput()
        {
            decimal syxh = GetCurrentPat();
            if (syxh < 0) return;

            //add by  ywk 
            DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
            string noofinpat = dataRow["noofinpat"].ToString();

            if (DataManager.HasBaby(noofinpat))
            {
                //MessageBox.Show("有！");
                ChoosePatOrBaby choosepat = new ChoosePatOrBaby(App, noofinpat);
                choosepat.StartPosition = FormStartPosition.CenterParent;
                if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    App.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                    App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                }
            }

            else
            {
                App.ChoosePatient(syxh);
                App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
        }

        /// <summary>
        /// 获取当前病患
        /// </summary>
        /// <returns></returns>
        private decimal GetCurrentPat()
        {

            if (gridViewGridWardPat.FocusedRowHandle < 0)
                return -1;
            else
            {

                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null) return -1;

                return Convert.ToDecimal(dataRow["NoOfInpat"]);
            }

        }

        /// <summary>
        /// 右键事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、右键小标题无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewGridWardPat_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewGridWardPat.CalcHitInfo(gridMain.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                if (e.Button == MouseButtons.Right)
                {
                    this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病人信息事件
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法重新封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ViewPatientInfo();
            }
            catch (Exception)
            {
                App.CustomMessageBox.MessageShow("查看病人信息失败！");
            }
        }

        /// <summary>
        /// 病人信息方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        public void ViewPatientInfo()
        {
            try
            {
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null || string.IsNullOrEmpty(dataRow["NoOfInpat"].ToString())) return;
                //to do 调用病患基本信息窗体
                //BasePatientInfo info = new BasePatientInfo(App);
                //info.ShowCurrentPatInfo(dataRow["NoOfInpat"].ToString());
                XtraFormPatientInfo patientInfo = new XtraFormPatientInfo(App, dataRow["NoOfInpat"].ToString());
                patientInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 临床路径事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ClinicalPath();
            }
            catch (Exception)
            {
                App.CustomMessageBox.MessageShow("查看病人信息失败！");
            }
        }

        /// <summary>
        /// 临床路径方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        public void ClinicalPath()
        {
            try
            {
                decimal syxh = GetCurrentPat();
                if (syxh < 0) return;
                App.ChoosePatient(syxh);
                App.LoadPlugIn("DrectSoft.Core.DoctorTasks.dll", "DrectSoft.Core.DoctorTasks.InpatientPathForm");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 文书录入
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DocumentsWrite();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 文书录入方法 --- 全部病人
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        public void DocumentsWrite()
        {
            try
            {
                decimal syxh = GetCurrentPat();
                if (syxh < 0) return;
                App.ChoosePatient(syxh);
                App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设为我的病人事件
        /// edit by Yanqiao.Cai 2012-11-08
        /// 1、add try ... catch
        /// 2、方法提取封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ownInp_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SetMyPatient();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设为我的病人方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-08</date>
        /// </summary>
        public void SetMyPatient()
        {
            try
            {
                decimal FPNo;
                if (gridViewGridWardPat.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择一条病人记录。");
                    return;
                }
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null)//add by xlb 2012-12-25
                {
                    return;
                }
                string syxh = dataRow["NoOfInpat"].ToString();
                if ((!string.IsNullOrEmpty(syxh)) && (syxh != "-1"))
                {
                    FPNo = Convert.ToDecimal(syxh);
                    if ((FPNo > 0) && (MyMessageBox.Show("您确定要将 " + dataRow["PATNAME"].ToString() + " 设为我的分管病人吗？", "提示信息", MyMessageBoxButtons.OkCancel) == DialogResult.OK))
                    {
                        string str = string.Format(@"SELECT Users.Name Users_Name,Doctor_AssignPatient.* 
                                                        FROM dbo.Doctor_AssignPatient Doctor_AssignPatient 
                                                        LEFT JOIN dbo.Users on Users.ID = Doctor_AssignPatient.ID
                                                        WHERE Doctor_AssignPatient.Valid = 1  
                                                        AND Doctor_AssignPatient.NoOfInpat = '{0}'", FPNo);

                        DataTable dt = this.m_DataAccessEmrly.ExecuteDataTable(str);
                        //判断该病人是否有分管医生
                        if (dt.Rows.Count > 0)
                        {
                            //判断是否为为的分管病人
                            if (dt.Rows[0]["ID"].ToString().Trim() == this._app.User.Id)
                            {
                                MessageBox.Show(dataRow["PATNAME"].ToString() + " 已经是我的分管病人");
                                return;
                            }
                            //强制将当前病人设置为我的分管病人
                            else if (MyMessageBox.Show(string.Format(dataRow["PATNAME"].ToString() + " 已有分管医生【{0}】，是否强制设为我的分管病人？", dt.Rows[0]["Users_Name"]), "提示信息", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                            {
                                str = string.Format(@"UPDATE Doctor_AssignPatient SET ID = '{0}',
                                                        Create_time = sysdate,
                                                        Create_user = '{0}'
                                                        WHERE NoOfInpat = '{1}'
                                                        AND Valid = 1",
                                                        this._app.User.Id,
                                                        FPNo);
                                this.m_DataAccessEmrly.ExecuteNoneQuery(str);
                            }
                        }
                        else
                        {
                            //str = @"insert into Doctor_AssignPatient(id, noofinpat, valid, create_time, create_user) 
                            //values ('" + this._app.User.Id + "'," + FPNo + ",1,sysdate,'" + this._app.User.Id + "')";
                            str = @"insert into  Doctor_AssignPatient(id, noofinpat, valid, create_time, create_user) values(@userid,@noofinpat,@valid,sysdate,@userid)";//edit by xlb 2012-12-24
                            SqlParameter[] sps ={
                                               new SqlParameter("@userid",_app.User.Id),
                                               new SqlParameter("@noofinpat",FPNo),
                                               new SqlParameter("@valid",1),
                                               };
                            //this.m_DataAccessEmrly.ExecuteNoneQuery(str);
                            DS_SqlHelper.ExecuteNonQuery(str, sps, CommandType.Text);
                        }

                        //刷新我的病人
                        if (this.Parent != null && this.Parent.Parent != null && this.Parent.Parent.Parent != null)
                        {
                            DocCenter docCenter = this.Parent.Parent.Parent as DocCenter;
                            if (docCenter != null)
                            {
                                docCenter.InitMyInpatient();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        int oldFocusRowHandle;
        /// <summary>
        /// 设定婴儿功能的添加
        /// add by ywk 2012年6月6日 14:15:15 
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetMyBaby_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SetBabys();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设定婴儿方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        public void SetBabys()
        {
            try
            {
                if (gridViewGridWardPat.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null)//add by xlb 2012-12-24
                {
                    return;
                }
                string syxh = dataRow["NoOfInpat"].ToString();
                string patname = dataRow["PatName"].ToString();
                if ((!string.IsNullOrEmpty(syxh)) && (syxh != "-1"))
                {
                    oldFocusRowHandle = gridViewGridWardPat.FocusedRowHandle;
                    SetPatientsBaby setBaby = new SetPatientsBaby(syxh, _app, patname, this);
                    setBaby.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
                    setBaby.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataRow GetSelectedGridViewRow()
        {
            DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
            return dataRow;
        }

        private void textEditPatientSN_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                refreshGridView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditPatientName_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                refreshGridView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void textEditBedNo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                refreshGridView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void refreshGridView()
        {
            try
            {
                string PatientSN = textEditPatientSN.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
                string PatientName = textEditPatientName.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
                string BedNo = textEditBedNo.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
                string InwDia = textEditInwDia.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");

                string filter = " PATID like '%{0}%' and (PatName like '%{1}%' or PY like '%{1}%' or WB like '%{1}%') and (BedID like '%{2}%' or '{2}' is null or '{2}'='') and (ZDMC like '%{3}%' or '{3}' is null or '{3}' = '') ";
                //DataView dv = gridMain.DataSource as DataView;

                DataTable dv = gridMain.DataSource as DataTable;
                if (dv != null)
                {
                    if (m_DefaultRowFilter != "")
                    {
                        dv.DefaultView.RowFilter = m_DefaultRowFilter + " and " + string.Format(filter, PatientSN, PatientName, BedNo, InwDia);
                    }
                    else
                    {
                        dv.DefaultView.RowFilter = string.Format(filter, PatientSN, PatientName, BedNo, InwDia);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void gridMain_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawString(labelControlBedNO.Text, labelControlBedNO.Font, new SolidBrush(Color.Black), labelControlBedNO.Location);
            //e.Graphics.DrawString(labelControlName.Text, labelControlName.Font, new SolidBrush(Color.Black), labelControlName.Location);
            //e.Graphics.DrawString(labelControlPatientSN.Text, labelControlPatientSN.Font, new SolidBrush(Color.Black), labelControlPatientSN.Location);
        }


        /// <summary>
        /// 从设定婴儿界面返回到这个界面要相应的刷新在那个页面编辑过的病人
        /// add by  ywk  2012年6月12日 08:53:31 
        /// </summary>
        /// <param name="EditedPats"></param>
        internal void RefreshPat(System.Collections.ArrayList EditedPats)
        {
            //设置入院日期为入院时间还是入科日期（依据配置）add by cyq 2013-03-14
            SetInHosOrInWardDate();

            DataTable dataTableOper = gridMain.DataSource as DataTable;
            dataTableOper = dataTableOper.Copy();
            string ResultName = string.Empty;
            string m_noofinpat = string.Empty;
            for (int i = 0; i < dataTableOper.Rows.Count; i++)
            {
                m_noofinpat = dataTableOper.Rows[i]["Noofinpat"].ToString();

                for (int j = 0; j < EditedPats.Count; j++)
                {
                    if (EditedPats[j].ToString() == m_noofinpat)
                    {
                        ResultName = DataManager.GetPatsBabyContent(App, m_noofinpat);
                        dataTableOper.Rows[i]["PatName"] = ResultName;
                    }
                }
            }
            this.gridMain.DataSource = dataTableOper;
            this.gridViewGridWardPat.FocusedRowHandle = oldFocusRowHandle;
        }

        /// <summary>
        /// 会诊申请事件
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemConsultationApply_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ApplyConsult();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 会诊申请方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        public void ApplyConsult()
        {
            try
            {
                //decimal syxh = GetCurrentPat();
                //if (syxh < 0) return;
                //App.ChoosePatient(syxh);

                //FormConsultationApply formApply = new FormConsultationApply(syxh.ToString(), App, true);
                //formApply.StartPosition = FormStartPosition.CenterParent;
                //formApply.ShowDialog();
                decimal syxh = GetCurrentPat();
                if (syxh < 0)
                {
                    return;
                }
                //调用新的会诊申请界面
                FormApplyForMultiply formApply = new FormApplyForMultiply(syxh.ToString(), App, "", false);
                formApply.StartPosition = FormStartPosition.CenterParent;
                formApply.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 院内会诊 --- 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_consultation_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                _app.LoadPlugIn("DrectSoft.Core.Consultation.dll", "DrectSoft.Core.Consultation.ConsultationFormStartUp");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 入院登记 --- 右键
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-05</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_inHos_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                InHosLogin();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 入院登记方法
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-05</date>
        private void InHosLogin()
        {
            try
            {
                XtraFormInHosLogin patientInfo = new XtraFormInHosLogin(_app, null);
                patientInfo.ShowDialog();
                //add by cyq 2012-11-15 入院登记后刷新
                if (patientInfo.refreashFlag)
                {
                    RefreshInpatientList();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary> 
        /// 更新病人  晋城需求  xll
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangePatient_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                EditPatientInfo();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑病人信息方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        public void EditPatientInfo()
        {
            try
            {
                if (gridViewGridWardPat.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选中一条记录");
                    return;
                }
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null)//add by xlb 2012-12-24
                {
                    return;
                }
                string syxh = dataRow["Noofinpat"].ToString();

                XtraFormInHosLogin patientInfo = new XtraFormInHosLogin(App, syxh);
                patientInfo.SetEnable(false);
                patientInfo.Text = "编辑病人信息";
                patientInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 病人出院
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SetPatientOutHos();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病人出院方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        /// edit by xlb 2012-12-24
        public void SetPatientOutHos()
        {
            try
            {
                if (gridViewGridWardPat.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择一条病人记录。");
                    return;
                }
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null)//add by xlb 2012-12-24
                {
                    return;
                }
                string syxh = dataRow["NoOfInpat"].ToString();
                string patname = dataRow["PatName"].ToString();
                if ((!string.IsNullOrEmpty(syxh)))
                {
                    //DialogResult dResult = App.CustomMessageBox.MessageShow("确定让病人出院吗？", CustomMessageBoxKind.QuestionYesNo);
                    //if (dResult == DialogResult.Yes)
                    if (MessageBox.Show("您确定让 " + dataRow["PATNAME"].ToString() + " 出院吗？", "病人出院", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //string sql = string.Format("update inpatient i set i.status=1503 and i.outhosdept='{0}' and i.outhosward='{1}' and i.outwarddate='{2}' and i.outhosdate='{3}' where inpatient.noofinpat={4}",
                        // m_App.User.CurrentDeptId,
                        //m_App.User.CurrentWardId,
                        //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 
                        //Convert.ToInt32(syxh));
                        // m_App.SqlHelper.ExecuteNoneQuery(sql, CommandType.Text);
                        //xll i.emrouthos='1' 电子病历出院标记 2013-08-01
                        string sql = "update inpatient i set i.status=1503,i.emrouthos='1',i.outhosdept=@outhostdept, i.outhosward=@outhostward,i.outwarddate=@outwarddate,i.outhosdate=@outhostdate where i.noofinpat=@noofinpat";//add by xlb 2012-12-24
                        SqlParameter[] sps ={
                                               new SqlParameter("@outhostdept",App.User.CurrentDeptId),
                                               new SqlParameter("@outhostward",App.User.CurrentWardId),
                                               new SqlParameter("@outwarddate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                               new SqlParameter("@outhostdate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                               new SqlParameter("@noofinpat",Convert.ToInt32(syxh))
                                            };
                        DS_SqlHelper.ExecuteNonQuery(sql, sps, CommandType.Text);
                        RefreshControl();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 病案借阅 --- 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_emrApply_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                /*
                ApplyExamine frm = new ApplyExamine(_app);
                frm.ShowDialog();
                */
                MedicalRecordManage.UI.MedicalRecordApply frm = new MedicalRecordManage.UI.MedicalRecordApply(_app);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 传染病上报 --- 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_reportCard_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                _app.LoadPlugIn("DrectSoft.Core.ZymosisReport.dll", "DrectSoft.Core.ZymosisReport.MainForm");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 回车切换焦点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-11</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                //_app.CustomMessageBox.MessageShow(ex.Message);
                MyMessageBox.Show(1, ex);
            }
        }

        private void textEditPid_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                refreshGridView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设置工具栏图片按钮和右键按钮的显示或隐藏
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-08</date>
        /// </summary>
        private void SetButtonState()
        {
            try
            {
                ///基本信息维护功能（是否手动维护：1-是；0-否）
                string configKeyBase = GetConfigValueByKey("ManualMaintainBasicInfo");
                ///从his查病人信息功能（1-是；0-否）
                string configKeyHis = GetConfigValueByKey("GetInpatientForHis");
                ///简版工作站
                string configKeySimple = GetConfigValueByKey("SimpleDoctorCentor");
                string configClinical = GetConfigValueByKey("IsShowClinicalButton");

                #region 基本信息维护 &&从His查询病人信息
                if (configKeyBase == "1")
                {///手动维护
                    //病人信息
                    this.barButtonItem1.Visibility = BarItemVisibility.Never;
                    //入院登记
                    this.barButtonItem_inHos.Visibility = BarItemVisibility.Always;
                    //编辑病人信息
                    this.btnChangePatient.Visibility = BarItemVisibility.Always;
                    //病人出院
                    this.barButtonItem8.Visibility = BarItemVisibility.Never;
                }
                else
                {///非手动维护
                    if (configKeyHis == "1" && configKeySimple != "1")
                    {///从His查询病人信息 && 不是简版工作站
                        //病人信息
                        this.barButtonItem1.Visibility = BarItemVisibility.Always;
                    }
                    else
                    {
                        //病人信息
                        this.barButtonItem1.Visibility = BarItemVisibility.Never;
                    }
                    //入院登记
                    this.barButtonItem_inHos.Visibility = BarItemVisibility.Never;
                    //编辑病人信息
                    this.btnChangePatient.Visibility = BarItemVisibility.Never;
                    //病人出院
                    this.barButtonItem8.Visibility = BarItemVisibility.Never;
                }
                #endregion

                #region 简版工作站
                if (configKeySimple == "1")
                {///简版工作站
                    //会诊申请
                    this.barButtonItemConsultationApply.Visibility = BarItemVisibility.Never;
                    //院内会诊
                    this.barButtonItem_consultation.Visibility = BarItemVisibility.Never;
                    //病案借阅
                    this.barButtonItem_emrApply.Visibility = BarItemVisibility.Never;
                    //传染病上报
                    this.barButtonItem_reportCard.Visibility = BarItemVisibility.Never;
                    //临床路径
                    this.barButtonItem3.Visibility = BarItemVisibility.Never;
                }
                else
                {
                    //会诊申请
                    this.barButtonItemConsultationApply.Visibility = BarItemVisibility.Always;
                    //院内会诊
                    this.barButtonItem_consultation.Visibility = BarItemVisibility.Always;
                    //病案借阅
                    this.barButtonItem_emrApply.Visibility = BarItemVisibility.Always;
                    //传染病上报
                    this.barButtonItem_reportCard.Visibility = BarItemVisibility.Always;
                    //临床路径
                    this.barButtonItem3.Visibility = BarItemVisibility.Always;
                }
                #endregion

                //文书录入
                this.barButtonItem2.Visibility = BarItemVisibility.Always;
                //设为我的病人
                this.btn_ownInp.Visibility = NeedUndoMyInpatient.Equals(true) ? BarItemVisibility.Always : BarItemVisibility.Never;
                //设定婴儿
                this.btnSetMyBaby.Visibility = BarItemVisibility.Always;
                if (!configClinical.Trim().Equals("1"))
                {
                    this.barButtonItem3.Visibility = BarItemVisibility.Never;
                }

                bool hasHZXT = DrectSoft.Service.DS_BaseService.FlieHasKey("HZXT");  //会诊系统模块是否存在
                if (!hasHZXT)
                {
                    //2个会诊按钮不显示 同时右边会诊列表也不显示




                    barButtonItemConsultationApply.Visibility = barButtonItem_consultation.Visibility = BarItemVisibility.Never;

                }
                bool hasBLLL = DrectSoft.Service.DS_BaseService.FlieHasKey("BLLL");  //病历浏览模块是否存在
                if (!hasBLLL)
                {
                    barButtonItem_emrApply.Visibility = BarItemVisibility.Never;
                }

                bool hasCRBSB = DrectSoft.Service.DS_BaseService.FlieHasKey("CRBSB");  //传染病模块是否存在
                if (!hasCRBSB)
                {

                    barButtonItem_reportCard.Visibility = BarItemVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 清空事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-08</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            try
            {
                this.textEditBedNo.Text = string.Empty;
                this.textEditPatientName.Text = string.Empty;
                this.textEditPatientSN.Text = string.Empty;
                this.textEditInwDia.Text = string.Empty;
                this.textEditBedNo.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void textEditInwDia_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                refreshGridView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                string noofinpat = dataRow["noofinpat"].ToString();
                nursingRecordForm = NursingRecordForm.CreateInstance();
                this.ShowVigalsHandle += new del_ShowVigals(nursingRecordForm.InitNursingRecord);
                OnShowVigals(noofinpat);
                nursingRecordForm.Show();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridMain_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (nursingRecordForm != null)
                {
                    DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                    string noofinpat = dataRow["noofinpat"].ToString();
                    nursingRecordForm = NursingRecordForm.CreateInstance();
                    this.ShowVigalsHandle += new del_ShowVigals(nursingRecordForm.InitNursingRecord);
                    OnShowVigals(noofinpat);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
