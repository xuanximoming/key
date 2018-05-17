using System;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.DoctorTasks
{
    /// <summary>
    /// 新闻通知主界面
    /// </summary>
    public partial class NewCenter : DevExpress.XtraEditors.XtraForm, IStartPlugIn
    {
        public NewCenter()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region IStartPlugIn 成员

        public IPlugIn Run(IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);

            return plg;
        }

        #endregion

        private void NewCenter_Load(object sender, EventArgs e)
        {
            try
            {
                string ServerURL = BasicSettings.GetStringConfig("YindanWebServerUrL");
                webBrowser1.Navigate("http://" + ServerURL + "/Applications/Manage/NewsDefault.aspx");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}