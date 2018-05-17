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
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.Consultation
{
    public partial class PrintForm : DevExpress.XtraEditors.XtraForm
    {

        Metafile mf1;
        
                Metafile mf2;
        PrintDocument m_PrintDocument;
        DrawConsultUtil m_DrawConsultUtil;
        private IEmrHost m_app;
        /// <summary>
        /// //定义此变量，用于护士工作站中打印的时候，往会诊单据表里回写打印单据时间（
        /// 现控制到护士工作站打印操作，进行插入，文书录入中打印不操作会诊单据表）
        /// </summary>
        public bool IsPrint;
        public string consapply;//单据号

        public PrintForm(ConsultationEntity consultationEntity)
        {
            InitializeComponent();

            m_DrawConsultUtil = new DrawConsultUtil(consultationEntity);
            m_PrintDocument = new PrintDocument();
            
        }

        public PrintForm(ConsultationEntity consultationEntity, bool PrintTip,IEmrHost m_App)
        {
            InitializeComponent();

            m_DrawConsultUtil = new DrawConsultUtil(consultationEntity);
            m_PrintDocument = new PrintDocument();
            IsPrint = PrintTip;
            consapply = consultationEntity.ConsultApplySn;
            m_app = m_App;
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitPrinterList();
                InitPageSize();
                InitPreviewControl();
                InitMetaFile();
                button1.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitPreviewControl()
        {
            try
            {
                m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打印事件
        /// edit by xlb 2013-03-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                {
                    e.Graphics.DrawImage(mf1, new RectangleF(10, 0, m_DrawConsultUtil.PageWidth * 0.98f, m_DrawConsultUtil.PageHeight * 0.98f));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                {
                    e.Graphics.DrawImage(mf1, new RectangleF(5, 0, m_DrawConsultUtil.PageWidth * 0.88f, m_DrawConsultUtil.PageHeight * 0.88f));
                }

                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 初始化打印机列表
        /// </summary>
        private void InitPrinterList()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化页面大小
        /// </summary>
        private void InitPageSize()
        {
            try
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

                lookUpEditPageSize.Properties.DataSource = dataTablePageSize;
                lookUpEditPageSize.Properties.DisplayMember = "PageName";
                lookUpEditPageSize.Properties.ValueMember = "PageValue";

                lookUpEditPageSize.EditValue = "A4";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绘制首页矢量图
        /// edit by xlb 2013-03-18
        /// </summary>
        private void InitMetaFile()
        {
            try
            {
                mf1 = m_DrawConsultUtil.MF1;
                pictureBox1.BackgroundImage = mf1;
                pictureBox1.Width = m_DrawConsultUtil.PageWidth;
                pictureBox1.Height = m_DrawConsultUtil.PageHeight;
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
                mf2 = m_DrawConsultUtil.MF2;
                pictureBox2.BackgroundImage = mf2;
                pictureBox2.Width = m_DrawConsultUtil.PageWidth;
                pictureBox2.Height = m_DrawConsultUtil.PageHeight;
                pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
               
                ReLocationPicture();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重新定位PictureBox
        /// edit by xlb 2013-03-18
        /// </summary>
        private void ReLocationPicture()
        {
            try
            {
               // pictureBox1.Location = new Point((panelContainer.Width - pictureBox1.Width) / 2, panelContainer.AutoScrollPosition.Y);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// Edit by xlb
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                m_DrawConsultUtil.DeleteMetaFile();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void panelContainer_Click(object sender, EventArgs e)
        {
            button1.Focus();
        }

        /// <summary>
        /// 显示比例改变触发事件
        /// Edit by xlb 2013-03-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBarControl1_EditValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                int width = m_DrawConsultUtil.PageWidth * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
                int height = m_DrawConsultUtil.PageHeight * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
                pictureBox1.Width = width;
                pictureBox1.Height = height;
                pictureBox2.Width = width;
                pictureBox2.Height = height;
                ReLocationPicture();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 打印会诊单据
        /// edit by xlb 2013-03-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonPrint_Click(object sender, EventArgs e)
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
                //if (pageSetupDialog.ShowDialog() == DialogResult.OK)
                {
                    //开始打印
                    for (int i = 0; i < Convert.ToInt32(spinEditPrintCount.EditValue); i++)
                    {
                        m_PrintDocument.Print();
                    }
                }
                if (IsPrint)
                {
                    InsertPrintTime(consapply);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 导出Exccel
        /// Edit by xlb 2013-03-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog file = new SaveFileDialog();
                file.InitialDirectory = "D:\\";
                file.Filter = "Image   Files(*.jpg)|*.jpg";
                file.FileName = "会诊单";
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (mf1 != null)
                    {
                        mf1.Save(file.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 会诊申请单打印时，
        /// </summary>
        /// <param name="consultapplysn"></param>
        public void InsertPrintTime(string consultapplysn)
        {
            Dal.DataAccess.InsertPrintTime(consultapplysn, m_app);
        }
    }
}