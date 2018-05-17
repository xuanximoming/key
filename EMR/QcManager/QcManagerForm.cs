using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Windows.Forms;

namespace DrectSoft.Emr.QcManager
{
    public partial class QcManagerForm : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_app;
        InWardPatRecInfo m_WardInfo;
        OutHospitalNoSubmit m_OutHospital;
        OutHospitalNoLock m_OutHospitalLock;
        QualityMedicalRecord m_QualityMedicalRecord;

        OutMedicalScore m_OutMedicalScore;
        QCDieInfo m_QC_Die_Info;//死亡信息统计
        UcOperatInfo m_QC_Operat_Info;//手术信息统计
        UCRecord m_Consult;      //会诊明细
        UcRescueInfo m_QC_Rescue_Info;//抢救信息统计
        QC_LostScoreCat m_QC_LostScoreCat;
        QC_ScoreRecord m_QC_ScoreRecord;
        //UcQcHospitalRate m_QcHospitalRate;//全院病历质控率
        UcDeptRate m_UcDeptRate;//科室病历质控率
        UcDeptQuery m_UcDeptQuery;//科室医疗质量统计 
        UcQcDoctorQuery m_UcQcDoctorQuery;//医师医疗质量统计
        UcSingleDisease m_UcSingleDisease;//单病种医疗质量分析
        QC_DiagOperRecord m_UcDiagOper;//按手术诊断查看
        QC_RecordByDoctor m_UcByDoctor;//按书写查看
        QCPatDiaDetail m_Ucpatdiadetail;
        //UserLoginInfo m_UserLoginInfo;//用户登录日志审计
        //ActiveUser m_ActiveUser;//活跃用户审计
        //DiagGroupInfo m_DiagGroupInfo;
        //DataBaseTable m_DataBaseTable;

        public QcManagerForm()
        {
            InitializeComponent();
        }

        private void LoadWardInfo()
        {
            try
            {
                if (m_WardInfo == null)
                    m_WardInfo = new InWardPatRecInfo(m_app);
                m_WardInfo.Dock = DockStyle.Fill;
                xtraTabPage1.Controls.Add(m_WardInfo);
                m_WardInfo.RefreshData();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定出院未提交信息
        /// </summary>
        private void LoadOutHospital()
        {
            try
            {
                if (m_OutHospital == null)
                {
                    m_OutHospital = new OutHospitalNoSubmit(m_app);
                }
                m_OutHospital.Dock = DockStyle.Fill;
                xtraTabPageOutHospital.Controls.Add(m_OutHospital);
                xtraTabPageOutHospital.PageVisible = true;
                m_OutHospital.RefreshData();

                xtraTabQcManager.SelectedTabPage = xtraTabPageOutHospital;
                xtraTabQcManager.TabPages.Add(xtraTabPageOutHospital);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定出院未归档病历
        /// </summary>
        private void LoadOutHospitalLock()
        {
            try
            {
                if (m_OutHospitalLock == null)
                {
                    m_OutHospitalLock = new OutHospitalNoLock(m_app);
                }
                m_OutHospitalLock.Dock = DockStyle.Fill;
                xtraTabPageOutHospitalLock.Controls.Add(m_OutHospitalLock);
                xtraTabPageOutHospitalLock.PageVisible = true;
                m_OutHospitalLock.RefreshData();

                xtraTabQcManager.SelectedTabPage = xtraTabPageOutHospitalLock;
                //关闭Tab页再重新选中上级的按钮，此处要重新加上TAb add by ywk
                xtraTabQcManager.TabPages.Add(xtraTabPageOutHospitalLock);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 会诊明细
        /// </summary>
        private void LoadConsultation()
        {
            try
            {
                if (m_Consult == null)
                {
                    m_Consult = new UCRecord(m_app);
                }
                m_Consult.Dock = DockStyle.Fill;

                xtraTabPageConsult.Controls.Add(m_Consult);
                xtraTabPageConsult.PageVisible = true;

                xtraTabQcManager.SelectedTabPage = xtraTabPageConsult;
                //关闭Tab页再重新选中上级的按钮，此处要重新加上TAb add by wyt
                xtraTabQcManager.TabPages.Add(xtraTabPageConsult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定时限质控
        /// 
        /// </summary>
        private void LoadQualityMedicalRecord()
        {
            try
            {
                xTabPageEmrScore.PageVisible = false;

                if (m_QualityMedicalRecord == null)
                {
                    m_QualityMedicalRecord = new QualityMedicalRecord(m_app);
                }
                m_QualityMedicalRecord.Dock = DockStyle.Fill;
                xtraTabPageQualityMedicalRecord.Controls.Add(m_QualityMedicalRecord);
                xtraTabPageQualityMedicalRecord.PageVisible = true;

                xtraTabQcManager.SelectedTabPage = xtraTabPageQualityMedicalRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region IStartPlugIn 成员

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }

        #endregion

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QcManagerForm_Load(object sender, EventArgs e)
        {
            try
            {
                //LoadWardInfo();
                LoadQualityMedicalRecord();
                //treeView1.Nodes.RemoveByKey("NodOutMedicalScore");
                TreeNode[] tt = treeView1.Nodes.Find("NodDept_Rate", true);
                tt[1].Remove();
                tt[0].Remove();
                tt = treeView1.Nodes.Find("NodDocQuery", true);
                tt[0].Remove();
                tt = treeView1.Nodes.Find("NodSingleDisease", true);
                tt[0].Remove();
                tt = treeView1.Nodes.Find("NodOutMedicalScore", true);
                tt[0].Remove();
                tt = treeView1.Nodes.Find("NodQCRescueInfo", true);
                tt[0].Remove();
                tt = treeView1.Nodes.Find("NodPerson", true);
                tt[0].Remove();
                //tt = treeView1.Nodes.Find("NodPointConfig", true);
                //tt[0].Remove();
                //tt = treeView1.Nodes.Find("MainpageQC", true);
                //tt[0].Remove();
                //tt = treeView1.Nodes.Find("节点15", true);
                //tt[0].Remove();
                //tt = treeView1.Nodes.Find("Consulting", true);   // 删掉两个节点报表//仁和 
                //tt[0].Remove();
                //tt = treeView1.Nodes.Find("NodOutHosNoSubmit", true);
                //tt[0].Remove();
                //NodDept_Rate
                //this.treeView1.Nodes[1].Nodes[2].Remove();
                //this.ActiveControl = m_QualityMedicalRecord;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 出院未提交患者 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadOutHospital();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 出院未归档病历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadOutHospitalLock();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 会诊明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemConsult_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadConsultation();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 标准评分大项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QCScoreType qcscoretype = new QCScoreType(m_app);
                qcscoretype.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 标准评分细项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QCScoreItem qcscoreitem = new QCScoreItem(m_app);
                qcscoreitem.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 医师医疗质量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_qc_doctor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_Doctor_Query m_QC_Doctor_Query = new QC_Doctor_Query(m_app);
                m_QC_Doctor_Query.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 科室医疗质量统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_qc_dept_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_Dept_Query m_QC_Dept_Query = new QC_Dept_Query(m_app);
                m_QC_Dept_Query.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 全院病历质控率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_hospital_info_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_Hospital_Rate m_QC_Hospital_Rate = new QC_Hospital_Rate(m_app);
                m_QC_Hospital_Rate.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 科室病历质控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_dept_rate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_Dept_Record_Rate m_QC_Dept_Record_Rate = new QC_Dept_Record_Rate(m_app);
                m_QC_Dept_Record_Rate.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 死亡统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_Die_Info m_QC_Die_Info = new QC_Die_Info(m_app);
                m_QC_Die_Info.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 抢救统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_Rescue_Info m_QC_Rescue_Info = new QC_Rescue_Info(m_app);
                m_QC_Rescue_Info.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 手术统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_Operat_Info m_QC_Operat_Info = new QC_Operat_Info(m_app);
                m_QC_Operat_Info.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 单病种医疗质量统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_qc_SingleDisease_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_SingleDisease_Query m_QC_SingleDisease_Query = new QC_SingleDisease_Query(m_app);
                m_QC_SingleDisease_Query.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 自动监控设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_monitor_item_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_monitor_item m_QC_monitor_item = new QC_monitor_item(m_app);
                m_QC_monitor_item.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 单病种设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Disease_Level_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                QC_Disease_Level m_QC_Disease_Level = new QC_Disease_Level(m_app);
                m_QC_Disease_Level.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (e.Page.Name == "xtraTabPage1")
                {
                    LoadWardInfo();
                }
                //add by wyt 2012-11-02
                //切换tab页时自动获取焦点
                if (xtraTabQcManager.SelectedTabPage.Controls.Count > 0)
                {
                    xtraTabQcManager.SelectedTabPage.Controls[0].Focus();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增的病历评分报表页面
        /// add by ywk 2012年4月5日10:45:07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnPointSum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadMedicalSorce();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病历评分页面
        /// </summary>
        private void LoadMedicalSorce()
        {
            try
            {
                if (m_OutMedicalScore == null)
                    m_OutMedicalScore = new OutMedicalScore(m_app);

                m_OutMedicalScore.Dock = DockStyle.Fill;
                xTabPageEmrScore.Controls.Add(m_OutMedicalScore);
                xTabPageEmrScore.PageVisible = true;
                //m_OutMedicalScore.RefreshData();

                xtraTabQcManager.SelectedTabPage = xTabPageEmrScore;

                xtraTabQcManager.TabPages.Add(xTabPageEmrScore);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 科室质控人员配置
        /// add by ywk 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnConfigQCAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ConfigQCAudit configaudit = new ConfigQCAudit(m_app);
                configaudit.StartPosition = FormStartPosition.CenterScreen;
                configaudit.ShowDialog();
                configaudit.Dispose();

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// TAb页关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (xtraTabQcManager.TabPages.Count == 1)//如果关闭到只剩下一个Tab页
                {
                    return;
                }
                else
                {
                    //if (this.xtraTabQcManager.SelectedTabPage == xtraTabPageOutHospital)//出院未提交
                    //{
                    //    m_OutHospital = new OutHospitalNoSubmit(m_app);
                    //    m_OutHospital.ResetControl();
                    //}
                    xtraTabQcManager.TabPages.Remove(this.xtraTabQcManager.SelectedTabPage);

                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击菜单进行处理，加载用户控件
        /// add by ywk  2012年10月22日 17:46:40
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //treeView1.Nodes.
                if (treeView1.SelectedNode == null) return;
                string nodename = treeView1.SelectedNode.Name.ToString();//节点的Name
                //NodOutHosNoSubmit
                switch (nodename)
                {
                    case "NodOutHosNoSubmit"://出院未提交患者
                        if (m_OutHospital == null)
                            m_OutHospital = new OutHospitalNoSubmit(m_app);
                        m_OutHospital.ResetControl();
                        m_OutHospital.Dock = DockStyle.Fill;
                        xtraTabPageOutHospital.Controls.Add(m_OutHospital);
                        xtraTabPageOutHospital.PageVisible = true;
                        m_OutHospital.RefreshData();

                        xtraTabQcManager.SelectedTabPage = xtraTabPageOutHospital;
                        xtraTabQcManager.TabPages.Add(xtraTabPageOutHospital);
                        break;
                    case "NoOutHosNoLock"://出院未归档病历
                        if (m_OutHospitalLock == null)
                            m_OutHospitalLock = new OutHospitalNoLock(m_app);
                        m_OutHospitalLock.ResetControl();
                        m_OutHospitalLock.Dock = DockStyle.Fill;
                        xtraTabPageOutHospitalLock.Controls.Add(m_OutHospitalLock);
                        xtraTabPageOutHospitalLock.PageVisible = true;
                        m_OutHospitalLock.RefreshData();

                        xtraTabQcManager.SelectedTabPage = xtraTabPageOutHospitalLock;
                        //关闭Tab页再重新选中上级的按钮，此处要重新加上Tab add by ywk
                        xtraTabQcManager.TabPages.Add(xtraTabPageOutHospitalLock);
                        break;
                    case "NodOutMedicalScore"://病历评分统计
                        if (m_OutMedicalScore == null)
                            m_OutMedicalScore = new OutMedicalScore(m_app);
                        m_OutMedicalScore.ResetControl();
                        m_OutMedicalScore.Dock = DockStyle.Fill;
                        xTabPageEmrScore.Controls.Add(m_OutMedicalScore);
                        xTabPageEmrScore.PageVisible = true;
                        //m_OutMedicalScore.RefreshData();
                        xtraTabQcManager.SelectedTabPage = xTabPageEmrScore;
                        xtraTabQcManager.TabPages.Add(xTabPageEmrScore);
                        break;
                    case "NoQC_Die_Info"://死亡信息统计
                        if (m_QC_Die_Info == null)
                        {
                            m_QC_Die_Info = new QCDieInfo(m_app);
                        }
                        m_QC_Die_Info.ResetControl();
                        m_QC_Die_Info.Dock = DockStyle.Fill;
                        xtraTabDieInfo.Controls.Add(m_QC_Die_Info);
                        xtraTabDieInfo.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabDieInfo;
                        xtraTabQcManager.TabPages.Add(xtraTabDieInfo);
                        break;
                    case "NoQC_Operat_Info"://手术信息统计
                        if (m_QC_Operat_Info == null)
                        {
                            m_QC_Operat_Info = new UcOperatInfo(m_app);
                        }
                        m_QC_Operat_Info.ResetControl();
                        m_QC_Operat_Info.Dock = DockStyle.Fill;
                        xtraTabQCOperatInfo.Controls.Add(m_QC_Operat_Info);
                        xtraTabQCOperatInfo.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabQCOperatInfo;
                        xtraTabQcManager.TabPages.Add(xtraTabQCOperatInfo);
                        break;
                    case "NodQCRescueInfo"://抢救信息统计
                        if (m_QC_Rescue_Info == null)
                        {
                            m_QC_Rescue_Info = new UcRescueInfo(m_app);
                        }
                        m_QC_Rescue_Info.Dock = DockStyle.Fill;
                        xtraTabQCRescueInfo.Controls.Add(m_QC_Rescue_Info);
                        xtraTabQCRescueInfo.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabQCRescueInfo;
                        xtraTabQcManager.TabPages.Add(xtraTabQCRescueInfo);
                        break;
                    case "NodeLostCat"://失分项目统计
                        if (m_QC_LostScoreCat == null)
                        {
                            m_QC_LostScoreCat = new QC_LostScoreCat(m_app);
                        }
                        m_QC_LostScoreCat.Dock = DockStyle.Fill;
                        xtraTabPageLostCat.Controls.Add(m_QC_LostScoreCat);
                        xtraTabPageLostCat.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabPageLostCat;
                        xtraTabQcManager.TabPages.Add(xtraTabPageLostCat);
                        break;
                    case "Nodscore"://质控评分表
                        if (m_QC_ScoreRecord == null)
                        {
                            m_QC_ScoreRecord = new QC_ScoreRecord(m_app);
                        }
                        m_QC_ScoreRecord.Dock = DockStyle.Fill;
                        xtraTabPageScore.Controls.Add(m_QC_ScoreRecord);
                        xtraTabPageScore.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabPageScore;
                        xtraTabQcManager.TabPages.Add(xtraTabPageScore);
                        break;
                    #region 去掉全院质控率，与科室质控率重复 edit by wyt 2012-11-08
                    //case "NodHospital_Rate"://全院病历质控率
                    //    if (m_QcHospitalRate == null)
                    //    {
                    //        m_QcHospitalRate = new UcQcHospitalRate(m_app);
                    //    }
                    //    m_QcHospitalRate.Dock = DockStyle.Fill;
                    //    xtraTabHosRate.Controls.Add(m_QcHospitalRate);
                    //    xtraTabHosRate.PageVisible = true;
                    //    xtraTabQcManager.SelectedTabPage = xtraTabHosRate;
                    //    xtraTabQcManager.TabPages.Add(xtraTabHosRate);
                    //    break;
                    #endregion
                    case "NodDept_Rate"://科室病历质控率//替代全院质控率 edit by wyt 2012-11-08
                        if (m_UcDeptRate == null)
                        {
                            m_UcDeptRate = new UcDeptRate(m_app);
                        }
                        m_UcDeptRate.Dock = DockStyle.Fill;
                        xtraTabDepRate.Controls.Add(m_UcDeptRate);
                        xtraTabDepRate.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabDepRate;
                        xtraTabQcManager.TabPages.Add(xtraTabDepRate);
                        break;
                    case "NodeConsultInfo"://会诊信息明细查询
                        if (m_Consult == null)
                            m_Consult = new UCRecord(m_app);
                        m_Consult.Dock = DockStyle.Fill;
                        m_Consult.ResetControl();
                        xtraTabPageConsult.Controls.Add(m_Consult);
                        xtraTabPageConsult.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabPageConsult;
                        xtraTabQcManager.TabPages.Add(xtraTabPageConsult);
                        break;
                    case "NodDocQuery"://医师医疗质量统计
                        if (m_UcQcDoctorQuery == null)
                        {
                            m_UcQcDoctorQuery = new UcQcDoctorQuery(m_app);
                        }
                        m_UcQcDoctorQuery.Dock = DockStyle.Fill;

                        xtraTabDocQuery.Controls.Add(m_UcQcDoctorQuery);
                        xtraTabDocQuery.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabDocQuery;
                        xtraTabQcManager.TabPages.Add(xtraTabDocQuery);
                        break;
                    case "NodDeptQuery"://科室医疗质量统计
                        if (m_UcDeptQuery == null)
                        {
                            m_UcDeptQuery = new UcDeptQuery(m_app);
                        }
                        m_UcDeptQuery.Dock = DockStyle.Fill;

                        xtraTabDeptQuery.Controls.Add(m_UcDeptQuery);
                        xtraTabDeptQuery.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabDeptQuery;
                        xtraTabQcManager.TabPages.Add(xtraTabDeptQuery);
                        break;
                    case "NodSingleDisease"://单病种医疗质量分析
                        if (m_UcSingleDisease == null)
                        {
                            m_UcSingleDisease = new UcSingleDisease(m_app);
                        }
                        m_UcSingleDisease.Dock = DockStyle.Fill;

                        xtraTabSingleDisease.Controls.Add(m_UcSingleDisease);
                        xtraTabSingleDisease.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabSingleDisease;
                        xtraTabQcManager.TabPages.Add(xtraTabSingleDisease);
                        break;
                    case "NodPerson"://质控人员配置
                        ConfigQCAudit configaudit = new ConfigQCAudit(m_app);
                        configaudit.StartPosition = FormStartPosition.CenterScreen;
                        configaudit.ShowDialog();
                        break;
                    case "NodPointConfig"://评分项配置
                        ConfigPoint m_configPoint = new ConfigPoint(m_app);
                        m_configPoint.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
                        m_configPoint.ShowDialog();
                        break;
                    case "MainpageQC"://病案首页评分项配置
                        QCIemMainpageConfig mainpageqc = new QCIemMainpageConfig(m_app);
                        mainpageqc.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
                        mainpageqc.ShowDialog();
                        break;
                    case "RecordByDoctor"://医师书写病历统计
                        if (m_UcByDoctor == null)
                        {
                            m_UcByDoctor = new QC_RecordByDoctor(m_app);
                        }
                        m_UcByDoctor.ResetControl();
                        m_UcByDoctor.Dock = DockStyle.Fill;
                        xtraTabRecorByDoctor.Controls.Add(m_UcByDoctor);
                        xtraTabRecorByDoctor.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabRecorByDoctor;
                        xtraTabQcManager.TabPages.Add(xtraTabRecorByDoctor);
                        break;
                    case "DiagOperRecord"://诊断手术病案统计
                        if (m_UcDiagOper == null)
                        {
                            m_UcDiagOper = new QC_DiagOperRecord(m_app);
                        }
                        m_UcDiagOper.ResetControl();
                        m_UcDiagOper.Dock = DockStyle.Fill;
                        xtraTabDiagOperRecord.Controls.Add(m_UcDiagOper);
                        xtraTabDiagOperRecord.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabDiagOperRecord;
                        xtraTabQcManager.TabPages.Add(xtraTabDiagOperRecord);
                        break;
                    case "Consulting"://会诊明细
                        if (m_Consult == null)
                        {
                            m_Consult = new UCRecord(m_app);
                        }
                        m_Consult.ResetControl();
                        m_Consult.Dock = DockStyle.Fill;
                        xtraTabPageConsult.Controls.Add(m_Consult);
                        xtraTabPageConsult.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabPageConsult;
                        xtraTabQcManager.TabPages.Add(xtraTabPageConsult);
                        break;
                    case "Nodpatdiadetail"://新增 入院诊断  明细查询
                        if (m_Ucpatdiadetail == null)
                        {
                            m_Ucpatdiadetail = new QCPatDiaDetail(m_app);
                        }
                        m_Ucpatdiadetail.ResetControl();
                        m_Ucpatdiadetail.Dock = DockStyle.Fill;
                        xtraTabPageDia.Controls.Add(m_Ucpatdiadetail);
                        xtraTabPageDia.PageVisible = true;
                        xtraTabQcManager.SelectedTabPage = xtraTabPageDia;
                        xtraTabQcManager.TabPages.Add(xtraTabPageDia);
                        break;

                    //case "UserLogin"://登录日志审计
                    //    if (m_UserLoginInfo==null)
                    //    {
                    //        m_UserLoginInfo = new UserLoginInfo();
                    //    }
                    //    m_UserLoginInfo.Dock = DockStyle.Fill;
                    //    xtabUserLoginInfo.Controls.Add(m_UserLoginInfo);
                    //    xtabUserLoginInfo.PageVisible = true;
                    //    xtraTabQcManager.SelectedTabPage = xtabUserLoginInfo;
                    //    xtraTabQcManager.TabPages.Add(xtabUserLoginInfo);
                    //    break;
                    //case "ActiveUser"://活跃用户审计
                    //    if (m_ActiveUser == null)
                    //    {
                    //        m_ActiveUser = new ActiveUser();
                    //    }
                    //    m_ActiveUser.Dock = DockStyle.Fill;
                    //    xtabActiveUser.Controls.Add(m_ActiveUser);
                    //    xtabActiveUser.PageVisible = true;
                    //    xtraTabQcManager.SelectedTabPage = xtabActiveUser;
                    //    xtraTabQcManager.TabPages.Add(xtabActiveUser);
                    //    break;
                    //case "DiagGroupInfo":
                    //    if (m_DiagGroupInfo == null)
                    //    {
                    //        m_DiagGroupInfo = new DiagGroupInfo();
                    //    }
                    //    m_DiagGroupInfo.Dock = DockStyle.Fill;
                    //    xtabDiagGroupInfo.Controls.Add(m_DiagGroupInfo);
                    //    xtabDiagGroupInfo.PageVisible = true;
                    //    xtraTabQcManager.SelectedTabPage = xtabDiagGroupInfo;
                    //    xtraTabQcManager.TabPages.Add(xtabDiagGroupInfo);
                    //    break;

                    //case "DataBaseTable":
                    //    if (m_DataBaseTable == null)
                    //    {
                    //        m_DataBaseTable = new DataBaseTable();
                    //    }
                    //    m_DataBaseTable.Dock = DockStyle.Fill;
                    //    xtabDataBaseTable.Controls.Add(m_DataBaseTable);
                    //    xtabDataBaseTable.PageVisible = true;
                    //    xtraTabQcManager.SelectedTabPage = xtabDataBaseTable;
                    //    xtraTabQcManager.TabPages.Add(xtabDataBaseTable);
                    //    break;
                    default:
                        break; ;
                }
            }
            //MessageBox.Show(treeView1.SelectedNode.Name);

            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void xtraTabPageOutHospital_ControlRemoved(object sender, ControlEventArgs e)
        {

        }
    }
}
