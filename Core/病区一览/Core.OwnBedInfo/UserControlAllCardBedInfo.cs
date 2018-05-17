using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraGrid.Views.Card.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;


using System.Xml.Serialization;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Common.Eop;
using YidanSoft.Resources;


namespace YidanSoft.Core.OwnBedInfo
{
    public partial class UserControlAllCardBedInfo : UserControl
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
                //colBaby.Visible = _flagObstetricWard;记得
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

        private WaitDialogForm m_WaitDialog;

        #endregion

        #region Layout setting members
        /// <summary>
        /// 卡片模式背景色
        /// </summary>
        public Color CardBackGround
        {
            get
            {
                //if ((_cardBackGround == Color.Empty) || (_cardBackGround == null))
                //    _cardBackGround = Color.LightYellow;
                return _cardBackGround;
            }
            set
            {
                _cardBackGround = value;

            }
        }

        private int m_BedsCount;
        private Color _cardBackGround;
        /// <summary>
        /// 卡片显示时一页显示的最大记录数
        /// </summary>
        private int RowWholeNumber;


        /// <summary>
        /// 卡片显示时一页显示的最大行数
        /// </summary>
        private int m_RowCardView;
        /// <summary>
        /// 卡片显示时一页显示的最大列数
        /// </summary>
        private int m_ColCardView;
        private ImageList m_ImageListzifei;
        /// <summary>
        /// 连接数据库的SqlHelper
        /// </summary>
        private IDataAccess m_DataAccessEmrly;
        private DataManager m_DataManager;

        /// <summary>
        /// 当前按纽状态
        /// </summary>
        public ViewState CurrentState
        {
            get { return _currentState; }
            set
            {
                if (value == ViewState.InHospitalCardFirst)
                    barButtonItemFirstPage.Down = true;
                else if (value == ViewState.InHospitalCardSecond)
                    barButtonSecondPage.Down = true;
                _currentState = value;

            }
        }
        private ViewState _currentState;

        /// <summary>
        /// 分管状态
        /// </summary>
        public ViewState DistriState
        {
            get { return _distriState; }
            set { _distriState = value; }
        }
        private ViewState _distriState;

        /// <summary>
        /// 当前查询状态
        /// 全部OR分管
        /// </summary>
        public QueryType CQueryType
        {
            get { return _queryType; }
            set { _queryType = value; }
        }
        private QueryType _queryType;
        #endregion

        #region properties
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        private IYidanEmrHost _app;
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        public IYidanEmrHost App
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


        private ColumnView CurrentView
        {
            get
            {
                return (gridMain.MainView as DevExpress.XtraGrid.Views.Base.ColumnView);
            }
        }

        private GridControl CurrentGrid
        {
            get
            {
                return gridMain;
            }
        }

        private DataRow CurrentViewRow
        {
            get
            {
                if (CurrentView != null)
                    return CurrentView.GetDataRow(CurrentView.FocusedRowHandle);
                else
                    return null;
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
        /// 病区一览全部病患CARD模式
        /// </summary>
        /// <param name="application"></param>
        public UserControlAllCardBedInfo(IYidanEmrHost application)
        {
            InitializeComponent();

            App = application;
        }
        #endregion

        private void UserControlAllBedInfo_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            InitApp(App);
            this.ResumeLayout();
            this.SizeChanged += new System.EventHandler(this.UserControlAllCardBedInfo_SizeChanged);
        }

        #region events
        #region cardview Draw methods

        /// <summary>
        /// 绘制床头卡头部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cardViewBedMapping_CustomCardCaptionImage(object sender, CardCaptionImageEventArgs e)
        {
            DataRow dr = cardViewBedMapping.GetDataRow(e.RowHandle);
            //画框架病人的卡片标题背景
            int PicNum = 0;
            if (dr == null)
                return;
            if (!string.IsNullOrEmpty(dr[PatientWardField.Fieldbrxb].ToString()))
            {
                PicNum = Convert.ToInt32(dr[PatientWardField.Fieldbrxb]) - 1;
            }
            else if (!string.IsNullOrEmpty(dr[PatientWardField.Fieldcwlx].ToString()))
            {
                PicNum = Convert.ToInt32(dr[PatientWardField.Fieldcwlx]) - 1100 + 2;
            }
            if ((!string.IsNullOrEmpty(dr[PatientWardField.Fieldcwdm].ToString())) && (PicNum <= 3))
                e.Image = imageListcwdm.Images[PicNum];
        }

        private void cardViewBedMapping_CustomDrawCardCaption(object sender, CardCaptionCustomDrawEventArgs e)
        {
            try
            {
                DataRow dr = cardViewBedMapping.GetDataRow(e.RowHandle);
                if (dr == null)
                    return;
                decimal syxh = CheckCurrentRow(dr);
                if (syxh != -1)
                {
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.Font = new Font("宋体", 18F, FontStyle.Bold);
                    Brush brushFocused = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, Color.LavenderBlush, Color.Navy, 270);
                    CardView cv = sender as CardView;

                    Rectangle r = e.Bounds;
                    ControlPaint.DrawBorder3D(e.Graphics, r, Border3DStyle.RaisedInner);
                    r.Inflate(-1, -1);
                    e.Graphics.FillRectangle(brushFocused, r);
                    r.Inflate(-2, 0);
                    e.Appearance.DrawString(e.Cache, cv.GetCardCaption(e.RowHandle), r);
                    e.Graphics.FillRectangle(e.Appearance.GetBorderBrush(e.Cache), e.Bounds.X - 1, e.Bounds.Y - 1, 1, e.Bounds.Height);
                    e.Graphics.FillRectangle(e.Appearance.GetBorderBrush(e.Cache), e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width + 1, 1);
                    e.Graphics.FillRectangle(e.Appearance.GetBorderBrush(e.Cache), e.Bounds.X + e.Bounds.Width, e.Bounds.Y - 1, 1, e.Bounds.Height);

                    Point StartPoint = new Point(e.Bounds.Location.X + 3, e.Bounds.Location.Y + 7);
                    int PicNum = 0;
                    if (!string.IsNullOrEmpty(dr[PatientWardField.Fieldbrxb].ToString()))
                        PicNum = Convert.ToInt32(dr[PatientWardField.Fieldbrxb]) - 1;
                    else if (!string.IsNullOrEmpty(dr[PatientWardField.Fieldcwlx].ToString()))
                        PicNum = Convert.ToInt32(dr[PatientWardField.Fieldcwlx]) - 1100 + 2;
                    if ((!string.IsNullOrEmpty(dr[PatientWardField.Fieldcwdm].ToString())) && (PicNum <= 3))
                        e.Graphics.DrawImageUnscaled(imageListcwdm.Images[PicNum], StartPoint);
                    e.Handled = true;
                }

                //画框架病人的卡片标题背景
                CardInfo ccvi = e.CardInfo as CardInfo;

                e.Appearance.ForeColor = Color.Black;
                if (Convert.ToString(dr[PatientWardField.Fieldjcbz]) == "0")
                    ccvi.CaptionInfo.CardCaption = dr[PatientWardField.Fieldcwdm].ToString();
                else if (Convert.ToString(dr[PatientWardField.Fieldjcbz]) == "1")
                {
                    string str;
                    if (((dr[PatientWardField.Fieldybqdm].ToString().Trim() != m_WardId) || (dr[PatientWardField.Fieldyksdm].ToString().Trim() != m_DeptId)) && (!string.IsNullOrEmpty(dr[PatientWardField.Fieldybqdm].ToString()) && !string.IsNullOrEmpty(dr[PatientWardField.Fieldyksdm].ToString())))
                        str = dr[PatientWardField.Fieldcwdm].ToString() + "借入";
                    else
                        str = dr[PatientWardField.Fieldcwdm].ToString() + "借出";
                    ccvi.CaptionInfo.CardCaption = str;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 首页序号
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private decimal CheckCurrentRow(DataRow row)
        {
            if (row == null)
                return -1;
            try
            {
                string syxh = Convert.ToString(row[PatientWardField.Fieldsyxh]);
                if (string.IsNullOrEmpty(syxh) || (syxh == "-1") || (App.CurrentPatientInfo == null))
                    return -1;
                else
                {
                    decimal FPNo = Convert.ToDecimal(syxh);
                    return FPNo == App.CurrentPatientInfo.NoOfFirstPage ? FPNo : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        private void cardViewBedMapping_CustomDrawCardFieldValue(object sender, RowCellCustomDrawEventArgs e)
        {
            try
            {
                DataRow currentRow = cardViewBedMapping.GetDataRow(e.RowHandle);
                if (currentRow == null)
                    return;
                Rectangle rec = e.Bounds;
                rec.Size = new Size(16, 16);
                Font extraInfoFont = new Font(e.Appearance.Font.FontFamily, 8, e.Appearance.Font.Style);
                //向右移动10象素点再画
                //r.X += 10;
                if (e.Column.FieldName.ToUpper() == PatientWardField.Fieldhzxm.ToUpper())
                {
                    //费用欠费标志，该处不能再改，否则问题无法解决
                    if ((!string.IsNullOrEmpty(currentRow[PatientWardField.Fieldfee].ToString()))
                       && (currentRow[PatientWardField.Fieldfee] != DBNull.Value)
                       && (Convert.ToSingle(currentRow[PatientWardField.Fieldfee]) < ConstResource.OweLine))
                    {
                        e.Column.AppearanceCell.ForeColor = BackPayColor;
                        e.Appearance.ForeColor = BackPayColor;
                    }
                    else
                    {
                        e.Column.AppearanceCell.ForeColor = NotBackPayColor;
                        e.Appearance.ForeColor = NotBackPayColor;
                    }
                    //包床标志
                    if (Convert.ToString(currentRow[PatientWardField.Fieldzcbz]) == "1302")
                    {
                        string str = e.DisplayText + " 包床";
                        e.Appearance.ForeColor = Color.Gray;
                        e.Appearance.TextOptions.HAlignment = HorzAlignment.Default;
                        e.Appearance.DrawString(e.Cache, str, e.Bounds);
                    }
                    else
                        e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                    //if (string.IsNullOrEmpty(currentRow[PatientWardField.Fieldhzxm].ToString()))
                    //   e.Handled = true;
                }
                //额外信息字段，现在将这行改为图标行，将所有信息用图标表示
                else if (e.Column.FieldName.ToUpper() == PatientWardField.Fieldextra.ToUpper())
                {
                    Rectangle rect = e.Bounds;
                    //rect.Size = new Size(16, 16);
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Default;
                    e.Appearance.ForeColor = Color.Green;
                    e.Appearance.Font = extraInfoFont;
                    e.Appearance.DrawString(e.Cache, e.DisplayText, rect);
                }

                else if (e.Column.FieldName.ToUpper() == PatientWardField.Fieldcpstatus.ToUpper())
                {
                    Rectangle rect = e.Bounds;
                    //rect.Size = new Size(16, 16);
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                    e.Appearance.ForeColor = Color.Brown;
                    e.Appearance.Font = extraInfoFont;
                    e.Appearance.DrawString(e.Cache, e.DisplayText, rect);
                }
                else if (e.Column.FieldName.ToUpper() == PatientWardField.Fieldhljb.ToUpper())
                {
                    Rectangle rectHljb = e.Bounds;
                    //rect.Size = new Size(16, 16);
                    e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                    e.Appearance.ForeColor = Color.Green;
                    e.Appearance.Font = extraInfoFont;
                    if (e.DisplayText.EndsWith("费"))
                        e.Appearance.DrawString(e.Cache, e.DisplayText.Remove(e.DisplayText.Length - 1, 1), rectHljb);
                    else
                        e.Appearance.DrawString(e.Cache, e.DisplayText, rectHljb);
                }
                //危重级别和自费标志处理
                else if (e.Column.FieldName.ToUpper() == (PatientWardField.Fieldwzjb.ToUpper()))//"_" + 
                {
                    if ((!string.IsNullOrEmpty(currentRow[PatientWardField.Fieldwzjb].ToString()))
                       && (!string.IsNullOrEmpty(currentRow[PatientWardField.Fieldhzxm].ToString()))
                       && (currentRow[PatientWardField.Fieldsyxh].ToString() != "-1"))
                    {
                        rec.Y += 5;
                        e.Graphics.DrawImageUnscaled(imageListCustomwzjb.Images[Convert.ToInt16(currentRow[PatientWardField.Fieldwzjb])], rec);
                        //偏移起始画的位置
                        rec.X += 22;
                        if (currentRow[PatientWardField.Fieldpzlx].ToString().Trim() == "0")
                        {
                            e.Graphics.DrawImageUnscaled(m_ImageListzifei.Images[0], rec);
                        }
                    }
                }
                e.Handled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void barButtonItemFirstPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.SuspendLayout();
            ResetGridControl(DistriState);
            CurrentState = ViewState.InHospitalCardFirst;
            SetGridDataSource(m_DataManager.GetGridControlData(DistriState, CurrentState), CurrentState, DistriState);
            SetPageView(cardViewBedMapping, false);
            this.ResumeLayout();
        }

        private void barButtonSecondPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.SuspendLayout();
            ResetGridControl(DistriState);
            CurrentState = ViewState.InHospitalCardSecond;
            SetGridDataSource(m_DataManager.GetGridControlData(DistriState, CurrentState), CurrentState, DistriState);
            SetPageView(cardViewBedMapping, false);
            this.ResumeLayout();
        }

        private void barButtonThirdPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.SuspendLayout();
            ResetGridControl(DistriState);
            CurrentState = ViewState.InHospitalCardThird;
            SetGridDataSource(m_DataManager.GetGridControlData(DistriState, CurrentState), CurrentState, DistriState);
            SetPageView(cardViewBedMapping, false);
            this.ResumeLayout();
        }

        private void cardViewBedMapping_DoubleClick(object sender, EventArgs e)
        {
            LoadRecordInput();
        }

        /// <summary>
        /// 进入文书录入
        /// </summary>
        public void LoadRecordInput()
        {
            decimal FPNo;
            if ((CurrentView.FocusedRowHandle >= 0) && (CurrentViewRow != null))
            {
                if (FlagObstetricWard && (CurrentView == cardViewBedMapping))
                {
                    string babyName = CurrentViewRow[PatientWardField.Fieldtempbaby].ToString();
                    string syxhMother = CurrentViewRow[PatientWardField.Fieldsyxh].ToString();
                    if (string.IsNullOrEmpty(babyName))
                    {
                        if ((!string.IsNullOrEmpty(syxhMother)) && (syxhMother != "-1"))
                        {
                            FPNo = Convert.ToDecimal(syxhMother);
                            if (FPNo >= 0)
                            {
                                App.ChoosePatient(Convert.ToDecimal(syxhMother));
                                App.LoadPlugIn("YidanSoft.Core.RecordsInput.dll", "YidanSoft.Core.RecordsInput.FormMain");

                            }
                        }

                    }
                    else
                    {
                        DataRow row = m_DataManager.SelectBabyRow(babyName, Convert.ToDecimal(syxhMother));
                        if (row != null)
                        {
                            App.ChoosePatient(Convert.ToDecimal(row[PatientWardField.Fieldsyxh]));
                            // 调用绑定到双击的附件动作
                            App.LoadPlugIn("YidanSoft.Core.RecordsInput.dll", "YidanSoft.Core.RecordsInput.FormMain");
                        }
                    }

                }
                else if ((CurrentView == cardViewBedMapping))
                {
                    string syxh = Convert.ToString(CurrentViewRow[PatientWardField.Fieldsyxh]);
                    if ((!string.IsNullOrEmpty(syxh)) && (syxh != "-1"))
                    {
                        FPNo = Convert.ToDecimal(syxh);
                        if (FPNo >= 0)
                        {
                            App.ChoosePatient(Convert.ToDecimal(syxh));
                            // 调用绑定到双击的附件动作
                            App.LoadPlugIn("YidanSoft.Core.RecordsInput.dll", "YidanSoft.Core.RecordsInput.FormMain");
                        }
                    }
                }
            }
        }


        private void cardViewBedMapping_MouseDown(object sender, MouseEventArgs e)
        {
            //窗体右击事件
            if (e.Button == MouseButtons.Right)
            {
                //this.popupMenu1.ShowCaption = true;
                if (CurrentViewRow == null)
                    return;
                this.popupMenu1.ShowPopup(this.barManagerMenu, new Point(e.X+180, e.Y+120));
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            decimal FPNo;
            if (CurrentViewRow == null)
                return;
            string syxh = Convert.ToString(CurrentViewRow[PatientWardField.Fieldsyxh]);
            if ((!string.IsNullOrEmpty(syxh)) && (syxh != "-1"))
            {
                FPNo = Convert.ToDecimal(syxh);
                if ((FPNo > 0) && (this._app.CustomMessageBox.MessageShow("是否将该病人设为我的分管病人?", CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK))
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
                            this._app.CustomMessageBox.MessageShow("该病人已经为我的分管病人!");
                            return;
                        }
                        //强制将当前病人设置为我的分管病人
                        else if (this._app.CustomMessageBox.MessageShow(string.Format("该病人已有分管医生【{0}】，是否强制将该病人设为我的分管病人?", dt.Rows[0]["Users_Name"]), CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                        {
                            str = string.Format(@"UPDATE Doctor_AssignPatient SET ID = '{0}',
                                                        Create_time = GETDATE(),
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
                        str = "insert into Doctor_AssignPatient values ('" + this._app.User.Id + "'," + FPNo + ",1,getdate(),'" + this._app.User.Id + "')";
                        this.m_DataAccessEmrly.ExecuteNoneQuery(str);
                    }

                }
            }
        }

        private void barButtonItem_Cp_ItemClick(object sender, ItemClickEventArgs e)
        {

            decimal FPNo;
            if ((CurrentView.FocusedRowHandle >= 0) && (CurrentViewRow != null))
            {
                //SetWaitDialogCaption("正在加载数据");
                if (FlagObstetricWard && (CurrentView == cardViewBedMapping))
                {
                    string babyName = CurrentViewRow[PatientWardField.Fieldtempbaby].ToString();
                    string syxhMother = CurrentViewRow[PatientWardField.Fieldsyxh].ToString();
                    if (string.IsNullOrEmpty(babyName))
                    {
                        if ((!string.IsNullOrEmpty(syxhMother)) && (syxhMother != "-1"))
                        {
                            FPNo = Convert.ToDecimal(syxhMother);
                            if (FPNo >= 0)
                            {
                                App.ChoosePatient(Convert.ToDecimal(syxhMother));
                                App.LoadPlugIn("YidanSoft.Core.DoctorTasks.dll", "YidanSoft.Core.DoctorTasks.InpatientPathForm");

                            }
                        }

                    }
                    else
                    {
                        DataRow row = m_DataManager.SelectBabyRow(babyName, Convert.ToDecimal(syxhMother));
                        if (row != null)
                        {
                            App.ChoosePatient(Convert.ToDecimal(row[PatientWardField.Fieldsyxh]));
                            // 调用绑定到双击的附件动作
                            App.LoadPlugIn("YidanSoft.Core.DoctorTasks.dll", "YidanSoft.Core.DoctorTasks.InpatientPathForm");
                        }
                    }

                }
                else if ((CurrentView == cardViewBedMapping))
                {
                    string syxh = Convert.ToString(CurrentViewRow[PatientWardField.Fieldsyxh]);
                    if ((!string.IsNullOrEmpty(syxh)) && (syxh != "-1"))
                    {
                        FPNo = Convert.ToDecimal(syxh);
                        if (FPNo >= 0)
                        {
                            App.ChoosePatient(Convert.ToDecimal(syxh));
                            // 调用绑定到双击的附件动作
                            App.LoadPlugIn("YidanSoft.Core.DoctorTasks.dll", "YidanSoft.Core.DoctorTasks.InpatientPathForm");
                        }
                    }
                }
                //HideWaitDialog();
            }

        }

        /// <summary>
        /// 大小改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlAllCardBedInfo_SizeChanged(object sender, EventArgs e)
        {
            GetDataByUISetting();
            InitAllControl();
        }

        #endregion

        #region methods

        #region public methods of WaitDialog
        private void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }
        }

        private void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        #endregion
        protected virtual void InitApp(IYidanEmrHost application)
        {
            try
            {

                m_WaitDialog = new WaitDialogForm("正在初始化数据", "请稍候....");
                //初始化carview
                cardViewBedMapping.BeginInit();
                _app = application;
                m_DataAccessEmrly = _app.SqlHelper;
                //_cardBackGround = Color.LightYellow;
                InitializeImage();

                if (App.User.CurrentDeptId == null)
                {
                    m_DeptId = "3225";
                    m_WardId = "2922";
                }
                else
                {
                    m_DeptId = App.User.CurrentDeptId;
                    m_WardId = App.User.CurrentWardId;
                }
                m_DataManager = new DataManager(application, m_DataAccessEmrly, m_DeptId, m_WardId, FlagObstetricWard);
                AddEventHander();
                GetDataByUISetting();
                InitAllControl();
                this.Text = ConstResource.AppName;
                SetScreenFont();

                cardViewBedMapping.Appearance.EmptySpace.BackColor = CardBackGround;
                cardViewBedMapping.Appearance.FieldCaption.ForeColor = CardBackGround;
                repItemPopupPatInfo.Appearance.BackColor = CardBackGround;
                repCbBaby.Appearance.BackColor = CardBackGround;
                cardViewBedMapping.LayoutChanged();
                cardViewBedMapping.EndInit();
                HideWaitDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 注册事件
        /// 通过
        /// </summary>
        private void AddEventHander()
        {
            cardViewBedMapping.CustomDrawCardFieldValue += new RowCellCustomDrawEventHandler(cardViewBedMapping_CustomDrawCardFieldValue);
            cardViewBedMapping.CustomDrawCardCaption += new CardCaptionCustomDrawEventHandler(cardViewBedMapping_CustomDrawCardCaption);
            cardViewBedMapping.CustomCardCaptionImage += new CardCaptionImageEventHandler(cardViewBedMapping_CustomCardCaptionImage);

            barButtonItemFirstPage.ItemClick += new ItemClickEventHandler(barButtonItemFirstPage_ItemClick);
            barButtonSecondPage.ItemClick += new ItemClickEventHandler(barButtonSecondPage_ItemClick);
            barButtonThirdPage.ItemClick += new ItemClickEventHandler(barButtonThirdPage_ItemClick);
        }

        /// <summary>
        /// 设置cardview
        /// 基本通过,注意CQueryType
        /// </summary>
        private void GetDataByUISetting()
        {
            try
            {
                CQueryType = QueryType.ALL;
                m_BedsCount = m_DataManager.GetWardData(m_WardId, m_DeptId, true, CQueryType);
                SetScreenFont();
                SetScreenLayout();
                m_DataManager.SetParameters(RowWholeNumber, m_RowCardView, m_ColCardView);
                m_DataManager.InitPatientWardData(FlagObstetricWard);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化控件
        /// 基本通过,注意DistriState,CurrentState
        /// </summary>
        private void InitAllControl()
        {
            DistriState = ViewState.AllPats;
            CurrentState = ViewState.InHospitalCardFirst;
            SetGridDataSource(m_DataManager.GetGridControlData(DistriState, CurrentState), CurrentState, DistriState);
            SetPageView(cardViewBedMapping, false);
        }

        /// <summary>
        /// 初始化图片
        /// 通过
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
                //int i = imageListcwdm.Images.Count;
                //imageListcwdm.Images.RemoveAt(imageListcwdm.Images.IndexOfKey(ImageKeyString.ImageMale));
                //imageListcwdm.Images.RemoveAt(imageListcwdm.Images.IndexOfKey(ImageKeyString.ImageFemale));

                //imageListcwdm.Images.Add(ImageKeyString.ImageMale, Resources.ResourceManager.GetSmallIcon(ResourceNames.Male, IconType.Normal));
                //imageListcwdm.Images.Add(ImageKeyString.ImageFemale, Resources.ResourceManager.GetSmallIcon(ResourceNames.Female, IconType.Normal));

                m_ImageListzifei = ImageHelper.GetImageListPay();
                imageListBrxb = ImageHelper.GetImageListBrxb();
                repItemImageComboBoxBrxb.SmallImages = imageListBrxb;
                //ImageComboBoxItem ImageComboItemMale = new ImageComboBoxItem("男", "1", 1);
                //ImageComboBoxItem ImageComboItemFemale = new ImageComboBoxItem("女", "2", 0);
                //repItemImageComboBoxBrxb.Items.Add(ImageComboItemMale);
                //repItemImageComboBoxBrxb.Items.Add(ImageComboItemFemale);
                #region 暂时保留
                //_ImageListzifei = ImageHelper.GetImageListPay();
                //m_ImageExtra = ImageHelper.GetImageListExtra();
                #endregion
                repItemPopupPatInfo.Buttons[0].Image = ImageHelper.GetImageBykey(ImageKeyString.ImageInfomation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置显示的数据源
        /// </summary>
        /// <param name="o"></param>
        /// <param name="state"></param>
        /// <param name="distriState"></param>
        private void SetGridDataSource(object o, ViewState state, ViewState distriState)
        {
            if (o == null)
                return;
            try
            {
                switch (state)
                {
                    case ViewState.AllPats:
                        break;
                    case ViewState.DistriPats:
                        break;
                    case ViewState.NoAllAndDistri:
                        break;
                    case ViewState.InHospitalCardFirst:
                        SetGridData(o, cardViewBedMapping);
                        break;
                    case ViewState.InHospitalCardSecond:
                        SetGridData(o, cardViewBedMapping);
                        break;
                    case ViewState.InHospitalCardThird:
                        SetGridData(o, cardViewBedMapping);
                        break;
                    case ViewState.InHospitalList:
                        break;
                    case ViewState.ChangeWard:
                        //SetGridData(o, gridViewChangeWard);
                        break;
                    case ViewState.OutHospital:
                        //SetGridData(o, gridViewNotArchived);
                        break;
                    case ViewState.History:
                        break;
                    case ViewState.NoFunction:
                        break;
                    default:
                        break;
                }
                if (distriState == ViewState.AllPats)
                    SetGridData(o, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <param name="o"></param>
        /// <param name="view"></param>
        private void SetGridData(object o, BaseView view)
        {
            try
            {
                DataView dataView = null;
                if (view != null)
                    gridMain.MainView = view;
                if (o != null)
                    dataView = o as DataView;
                gridMain.DataSource = null;
                gridMain.DataSource = dataView;
                _app.PublicMethod.ConvertGridCardDataSourceUpper(cardViewBedMapping);
                gridMain.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void SetPageVisiblityAndLocate()
        {
            if (CurrentState == ViewState.InHospitalList)
            {
                SetPageView(null, true);
                AutoLocateCardAtBedmapping(true);
            }
            else if ((CurrentState == ViewState.InHospitalCardFirst) || (CurrentState == ViewState.InHospitalCardSecond) || (CurrentState == ViewState.InHospitalCardThird))
            {
                SetPageView(cardViewBedMapping, false);
                AutoLocateCardAtBedmapping(false);
            }
        }

        /// <summary>
        /// 多页button是否可见
        /// </summary>
        /// <param name="view"></param>
        /// <param name="hidePage"></param>
        private void SetPageView(CardView view, bool hidePage)
        {
            if ((hidePage) || (view == null))
            {
                barPage.Visible = false;
                return;
            }
            if (m_DataManager.RowCount <= RowWholeNumber)
                barPage.Visible = false;
            else
            {
                if (DistriState == ViewState.AllPats)
                {
                    barPage.Visible = true;
                    if (m_DataManager.RowCount > 2 * RowWholeNumber)
                        barButtonThirdPage.Visibility = BarItemVisibility.Always;
                    else
                        barButtonThirdPage.Visibility = BarItemVisibility.Never;
                }
                else
                    barPage.Visible = false;
            }
        }

        /// <summary>
        /// 自动定位到框架选中的病人卡片上
        /// </summary>
        private void AutoLocateCardAtBedmapping(bool isGridView)
        {
            string foundFPNo = string.Empty;
            if ((App.CurrentPatientInfo != null)
               && (!string.IsNullOrEmpty(App.CurrentPatientInfo.NoOfFirstPage.ToString()))
               && (Convert.ToDecimal(App.CurrentPatientInfo.NoOfFirstPage) != 0))
            {
                if (isGridView)
                {
                    if ((App.CurrentPatientInfo != null)
                       && (!string.IsNullOrEmpty(App.CurrentPatientInfo.NoOfFirstPage.ToString()))
                       && (Convert.ToDecimal(App.CurrentPatientInfo.NoOfFirstPage) != 0))
                    {
                    }
                    return;
                }
                if (FlagObstetricWard && App.CurrentPatientInfo.IsBaby)
                {
                    int motherHandle = GetSpecialCardFocused(App.CurrentPatientInfo.PersonalInformation.MotherFirstPageNo.ToString());
                    if (motherHandle != -1)
                    {
                        if (motherHandle != cardViewBedMapping.FocusedRowHandle)
                            cardViewBedMapping.FocusedRowHandle = motherHandle;
                        else
                        {
                            DataRow row = cardViewBedMapping.GetDataRow(cardViewBedMapping.FocusedRowHandle);
                            row[PatientWardField.Fieldtempbaby] = App.CurrentPatientInfo.PersonalInformation.PatientName;
                        }
                    }
                }
                else
                {
                    int patientHandle = GetSpecialCardFocused(App.CurrentPatientInfo.NoOfFirstPage.ToString());
                    if (patientHandle == -1)
                        cardViewBedMapping.FocusedRowHandle = 0;
                    else
                        cardViewBedMapping.FocusedRowHandle = patientHandle;
                }
            }
            else
            {
                cardViewBedMapping.FocusedRowHandle = 0;
            }
        }

        private int GetSpecialCardFocused(string foundFPNo)
        {
            int locate = 0;
            if ((m_DataManager.RowCount <= RowWholeNumber) || (DistriState == ViewState.DistriPats))
            {  //只有一个页面时,包括了全部和分管的状态
                locate = cardViewBedMapping.LocateByDisplayText(0, colsyxh, foundFPNo);
                if (locate != GridControl.InvalidRowHandle)
                    return locate;
                else
                    return -1;
            }
            int temp;
            //多于一个页面时,如果有第三页先找第三页，否则找第二页，最后找第一页
            if (m_DataManager.RowCount > 2 * RowWholeNumber)
            {
                DistriState = ViewState.AllPats;
                CurrentState = ViewState.InHospitalCardThird;
                temp = SetSpecialCardFocused(foundFPNo);
                if (temp != -1)
                    return temp;
            }
            else if (m_DataManager.RowCount > RowWholeNumber)
            {
                DistriState = ViewState.AllPats;
                CurrentState = ViewState.InHospitalCardSecond;
                temp = SetSpecialCardFocused(foundFPNo);
                if (temp != -1)
                    return temp;
            }
            //显示第一页并搜索这个页,如果还找不到就直接显示第一页
            CurrentState = ViewState.InHospitalCardFirst;
            return SetSpecialCardFocused(foundFPNo);
        }

        private int SetSpecialCardFocused(string foundFPNo)
        {
            int locate;
            SetGridDataSource(m_DataManager.GetGridControlData(DistriState, CurrentState), CurrentState, DistriState);
            locate = cardViewBedMapping.LocateByDisplayText(0, colsyxh, foundFPNo);
            if (locate != GridControl.InvalidRowHandle)
                return locate;
            else
                return -1;
        }

        /// <summary>
        /// 设置GridControl的CardView的最大行列值
        /// </summary>
        /// <param name="distriState">是否为分管状态</param>
        private void ResetGridControl(ViewState distriState)
        {
            cardViewBedMapping.MaximumCardColumns = m_ColCardView;
            cardViewBedMapping.MaximumCardRows = m_RowCardView;
        }



        /// <summary>
        /// 设置屏幕的行列数
        /// 对RowWholeNumber, m_RowCardView, m_ColCardView赋值
        /// </summary>
        private void SetScreenLayout()
        {
            try
            {
                lock (cardViewBedMapping)
                {
                    cardViewBedMapping.BeginUpdate();
                    LayoutMaster.ContainerWidth = this.Width;
                    Dictionary<string, int> result = LayoutMaster.GetScreenAutoSize(m_BedsCount, FlagObstetricWard);
                    m_RowCardView = result["rowCardView"];
                    m_ColCardView = result["colCardView"];
                    RowWholeNumber = result["rowWholeNumber"];
                    cardViewBedMapping.CardInterval = result["cardInterval"];
                    cardViewBedMapping.CardWidth = result["cardWidth"];
                    cardViewBedMapping.MaximumCardColumns = m_ColCardView;
                    cardViewBedMapping.MaximumCardRows = m_RowCardView;
                    int expandedRows = result["expandedRows"];
                    //处理增加行的高度，多加一个列比较适合
                    Collection<GridColumn> deletedColumns = new Collection<GridColumn>();
                    for (int index = 0; index < cardViewBedMapping.Columns.Count; index++)
                    {
                        if (cardViewBedMapping.Columns[index].Name.Contains("blankColumn"))
                            deletedColumns.Add(cardViewBedMapping.Columns[index]);
                    }
                    foreach (GridColumn column in deletedColumns)
                    {
                        cardViewBedMapping.Columns.Remove(column);
                    }
                    if (expandedRows > 0)
                    {
                        for (int i = 0; i < expandedRows; i++)
                        {
                            GridColumn blankColumn = new GridColumn();
                            blankColumn.Caption = "";
                            blankColumn.FieldName = "";
                            blankColumn.Name = "blankColumn" + i.ToString();
                            blankColumn.Visible = true;
                            blankColumn.VisibleIndex = 1;
                            blankColumn.Width = 61;
                            blankColumn.OptionsColumn.AllowEdit = false;
                            cardViewBedMapping.Columns.Add(blankColumn);
                        }
                    }
                    cardViewBedMapping.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置屏幕的大小字体
        /// </summary>
        private void SetScreenFont()
        {
            Dictionary<string, Font> font = LayoutMaster.GetGridFont(cardViewBedMapping);
            cardViewBedMapping.Appearance.FieldCaption.Font = font["FieldCaption"];
            cardViewBedMapping.Appearance.FieldValue.Font = font["FieldValue"];
            cardViewBedMapping.Appearance.CardCaption.Font = font["CardCaption"];
            cardViewBedMapping.Appearance.FocusedCardCaption.Font = font["FocusedCardCaption"];
            cardViewBedMapping.Appearance.SelectedCardCaption.Font = font["SelectedCardCaption"];
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void RefreshControl()
        {
            this.SuspendLayout();
            InitApp(App);
            this.ResumeLayout();
        }

        /// <summary>
        /// 获取当前病患
        /// </summary>
        /// <returns></returns>
        private decimal GetCurrentPat()
        {

            decimal syxh = 0;
            if (CurrentViewRow == null)
                return syxh = -1;
            if (CurrentView.FocusedRowHandle < 0)
                return syxh = -1;
            if (FlagObstetricWard && (CurrentView == cardViewBedMapping))
            {
                string babyName = CurrentViewRow[PatientWardField.Fieldtempbaby].ToString();
                string syxhMother = string.Empty;
                if (!CurrentViewRow.IsNull(PatientWardField.Fieldsyxh))
                    syxhMother = CurrentViewRow[PatientWardField.Fieldsyxh].ToString();
                if (string.IsNullOrEmpty(babyName))
                {
                    if ((!string.IsNullOrEmpty(syxhMother)) && (syxhMother != "-1"))
                    {
                        syxh = Convert.ToDecimal(syxhMother);
                    }

                }
                else
                {
                    DataRow row = m_DataManager.SelectBabyRow(babyName, Convert.ToDecimal(syxhMother));
                    if (row != null)
                    {
                        if (!CurrentViewRow.IsNull(PatientWardField.Fieldsyxh))
                            syxh = Convert.ToDecimal(row[PatientWardField.Fieldsyxh]);
                    }
                }

            }
            else if ((CurrentView == cardViewBedMapping))
            {
                if (!CurrentViewRow.IsNull(PatientWardField.Fieldsyxh))
                    syxh = Convert.ToDecimal(CurrentViewRow[PatientWardField.Fieldsyxh].ToString());
            }
            return syxh;

        }

        #endregion

    }
}
