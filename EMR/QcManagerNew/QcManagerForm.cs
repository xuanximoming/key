using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;
using System.Data.OracleClient;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Emr.QcManagerNew
{
    public partial class QcManagerForm : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_app;
        DepartmentList m_DepartmentList;
        OutPatsNoSubmitList m_OutPatsNoSubmitList;
        OutPatsSubmitList m_OutPatsSubmitList;
        public static string deptcode = "";
        public QcManagerForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定科室统计详情一览
        /// </summary>
        private void LoadDepartmentList()
        {
            try
            {
                if (m_DepartmentList == null)
                    m_DepartmentList = new DepartmentList(m_app);

                m_DepartmentList.Dock = DockStyle.Fill;
                xtraTabPageQualityMedicalRecord.Controls.Add(m_DepartmentList);
                xtraTabPageQualityMedicalRecord.PageVisible = true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 绑定未归档病人详情一览
        /// </summary>
        private void LoadNoSubList()
        {
            try
            {
                if (m_OutPatsNoSubmitList == null)
                    m_OutPatsNoSubmitList = new OutPatsNoSubmitList(m_app);

                m_OutPatsNoSubmitList.Dock = DockStyle.Fill;
                xtraTabPageNoSub.Controls.Add(m_OutPatsNoSubmitList);
                xtraTabPageNoSub.PageVisible = true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 绑定未归档病人详情一览
        /// </summary>
        private void LoadSubList()
        {
            try
            {
                if (m_OutPatsSubmitList == null)
                    m_OutPatsSubmitList = new OutPatsSubmitList(m_app);

                m_OutPatsSubmitList.Dock = DockStyle.Fill;
                xtraTabPageSub.Controls.Add(m_OutPatsSubmitList);
                xtraTabPageSub.PageVisible = true;

            }
            catch (Exception)
            {
                throw;
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
                //LoadQualityMedicalRecord();
                LoadDepartmentList();
                LoadNoSubList();
                LoadSubList();
                xtraTabQcManager.SelectedTabPage = xtraTabPageQualityMedicalRecord;
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
                    //LoadWardInfo();
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
                    xtraTabQcManager.TabPages.Remove(this.xtraTabQcManager.SelectedTabPage);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
