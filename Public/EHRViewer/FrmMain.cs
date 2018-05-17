using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using System;

namespace DrectSoft.Core.EHRViewer
{
    public partial class FrmMain : DevBaseForm
    {
        string m_url = string.Empty;

        public FrmMain()
        {
            InitializeComponent();
        }

        public FrmMain(string url)
        {
            InitializeComponent();
            m_url = url;
        }

        private void DrectDevButtonQurey1_Click(object sender, EventArgs e)
        {
            string strUrl = txtUrl.Text;
            try
            {
                webBrowser1.Navigate(strUrl);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message);
            }
        }

        private void barButtonBack_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            webBrowser1.Navigate("");
        }

        private void barButtonGo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            webBrowser1.Navigate("");
        }

        //读卡
        private void barButtonLoadID_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Text += ICHelper.ReadPersonalBasicInfo().pid;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message);
            }

        }
    }
}