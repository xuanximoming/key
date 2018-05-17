using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.IO;

namespace DrectSoft.Core.EMR_NursingDocument
{
    public partial class PrintAllForm : DevExpress.XtraEditors.XtraForm
    {
        #region Property && Field
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

        public DateTime DateTimeFrom
        {
            get
            {
                return (DateTime)dateEditFrom.EditValue;
            }
        }
        public DateTime DateTimeTo
        {
            get
            {
                return (DateTime)dateEditEnd.EditValue;
            }
        }
        DateTime m_DtFrom;

        Bitmap m_Image;
        Metafile m_MetaFile;

        List<MetaFileInfo> m_MetaFileInfo = new List<MetaFileInfo>();
        PrintDocument m_PrintDocument = new PrintDocument();
        #endregion

        #region .ctor
        public PrintAllForm(DateTime dtFrom)
        {
            InitializeComponent();
            m_DtFrom = dtFrom;
            dateEditFrom.EditValue = dtFrom;
            dateEditEnd.EditValue = DateTime.Now;
            m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);
        }
        #endregion

        #region Load
        private void PrintAllForm_Load(object sender, EventArgs e)
        {
            InitPrinterList();
            InitPageSize();
            simpleButtonPrint.Focus();
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
        /// 初始化纸张大小
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
        #endregion

        public void Print(List<MetaFileInfo> metaFileInfo)
        {
            m_MetaFileInfo = metaFileInfo;

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
            //开始打印
            for (int i = 0; i < Convert.ToInt32(spinEditPrintCount.EditValue); i++)
            {
                foreach (MetaFileInfo info in m_MetaFileInfo)
                {
                    m_Image = info.Bmp;
                    m_MetaFile = info.MetaFile;
                    m_PrintDocument.Print();
                }
            }
            DeleteMetaFile();
        }

        void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            PrintInner(g);
            e.HasMorePages = false;
        }

        private void PrintInner(Graphics g)
        {
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
                            new Rectangle(-5, -10, Convert.ToInt32(imageSize.Width * 0.91), Convert.ToInt32(imageSize.Height * 0.91)),
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
        }

        private void DeleteMetaFile()
        {
            foreach (MetaFileInfo info in m_MetaFileInfo)
            {
                m_Image = info.Bmp;
                m_MetaFile = info.MetaFile;
                string filePath = info.FilePath;
                m_Image.Dispose();
                m_MetaFile.Dispose();
                File.Delete(filePath);
            }
        }
    }

    public class MetaFileInfo
    {
        public string FilePath
        {
            get;
            set;
        }
        public Bitmap Bmp
        {
            get;
            set;
        }
        public Metafile MetaFile
        {
            get;
            set;
        }
        public MetaFileInfo(string filePath, Bitmap bmp, Metafile metaFile)
        {
            FilePath = filePath;
            Bmp = bmp;
            MetaFile = metaFile;
        }
    }
}