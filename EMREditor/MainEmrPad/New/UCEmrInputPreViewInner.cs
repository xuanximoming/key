using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Print;
using DrectSoft.Service;
using DrectSoft.DSSqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad.New
{
    /// <summary>
    /// 文书预览功能
    /// </summary>
    public partial class UCEmrInputPreViewInner : DevExpress.XtraEditors.XtraUserControl
    {
        #region Field && Property

        IEmrHost m_App;
        IEmrHost App
        {
            get
            {
                if (m_App == null)
                {
                    m_App = Util.GetEmrHost(this);
                }
                return m_App;
            }
        }

        Inpatient m_CurrentInpatient;
        Inpatient CurrentInpatient
        {
            get
            {
                if (m_CurrentInpatient == null)
                {
                    m_CurrentInpatient = Util.GetCurrentInpatient(this);
                }
                return m_CurrentInpatient;
            }
        }

        /// <summary>
        /// 病历对应科室，recorddetail.changeID 与 inpatientchangeinfo.id
        /// </summary>
        string ChangeID = string.Empty;

        /// <summary>
        /// 第一次调用此用户控件时传入的病历号ID
        /// </summary>
        string FirstCallRecordDetailID = string.Empty;
        /// <summary>
        /// 第一次调用此用户控件时传入的病历中CaptionDateTime的值
        /// </summary>
        string FirstCallRecordDetailCaptionDateTime = string.Empty;

        /// <summary>
        /// 默认勾中20份病程，意味着FirstCallRecordDetailID周围最近的20份病历在Load页面的时候会加载到预览区中显示
        /// </summary>
        int m_NeedCheckItemCount = 40;

        /// <summary>
        /// 第一次页面打开时的高度
        /// </summary>
        public int FirstHeight = 180;
        /// <summary>
        /// 点击恢复预览区时的高度
        /// </summary>
        public int DefaultHeight = 300;

        public EditorForm CurrentEditorForm
        {
            get
            {
                return m_EditorForm;
            }
            set
            {
                m_EditorForm = value;
            }
        }
        EditorForm m_EditorForm;

        private UCEmrInputBody m_CurrentUCEmrInputBody;
        public UCEmrInputBody CurrentUCEmrInputBody
        {
            get
            {
                if (m_CurrentUCEmrInputBody == null)
                {
                    m_CurrentUCEmrInputBody = Util.GetParentUserControl<UCEmrInputBody>(this);
                }
                return m_CurrentUCEmrInputBody;
            }
        }

        private CheckedListBoxItem m_FocusedCheckItem;

        #endregion

        #region WaitDialog
        /// <summary>
        /// 界面中调用的等待窗体
        /// </summary>
        private WaitDialogForm m_WaitDialog;

        public void SetWaitDialogCaption(string caption)
        {
            try
            {
                if (m_WaitDialog != null)
                {
                    if (!m_WaitDialog.Visible)
                    {
                        m_WaitDialog.Visible = true;
                    }
                    m_WaitDialog.Caption = caption;
                }
                else
                {
                    m_WaitDialog = new WaitDialogForm(caption, "提示");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HideWaitDialog()
        {
            try
            {
                if (m_WaitDialog != null)
                    m_WaitDialog.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region .ctor

        public UCEmrInputPreViewInner()
        {
            InitializeComponent();
            navBarGroupRight.NavigationPaneVisible = false;
            InitSize();
        }

        public UCEmrInputPreViewInner(string changeID, string recordDetailID)
            : this()
        {
            ChangeID = changeID;
            FirstCallRecordDetailID = recordDetailID;
        }

        private void Init()
        {
            try
            {
                GetNeedCheckItemCount();
                m_EditorForm = new EditorForm(CurrentInpatient, App);
                m_EditorForm.Dock = DockStyle.Fill;
                panelControlMain.Controls.Add(m_EditorForm);
                RegisterEvent();
                btnCopy.BringToFront();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitSize()
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("EmrSetting");
                if (!string.IsNullOrEmpty(config))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(config);
                    XmlNodeList initList = doc.GetElementsByTagName("DailyPreViewInitHeight");
                    if (null != initList && initList.Count > 0)
                    {
                        string initValue = null == initList[0].InnerText ? "" : initList[0].InnerText.Trim();
                        if (!string.IsNullOrEmpty(initValue) && Tool.IsNumeric(initValue))
                        {
                            FirstHeight = int.Parse(initValue);
                        }
                    }
                    XmlNodeList defaultList = doc.GetElementsByTagName("DailyPreViewDefaultHeight");
                    if (null != defaultList && defaultList.Count > 0)
                    {
                        string defaultValue = null == defaultList[0].InnerText ? "" : defaultList[0].InnerText.Trim();
                        if (!string.IsNullOrEmpty(defaultValue) && Tool.IsNumeric(defaultValue))
                        {
                            DefaultHeight = int.Parse(defaultValue);
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
        /// 获取需要选中的病程数目
        /// </summary>
        private void GetNeedCheckItemCount()
        {
            try
            {
                string cnt = DS_SqlService.GetConfigValueByKey("PreViewEmrCount");
                if (null != cnt && !string.IsNullOrEmpty(cnt.Trim()) && cnt.Split('|').Length == 2)
                {
                    m_NeedCheckItemCount = Convert.ToInt32(cnt.Split('|')[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Load 界面时需要展示的数据
        private void UCEmrInputPreViewInner_Load(object sender, EventArgs e)
        {
            try
            {
                Init();
                InitEditor();
                SetCheckBoxList();
                //(new Thread(new ThreadStart(SetCheckBoxList))).Start();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitEditor()
        {
            try
            {
                m_EditorForm.CurrentEditorControl.EMRDoc.ShowHeader = false;
                m_EditorForm.CurrentEditorControl.EMRDoc.ShowFooter = false;
                m_EditorForm.CurrentEditorControl.EMRDoc.ShowFooterLine = false;
                m_EditorForm.CurrentEditorControl.EMRDoc.ShowHeaderLine = false;
                m_EditorForm.CurrentEditorControl.PageSpacing = 0;
                m_EditorForm.SetBackColor(Color.WhiteSmoke);
                m_EditorForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Read;
                m_EditorForm.EditorRefresh();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 从m_Appcfg表中捞取病历预览的页面配置的Xml
        /// </summary>
        private void InitEmrPageSetting()
        {
            try
            {
                string xmldoc = BasicSettings.GetStringConfig("PageSettingPreView");
                System.Xml.XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmldoc);
                XmlNode pagesetting = doc.ChildNodes[0].SelectSingleNode("pagesettings");
                if (pagesetting != null)
                {
                    XmlElement ele = (pagesetting as XmlElement).SelectSingleNode("page") as XmlElement;
                    XPageSettings ps = new XPageSettings();
                    ps.PaperSize.Kind = (PaperKind)Enum.Parse(typeof(PaperKind), ele.GetAttribute("kind"));
                    if (ps.PaperSize.Kind == PaperKind.Custom)
                    {
                        ps.PaperSize.Height = int.Parse(ele.GetAttribute("height"));
                        ps.PaperSize.Width = int.Parse(ele.GetAttribute("width"));
                    }
                    ele = (pagesetting as XmlElement).SelectSingleNode("margins") as XmlElement;
                    ps.Margins.Left = int.Parse(ele.GetAttribute("left"));
                    ps.Margins.Right = int.Parse(ele.GetAttribute("right"));
                    ps.Margins.Top = int.Parse(ele.GetAttribute("top"));
                    ps.Margins.Bottom = int.Parse(ele.GetAttribute("bottom"));

                    ele = (pagesetting as XmlElement).SelectSingleNode("landscape") as XmlElement;
                    ps.Landscape = bool.Parse(ele.GetAttribute("value"));
                    ele = (pagesetting as XmlElement).SelectSingleNode("header") as XmlElement;
                    ps.HeaderHeight = int.Parse(ele.GetAttribute("height"));
                    ele = (pagesetting as XmlElement).SelectSingleNode("footer") as XmlElement;
                    ps.FooterHeight = int.Parse(ele.GetAttribute("height"));
                    this.m_EditorForm.CurrentEditorControl.EMRDoc.Pages.PageSettings = ps;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置复选框列表
        /// </summary>
        public void SetCheckBoxList()
        {
            try
            {
                //SetPreViewVisible(false);
                DataTable dt = GetEmrRecord(m_CurrentInpatient.NoOfFirstPage.ToString(), ChangeID);
                if (CheckIfNeedRefresh(dt, CurrentUCEmrInputBody.CurrentTreeList.FocusedNode))
                {
                    if (MyMessageBox.Show("数据库中病历有更新，建议您刷新后预览。您是否要刷新病程记录？", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Yes)
                    {
                        TreeListNode focusedNode = CurrentUCEmrInputBody.CurrentTreeList.FocusedNode;
                        if (null != focusedNode && null != focusedNode.Tag && focusedNode.Tag is EmrModel)
                        {
                            EmrModel model = focusedNode.Tag as EmrModel;
                            m_App.CurrentSelectedEmrID = model.InstanceId.ToString();
                        }
                        CurrentUCEmrInputBody.RefreshBingChengJiLu();
                    }
                }

                m_WaitDialog = new WaitDialogForm("正在加载病程预览区...", "请稍候");
                SetCheckBoxListSource(dt);
                DefaultCheckItem();
                XmlDocument doc = ConcatCheckedEmrContent();

                SetEmrContent(doc);
                SetPreViewVisible(true);
                CurrentEditorForm.SetCurrentElement(FirstCallRecordDetailCaptionDateTime);
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DataTable GetEmrRecord(string noofinpat, string changeID)
        {
            try
            {
                //获取指定病人所在病区的所有病历 TODO
                string sqlGetEmrRecord = string.Format(
                    @"select id, name, captiondatetime, content from recorddetail where noofinpat = '{0}' and changeid = '{1}' and valid = 1 and sortid = 'AC' order by captiondatetime",
                    noofinpat, changeID);
                return DS_SqlHelper.ExecuteDataTable(sqlGetEmrRecord, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataTable GetEmrRecordByID(string recordDetailID)
        {
            try
            {
                //获取指定病人所在病区的所有病历 TODO
                string sqlGetEmrRecord = string.Format(
                    @"select id, name, captiondatetime, '' content from recorddetail where id = '{0}' order by captiondatetime", recordDetailID);
                return DS_SqlHelper.ExecuteDataTable(sqlGetEmrRecord, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="dt"></param>
        private void SetCheckBoxListSource(DataTable dt)
        {
            try
            {
                if (checkedListBoxControlEmrNode.InvokeRequired)
                {
                    checkedListBoxControlEmrNode.Invoke(new Action<DataTable>(SetCheckBoxListSource), dt);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        checkedListBoxControlEmrNode.Items.Add(new EmrNode(dr), false);
                    }
                }
                if (null != checkedListBoxControlEmrNode.Items && checkedListBoxControlEmrNode.Items.Count > 0)
                {
                    m_FocusedCheckItem = checkedListBoxControlEmrNode.Items[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 默认选中复选框列表中的指定项目
        /// </summary>
        private void DefaultCheckItem()
        {
            try
            {
                if (checkedListBoxControlEmrNode.InvokeRequired)
                {
                    checkedListBoxControlEmrNode.Invoke(new Action(DefaultCheckItem));
                }
                else
                {
                    //if (checkedListBoxControlEmrNode.Items.Count <= m_NeedCheckItemCount)
                    //{
                    //    checkedListBoxControlEmrNode.CheckAll();
                    //}
                    //else
                    {
                        int index = -1;//FirstCallRecordDetailID中在复选框列表中的索引
                        int needCheckCount = m_NeedCheckItemCount;//在复选框列表中需要选中的项目
                        foreach (CheckedListBoxItem item in checkedListBoxControlEmrNode.Items)
                        {
                            EmrNode emrNode = ((ListBoxItem)item).Value as EmrNode;
                            if (emrNode != null)
                            {
                                if (emrNode.ID == FirstCallRecordDetailID.ToString())
                                {
                                    FirstCallRecordDetailCaptionDateTime = emrNode.DataRowItem["captiondatetime"].ToString();
                                    index = checkedListBoxControlEmrNode.Items.IndexOf(item);
                                }
                            }
                        }

                        if (index != -1)
                        {
                            #region 勾中指定病历周围m_NeedCheckItemCount份病历
                            checkedListBoxControlEmrNode.Items[index].CheckState = CheckState.Checked;
                            for (int i = 1; i <= m_NeedCheckItemCount; i++)
                            {
                                if (index + i < checkedListBoxControlEmrNode.Items.Count)
                                {
                                    checkedListBoxControlEmrNode.Items[index + i].CheckState = CheckState.Checked;
                                }

                                if (checkedListBoxControlEmrNode.CheckedItems.Count == m_NeedCheckItemCount) break;

                                if (index - i >= 0)
                                {
                                    checkedListBoxControlEmrNode.Items[index - i].CheckState = CheckState.Checked;
                                }

                                if (checkedListBoxControlEmrNode.CheckedItems.Count == m_NeedCheckItemCount) break;
                            }
                            #endregion

                            checkedListBoxControlEmrNode.SelectedIndex = index;
                            checkedListBoxControlEmrNode.MakeItemVisible(index);
                        }
                        else
                        {
                            int itemCount = checkedListBoxControlEmrNode.Items.Count;
                            for (int i = 1; i <= m_NeedCheckItemCount; i++)
                            {
                                if (itemCount - i >= 0)
                                {
                                    checkedListBoxControlEmrNode.Items[itemCount - i].CheckState = CheckState.Checked;
                                }
                            }

                            int selectIndex = itemCount - m_NeedCheckItemCount;
                            checkedListBoxControlEmrNode.SelectedIndex = selectIndex;
                            checkedListBoxControlEmrNode.MakeItemVisible(selectIndex);
                        }

                        if (checkedListBoxControlEmrNode.CheckedItems.Count == checkedListBoxControlEmrNode.Items.Count)
                        {
                            checkEditAll.Checked = true;
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
        /// 拼接选中病历的内容
        /// </summary>
        private XmlDocument ConcatCheckedEmrContent()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode nodeBody = null;

                List<string> emrList = new List<string>();
                foreach (CheckedListBoxItem item in checkedListBoxControlEmrNode.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        EmrNode emrNode = ((ListBoxItem)item).Value as EmrNode;
                        if (emrNode.DataRowItem["content"].ToString() == "")
                        {
                            emrNode.DataRowItem["content"] = GetEmrContentByID(emrNode.ID);
                        }
                        string emrContent = emrNode.DataRowItem["content"].ToString();
                        if (!string.IsNullOrEmpty(emrContent))
                        {
                            emrList.Add(emrContent);
                        }
                    }
                }

                if (emrList.Count > 0)
                {
                    doc = new XmlDocument();
                    for (int i = 0; i < emrList.Count; i++)
                    {
                        try
                        {
                            if (doc.InnerText == "")
                            {
                                string emrContent = emrList[i];
                                doc.PreserveWhitespace = true;
                                doc.LoadXml(emrContent);
                                nodeBody = doc.SelectSingleNode("document/body");
                            }
                            else
                            {
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.PreserveWhitespace = true;
                                xmlDoc.LoadXml(emrList[i]);
                                XmlNode bodyNode = xmlDoc.SelectSingleNode("document/body");
                                foreach (XmlNode node in bodyNode.ChildNodes)
                                {
                                    XmlNode deepCopyNode = doc.ImportNode(node, true);
                                    nodeBody.AppendChild(deepCopyNode);
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                return doc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetEmrContentByID(string id)
        {
            try
            {
                string sqlGetEmrContent = string.Format("select content from recorddetail where id = '{0}'", id);
                return DS_SqlHelper.ExecuteScalar(sqlGetEmrContent, CommandType.Text).ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 设置病历内容
        /// </summary>
        private void SetEmrContent(XmlDocument doc)
        {
            try
            {
                if (doc.OuterXml != "")
                {
                    if (m_EditorForm.InvokeRequired)
                    {
                        m_EditorForm.Invoke(new Action<XmlDocument>(SetEmrContent), doc);
                    }
                    else
                    {
                        m_EditorForm.SetClearModel();
                        m_EditorForm.LoadDocument(doc.OuterXml);
                        InitEmrPageSetting();
                        m_EditorForm.EditorRefresh();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetPreViewOperation(int i)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<int>(SetPreViewOperation), i);
                }
                else
                {
                    if (i == 1)
                    {
                        GetParentUserControl().BtnUp();
                    }
                    else if (i == 2)
                    {
                        GetParentUserControl().Restore();
                    }
                    else if (i == 3)
                    {
                        GetParentUserControl().BtnDown();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 父窗体是否可见
        /// </summary>
        /// <param name="isShow"></param>
        private void SetPreViewVisible(bool isShow)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<bool>(SetPreViewVisible), isShow);
                }
                else
                {
                    //GetParentUserControl().Visible = isShow;
                    if (isShow == true)
                    {
                        GetParentUserControl().Height = FirstHeight;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Method
        private UCEmrInputPreView GetParentUserControl()
        {
            try
            {
                Control ctl = this;
                while (!(ctl is UCEmrInputPreView))
                {
                    ctl = ctl.Parent;
                }
                return (UCEmrInputPreView)ctl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RegisterEvent()
        {
            try
            {
                m_EditorForm.CurrentEditorControl.MouseUp += new MouseEventHandler(CurrentEditorControl_MouseUp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 编辑器中右键
        void CurrentEditorControl_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    popupMenuEditor.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 定位指定的病程
        private void btnItemOpenForEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string msg = CurrentEditorForm.LocateDailyEmrByCursor();
                GetParentUserControl().OnLocateDailyEmrForEdit(ChangeID, msg);
                UCEmrInputPreView preView = Util.GetParentUserControl<UCEmrInputPreView>(this);

                //打开指定的病程后重新控制病程预览区的高度
                if (preView.Height > FirstHeight)
                {
                    preView.Height = FirstHeight;
                    preView.ButtonRestore.Enabled = false;
                    preView.ButtonUp.Enabled = true;
                    preView.ButtonDown.Enabled = true;
                }
                this.CurrentEditorForm.SetCurrentElement(msg);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region ButtonClickEvent
        /// <summary>
        /// 设置背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackColor_Click(object sender, EventArgs e)
        {
            try
            {
                using (ColorDialog cdia = new ColorDialog())
                {
                    if (cdia.ShowDialog() == DialogResult.OK)
                    {

                        m_EditorForm.CurrentEditorControl.PageBackColor = cdia.Color;
                        m_EditorForm.CurrentEditorControl.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复制编辑器中选中内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                m_EditorForm.CurrentEditorControl.EMRDoc._Copy();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设置选中的字体颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFontColor_Click(object sender, EventArgs e)
        {
            try
            {
                using (ColorDialog cdia = new ColorDialog())
                {
                    if (cdia.ShowDialog() == DialogResult.OK)
                    {
                        m_EditorForm.CurrentEditorControl.EMRDoc.SetSelectionColor(cdia.Color);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 全选
        private void checkEditAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditAll.Checked)
                {
                    checkedListBoxControlEmrNode.CheckAll();
                }
                else
                {
                    checkedListBoxControlEmrNode.UnCheckAll();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void checkedListBoxControlEmrNode_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                var obj = checkedListBoxControlEmrNode.GetItem(e.Index);
                CheckedListBoxItem item = (CheckedListBoxItem)obj;
                if (null == item)
                {
                    return;
                }
                if (item.CheckState == CheckState.Unchecked)
                {
                    SetCheckAllStateUnEvent(false);
                }
                else
                {
                    foreach (CheckedListBoxItem subItem in checkedListBoxControlEmrNode.Items)
                    {
                        if (subItem.CheckState == CheckState.Unchecked)
                        {
                            SetCheckAllStateUnEvent(false);
                            return;
                        }
                    }
                    SetCheckAllStateUnEvent(true);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void SetCheckAllStateUnEvent(bool boo)
        {
            try
            {
                checkEditAll.CheckedChanged -= new EventHandler(checkEditAll_CheckedChanged);
                checkEditAll.Checked = boo;
                checkEditAll.CheckedChanged += new EventHandler(checkEditAll_CheckedChanged);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 刷新选中的病程
        /// <summary>
        /// 显示勾中的病程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBoxControlEmrNode.CheckedItems.Count > 0)
                {
                    RefreshEmrContent();
                }
                else
                {
                    MyMessageBox.Show("请勾选需要显示的病程记录", "提示", MyMessageBoxButtons.Yes, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void RefreshEmrContent()
        {
            try
            {
                SetWaitDialogCaption("正在捞取病程记录，请稍后...");
                XmlDocument doc = ConcatCheckedEmrContent();
                SetWaitDialogCaption("正在组装病程记录，请稍后...");
                SetEmrContent(doc);
                CurrentEditorForm.CurrentEditorControl.EMRDoc.Content.SelectStart = 0;
                CurrentEditorForm.CurrentEditorControl.ScrollViewtopToCurrentElement();
                HideWaitDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 编辑器中的病历保存之后对病程预览区的影响 
        /// 如果是新增：直接在预览区左侧的列表中增加此节点，但是不勾中，预览区中显示的病程内容不发生改变
        /// 如果是修改：在预览区左侧的列表中判断此病历有无勾中，如果勾中则重新捞取此病历的内容，并重新显示在预览区中
        /// </summary>
        public void AfterSaveEmr(string recorddetailID, string displayDateTime)
        {
            try
            {
                //获取recorddetail.ID == recorddetailID的病历
                var listBoxItem = checkedListBoxControlEmrNode.Items.Cast<CheckedListBoxItem>().Where(item =>
                    {
                        EmrNode emrNode = item.Value as EmrNode;
                        if (emrNode != null && emrNode.ID == recorddetailID)
                        {
                            return true;
                        }
                        return false;
                    }).FirstOrDefault<CheckedListBoxItem>();
                if (listBoxItem == null)
                {
                    #region 新增树节点
                    DataTable dtRecord = GetEmrRecordByID(recorddetailID);
                    if (dtRecord.Rows.Count > 0)
                    {
                        EmrNode emrNode = new EmrNode(dtRecord.Rows[0]);
                        int insertIndex = 0;
                        if (checkedListBoxControlEmrNode.Items.Count > 0)
                        {
                            //计算比上面获取病历的captiondatetime小的病历记录数目
                            int cnt = checkedListBoxControlEmrNode.Items.Cast<ListBoxItem>().Count(item =>
                                {
                                    EmrNode node = item.Value as EmrNode;
                                    if (emrNode != null &&
                                        (Convert.ToDateTime(emrNode.DataRowItem["captiondatetime"]) - Convert.ToDateTime(node.DataRowItem["captiondatetime"])).TotalSeconds > 0)
                                    {
                                        return true;
                                    }
                                    return false;
                                });
                            insertIndex += cnt;
                        }
                        //在指定的位置插入树节点
                        CheckedListBoxItem checkedListBoxItem = new CheckedListBoxItem(emrNode, true);
                        checkedListBoxControlEmrNode.Items.Insert(insertIndex, checkedListBoxItem);
                        Point scrollPosition = CurrentEditorForm.CurrentEditorControl.AutoScrollPosition;
                        RefreshEmrContent();
                        CurrentEditorForm.CurrentEditorControl.SetAutoScrollPosition(new Point(scrollPosition.X, -scrollPosition.Y));
                    }
                    #endregion
                }
                else
                {
                    #region 修改树节点，并重新刷数据

                    DataTable dtRecord = GetEmrRecordByID(recorddetailID);
                    if (dtRecord.Rows.Count > 0)
                    {
                        EmrNode node = listBoxItem.Value as EmrNode;
                        node.DataRowItem.ItemArray = dtRecord.Rows[0].ItemArray;
                        node.Name = node.DataRowItem["name"].ToString();

                        bool isSorted = SortTreeNodeInList();//对节点进行排序
                        if (listBoxItem.CheckState == CheckState.Checked)
                        {
                            Point scrollPosition = CurrentEditorForm.CurrentEditorControl.AutoScrollPosition;
                            RefreshEmrContent();

                            if (!isSorted)
                            {//如果没有进行排序，则需要定位原先的Scroll的位置
                                CurrentEditorForm.CurrentEditorControl.SetAutoScrollPosition(new Point(scrollPosition.X, -scrollPosition.Y));
                            }
                            else
                            {
                                CurrentEditorForm.SetCurrentElement(displayDateTime);
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除一份病历后，删除在病程预览区中的该病程记录
        /// </summary>
        /// <param name="recorddetailID"></param>
        public void AfterDeleteEmr(string recorddetailID)
        {
            try
            {
                //获取recorddetail.ID == recorddetailID的病历
                var listBoxItem = checkedListBoxControlEmrNode.Items.Cast<CheckedListBoxItem>().Where(item =>
                {
                    EmrNode emrNode = item.Value as EmrNode;
                    if (emrNode != null && emrNode.ID == recorddetailID)
                    {
                        return true;
                    }
                    return false;
                }).FirstOrDefault<CheckedListBoxItem>();
                if (listBoxItem != null)
                {
                    //被选中则重新刷数据
                    if (listBoxItem.CheckState == CheckState.Checked)
                    {
                        listBoxItem.CheckState = CheckState.Unchecked;
                        RefreshEmrContent();
                    }
                    //移除删除的节点
                    checkedListBoxControlEmrNode.Items.Remove(listBoxItem);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 显示CheckedListBox提示信息
        private void checkedListBoxControlEmrNode_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                int itemIndex = checkedListBoxControlEmrNode.IndexFromPoint(checkedListBoxControlEmrNode.PointToClient(Cursor.Position));
                if (itemIndex >= 0)
                {
                    ListBoxItem item = checkedListBoxControlEmrNode.Items[itemIndex];
                    if (item != null)
                    {
                        string name = ((EmrNode)item.Value).Name;

                        if (toolTipControllerEmrNode.GetToolTip(checkedListBoxControlEmrNode) != name)
                        {
                            toolTipControllerEmrNode.SetToolTip(checkedListBoxControlEmrNode, name);
                            toolTipControllerEmrNode.ShowHint(name, new Point(Cursor.Position.X + 20, Cursor.Position.Y - 40));
                        }
                    }
                }
                else
                {
                    toolTipControllerEmrNode.HideHint();
                    toolTipControllerEmrNode.SetToolTip(checkedListBoxControlEmrNode, "");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        /// <summary>
        /// 对TreeList中的节点按照CaptionDateTime进行排序
        /// </summary>
        public bool SortTreeNodeInList()
        {
            try
            {
                List<CheckedListBoxItem> items = checkedListBoxControlEmrNode.Items.Cast<CheckedListBoxItem>().ToList();
                List<CheckedListBoxItem> itemsOrdered = items.OrderBy(item =>
                {
                    EmrNode node = item.Value as EmrNode;
                    return node.DataRowItem["captiondatetime"].ToString();
                }).ToList();
                bool isNeedReSort = false;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] != itemsOrdered[i])
                    {
                        isNeedReSort = true;
                        break;
                    }
                }
                if (isNeedReSort)
                {
                    checkedListBoxControlEmrNode.Items.Clear();
                    foreach (CheckedListBoxItem item in itemsOrdered)
                    {
                        checkedListBoxControlEmrNode.Items.Add(item);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void checkedListBoxControlEmrNode_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Point point = checkedListBoxControlEmrNode.PointToClient(Cursor.Position);
                    int itemIndex = checkedListBoxControlEmrNode.IndexFromPoint(point);
                    if (itemIndex >= 0)
                    {
                        CheckedListBoxItem item = (CheckedListBoxItem)checkedListBoxControlEmrNode.GetItem(itemIndex);
                        if (point.X >= 4 && point.X <= 16)
                        {
                            if (m_FocusedCheckItem != item)
                            {
                                if (item.CheckState == CheckState.Checked)
                                {
                                    item.CheckState = CheckState.Unchecked;
                                }
                                else
                                {
                                    item.CheckState = CheckState.Checked;
                                }
                            }
                            m_FocusedCheckItem = item;
                            return;
                        }
                        if (item != null)
                        {

                            EmrNode emrNode = item.Value as EmrNode;
                            if (emrNode != null)
                            {
                                string captionDateTime = emrNode.DataRowItem["captiondatetime"].ToString();
                                this.CurrentEditorForm.SetCurrentElement(captionDateTime);
                            }
                        }
                        m_FocusedCheckItem = item;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CurrentEditorForm.CurrentEditorControl.EMRDoc.ShowFindForm();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 通过时间选中指定的节点,并使选中的节点显示在可视范围内 Add by wwj 2013-05-01
        /// </summary>
        /// <param name="captionDatetime"></param>
        public void SelectNodeByCaptionDatetime(string captionDatetime)
        {
            try
            {
                int index = -1;//索引
                foreach (CheckedListBoxItem item in checkedListBoxControlEmrNode.Items)
                {
                    EmrNode emrNode = ((ListBoxItem)item).Value as EmrNode;
                    if (emrNode != null)
                    {
                        string captionDateTimeTemp = emrNode.DataRowItem["captiondatetime"].ToString();
                        if (captionDateTimeTemp == captionDatetime)
                        {
                            index = checkedListBoxControlEmrNode.Items.IndexOf(item);
                            checkedListBoxControlEmrNode.SelectedIndex = index;
                            checkedListBoxControlEmrNode.MakeItemVisible(index);
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 检查是否需要刷新病程记录
        /// </summary>
        /// <param name="dt">数据库中该病程科室下所有记录</param>
        /// <param name="focusedNode">获取焦点的病历</param>
        /// <returns></returns>
        private bool CheckIfNeedRefresh(DataTable dt, TreeListNode focusedNode)
        {
            try
            {
                if (null == focusedNode || null == focusedNode.ParentNode || !(focusedNode.ParentNode.Tag is EmrModelDeptContainer))
                {
                    return false;
                }
                List<EmrModel> savedList = CurrentUCEmrInputBody.GetSavedEmrModelsInFloader(CurrentUCEmrInputBody.CurrentTreeList.FocusedNode);
                if (savedList.Count() != dt.Rows.Count)
                {
                    return true;
                }
                else if (null != focusedNode.ParentNode.Nodes && focusedNode.ParentNode.Nodes.Count > 0)
                {
                    foreach (TreeListNode node in focusedNode.ParentNode.Nodes)
                    {
                        if (null != node.Tag && node.Tag is EmrModel)
                        {
                            EmrModel model = node.Tag as EmrModel;
                            if (model.InstanceId != -1 && !dt.AsEnumerable().Any(p => Convert.ToInt32(p["id"]) == model.InstanceId))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    /// <summary>
    /// 复选框列表中的每一项，供CheckedListBox使用
    /// </summary>
    public class EmrNode
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public DataRow DataRowItem { get; set; }

        public EmrNode(DataRow dr)
        {
            Name = dr["name"].ToString();
            ID = dr["id"].ToString();
            DataRowItem = dr;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
