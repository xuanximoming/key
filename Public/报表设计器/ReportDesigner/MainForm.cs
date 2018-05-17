using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using System.Data;
using DrectSoft.Common.Report;

namespace ReportDesigner
{
    public partial class MainForm : XtraForm
    {
        public XRDesignPanel ActiveXRDesignPanel
        {
            get
            {
                return this.xrDesignMdiController1.ActiveDesignPanel;
            }
        }

        public MainForm()
        {
            this.InitializeComponent();
            BarItem item = new BarLookAndFeelListItem(UserLookAndFeel.Default);
            this.xrDesignBarManager1.Items.Add(item);
            this.bsiLookAndFeel.AddItem(item);
        }
        /// <summary>
        /// 打开报表
        /// </summary>
        /// <param name="newReport"></param>
        public void OpenReport(XtraReport newReport)
        {
            this.xrDesignMdiController1.OpenReport(newReport);
        }
        /// <summary>
        /// 新增报表
        /// </summary>
        /// <param name="ds"></param>
        public void CreateNewReport(DataSet ds)
        {
            this.xrDesignMdiController1.CreateNewReport();
            this.xrDesignMdiController1.ActiveDesignPanel.Report.DataSource = ds;
        }
        /// <summary>
        /// 创建加载等待信息
        /// </summary>
        /// <param name="caption"></param>
        public void SetWaitDialogCaption(string caption)
        {
            if (this.m_WaitDialog == null)
            {
                this.m_WaitDialog = new WaitDialogForm("正在读取数据……", "请稍等。");
            }
            if (!this.m_WaitDialog.Visible)
            {
                this.m_WaitDialog.Visible = true;
            }
            this.m_WaitDialog.Caption = caption;
        }
        /// <summary>
        /// 隐藏加载信息
        /// </summary>
        public void HideWaitDialog()
        {
            if (this.m_WaitDialog != null)
            {
                this.m_WaitDialog.Hide();
            }
        }
        /// <summary>
        /// 新建按钮的触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NEW_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormDataSoruce formDataSoruce = new FormDataSoruce();
            if (formDataSoruce.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.SetWaitDialogCaption("正在创建报表");
                this.CreateNewReport(formDataSoruce.ReportDataSoruce);
                this.HideWaitDialog();
            }
        }
    }
}
