using DevExpress.Utils;
using DevExpress.XtraGrid;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    /// <summary>
    /// 申请页面
    /// </summary>
    public partial class MainForm : DevBaseForm, IStartPlugIn
    {
        /// <summary>
        /// 病历内容窗体
        /// </summary>
        UCEmrInput m_UCEmrInput;
        IEmrHost m_App;
        SqlHelper m_SqlHelper;
        SearchDataEntity m_SearchDataEntity;
        string m_Noofinpat;
        Applicant m_UserRole;
        UCReportCard CurrentReprotCard
        {
            get
            {
                if (panelControlReprotCard.Controls.Count == 1)
                {
                    UCReportCard card = panelControlReprotCard.Controls[0] as UCReportCard;
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
        public MainForm(string noofinpat)
        {
            InitializeComponent();
            m_Noofinpat = noofinpat;
            InitControlState();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            InitControlState();
        }
        #endregion

        public void InitControlState()
        {
            lookUpEditDept.Properties.ReadOnly = true;
            checkEditOwner.Checked = true;
            checkButtonSave.Checked = true;
        }

        private void AddUCReportCard()
        {
            UCReportCard card = new UCReportCard(m_App);
            panelControlReprotCard.Controls.Add(card);
            card.Dock = DockStyle.Fill;
            ResizeReprotCard();
        }

        private void AddEmrInput()
        {
            m_UCEmrInput = new UCEmrInput();
            m_UCEmrInput.HideBar();
            RecordDal m_RecordDal = new RecordDal(m_App.SqlHelper);
            m_UCEmrInput.SetInnerVar(m_App, m_RecordDal);
            xtraTabControlCardInfo.Controls.Add(m_UCEmrInput);
            m_UCEmrInput.Dock = DockStyle.Fill;
        }

        private void LoadEmrContent()
        {
            DataRowView drv = gridViewCardList.GetRow(gridViewCardList.FocusedRowHandle) as DataRowView;
            if (drv != null)
            {
                //m_App.ChoosePatient(Convert.ToDecimal(drv["noofinpat"]));
                //m_UCEmrInput.PatientChanged();
                m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(drv["noofinpat"]));
            }
        }

        #region Load
        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                m_UserRole = new Applicant(m_SqlHelper, m_App);
                SetGridControlConsultationColor();
                InitDeptList();
                AddUCReportCard();
                InitReprotCard();
                if (string.IsNullOrEmpty(m_Noofinpat))
                {
                    Search();
                }
                this.ActiveControl = this.textEditName;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitReprotCard()
        {
            if (string.IsNullOrEmpty(m_Noofinpat))
            {
                CurrentReprotCard.LoadPage(m_Noofinpat, "2", "1");
            }
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
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CreateCard();
        }

        private void CreateCard()
        {
            PatientListForNew patientList = new PatientListForNew(m_App);
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

        private void barLargeButtonItemReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //报表
            ReportForms _ReportForms = new ReportForms(m_App);
            _ReportForms.ShowDialog();
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
        private void barLargeButtonItemSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetWaitDialogCaption("正在查询，请稍等...");
            Search();
            HideWaitDialog();
        }

        private void Search()
        {
            m_SearchDataEntity.FirstReport = checkEditFirst.Checked ? "1" : ""; //1、初次报告  
            m_SearchDataEntity.ModifiedReport = checkEditModify.Checked ? "2" : ""; //2、订正报告
            m_SearchDataEntity.PatientName = textEditName.Text.Trim();
            m_SearchDataEntity.PatID = textEditPatID.Text.Trim();
            m_SearchDataEntity.DeptID = lookUpEditDept.EditValue.ToString();
            m_SearchDataEntity.Owner = checkEditOwner.Checked ? m_App.User.DoctorId : "";
            int status = -1;
            if (checkButtonSave.Checked)
            {
                status = (int)ZymosisReportEnum.Save;
            }
            else if (checkButtonSubmit.Checked)
            {
                status = (int)ZymosisReportEnum.Submit;
            }
            //else if (checkButtonWithDraw.Checked)
            //{
            //    status = (int)ZymosisReportEnum.WithDraw;
            //}
            else if (checkButtonAudit.Checked)
            {
                status = (int)ZymosisReportEnum.Pass;
            }
            else if (checkButtonReject.Checked)
            {
                status = (int)ZymosisReportEnum.Reject;
            }
            m_SearchDataEntity.Status = status == -1 ? "" : status.ToString();
            string querytype = "2";//不受时间限制(区分传染病审批和传染病上报两个列表的查询条件)
            DataTable dt = m_SqlHelper.GetCardList(m_SearchDataEntity, querytype);
            gridControlCardList.DataSource = dt;
        }
        #endregion
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //保存
            if (!string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat) && CurrentReprotCard.Save())
            {
                m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                SetButtonEnable();
            }
            Search();
        }
        /// <summary>
        /// 提交操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemSubmit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //提交
            if (!string.IsNullOrEmpty(CurrentReprotCard.m_Noofinpat) && CurrentReprotCard.Submit())
            {
                m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                SetButtonEnable();
                RefreshGridControl(ReportState.Submit, CurrentReprotCard.ReportID);
                CurrentReprotCard.ClearPage();
            }
            Search();//相应操作后进行数据的刷新
        }

        private void barLargeButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //删除
            if (CurrentReprotCard.Cancel())
            {
                m_UserRole.RefreshValue(CurrentReprotCard.ReportID);
                SetButtonEnable();
                RefreshGridControl(ReportState.Cancel, CurrentReprotCard.ReportID);
                CurrentReprotCard.ClearPage();
            }
            Search();//相应操作后进行数据的刷新
        }

        private void RefreshGridControl(ReportState rs, string reportID)
        {
            //删除
            if (
                (rs == ReportState.Cancel && checkButtonSave.Checked)
                ||
                (rs == ReportState.Submit && (checkButtonSave.Checked || checkButtonReject.Checked))
               )
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
                    CurrentReprotCard.LoadPage(reportID, "1", "1");
                }
            }
            #region 选择左侧列表控制右侧报告卡的编辑权限
            //add  by ywk 2012年3月28日10:57:08
            DataRow foucesRow = gridViewCardList.GetDataRow(gridViewCardList.FocusedRowHandle);
            if (foucesRow == null)
                return;
            string stateid = foucesRow["state"].ToString();//当前选中的报告单状态
            if (!stateid.Equals("1"))//除了保存状态，其他的状态都禁掉编辑操作
            {
                CurrentReprotCard.EnableState(false);
                #region 注释 by cyq 2012-10-24
                ////现在控制到右侧的传染病上报卡的文本框可编辑性
                //UCReportCard myCard = panelControlReprotCard.Controls[0] as UCReportCard;
                ////报告卡编号
                //TextEdit txtReportNo = myCard.Controls["txtReportNo"] as TextEdit;
                //txtReportNo.Properties.ReadOnly = true;
                ////报告卡类型 1、初次报告  2、订正报告
                //CheckEdit cheReporTtype1 = myCard.Controls["chkReportType1"] as CheckEdit;
                //CheckEdit cheReporTtype2 = myCard.Controls["chkReportType2"] as CheckEdit;
                //cheReporTtype1.Properties.ReadOnly = true;
                //cheReporTtype2.Properties.ReadOnly = true;
                ////患者姓名
                //TextEdit txtName = myCard.Controls["txtName"] as TextEdit;
                //txtName.Properties.ReadOnly = true;
                ////家长姓名
                //TextEdit txtParentName = myCard.Controls["txtParentname"] as TextEdit;
                //txtParentName.Properties.ReadOnly = true;
                ////身份证号码
                //TextEdit txtIdNo = myCard.Controls["txtIdno"] as TextEdit;
                //txtIdNo.Properties.ReadOnly = true;
                ////患者性别
                //CheckEdit chksex1 = myCard.Controls["chkSex1"] as CheckEdit;
                //CheckEdit chksex2 = myCard.Controls["chkSex2"] as CheckEdit;
                //chksex1.Properties.ReadOnly = true;
                //chksex2.Properties.ReadOnly = true;
                ////出生日期
                //DateEdit dateBrith = myCard.Controls["dateBirth"] as DateEdit;
                //dateBrith.Properties.ReadOnly = true;
                ////实足年龄
                //TextEdit txtAge = myCard.Controls["txtAge"] as TextEdit;
                //txtAge.Properties.ReadOnly = true;
                ////实足年龄单位
                //CheckEdit chkAgeUnit1 = myCard.Controls["chkAgeUnit1"] as CheckEdit;
                //CheckEdit chkAgeUnit2 = myCard.Controls["chkAgeUnit2"] as CheckEdit;
                //CheckEdit chkAgeUnit3 = myCard.Controls["chkAgeUnit3"] as CheckEdit;
                //chkAgeUnit1.Properties.ReadOnly = true;
                //chkAgeUnit2.Properties.ReadOnly = true;
                //chkAgeUnit3.Properties.ReadOnly = true;
                ////工作单位
                //TextEdit txtOrganization = myCard.Controls["txtOrganization"] as TextEdit;
                //txtOrganization.Properties.ReadOnly = true;
                ////单位电话
                //TextEdit txtOfficetel = myCard.Controls["txtOfficetel"] as TextEdit;
                //txtOfficetel.Properties.ReadOnly = true;
                ////病人属于地区
                //CheckEdit chkAddresstype1 = myCard.Controls["chkAddresstype1"] as CheckEdit;
                //CheckEdit chkAddresstype2 = myCard.Controls["chkAddresstype2"] as CheckEdit;
                //CheckEdit chkAddresstype3 = myCard.Controls["chkAddresstype3"] as CheckEdit;
                //CheckEdit chkAddresstype4 = myCard.Controls["chkAddresstype4"] as CheckEdit;
                //CheckEdit chkAddresstype5 = myCard.Controls["chkAddresstype5"] as CheckEdit;
                //CheckEdit chkAddresstype6 = myCard.Controls["chkAddresstype6"] as CheckEdit;
                //chkAddresstype1.Properties.ReadOnly = true;
                //chkAddresstype2.Properties.ReadOnly = true;
                //chkAddresstype3.Properties.ReadOnly = true;
                //chkAddresstype4.Properties.ReadOnly = true;
                //chkAddresstype5.Properties.ReadOnly = true;
                //chkAddresstype6.Properties.ReadOnly = true;
                ////详细地址 【村，街道，门牌号】
                //TextEdit txtaddress = myCard.Controls["txtAddress"] as TextEdit;
                //txtaddress.Properties.ReadOnly = true;
                ////职业代码
                //CheckEdit chkJobid1 = myCard.Controls["chkJobid1"] as CheckEdit;
                //CheckEdit chkJobid2 = myCard.Controls["chkJobid2"] as CheckEdit;
                //CheckEdit chkJobid3 = myCard.Controls["chkJobid3"] as CheckEdit;
                //CheckEdit chkJobid4 = myCard.Controls["chkJobid4"] as CheckEdit;
                //CheckEdit chkJobid5 = myCard.Controls["chkJobid5"] as CheckEdit;
                //CheckEdit chkJobid6 = myCard.Controls["chkJobid6"] as CheckEdit;
                //CheckEdit chkJobid7 = myCard.Controls["chkJobid7"] as CheckEdit;
                //CheckEdit chkJobid8 = myCard.Controls["chkJobid8"] as CheckEdit;
                //CheckEdit chkJobid9 = myCard.Controls["chkJobid9"] as CheckEdit;
                //CheckEdit chkJobid10 = myCard.Controls["chkJobid10"] as CheckEdit;
                //CheckEdit chkJobid11 = myCard.Controls["chkJobid11"] as CheckEdit;
                //CheckEdit chkJobid12 = myCard.Controls["chkJobid12"] as CheckEdit;
                //CheckEdit chkJobid13 = myCard.Controls["chkJobid13"] as CheckEdit;
                //CheckEdit chkJobid14 = myCard.Controls["chkJobid14"] as CheckEdit;
                //CheckEdit chkJobid15 = myCard.Controls["chkJobid15"] as CheckEdit;
                //CheckEdit chkJobid16 = myCard.Controls["chkJobid16"] as CheckEdit;
                //CheckEdit chkJobid17 = myCard.Controls["chkJobid17"] as CheckEdit;
                //CheckEdit chkJobid18 = myCard.Controls["chkJobid18"] as CheckEdit;
                //chkJobid1.Properties.ReadOnly = true;
                //chkJobid2.Properties.ReadOnly = true;
                //chkJobid3.Properties.ReadOnly = true;
                //chkJobid4.Properties.ReadOnly = true;
                //chkJobid5.Properties.ReadOnly = true;
                //chkJobid6.Properties.ReadOnly = true;
                //chkJobid7.Properties.ReadOnly = true;
                //chkJobid8.Properties.ReadOnly = true;
                //chkJobid9.Properties.ReadOnly = true;
                //chkJobid10.Properties.ReadOnly = true;
                //chkJobid11.Properties.ReadOnly = true;
                //chkJobid12.Properties.ReadOnly = true;
                //chkJobid13.Properties.ReadOnly = true;
                //chkJobid14.Properties.ReadOnly = true;
                //chkJobid15.Properties.ReadOnly = true;
                //chkJobid16.Properties.ReadOnly = true;
                //chkJobid17.Properties.ReadOnly = true;
                //chkJobid18.Properties.ReadOnly = true;
                ////病历分类 1、疑似病历	2、临床诊断病历	3、实验室确诊病历
                //CheckEdit chkRecordtype11 = myCard.Controls["chkRecordtype11"] as CheckEdit;
                //CheckEdit chkRecordtype12 = myCard.Controls["chkRecordtype12"] as CheckEdit;
                //CheckEdit chkRecordtype13 = myCard.Controls["chkRecordtype13"] as CheckEdit;
                //chkRecordtype11.Properties.ReadOnly = true;
                //chkRecordtype12.Properties.ReadOnly = true;
                //chkRecordtype13.Properties.ReadOnly = true;
                ////病历分类（乙型肝炎、血吸虫病填写）	1、急性	2、慢性 3、未分类
                //CheckEdit chkRecordtype21 = myCard.Controls["chkRecordtype21"] as CheckEdit;
                //CheckEdit chkRecordtype22 = myCard.Controls["chkRecordtype22"] as CheckEdit;
                //CheckEdit chkRecordtype23 = myCard.Controls["chkRecordtype23"] as CheckEdit;
                //chkRecordtype21.Properties.ReadOnly = true;
                //chkRecordtype22.Properties.ReadOnly = true;
                //chkRecordtype23.Properties.ReadOnly = true;
                ////发病日期（病原携带者填初检日期或就诊日期）
                //DateEdit dateAttackdate = myCard.Controls["dateAttackdate"] as DateEdit;
                //dateAttackdate.Properties.ReadOnly = true;
                ////诊断日期
                //DateEdit dateDiagdate = myCard.Controls["dateDiagdate"] as DateEdit;
                //TimeEdit timeDiagdate = myCard.Controls["timeDiagdate"] as TimeEdit;
                //dateDiagdate.Properties.ReadOnly = true;
                //timeDiagdate.Properties.ReadOnly = true;
                ////死亡日期
                //DateEdit Diedate = myCard.Controls["Diedate"] as DateEdit;
                //Diedate.Properties.ReadOnly = true;
                ////传染病病种(对应传染病诊断库)
                //LookUpEditor lookUpEditorZymosis = myCard.Controls["lookUpEditorZymosis"] as LookUpEditor;
                //lookUpEditorZymosis.ReadOnly = true;
                ////订正病名
                //TextEdit textCorrectName = myCard.Controls["textCorrectName"] as TextEdit;
                //textCorrectName.Properties.ReadOnly = true;
                ////退卡原因
                //TextEdit txtCancelReason = myCard.Controls["txtCancelReason"] as TextEdit;
                //txtCancelReason.Properties.ReadOnly = true;
                ////报告科室编号
                //LookUpEditor lookUpEditorDept = myCard.Controls["lookUpEditorDept"] as LookUpEditor;
                //lookUpEditorDept.ReadOnly = true;
                ////报告医生联系电话
                //TextEdit txtDoctortel = myCard.Controls["txtDoctortel"] as TextEdit;
                //txtDoctortel.Properties.ReadOnly = true;

                ////报告医生编号
                //LookUpEditor lookUpEditorDoc = myCard.Controls["lookUpEditorDoc"] as LookUpEditor;
                //lookUpEditorDoc.ReadOnly = true;
                ////填卡时间
                //DateEdit dateReportDate = myCard.Controls["dateReportDate"] as DateEdit;
                //dateReportDate.Properties.ReadOnly = true;

                ////备注
                //MemoEdit memoMemo = myCard.Controls["memoMemo"] as MemoEdit;
                ////memoMemo.Enabled = false;
                //memoMemo.Properties.ReadOnly = true;

                ////其他法定管理以及重点监测传染病：
                //MemoEdit memoOtherDiag = myCard.Controls["memoOtherDiag"] as MemoEdit;
                ////memoOtherDiag.Enabled = false;
                //memoOtherDiag.Properties.ReadOnly = true;
                #endregion
            }
            #endregion
        }

        void SetButtonEnable()
        {
            //原来的控制到（保存，删除，提交按钮的可编辑性）
            barLargeButtonItemSave.Enabled = m_UserRole.CanSave;
            barLargeButtonItemSubmit.Enabled = m_UserRole.CanSubmit;
            barLargeButtonItemDelete.Enabled = m_UserRole.CanDelete;
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

        private void panelControlReprotCard_Resize(object sender, EventArgs e)
        {
            ResizeReprotCard();
        }

        void ResizeReprotCard()
        {
            //if (CurrentReprotCard != null)
            //{
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

        private void checkButtonSave_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkButtonSave.Checked)
            {
                Search();
            }
        }

        private void checkButtonSubmit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkButtonSubmit.Checked)
            {
                Search();
            }
        }

        private void checkButtonAudit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkButtonAudit.Checked)
            {
                Search();
            }
        }

        private void checkButtonWithDraw_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.checkButtonWithDraw.Checked)
            //{
            //    Search();
            //}
        }

        private void checkButtonReject_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkButtonReject.Checked)
            {
                Search();
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCardList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
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

    }

    #region Entity
    /// <summary>
    ///  查询界面数据实体
    /// </summary>
    public class SearchDataEntity
    {
        /// <summary>
        /// 初次报告
        /// </summary>
        public string FirstReport { get; set; }
        /// <summary>
        /// 订正报告
        /// </summary>
        public string ModifiedReport { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName { get; set; }
        /// <summary>
        /// 病历号
        /// </summary>
        public string PatID { get; set; }
        /// <summary>
        /// 部门码
        /// </summary>
        public string DeptID { get; set; }
        /// <summary>
        /// 表单申请人
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 上报日期开始时间
        /// </summary>
        public string CreateDateStart { get; set; }
        /// <summary>
        /// 上报日期结束时间
        /// </summary>
        public string CreateDateEnd { get; set; }
    }
    #endregion
}