using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Xml;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.Permission
{
    public partial class PermissionMainForm : DevBaseForm
    {
        private IEmrHost m_App;

        public PermissionMainForm()
        {
            InitializeComponent();
            //UCJobManager test = new UCJobManager();
            //xtraTabPageJob.Controls.Add(test);
            //test.Dock = DockStyle.Fill;
        }

        public PermissionMainForm(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_App = app;

                //加载岗位管理页面
                UCJobsManager jobsManager = new UCJobsManager(app);
                xtraTabPageJobs.Controls.Add(jobsManager);
                jobsManager.Dock = DockStyle.Fill;

                //加载员工管理页面
                UCUsersManager usersManager = new UCUsersManager(app);
                xtraTabPageUsers.Controls.Add(usersManager);
                usersManager.Dock = DockStyle.Fill;

                //加载签名图片管理页面
                UCPicSign picSignManager = new UCPicSign(app);
                xtraTabPagePicSign.Controls.Add(picSignManager);
                picSignManager.Dock = DockStyle.Fill;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private void PermissionMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_App == null)
                {
                    m_App.CustomMessageBox.MessageShow("加载插件失败，请联系管理员！");
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}