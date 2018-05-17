using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public partial class ReportCardApply : DevBaseForm
    {
        IEmrHost m_App;
        string m_NoOfInpat;

        UCReportCard CurrentReprotCard
        {
            get
            {
                if (panelReportCard.Controls.Count == 1)
                {
                    UCReportCard card = panelReportCard.Controls[0] as UCReportCard;
                    if (card != null)
                    {
                        return card;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public ReportCardApply()
        {
            InitializeComponent();
        }

        public ReportCardApply(IEmrHost app, string noofinpat)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            m_App = app;
            m_NoOfInpat = noofinpat;
        }

        private void ReportCardApply_Load(object sender, EventArgs e)
        {
            AddUCReportCard();
        }

        private void AddUCReportCard()
        {
            UCReportCard card = new UCReportCard(m_App);
            //card.Dock = DockStyle.Fill;
            panelReportCard.Controls.Add(card);

            card.LoadPage(m_NoOfInpat, "2", "1");
        }

        /// <summary>
        /// 退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Close();
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
        private void barLargeButtonItemSubmit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CurrentReprotCard.Submit())
                {
                    this.Close();
                }
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
        private void barLargeButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CurrentReprotCard.Cancel())
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CurrentReprotCard.Save())
                {

                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 清空事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-24</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CurrentReprotCard.ClearPage();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}
