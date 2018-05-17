using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public partial class PrintFroms : DevBaseForm
    {

        PrintDocument m_PrintDocument;
        Drawingform drawingform;

        int m_PageIndex = 1;
        public PrintFroms(Drawingform dmpu)
        {

            InitializeComponent();
            ReLocationPicture();
            drawingform = dmpu;
        }
        #region 事件


        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintFroms_Load(object sender, EventArgs e)
        {
            InitPrinterList();
            InitPageSize();
            InitPreviewControl();
            InitMetaFile();

            button1.Focus();
        }

        private void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            string pageType = lookUpEditPageSize.EditValue.ToString();
            m_PrintDocument.PrinterSettings.PrinterName = lookUpEditPrinter.EditValue.ToString();

            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.Document = m_PrintDocument;
            PaperSize p = new PaperSize("16K", 275, 457);//默认16K的纸
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
                    m_PrintDocument.Print();
                }
            }
        }
        /// <summary>
        /// 进度条拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            //int width = drawingform.m_PageWidth * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            //int height = drawingform.m_PageHeight * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            int width = drawingform.m_PageWidth * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            int height = drawingform.m_PageHeight * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            pictureBox2.Width = width;
            pictureBox2.Height = height;
            ReLocationPicture();

        }




        //private void panelContainer_Click(object sender, EventArgs e)
        //{
        //    button1.Focus();
        //}

        //private void pictureBox1_Click(object sender, EventArgs e)
        //{
        //    button1.Focus();
        //}

        //private void pictureBox2_Click(object sender, EventArgs e)
        //{
        //    button1.Focus();
        //}

        //private void simpleButtonExport_Click(object sender, EventArgs e)
        //{
        //    //panelContainer.
        //}

        void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (m_PageIndex == 1)
            {
                if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                {
                    e.Graphics.DrawImage(drawingform.MF1, new RectangleF(10, 0, drawingform.m_PageWidth * 0.98f, drawingform.m_PageHeight * 0.98f));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                {
                    e.Graphics.DrawImage(drawingform.MF1, new RectangleF(5, 0, drawingform.m_PageWidth * 0.88f, drawingform.m_PageHeight * 0.88f));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
                {
                    e.Graphics.DrawImage(drawingform.MF1, new RectangleF(30, 0, drawingform.m_PageWidth * 0.90f, drawingform.m_PageHeight * 0.90f));
                }

                m_PageIndex++;
                e.HasMorePages = false;
            }
            else if (m_PageIndex == 2)
            {
                if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                {
                    e.Graphics.DrawImage(drawingform.MF2, new RectangleF(10, 0, drawingform.m_PageWidth * 0.98f, drawingform.m_PageHeight * 0.98f));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                {
                    e.Graphics.DrawImage(drawingform.MF2, new RectangleF(5, 0, drawingform.m_PageWidth * 0.88f, drawingform.m_PageHeight * 0.88f));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
                {
                    e.Graphics.DrawImage(drawingform.MF2, new RectangleF(30, 0, drawingform.m_PageWidth * 0.90f, drawingform.m_PageHeight * 0.90f));
                }

                m_PageIndex = 1;
                e.HasMorePages = false;
            }
        }

        #endregion

        #region 函数方法

        public void InitPreviewControl()
        {
            m_PrintDocument = new PrintDocument();
            m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);
            this.StartPosition = FormStartPosition.CenterScreen;
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

        /// <summary>
        /// 初始化页面大小
        /// </summary>
        private void InitPageSize()
        {
            DataTable dataTablePageSize = new DataTable();
            dataTablePageSize.Columns.Add("PageName");
            dataTablePageSize.Columns.Add("PageValue");

            DataRow dr = dataTablePageSize.NewRow();
            dr["PageName"] = "A4";
            dr["PageValue"] = "A4";
            dataTablePageSize.Rows.Add(dr);

            dr = dataTablePageSize.NewRow();
            dr["PageName"] = "B5";
            dr["PageValue"] = "B5 (JIS)";
            dataTablePageSize.Rows.Add(dr);

            dr = dataTablePageSize.NewRow();
            dr["PageName"] = "16K";
            dr["PageValue"] = "16K";
            dataTablePageSize.Rows.Add(dr);


            lookUpEditPageSize.Properties.DataSource = dataTablePageSize;
            lookUpEditPageSize.Properties.DisplayMember = "PageName";
            lookUpEditPageSize.Properties.ValueMember = "PageValue";


            lookUpEditPageSize.EditValue = "A4";
        }

        /// <summary>
        /// 绘制首页矢量图
        /// </summary>
        private void InitMetaFile()
        {
            pictureBox1.BackgroundImage = drawingform.MF1;
            pictureBox1.Width = drawingform.m_PageWidth;
            pictureBox1.Height = drawingform.m_PageHeight;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;

            pictureBox2.BackgroundImage = drawingform.MF2;
            pictureBox2.Width = drawingform.m_PageWidth;
            pictureBox2.Height = drawingform.m_PageHeight;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;

            ReLocationPicture();
        }

        /// <summary>
        /// 重新定位PictureBox
        /// </summary>
        private void ReLocationPicture()
        {
            if (pictureBox1.Height < panelContainer.Height - 21)
            {
                pictureBox1.Location = new Point(panelContainer.Width / 2 - pictureBox1.Width - 20, panelContainer.AutoScrollPosition.Y);
                pictureBox2.Location = new Point(panelContainer.Width / 2 + 20, panelContainer.AutoScrollPosition.Y);
            }
            else
            {
                pictureBox1.Location = new Point((panelContainer.Width - pictureBox1.Width) / 2, panelContainer.AutoScrollPosition.Y);
                pictureBox2.Location = new Point((panelContainer.Width - pictureBox1.Width) / 2, pictureBox1.Location.Y + pictureBox1.Height + 20);
            }
        }

        #endregion





    }
}