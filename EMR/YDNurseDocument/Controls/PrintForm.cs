using DevExpress.Utils;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace DrectSoft.Core.NurseDocument.Controls
{
    public partial class PrintForm : DevBaseForm
    {
        PrintDocument m_PrintDocument;
        PaperSize p = null;
        Metafile m_Image = null;
        int m_Width;
        int m_Height;
        public ThreeMeasureDrawHepler threeMeasureDrawHepler = null;

        private const int c_Dpi = 96;

        /// <summary>
        /// 默认打印纸张
        /// </summary>
        public string DefaultPageSize
        {
            set
            {
                m_DefaultPageSize = value;
            }
        }
        string m_DefaultPageSize;

        public PrintForm(ThreeMeasureDrawHepler threeMeasureDrawHepler)
        {
            try
            {
                //给变量赋值 xll 2013-06-21
                this.threeMeasureDrawHepler = threeMeasureDrawHepler;
                InitializeComponent();
                this.FormClosed += new FormClosedEventHandler(PrintForm_FormClosed);
                Bitmap _dataImage = new Bitmap(ConfigInfo.dataIamgeSize.Width, ConfigInfo.dataIamgeSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb); //用于绘制数据表单
                Graphics g = Graphics.FromImage(_dataImage);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, ConfigInfo.dataIamgeSize.Width, ConfigInfo.dataIamgeSize.Height);
                m_Image = new Metafile(ConfigInfo.MetafilePath, g.GetHdc(), rect, MetafileFrameUnit.Pixel);
                Graphics gg = Graphics.FromImage(m_Image);
                threeMeasureDrawHepler.DrawDataImage(gg);
                gg.Save();
                gg.Dispose();
                ReDrawImage((Metafile)System.Drawing.Image.FromFile(ConfigInfo.MetafilePath));
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        void PrintForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                File.Delete(ConfigInfo.MetafilePath);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void InitPreviewControl()
        {
            panelContainer.BackColor = Color.White;
            m_PrintDocument = new PrintDocument();
            m_PrintDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            m_PrintDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            ReDrawImage(this.m_Image);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void ReDrawImage(Metafile img)
        {
            try
            {
                //m_Image = img;
                pictureBoxNurseDocument.Width = img.Width;
                pictureBoxNurseDocument.Height = img.Height;
                m_Width = img.Width;
                m_Height = img.Height;
                System.Drawing.Image image = System.Drawing.Image.FromFile(ConfigInfo.MetafilePath);
                System.Drawing.Image bmp = new System.Drawing.Bitmap(image);
                img.Dispose();

                pictureBoxNurseDocument.BackgroundImage = bmp;
                pictureBoxNurseDocument.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            InitPrinterList();
            InitPercent();
            InitPageSize();
            InitPreviewControl();
            simpleButtonPrint.Focus();
        }

        private void PrintForm_Resize(object sender, EventArgs e)
        {
            RelocationPictureBox();
        }

        private void RelocationPictureBox()
        {
            pictureBoxNurseDocument.Location = new Point((this.Width - pictureBoxNurseDocument.Width) / 2, pictureBoxNurseDocument.Location.Y);
        }

        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {

            Graphics g = e.Graphics;

            if (m_Image != null)
            {
                string pageType = lookUpEditPageSize.Text;

                Size imageSize = new Size(ConfigInfo.dataIamgeSize.Width, ConfigInfo.dataIamgeSize.Height);

                if (pageType == "A4")
                {
                    g.DrawImage(System.Drawing.Image.FromFile(ConfigInfo.MetafilePath),
                            new System.Drawing.Rectangle((p.Width - imageSize.Width) / 2, (p.Height - imageSize.Height) / 2, Convert.ToInt32(imageSize.Width), Convert.ToInt32(imageSize.Height)),
                            0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);
                }
                else if (pageType == "B5")
                {
                    g.DrawImage(System.Drawing.Image.FromFile(ConfigInfo.MetafilePath),
                            new System.Drawing.Rectangle((p.Width - imageSize.Width) / 2, (p.Height - imageSize.Height) / 2, Convert.ToInt32(imageSize.Width * 0.92), Convert.ToInt32(imageSize.Height * 0.92)),
                            0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);
                }
                else if (pageType == "16K")
                {
                    g.DrawImage(System.Drawing.Image.FromFile(ConfigInfo.MetafilePath),
        new System.Drawing.Rectangle((p.Width - imageSize.Width) / 2, (p.Height - imageSize.Height) / 2, Convert.ToInt32(imageSize.Width * 1), Convert.ToInt32(imageSize.Height * 1)),
        0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(System.Drawing.Image.FromFile(ConfigInfo.MetafilePath), 0, 0);
                }
            }
            e.HasMorePages = false;
        }

        private void simpleButtonPrintDialog_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = m_PrintDocument;
            printDialog.ShowDialog();
        }

        private void simpleButtonPageSetupDialog_Click(object sender, EventArgs e)
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.Document = m_PrintDocument;
            pageSetupDialog.ShowDialog();
        }

        private void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            string pageType = lookUpEditPageSize.EditValue.ToString();
            m_PrintDocument.PrinterSettings.PrinterName = lookUpEditPrinter.EditValue.ToString();

            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.Document = m_PrintDocument;
            //PaperSize p = new PaperSize("16K", 275, 457);//默认16K的纸
            //xll 2013-2-6  2013--03-05
            p = new PaperSize("16K", 785, 1012);//默认16K的纸 调试获得 不是太准确

            foreach (PaperSize ps in pageSetupDialog.Document.PrinterSettings.PaperSizes)
            {
                if (ps.Kind.ToString().ToUpper().Equals(pageType.ToUpper()))//这里设置纸张大小,但必须是定义好的  
                    p = ps;
            }

            if (p != null)
            {
                pageSetupDialog.Document.DefaultPageSettings.PaperSize = p;
            }
            else
            {
                foreach (PaperSize ps in pageSetupDialog.Document.PrinterSettings.PaperSizes)
                {
                    if (ps.Kind.ToString().ToUpper().Equals("A4"))//这里设置纸张大小,但必须是定义好的  
                        p = ps;
                }

                // Common.Ctrs.DLG.MessageBox.Show("打印机不支持" + pageType + "打印!");
                // return;
            }

            pageSetupDialog.Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            //if (pageSetupDialog.ShowDialog() == DialogResult.OK)
            {
                //开始打印
                for (int i = 0; i < Convert.ToInt32(spinEditPrintCount.EditValue); i++)
                {
                    m_PrintDocument.Print();
                }
            }
            AddPrintHistory(DataLoader.WeekIndex +
            1, DataLoader.WeekIndex + 1, Convert.ToInt32(spinEditPrintCount.Value));

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



        /// <summary>
        /// 初始化打印机列表
        /// </summary>
        private void InitPrinterList()
        {
            DataTable dataTablePrinter = new DataTable();
            dataTablePrinter.Columns.Add("PrintName");

            PrintDocument prtdoc = new PrintDocument();
            string strDefaultPrinter = prtdoc.PrinterSettings.PrinterName;//獲取默認的打印機名
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                DataRow dr = dataTablePrinter.NewRow();
                dr["PrintName"] = strPrinter;
                dataTablePrinter.Rows.Add(dr);
            }
            lookUpEditPrinter.Properties.DataSource = dataTablePrinter;
            lookUpEditPrinter.Properties.DisplayMember = "PrintName";
            lookUpEditPrinter.Properties.ValueMember = "PrintName";
            lookUpEditPrinter.EditValue = strDefaultPrinter;
        }

        private void InitPercent()
        {
            DataTable dataTablePercent = new DataTable();
            dataTablePercent.Columns.Add("Percent");

            for (int i = 50; i <= 100; i++)
            {
                DataRow dr = dataTablePercent.NewRow();
                dr["Percent"] = i + "%";
                dataTablePercent.Rows.Add(dr);
            }
            lookUpEditPercent.Properties.DataSource = dataTablePercent;
            lookUpEditPercent.Properties.DisplayMember = "Percent";
            lookUpEditPercent.Properties.ValueMember = "Percent";
            lookUpEditPercent.EditValue = "100%";
        }

        private void InitPageSize()
        {
            DataTable dataTablePageSize = new DataTable();
            dataTablePageSize.Columns.Add("PageName");
            dataTablePageSize.Columns.Add("PageValue");

            DataRow dr = dataTablePageSize.NewRow();
            dr["PageName"] = "16K";
            dr["PageValue"] = "16K";
            dataTablePageSize.Rows.Add(dr);

            dr = dataTablePageSize.NewRow();
            dr["PageName"] = "A4";
            dr["PageValue"] = "A4";
            dataTablePageSize.Rows.Add(dr);

            dr = dataTablePageSize.NewRow();
            dr["PageName"] = "B5";
            dr["PageValue"] = "B5 (JIS)";
            dataTablePageSize.Rows.Add(dr);



            lookUpEditPageSize.Properties.DataSource = dataTablePageSize;
            lookUpEditPageSize.Properties.DisplayMember = "PageName";
            lookUpEditPageSize.Properties.ValueMember = "PageValue";
            m_DefaultPageSize = ConfigInfo.GetDefaultPrintSize();
            if (!string.IsNullOrEmpty(m_DefaultPageSize))//设置默认纸张
            {
                lookUpEditPageSize.EditValue = m_DefaultPageSize;
            }
            else
            {
                lookUpEditPageSize.EditValue = "16K";
            }
        }

        private void GetDefaultPageSize()
        {

        }

        private void pictureBoxNurseDocument_Click(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void panelContainer_Click(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void lookUpEditPercent_EditValueChanged(object sender, EventArgs e)
        {
            //m_Width = m_Image.Width * Convert.ToInt32(lookUpEditPercent.EditValue.ToString().TrimEnd('%')) / 100;
            //m_Height = m_Image.Height * Convert.ToInt32(lookUpEditPercent.EditValue.ToString().TrimEnd('%')) / 100;
            //pictureBoxNurseDocument.Width = m_Width;
            //pictureBoxNurseDocument.Height = m_Height;
            RelocationPictureBox();
        }

        private void pictureBoxNurseDocument_Paint(object sender, PaintEventArgs e)
        {
        }

        private void simpleButtonExport_Click(object sender, EventArgs e)
        {

            try
            {
                SaveFileDialog file = new SaveFileDialog();
                file.InitialDirectory = ".\\PrintImage";
                file.Filter = "Pdf Files(*.pdf)|*.pdf";
                file.FileName = "体温单";
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    WaitDialogForm m_WaitDialog = new WaitDialogForm("正在导出PDF文件...", "请稍后");
                    Document doc = new Document();
                    PdfWriter.GetInstance(doc, new FileStream(file.FileName, FileMode.OpenOrCreate));
                    doc.Open();
                    Bitmap bt = new Bitmap(ConfigInfo.MetafilePath);
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(bt, BaseColor.WHITE);
                    // iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(m_Image, BaseColor.WHITE);
                    image.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                    image.ScalePercent(70);
                    doc.Add(image);
                    doc.Close();
                    m_WaitDialog.Close();
                    m_WaitDialog.Dispose();
                    MessageBox.Show("导出成功！");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }


            //SaveFileDialog file = new SaveFileDialog();
            //file.InitialDirectory = "D:\\";
            //file.Filter = "Image   Files(*.jpg)|*.jpg";
            //file.FileName = "三测单";
            //if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    if (m_Image != null)
            //    {
            //        //string filePath = file.FileName;
            //        //Bitmap bmp = new Bitmap(m_Image.Width, m_Image.Height);
            //        //Graphics g = Graphics.FromImage(bmp);
            //        //Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //        //Metafile mf = new Metafile(filePath, g.GetHdc(), rect, MetafileFrameUnit.Pixel);
            //        //g = Graphics.FromImage(mf);
            //        //g.DrawImage(m_MetaFile, new Point(0, 0));
            //        //g.Save();
            //        //g.Dispose();
            //        //mf.Dispose();

            //        //徐亮亮 2012-11-28
            //        //m_Image.Save(file.FileName);

            //    }
            //}
        }

        private void lookUpEditPrinter_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_Width = ConfigInfo.dataIamgeSize.Width * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
                m_Height = ConfigInfo.dataIamgeSize.Height * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
                pictureBoxNurseDocument.Width = m_Width;
                pictureBoxNurseDocument.Height = m_Height;
                RelocationPictureBox();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void DevButtonClose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}