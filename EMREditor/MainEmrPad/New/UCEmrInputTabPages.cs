using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Document;

namespace DrectSoft.Core.MainEmrPad.New
{
    public partial class UCEmrInputTabPages : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_App;
        Inpatient m_CurrentInpatient;

        /// <summary>
        /// 当前界面选中的编辑器
        /// </summary>
        public EditorForm CurrentForm
        {
            get
            {
                try
                {
                    if (xtraTabControlEmrContent.SelectedTabPage != null)
                    {
                        if (xtraTabControlEmrContent.SelectedTabPage.Controls.Count > 0)
                        {
                            return xtraTabControlEmrContent.SelectedTabPage.Controls[0] as EditorForm;
                        }
                    }
                    return null;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 病案首页
        /// </summary>
        public XtraTabPage tabPageIEMMainPage
        {
            get
            {
                return xtraTabPageIEMMainPage;
            }
        }

        /// <summary>
        /// tab容器
        /// </summary>
        public XtraTabControl tabControlEmr
        {
            get
            {
                return xtraTabControlEmrContent;
            }
        }

        private UCEmrInputBody CurrentUCEmrInputBody
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

        /// <summary>
        /// 对外公开XtraTabControl
        /// </summary>
        public XtraTabControl CurrentTabControl
        {
            get
            {
                return xtraTabControlEmrContent;
            }
        }

        public UCEmrInputTabPages()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public UCEmrInputTabPages(Inpatient inpatient, IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_CurrentInpatient = inpatient;
                m_App = app;
                SelectedPages.Add(xtraTabPageIEMMainPage);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 添加病历预览页面
        /// </summary>
        public void AddEmrInputPreView(UCEmrInputPreView preView, XtraTabPage page)
        {
            try
            {
                if (null == preView)
                {
                    return;
                }
                preView.Dock = DockStyle.Bottom;
                page.Controls.Add(preView);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 移除病历预览页面
        /// </summary>
        public void RemoveEmrInputPreView(UCEmrInputPreView preView, XtraTabPage page)
        {
            try
            {
                if (null == preView)
                {
                    return;
                }
                page.Controls.Remove(preView);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加拆分控件
        /// </summary>
        public void AddSplitterControl(XtraTabPage page)
        {
            try
            {
                SplitterControl splitterControl = new SplitterControl();
                splitterControl.Dock = DockStyle.Bottom;
                splitterControl.MouseEnter += new EventHandler(splitterControl_MouseEnter);
                splitterControl.MouseLeave += new EventHandler(splitterControl_MouseLeave);
                splitterControl.MouseDown += new MouseEventHandler(splitterControl_MouseDown);
                //this.Controls.Add(splitterControl);
                page.Controls.Add(splitterControl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void splitterControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                toolTipControllerSplit.HideHint();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void splitterControl_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                Point pt = Cursor.Position;
                InitToolTipControl("提示", "通过鼠标进行上下拖拽，控制上方编辑区和下方预览区界面的大小", pt, 0);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void splitterControl_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                toolTipControllerSplit.HideHint();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitToolTipControl(string title, string displayText, Point pt, int imageIndex)
        {
            try
            {
                ToolTipControllerShowEventArgs args = toolTipControllerSplit.CreateShowArgs();
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
                toolTipControllerSplit.ImageIndex = imageIndex;
                toolTipControllerSplit.ShowHint(args, pt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 大纲视图
        public void GetBrief(TreeView treeListEmrView)
        {
            try
            {
                if (CurrentForm != null)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.GetBrief(treeListEmrView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AfterSelected(TreeNode node)
        {
            try
            {
                if (node != null)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.Content.AutoClearSelection = true;
                    CurrentForm.CurrentEditorControl.EMRDoc.Content.MoveSelectStart((node.Tag as ZYFixedText).FirstElement);
                    CurrentForm.CurrentEditorControl.ScrollViewtopToCurrentElement();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region SelectedPageChanged 事件

        public List<XtraTabPage> SelectedPages = new List<XtraTabPage>();
        private void xtraTabControlEmrContent_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            try
            {
                SelectedPageChanged(e.Page);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void SelectedPageChanged(XtraTabPage page)
        {
            try
            {
                SelectedPages.Add(page);
                CurrentUCEmrInputBody.SelectedPageChanged(page);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region CloseButtonClick Event
        private void xtraTabControlEmrContent_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                SelectedPages.RemoveAll(page => { return page == xtraTabControlEmrContent.SelectedTabPage; });

                UnRegisterEvent();
                CurrentUCEmrInputBody.ClickTabPageCloseButton();

                if (SelectedPages.Count > 0)
                {
                    xtraTabControlEmrContent.SelectedTabPage = SelectedPages[SelectedPages.Count - 1];
                    SelectedPageChanged(xtraTabControlEmrContent.SelectedTabPage);
                }

                RegisterEvent();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }
        #endregion

        #region 添加移除

        public void RegisterEvent()
        {
            try
            {
                xtraTabControlEmrContent.SelectedPageChanged -= new TabPageChangedEventHandler(xtraTabControlEmrContent_SelectedPageChanged);
                xtraTabControlEmrContent.SelectedPageChanged += new TabPageChangedEventHandler(xtraTabControlEmrContent_SelectedPageChanged);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UnRegisterEvent()
        {
            try
            {
                xtraTabControlEmrContent.SelectedPageChanged -= new TabPageChangedEventHandler(xtraTabControlEmrContent_SelectedPageChanged);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        /// 移除所有tab页(不触发事件)
        /// </summary>
        public void RemoveAllTabPageUnEvent()
        {
            try
            {
                xtraTabControlEmrContent.SelectedPageChanged -= new TabPageChangedEventHandler(xtraTabControlEmrContent_SelectedPageChanged);
                xtraTabControlEmrContent.CloseButtonClick -= new EventHandler(xtraTabControlEmrContent_CloseButtonClick);
                RemoveAllTabPage();
                xtraTabControlEmrContent.SelectedPageChanged += new TabPageChangedEventHandler(xtraTabControlEmrContent_SelectedPageChanged);
                xtraTabControlEmrContent.CloseButtonClick += new EventHandler(xtraTabControlEmrContent_CloseButtonClick);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 移除所有tab页
        /// </summary>
        public void RemoveAllTabPage()
        {
            try
            {
                int pageCount = xtraTabControlEmrContent.TabPages.Count;
                if (pageCount > 0)
                {
                    if (pageCount > 1)
                    {
                        for (int i = pageCount - 1; i > 0; i--)
                        {
                            xtraTabControlEmrContent.TabPages.RemoveAt(i);
                        }
                    }
                    xtraTabControlEmrContent.TabPages[0].Controls.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RegisterPageEvent()
        {
            try
            {
                xtraTabControlEmrContent.SelectedPageChanged -= new TabPageChangedEventHandler(xtraTabControlEmrContent_SelectedPageChanged);
                xtraTabControlEmrContent.SelectedPageChanged += new TabPageChangedEventHandler(xtraTabControlEmrContent_SelectedPageChanged);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UnRegisterPageEvent()
        {
            try
            {
                xtraTabControlEmrContent.SelectedPageChanged -= new TabPageChangedEventHandler(xtraTabControlEmrContent_SelectedPageChanged);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
