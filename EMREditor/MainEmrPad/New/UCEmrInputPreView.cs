using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Eop;
using DevExpress.Utils;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Service;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad.New
{
    /// <summary>
    /// 文书预览功能
    /// </summary>
    public partial class UCEmrInputPreView : DevExpress.XtraEditors.XtraUserControl
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

        private UCEmrInputPreViewInner m_PreViewInner;
        public UCEmrInputPreViewInner PreViewInner
        {
            get { return m_PreViewInner;}
            set { m_PreViewInner = value;}
        }

        WaitDialogForm m_waitForm;

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
        private UCEmrInputBody m_CurrentUCEmrInputBody;

        public SimpleButton ButtonRestore
        {
            get { return simpleButtonRestore; }
        }

        public SimpleButton ButtonUp
        {
            get { return simpleButtonUp; }
        }

        public SimpleButton ButtonDown
        {
            get { return simpleButtonDown; }
        }

        /// <summary>
        /// 第一次页面打开时的高度
        /// </summary>
        public int FirstHeight = 180;
        /// <summary>
        /// 点击恢复预览区时的高度
        /// </summary>
        public int DefaultHeight = 300;
        #endregion

        #region .ctor

        public UCEmrInputPreView(string changeID, string recordDetailID)
        {
            try
            {
                m_waitForm = new WaitDialogForm("正在加载病历预览区...", "请稍候");
                InitializeComponent();
                Init(changeID, recordDetailID);
                InitSize();
                this.LocatedDailyEmrForEditEvent += new Action<string, string>(UCEmrInputPreView_LocatedDailyEmrForEditEvent);
                m_waitForm.Hide();
            }
            catch (Exception ex)
            {
                DS_Common.HideWaitDialog(m_waitForm);
                MessageBox.Show(ex.Message);
            }
        }

        private void Init(string changeID, string recordDetailID)
        {
            try
            {
                m_PreViewInner = new UCEmrInputPreViewInner(changeID, recordDetailID);
                m_PreViewInner.Dock = DockStyle.Fill;
                panelControlBody.Controls.Add(m_PreViewInner);
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

        #endregion

        #region Load
        private void UCEmrInputPreView_Load(object sender, EventArgs e)
        {
            try
            {
                simpleButtonRestore.Enabled = false;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region Event
        private void panelControlTop_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                ResizeButton();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void ResizeButton()
        {
            try
            {
                simpleButtonUp.Width = panelControlTop.Width / 3;
                simpleButtonDown.Width = panelControlTop.Width / 3;
                simpleButtonRestore.Width = panelControlTop.Width / 3;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region ButtonClickEvent
        private void simpleButtonUp_Click(object sender, EventArgs e)
        {
            try
            {
                BtnUp();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void BtnUp()
        {
            try
            {
                if (this.Parent != null)
                {
                    this.Height = this.Parent.Height - 5;
                    simpleButtonRestore.Enabled = true;
                    simpleButtonRestore.Text = "恢复病程编辑区";
                    simpleButtonRestore.ToolTipTitle = simpleButtonRestore.Text;
                    simpleButtonRestore.ToolTip = "点击后恢复病程编辑区至原始状态，病程编辑区域在上方，病程预览区在下方";
                    simpleButtonUp.Enabled = false;
                    simpleButtonDown.Enabled = true;
                    ResizeButton();
                    m_PreViewInner.CurrentEditorForm.Focus();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void simpleButtonDown_Click(object sender, EventArgs e)
        {
            try
            {
                BtnDown();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void BtnDown()
        {
            try
            {
                if (this.Parent != null)
                {
                    this.Height = panelControlTop.Height;
                    simpleButtonRestore.Enabled = true;
                    simpleButtonRestore.Text = "恢复病程预览区";
                    simpleButtonRestore.ToolTipTitle = simpleButtonRestore.Text;
                    simpleButtonRestore.ToolTip = "点击后恢复病程预览区至原始状态，病程编辑区域在上方，病程预览区在下方";
                    simpleButtonUp.Enabled = true;
                    simpleButtonDown.Enabled = false;
                    ResizeButton();
                    m_PreViewInner.CurrentEditorForm.Focus();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void simpleButtonRestore_Click(object sender, EventArgs e)
        {
            try
            {
                Restore();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void Restore()
        {
            try
            {
                if (this.Parent != null)
                {
                    this.Height = DefaultHeight;
                    simpleButtonRestore.Enabled = false;
                    simpleButtonUp.Enabled = true;
                    simpleButtonDown.Enabled = true;
                    ResizeButton();
                    m_PreViewInner.CurrentEditorForm.Focus();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        private void InitToolTipControl(string title, string displayText, Point pt, int imageIndex)
        {
            try
            {
                ToolTipControllerShowEventArgs args = toolTipControllerBtnRestore.CreateShowArgs();
                args.IconSize = ToolTipIconSize.Large;
                args.Rounded = true;
                args.RoundRadius = 7;
                args.Show = true;
                args.ShowBeak = true;
                args.ToolTipLocation = ToolTipLocation.TopRight;
                args.Title = title;
                args.ToolTip = displayText;
                args.IconType = ToolTipIconType.Information;
                args.IconSize = ToolTipIconSize.Large;
                toolTipControllerBtnRestore.ImageIndex = imageIndex;
                toolTipControllerBtnRestore.ShowHint(args, pt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 定位节点
        /// </summary>
        /// <param name="changeID"></param>
        /// <param name="captionDateTime"></param>
        private void UCEmrInputPreView_LocatedDailyEmrForEditEvent(string changeID, string captionDateTime)
        {
            try
            {
                CurrentUCEmrInputBody.SetNodeFocused(CurrentUCEmrInputBody.CurrentTreeList.Nodes, changeID, captionDateTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 在预览区中定位后对外公开的事件
        /// <summary>
        /// 在预览区中定位后对外公开的事件
        /// </summary>
        public event Action<string, string> LocatedDailyEmrForEditEvent;
        public void OnLocateDailyEmrForEdit(string changeID, string captionDateTime)
        {
            try
            {
                if (LocatedDailyEmrForEditEvent != null)
                {
                    LocatedDailyEmrForEditEvent(changeID, captionDateTime);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 重置预览区域
        /// 注：修复窗体最大化最小化时预览区不变的问题
        /// </summary>
        public void ResetSize()
        {
            try
            {
                if (simpleButtonUp.Enabled == false)
                {
                    BtnUp();
                }
                else if (simpleButtonDown.Enabled == false)
                {
                    BtnDown();
                }
                else if (simpleButtonRestore.Enabled == false)
                {
                    Restore();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
