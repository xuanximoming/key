using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Drawing.Printing;
using System.IO;
using DrectSoft.Core.NursingDocuments.UserControls;
using System.Drawing.Imaging;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.NursingDocuments
{
    public partial class PrintForm : DevBaseForm
    {
        PrintDocument m_PrintDocument;
        UCThreeMeasureTable m_UCThreeMeasureTable;
        Metafile m_MetaFile;
        Bitmap m_Image;
        int m_Width;
        int m_Height;

        private const int c_Dpi = 96;

        /// <summary>
        /// 默认打印纸张
        /// </summary>
        public string DefaultPageSize
        {
            set
            {
                m_DefaultPageSize = "A4";
            }
        }
        string m_DefaultPageSize;

        public PrintForm()
        {
            InitializeComponent();
        }

        public void InitPreviewControl(Metafile mf, Bitmap bmp, UCThreeMeasureTable ucThreeMeasureTable)
        {
            panelContainer.BackColor = Color.White;
            m_PrintDocument = new PrintDocument();
            m_PrintDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            ReDrawImage(mf, bmp);
            this.StartPosition = FormStartPosition.CenterScreen;
            m_UCThreeMeasureTable = ucThreeMeasureTable;
        }

        private void ReDrawImage(Metafile metaFile, Bitmap bitmap)
        {
            m_MetaFile = metaFile;
            m_Image = bitmap;

            pictureBoxNurseDocument.Width = bitmap.Width;
            pictureBoxNurseDocument.Height = bitmap.Height;
            pictureBoxNurseDocument.BackgroundImage = metaFile;
            pictureBoxNurseDocument.BackgroundImageLayout = ImageLayout.Stretch;

            m_Width = m_Image.Width;
            m_Height = m_Image.Height;
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            InitPrinterList();
            InitPercent();
            InitPageSize();
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
                Size imageSize = new Size(m_Image.Width, m_Image.Height);

                if (pageType == "A4")
                {
                    g.DrawImage(m_MetaFile,
                            new Rectangle(16, 8, Convert.ToInt32(imageSize.Width * 1.02), Convert.ToInt32(imageSize.Height * 1.02)),
                            0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);//y  15
                }
                else if (pageType == "B5")
                {
                    g.DrawImage(m_MetaFile,
                            new Rectangle(0, 15, Convert.ToInt32(imageSize.Width * 0.92), Convert.ToInt32(imageSize.Height * 0.92)),
                            0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);
                }
                else if (pageType == "16K")
                {
                    g.DrawImage(m_MetaFile,
                            new Rectangle(40, 10, Convert.ToInt32(imageSize.Width * 0.95), Convert.ToInt32(imageSize.Height * 0.95)),
                            0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);//3
                }
                else
                {
                    g.DrawImage(m_Image, 0, 0);
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
            //中心医院需求，三测单打印默认是A4 add by ywk 2012年11月8日17:36:40
            PaperSize p = new PaperSize("A4", 275, 457);//默认16K的纸
            foreach (PaperSize ps in pageSetupDialog.Document.PrinterSettings.PaperSizes)
            {
                if (ps.PaperName.Equals(pageType))//这里设置纸张大小,但必须是定义好的  
                    p = ps;
            }
            
            if (p != null)
            {
                pageSetupDialog.Document.DefaultPageSettings.PaperSize = p;
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
            dr["PageName"] = "A4";
            dr["PageValue"] = "A4";
            dataTablePageSize.Rows.Add(dr);

            //dr = dataTablePageSize.NewRow();
            //dr["PageName"] = "B5";
            //dr["PageValue"] = "B5 (JIS)";
            //dataTablePageSize.Rows.Add(dr);

            dr = dataTablePageSize.NewRow();
            dr["PageName"] = "16K";
            dr["PageValue"] = "16K";
            dataTablePageSize.Rows.Add(dr);

            lookUpEditPageSize.Properties.DataSource = dataTablePageSize;
            lookUpEditPageSize.Properties.DisplayMember = "PageName";
            lookUpEditPageSize.Properties.ValueMember = "PageValue";

            if (m_DefaultPageSize != string.Empty)//设置默认纸张
            {
                lookUpEditPageSize.EditValue = m_DefaultPageSize;
            }
            else
            {
                lookUpEditPageSize.EditValue = "A4";
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
            m_Width = m_Image.Width * Convert.ToInt32(lookUpEditPercent.EditValue.ToString().TrimEnd('%')) / 100;
            m_Height = m_Image.Height * Convert.ToInt32(lookUpEditPercent.EditValue.ToString().TrimEnd('%')) / 100;
            pictureBoxNurseDocument.Width = m_Width;
            pictureBoxNurseDocument.Height = m_Height;
            RelocationPictureBox();
        }

        private void pictureBoxNurseDocument_Paint(object sender, PaintEventArgs e)
        {
        }

        private void simpleButtonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.InitialDirectory = "D:\\";
            file.Filter = "Image   Files(*.jpg)|*.jpg";
            file.FileName = "三测单";
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (m_MetaFile != null)
                {
                    //string filePath = file.FileName;
                    //Bitmap bmp = new Bitmap(m_Image.Width, m_Image.Height);
                    //Graphics g = Graphics.FromImage(bmp);
                    //Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    //Metafile mf = new Metafile(filePath, g.GetHdc(), rect, MetafileFrameUnit.Pixel);
                    //g = Graphics.FromImage(mf);
                    //g.DrawImage(m_MetaFile, new Point(0, 0));
                    //g.Save();
                    //g.Dispose();
                    //mf.Dispose();
                    m_MetaFile.Save(file.FileName);
                }
            }
        }

        private void lookUpEditPrinter_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            m_Width = m_Image.Width * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            m_Height = m_Image.Height * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            pictureBoxNurseDocument.Width = m_Width;
            pictureBoxNurseDocument.Height = m_Height;
            RelocationPictureBox();
        }
    }
}