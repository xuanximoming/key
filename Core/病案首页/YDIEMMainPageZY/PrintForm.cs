using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace DrectSoft.Core.IEMMainPageZY
{
    public partial class PrintForm : DevExpress.XtraEditors.XtraForm
    {
        #region Property && Field

        PrintDocument m_PrintDocument;

        int m_PageIndex = 1;

        DrawMainPageUtil util;
        #endregion

        #region .ctor

        public PrintForm(DrawMainPageUtil dmpu)
        {
            InitializeComponent();
            util = dmpu;
        }

        #endregion

        #region Method

        public void InitPreviewControl()
        {
            this.cmbPrintPage.SelectedText = "全部打印";
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


            lookUpEditPageSize.EditValue = "A4";//默认选择A4
        }

        /// <summary>
        /// 绘制首页矢量图
        /// </summary>
        private void InitMetaFile()
        {
            pictureBox1.BackgroundImage = util.MF1;
            pictureBox1.Width = util.m_PageWidth;
            pictureBox1.Height = util.m_PageHeight;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;

            pictureBox2.BackgroundImage = util.MF2;
            pictureBox2.Width = util.m_PageWidth;
            pictureBox2.Height = util.m_PageHeight;
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

        #region Event

        private void PrintForm_Load(object sender, EventArgs e)
        {
            InitPrinterList();
            InitPageSize();
            InitPreviewControl();
            InitMetaFile();

            button1.Focus();
        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            int width = util.m_PageWidth * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            int height = util.m_PageHeight * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            pictureBox2.Width = width;
            pictureBox2.Height = height;
            ReLocationPicture();
        }

        private void simpleButtonClick(object sender, EventArgs e)
        {
            try
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
                if (checkEditPrintDuplex.Checked)
                {
                    if (pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.CanDuplex)
                    {
                        //add by ywk 2012年10月23日 19:59:34  要求左右上下反转打印
                        if (rabLeftRight.Checked)//左右翻转
                        {
                            pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
                        }
                        else if (rabUpDown.Checked)//上下翻转
                        {
                            pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                        }
                        //pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                        else//一个都不选择就是默认
                        {
                            pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                        }
                    }
                    else
                    {
                        MessageBox.Show("此打印机不支持双面打印!", "警告");
                        checkEditPrintDuplex.Checked = false;
                        return;
                    }
                }
                else
                {
                    pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                }

                //if (pageSetupDialog.ShowDialog() == DialogResult.OK)
                {
                    //开始打印
                    for (int i = 0; i < Convert.ToInt32(spinEditPrintCount.EditValue); i++)
                    {
                        if (rabLeftRight.Checked)//左右翻转
                        {
                            pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
                            m_PrintDocument.Print();
                        }
                        else if (rabUpDown.Checked)//上下翻转
                        {
                            pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                            m_PrintDocument.Print();
                        }
                        else//什么都不选中，默认
                        {
                            pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                            m_PrintDocument.Print();
                        }
                        //m_PrintDocument.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
          
        }

        private void panelContainer_Click(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void simpleButtonExport_Click(object sender, EventArgs e)
        {
            //panelContainer.
        }

        void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (cmbPrintPage.SelectedText == "打印第一页")
            {
                if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                {
                    e.Graphics.DrawImage(util.MF1, new RectangleF(5, 0, util.m_PageWidth * 0.98f, util.m_PageHeight * 0.98f));//10
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                {
                    e.Graphics.DrawImage(util.MF1, new RectangleF(5, 0, util.m_PageWidth * 0.88f, util.m_PageHeight * 0.88f));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
                {
                    e.Graphics.DrawImage(util.MF1, new RectangleF(30, 0, util.m_PageWidth * 0.90f, util.m_PageHeight * 0.90f));
                }

                //m_PageIndex++;
                m_PageIndex = 1;
                e.HasMorePages = false;
            }
            if (cmbPrintPage.SelectedText == "打印第二页")
            {
                if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                {
                    e.Graphics.DrawImage(util.MF2, new RectangleF(5, 0, util.m_PageWidth * 0.98f, util.m_PageHeight * 0.98f));//10
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                {
                    e.Graphics.DrawImage(util.MF2, new RectangleF(5, 0, util.m_PageWidth * 0.88f, util.m_PageHeight * 0.88f));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
                {
                    e.Graphics.DrawImage(util.MF2, new RectangleF(30, 0, util.m_PageWidth * 0.90f, util.m_PageHeight * 0.90f));
                }

                //m_PageIndex++;
                m_PageIndex = 2;
                e.HasMorePages = false;
            }
            if (cmbPrintPage.SelectedText == "全部打印" || cmbPrintPage.SelectedText == "")
            {
                if (m_PageIndex == 1)
                {
                    if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                    {
                        e.Graphics.DrawImage(util.MF1, new RectangleF(5, 0, util.m_PageWidth * 0.98f, util.m_PageHeight * 0.98f));//10
                    }
                    else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                    {
                        e.Graphics.DrawImage(util.MF1, new RectangleF(5, 0, util.m_PageWidth * 0.88f, util.m_PageHeight * 0.88f));
                    }
                    else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
                    {
                        e.Graphics.DrawImage(util.MF1, new RectangleF(30, 0, util.m_PageWidth * 0.90f, util.m_PageHeight * 0.90f));
                    }

                    m_PageIndex++;
                    e.HasMorePages = true;
                }
                else if (m_PageIndex == 2)
                {
                    if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                    {
                        e.Graphics.DrawImage(util.MF2, new RectangleF(5, 0, util.m_PageWidth * 0.98f, util.m_PageHeight * 0.98f));//10
                    }
                    else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                    {
                        e.Graphics.DrawImage(util.MF2, new RectangleF(5, 0, util.m_PageWidth * 0.88f, util.m_PageHeight * 0.88f));
                    }
                    else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
                    {
                        e.Graphics.DrawImage(util.MF2, new RectangleF(30, 0, util.m_PageWidth * 0.90f, util.m_PageHeight * 0.90f));
                    }

                    m_PageIndex = 1;
                    e.HasMorePages = false;
                }
            }


            //if (m_PageIndex == 1)
            //{
            //    if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
            //    {
            //        e.Graphics.DrawImage(util.MF1, new RectangleF(5, 0, util.m_PageWidth * 0.98f, util.m_PageHeight * 0.98f));//10
            //    }
            //    else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
            //    {
            //        e.Graphics.DrawImage(util.MF1, new RectangleF(5, 0, util.m_PageWidth * 0.88f, util.m_PageHeight * 0.88f));
            //    }
            //    else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
            //    {
            //        e.Graphics.DrawImage(util.MF1, new RectangleF(30, 0, util.m_PageWidth * 0.90f, util.m_PageHeight * 0.90f));
            //    }

            //    m_PageIndex++;
            //    e.HasMorePages = false;
            //}
            //else if (m_PageIndex == 2)
            //{
            //    if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
            //    {
            //        e.Graphics.DrawImage(util.MF2, new RectangleF(5, 0, util.m_PageWidth * 0.98f, util.m_PageHeight * 0.98f));//10
            //    }
            //    else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
            //    {
            //        e.Graphics.DrawImage(util.MF2, new RectangleF(5, 0, util.m_PageWidth * 0.88f, util.m_PageHeight * 0.88f));
            //    }
            //    else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
            //    {
            //        e.Graphics.DrawImage(util.MF2, new RectangleF(30, 0, util.m_PageWidth * 0.90f, util.m_PageHeight * 0.90f));
            //    }

            //    m_PageIndex = 1;
            //    e.HasMorePages = false;
            //}
        }

        #endregion

        private void checkEditPrintDuplex_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditPrintDuplex.Checked)
                {
                    rabLeftRight.Visible = true;
                    rabUpDown.Visible = true;
                }
                else
                {
                    rabLeftRight.Visible = false;
                    rabUpDown.Visible = false;
                }
        
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
        }
    }
}