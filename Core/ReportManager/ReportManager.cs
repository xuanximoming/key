using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.DSReportManager;
using DrectSoft.Core.ReportManager.UCControl;
using DrectSoft.DrawDriver;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;

namespace DrectSoft.Core.ReportManager
{
    /// <summary>
    /// /新增的所有报告卡的管理界面
    /// add by ywk 2013年6月26日 11:08:11
    /// </summary>
    public partial class ReportManager : DevBaseForm, IStartPlugIn
    {

        #region 属性和变量
        IEmrHost m_Host;
        SqlHelper m_SqlHelper;
        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_UCEmrInput;
        Auditor m_UserRole;
        ReportCardEntity _reportEntity;
        ReportCardEntity1 _reportEntity1;
        CardiovasularEntity _CardiovasularEntity;

        string _cardisshow;   //add by jxh 获取该页面显示的是哪种报告卡

        bool m_HaveAuditAuthority = false;//当前登录人是否有审核权限
        #endregion

        //edit by cyq 2012-10-30 
        TheriomaControl CurrentReprotCard
        {
            get
            {
                if (tabPageReport.Controls.Count > 0)
                {
                    TheriomaControl card = tabPageReport.Controls[0] as TheriomaControl;
                    //BirthDefectsControl birthcard = tabPageReport.Controls[0] as BirthDefectsControl;
                    if (card == null)
                    {
                        return null;
                    }
                    return card;
                }
                else
                {
                    return null;
                }
            }
        }

        BirthDefectsControl ReprotCard
        {
            get
            {
                if (TabPageCardInfo.Controls.Count > 0)
                {
                    BirthDefectsControl Card = tabPageReport1.Controls[0] as BirthDefectsControl;
                    if (Card == null)
                    {
                        return null;
                    }
                    return Card;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 脑卒中报告卡
        /// </summary>
        Cardiovascular CardiovascularCard
        {
            get
            {
                if (TabPageCardInfo.Controls.Count > 0)
                {
                    Cardiovascular Card = tabPageReport2.Controls[0] as Cardiovascular;
                    if (Card == null)
                    {
                        return null;
                    }
                    return Card;
                }
                else
                {
                    return null;
                }
            }
        }


        #region 方法

        /// <summary>
        /// 实现接口
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost application)
        {
            try
            {
                if (application == null)
                    throw new ArgumentNullException("application");
                m_Host = application;
                m_SqlHelper = new SqlHelper(m_Host.SqlHelper);
                PlugIn plg = new PlugIn(this.GetType().ToString(), this);
                return plg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 构造器

        public ReportManager()
        {
            InitializeComponent();
            TreeReport.GetStateImage += new GetStateImageEventHandler(TreeReport_GetStateImage);
            RegisterEvent();

        }

        #endregion

        #region 初始化 加载

        public void InitializeParameter()
        {
            try
            {

                LoadReportCart();
                InitFromShow();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion


        #region 事件
        /// <summary>
        /// 配置事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCardConfig_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DiseaseSetForm diseaseSetForm = new DiseaseSetForm(m_Host, m_SqlHelper);
                diseaseSetForm.StartPosition = FormStartPosition.CenterScreen;
                diseaseSetForm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportManager_Load(object sender, EventArgs e)
        {
            try
            {
                m_UserRole = new Auditor(m_SqlHelper);
                InitializeParameter();
                Search();
                SetButtonRole();
                SetButtonEnable2();
                CurrentReprotCard.ReadOnlyControl("2");
                ReprotCard.ReadOnlyControl("2");
                this.tabPageReport1.PageVisible = false;
                this.tabPageReport2.PageVisible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 新增 ,修改，保存，删除，提交，审核

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddNewPort_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbReportList.Text))
                {
                    MessageBox.Show("请选择需要添加的报告卡类别");
                    this.cmbReportList.Focus();

                }
                else
                {
                    CreateCard();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void CreateCard()
        {
            try
            {
                PatientListForNew patientList = new PatientListForNew(m_Host);
                //DataTable ReportList = m_SqlHelper.GetReportCategoryList();
                //_cardisshow = ReportList.Rows[0]["TABLENAME"].ToString();              
                patientList.cardisshow = _cardisshow;    //传一个状态到PatientListForNew页面
                patientList.StartPosition = FormStartPosition.CenterScreen;
                if (patientList.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataRow dr = patientList.GetSelectedRow();
                    if (dr != null)
                    {
                        string noofinpat = dr["noofinpat"].ToString();
                        string iemNo = dr["iem_mainpage_no"].ToString();
                        string icd = dr["icd"].ToString();

                        if (!string.IsNullOrEmpty(noofinpat))
                        {
                            if (_cardisshow == "2")
                            {
                                CurrentReprotCard.DiagICD10 = icd;
                                CurrentReprotCard.LoadPage(noofinpat, "2", "1");
                                _reportEntity = CurrentReprotCard.m_ReportCardEntity;
                            }
                            else if (_cardisshow == "3")
                            {
                                ReprotCard.DiagICD10 = icd;
                                ReprotCard.LoadPage(noofinpat, "2", "1");
                                _reportEntity1 = ReprotCard.m_ReportCardEntity;
                            }
                            else if (_cardisshow == "4")
                            {
                                //yours
                                CardiovascularCard.DiagICD = icd;
                                CardiovascularCard.LoadPage(noofinpat, "2", "1");
                                _CardiovasularEntity = CardiovascularCard.m_CardiovasularEntity;
                            }

                            SetButtonEnable2();

                            btnSaveReportCard.Enabled = true;
                            btnSumbit.Enabled = true;
                            btnEditRePortCard.Enabled = false;
                            btnDelReportCard.Enabled = false;
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
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelReportCard_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (tabPageReport.PageVisible)
                {
                    //删除
                    if (CurrentReprotCard.Cancel())
                    {
                        SetButtonEnable2();
                        UpdateRecordState();
                        CurrentReprotCard.ReadOnlyControl("2");
                        CurrentReprotCard.ClearPage();
                        Search();
                    }
                }
                if (tabPageReport1.PageVisible)
                {
                    if (ReprotCard.Cancel())
                    {
                        SetButtonEnable2();
                        UpdateRecordState();
                        ReprotCard.ReadOnlyControl("2");
                        ReprotCard.ClearPage();
                        Search();
                    }
                }

                if (tabPageReport2.PageVisible)
                {
                    if (CardiovascularCard.Cancel())
                    {
                        SetButtonEnable2();
                        UpdateRecordState();
                        ReprotCard.ReadOnlyControl("2");
                        CardiovascularCard.ClearPageControl();
                        Search();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSumbit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //肿瘤病报告卡
                if (this.tabPageReport.PageVisible == true)
                {
                    if (CurrentReprotCard.Save())
                    {
                        if (CurrentReprotCard.Submit())
                        {
                            CurrentReprotCard.GetDataDiagNosis(CurrentReprotCard.m_Noofinpat);
                            //CurrentReprotCard.m_upCount++;
                            //if (CurrentReprotCard.m_upCount < CurrentReprotCard.m_dataTableDiagicd.Rows.Count)
                            bool isNeedReadOnlyControl = true;
                            if (CurrentReprotCard.m_dataTableDiagicd.Rows.Count > 0)
                            {
                                if (MyMessageBox.Show("该病人出院诊断还存在符合肿瘤病上报条件，是否立即填报？", "肿瘤病上报", MyMessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    SetButtonEnable2();
                                    UpdateRecordState();
                                    if (isNeedReadOnlyControl)
                                    {
                                        CurrentReprotCard.ReadOnlyControl("2");
                                    }
                                    else
                                    {
                                        CurrentReprotCard.ReadOnlyControl("1");

                                        btnSaveReportCard.Enabled = true;
                                        btnSumbit.Enabled = true;
                                        btnEditRePortCard.Enabled = false;
                                        btnDelReportCard.Enabled = false;
                                    }
                                }
                                else
                                {
                                    CreateCard();
                                    SetButtonEnable2();
                                    btnSaveReportCard.Enabled = true;
                                    btnSumbit.Enabled = true;
                                    btnEditRePortCard.Enabled = false;
                                    btnDelReportCard.Enabled = false;
                                }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                    {
                        MessageBox.Show("请选择一条肿瘤上报记录或补录肿瘤报告信息");
                        return;
                    }
                }

                if (this.tabPageReport1.PageVisible == true)//出生缺陷报告卡  add  jxh
                {
                    if (ReprotCard.Save())
                    {
                        if (ReprotCard.Submit())
                        {

                            ReprotCard.GetDataDiagNosis(ReprotCard.m_Noofinpat);

                            bool isNeedReadOnlyControl = true;
                            if (ReprotCard.m_dataTableDiagicd.Rows.Count > 0)
                            {
                                if (MyMessageBox.Show("该病人出院诊断还存在符合出生缺陷报告卡上报条件，是否立即填报？", "出生缺陷上报", MyMessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    Search();
                                    SetButtonEnable2();
                                    UpdateRecordState();
                                }
                                else
                                {
                                    CreateCard();
                                    SetButtonEnable2();
                                    btnSaveReportCard.Enabled = true;
                                    btnSumbit.Enabled = true;
                                    btnEditRePortCard.Enabled = false;
                                    btnDelReportCard.Enabled = false;
                                }
                            }
                        }
                    }
                }

                if (this.tabPageReport2.PageVisible == true)//脑卒中报告卡
                {
                    if (CardiovascularCard.Save())
                    {
                        if (CardiovascularCard.Submit())
                        {
                            CardiovascularCard.GetDataDiagNosis(CardiovascularCard.m_Noofinpat);   //edit   jxh   原来是CurrentReprotCard？

                            bool isNeedReadOnlyControl = true;
                            if (CardiovascularCard.m_DTCularDiagicd.Rows.Count > 0)
                            {
                                if (MyMessageBox.Show("该病人出院诊断还存在符合心脑血管病报告卡上报条件，是否立即填报？", "心脑血管病上报", MyMessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    Search();
                                    SetButtonEnable2();
                                    UpdateRecordState();
                                }
                                else
                                {
                                    //CardiovascularCard.DiagICD = CardiovascularCard.m_DTCularDiagicd.Rows[0]["icd"].ToString();
                                    //CardiovascularCard.m_CardiovasularEntity = null;
                                    //CardiovascularCard.LoadPage(CardiovascularCard.m_Noofinpat, "2", "1");
                                    //isNeedReadOnlyControl = false;
                                    CreateCard();
                                    SetButtonEnable2();
                                    btnSaveReportCard.Enabled = true;
                                    btnSumbit.Enabled = true;
                                    btnEditRePortCard.Enabled = false;
                                    btnDelReportCard.Enabled = false;
                                }
                            }

                            //if (isNeedReadOnlyControl)
                            //{
                            //    CurrentReprotCard.ReadOnlyControl("2");
                            //}
                            //else
                            //{
                            //    CurrentReprotCard.ReadOnlyControl("1");

                            //    btnSaveReportCard.Enabled = true;
                            //    btnSumbit.Enabled = true;
                            //    btnEditRePortCard.Enabled = false;
                            //    btnDelReportCard.Enabled = false;
                            //}

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveReportCard_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                bool isNeedSearch = false;
                //出生缺陷卡
                if (this.tabPageReport1.PageVisible == true)  //add  jxh  缺陷卡保存
                {
                    if (ReprotCard.m_ReportCardEntity.State == "")
                    {
                        isNeedSearch = true;
                    }
                    if (ReprotCard.Save())
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功");

                        bool isNeedReadOnlyControl = true;
                        if (isNeedSearch)
                        {
                            this.Search();
                        }
                        else
                        {
                            UpdateRecordState();   //待定，此方法还没写，暂未用到
                        }
                        SetButtonEnable2();
                        if (isNeedReadOnlyControl)
                        {
                            ReprotCard.ReadOnlyControl("2");
                        }
                        else
                        {
                            ReprotCard.ReadOnlyControl("1");

                            btnSaveReportCard.Enabled = true;
                            btnSumbit.Enabled = true;
                            btnEditRePortCard.Enabled = false;
                            btnDelReportCard.Enabled = false;
                        }
                        ReprotCard.GetDataDiagNosis(ReprotCard.m_Noofinpat);
                        if (ReprotCard.m_dataTableDiagicd.Rows.Count > 0)
                        {
                            if (MyMessageBox.Show("该病人出院诊断还存在符合出生缺陷报告卡上报条件，是否立即填报？", "出生缺陷上报", MyMessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                Search();
                                SetButtonEnable2();
                                UpdateRecordState();
                            }
                            else
                            {
                                CreateCard();
                                SetButtonEnable2();
                                btnSaveReportCard.Enabled = true;
                                btnSumbit.Enabled = true;
                                ReprotCard.ReadOnlyControl("1");
                                btnEditRePortCard.Enabled = false;
                                btnDelReportCard.Enabled = false;
                            }
                        }
                    }
                }

                //脑卒中报告卡
                if (this.tabPageReport2.PageVisible == true)
                {
                    if (CardiovascularCard.m_CardiovasularEntity.STATE == "")
                    {
                        isNeedSearch = true;
                    }
                    if (CardiovascularCard.Save())
                    {
                        //Common.Ctrs.DLG.MessageBox.Show("保存成功");

                        bool isNeedReadOnlyControl = true;
                        if (isNeedSearch)
                        {
                            this.Search();
                        }
                        else
                        {
                            UpdateRecordState();   //待定，此方法还没写，暂未用到
                        }

                        SetButtonEnable2();

                        if (isNeedReadOnlyControl)
                        {
                            CardiovascularCard.ReadOnlyControl("2");
                        }
                        else
                        {
                            CardiovascularCard.ReadOnlyControl("1");

                            btnSaveReportCard.Enabled = true;
                            btnSumbit.Enabled = true;
                            btnEditRePortCard.Enabled = false;
                            btnDelReportCard.Enabled = false;
                        }
                        CardiovascularCard.GetDataDiagNosis(CardiovascularCard.m_Noofinpat);
                        if (CardiovascularCard.m_DTCularDiagicd.Rows.Count > 0)
                        {
                            if (MyMessageBox.Show("该病人出院诊断还存在符合心脑血管病报告卡上报条件，是否立即填报？", "心血管病报告卡上报", MyMessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                SetButtonEnable2();
                                UpdateRecordState();
                                if (isNeedReadOnlyControl)
                                {
                                    CurrentReprotCard.ReadOnlyControl("2");
                                }
                                else
                                {
                                    CurrentReprotCard.ReadOnlyControl("1");

                                    btnSaveReportCard.Enabled = true;
                                    btnSumbit.Enabled = true;
                                    btnEditRePortCard.Enabled = false;
                                    btnDelReportCard.Enabled = false;
                                }
                            }
                            else
                            {
                                CreateCard();
                                SetButtonEnable2();
                                btnSaveReportCard.Enabled = true;
                                btnSumbit.Enabled = true;
                                CardiovascularCard.ReadOnlyControl("1");
                                btnEditRePortCard.Enabled = false;
                                btnDelReportCard.Enabled = false;
                            }
                        }
                    }
                }


                //肿瘤报告卡
                if (this.tabPageReport.PageVisible == true)
                {
                    if (CurrentReprotCard.m_ReportCardEntity.State == "")
                    {
                        isNeedSearch = true;
                    }
                    if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                    {
                        MessageBox.Show("请选择一条肿瘤上报记录或补录肿瘤报告信息");
                        return;
                    }
                    //保存
                    if (CurrentReprotCard.Save())
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功");
                        //刷新实体数据
                        //m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                        //this.Search();
                        //UpdateRecordState();
                        bool isNeedReadOnlyControl = true;
                        //在新增后的保存操作应该刷一下病人列表比较好
                        if (isNeedSearch)
                        {
                            this.Search();
                        }
                        else
                        {
                            UpdateRecordState();
                        }

                        SetButtonEnable2();

                        if (isNeedReadOnlyControl)
                        {
                            CurrentReprotCard.ReadOnlyControl("2");
                        }
                        else
                        {
                            CurrentReprotCard.ReadOnlyControl("1");

                            btnSaveReportCard.Enabled = true;
                            btnSumbit.Enabled = true;
                            btnEditRePortCard.Enabled = false;
                            btnDelReportCard.Enabled = false;
                        }
                        CurrentReprotCard.GetDataDiagNosis(CurrentReprotCard.m_Noofinpat);
                        //CurrentReprotCard.m_upCount++;
                        //if (CurrentReprotCard.m_upCount < CurrentReprotCard.m_dataTableDiagicd.Rows.Count)

                        if (CurrentReprotCard.m_dataTableDiagicd.Rows.Count > 0)
                        {
                            if (MyMessageBox.Show("该病人出院诊断还存在符合肿瘤病上报条件，是否立即填报？", "肿瘤病上报", MyMessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                //this.Close();
                            }
                            else
                            {
                                CreateCard();
                                SetButtonEnable2();
                                btnSaveReportCard.Enabled = true;
                                btnSumbit.Enabled = true;
                                CurrentReprotCard.ReadOnlyControl("1");
                                btnEditRePortCard.Enabled = false;
                                btnDelReportCard.Enabled = false;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 否决
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (tabPageReport.PageVisible)
                {
                    if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条肿瘤上报记录");
                        return;
                    }
                    if (CurrentReprotCard.UnPassApprove())
                    {
                        //m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                        SetButtonEnable2();
                        UpdateRecordState();
                        CurrentReprotCard.ReadOnlyControl("2");
                        Search();
                        //CurrentReprotCard.ClearPage();
                    }
                }
                if (tabPageReport1.PageVisible)  //add jxh 出现缺陷卡否决
                {
                    if (string.IsNullOrEmpty(ReprotCard.m_Noofinpat))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条出生缺陷上报记录");
                        return;
                    }
                    if (ReprotCard.UnPassApprove())
                    {
                        //m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                        SetButtonEnable2();
                        UpdateRecordState();
                        CurrentReprotCard.ReadOnlyControl("2");
                        Search();
                        //CurrentReprotCard.ClearPage();
                    }
                }

                if (tabPageReport2.PageVisible)  //add jxh 出现缺陷卡否决
                {
                    if (string.IsNullOrEmpty(CardiovascularCard.m_Noofinpat))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条心脑血管病上报记录");
                        return;
                    }
                    if (CardiovascularCard.UnPassApprove())
                    {
                        //m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                        SetButtonEnable2();
                        UpdateRecordState();
                        CurrentReprotCard.ReadOnlyControl("2");
                        Search();
                        //CurrentReprotCard.ClearPage();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion


        /// <summary>
        /// 肿瘤报告卡列表
        /// </summary>
        public void LoadReportCart()
        {
            try
            {

                DataTable dt = m_SqlHelper.GetReportCategoryList();
                List<ListItem> itemList = new List<ListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ListItem list = new ListItem(dt.Rows[i]["ID"].ToString(), dt.Rows[i]["Name"].ToString());
                    itemList.Add(list);

                }
                cmbReportList.DataSource = itemList;
                _cardisshow = (this.cmbReportList.SelectedItem as ListItem).Value;
                //cmbReportList.DisplayMember = "NAME";
                //cmbReportList.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化界面加载肿瘤报告卡界面
        /// </summary>
        public void InitFromShow()
        {
            try
            {

                TheriomaControl theriomafrm = new TheriomaControl(m_Host);
                theriomafrm.Dock = DockStyle.Fill;
                this.tabPageReport.Controls.Add(theriomafrm);

                BirthDefectsControl birthdefectsfrm = new BirthDefectsControl(m_Host);   //出生缺陷儿报告卡
                birthdefectsfrm.Dock = DockStyle.Fill;
                this.tabPageReport1.Controls.Add(birthdefectsfrm);

                Cardiovascular cardiovascular = new Cardiovascular(m_Host);
                cardiovascular.Dock = DockStyle.Fill;
                this.tabPageReport2.Controls.Add(cardiovascular);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        private void Search()
        {
            try
            {
                string beginTime = this.dateStart.Text;
                string endTime = this.dateEnd.Text;
                string categoryid = (this.cmbReportList.SelectedItem as ListItem).Value;
                string state = this.cmbReportState.Text;

                if (categoryid == "2")//肿瘤报告卡
                {
                    List<ReportConfig> listConfig = m_SqlHelper.GetReportCart(categoryid, beginTime, endTime, state);
                    TreeReport.ClearNodes();
                    foreach (ReportConfig item in listConfig)
                    {
                        InitTreeNode(item);
                    }
                }
                if (categoryid == "3")//出生缺陷报告卡
                {
                    TreeReport.ClearNodes();
                    List<ReportConfig> listConfig = m_SqlHelper.GetReportCart(categoryid, beginTime, endTime, state);
                    foreach (ReportConfig item in listConfig)
                    {
                        InitTreeNode(item);
                    }
                }
                if (categoryid == "4")//脑卒中、冠心病病例报告卡
                {
                    TreeReport.ClearNodes();
                    List<ReportConfig> listConfig = m_SqlHelper.GetReportCart(categoryid, beginTime, endTime, state);
                    foreach (ReportConfig item in listConfig)
                    {
                        InitTreeNode(item);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定树各级节点
        /// </summary>
        private void InitTreeNode(ReportConfig _reportConfig)
        {
            try
            {
                TreeListNode treeNode = null;
                treeNode = TreeReport.AppendNode(new object[] { _reportConfig.Name, "Foloder", "" }, null);
                treeNode.Tag = _reportConfig;
                InitTreeNodeTwo(_reportConfig.ReportCategory, treeNode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载树的二级节点
        /// </summary>
        /// <param name="_reportCategory"></param>
        /// <param name="treeNode"></param>
        private void InitTreeNodeTwo(List<ReportCategory> _reportCategory, TreeListNode treeNode)
        {
            try
            {
                TreeListNode Node = null;
                if (_reportCategory == null || _reportCategory.Count <= 0)
                {
                    return;
                }
                foreach (ReportCategory rc in _reportCategory)
                {
                    Node = TreeReport.AppendNode(new object[] { rc.NAME, "Leaf", "" }, treeNode);
                    Node.Tag = rc;
                    InitTreeNodeThree(rc.ReportCategoryInfo, Node);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载树的三级节点
        /// </summary>
        /// <param name="_reportCategoryInfo"></param>
        /// <param name="treeNode"></param>
        private void InitTreeNodeThree(List<ReportCategoryInfo> _reportCategoryInfo, TreeListNode treeNode)
        {
            try
            {
                TreeListNode Node = null;
                if (_reportCategoryInfo == null || _reportCategoryInfo.Count <= 0)
                {
                    return;
                }
                foreach (ReportCategoryInfo rcinfo in _reportCategoryInfo)
                {
                    string name = rcinfo.NAME;
                    Node = TreeReport.AppendNode(new object[] { name, "State", "" + rcinfo.State + "" }, treeNode);
                    Node.Tag = rcinfo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置当前登录人是否具有审核权限
        /// </summary>
        void SetButtonRole()
        {
            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AuditUserCode");
            if (valueStr.ToLower().Contains("," + m_Host.User.Id + ","))
            {
                m_HaveAuditAuthority = true;
            }
            else
            {
                m_HaveAuditAuthority = false;
            }
        }

        private void btnEditRePortCard_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //if (CurrentReprotCard.EditPass())
                //{
                //    m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                //    CurrentReprotCard.LoadPage(CurrentReprotCard.ReportID, "1", "1");
                //    SetButtonEnable();
                //    //RefreshGridControl(ReportState.Submit, CurrentReprotCard.ReportID);
                //    //CurrentReprotCard.ClearPage();
                //    //Search();//相应操作后进行数据的刷新
                //}
                //DataTable ReportList = m_SqlHelper.GetReportCategoryList();
                // _cardisshow = ReportList.Rows[0]["TABLENAME"].ToString();
                if (_cardisshow == "2")
                {
                    if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                    {
                        MessageBox.Show("请选择一条肿瘤上报记录或补录肿瘤报告信息");
                        return;
                    }
                    CurrentReprotCard.ReadOnlyControl("1");
                }
                else if (_cardisshow == "3")
                {
                    if (string.IsNullOrEmpty(ReprotCard.m_Noofinpat))
                    {
                        MessageBox.Show("请选择一条出生缺陷上报记录或补录出生缺陷报告信息");
                        return;
                    }
                    ReprotCard.ReadOnlyControl("1");
                }
                else if (_cardisshow == "4")
                {
                    //yours
                    if (string.IsNullOrEmpty(CardiovascularCard.m_Noofinpat))
                    {
                        MessageBox.Show("请选择一条心脑血管病上报记录或补录心脑血管病报告信息");
                        return;
                    }
                    CardiovascularCard.ReadOnlyControl("1");
                }

                //编辑的时候开启保存按钮
                btnEditRePortCard.Enabled = false;
                btnSaveReportCard.Enabled = true;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private bool AddEmrInput()
        {
            try
            {
                if (m_UCEmrInput == null && m_Host.CurrentPatientInfo != null)
                {
                    m_UCEmrInput = new DrectSoft.Core.MainEmrPad.New.UCEmrInput(m_Host.CurrentPatientInfo, m_Host, FloderState.None);
                    m_UCEmrInput.SetVarData(m_Host);
                    xtraTabPageEmrContant.Controls.Add(m_UCEmrInput);
                    m_UCEmrInput.HideBar();
                    m_UCEmrInput.Dock = DockStyle.Fill;



                    //m_Host.ChoosePatient(m_UCEmrInput.CurrentInpatient.NoOfFirstPage);//切换病人
                    //m_UCEmrInput = new DrectSoft.Core.MainEmrPad.New.UCEmrInput(m_Host.CurrentPatientInfo, m_Host, FloderState.None);
                    //m_UCEmrInput.SetVarData(m_Host);
                    //xtraTabPageEmrContant.Controls.Add(m_UCEmrInput);
                    //m_UCEmrInput.OnLoad();
                    //m_UCEmrInput.HideBar();
                    //m_UCEmrInput.Dock = DockStyle.Fill;

                }
                else if (m_UCEmrInput == null && m_Host.CurrentPatientInfo == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 树双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeReport_DoubleClick(object sender, EventArgs e)
        {
        }

        private void TreeReport_Click(object sender, EventArgs e)
        {
            try
            {
                TreeListNode clickedNode = this.TreeReport.FocusedNode;
                if (clickedNode != null)
                {
                    if (clickedNode.Level == 2)
                    {
                        ReportCategoryInfo info = null;
                        if (clickedNode.Tag != null && clickedNode.Tag is ReportCategoryInfo)
                        {
                            info = clickedNode.Tag as ReportCategoryInfo;
                            //add by ywk 2013年8月14日 16:37:33 选择报告卡后要激活Tabpage
                            this.ActiveControl = this.TabPageCardInfo;
                            this.TabPageCardInfo.Controls[0].Focus();
                            this.TabPageCardInfo.SelectedTabPageIndex = 0;
                        }
                        if (!string.IsNullOrEmpty(info.ID))
                        {
                            SqlHelper sqlHelp = new SqlHelper(m_Host.SqlHelper);
                            if (TabPageCardInfo.SelectedTabPage == tabPageReport)  //edit by jxh 报告卡选择  江西省居民肿瘤病例报告卡
                            {
                                CurrentReprotCard.LoadPage(info.ID, "1", "2");
                                // xtraTabPageEmrContant.PageEnabled = true;
                                _reportEntity = sqlHelp.GetReportCardEntity(info.ID);
                            }
                            else if (TabPageCardInfo.SelectedTabPage == tabPageReport1)  //出生缺陷报告卡
                            {
                                ReprotCard.LoadPage(info.ID, "1", "2");
                                //xtraTabPageEmrContant.PageEnabled = true;
                                _reportEntity1 = sqlHelp.GetBirthDefectsReportCardEntity(info.ID);
                            }
                            else if (TabPageCardInfo.SelectedTabPage == tabPageReport2)//脑卒中、冠心病病例
                            {
                                //yours

                                CardiovascularCard.LoadPage(info.ID, "1", "2");
                                //xtraTabPageEmrContant.PageEnabled = true;
                                _CardiovasularEntity = sqlHelp.GetCardiovasularEntity(info.ID);

                            }
                            else if (TabPageCardInfo.SelectedTabPage == xtraTabPageEmrContant)
                            {
                                if (m_UCEmrInput != null)
                                {
                                    m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(info.Noofinpat));
                                    m_UCEmrInput.HideBar();
                                }


                            }
                            else
                            {
                                //this.ActiveControl = this.xtraTabPageCardInfo;
                                //this.xtraTabPageCardInfo.Controls[0].Focus();
                                //CurrentReprotCard.LoadPage(info.ID, "1", "2");
                                //xtraTabPageCardInfo.a
                            }
                            CurrentReprotCard.SetFocusControl();
                        }
                        //string searchedrep = this.cmbReportList.Text.ToString().Trim();
                        SetButtonEnable2();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 这段代码太臃肿了
        /// </summary>
        /// <param name="lookcard"></param>
        private void SetButtonEnable2()
        {
            if (tabPageReport.PageVisible)
            {
                if (_reportEntity != null)
                {
                    btnEditRePortCard.Enabled = true;//修改
                    btnSaveReportCard.Enabled = true;//保存
                    btnDelReportCard.Enabled = true;//删除
                    btnSumbit.Enabled = true;//提交
                    btnAudit.Enabled = true;//审核通过
                    btnReject.Enabled = true;//否决

                    if (_reportEntity.State == "4" || _reportEntity.State == "6" || _reportEntity.State == "7")
                    {
                        //审核通过的记录不能 修改、保存、删除、提交、审核通过、否决    
                        //上报状态的记录暂时不能进行任何操作 
                        //作废的记录不能 修改、保存、删除、提交、审核通过、否决

                        btnEditRePortCard.Enabled = false;//修改
                        btnSaveReportCard.Enabled = false;//保存
                        btnDelReportCard.Enabled = false;//删除
                        btnSumbit.Enabled = false;//提交
                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_reportEntity.State == "5")
                    {
                        //被否决的记录不能  审核通过、否决

                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_reportEntity.State == "3")
                    {
                        //被撤回的记录不能 审核通过、否决

                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_reportEntity.State == "2")
                    {
                        //被提交的记录不能 修改，保存，删除，提交

                        btnEditRePortCard.Enabled = false;//修改
                        btnSaveReportCard.Enabled = false;//保存
                        btnDelReportCard.Enabled = false;//删除
                        btnSumbit.Enabled = false;//提交
                    }
                    else if (_reportEntity.State == "1")
                    {
                        //新增保存状态的记录不能 审核通过 否决

                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_reportEntity.State == "")
                    {
                        btnEditRePortCard.Enabled = false;//修改
                        btnSaveReportCard.Enabled = false;//保存
                        btnDelReportCard.Enabled = false;//删除
                        btnSumbit.Enabled = false;//提交
                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                }
                else
                {
                    btnEditRePortCard.Enabled = false;//修改
                    btnSaveReportCard.Enabled = false;//保存
                    btnDelReportCard.Enabled = false;//删除
                    btnSumbit.Enabled = false;//提交
                    btnAudit.Enabled = false;//审核通过
                    btnReject.Enabled = false;//否决
                }
            }

            if (tabPageReport1.PageVisible)
            {
                if (_reportEntity1 != null)
                {
                    btnEditRePortCard.Enabled = true;//修改
                    btnSaveReportCard.Enabled = true;//保存
                    btnDelReportCard.Enabled = true;//删除
                    btnSumbit.Enabled = true;//提交
                    btnAudit.Enabled = true;//审核通过
                    btnReject.Enabled = true;//否决

                    if (_reportEntity1.State == "4" || _reportEntity1.State == "6" || _reportEntity1.State == "7")
                    {
                        //审核通过的记录不能 修改、保存、删除、提交、审核通过、否决    
                        //上报状态的记录暂时不能进行任何操作 
                        //作废的记录不能 修改、保存、删除、提交、审核通过、否决

                        btnEditRePortCard.Enabled = false;//修改
                        btnSaveReportCard.Enabled = false;//保存
                        btnDelReportCard.Enabled = false;//删除
                        btnSumbit.Enabled = false;//提交
                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_reportEntity1.State == "5")
                    {
                        //被否决的记录不能  审核通过、否决

                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_reportEntity1.State == "3")
                    {
                        //被撤回的记录不能 审核通过、否决

                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_reportEntity1.State == "2")
                    {
                        //被提交的记录不能 修改，保存，删除，提交

                        btnEditRePortCard.Enabled = false;//修改
                        btnSaveReportCard.Enabled = false;//保存
                        btnDelReportCard.Enabled = false;//删除
                        btnSumbit.Enabled = false;//提交
                    }
                    else if (_reportEntity1.State == "1")
                    {
                        //新增保存状态的记录不能 审核通过 否决

                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_reportEntity1.State == "")
                    {
                        btnEditRePortCard.Enabled = false;//修改
                        btnSaveReportCard.Enabled = false;//保存
                        btnDelReportCard.Enabled = false;//删除
                        btnSumbit.Enabled = false;//提交
                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                }
                else
                {
                    btnEditRePortCard.Enabled = false;//修改
                    btnSaveReportCard.Enabled = false;//保存
                    btnDelReportCard.Enabled = false;//删除
                    btnSumbit.Enabled = false;//提交
                    btnAudit.Enabled = false;//审核通过
                    btnReject.Enabled = false;//否决
                }
            }

            if (tabPageReport2.PageVisible)
            {
                if (_CardiovasularEntity != null)
                {
                    btnEditRePortCard.Enabled = true;//修改
                    btnSaveReportCard.Enabled = true;//保存
                    btnDelReportCard.Enabled = true;//删除
                    btnSumbit.Enabled = true;//提交
                    btnAudit.Enabled = true;//审核通过
                    btnReject.Enabled = true;//否决

                    if (_CardiovasularEntity.STATE == "4" || _CardiovasularEntity.STATE == "6" || _CardiovasularEntity.STATE == "7")
                    {
                        //审核通过的记录不能 修改、保存、删除、提交、审核通过、否决    
                        //上报状态的记录暂时不能进行任何操作 
                        //作废的记录不能 修改、保存、删除、提交、审核通过、否决

                        btnEditRePortCard.Enabled = false;//修改
                        btnSaveReportCard.Enabled = false;//保存
                        btnDelReportCard.Enabled = false;//删除
                        btnSumbit.Enabled = false;//提交
                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_CardiovasularEntity.STATE == "5")
                    {
                        //被否决的记录不能  审核通过、否决

                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_CardiovasularEntity.STATE == "3")
                    {
                        //被撤回的记录不能 审核通过、否决

                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_CardiovasularEntity.STATE == "2")
                    {
                        //被提交的记录不能 修改，保存，删除，提交

                        btnEditRePortCard.Enabled = false;//修改
                        btnSaveReportCard.Enabled = false;//保存
                        btnDelReportCard.Enabled = false;//删除
                        btnSumbit.Enabled = false;//提交
                    }
                    else if (_CardiovasularEntity.STATE == "1")
                    {
                        //新增保存状态的记录不能 审核通过 否决

                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                    else if (_CardiovasularEntity.STATE == "")
                    {
                        btnEditRePortCard.Enabled = false;//修改
                        btnSaveReportCard.Enabled = false;//保存
                        btnDelReportCard.Enabled = false;//删除
                        btnSumbit.Enabled = false;//提交
                        btnAudit.Enabled = false;//审核通过
                        btnReject.Enabled = false;//否决
                    }
                }
                else
                {
                    btnEditRePortCard.Enabled = false;//修改
                    btnSaveReportCard.Enabled = false;//保存
                    btnDelReportCard.Enabled = false;//删除
                    btnSumbit.Enabled = false;//提交
                    btnAudit.Enabled = false;//审核通过
                    btnReject.Enabled = false;//否决
                }
            }


            //根据登录人的权限判断按钮的使用情况
            if (m_HaveAuditAuthority)
            {
                btnCardConfig.Enabled = true;//配置

                if (_reportEntity != null && _reportEntity.State == "2")
                {//对于有审核权限的用户，可以对已经提交的报告进行审核后否决操作
                    btnAudit.Enabled = true;//审核通过
                    btnReject.Enabled = true;//否决
                }
            }
            else
            {
                btnCardConfig.Enabled = false;//配置
                btnAudit.Enabled = false;//审核通过
                btnReject.Enabled = false;//否决
            }

            //必须是报告单的创建人或报告医师才能进行 修改、保存、删除、提交操作
            if (_reportEntity != null)
            {
                if (_reportEntity.CreateUsercode != m_Host.User.Id && _reportEntity.ReportDoctor != m_Host.User.Id)
                {
                    if (btnEditRePortCard.Enabled)
                    {
                        btnEditRePortCard.Enabled = false;
                        btnSaveReportCard.Enabled = false;
                        btnDelReportCard.Enabled = false;
                        btnSumbit.Enabled = false;
                    }
                }
            }

            if (btnEditRePortCard.Enabled)
            {
                btnSaveReportCard.Enabled = false;
            }
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (tabPageReport.PageVisible)
                {
                    if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                    {
                        MessageBox.Show("请选择一条肿瘤上报记录或补录肿瘤报告信息");
                        return;
                    }
                    string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AuditUserCode");

                    if (valueStr.ToLower().Contains("," + m_Host.User.Id + ","))
                    {
                        if (CurrentReprotCard.Approv())
                        {
                            //m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                            Search();
                            SetButtonEnable2();
                            UpdateRecordState();
                            ReprotCard.ReadOnlyControl("2");
                            //CurrentReprotCard.ClearPage();
                        }
                    }
                    else
                    {
                        MessageBox.Show("对不起您没有审核权限");
                    }
                }
                if (tabPageReport1.PageVisible)   // add jxh  出生缺陷卡审核
                {
                    if (string.IsNullOrEmpty(ReprotCard.m_Noofinpat))
                    {
                        MessageBox.Show("请选择一条出生缺陷上报记录或补录出生缺陷报告信息");
                        return;
                    }
                    string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AuditUserCode");

                    if (valueStr.ToLower().Contains("," + m_Host.User.Id + ","))
                    {
                        if (ReprotCard.Approv())
                        {
                            //m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                            Search();
                            SetButtonEnable2();
                            UpdateRecordState();
                            CurrentReprotCard.ReadOnlyControl("2");
                            //CurrentReprotCard.ClearPage();
                        }
                    }
                    else
                    {
                        MessageBox.Show("对不起您没有审核权限");
                    }
                }

                if (tabPageReport2.PageVisible)   // add jxh  脑卒中审核
                {
                    if (string.IsNullOrEmpty(CardiovascularCard.m_Noofinpat))
                    {
                        MessageBox.Show("请选择一条心脑血管病上报记录或补录心脑血管病报告信息");
                        return;
                    }
                    string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AuditUserCode");

                    if (valueStr.ToLower().Contains("," + m_Host.User.Id + ","))
                    {
                        if (CardiovascularCard.Approv())
                        {
                            //m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                            Search();
                            SetButtonEnable2();
                            UpdateRecordState();
                            CurrentReprotCard.ReadOnlyControl("2");
                            //CurrentReprotCard.ClearPage();
                        }
                    }
                    else
                    {
                        MessageBox.Show("对不起您没有审核权限");
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnQurey_Click(object sender, EventArgs e)
        {
            try
            {
                Search();
                _reportEntity = null;
                CurrentReprotCard.m_ReportCardEntity = null;
                CurrentReprotCard.m_Noofinpat = null;
                CurrentReprotCard.m_upCount = 0;
                CurrentReprotCard.ClearPage();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// <date>2013-07-12</date>
        /// 注册事件
        /// <auth>XLB</auth>
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                btnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnPrint_ItemClick);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 打印方法

        /// <summary>
        /// <date>2013-07-12</date>
        /// 获得打印图片集合
        /// <auth>XLB</auth>
        /// </summary>
        /// <returns></returns>
        private List<Metafile> GetPrintImages()
        {
            try
            {
                if (tabPageReport.PageVisible)
                {
                    XmlCommomOp.Doc = null;
                    XmlCommomOp.xmlPath = AppDomain.CurrentDomain.BaseDirectory + "reportthe.xml";
                    XmlCommomOp.CreaetDocument();
                    XmlCommomOp.BindingDate(null, CreateParamsData(_reportEntity));

                }
                if (tabPageReport1.PageVisible)
                {
                    XmlCommomOp.Doc = null;
                    XmlCommomOp.xmlPath = AppDomain.CurrentDomain.BaseDirectory + "reportthe_QX.xml";
                    XmlCommomOp.CreaetDocument();
                    XmlCommomOp.BindingDate(null, CreateParamsDataList(_reportEntity1));

                }
                if (tabPageReport2.PageVisible)
                {
                    XmlCommomOp.Doc = null;
                    XmlCommomOp.xmlPath = AppDomain.CurrentDomain.BaseDirectory + "reportthe_NZZ.xml";
                    XmlCommomOp.CreaetDocument();
                    XmlCommomOp.BindingDate(null, CreateParamsDataSet(_CardiovasularEntity));

                }
                List<Metafile> listMetafile = DrawOp.MakeImagesByXmlDocument(XmlCommomOp.Doc);
                return listMetafile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <date>2013-07-12</date>
        /// 创建参数集合
        /// <auth>jxh</auth>
        /// </summary>
        /// <param name="reportEntity">报告卡实体</param>
        /// <returns>参数集合</returns>
        private Dictionary<string, ParamObject> CreateParamsDataList(ReportCardEntity1 reportEntity)
        {
            try
            {
                //参数集合
                Dictionary<string, ParamObject> dictionaryParams = new Dictionary<string, ParamObject>();
                if (reportEntity == null)
                {
                    reportEntity = new ReportCardEntity1();
                }
                //获取实体所有属性集合
                PropertyInfo[] propertys = reportEntity.GetType().GetProperties();
                foreach (PropertyInfo property in propertys)
                {
                    //参数结构体信息
                    ParamObject paramsmeter = new ParamObject(property.Name, "", property.GetValue(reportEntity, null) == null ? "" : property.GetValue(reportEntity, null).ToString());
                    if (!dictionaryParams.ContainsKey(property.Name))
                    {
                        dictionaryParams.Add(property.Name, paramsmeter);
                    }
                }
                return dictionaryParams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <date>2013-07-12</date>
        /// 创建参数集合
        /// <auth>jxh</auth>
        /// </summary>
        /// <param name="reportEntity">报告卡实体</param>
        /// <returns>参数集合</returns>
        private Dictionary<string, ParamObject> CreateParamsDataSet(CardiovasularEntity _CardiovasularEntity)
        {
            try
            {
                //参数集合
                Dictionary<string, ParamObject> dictionaryParams = new Dictionary<string, ParamObject>();
                if (_CardiovasularEntity == null)
                {
                    _CardiovasularEntity = new CardiovasularEntity();
                }
                //获取实体所有属性集合
                PropertyInfo[] propertys = _CardiovasularEntity.GetType().GetProperties();
                foreach (PropertyInfo property in propertys)
                {
                    //参数结构体信息
                    ParamObject paramsmeter = new ParamObject(property.Name, "", property.GetValue(_CardiovasularEntity, null) == null ? "" : property.GetValue(_CardiovasularEntity, null).ToString());
                    if (!dictionaryParams.ContainsKey(property.Name))
                    {
                        dictionaryParams.Add(property.Name, paramsmeter);
                    }
                }
                return dictionaryParams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <date>2013-07-12</date>
        /// 创建参数集合
        /// <auth>XLB</auth>
        /// </summary>
        /// <param name="reportEntity">报告卡实体</param>
        /// <returns>参数集合</returns>
        private Dictionary<string, ParamObject> CreateParamsData(ReportCardEntity reportEntity)
        {
            try
            {
                //参数集合
                Dictionary<string, ParamObject> dictionaryParams = new Dictionary<string, ParamObject>();
                if (reportEntity == null)
                {
                    reportEntity = new ReportCardEntity();
                }
                //获取实体所有属性集合
                PropertyInfo[] propertys = reportEntity.GetType().GetProperties();
                foreach (PropertyInfo property in propertys)
                {
                    //参数结构体信息
                    ParamObject paramsmeter = new ParamObject(property.Name, "", property.GetValue(reportEntity, null) == null ? "" : property.GetValue(reportEntity, null).ToString());
                    if (!dictionaryParams.ContainsKey(property.Name))
                    {
                        dictionaryParams.Add(property.Name, paramsmeter);
                    }
                }
                return dictionaryParams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// <date>2013-07-12</date>
        /// 打印事件
        /// <auth>XLB</auth>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (tabPageReport.PageVisible)
                {
                    if (CurrentReprotCard.m_ReportCardEntity == null)
                    {
                        MessageBox.Show("尚未选择报告卡");
                        return;
                    }
                    CurrentReprotCard.GetEntityUI(CurrentReprotCard.m_ReportCardEntity);
                    _reportEntity = CurrentReprotCard.m_ReportCardEntity;
                    if (_reportEntity != null)
                    {
                        if (_reportEntity.State == "")
                        {
                            MessageBox.Show("请保存报告卡");
                        }
                        else
                        {
                            List<Metafile> metafileList = GetPrintImages();
                            DrawOp.PrintView(metafileList);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择报告卡");
                    }
                }
                if (tabPageReport1.PageVisible)
                {
                    if (ReprotCard.m_ReportCardEntity == null)
                    {
                        MessageBox.Show("尚未选择报告卡");
                        return;
                    }
                    ReprotCard.GetEntityUI(ReprotCard.m_ReportCardEntity);
                    _reportEntity1 = ReprotCard.m_ReportCardEntity;
                    if (_reportEntity1 != null)
                    {
                        if (_reportEntity1.State == "")
                        {
                            MessageBox.Show("请保存报告卡");
                        }
                        else
                        {
                            List<Metafile> metafileList = GetPrintImages();
                            DrawOp.PrintView(metafileList);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择报告卡");
                    }
                }
                if (tabPageReport2.PageVisible)
                {
                    if (CardiovascularCard.m_CardiovasularEntity == null)
                    {
                        MessageBox.Show("尚未选择报告卡");
                        return;
                    }
                    _CardiovasularEntity = CardiovascularCard.GetEntityUI();
                    _CardiovasularEntity = CardiovascularCard.m_CardiovasularEntity;
                    if (_CardiovasularEntity != null)
                    {
                        if (_CardiovasularEntity.STATE == "")
                        {
                            MessageBox.Show("请保存报告卡");
                        }
                        else
                        {
                            List<Metafile> metafileList = GetPrintImages();
                            DrawOp.PrintView(metafileList);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择报告卡");
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        /// <summary>
        /// <Date>2013-07-18</Date>
        /// 设置树各级节点状态图片
        /// <auth>XLB</auth>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreeReport_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            try
            {
                TreeListNode node = e.Node;
                //节点属性为文件则标为第一级节点
                if (node.GetValue("Modify") != null)
                {
                    switch (node.GetValue("Modify").ToString())
                    {
                        //第一级节点
                        case "Foloder":
                            if (node.Expanded)
                            {
                                e.NodeImageIndex = 3;
                            }
                            else
                            {
                                e.NodeImageIndex = 2;
                            }
                            break;
                        //第二级节点
                        case "Leaf":
                            e.NodeImageIndex = 1;
                            break;
                        //第三级节点
                        case "State":
                            switch (node.GetValue("Status").ToString().Trim())
                            {
                                //新增保存状态
                                case "新增保存":
                                    e.NodeImageIndex = 14;
                                    break;
                                //提交状态
                                case "提交":
                                    e.NodeImageIndex = 13;
                                    break;
                                //审核通过状态
                                case "审核通过":
                                    e.NodeImageIndex = 18;
                                    break;
                                //否决状态
                                case "否决":
                                    e.NodeImageIndex = 21;
                                    break;
                                //上报状态
                                case "上报":
                                    e.NodeImageIndex = 19;
                                    break;
                                //撤销或作废状态
                                case "撤销":
                                case "作废":
                                    e.NodeImageIndex = 10;
                                    break;
                                default:
                                    e.NodeImageIndex = 0;
                                    break;

                            }
                            break;
                    }
                }
                else
                {
                    e.NodeImageIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 更新记录的状态
        /// </summary>
        private void UpdateRecordState()
        {
            if (this.tabPageReport1.PageVisible == true)//肿瘤报告卡
            {
                if (CurrentReprotCard.m_ReportCardEntity != null)
                {
                    string reportID = CurrentReprotCard.m_ReportCardEntity.Report_Id;
                    TreeListNode node = null;
                    GetNodeByReportID(reportID, null, ref node);
                    if (node != null)
                    {
                        string stateName = SqlHelper.StateName(CurrentReprotCard.m_ReportCardEntity.State);
                        node.SetValue("Status", stateName);
                    }
                }
            }
            if (this.tabPageReport2.PageVisible == true)//脑卒中报告卡
            {

            }

        }

        private void GetNodeByReportID(string reportID, TreeListNodes nodes, ref TreeListNode valueNode)
        {
            if (valueNode != null) return;

            if (nodes == null)
            {
                nodes = TreeReport.Nodes;
            }

            foreach (TreeListNode subNode in nodes)
            {
                if (subNode.Nodes.Count > 0)
                {
                    GetNodeByReportID(reportID, subNode.Nodes, ref valueNode);
                }
                ReportCategoryInfo info = subNode.Tag as ReportCategoryInfo;
                if (info != null && info.ID == reportID)
                {
                    valueNode = subNode;
                }
            }
        }

        private void xtraTabPageCardInfo_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (_cardisshow == "2")
                {
                    if (CurrentReprotCard.m_ReportCardEntity != null)     //  肿瘤
                    {
                        decimal noofinpat = Convert.ToDecimal(CurrentReprotCard.m_ReportCardEntity.Report_Noofinpat);
                        m_Host.ChoosePatient(Convert.ToDecimal(noofinpat), FloderState.None.ToString());//切换病人

                        if (AddEmrInput())
                        {
                            //if (m_UCEmrInput.CurrentInpatient == null || m_UCEmrInput.CurrentInpatient.NoOfFirstPage != noofinpat)
                            //{
                            m_UCEmrInput.PatientChangedByIEmrHost(noofinpat);
                            m_UCEmrInput.HideBar();
                            //}
                        }
                        else
                        {
                            MessageBox.Show("请选择病人");
                        }
                    }
                }
                if (_cardisshow == "3")
                {
                    if (ReprotCard.m_ReportCardEntity != null)          //缺陷
                    {
                        decimal noofinpat = Convert.ToDecimal(ReprotCard.m_ReportCardEntity.ReportNoofinpat);
                        m_Host.ChoosePatient(Convert.ToDecimal(noofinpat), FloderState.None.ToString());//切换病人

                        if (AddEmrInput())
                        {
                            //if (m_UCEmrInput.CurrentInpatient == null || m_UCEmrInput.CurrentInpatient.NoOfFirstPage != noofinpat)
                            //{
                            m_UCEmrInput.PatientChangedByIEmrHost(noofinpat);
                            m_UCEmrInput.HideBar();
                            //}
                        }
                        else
                        {
                            MessageBox.Show("请选择病人");
                        }
                    }
                }
                if (_cardisshow == "4")
                {
                    if (CardiovascularCard.m_CardiovasularEntity != null)          //缺陷
                    {
                        decimal noofinpat = Convert.ToDecimal(CardiovascularCard.m_CardiovasularEntity.NOOFINPAT);
                        m_Host.ChoosePatient(Convert.ToDecimal(noofinpat), FloderState.None.ToString());//切换病人

                        if (AddEmrInput())
                        {
                            //if (m_UCEmrInput.CurrentInpatient == null || m_UCEmrInput.CurrentInpatient.NoOfFirstPage != noofinpat)
                            //{
                            m_UCEmrInput.PatientChangedByIEmrHost(noofinpat);
                            m_UCEmrInput.HideBar();
                            //}
                        }
                        else
                        {
                            MessageBox.Show("请选择病人");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        ///弹出报表统计模块
        ///add by ywk 2013年7月30日 19:43:27
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReportList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ReportStatistic m_reportList = new ReportStatistic(m_Host);

            m_reportList.Show();
            m_reportList.StartPosition = FormStartPosition.CenterParent;
        }
        /// <summary>
        /// 下拉选择显示报告卡  add  by jxh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbReportList_TextChanged(object sender, EventArgs e)
        {
            _cardisshow = (this.cmbReportList.SelectedItem as ListItem).Value;
            if (_cardisshow == "2")
            {
                if (CurrentReprotCard != null)
                {
                    CurrentReprotCard.m_ReportCardEntity = null;
                    CurrentReprotCard.ClearPage();
                    CurrentReprotCard.ReadOnlyControl("2");
                }
                this.tabPageReport1.PageVisible = false;
                this.tabPageReport2.PageVisible = false;
                this.tabPageReport.PageVisible = true;
                this.TabPageCardInfo.SelectedTabPage = tabPageReport;
                Search();


                btnEditRePortCard.Enabled = false;
                btnSaveReportCard.Enabled = false;
                btnDelReportCard.Enabled = false;
                btnSumbit.Enabled = false;
            }
            else if (_cardisshow == "3")
            {
                ReprotCard.m_ReportCardEntity = null;
                this.tabPageReport.PageVisible = false;
                this.tabPageReport1.PageVisible = true;
                this.tabPageReport2.PageVisible = false;
                this.TabPageCardInfo.SelectedTabPage = tabPageReport1;
                Search();
                ReprotCard.ClearPage();
                ReprotCard.ReadOnlyControl("2");
                btnEditRePortCard.Enabled = false;
                btnSaveReportCard.Enabled = false;
                btnDelReportCard.Enabled = false;
                btnSumbit.Enabled = false;
            }
            else if (_cardisshow == "4")
            {
                CardiovascularCard.m_CardiovasularEntity = null;
                this.tabPageReport.PageVisible = false;
                this.tabPageReport1.PageVisible = false;
                this.tabPageReport2.PageVisible = true;
                this.TabPageCardInfo.SelectedTabPage = tabPageReport2;
                Search();
                CardiovascularCard.ClearPageControl();
                CardiovascularCard.ReadOnlyControl("2");
                btnEditRePortCard.Enabled = false;
                btnSaveReportCard.Enabled = false;
                btnDelReportCard.Enabled = false;
                btnSumbit.Enabled = false;
            }
        }
    }
}