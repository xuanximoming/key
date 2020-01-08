using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Core.IEMMainPage;
using DrectSoft.Core.MainEmrPad.HelpForm;
using DrectSoft.Core.MainEmrPad.HistoryEMR;
using DrectSoft.Core.MainEmrPad.ThreeLevelCheck;
using DrectSoft.Core.PersonTtemTemplet;
using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Common;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad.New
{
    public partial class UCEmrInput : DevExpress.XtraEditors.XtraUserControl
    {
        #region Field && Property
        private IEmrHost m_app;
        public IEmrHost App
        {
            get { return m_app; }
            set { m_app = value; }
        }

        public FloderState floaderState = FloderState.Default; //左侧菜单图片文件夹状态
        public TreeListNode focusedNode;//获取焦点的菜单树节点
        private string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("DiagIsJudge");  // add by jxh 
        public string CheckMessage;//用于存放出科检查验证结果的变量 add by ywk 二〇一三年七月十一日 11:56:47 

        /// <summary>
        /// 当前用户
        /// </summary>
        private Employee CurrentEmployee
        {
            get
            {
                if ((_currentEmployee == null) || (!_currentEmployee.Code.Equals(DS_Common.currentUser.Id)))
                {
                    _currentEmployee = new Employee(DS_Common.currentUser.Id);
                    _currentEmployee.ReInitializeProperties();
                }
                return _currentEmployee;
            }
        }
        private Employee _currentEmployee;

        /// <summary>
        /// 当前病人
        /// </summary>
        private Inpatient m_CurrentInpatient;
        public Inpatient CurrentInpatient
        {
            get { return m_CurrentInpatient; }
            set { m_CurrentInpatient = value; }
        }

        /// <summary>
        /// 界面中调用的等待窗体
        /// </summary>
        private WaitDialogForm m_WaitDialog;

        public UCEmrInputBody CurrentInputBody
        {
            get
            {
                try
                {
                    if (panelControlBody.Controls.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return panelControlBody.Controls[0] as UCEmrInputBody;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public EditorForm CurrentForm
        {
            get
            {
                return CurrentInputBody.CurrentForm;
            }
        }

        private PersonItemManager m_PersonItem;///科室小模板

        /// <summary>
        /// 编辑器是否可以编辑的开关
        /// </summary>
        public bool m_EditorEnableFlag = true;

        #region 按钮公开 - 方便UCEmrInputBody控件调用
        ///删除按钮
        public BarButtonItem btnItemDelete
        {
            get
            {
                return this.btnItem_Delete;
            }
        }
        ///提交按钮
        public BarButtonItem btnItemSubmit
        {
            get
            {
                return this.btnItem_Submit;
            }
        }
        ///审核按钮
        public BarButtonItem btnItemAudit
        {
            get
            {
                return this.btnItem_Audit;
            }
        }
        ///取消审核按钮
        public BarButtonItem btnItemCancelAudit
        {
            get
            {
                return this.btnItem_CancelAudit;
            }
        }
        #endregion
        #endregion

        #region .ctor
        public UCEmrInput()
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在创建病历编辑组件...", "请稍候");
                InitializeComponent();
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public UCEmrInput(Inpatient inpatient, IEmrHost app)
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在创建病历编辑组件...", "请稍候");
                InitializeComponent();
                m_CurrentInpatient = inpatient;
                m_app = app;
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public UCEmrInput(Inpatient inpatient, IEmrHost app, FloderState floadState)
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("正在创建病历编辑组件...", "请稍候");
                InitializeComponent();
                m_CurrentInpatient = inpatient;
                m_app = app;
                m_app.FloderState = floadState.ToString();
                floaderState = floadState;
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCEmrInput_Load(object sender, EventArgs e)
        {
            try
            {
                OnLoad();
                //未完成状态的病历允许编辑
                string lockState = getReocrdDetailState(m_app.CurrentPatientInfo.NoOfFirstPage.ToString()).Trim();

                if (!(lockState == "4700" || lockState == ""))
                {
                    this.HideBar();
                }

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 获取当前病人病历状态
        /// </summary>
        /// <param name="noofinpat"></param>
        string getReocrdDetailState(string noofinpat)
        {
            if (noofinpat.Trim() != "")
            {
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable("select * from inpatient a where a.noofinpat=" + noofinpat);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["islock"].ToString();
                }
                else
                {

                    return "";
                }
            }
            else
            {
                return "";
            }

        }

        /// <summary>
        /// 窗体加载方法
        /// </summary>
        public void OnLoad()
        {
            try
            {
                this.th = null;
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病人基本信息...");

                //消除界面中Dev自带右键菜单
                repositoryItemComboBoxFont.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                repositoryItemComboBox_Size.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                repositoryItemComboBox1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                ///设置bar2按钮不可用
                CanEditEmrDoc(false);
                ///设置工具栏按钮显示状态
                SetToolBarButtonState(false);
                ///设置暂不启用的按钮显示状态
                NeedHideBtn();
                ///根据医生护士设置工具栏的显示状态
                SetBarAndBtnStateOfUser();
                ///初始化字体
                InitFont();
                ///绑定病人基本信息
                BindPatBasic((int)m_CurrentInpatient.NoOfFirstPage);
                ///添加主体
                AddEmrInputBody();

                ///历史病历
                if (floaderState == FloderState.All || floaderState == FloderState.Default || floaderState == FloderState.Doctor || floaderState == FloderState.NoneAudit)
                {
                    ///历史病历按钮显示状态
                    btnItem_ImportHostoryEmr.Enabled = CurrentInputBody.CanImportHistoryEmr();
                    ///历史病历自动导入
                    this.FindForm().Shown += new EventHandler(UCEmrInput_Shown);
                }
                ///配置按钮可见性(调用PACS图像、出科检查)
                ConfigBtnShowState();

                //重新加入【更改常用词】功能 add by ywk 2013年9月4日 17:20:16
                if (m_app.User.Id == "00")
                {
                    btnChangeWord.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void AddEmrInputBody()
        {
            try
            {
                if (null == CurrentInputBody)
                {
                    UCEmrInputBody body = new UCEmrInputBody(m_CurrentInpatient, m_app, floaderState);
                    body.Dock = DockStyle.Fill;
                    body.WaitDialog = m_WaitDialog;
                    panelControlBody.Controls.Add(body);
                }
                CurrentInputBody.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化用到的变量
        /// </summary>
        private void InitDictionary()
        {
            try
            {
                CurrentInputBody.InitDictionary();
                m_PersonItem = null;
                m_EditorEnableFlag = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 设置病人基本信息

        /// <summary>
        /// 设置病人基本信息
        /// Modify by xlb 2013-05-28
        /// 病历号截取配置项从指定数字开始截取
        /// </summary>
        private void BindPatBasic(int noofinpat)
        {
            try
            {
                DataTable dt = DS_SqlService.GetInpatientByID(noofinpat, 2);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return;
                }
                DataRow patient = dt.Rows[0];
                ///床号
                barStaticItem_bedNo.Caption = "床号：" + patient["outbed"].ToString();
                ///姓名
                barStaticItem_patientName.Caption = "姓名：" + patient["name"].ToString();
                ///病历号
                string config1 = DS_SqlService.GetConfigValueByKey("IsShowBigPatNo");
                int getVal = -1;
                if (!string.IsNullOrEmpty(config1))
                {
                    string[] cfg1 = config1.Split(',');
                    if (null != cfg1 && cfg1.Length > 1)
                    {
                        getVal = Convert.ToInt32(cfg1[1]);
                    }
                }
                if (getVal > 0 && patient["patid"].ToString().Length > getVal + 1)
                {
                    barStaticItem_patID.Caption = "病历号：" + patient["patid"].ToString().Substring(getVal);
                }
                else
                {
                    barStaticItem_patID.Caption = "病历号：" + patient["patid"].ToString();
                }
                ///性别
                barStaticItem_gender.Caption = "性别：" + patient["sexname"].ToString();
                ///年龄
                barStaticItem_age.Caption = "年龄：" + patient["agestr"].ToString();
                ///入院日期
                string inHosDate = string.IsNullOrEmpty(patient["inwarddate"].ToString()) ? patient["admitdate"].ToString() : patient["inwarddate"].ToString();
                string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                if (!string.IsNullOrEmpty(config))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(config);
                    XmlNodeList nodeList = doc.GetElementsByTagName("InHosTimeType");
                    if (null != nodeList && nodeList.Count > 0)
                    {
                        string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                        if (cfgValue == "0")
                        {//入院
                            inHosDate = patient["admitdate"].ToString();
                        }
                        else
                        {//入科
                            inHosDate = patient["inwarddate"].ToString();
                        }
                    }
                }
                barStaticItem_inHosDate.Caption = "入院日期：" + inHosDate;
                ///科室病区
                barStaticItem_currentDept.Caption = "科室病区：" + patient["deptname"].ToString() + "/" + patient["wardname"].ToString();
                ///入院次数
                btnItem_inHosCount.Caption = "入院次数：" + DS_SqlService.GetInHosCountNew(noofinpat);
                btnItem_inHosCount.Appearance.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 切换病人
        public void SetVarData(IEmrHost app)
        {
            try
            {
                m_app = app;
                m_CurrentInpatient = m_app.CurrentPatientInfo;
                ///设置权限状态
                floaderState = string.IsNullOrEmpty(m_app.FloderState) ? FloderState.Default : (FloderState)Enum.Parse(typeof(FloderState), m_app.FloderState);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PatientChanging()
        {
            try
            {
                CloseAllTabPages();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PatientChanged(Inpatient inpatient)
        {
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病人基本信息...");
                m_CurrentInpatient = inpatient;
                m_CurrentInpatient.ReInitializeAllProperties();
                CurrentInputBody.App = m_app;
                ///设置权限状态
                floaderState = string.IsNullOrEmpty(m_app.FloderState) ? FloderState.Default : (FloderState)Enum.Parse(typeof(FloderState), m_app.FloderState);
                CurrentInputBody.CurrentInpatient = m_app.CurrentPatientInfo;
                ///初始化基本变量
                InitDictionary();
                ///设置bar2按钮不可用
                CanEditEmrDoc(false);
                ///设置工具栏按钮显示状态
                SetToolBarButtonState(false);
                ///设置暂不启用的按钮显示状态
                NeedHideBtn();
                ///其它按钮的显示状态
                SetBarAndBtnStateOfUser();
                ///初始化字体
                InitFont();
                ///绑定病人基本信息
                BindPatBasic((int)m_CurrentInpatient.NoOfFirstPage);
                ///历史病历按钮显示状态
                if (floaderState == FloderState.All || floaderState == FloderState.Default || floaderState == FloderState.Doctor || floaderState == FloderState.NoneAudit)
                {
                    ///历史病历按钮显示状态
                    btnItem_ImportHostoryEmr.Enabled = CurrentInputBody.CanImportHistoryEmr();
                    ///历史病历自动导入
                    this.FindForm().Shown += new EventHandler(UCEmrInput_Shown);
                }
                ///配置按钮可见性(调用PACS图像、出科检查)
                ConfigBtnShowState();

                ///初始化主体部分
                CurrentInputBody.CurrentInputTabPages.RemoveAllTabPageUnEvent();
                CurrentInputBody.OnLoad();

                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CloseAllTabPages()
        {
            try
            {
                int pageCount = CurrentInputBody.xtraTabControlEmr.TabPages.Count;
                for (int i = pageCount - 1; i >= 0; i--)
                {
                    XtraTabPage page = CurrentInputBody.xtraTabControlEmr.TabPages[i];
                    CurrentInputBody.xtraTabControlEmr.SelectedTabPage = page;

                    if (CurrentForm != null)
                    {
                        CurrentInputBody.ClickTabPageCloseButton();
                    }
                    else if (page.Controls.Count > 0 && page.Controls[0] is IEMREditor)
                    {
                        //IEMREditor emrEditor = page.Controls[0] as IEMREditor;
                        //emrEditor.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PatientChangedByIEmrHost(decimal noofinpat)
        {
            try
            {
                CloseAllTabPages();
                PatientChanged(new Inpatient(noofinpat));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 设置按钮状态
        /// <summary>
        /// 可以编辑就可以用的BarButton【不包括提交和删除】
        /// </summary>
        /// <param name="b"></param>
        private void CanEditEmrDoc(bool boo)
        {
            try
            {
                //btnItem_New.Enabled = boo;//新增
                btnItem_Edit.Enabled = boo;//编辑
                //btnItem_Delete.Enabled = boo;//删除(审核状态下的病历不能删除)
                //btnItem_Readonly.Enabled = boo;//只读
                btnItem_Save.Enabled = boo;//保存
                btnItem_SaveAll.Enabled = boo;//全部保存
                btnItem_SaveAs.Enabled = boo;//另存
                btnItem_CheckSelf.Enabled = boo;//自检

                barSubItem_Table.Enabled = boo;//表格
                btnItem_Copy.Enabled = boo;//复制
                btnItem_Paste.Enabled = boo;//粘贴
                barItem_Cut.Enabled = boo;//剪切
                btnItem_Undo.Enabled = boo;//撤销
                btnItem_Redo.Enabled = boo;//还原
                barEditItem_Font.Enabled = boo;//字体
                btnItem_FontColor.Enabled = boo;//字体颜色
                btnItem_BackColor.Enabled = boo;//设置背景颜色
                barEditItem_FontSize.Enabled = boo;//字体大小
                btnItem_Bold.Enabled = boo;//加粗
                btnItem_Italy.Enabled = boo;//斜体
                btnItem_UnderLine.Enabled = boo;//下划线
                btnItem_QZ.Enabled = boo;//圈字
                btnItem_Sup.Enabled = boo;//上标
                btnItem_Sub.Enabled = boo;//下标
                btnItem_Collect.Enabled = boo;//提取

                btnItem_ReplaceItem.Enabled = boo;///替换复用元素 add by ywk 二〇一三年六月七日 15:32:08 
                barItem_InsertPic.Enabled = boo;//新增的插入图片 add by ywk 二〇一三年六月七日 15:07:06  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置提交审核按钮的可用状态
        /// </summary>
        /// <param name="boo"></param>
        public void SetAuditButState(bool boo)
        {
            try
            {
                btnItem_Submit.Enabled = boo;//提交
                btnItem_Audit.Enabled = boo;//审核
                btnItem_CancelAudit.Enabled = boo;//取消审核
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置其它按钮的显示状态
        /// </summary>
        /// <param name="boo"></param>
        public void SetBarAndBtnStateOfUser()
        {
            try
            {
                if (null == CurrentEmployee || CurrentEmployee.Kind == EmployeeKind.Nurse || CurrentEmployee.Kind == EmployeeKind.None)
                {
                    btnItem_ThreeLevelCheck.Enabled = false;//三级检诊
                    btnItem_CheckOutDept.Enabled = false;//出科检查
                }
                if (floaderState == FloderState.NoneAudit)
                {
                    SetAuditButState(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 隐藏主界面工具栏
        /// </summary>
        public void HideBar()
        {
            try
            {

                bar1.Visible = false;
                bar2.Visible = false;
                btnItem_Print2.Visibility = BarItemVisibility.Always;
                CurrentInputBody.PanelContainerRight.Visibility = DockVisibility.Hidden;
                CurrentInputBody.RightMouseButtonFlag = false;
                CurrentInputBody.EditorEnableFlag = false;
                CurrentInputBody.xtraTabPageIEMMainPage.AccessibleName = "不能编辑";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 不开放功能，暂时隐藏的按钮
        /// </summary>
        public void NeedHideBtn()
        {
            try
            {
                btnItem_Edit.Visibility = BarItemVisibility.Never;///编辑
                btnItem_Readonly.Visibility = BarItemVisibility.Never;///只读
                btnItem_SaveAll.Visibility = BarItemVisibility.Never;///全部保存
                //btnItem_Print2.Visibility = BarItemVisibility.Never;///打印
                //btnItem_Copy2.Visibility = BarItemVisibility.Never;///复制
                //btnItem_ReplaceItem.Visibility = BarItemVisibility.Never;///替换复用元素 放开此功能 ywk 二〇一三年六月七日 15:31:28 
                barButtonItem_CellInfo.Visibility = BarItemVisibility.Never;///单元格属性
                barButtonItem_SplitCell.Visibility = BarItemVisibility.Never;///拆分单元格
                barButtonItem_CombineCell.Visibility = BarItemVisibility.Never;///合并单元格
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 配置按钮的可见性
        /// 调用PACS图像、出科检查
        /// </summary>
        /// edit by ywk 2013年8月9日 16:16:42 调用病理检查结果按钮
        private void ConfigBtnShowState()
        {
            try
            {
                string cfgvalue = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cfgvalue);
                ///调用PACS图像按钮
                if (doc.GetElementsByTagName("BtnPacsImageVisable")[0].InnerText == "0")
                {
                    btnItem_Pacs.Visibility = BarItemVisibility.Never;
                }
                else
                {
                    btnItem_Pacs.Visibility = BarItemVisibility.Always;
                }
                ///出科检查按钮
                if (doc.GetElementsByTagName("IsShowCheckBtn")[0].InnerText == "0")
                {
                    btnItem_CheckOutDept.Visibility = BarItemVisibility.Never;
                }
                else
                {
                    btnItem_CheckOutDept.Visibility = BarItemVisibility.Always;
                }

                //调用病理检查结果按钮 add by ywk 2013年8月9日 16:18:08
                string isshowpath = DS_SqlService.GetConfigValueByKey("IsShowPathologic");
                if (isshowpath == "1")
                {
                    btnCallPathologic.Visibility = BarItemVisibility.Always;
                }
                else
                {
                    btnCallPathologic.Visibility = BarItemVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 设置按钮可见性及可用性
        /// <summary>
        /// 根据获取焦点的节点设置按钮状态
        /// </summary>
        /// <param name="node"></param>
        public void SetToolBarButtonState(bool boo)
        {
            try
            {
                this.btnItem_New.Enabled = boo;///新增
                this.btnItem_Edit.Enabled = boo;///编辑
                this.btnItem_Delete.Enabled = boo;///删除
                this.btnItem_Readonly.Enabled = boo;///只读
                this.btnItem_Save.Enabled = boo;///保存
                this.btnItem_SaveAs.Enabled = boo;///另存为
                this.btnItem_SaveAll.Enabled = boo;///全部保存
                this.btnItem_CheckSelf.Enabled = boo;///自检
                this.btnItem_Submit.Enabled = boo;///提交
                this.btnItem_Audit.Enabled = boo;///审核
                this.btnItem_CancelAudit.Enabled = boo;///取消审核
                this.barSubItem_Table.Enabled = boo;///表格
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 工具栏按钮事件 - Bar1
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Save_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (null != CurrentInputBody)
                {
                    CurrentInputBody.Save();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 另存为事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_SaveAs_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.SaveAs();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_New_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.AddRecord();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Delete_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.DeleteDocument();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 提交事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Submit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.SubmitDocment();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 审核事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Audit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.AuditDocment();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_CancelAudit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.CancelAudit();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 自检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_CheckSelf_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.CheckSelf();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Print_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (null != CurrentInputBody.CurrentModel)
                {
                    if (CurrentInputBody.CurrentModel.InstanceId != -1)
                    {
                        DataTable dt = DS_SqlService.GetRecordByIDContainsDel(CurrentInputBody.CurrentModel.InstanceId);
                        if (null != dt && dt.Rows.Count == 1 && Convert.ToInt32(dt.Rows[0]["VALID"]) == 0)
                        {
                            string userNameAndID = DS_BaseService.GetUserNameAndID(dt.Rows[0]["owner"].ToString());
                            MyMessageBox.Show("该病人首次病程已经被 " + userNameAndID + " 删除，无法打印。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                            return;
                        }
                        if (CurrentInputBody.CurrentModel.ModelCatalog == "AI" || CurrentInputBody.CurrentModel.ModelCatalog == "AJ" || CurrentInputBody.CurrentModel.ModelCatalog == "AK")
                        {
                        }
                        else
                        {
                            if (dt != null && dt.Rows.Count == 1 && Convert.ToInt32(dt.Rows[0]["hassubmit"]) == 4600)
                            {
                                MyMessageBox.Show("打印前请先提交病历", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                                return;
                            }
                        }
                    }

                    if (CurrentInputBody.CurrentModel.ModelCatalog == "AI" || CurrentInputBody.CurrentModel.ModelCatalog == "AJ" || CurrentInputBody.CurrentModel.ModelCatalog == "AK")
                    {

                    }
                    else
                    {

                        if (!CurrentInputBody.CheckEditorIsSubmit(CurrentInputBody.CurrentModel))
                        {
                            MyMessageBox.Show("打印前请先提交病历", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                            return;
                        }
                    }
                    New.PrintForm printForm = new New.PrintForm(CurrentInputBody.CurrentModel, m_CurrentInpatient, m_app, CurrentInputBody.CurrentModel.DeptChangeID);
                    printForm.ShowDialog();
                }
                else if (null != CurrentInputBody.CurrentEmrEditor)
                {
                    if (valueStr == "0")
                    {
                        //************************
                        string printbasy = DS_SqlService.GetConfigValueByKey("PrintBasy");

                        if (printbasy == "1")  //敦化中医院
                        {

                            MessageBox.Show("请在住院医生站打印病案首页。");
                            return;
                        }
                        //************************************

                        if (CurrentInputBody.CurrentEmrEditor.CurrentNoofinpat != "")          //add  by jxh 打印判断主要诊断问题
                        {
                            string sql = string.Format(@"select d.diagnosis_name from iem_mainpage_diagnosis_sx d join iem_mainpage_basicinfo_sx b on d.iem_mainpage_no=b.iem_mainpage_no  and b.noofinpat={0} and d.diagnosis_type_id in(7,8) and d.valide='1'", CurrentInputBody.CurrentEmrEditor.CurrentNoofinpat);
                            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                CurrentInputBody.CurrentEmrEditor.Print();
                            }
                            else
                            {
                                MessageBox.Show("对不起，主要诊断未填写，不能打印，请先填写主要诊断");
                            }
                        }
                    }
                    else
                    {
                        CurrentInputBody.CurrentEmrEditor.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 三级检诊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_ThreeLevelCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                UserSettingForm userSettingForm = new UserSettingForm(this.m_app);
                userSettingForm.SetDepartmentNonEnableFlag();
                userSettingForm.StartPosition = FormStartPosition.CenterScreen;
                userSettingForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 调用PACS图像
        /// 此方法用于pacs接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Pacs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string valuestr = DS_SqlService.GetConfigValueByKey("PACSRevision");
                PACSOutSide.PacsAll(m_CurrentInpatient);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(3, ex);
            }
        }


        /// <summary>
        /// 中心医院调用病案系统
        /// add by ywk 2013年7月31日 09:00:55
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallMedRecord_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string valuestr = DS_SqlService.GetConfigValueByKey("CallMedRecord");
                string[] value = valuestr.Split(',');
                if (value.Length > 0)
                {
                    if (value[0].ToString() == "0")
                    {
                        MessageBox.Show("对不起，暂未做病案系统接口。");
                    }
                    if (value[0].ToString() == "1")
                    {
                        string temppacsUrl = value[1].ToString();
                        string medrecordUrl = string.Format(temppacsUrl, m_CurrentInpatient.RecordNoOfHospital);
                        System.Diagnostics.Process.Start("IEXPLORE.EXE", medrecordUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("调用病案系统接口错误" + ex.Message);
            }

        }


        /// <summary>
        /// 出科检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_CheckOutDept_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                GOMyCheck();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }




        #region 替换复用元素相关

        ///// <summary>
        ///// 替换复用元素 - 此功能暂时停用
        ///// add by ywk 2013年6月7日 15:34:46
        ///// 此功能最好开启!确保用户有需求：要求病程记录和住院志各项目内容一致
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        private void btnItem_ReplaceItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!CheckIsDoctor())
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("此功能只对医生开放。");
                    return;
                }
                ReplaceItemForAllEmrRecord();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病历树的所有节点
        /// </summary>
        private List<TreeListNode> m_TreeAllNode = new List<TreeListNode>();

        /// <summary>
        /// 执行替换复用元素功能
        /// add  by ywk 2013年6月7日 15:36:23
        /// </summary>
        private void ReplaceItemForAllEmrRecord()
        {
            if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定重新替换所有复用元素吗？", "替换复用元素", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RefreshEMRMainPad();

                DataTable dt = DS_SqlService.GetAllRecordsByNoofinpat(Int32.Parse(m_CurrentInpatient.NoOfFirstPage.ToString()));
                m_TreeAllNode = new List<TreeListNode>();
                GetTreeAllNodeForReplaceItem(CurrentInputBody.CurrentTreeList.Nodes);
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (TreeListNode node in m_TreeAllNode)
                    {
                        EmrModel model = node.Tag as EmrModel;
                        if (model != null)
                        {
                            if (dr["id"].ToString() == model.InstanceId.ToString())
                            {
                                CurrentInputBody.CurrentTreeList.FocusedNode = node;
                                //ReplaceModelMacro(model);
                                CurrentInputBody.CurrentForm.ReplaceModelMacro(model.ModelName);
                            }
                        }
                    }
                }
                ////病历内容已经改变，询问是否保存？
                //foreach (TreeListNode node in m_TreeAllNode)
                //{
                //    if (CurrentInputBody.CheckEditorIsSaved(node.Tag as EmrModel))
                //    {
                //        if (Common.Ctrs.DLG.MessageBox.Show((node.Tag as EmrModel).ModelName + " 已经修改，您是否保存？", "提示信息", Common.Ctrs.DLG.MessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Yes)
                //        {
                //            //CurrentInputBody.Save(node.Tag as EmrModel);
                //            CurrentInputBody.CurrentEmrEditor.Save();
                //        }

                //    }
                //}
                //RefreshEMRMainPad();
            }

        }

        /// <summary>
        /// 得到病历树中的节点用于替换复用元素
        /// add by ywk 二〇一三年六月七日 15:48:07 
        /// </summary>
        /// <param name="treeListNodes"></param>
        private void GetTreeAllNodeForReplaceItem(TreeListNodes treeListNodes)
        {
            try
            {
                foreach (TreeListNode node in treeListNodes)
                {
                    if (node.Nodes.Count > 0)
                    {
                        GetTreeAllNodeForReplaceItem(node.Nodes);
                    }
                    else
                    {
                        if ((!node.HasChildren) && (node.Tag is EmrModel))//如果是模板类，则说明是叶子节点
                        {
                            m_TreeAllNode.Add(node);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 工具相关
        /// <summary>
        /// 导出XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_ExportXML_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ExprotXML();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 导出病历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_ExportEmr_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ExportFile();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Search_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Find();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 科室小模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_DeptTemplete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_PersonItem == null)
                {
                    m_PersonItem = new PersonItemManager(m_app);
                }
                m_PersonItem.ShowDialog();
                ///刷新模板列表
                //   CurrentInputBody.InitPersonTree(true, string.Empty);
                CurrentInputBody.InitPersonTree(true, CurrentInputBody.MyContainerCode);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 表格相关
        /// <summary>
        /// 插入表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_InsertTable_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.InsertTable();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_InsertRow_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.InsertTableRow();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 插入列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_InsertColumn_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.InsertTableColumn();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_DeleteTable_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.DeleteTable();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Row_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.DeleteTableRow();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Column_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.DeleteTableColumn();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barCheckItem_Selected_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.ChoiceTable();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 单元格属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_CellInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 表格属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_TableInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CurrentInputBody.SetTableProperty();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 拆分单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_SplitCell_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_CombineCell_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion
        #endregion

        #region 工具栏按钮事件 - Bar2
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Copy_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.CurrentForm.CurrentEditorControl.EMRDoc._Copy();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Paste_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!CurrentInputBody.CanEdit())
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc._Paste();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 剪切
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barItem_Cut_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!CurrentInputBody.CanEdit())
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc._Cut();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Undo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.CurrentForm.CurrentEditorControl.EMRDoc._Undo();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重做
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Redo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.CurrentForm.CurrentEditorControl.EMRDoc._Redo();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem_Font_EditValueChanged(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (barEditItem_Font.EditValue == null || !CurrentInputBody.CanEdit())
                {
                    return;
                }

                this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectioinFontName(barEditItem_Font.EditValue.ToString());
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem_Font_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (barEditItem_Font.EditValue == null || !CurrentInputBody.CanEdit())
                {
                    return;
                }

                this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectioinFontName(barEditItem_Font.EditValue.ToString());
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_BackColor_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (ColorDialog cdia = new ColorDialog())
                {
                    if (cdia.ShowDialog() == DialogResult.OK)
                    {
                        this.CurrentForm.CurrentEditorControl.PageBackColor = cdia.Color;
                        this.CurrentForm.CurrentEditorControl.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_FontColor_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (ColorDialog cdia = new ColorDialog())
                {
                    if (cdia.ShowDialog() == DialogResult.OK)
                    {
                        this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionColor(cdia.Color);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem_FontSize_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (barEditItem_FontSize.EditValue == null || !CurrentInputBody.CanEdit())
                {
                    return;
                }

                float size = FontCommon.GetFontSizeByName(barEditItem_FontSize.EditValue.ToString());
                this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionFontSize(size);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 字体大小切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItem_FontSize_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (barEditItem_FontSize.EditValue == null || !CurrentInputBody.CanEdit())
                {
                    return;
                }

                float size = FontCommon.GetFontSizeByName(barEditItem_FontSize.EditValue.ToString());
                this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionFontSize(size);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 斜体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Italy_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!CurrentInputBody.CanEdit())
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionFontItalic(this.btnItem_Italy.Down);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 下划线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_UnderLine_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!CurrentInputBody.CanEdit())
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionUnderLine(this.btnItem_UnderLine.Down);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 粗体（加粗）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Bold_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!CurrentInputBody.CanEdit())
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionFontBold(this.btnItem_Bold.Down);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 圈字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_QZ_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!CurrentInputBody.CanEdit())
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.SetCircleFont(this.btnItem_QZ.Down);
                this.CurrentForm.CurrentEditorControl.EMRDoc.Refresh();
                this.CurrentForm.CurrentEditorControl.Refresh();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 上标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Sup_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!CurrentInputBody.CanEdit())
                {
                    return;
                }
                ReSetSupSub();
                this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionSup(btnItem_Sup.Down);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 下标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Sub_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!CurrentInputBody.CanEdit())
                {
                    return;
                }
                ReSetSupSub();
                this.CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionSub(btnItem_Sub.Down);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 提取病历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_Collect_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string errorMessage = "请选择要提取的病历内容";
                string correctMessage = "病历提取成功";
                if (CurrentForm == null)
                {
                    MessageBox.Show(errorMessage);
                    return;
                }

                string collectContent = CurrentForm.CurrentEditorControl.EMRDoc.Content.GetSelectedText();
                if (collectContent != null && collectContent.Trim() != "")
                {
                    CurrentInputBody.PatUtil.InsertDocCollectContent(collectContent);
                    CurrentInputBody.InitBLCollect();
                    MessageBox.Show(correctMessage);
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 诊疗计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_AssessmentPlan_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DiagLibForm libForm = new DiagLibForm(m_app, "诊疗计划", "2");
                libForm.ShowDialog();
                if (CurrentForm == null || string.IsNullOrEmpty(libForm.GetDiag()) || !CurrentInputBody.CanEdit())
                {
                    return;
                }
                CurrentForm.CurrentEditorControl.EMRDoc._InserString(libForm.GetDiag());
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增的插入图片功能
        /// add by ywk 二〇一三年六月七日 15:10:49 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barItem_InsertPic_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog dlgFile = new System.Windows.Forms.OpenFileDialog())
                {
                    dlgFile.Filter = "图像文件 (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|所有文件 (*.*)|*.*";
                    dlgFile.Title = "插入图片";

                    dlgFile.ShowReadOnly = false;
                    dlgFile.ShowHelp = false;
                    dlgFile.CheckFileExists = true;
                    dlgFile.ReadOnlyChecked = false;
                    if (dlgFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Image img = Image.FromFile(dlgFile.FileName);
                        MemoryStream ms = new MemoryStream();
                        byte[] imagedata = null;
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                        imagedata = ms.GetBuffer();

                        this.CurrentForm.CurrentEditorControl.EMRDoc._InsertImage(imagedata);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请确认你所选文件是否为图片格式。");
            }
        }


        #endregion

        #region 工具栏按钮事件对应方法

        #region 出科检查
        private bool isReportVialde = false;
        private bool isReportVialde_to = false;
        private Thread th = null;
        private bool checkFlag;
        /// <summary>
        /// 出科检查
        /// </summary>
        public void GOMyCheck()
        {
            try
            {

                string showcontent = string.Empty;
                string hosCode = DS_SqlService.GetConfigValueByKey("HosCode");
                ///注：仁和医院和中心医院病历验证通过值相反
                bool checkFlagPassValue = hosCode == "1";
                Control.CheckForIllegalCrossThreadCalls = false;
                if (checkFlagPassValue)
                {
                    this.th = new Thread(new ThreadStart(this.GetReportVialde));
                    this.th.IsBackground = true;
                    this.th.Start();
                    while (this.th != null)
                    {
                        Application.DoEvents();
                    }
                }
                if (!isReportVialde)
                {
                    this.th = new Thread(new ThreadStart(this.OutDeptCheckFlag));
                    this.th.IsBackground = true;
                    this.th.Start();
                    while (this.th != null)
                    {
                        Application.DoEvents();
                    }
                    if (!isReportVialde_to)
                    {
                        if (checkFlag)
                        {///出科检查通过

                            DS_SqlService.UpdateOutDeptCheckFlag((int)m_CurrentInpatient.NoOfFirstPage, checkFlagPassValue);
                            MyMessageBox.Show("验证通过", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                        }
                        else
                        {
                            if (CheckMessage == "")
                            {
                                DS_SqlService.UpdateOutDeptCheckFlag((int)m_CurrentInpatient.NoOfFirstPage, checkFlagPassValue);
                                MyMessageBox.Show("验证通过", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                            }
                            else
                            {
                                DS_SqlService.UpdateOutDeptCheckFlag((int)m_CurrentInpatient.NoOfFirstPage, !checkFlagPassValue);

                                CheckTipContent m_CheckT = new CheckTipContent(m_app, CheckMessage, this);
                                m_CheckT.TopMost = true;

                                int workWidth = Screen.PrimaryScreen.WorkingArea.Width;
                                int thisWidth = m_CheckT.Width;
                                int m_Width = workWidth - thisWidth;
                                m_CheckT.Location = new Point(m_Width, 100);
                                m_CheckT.StartPosition = FormStartPosition.Manual;
                                m_CheckT.Show();
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

        public void OutDeptCheckFlag()
        {
            try
            {
                string showcontent = string.Empty;
                string hosCode = DS_SqlService.GetConfigValueByKey("HosCode");
                ///注：仁和医院和中心医院病历验证通过值相反
                bool checkFlagPassValue = hosCode == "1";
                isReportVialde_to = true;
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在检查验证相关数据...");
                checkFlag = CheckInpatient((int)m_CurrentInpatient.NoOfFirstPage, out showcontent);
                CheckMessage = showcontent;
                DS_Common.HideWaitDialog(m_WaitDialog);
                this.th.Interrupt();
                this.th = null;
            }
            catch (Exception)
            {

                throw;
            }
            isReportVialde_to = false;
        }


        public void GetReportVialde()
        {
            try
            {
                isReportVialde = true;
                int refresh = 0;
                IemMainPageManger m_IemMainPage = new IemMainPageManger(m_app, m_CurrentInpatient);
                if (m_IemMainPage != null)
                {
                    DrectSoft.Core.IEMMainPage.IemMainPageInfo IemInfo = m_IemMainPage.GetIemInfo();
                    if (IemInfo != null && IemInfo.IemBasicInfo != null)
                    {
                        //验证逻辑： 出院诊断列表中，存在属于传染病诊断的列表，且已经申报的次数小于该诊断设置的最大申报次数
                        string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                        string sqlText = "select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_2012 ie    left join zymosis_diagnosis z on ie.diagnosis_code=z.icd   left join iem_mainpage_basicinfo_2012 imb on imb.iem_mainpage_no=ie.iem_mainpage_no   where   z.valid=1  and ie.valide=1 and imb.valide=1   and  ie.iem_mainpage_no=@iem_mainpage_no  and (SELECT COUNT(zr.report_id) FROM zymosis_report zr where zr.diagicd10=z.icd and zr.noofinpat=imb.noofinpat and zr.vaild=1)<z.upcount  group by ie.iem_mainpage_no,z.icd,z.upcount,z.name    having count(z.icd)>0 ";
                        if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                        {
                            sqlText = "select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_sx ie    left join zymosis_diagnosis z on ie.diagnosis_code=z.icd   left join iem_mainpage_basicinfo_sx imb on imb.iem_mainpage_no=ie.iem_mainpage_no  where  z.valid=1  and ie.valide=1    and  ie.iem_mainpage_no=@iem_mainpage_no  and (SELECT COUNT(zr.report_id) FROM zymosis_report zr where zr.diagicd10=z.icd and zr.noofinpat=imb.noofinpat  and zr.vaild=1)<z.upcount  group by ie.iem_mainpage_no,z.icd,z.upcount,z.name    having count(z.icd)>0 ";

                        }
                        DataTable table = m_app.SqlHelper.ExecuteDataTable(sqlText, new SqlParameter[] { new SqlParameter("@iem_mainpage_no", IemInfo.IemBasicInfo.Iem_Mainpage_NO) }, CommandType.Text);
                        if (table != null && table.Rows.Count > 0)
                        {

                            if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人出院诊断符合传染病上报条件，是否立即填报？", "传染病上报", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                            {

                                // todo zhouhui
                                //ReportOperateDialog reportOperateDialog = new ReportOperateDialog(m_app, IemInfo.IemBasicInfo.Iem_Mainpage_NO);
                                //reportOperateDialog.LoadPage(m_CurrentInpatient.NoOfFirstPage.ToString(), "2", "1");
                                //reportOperateDialog.ShowDialog();
                            }


                        }
                    }
                }
                this.th.Interrupt();
                this.th = null;
            }
            catch (Exception)
            {

                throw;
            }
            isReportVialde = false;

        }

        /// <summary>
        /// 检查病人相关信息是否符合(出科检查)相关标准
        /// </summary>
        /// 注：输出提示信息
        /// <param name="noofinpat"></param>
        /// <param name="TipMessage"></param>
        /// <returns>输出提示信息</returns>
        public bool CheckInpatient(int noofinpat, out string TipMessage)
        {
            try
            {
                //先创建事务 add by ywk2013年7月11日 11:41:40
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.BeginTransaction();


                IemMainPageManger m_IemMainPage = new IemMainPageManger(m_app, m_CurrentInpatient);
                TipMessage = string.Empty;
                string ReturnMessage = string.Empty;//要返回的提示信息

                bool CheckRight = true;//出科检查是否检查通过
                bool CheckInDate = false;
                bool CheckOutDate = false;
                //此处原来是取所有的病人，再根据首页序号筛选
                //DataTable m_Dt = DS_SqlService.GetAllPatient();//此处数据要重新构建病人表， m_app.PatientInfos.Tables[0】取的是在院人员 

                //DataTable InpatientDt = m_Dt.Clone();//存储病人信息的表数据
                //DataRow[] filterrow = m_Dt.Select("noofinpat=" + noofinpat + "");
                //if (filterrow.Length > 0)
                //{
                //    for (int i = 0; i < filterrow.Length; i++)
                //    {
                //        InpatientDt.ImportRow(filterrow[i]);
                //    }
                //}
                //现改为直接根据病人首页序号查询 add by ywk 二〇一三年七月十一日 11:29:16
                DataTable InpatientDt = new DataTable();
                InpatientDt = DS_SqlService.GetInpatientByIDInTran(noofinpat);


                //0221:  messagebox("系统提示","入院日期不能大于出院日期")
                string AdmitHosDate1 = InpatientDt.Rows[0]["ADMITDATE"].ToString();//入院时间
                string OutHosDate1 = InpatientDt.Rows[0]["OUTHOSDATE"].ToString();//出院时间
                if (string.IsNullOrEmpty(AdmitHosDate1))//入院日期为空
                {
                    ReturnMessage += "入院日期不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                else
                {
                    CheckInDate = true;
                }
                if (string.IsNullOrEmpty(OutHosDate1))//出院日期为空
                {
                    ReturnMessage += "出院日期不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                else
                {
                    CheckOutDate = true;
                }

                if (CheckInDate && CheckOutDate)
                {
                    DateTime AdmitHosDate = DateTime.Parse(AdmitHosDate1);
                    DateTime OutHosDate = DateTime.Parse(OutHosDate1);
                    if (AdmitHosDate > OutHosDate)//入院日期大于出院日期！
                    {
                        ReturnMessage += "入院日期不能大于出院日期\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                }

                string m_totaldays = InpatientDt.Rows[0]["TOTALDAYS"].ToString();
                int I_TotalDay = Int32.Parse(m_totaldays == "" ? "0" : m_totaldays);
                if (string.IsNullOrEmpty(m_totaldays))//住院天数为空
                {
                    ReturnMessage += "住院天数不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                if (I_TotalDay < 0)
                {
                    ReturnMessage += "住院天数不能为负数\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                if (I_TotalDay == 0)
                {
                    ReturnMessage += "住院天数不能为0\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;

                }
                //0240:  messagebox("系统提示","出生日期不应该大于入院日期!")
                DateTime m_DtBirthDay = DateTime.Parse(InpatientDt.Rows[0]["BIRTH"].ToString());
                if (!string.IsNullOrEmpty(AdmitHosDate1))
                {
                    DateTime AdmitHosDate = DateTime.Parse(AdmitHosDate1);
                    if (m_DtBirthDay > AdmitHosDate)//出生日期大于入院日期
                    {
                        ReturnMessage += "出生日期不能大于入院日期\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                }
                //0247:  messagebox("提示","请在出院卡片首页中输入主要诊断")
                DrectSoft.Core.IEMMainPage.IemMainPageInfo IemInfo = m_IemMainPage.GetIemInfo();
                if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count < 0)
                {
                    ReturnMessage += "出院卡片首页中主要诊断不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                int m_Age = Int32.Parse(InpatientDt.Rows[0]["AGE"].ToString());
                if (m_Age < 0 || m_Age > 100)
                {
                    ReturnMessage += "年龄只能是0至100之间的整数\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }

                int m_MenAndInHop = Int32.Parse(IemInfo.IemBasicInfo.MenAndInHop.ToString() == "" ? "10" : IemInfo.IemBasicInfo.MenAndInHop.ToString());
                int m_InHopAndOutHop = Int32.Parse(IemInfo.IemBasicInfo.InHopAndOutHop.ToString() == "" ? "10" : IemInfo.IemBasicInfo.InHopAndOutHop.ToString());
                int m_BeforeOpeAndAfterOper = Int32.Parse(IemInfo.IemBasicInfo.BeforeOpeAndAfterOper.ToString() == "" ? "10" : IemInfo.IemBasicInfo.BeforeOpeAndAfterOper.ToString());
                int m_LinAndBingLi = Int32.Parse(IemInfo.IemBasicInfo.LinAndBingLi.ToString() == "" ? "10" : IemInfo.IemBasicInfo.LinAndBingLi.ToString());
                int m_InHopThree = Int32.Parse(IemInfo.IemBasicInfo.InHopThree.ToString() == "" ? "10" : IemInfo.IemBasicInfo.InHopThree.ToString());
                int m_FangAndBingLi = Int32.Parse(IemInfo.IemBasicInfo.FangAndBingLi.ToString() == "" ? "10" : IemInfo.IemBasicInfo.FangAndBingLi.ToString());
                if (m_MenAndInHop < 0 || m_MenAndInHop > 4 || m_InHopAndOutHop < 0 || m_InHopAndOutHop > 4 || m_BeforeOpeAndAfterOper < 0 || m_BeforeOpeAndAfterOper > 4 || m_LinAndBingLi < 0 || m_LinAndBingLi > 4 || m_InHopThree < 0 || m_InHopThree > 4 || m_FangAndBingLi < 0 || m_FangAndBingLi > 4)
                {
                    ReturnMessage += "诊断符合情况只能输入0到4之间的数字\n\r";//新版首页（1、符合 2、不符合 3、不肯定 4、未做）
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                if (m_MenAndInHop == 10 || m_InHopAndOutHop == 10 || m_BeforeOpeAndAfterOper == 10 || m_LinAndBingLi == 10 || m_InHopThree == 10 || m_FangAndBingLi == 10)
                {
                    ReturnMessage += "诊断符合情况不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }

                ArrayList DiagInfo = new ArrayList();
                DiagInfo.Add(IemInfo.IemBasicInfo.MenAndInHop.ToString());       //门诊与出院诊断符合情况  0.未做 1.符合 2.不符合 3.不肯定  4、未做  
                DiagInfo.Add(IemInfo.IemBasicInfo.InHopAndOutHop.ToString());    //入院与出院诊断符合情况 
                DiagInfo.Add(IemInfo.IemBasicInfo.BeforeOpeAndAfterOper.ToString());  //术前与术后诊断符合情况
                DiagInfo.Add(IemInfo.IemBasicInfo.LinAndBingLi.ToString());    //临床与病理诊断符合情况
                DiagInfo.Add(IemInfo.IemBasicInfo.InHopThree.ToString());
                DiagInfo.Add(IemInfo.IemBasicInfo.FangAndBingLi.ToString());    //放射与病理诊断符合情况 
                if (DiagInfo.Contains("2"))//存在误诊的情况//当前新版首页不存在误诊这个选项 add by ywk 2013年2月22日10:27:41  不符合就是误诊
                {
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(string.Format("诊断符合情况有输入[不符合]的选项，您是否要继续？"), "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                    {
                        CheckRight = true;
                    }
                    else
                    {
                        CheckRight = false;
                    }
                }
                string cansee = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cansee);
                string pageContorlVisable = doc.GetElementsByTagName("IemPageContorlVisable")[0].InnerText;
                if (CheckRight)//上面判断有误诊选择了继续
                {
                    if (ReturnMessage.Length > 10)
                    {
                        CheckRight = false;
                    }
                    if (DiagInfo[0].ToString() == "4" || DiagInfo[1].ToString() == "4" || DiagInfo[4].ToString() == "4")
                    {
                        ReturnMessage += "诊断符合情况中[门诊与住院]、[入院与出院]、[入院三日内]不能填[未做]\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    if (DiagInfo[2].ToString() == "1" && IemInfo.IemOperInfo.Operation_Table.Rows.Count == 0)//是确诊，手术信息为空的话 
                    {
                        ReturnMessage += "术前和术后手术信息为确诊，但无手术信息。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    if (DiagInfo[2].ToString() == "0" && IemInfo.IemOperInfo.Operation_Table.Rows.Count > 0)//是未作，手术信息有数据的话 
                    {
                        ReturnMessage += "术前和术后手术信息为未作，但有手术信息。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }

                    DataTable DtOper = IemInfo.IemOperInfo.Operation_Table;
                    if (DtOper.Rows.Count > 0)
                    {
                        for (int i = 0; i < DtOper.Rows.Count; i++)
                        {
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Operation_Date"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但手术日期不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCHOOSEDATE"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否择期手术不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                }
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCLEAROPE"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否无菌手术不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                }
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Anaesthesia_Type_Id"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但麻醉方式不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Execute_User1"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但手术医师不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Close_Level"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但切口愈合方式不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISGANRAN"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否感染不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                }
                            }
                        }
                    }
                    else
                    {
                        ReturnMessage += "无手术信息。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                }
                else//没选择误诊也要接着判断手术的信息
                {
                    DataTable DtOper = IemInfo.IemOperInfo.Operation_Table;
                    if (DtOper.Rows.Count > 0)
                    {
                        for (int i = 0; i < DtOper.Rows.Count; i++)
                        {
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Operation_Date"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但手术日期不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCHOOSEDATE"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否择期手术不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                }
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCLEAROPE"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否无菌手术不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                }
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Anaesthesia_Type_Id"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但麻醉方式不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Execute_User1"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但手术医师不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Close_Level"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但切口愈合方式不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISGANRAN"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否感染不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                }
                            }
                        }
                    }
                    else
                    {
                        ReturnMessage += "无手术信息。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                }

                //如果主诊断是关于生孩子的诊断，则需要判断以下内容
                //*********注：（diagnosis_type_id=7是表示主诊断，8是标识其他诊断 ）******* 
                //  如果主诊断icd码第一位是S或T开头，2，3位编码小于90的必须输外部损伤原因
                //    0034:  MessageBox('提示','你录入的婴儿数量与维护的不符，请点击多胎~r~n按钮维护婴儿记录！')
                string M_DiagICD = string.Empty;//用于存放主诊断的ICD编码

                if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count > 0)
                {
                    DataRow[] drrow = IemInfo.IemDiagInfo.OutDiagTable.Select(" diagnosis_type_id=7 ");
                    if (drrow.Length > 0)
                    {
                        M_DiagICD = drrow[0]["Diagnosis_Code"].ToString();
                    }
                    if (M_DiagICD.StartsWith("S") || M_DiagICD.StartsWith("T"))
                    {
                        int SencondThirdDiag = Int32.Parse(M_DiagICD.Substring(1, 2));
                        if (SencondThirdDiag < 90)
                        {
                            if (string.IsNullOrEmpty(IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID))
                            {
                                ReturnMessage += "请输入外部损伤原因。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                        }
                    }
                }

                ArrayList BabyDiagArr = new ArrayList();//存储关于生孩子的诊断信息
                DataTable dtDiagBaby = DS_SqlService.GetBabyDiagInfo();
                bool ISAboutBaby = false;//此变量标识诊断是否是生孩子的诊断
                if (!string.IsNullOrEmpty(M_DiagICD))//表明主诊断有值
                {
                    for (int i = 0; i < dtDiagBaby.Rows.Count; i++)
                    {
                        if (M_DiagICD.ToString() == dtDiagBaby.Rows[i]["icd"].ToString())
                        {
                            ISAboutBaby = true;
                            break;
                        }
                        else
                        {
                            ISAboutBaby = false;
                        }
                    }
                }

                if (ISAboutBaby)
                {
                    if (!string.IsNullOrEmpty(OutHosDate1)) //出院时间
                    {
                        DateTime OutHosDate = DateTime.Parse(OutHosDate1);
                        DateTime NowDate = DateTime.Now;
                        TimeSpan timeSpan = OutHosDate - NowDate;//两个时间相减 
                        if (timeSpan.Days > 10)
                        {
                            ReturnMessage += "出院日期大于当前日期10天以上，不能录入。\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                        }
                    }
                    if (I_TotalDay > 50)
                    {
                        ReturnMessage += "该患者住院天数超过50天。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    if (string.IsNullOrEmpty(IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID))
                    {
                        ReturnMessage += "损伤外部原因不能为空\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    if (string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.BithDay))
                    {
                        ReturnMessage += "请选择婴儿出生日期\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    if (string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.APJ) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.BithDayPrint) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.CC) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.CCQK) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.CFHYPLD) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.FMFS) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.Heigh))
                    {
                        ReturnMessage += "请在出院卡片Ⅱ中录入完整的妇婴信息\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    if (string.IsNullOrEmpty(IemInfo.IemBasicInfo.ZG_FLAG))
                    {
                        ReturnMessage += "请给出诊断的转归代码\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count == 0)
                    {
                        ReturnMessage += "该患者没有诊断\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(IemInfo.IemDiagInfo.OutDiagTable.Rows[0]["diagnosis_type_id"].ToString()))
                        {
                            ReturnMessage += "请选择诊断类别\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                        }
                    }
                }
                TipMessage = ReturnMessage;

                DS_SqlHelper.CommitTransaction();

                return CheckRight;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 上标和下标
        void ReSetSupSub()
        {
            try
            {
                CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionSup(false);
                CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionSub(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 下标
        /// </summary>
        private void SupChar(bool reslut)
        {
            try
            {
                ReSetSupSub();
                CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionSup(reslut);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 上标
        /// </summary>
        private void SubChar(bool reslut)
        {
            try
            {
                ReSetSupSub();
                CurrentForm.CurrentEditorControl.EMRDoc.SetSelectionSub(reslut);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 字体
        /// <summary>
        /// 设置字体下拉框
        /// </summary>
        private void InitFont()
        {
            try
            {
                repositoryItemComboBoxFont.Items.AddRange(FontCommon.FontList.ToArray());

                //添加字号
                foreach (string fontsizename in FontCommon.allFontSizeName)
                {
                    repositoryItemComboBox_Size.Items.Add(fontsizename);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 查找
        private SearchFrm m_Searchfrm;
        /// <summary>
        /// 查找
        /// </summary>
        public void Find()
        {
            try
            {
                if (CurrentForm == null)
                {
                    return;
                }
                if (m_Searchfrm != null)
                {
                    m_Searchfrm.Close();
                    m_Searchfrm.Dispose();
                }
                m_Searchfrm = new SearchFrm(m_app, CurrentForm.CurrentEditorControl);
                m_Searchfrm.Show(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 导出
        /// <summary>
        /// 导出病历文件
        /// </summary>
        private void ExportFile()
        {
            try
            {
                if (CurrentForm == null) return;
                using (System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog())
                {
                    string filename = CurrentForm.CurrentEditorControl.Text;
                    dlg.Filter = "电子病历文件|*.b";
                    dlg.FileName = filename;

                    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        byte[] data = CurrentForm.CurrentEditorControl.SaveBinary();
                        System.IO.FileStream fs = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.Create);
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 导出XML文件
        /// </summary>
        private void ExprotXML()
        {
            try
            {
                if (CurrentForm == null) return;
                using (System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog())
                {
                    string filename = CurrentForm.CurrentEditorControl.Text;
                    dlg.Filter = "XML文件|*.xml";
                    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        CurrentForm.CurrentEditorControl.EMRDoc.ToXMLFile(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region 历史病历
        /// <summary>
        /// 导入历史病历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItem_ImportHostoryEmr_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("IsDisableHistoryBatchIn");
                if (config == "1")
                {
                    MyMessageBox.Show("历史病历导入暂时无法使用，请稍后。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }

                if (CheckIsDoctor())
                {
                    string deptChangeID = string.Empty;
                    ChooseDeptForDailyEmrPrint chooseDept = new ChooseDeptForDailyEmrPrint("请选择病历需要导入的科室", m_CurrentInpatient.NoOfFirstPage.ToString(), true);
                    if (chooseDept.IsNeedShow)
                    {
                        chooseDept.ShowDialog();
                    }
                    if (chooseDept.DialogResult == DialogResult.Yes)
                    {
                        deptChangeID = chooseDept.DeptChangeID;
                    }
                    else
                    {
                        return;
                    }
                    HistoryEmrBatchInFormNew batchInForm = new HistoryEmrBatchInFormNew(m_app, m_CurrentInpatient, deptChangeID, this);
                    batchInForm.StartPosition = FormStartPosition.CenterScreen;
                    batchInForm.ShowDialog();
                }
                else if (CurrentEmployee.Kind == EmployeeKind.Nurse)
                {
                    HistoryEmrBatchInFormNurse nurseForm = new HistoryEmrBatchInFormNurse(m_app, Util.GetParentUserControl<UCEmrInput>(this));
                    nurseForm.StartPosition = FormStartPosition.CenterScreen;
                    nurseForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        void UCEmrInput_Shown(object sender, EventArgs e)
        {
            try
            {
                //自动显示病历批量导入界面
                CurrentInputBody.AutoShowHistoryEmrBatchInForm();
                this.FindForm().Shown -= new EventHandler(UCEmrInput_Shown);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void UCEmrInput_Activated(object sender, EventArgs e)
        {
            try
            {
                //自动显示病历批量导入界面
                CurrentInputBody.AutoShowHistoryEmrBatchInForm();
                this.FindForm().Activated -= new EventHandler(UCEmrInput_Activated);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 根据权限、病历状态、登录人决定BarButton和PopupButton的显示

        /// <summary>
        /// 处于审核状态时BarButton可以使用的情况
        /// </summary>
        /// <param name="b"></param>
        private void AuditStateBarButton(bool b)
        {
            try
            {
                btnItem_Audit.Enabled = b;
                btnItem_CancelAudit.Enabled = b;//取消审核 todo wwj 2011-09-06

                //可以审核就可以保存、编辑、只读等
                CanEditEmrDoc(b);

                //处于审核状态的病历不能删除、不能提交
                btnItem_Delete.Enabled = false;
                btnItem_Submit.Enabled = false;


                if (b)
                {//可以审核就可以编辑
                    SetDomentEditMode();
                }
                else
                {//不能审核时需要把编辑器置为只读
                    SetDocmentReadOnlyMode();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重设编辑模式下的Action状态
        /// </summary>
        public void ResetEditModeAction(EmrModel model)
        {
            try
            {
                if (model == null)
                {
                    return;
                }
                //重置所有的BarButton
                btnItem_New.Enabled = false;
                //设置编辑器相关按钮不可用
                CanEditEmrDoc(false);
                //设置审核相关按钮不可用
                SetAuditButState(false);

                IEmrModelPermision modelPermision;
                bool b;

                #region 根据权限
                modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Audit, CurrentEmployee);
                b = modelPermision.CanDo(model);
                if (b) // 文件已提交，处于审核模式，且有审核权限
                {
                    if (model.State == ExamineState.FirstExamine //主治医已经检查完成
                        && CurrentEmployee.Grade.Trim() != ""
                        && (DoctorGrade)Enum.Parse(typeof(DoctorGrade), CurrentEmployee.Grade) == DoctorGrade.Attending)//登录人是主治医
                    {
                        //主治医已经审核过，不能再修改
                        AuditStateBarButton(!b);
                    }
                    else
                    {
                        //主任医可以再次审核
                        AuditStateBarButton(b);
                    }
                }
                else // 非审核模式下，针对新增文件的操作才可用
                {
                    if (model.State == ExamineState.NotSubmit)
                    {
                        // 保存——具有编辑权限
                        modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Edit, CurrentEmployee);
                        b = modelPermision.CanDo(model);
                        btnItem_Save.Enabled = b;

                        if (!btnItem_Save.Enabled)//保存按钮不可用
                        {
                            //不可编辑
                            CanEditEmrDoc(false);
                            SetDocmentReadOnlyMode();
                        }
                        else
                        {
                            //可以编辑
                            CanEditEmrDoc(true);
                            SetDomentEditMode();
                        }

                        // 提交——新增状态，未归档，本人创建
                        modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Submit, CurrentEmployee);
                        b = modelPermision.CanDo(model);
                        btnItem_Submit.Enabled = b;

                        // 删除——新增状态，未归档，本人创建
                        modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Delete, CurrentEmployee);
                        b = modelPermision.CanDo(model);
                        btnItem_Delete.Enabled = b;
                    }
                    else
                    {
                        btnItem_Save.Enabled = false;
                        //不可编辑
                        CanEditEmrDoc(false);
                        SetDocmentReadOnlyMode();
                        btnItem_Submit.Enabled = false;
                        btnItem_Delete.Enabled = false;
                    }
                }
                #endregion

                //病案管理 add by cyq 2012-12-10
                if (floaderState == FloderState.None || floaderState == FloderState.FirstPage)
                {
                    CanEditEmrDoc(false);
                    SetDocmentReadOnlyMode();
                }
                else if (floaderState == FloderState.NoneAudit)
                {
                    SetAuditButState(false);
                }

                ControlTreeListNodeByUser();
                ///设置暂不启用的按钮显示状态
                NeedHideBtn();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 是否书写入院记录
        /// </summary>
        private bool BNew = false;
        /// <summary>
        /// 病历类型的界面例如入院记录只允许创建一份 update by Ukey 2016-08-28
        /// </summary>
        /// <param name="BNew"></param>
        public void BGetHasChildren(bool BNew)
        {
            this.BNew = BNew;
        }
        /// <summary>
        /// 重设编辑模式下的Action状态
        /// </summary>
        /// <param name="container"></param>
        public void ResetEditModeAction(EmrModelContainer container)
        {
            try
            {
                if (null == container)
                {
                    return;
                }

                //设置编辑器相关按钮不可用
                CanEditEmrDoc(false);
                //设置删除按钮不可用
                btnItem_Delete.Enabled = false;
                //设置审核相关按钮不可用
                SetAuditButState(false);

                //非病历类型的界面 如：首页、三测单没有删除和添加功能
                if (container.EmrContainerType == ContainerType.None || container.EmrContainerType == ContainerType.CommonDocment)
                {
                    btnItem_New.Enabled = false;
                }
                else
                {
                    IEmrModelPermision modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Create, CurrentEmployee);
                    btnItem_New.Enabled = modelPermision.CanDo(null);//新增的权限也要判断职工的医师级别
                }
                if (BNew == true && container.Name == "入院记录") btnItem_New.Enabled = false;//病历类型的界面例如入院记录只允许创建一份 update by Ukey 2016-08-28
                ControlTreeListNodeByUser();
                ///设置暂不启用的按钮显示状态
                NeedHideBtn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ResetEditModeAction方法出错" + ex.Message);
            }
        }

        /// <summary>
        /// 重设编辑模式下的Action状态
        /// </summary>
        /// <param name="container"></param>
        public void ResetEditModeAction(EmrModelDeptContainer container)
        {
            try
            {
                if (null == container)
                {
                    return;
                }

                //设置编辑器相关按钮不可用
                CanEditEmrDoc(false);
                //设置删除按钮不可用
                btnItem_Delete.Enabled = false;
                //设置审核相关按钮不可用
                SetAuditButState(false);

                IEmrModelPermision modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Create, CurrentEmployee);
                btnItem_New.Enabled = modelPermision.CanDo(null);//新增的权限也要判断职工的医师级别

                ControlTreeListNodeByUser();
                ///设置暂不启用的按钮显示状态
                NeedHideBtn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ResetEditModeAction方法出错" + ex.Message);
            }
        }

        #endregion

        #region 对登录人员进行控管，护士只能修改“护士文档”，医生能修改除“护士文档”以外所以的文档
        /// <summary>
        /// 对登录人员进行控管，护士只能修改“护士文档”，医生能修改除“护士文档”以外所以的文档
        /// </summary>
        private void ControlTreeListNodeByUser()
        {
            try
            {
                if (focusedNode == null)
                {
                    return;
                }
                if (CurrentEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
                {
                    bool isNurseNode = false;
                    SetBarButtonStatusNurse(focusedNode, ref isNurseNode);
                    if (!isNurseNode)
                    {
                        SetEmrDocNotEdit();
                    }
                }
                else if (CheckIsDoctor())//当前登录人是医生
                {
                    bool isDoctorNode = true;
                    SetBarButtonStatusDoctor(focusedNode, ref isDoctorNode);
                    if (!isDoctorNode)
                    {
                        SetEmrDocNotEdit();
                    }
                }
                else//其他人
                {
                    SetEmrDocNotEdit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 不可编辑
        /// </summary>
        private void SetEmrDocNotEdit()
        {
            try
            {
                CanEditEmrDoc(false);
                SetDocmentReadOnlyMode();
                btnItem_New.Enabled = false;
                btnItem_Submit.Enabled = false;//提交
                btnItem_Audit.Enabled = false;//审核
                btnItem_CancelAudit.Enabled = false;//取消审核
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 判断树节点是否是护士文档的节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isNurseNode"></param>
        private void SetBarButtonStatusNurse(TreeListNode node, ref bool isNurseNode)
        {
            try
            {
                if (node.ParentNode == null)
                {
                    return;
                }
                else
                {
                    if (node.ParentNode.GetValue("colName").ToString() == "护士文档")
                    {
                        isNurseNode = true;
                        return;
                    }
                }
                SetBarButtonStatusNurse(node.ParentNode, ref isNurseNode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 判断树节点是否是除护士文档以外的节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isDoctorNode"></param>
        private void SetBarButtonStatusDoctor(TreeListNode node, ref bool isDoctorNode)
        {
            try
            {
                if (node.ParentNode == null)
                {
                    return;
                }
                else
                {
                    if (node.ParentNode.GetValue("colName").ToString() == "护士文档")
                    {
                        isDoctorNode = false;
                        return;
                    }
                }
                SetBarButtonStatusDoctor(node.ParentNode, ref isDoctorNode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据编辑器开关控制编辑器的编辑情况
        /// </summary>
        private void SetEditorNonEnable()
        {
            try
            {
                if (!m_EditorEnableFlag)
                {
                    SetEmrDocNotEdit();
                    if (CurrentForm != null)
                    {
                        SetDocmentReadOnlyMode();
                        CurrentForm.CurrentEditorControl.EMRDoc.Info.ShowParagraphFlag = false;
                        CurrentForm.CurrentEditorControl.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 判断当前用户是否是医生
        /// </summary>
        /// <returns></returns>
        private bool CheckIsDoctor()
        {
            try
            {
                if (CurrentEmployee.Kind == EmployeeKind.Doctor || CurrentEmployee.Kind == EmployeeKind.Specialist || CurrentEmployee.Kind == EmployeeKind.Outdoctor)//当前登录人是医生
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 设置编辑器 DocumentModel

        /// <summary>
        /// 设置当前编辑器为只读模式
        /// </summary>
        public void SetDocmentReadOnlyMode()
        {
            try
            {
                if (CurrentForm == null)
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Read;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置当前编辑器为可编辑模式
        /// </summary>
        public void SetDomentEditMode()
        {
            try
            {
                if (CurrentForm == null)
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置当前编辑器为设计模式
        /// </summary>
        public void SetDocmentDesginMode()
        {
            try
            {
                if (CurrentForm == null)
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Design;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 设置当前编辑器为清洁模式
        /// </summary>
        public void SetDocmentClearMode()
        {
            try
            {
                if (CurrentForm == null)
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Clear;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 刷新文书录入
        /// <summary>
        /// 重新加载整个文书录入界面
        /// </summary>
        private void RefreshEMRMainPad()
        {
            try
            {
                m_app.ChoosePatient(m_CurrentInpatient.NoOfFirstPage);
                m_app.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 宏元素
        /// <summary>
        /// 刷新文书录入中的宏元素  此方法供外部反射调用，用于刷新宏元素的值
        /// </summary>
        public void RefreshMacroData()
        {
            try
            {
                MacroUtil.MacroSource = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 窗体大小改变事件
        /// <summary>
        /// 窗体大小切换时，病程记录预览区更随变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCEmrInput_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                //if (null == CurrentInputBody || null == CurrentInputBody.CurrentTreeList.FocusedNode || null == CurrentInputBody.CurrentTreeList.FocusedNode.Tag || !(null == CurrentInputBody.CurrentTreeList.FocusedNode.Tag is EmrModel) || null == CurrentInputBody.ucEmrInputPreView)
                //非病历 edit by ywk 2013年7月11日 16:59:58
                if (CurrentInputBody == null || CurrentInputBody.CurrentForm == null)
                {
                    return;
                }
                if (null == CurrentInputBody || null == CurrentInputBody.CurrentTreeList.FocusedNode || null == CurrentInputBody.CurrentTreeList.FocusedNode.Tag || null == CurrentInputBody.ucEmrInputPreView)
                {
                    return;
                }

                CurrentInputBody.ucEmrInputPreView.ResetSize();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion
        /// <summary>
        /// 调阅病理系统
        /// add by ywk 二〇一三年八月九日 16:26:55 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallPathologic_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ShowPathologic showp = new ShowPathologic(m_app);
                showp.StartPosition = FormStartPosition.CenterScreen;
                //showp.WindowState = FormWindowState.Maximized;
                showp.Show();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }

        }
        /// <summary>
        /// 更改常用词功能重新开放
        /// add by ywk 2013年9月4日 17:24:46
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeWord_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (this.CurrentInputBody != null && this.CurrentInputBody.CurrentModel != null)
                {
                    ChangeMacroFrom changeMacro = new ChangeMacroFrom(m_app, CurrentInputBody.CurrentForm, CurrentInpatient);
                    changeMacro.StartPosition = FormStartPosition.CenterScreen;
                    if (changeMacro.ShowDialog() == DialogResult.Yes)
                    {
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("确定要保存到数据库中吗？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            CurrentInputBody.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DrectSoft.Library.EmrEditor.Src.Clipboard.EmrClipboard.Data = null;
                DrectSoft.Library.EmrEditor.Src.Clipboard.EmrClipboard.PatientID = null;
                Clipboard.SetData(System.Windows.Forms.DataFormats.UnicodeText, null);
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("清空成功");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        DocumentModel DocumentModel = new DocumentModel();
        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (barCheckItem1.Checked)
                {
                    DocumentModel = this.CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel;
                    SetDocmentClearMode();
                }
                else
                {
                    if (DocumentModel == DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Read)
                    {
                        SetDocmentReadOnlyMode();
                    }
                    else if (DocumentModel == DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Edit)
                    {
                        SetDomentEditMode();
                    }
                    else if (DocumentModel == DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Design)
                    {
                        SetDocmentDesginMode();
                    }
                }

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_left_DownChanged(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!this.btn_left.Down)//取消左对齐
                {
                    this.CurrentForm.CurrentEditorControl.EMRDoc.SetAlign(ParagraphAlignConst.Left);
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.SetAlign(ParagraphAlignConst.Left);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        /// <summary>
        /// add by ukey 2017-02-10 Set up a repair record after admission, 
        /// transfer or admission records cannot write discharge diagnosis problems for the doctor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_get_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string sql = @"update RECORDDETAIL t set t.owner = '{0}', t.auditor = decode(t.auditor, '', '', '{0}') where t.noofinpat = '{1}' and t.sortid = 'AB' and t.valid = 1;";
                Inpatient InpatientDt = new Inpatient();
                InpatientDt = DS_SqlService.GetPatientInfo(m_CurrentInpatient.NoOfFirstPage.ToString());
                if (InpatientDt.InfoOfAdmission.DischargeInfo.CurrentDepartment.Code == m_app.User.CurrentDeptId)
                {
                    try
                    {
                        m_app.SqlHelper.ExecuteNoneQuery(string.Format(sql, m_app.User.Id, (int)m_CurrentInpatient.NoOfFirstPage), CommandType.Text);
                    }
                    catch (Exception ex)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                        return;
                    }
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("更改成功,关闭文书录入重新打开！");
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
