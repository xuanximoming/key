using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Core.NurseDocument.Controls;
using DevExpress.Utils;
using System.Drawing.Imaging;
using DrectSoft.Common.Ctrs.DLG;
using System.IO;
using DrectSoft.Common.Eop;
using DrectSoft.Common;

namespace DrectSoft.Core.NurseDocument
{
    public partial class UC_ImageBoard : UserControl
    {
        private ThreeMeasureDrawHepler threeMeasureDrawHepler = null; //XML配置文件操作

        //默认打印A4
        private string m_DefaultPrintSize = "16K";
        public DateTime m_currentDate = DateTime.Now;

        private Image _dataImage;
        public Image DataImage
        {
            get { return _dataImage; }
            set
            {
                _dataImage = value;
                this.labelBottom.Image = _dataImage;
            }
        }
        //private Metafile _mf;
        //public Metafile mf
        //{
        //    get { return _mf; }
        //    set
        //    {
        //        _mf = value;
        //    }
        //}

        WaitDialogForm m_WaitDialog;
        private void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog == null)
                m_WaitDialog = new WaitDialogForm("正在加载数据...", "请稍等...");
            if (!m_WaitDialog.Visible)
                m_WaitDialog.Visible = true;
            m_WaitDialog.Caption = caption;


        }

        private void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }

        public UC_ImageBoard()
        {
            InitializeComponent();
        }

        private void UC_ImageBoard_Load(object sender, EventArgs e)
        {
            InitImageContainer();
            this.VerticalScroll.Value = this.VerticalScroll.Maximum;
        }

        private void InitImageContainer()
        {
            this.labelBottom.Size = ConfigInfo.dataIamgeSize;
            this.labelBottom.ImageAlign = ContentAlignment.MiddleCenter;
            labelBottom.Left = (this.Width - labelBottom.Width) / 2;
        }

        private void UC_ImageBoard_SizeChanged(object sender, EventArgs e)
        {
            labelBottom.Left = (this.Width - labelBottom.Width)/2;
        }

        public void LoadData(decimal currInpatient, DataLoader dataLoader)
        {
            try
            {
                threeMeasureDrawHepler = new ThreeMeasureDrawHepler(currInpatient, dataLoader);

                Size size = ConfigInfo.GetImagePageBound();
                _dataImage = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb); //用于绘制数据表单
                Graphics g = Graphics.FromImage(_dataImage);
                threeMeasureDrawHepler.DrawDataImage(g);
                ConfigInfo.dataIamgeSize = size;
                DataImage = _dataImage;
                g.Save();
                g.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        public void PrintDocument(decimal currInpatient,DataLoader dataLoader)
        {
            try
            {
                threeMeasureDrawHepler = new ThreeMeasureDrawHepler(currInpatient, dataLoader);
                PrintForm printDocumentForm = new PrintForm(threeMeasureDrawHepler);
                printDocumentForm.DefaultPageSize = m_DefaultPrintSize;
                printDocumentForm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量打印
        /// </summary>
        public void PrintAllDocument(decimal currInpatient,DataLoader dataLoader)
        {
            try
            {
                DateTime patientInTime = Convert.ToDateTime(DataLoader.m_InTime);
                PrintAllForm printAllDocumentForm = new PrintAllForm(patientInTime);
                printAllDocumentForm.DefaultPageSize = m_DefaultPrintSize;
                printAllDocumentForm.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = printAllDocumentForm.ShowDialog();
                if (result != DialogResult.OK) return;

                SetWaitDialogCaption("正在获取数据...");
                DateTime dtFrom = printAllDocumentForm.DateTimeFrom;
                DateTime dtTo = printAllDocumentForm.DateTimeTo;
                int fromDay = (dtFrom - patientInTime).Days;
                int toDay = (dtTo - patientInTime).Days;
                int fromWeek = Convert.ToInt32(Math.Floor(fromDay / 7.0f));
                int toWeek = Convert.ToInt32(Math.Floor(toDay / 7.0f));

                m_currentDate = dtFrom;
                dataLoader.m_currentDate = dtFrom;
                List<Bitmap> list = new List<Bitmap>();
                for (int indexWeek = fromWeek; indexWeek <= toWeek; indexWeek++)
                {
                    SetWaitDialogCaption("正在获取" + patientInTime.AddDays(indexWeek * 7).ToString("yyyy-MM-dd") + "的数据...");
                    LoadData(currInpatient, dataLoader);
                    dataLoader.m_currentDate = dataLoader.m_currentDate.AddDays(7);
                    list.Add((Bitmap)_dataImage);
                }
                SetWaitDialogCaption("正在批量打印...");
                printAllDocumentForm.Print(list);
                AddPrintHistory(fromWeek + 1, toWeek + 1, Convert.ToInt32(printAllDocumentForm.spinEditPrintCount.Value));
                HideWaitDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// xll 添加历史打印记录
        /// </summary>
        /// <param name="startpage"></param>
        /// <param name="endpage"></param>
        /// <param name="printpages"></param>
        private void AddPrintHistory(int startpage, int endpage, int printpages)
        {
            try
            {
                PrintHistoryEntity printHistoryEntity = new PrintHistoryEntity();
                printHistoryEntity.PrintRecordFlow = threeMeasureDrawHepler.dataLoader.CurrentPat.ToString();
                printHistoryEntity.StartPage = startpage;
                printHistoryEntity.EndPage = endpage;
                printHistoryEntity.PrintPages = printpages;
                printHistoryEntity.PrintType = "2";
                DrectSoft.Common.PrintHistoryHistory.AddrintHistory(printHistoryEntity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
