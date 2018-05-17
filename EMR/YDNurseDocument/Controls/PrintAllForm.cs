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
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.NurseDocument.Controls
{
    public partial class PrintAllForm : DevBaseForm
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

        List<Bitmap> m_bitmapList = new List<Bitmap>();
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
        #endregion

        public void Print(List<Bitmap> bitmapList)
        {
            try
            {
                m_bitmapList = bitmapList;

                string pageType = lookUpEditPageSize.EditValue.ToString();
                m_PrintDocument.PrinterSettings.PrinterName = lookUpEditPrinter.EditValue.ToString();

                PageSetupDialog pageSetupDialog = new PageSetupDialog();
                pageSetupDialog.Document = m_PrintDocument;
              //  PaperSize p = null; //new PaperSize("16K", 275, 457);//默认16K的纸
                //xll 2013-2-6  2013--03-05
                PaperSize p = new PaperSize("16K", 785, 1012);//默认16K的纸 调试获得 不是太准确
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
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("打印机不支持" + pageType + "打印!");
                    return;
                }

                pageSetupDialog.Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                //开始打印
                for (int i = 0; i < Convert.ToInt32(spinEditPrintCount.EditValue); i++)
                {
                    foreach (Bitmap info in m_bitmapList)
                    {
                        m_Image = info;
                        m_PrintDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                PrintInner(g);
                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void PrintInner(Graphics g)
        {
            for (int i = 0; i < m_bitmapList.Count;i++)
            {
                if (m_Image != null)
                {
                    string pageType = lookUpEditPageSize.Text;
                    Size imageSize = new Size(m_Image.Width, m_Image.Height);

                    if (pageType == "A4")
                    {
                        g.DrawImage(m_Image,
                                new Rectangle(16, 8, Convert.ToInt32(imageSize.Width * 1.02), Convert.ToInt32(imageSize.Height * 1.02)),
                                0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);//y  15
                    }
                    else if (pageType == "B5")
                    {
                        g.DrawImage(m_Image,
                                 new Rectangle(0, 15, Convert.ToInt32(imageSize.Width * 0.92), Convert.ToInt32(imageSize.Height * 0.92)),
                                 0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);
                    }
                    else if (pageType == "16K")
                    {
                        g.DrawImage(m_Image,
                                new Rectangle(40, 0, Convert.ToInt32(imageSize.Width * 0.95), Convert.ToInt32(imageSize.Height * 0.95)),
                                0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        g.DrawImage(m_Image, 0, 0);
                    }
                }
            }
        }

        /// <summary>
        /// 批量打印
        /// </summary>
        /// 修复时间验证问题
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-23</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(dateEditFrom.Text.Trim())) 
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("打印开始时间不能为空");
                    return;
                }
                else if (string.IsNullOrEmpty(dateEditEnd.Text.Trim())) 
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("打印结束时间不能为空");
                    return;
                }
                if (dateEditFrom.DateTime > dateEditEnd.DateTime)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("打印开始时间不能大于结束时间");
                    return;
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
    }
}