using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Library;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Core.ZymosisReport.Print;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public partial class ApproveMainForm : DevBaseForm, IStartPlugIn
    {
        /// <summary>
        /// 病历内容窗体
        /// </summary>
        UCEmrInput m_UCEmrInput;
        IEmrHost m_App;
        SqlHelper m_SqlHelper;
        Auditor m_UserRole;
        SearchDataEntity m_SearchDataEntity;
        ZymosisReportEntity m_ZymosisReportEntity;
        private static Drawingform GetDrawingform(ZymosisReportEntity m_ZymosisReportEntitys)
        {
            Drawingform drawingform = new Drawingform(m_ZymosisReportEntitys);
            return drawingform;
        }
        //edit by cyq 2012-10-30 
        UCReportCard CurrentReprotCard
        {
            get
            {
                if (xtraTabPageCardInfo.Controls.Count == 1)
                {
                    UCReportCard card = xtraTabPageCardInfo.Controls[0] as UCReportCard;
                    if (card != null)
                    {
                        return card;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        #region IStartPlugIn 成员

        public IPlugIn Run(IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_App = host;
            m_SqlHelper = new SqlHelper(host.SqlHelper);
            m_SearchDataEntity = new SearchDataEntity();

            return plg;
        }

        #endregion

        #region .ctor
        /// <summary>
        /// .ctor
        /// </summary>
        public ApproveMainForm()
        {
            InitializeComponent();
            InitControlState();
        }
        #endregion

        public void InitControlState()
        {
            checkButtonSave.Checked = true;
        }

        private void AddUCReportCard()
        {
            //edit by cyq 2012-10-30
            UCReportCard card = new UCReportCard(m_App);
            //ResizeReprotCard();
            //card.Dock = DockStyle.Fill;
            //card.Height = 750;
            //panelControl2.Controls.Add(card);
            //panelControl2.Height = 780;
            this.xtraTabPageCardInfo.Controls.Add(card);
        }

        private void AddEmrInput()
        {
            m_UCEmrInput = new UCEmrInput();
            m_UCEmrInput.HideBar();
            RecordDal m_RecordDal = new RecordDal(m_App.SqlHelper);
            m_UCEmrInput.SetInnerVar(m_App, m_RecordDal);
            xtraTabPageEmrContant.Controls.Add(m_UCEmrInput);
            m_UCEmrInput.Dock = DockStyle.Fill;
        }

        /// <summary>
        /// tab页切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControlCardInfo_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (gridViewCardList.FocusedRowHandle < 0)
                {
                    return;
                }

                if (xtraTabControlCardInfo.SelectedTabPage == xtraTabPageEmrContant)
                {
                    LoadEmrContent();
                    this.ActiveControl = this.xtraTabPageEmrContant;
                }
                else
                {
                    this.ActiveControl = this.xtraTabPageCardInfo;
                    this.xtraTabPageCardInfo.Controls[0].Focus();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void LoadEmrContent()
        {
            string noofinpat = CurrentReprotCard.m_Noofinpat;
            if (!string.IsNullOrEmpty(noofinpat))
            {
                //m_App.ChoosePatient(Convert.ToDecimal(noofinpat));
                //m_UCEmrInput.PatientChanged();
                m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(noofinpat));
                m_UCEmrInput.HideBar();
            }
        }

        #region Load
        private void MainForm_Load(object sender, EventArgs e)
        {

            this.WindowState = FormWindowState.Maximized;
            m_UserRole = new Auditor(m_SqlHelper);
            InitDeptList();
            AddUCReportCard();
            //上报日期段默认一周
            this.CreateDateStart.DateTime = DateTime.Now.AddDays(-7);//开始时间
            this.CreateDateEnd.DateTime = DateTime.Now;//结束时间

            Search();
            AddEmrInput();

            this.ActiveControl = this.textEditName;
            this.textEditName.Focus();
        }

        private void InitDeptList()
        {
            DataTable dt = m_SqlHelper.GetDeptList();
            lookUpEditDept.Properties.DataSource = dt;
            lookUpEditDept.Properties.DisplayMember = "NAME";
            lookUpEditDept.Properties.ValueMember = "ID";
            lookUpEditDept.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUpEditDept.EditValue = "";
            lookUpEditDept.EditValue = m_App.User.CurrentDeptWard.DeptId;
        }
        #endregion

        /// <summary>
        /// 否决事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemReject_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                {
                    MyMessageBox.Show("请选择一条传染病上报记录");
                    return;
                }
                if (CurrentReprotCard.UnPassApprove())
                {
                    m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                    SetButtonEnable();
                    RefreshGridControl(ReportState.UnPassApprove, CurrentReprotCard.ReportID);
                    CurrentReprotCard.ClearPage();
                }
                Search();//否决或者通过后进行刷新
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 通过事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemPass_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                {
                    MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return;
                }

                if (CurrentReprotCard.Approv())
                {
                    m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                    SetButtonEnable();
                    RefreshGridControl(ReportState.Approv, CurrentReprotCard.ReportID);
                    CurrentReprotCard.ClearPage();
                }
                Search();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void RefreshGridControl(ReportState rs, string reportID)
        {
            //删除
            if ((rs == ReportState.UnPassApprove && checkButtonSave.Checked)
                || (rs == ReportState.Approv && checkButtonSave.Checked))
            {
                DataTable dt = gridControlCardList.DataSource as DataTable;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["REPORT_ID"].ToString() == reportID)
                        {
                            dt.Rows.Remove(dr);
                            break;
                        }
                    }
                    dt.AcceptChanges();
                }
            }
        }

        /// <summary>
        /// 报表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //报表
                ReportForms _ReportForms = new ReportForms(m_App);
                _ReportForms.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 退出事件
        /// edit by Yanqiao.Cai 2012-11-13
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MyMessageBox.Show("您确定要退出吗？", "退出", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                //退出
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 查询
        /// <summary>
        /// 查询事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// 1、add try ... catch
        /// 2、添加日期验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CreateDateStart.DateTime > CreateDateEnd.DateTime)
                {
                    MyMessageBox.Show("上报开始日期不能大于结束日期");
                    CreateDateStart.Focus();
                    return;
                }
                else if (CreateDateStart.DateTime > DateTime.Now)
                {
                    MyMessageBox.Show("上报开始日期不能大于当前日期");
                    CreateDateStart.Focus();
                    return;
                }
                SetWaitDialogCaption("正在查询，请稍等...");
                Search();
                CurrentReprotCard.ClearPage();
                HideWaitDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void Search()
        {
            m_SearchDataEntity.FirstReport = checkEditFirst.Checked ? "1" : ""; //1、初次报告  
            m_SearchDataEntity.ModifiedReport = checkEditModify.Checked ? "2" : ""; //2、订正报告
            m_SearchDataEntity.PatientName = textEditName.Text.Trim();
            m_SearchDataEntity.PatID = textEditPatID.Text.Trim();
            m_SearchDataEntity.DeptID = lookUpEditDept.EditValue.ToString();
            m_SearchDataEntity.Owner = "";//表单申请人为空
            //新增的按日期查询 add by ywk 2012年3月26日15:54:41
            m_SearchDataEntity.CreateDateStart = CreateDateStart.DateTime.ToString("yyyy-MM-dd");//上报日期开始
            m_SearchDataEntity.CreateDateEnd = CreateDateEnd.DateTime.ToString("yyyy-MM-dd");//上报日期结束

            string status = "";
            if (checkButtonSave.Checked)//已保存
            {
                status = ((int)ZymosisReportEnum.Save).ToString();
            }
            else if (checkButtonSumbit.Checked)//已提交 edit by ywk 2012年3月28日9:40:15
            {
                status = ((int)ZymosisReportEnum.Submit).ToString();
            }
            else if (checkButtonAudit.Checked)//已审核
            {
                status = ((int)ZymosisReportEnum.Pass).ToString();
            }
            else if (checkButtonReject.Checked)//已否决
            {
                status = ((int)ZymosisReportEnum.Reject).ToString();
            }
            m_SearchDataEntity.Status = status == "" ? "" : status.ToString();
            string querytype = "1";//受时间限制
            DataTable dt = m_SqlHelper.GetCardList(m_SearchDataEntity, querytype);
            gridControlCardList.DataSource = dt;
        }
        #endregion

        #region public methods of WaitDialog
        WaitDialogForm m_WaitDialog;
        public void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }
            else
            {
                m_WaitDialog = new WaitDialogForm(caption, "提示");
                m_WaitDialog.Visible = true;
            }

        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        #endregion

        private void SetGridControlConsultationColor()
        {
            StyleFormatCondition cn1;
            cn1 = new StyleFormatCondition(FormatConditionEnum.Equal, gridViewCardList.Columns[m_App.PublicMethod.ConvertProperty("REPORTTYPENAME")], null, "订正报告");
            cn1.Appearance.BackColor = Color.LightGreen;
            cn1.Appearance.ForeColor = Color.DarkGreen;
            gridViewCardList.FormatConditions.Add(cn1);
            StyleFormatCondition cn2;
            cn2 = new StyleFormatCondition(FormatConditionEnum.Equal, gridViewCardList.Columns[m_App.PublicMethod.ConvertProperty("REPORTTYPENAME")], null, "初步报告");
            cn2.Appearance.BackColor = Color.LightBlue;
            cn2.Appearance.ForeColor = Color.DarkBlue;
            gridViewCardList.FormatConditions.Add(cn2);
        }

        private void checkEditFirst_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEditFirst.Checked && !checkEditModify.Checked)
            {
                checkEditModify.Checked = true;
            }
        }

        private void checkEditModify_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEditModify.Checked && !checkEditFirst.Checked)
            {
                checkEditFirst.Checked = true;
            }
        }

        private void gridControlCardList_MouseDown(object sender, MouseEventArgs e)
        {
            int rowIndex = gridViewCardList.CalcHitInfo(e.Location).RowHandle;
            if (rowIndex >= 0)
            {
                DataRowView dr = gridViewCardList.GetRow(rowIndex) as DataRowView;
                if (dr != null)
                {
                    string reportID = dr["report_id"].ToString();
                    string noofinpat = dr["noofinpat"].ToString();
                    m_UserRole.RefreshValue(reportID);
                    SetButtonEnable();
                    //string cardReadOnly = m_UserRole.CanPass ? "1": "2";//判断是否可以编辑
                    //add by yxy 根据需求 审核人随时可以编辑
                    string cardReadOnly = "1";//判断是否可以编辑
                    CurrentReprotCard.LoadPage(reportID, "1", cardReadOnly);

                    if (xtraTabControlCardInfo.SelectedTabPage == xtraTabPageEmrContant)
                    {
                        //m_App.ChoosePatient(Convert.ToDecimal(noofinpat));
                        //m_UCEmrInput.PatientChanged();
                        m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(noofinpat));
                    }
                    CurrentReprotCard.SetFocusControl();
                }
            }
        }

        void SetButtonEnable()
        {
            //barLargeButtonItemPass.Enabled = m_UserRole.CanPass;
            //barLargeButtonItemReject.Enabled = m_UserRole.CanReject;
            //add by yxy 保健科要求随时能够修改保存
            barLargeButtonItemPass.Enabled = true;
            barLargeButtonItemReject.Enabled = true;
        }

        void ResizeReprotCard()
        {
            //if (CurrentReprotCard != null)
            //{
            //    panelControlReprotCard.BackColor = Color.CornflowerBlue;
            //    if (panelControlReprotCard.Height > CurrentReprotCard.Height)
            //    {
            //        CurrentReprotCard.Location =
            //            new Point((panelControlReprotCard.Width - CurrentReprotCard.Width) / 2, (panelControlReprotCard.Height - CurrentReprotCard.Height) / 2);
            //    }
            //    else
            //    {
            //        CurrentReprotCard.Location =
            //            new Point((panelControlReprotCard.Width - CurrentReprotCard.Width) / 2, 0);
            //    }
            //}
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkButtonSave_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkButtonSave.Checked)
                {
                    Search();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkButtonSumbit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkButtonSumbit.Checked)
                {
                    Search();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkButtonAudit_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkButtonAudit.Checked)
                {
                    Search();
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
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkButtonReject_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.checkButtonReject.Checked)
                {
                    Search();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void panelControlReprotCard_Paint(object sender, PaintEventArgs e)
        {
            ResizeReprotCard();
        }

        private void barLargeButtonItemBingZhong_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DiseaseSetForm diseaseSetForm = new DiseaseSetForm(m_App, m_SqlHelper);
            diseaseSetForm.StartPosition = FormStartPosition.CenterScreen;
            diseaseSetForm.ShowDialog();
            CurrentReprotCard.InitBzlb();
        }

        PrintDocument m_PrintDocument = new PrintDocument();
        /// <summary>
        /// 打印事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                {
                    m_App.CustomMessageBox.MessageShow("请选择一条传染病上报记录", CustomMessageBoxKind.InformationOk);
                    return;
                }
                //宜昌中心医院打印
                string configVal = DS_SqlService.GetConfigValueByKey("YiChangChuangRangBin");
                if (configVal == "1" || string.IsNullOrEmpty(configVal))
                {
                    #region 暂时作废
                    //m_ZymosisReportEntity = new ZymosisReportEntity();
                    //if (CurrentReprotCard.m_ZymosisReportEntity == null)
                    //{
                    //    m_ZymosisReportEntity.ReportId = 0;
                    //    m_ZymosisReportEntity.ReportNo = "";
                    //    m_ZymosisReportEntity.ReportType = "";
                    //    m_ZymosisReportEntity.Noofinpat = "";

                    //    m_ZymosisReportEntity.Patid = "";
                    //    m_ZymosisReportEntity.Name = "";
                    //    m_ZymosisReportEntity.Parentname = "";
                    //    m_ZymosisReportEntity.Idno = "";
                    //    m_ZymosisReportEntity.Sex = "";

                    //    m_ZymosisReportEntity.Birth = "1900-01-01";
                    //    m_ZymosisReportEntity.Age = "";
                    //    m_ZymosisReportEntity.AgeUnit = "";
                    //    m_ZymosisReportEntity.Organization = "";
                    //    m_ZymosisReportEntity.Officeplace = "";

                    //    m_ZymosisReportEntity.Officetel = "";
                    //    m_ZymosisReportEntity.Addresstype = "";
                    //    m_ZymosisReportEntity.Hometown = "";
                    //    m_ZymosisReportEntity.Address = "";
                    //    m_ZymosisReportEntity.Jobid = "";

                    //    m_ZymosisReportEntity.Recordtype1 = "";
                    //    m_ZymosisReportEntity.Recordtype2 = "";
                    //    m_ZymosisReportEntity.Attackdate = "";
                    //    m_ZymosisReportEntity.Diagdate = "";
                    //    m_ZymosisReportEntity.Diedate = "";

                    //    m_ZymosisReportEntity.Diagicd10 = "";
                    //    m_ZymosisReportEntity.Diagname = "";
                    //    m_ZymosisReportEntity.InfectotherFlag = "";
                    //    m_ZymosisReportEntity.Memo = "";
                    //    m_ZymosisReportEntity.CorrectFlag = "";

                    //    m_ZymosisReportEntity.CorrectName = "";
                    //    m_ZymosisReportEntity.CancelReason = "";
                    //    m_ZymosisReportEntity.Reportdeptcode = "";
                    //    m_ZymosisReportEntity.Reportdeptname = "";
                    //    m_ZymosisReportEntity.Reportdoccode = "";

                    //    m_ZymosisReportEntity.Reportdocname = "";
                    //    m_ZymosisReportEntity.Doctortel = "";
                    //    m_ZymosisReportEntity.ReportDate = "";
                    //    m_ZymosisReportEntity.State = "";
                    //    m_ZymosisReportEntity.CreateDate = "";

                    //    m_ZymosisReportEntity.CreateUsercode = "";
                    //    m_ZymosisReportEntity.CreateUsername = "";
                    //    m_ZymosisReportEntity.CreateDeptcode = "";
                    //    m_ZymosisReportEntity.CreateDeptname = "";
                    //    m_ZymosisReportEntity.ModifyDate = "";

                    //    m_ZymosisReportEntity.ModifyUsercode = "";
                    //    m_ZymosisReportEntity.ModifyUsername = "";
                    //    m_ZymosisReportEntity.ModifyDeptcode = "";
                    //    m_ZymosisReportEntity.ModifyDeptname = "";
                    //    m_ZymosisReportEntity.AuditDate = "";

                    //    m_ZymosisReportEntity.AuditUsercode = "";
                    //    m_ZymosisReportEntity.AuditUsername = "";
                    //    m_ZymosisReportEntity.AuditDeptcode = "";
                    //    m_ZymosisReportEntity.AuditDeptname = "";
                    //    m_ZymosisReportEntity.OtherDiag = "";
                    //}
                    //else
                    //{
                    //    m_ZymosisReportEntity = CurrentReprotCard.m_ZymosisReportEntity;
                    //}
                    //Drawingform drawingform = GetDrawingform(m_ZymosisReportEntity);
                    //PrintFroms printForm = new PrintFroms(drawingform);
                    //printForm.ShowDialog();
                    #endregion

                    if (xtraTabPageCardInfo.Controls.Count > 0 && xtraTabPageCardInfo.Controls[0] is UCReportCard)//if (panelControl2.Controls.Count > 0 && panelControl2.Controls[0] is UCReportCard)
                    {
                        PageSetupDialog pageSetupDialog = new PageSetupDialog();
                        pageSetupDialog.Document = m_PrintDocument;
                        PaperSize p = new PaperSize("16K", 275, 457);//默认16K的纸
                        foreach (PaperSize ps in pageSetupDialog.Document.PrinterSettings.PaperSizes)
                        {
                            if (ps.PaperName.Equals("A4"))//这里设置纸张大小,但必须是定义好的  
                                p = ps;
                        }
                        pageSetupDialog.Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                        m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);
                        m_PrintDocument.Print();
                    }
                }
                else
                {
                    if (CurrentReprotCard.m_ZymosisReportEntity == null || CurrentReprotCard.m_ZymosisReportEntity.ReportId <= 0)
                    {
                        MyMessageBox.Show("请先保存数据");
                        return;
                    }
                    PrintFormExt printFormExt = new PrintFormExt(CurrentReprotCard.m_ZymosisReportEntity.ReportId.ToString(), CurrentReprotCard.fukatype);
                    printFormExt.Show();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            UCReportCard reportCard = xtraTabPageCardInfo.Controls[0] as UCReportCard;//panelControl2.Controls[0] as UCReportCard;
            reportCard.PrintCard(e.Graphics, null);
        }

        /// <summary>
        /// 上报时间改变事件，时间结合选择的上报状态进行查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateDate_EditValueChanged(object sender, EventArgs e)
        {
            Search();
        }
        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //if (gridViewCardList.FocusedRowHandle < 0)
            //{
            //    m_App.CustomMessageBox.MessageShow("未选择单据！");
            //    return;
            //}
            //DataRow foucesRow = gridViewCardList.GetDataRow(gridViewCardList.FocusedRowHandle);
            //if (foucesRow == null)
            //    return;

            //if (foucesRow.IsNull("report_id"))//编号
            //    return;
            //string stateid = foucesRow["state"].ToString();//得到选择单据的状态字段

            //ZymosisReportEntity myZymosis = new ZymosisReportEntity();
            //myZymosis = GetEntityUI();
            ////？？？？现在流程。审核否决操作就是对信息的修改操作
            //if (stateid.Equals("4"))//审核通过的
            //{

            //}
            //else if (stateid.Equals("5"))//否决的
            //{

            //}
            //else if (stateid.Equals("1") || stateid.Equals("2"))//保存/提交的
            //{

            //}
        }

        /// <summary>
        /// 根据用户控件的所有项的值设置获取报告卡实体
        /// </summary>
        /// <param name="_ZymosisReportEntity"></param>
        /// <returns></returns>
        private ZymosisReportEntity GetEntityUI()
        {
            ZymosisReportEntity _ZymosisReportEntity = new ZymosisReportEntity();
            UCReportCard myCard = xtraTabPageCardInfo.Controls[0] as UCReportCard;//panelControl2.Controls[0] as UCReportCard; edit by cyq 2012-10-30
            #region new code
            #region 取到用户控件中更改后的值赋给实体

            //报告卡类型 1、初次报告  2、订正报告
            CheckEdit cheReporTtype1 = myCard.Controls["chkReportType1"] as CheckEdit;
            CheckEdit cheReporTtype2 = myCard.Controls["chkReportType2"] as CheckEdit;
            if (cheReporTtype1.Checked)
            {
                _ZymosisReportEntity.ReportType = "1";
            }
            else if (cheReporTtype2.Checked)
            {
                _ZymosisReportEntity.ReportType = "2";
            }

            //患者姓名
            TextEdit txtName = myCard.Controls["txtName"] as TextEdit;
            string txtname = txtName.EditValue.ToString();
            _ZymosisReportEntity.Name = txtname;

            //家长姓名
            TextEdit txtParentName = myCard.Controls["txtParentname"] as TextEdit;
            string txtparentname = txtParentName.EditValue.ToString();
            _ZymosisReportEntity.Parentname = txtparentname;

            //身份证号码
            TextEdit txtIdNo = myCard.Controls["txtIdno"] as TextEdit;
            string txtidno = txtIdNo.EditValue.ToString();
            _ZymosisReportEntity.Idno = txtidno;

            //患者性别
            CheckEdit chksex1 = myCard.Controls["chkSex1"] as CheckEdit;
            CheckEdit chksex2 = myCard.Controls["chkSex2"] as CheckEdit;
            if (chksex1.Checked)
            {
                _ZymosisReportEntity.Sex = "1";
            }
            else if (chksex2.Checked)
            {
                _ZymosisReportEntity.Sex = "2";
            }

            //出生日期
            DateEdit dateBrith = myCard.Controls["dateBirth"] as DateEdit;
            if (!(dateBrith.DateTime.CompareTo(DateTime.MinValue) == 0))
            {
                _ZymosisReportEntity.Birth = dateBrith.DateTime.ToString("yyyy-MM-dd");
            }

            //实足年龄
            TextEdit txtAge = myCard.Controls["txtAge"] as TextEdit;
            string txtage = txtAge.EditValue.ToString();
            _ZymosisReportEntity.Age = txtage;

            //实足年龄单位
            CheckEdit chkAgeUnit1 = myCard.Controls["chkAgeUnit1"] as CheckEdit;
            CheckEdit chkAgeUnit2 = myCard.Controls["chkAgeUnit2"] as CheckEdit;
            CheckEdit chkAgeUnit3 = myCard.Controls["chkAgeUnit3"] as CheckEdit;
            if (chkAgeUnit1.Checked)
                _ZymosisReportEntity.AgeUnit = "1";
            else if (chkAgeUnit2.Checked)
                _ZymosisReportEntity.AgeUnit = "2";
            else if (chkAgeUnit3.Checked)
                _ZymosisReportEntity.AgeUnit = "3";

            //工作单位
            TextEdit txtOrganization = myCard.Controls["txtOrganization"] as TextEdit;
            string txtorganization = txtOrganization.EditValue.ToString();
            _ZymosisReportEntity.Organization = txtorganization;

            //单位电话
            TextEdit txtOfficetel = myCard.Controls["txtOfficetel"] as TextEdit;
            string txtofficetel = txtOfficetel.EditValue.ToString();
            _ZymosisReportEntity.Officetel = txtofficetel;

            //病人属于地区
            CheckEdit chkAddresstype1 = myCard.Controls["chkAddresstype1"] as CheckEdit;
            CheckEdit chkAddresstype2 = myCard.Controls["chkAddresstype2"] as CheckEdit;
            CheckEdit chkAddresstype3 = myCard.Controls["chkAddresstype3"] as CheckEdit;
            CheckEdit chkAddresstype4 = myCard.Controls["chkAddresstype4"] as CheckEdit;
            CheckEdit chkAddresstype5 = myCard.Controls["chkAddresstype5"] as CheckEdit;
            CheckEdit chkAddresstype6 = myCard.Controls["chkAddresstype6"] as CheckEdit;
            if (chkAddresstype1.Checked)
                _ZymosisReportEntity.Addresstype = "1";
            else if (chkAddresstype2.Checked)
                _ZymosisReportEntity.Addresstype = "2";
            else if (chkAddresstype3.Checked)
                _ZymosisReportEntity.Addresstype = "3";
            else if (chkAddresstype4.Checked)
                _ZymosisReportEntity.Addresstype = "4";
            else if (chkAddresstype5.Checked)
                _ZymosisReportEntity.Addresstype = "5";
            else if (chkAddresstype6.Checked)
                _ZymosisReportEntity.Addresstype = "6";

            //详细地址 【村，街道，门牌号】
            TextEdit txtaddress = myCard.Controls["txtAddress"] as TextEdit;
            string address = txtaddress.EditValue.ToString();
            _ZymosisReportEntity.Address = address;

            //职业代码
            CheckEdit chkJobid1 = myCard.Controls["chkJobid1"] as CheckEdit;
            CheckEdit chkJobid2 = myCard.Controls["chkJobid2"] as CheckEdit;
            CheckEdit chkJobid3 = myCard.Controls["chkJobid3"] as CheckEdit;
            CheckEdit chkJobid4 = myCard.Controls["chkJobid4"] as CheckEdit;
            CheckEdit chkJobid5 = myCard.Controls["chkJobid5"] as CheckEdit;
            CheckEdit chkJobid6 = myCard.Controls["chkJobid6"] as CheckEdit;
            CheckEdit chkJobid7 = myCard.Controls["chkJobid7"] as CheckEdit;
            CheckEdit chkJobid8 = myCard.Controls["chkJobid8"] as CheckEdit;
            CheckEdit chkJobid9 = myCard.Controls["chkJobid9"] as CheckEdit;
            CheckEdit chkJobid10 = myCard.Controls["chkJobid10"] as CheckEdit;
            CheckEdit chkJobid11 = myCard.Controls["chkJobid11"] as CheckEdit;
            CheckEdit chkJobid12 = myCard.Controls["chkJobid12"] as CheckEdit;
            CheckEdit chkJobid13 = myCard.Controls["chkJobid13"] as CheckEdit;
            CheckEdit chkJobid14 = myCard.Controls["chkJobid14"] as CheckEdit;
            CheckEdit chkJobid15 = myCard.Controls["chkJobid15"] as CheckEdit;
            CheckEdit chkJobid16 = myCard.Controls["chkJobid16"] as CheckEdit;
            CheckEdit chkJobid17 = myCard.Controls["chkJobid17"] as CheckEdit;
            CheckEdit chkJobid18 = myCard.Controls["chkJobid18"] as CheckEdit;
            if (chkJobid1.Checked)
                _ZymosisReportEntity.Jobid = "1";
            else if (chkJobid2.Checked)
                _ZymosisReportEntity.Jobid = "2";
            else if (chkJobid3.Checked)
                _ZymosisReportEntity.Jobid = "3";
            else if (chkJobid4.Checked)
                _ZymosisReportEntity.Jobid = "4";
            else if (chkJobid5.Checked)
                _ZymosisReportEntity.Jobid = "5";
            else if (chkJobid6.Checked)
                _ZymosisReportEntity.Jobid = "6";
            else if (chkJobid7.Checked)
                _ZymosisReportEntity.Jobid = "7";
            else if (chkJobid8.Checked)
                _ZymosisReportEntity.Jobid = "8";
            else if (chkJobid9.Checked)
                _ZymosisReportEntity.Jobid = "9";
            else if (chkJobid10.Checked)
                _ZymosisReportEntity.Jobid = "10";
            else if (chkJobid11.Checked)
                _ZymosisReportEntity.Jobid = "11";
            else if (chkJobid12.Checked)
                _ZymosisReportEntity.Jobid = "12";
            else if (chkJobid13.Checked)
                _ZymosisReportEntity.Jobid = "13";
            else if (chkJobid14.Checked)
                _ZymosisReportEntity.Jobid = "14";
            else if (chkJobid15.Checked)
                _ZymosisReportEntity.Jobid = "15";
            else if (chkJobid16.Checked)
                _ZymosisReportEntity.Jobid = "16";
            else if (chkJobid17.Checked)
                _ZymosisReportEntity.Jobid = "17";
            else if (chkJobid18.Checked)
                _ZymosisReportEntity.Jobid = "18";

            //病历分类 1、疑似病历	2、临床诊断病历	3、实验室确诊病历	
            CheckEdit chkRecordtype11 = myCard.Controls["chkRecordtype11"] as CheckEdit;
            CheckEdit chkRecordtype12 = myCard.Controls["chkRecordtype12"] as CheckEdit;
            CheckEdit chkRecordtype13 = myCard.Controls["chkRecordtype13"] as CheckEdit;
            if (chkRecordtype11.Checked)
                _ZymosisReportEntity.Recordtype1 = "1";
            else if (chkRecordtype12.Checked)
                _ZymosisReportEntity.Recordtype1 = "2";
            else if (chkRecordtype13.Checked)
                _ZymosisReportEntity.Recordtype1 = "3";

            //病历分类（乙型肝炎、血吸虫病填写）	1、急性	2、慢性
            CheckEdit chkRecordtype21 = myCard.Controls["chkRecordtype21"] as CheckEdit;
            CheckEdit chkRecordtype22 = myCard.Controls["chkRecordtype22"] as CheckEdit;
            CheckEdit chkRecordtype23 = myCard.Controls["chkRecordtype23"] as CheckEdit;
            if (chkRecordtype21.Checked)
                _ZymosisReportEntity.Recordtype2 = "1";
            else if (chkRecordtype22.Checked)
                _ZymosisReportEntity.Recordtype2 = "2";
            else if (chkRecordtype23.Checked)
                _ZymosisReportEntity.Recordtype2 = "3";

            //发病日期（病原携带者填初检日期或就诊日期）
            DateEdit dateAttackdate = myCard.Controls["dateAttackdate"] as DateEdit;
            if (!(dateAttackdate.DateTime.CompareTo(DateTime.MinValue) == 0))
                _ZymosisReportEntity.Attackdate = dateAttackdate.DateTime.ToString("yyyy-MM-dd");

            //诊断日期
            DateEdit dateDiagdate = myCard.Controls["dateDiagdate"] as DateEdit;
            TimeEdit timeDiagdate = myCard.Controls["timeDiagdate"] as TimeEdit;
            if (!(dateDiagdate.DateTime.CompareTo(DateTime.MinValue) == 0))
                _ZymosisReportEntity.Diagdate = dateDiagdate.DateTime.ToString("yyyy-MM-dd") + " " + timeDiagdate.Time.ToString("HH:mm:ss");


            //死亡日期
            DateEdit Diedate = myCard.Controls["Diedate"] as DateEdit;
            if (!(Diedate.DateTime.CompareTo(DateTime.MinValue) == 0))
                _ZymosisReportEntity.Diedate = Diedate.DateTime.ToString("yyyy-MM-dd");

            //传染病病种(对应传染病诊断库)
            LookUpEditor lookUpEditorZymosis = myCard.Controls["lookUpEditorZymosis"] as LookUpEditor;
            _ZymosisReportEntity.Diagicd10 = lookUpEditorZymosis.CodeValue;

            //传染病病种名称
            _ZymosisReportEntity.Diagname = lookUpEditorZymosis.DisplayValue;

            //订正病名
            TextEdit textCorrectName = myCard.Controls["textCorrectName"] as TextEdit;
            string textcorrectName = textCorrectName.EditValue.ToString();
            _ZymosisReportEntity.CorrectName = textcorrectName;

            //退卡原因
            TextEdit txtCancelReason = myCard.Controls["txtCancelReason"] as TextEdit;
            string txtcancelReason = txtCancelReason.EditValue.ToString();
            _ZymosisReportEntity.CancelReason = txtcancelReason;

            //报告科室编号
            LookUpEditor lookUpEditorDept = myCard.Controls["lookUpEditorDept"] as LookUpEditor;
            _ZymosisReportEntity.Reportdeptcode = lookUpEditorDept.CodeValue;

            //报告科室名称
            _ZymosisReportEntity.Reportdeptname = lookUpEditorDept.DisplayValue;
            //_ZymosisReportEntity.Reportdeptname = lookUpEditorDept.SelectedText;

            //报告医生联系电话
            TextEdit txtDoctortel = myCard.Controls["txtDoctortel"] as TextEdit;
            string txtdoctortel = txtDoctortel.EditValue.ToString();
            _ZymosisReportEntity.Doctortel = txtdoctortel;

            //报告医生编号
            LookUpEditor lookUpEditorDoc = myCard.Controls["lookUpEditorDoc"] as LookUpEditor;
            _ZymosisReportEntity.Reportdoccode = lookUpEditorDoc.CodeValue;

            //报告医生姓名
            _ZymosisReportEntity.Reportdocname = lookUpEditorDoc.DisplayValue;

            //填卡时间
            DateEdit dateReportDate = myCard.Controls["dateReportDate"] as DateEdit;
            if (!(dateReportDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                _ZymosisReportEntity.ReportDate = dateReportDate.DateTime.ToString("yyyy-MM-dd");

            //备注
            MemoEdit memoMemo = myCard.Controls["memoMemo"] as MemoEdit;
            _ZymosisReportEntity.Memo = memoMemo.Text;

            //其他法定管理以及重点监测传染病：
            MemoEdit memoOtherDiag = myCard.Controls["memoOtherDiag"] as MemoEdit;
            _ZymosisReportEntity.OtherDiag = memoOtherDiag.Text;

            //修改时间
            _ZymosisReportEntity.ModifyDate = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");

            //修改人科室编号
            _ZymosisReportEntity.ModifyDeptcode = m_App.User.CurrentDeptId;

            //修改人科室名称
            _ZymosisReportEntity.ModifyDeptname = m_App.User.CurrentDeptName;

            //修改人编号
            _ZymosisReportEntity.ModifyUsercode = m_App.User.Id;

            //修改人姓名
            _ZymosisReportEntity.ModifyUsername = m_App.User.Name;

            #endregion
            #endregion


            return _ZymosisReportEntity;


        }
        //补录
        private void barLargeButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateCard();
        }

        private void CreateCard()
        {
            PatientListForNew patientList = new PatientListForNew(m_App, true);
            patientList.StartPosition = FormStartPosition.CenterScreen;
            if (patientList.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string noofinpat = patientList.GetSelectedRow();
                if (!string.IsNullOrEmpty(noofinpat))
                {
                    CurrentReprotCard.LoadPage(noofinpat, "2", "1");
                    barLargeButtonItemSave.Enabled = true;
                    barLargeButtonItemSubmit.Enabled = true;
                    barLargeButtonItemDelete.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 保存事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// 1、add try ... catch
        /// 2、添加提示信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemSav_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                {
                    MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return;
                }

                //保存
                CurrentReprotCard.Save();

                //刷新实体数据
                m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                SetButtonEnable();

                //相应操作后进行数据的刷新
                Search();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 提交事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemTiJiao_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat))
                {
                    MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return;
                }
                //提交
                if (CurrentReprotCard.Submit())
                {
                    m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                    SetButtonEnable();
                    RefreshGridControl(ReportState.Submit, CurrentReprotCard.ReportID);
                    CurrentReprotCard.ClearPage();
                    Search();//相应操作后进行数据的刷新
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-30</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            //HIVForm HIVForm = new HIVForm(m_ZymosisReportEntity.ReportId.ToString());
            //HIVForm.ShowDialog();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            PrintFormExt printFormExt = new PrintFormExt("", "");
            printFormExt.Show();
        }
    }
}