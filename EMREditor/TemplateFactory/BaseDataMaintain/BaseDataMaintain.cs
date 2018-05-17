using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.SysTableEdit;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DevExpress.XtraTab;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    public partial class BaseDataMaintain : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_app;
        /// <summary>
        /// 病种ICD-10页面
        /// </summary>
        UCEditDiagnosis m_UCEditDiagnosis;

        /// <summary>
        /// 中医诊断
        /// </summary>
        UCEditDiagnosisOfChinese m_UCEditDiagnosisOfChinese;

        /// <summary>
        /// 科室常用病种维护
        /// </summary>
        UCEditDiagnosisToDept m_UCEditDiagnosisToDept;

        /// <summary>
        /// 复用项目
        /// </summary>
        UCEditModelFactory m_UCEditModelFactory;

        public bool IsClose = false;
        /// <summary>
        /// 诊断按钮事件
        /// </summary>
        UCDiagnosisButton m_UCDiagnosisButton;

        /// <summary>
        /// 手术信息维护
        /// </summary>
        UCEditOperInfo m_UCEditOperInfo;

        /// <summary>
        /// 并发症维护
        /// </summary>
        UCEditComplications m_UCEditComplications;

        public BaseDataMaintain(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            ucDictionary1.Host = m_app;
            ucMacro1.Host = m_app;
            ucSpecialCharacter1.Host = m_app;
            this.ActiveControl = ucDictionary1;
        }

        public BaseDataMaintain()
        {
            InitializeComponent();
        }

        private void simpleButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
            IsClose = true;
        }

        private void simpleButtonPYWBUpdate_Click(object sender, EventArgs e)
        {
            ucDictionary1.UpdatePYWB();
        }

        /// <summary>
        /// tab页切换事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == null) return; //xll 添加
                if (xtraTabControl1.SelectedTabPage.Name == "tabPageDiag")
                {
                    if (m_UCEditDiagnosis == null)
                    {
                        m_UCEditDiagnosis = new UCEditDiagnosis(m_app);
                        tabPageDiag.Controls.Add(m_UCEditDiagnosis);
                        m_UCEditDiagnosis.Dock = DockStyle.Fill;
                    }
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "tabPageDiagChinese")
                {
                    if (m_UCEditDiagnosisOfChinese == null)
                    {
                        m_UCEditDiagnosisOfChinese = new UCEditDiagnosisOfChinese(m_app);
                        tabPageDiagChinese.Controls.Add(m_UCEditDiagnosisOfChinese);
                        m_UCEditDiagnosisOfChinese.Dock = DockStyle.Fill;
                    }
                }
                else if (xtraTabControl1.SelectedTabPage.Name == "TabPageDeptDiag")
                {
                    if (m_UCEditDiagnosisToDept == null)
                    {
                        m_UCEditDiagnosisToDept = new UCEditDiagnosisToDept(m_app);
                        TabPageDeptDiag.Controls.Add(m_UCEditDiagnosisToDept);
                        m_UCEditDiagnosisToDept.Dock = DockStyle.Fill;
                    }
                }
                //点击复用项目
                else if (xtraTabControl1.SelectedTabPage.Name == "TabPageModel")
                {
                    if (m_UCEditModelFactory == null)
                    {
                        m_UCEditModelFactory = new UCEditModelFactory(m_app);
                        TabPageModel.Controls.Add(m_UCEditModelFactory);
                        m_UCEditModelFactory.Dock = DockStyle.Fill;
                    }
                }
                //诊断按钮事件
                else if (xtraTabControl1.SelectedTabPage.Name == "TabPageDiagButton")
                {
                    if (m_UCDiagnosisButton == null)
                    {
                        m_UCDiagnosisButton = new UCDiagnosisButton(m_app);
                        TabPageDiagButton.Controls.Add(m_UCDiagnosisButton);
                        m_UCDiagnosisButton.Dock = DockStyle.Fill;
                    }
                }

                //点击手术信息维护
                else if (xtraTabControl1.SelectedTabPage.Name == "XTOPERPAGE")
                {
                    if (m_UCEditOperInfo == null)
                    {
                        m_UCEditOperInfo = new UCEditOperInfo(m_app);
                        XTOPERPAGE.Controls.Add(m_UCEditOperInfo);
                        m_UCEditOperInfo.Dock = DockStyle.Fill;
                    }
                }
                //并发症维护
                else if (xtraTabControl1.SelectedTabPage.Name == "xtraTabPage3")
                {
                    if (m_UCEditComplications == null)
                    {
                        m_UCEditComplications = new UCEditComplications(m_app);
                        xtraTabPage3.Controls.Add(m_UCEditComplications);
                        m_UCEditComplications.Dock = DockStyle.Fill;
                    }
                }
                else { }
                xtraTabControl1.SelectedTabPage.Controls[0].Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// xll 将该窗体变成主窗体
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost host)
        {
            try
            {
                IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
                m_app = host;
                JustShowOpeAndDia();
                return plg;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void JustShowOpeAndDia()
        {

            foreach (XtraTabPage item  in xtraTabControl1.TabPages)
            {
                item.PageVisible = false;
            }
            tabPageDiag.PageVisible = tabPageDiagChinese.PageVisible = XTOPERPAGE.PageVisible = true;
            simpleButtonClose.Visible = false;
        }
    }
}