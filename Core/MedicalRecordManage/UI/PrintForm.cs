using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using DevExpress.XtraEditors;
using DrectSoft.Emr.Util;
using System.Drawing.Printing;
using System.Xml;
using DevExpress.Utils;
using DrectSoft.Core;
using System.IO;
using System.Linq;
using DrectSoft.Common.Ctrs.DLG;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using DrectSoft.DSSqlHelper;
using DrectSoft.Common.Ctrs.FORM;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;

namespace MedicalRecordManage.UI
{
    public partial class PrintForm : DevBaseForm
    {
        #region 初始化
        private int m_PrintType = 0;//0-姓名索引卡；1-死亡索引卡；
        private List<DataRowView> m_PrintData;//索引卡数据集
        private List<PictureBox> m_PictureBoxList = new List<PictureBox>();
        private List<Metafile> m_MetaFileList = new List<Metafile>();
        private PrintDocument m_PrintDocument;
        private int m_PageIndex = 1;
        #endregion

        #region 默认值
        private string m_PaperType = "A4";//纸张大小
        private int m_Width = 827;//纸张宽度
        private int m_Height = 1169;//纸张高度1169
        private int m_CardWidth = 400;
        private int m_CardHeight = 210;
        private int m_RowHeight = 30;//行高度
        private int m_ColWidth = 10;//列宽度
        private int m_DefaultLineWidth = 120;//默认线条长度(输入项下的横线)
        private float m_PointXStart = 20;//起始横坐标
        private float m_PointYStart = 30;//起始纵坐标
        #endregion

        #region 初始化属性
        private Font m_BoldFont = new Font("宋体", 16f, FontStyle.Bold, GraphicsUnit.Pixel);
        private Font m_DefaultFont = new Font("宋体", 15f, FontStyle.Regular, GraphicsUnit.Pixel);
        private Pen pen = new Pen(Brushes.Black, 1);
        private Pen solidPen = new Pen(Brushes.Black, 2);
        private StringFormat sf = new StringFormat();
        private StringFormat sfVertical = new StringFormat();
        private StringFormat sfTop = new StringFormat();
        private Font m_SmallFont = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Pixel);
        #endregion

        public PrintForm(List<DataRowView> printData, int printType)
        {
            InitializeComponent();
            m_PrintType = printType;
            m_PrintData = printData;
            InitVariable();
            InitPrinterList();
            
            InitPreviewControl();
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (null == m_PrintData || m_PrintData.Count == 0)
                {
                    return;
                }
                PrintAllCard(m_PrintType);
                InitMetaFile();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 初始化
        /// <summary>
        /// 初始化变量
        /// </summary>
        private void InitVariable()
        {
            try
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                sfVertical.LineAlignment = StringAlignment.Near;
                sfTop.LineAlignment = StringAlignment.Near;
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
        /// 初始化控件状态
        /// </summary>
        public void InitPreviewControl()
        {
            try
            {
                this.che_All.Checked = true;
                m_PrintDocument = new PrintDocument();
                m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化指定页最大值
        /// </summary>
        /// <param name="maxNum"></param>
        private void InitPointPageValue(int maxNum)
        {
            try
            {

                che_Begin.Properties.MaxValue = maxNum > 28 ? 28 : maxNum;
                che_End.Properties.MaxValue = maxNum > 28 ? 28 : maxNum;
                che_Begin.EditValue = 1;
                che_End.EditValue = maxNum > 28 ? 28 : maxNum;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 显示打印图片
        /// </summary>
        private void InitMetaFile()
        {
            try
            {
                if (null == m_MetaFileList || m_MetaFileList.Count == 0)
                {
                    return;
                }
                int i = 0;
                int height = 0;
                foreach (Metafile file in m_MetaFileList)
                {
                    i++;
                    PictureBox box = new PictureBox();
                    box.BackgroundImage = file;
                    box.Width = m_Width;
                    box.Height = m_Height;
                    box.BackgroundImageLayout = ImageLayout.Stretch;
                    panelContainer.Controls.Add(box);
                    Point boxlocation = new Point((panelContainer.Width - box.Width) / 2, height);
                    //box.Location = new Point((panelContainer.Width - box.Width) / 2, height);
                    //box.ResumeLayout(true);
                    box.Location = boxlocation;
                    height += box.Height + 20;
                    box.Click += new EventHandler(box_Click);
                    if (i < 28)
                    {
                        m_PictureBoxList.Add(box);
                    }
                }
                panelContainer.Click += new EventHandler(panelContainer_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void panelContainer_Click(object sender, EventArgs e)
        {
            button1.Focus();
        }

        void box_Click(object sender, EventArgs e)
        {
            button1.Focus();
        }

        #region 事件
        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Close_Click(object sender, EventArgs e)
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
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Print_Click(object sender, EventArgs e)
        {
            try
            {
                m_PageIndex = che_All.Checked ? 1 : Convert.ToInt32(che_Begin.EditValue);

                m_PrintDocument.PrinterSettings.PrinterName = lookUpEditPrinter.EditValue.ToString();
                PageSetupDialog pageSetupDialog = new PageSetupDialog();
                pageSetupDialog.Document = m_PrintDocument;
                PaperSize p = new PaperSize("16K", 275, 457);//默认16K的纸
                foreach (PaperSize ps in pageSetupDialog.Document.PrinterSettings.PaperSizes)
                {
                    if (ps.PaperName.Equals(m_PaperType))//这里设置纸张大小,但必须是定义好的  
                    {
                        p = ps;
                    }
                }

                if (p != null)
                {
                    pageSetupDialog.Document.DefaultPageSettings.PaperSize = p;
                }

                pageSetupDialog.Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;

                for (int i = 0; i < Convert.ToInt32(lue_PrintCount.EditValue); i++)
                {
                    pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                    m_PrintDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 选择切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void che_All_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (che_All.Checked)
                {
                    che_Begin.Enabled = false;
                    che_End.Enabled = false;
                }
                else if (che_PointPage.Checked)
                {
                    che_Begin.Enabled = true;
                    che_End.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 打印文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (null == m_MetaFileList || m_MetaFileList.Count() == 0 || m_PageIndex > m_MetaFileList.Count())
                {
                    return;
                }
                if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                {
                    e.Graphics.DrawImage(m_MetaFileList[m_PageIndex - 1], new RectangleF(20, 10, m_Width * 0.95f, m_Height * 0.95f));//10
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                {
                    e.Graphics.DrawImage(m_MetaFileList[m_PageIndex - 1], new RectangleF(5, 0, m_Width * 0.88f, m_Height * 0.88f));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.Custom)
                {
                    e.Graphics.DrawImage(m_MetaFileList[m_PageIndex - 1], new RectangleF(30, 0, m_Width * 0.90f, m_Height * 0.90f));
                }

                if (che_All.Checked && m_PageIndex == m_MetaFileList.Count())
                {///全部打印
                    m_PageIndex = 1;
                    e.HasMorePages = false;
                }
                else if (che_PointPage.Checked && m_PageIndex == Convert.ToInt32(che_End.EditValue))
                {///打印指定页
                    m_PageIndex = Convert.ToInt32(che_Begin.EditValue);
                    e.HasMorePages = false;
                }
                else
                {
                    m_PageIndex++;
                    e.HasMorePages = true;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 画打印的图片
        /// <summary>
        /// 打印索引卡
        /// </summary>
        /// <param name="type">卡片类型：0-姓名索引卡；1-死亡索引卡</param>
        private void PrintAllCard(int type)
        {
            try
            {
                int pages = (int)Math.Ceiling(m_PrintData.Count / 10.00);///索引卡页数
                string folder = CreateFolder();
                for (int i = 0; i < pages; i++)
                {
                    Bitmap bmp1 = new Bitmap(m_Width, m_Height, PixelFormat.Format32bppArgb);
                    Graphics g1 = Graphics.FromImage(bmp1);
                    Rectangle rect = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
                    string m_FilePath1 = folder + Guid.NewGuid().ToString() + ".wmf";
                    Metafile mf1 = new Metafile(m_FilePath1, g1.GetHdc(), rect, MetafileFrameUnit.Pixel);
                    g1 = Graphics.FromImage(mf1);
                    g1.SmoothingMode = SmoothingMode.HighQuality;
                    g1.Clear(Color.White);
                    if (type == 0)//姓名索引卡
                    {
                        PrintPageOfNameCard(g1, m_PrintData.Skip(i * 10).Take(10).ToList(), i + 1);
                    }
                    else if (type == 1)//死亡索引卡
                    {
                        PrintPageOfDeathCard(g1, m_PrintData.AsEnumerable().Skip(i * 10).Take(10).ToList(), i + 1);
                    }
                    else if (type == 2)//手术索引卡
                    {
                        PrintPageOfOperationCard(g1, m_PrintData.AsEnumerable().Skip(i * 10).Take(10).ToList(), i + 1);
                    }
                    else if (type == 3)//疾病索引卡
                    {
                        PrintPageOfDiseaseCard(g1, m_PrintData.AsEnumerable().Skip(i * 10).Take(10).ToList(), i + 1);
                    }
                    if (i < 28)
                    {
                        m_MetaFileList.Add(mf1);
                    }
                    
                    g1.Save();
                    g1.Dispose();
                }
                InitPointPageValue(pages);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打印当前页(姓名索引卡)
        /// </summary>
        /// <param name="g"></param>
        private void PrintPageOfNameCard(Graphics g, List<DataRowView> rowList, int pageIndex)
        {
            try
            {
                if (null == rowList || rowList.Count() == 0)
                {
                    return;
                }
                int lineHeight = TextRenderer.MeasureText("高", m_DefaultFont).Height;
                int charWidth = TextRenderer.MeasureText("宽", m_DefaultFont).Height;

                float defaultPointX = m_PointXStart;
                float defaultPointY = m_PointYStart;
                for (int i = 0; i < rowList.Count(); i++)
                {
                    #region 初始化数据
                    DataRowView row = rowList[i];
                    float pointX = defaultPointX;
                    float pointY = defaultPointY;
                    #endregion

                    #region 边框
                    ///上边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth));
                    ///左边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth, pointY - m_ColWidth + m_CardHeight));
                    ///下边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth + m_CardHeight), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth + m_CardHeight));
                    ///右边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth + m_CardHeight));
                    #endregion

                    #region 卡片内容
                    ///姓名索引卡
                    ///edit by ywk 增加拼音码 2013年8月30日 10:08:16
                    int titleWidth = TextRenderer.MeasureText("姓名索引卡", m_DefaultFont).Width;
                    g.DrawString("姓名索引卡", m_BoldFont, Brushes.Black, new PointF(pointX, pointY));
                    pointX = defaultPointX;
                    g.DrawString("拼音码:", m_DefaultFont, Brushes.Black, new PointF(pointX + titleWidth + 60, pointY - m_ColWidth + 9));

                    g.DrawLine(pen, new PointF(pointX + titleWidth + 60, pointY + lineHeight), new PointF(pointX + titleWidth + 300, pointY + lineHeight));
                    g.DrawString(row["PY"].ToString(), m_DefaultFont, Brushes.Black, new PointF(pointX + titleWidth + 118, pointY - m_ColWidth + 9));
                    pointY += m_RowHeight;

                    //int titleWidth = TextRenderer.MeasureText("姓名索引卡", m_DefaultFont).Width;
                    //g.DrawString("姓名索引卡", m_BoldFont, Brushes.Black, new PointF((m_CardWidth - titleWidth) / 2 + pointX, pointY));
                    //pointX = defaultPointX;
                    //pointY += m_RowHeight;

                    ///病案号
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病 案 号", row["PATID"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "科    室", row["OUTHOSDEPTNAME"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///姓名
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "姓    名", row["NAME"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    ///性别
                    string sexname = row["SEXID"].ToString() == "1" ? "男" : (row["SEXID"].ToString() == "2" ? "女" : "未知");
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "性    别", sexname, 30, "", m_DefaultFont) + 5;
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "年龄", row["AGESTR"].ToString(), 42, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///入院日期
                    DateTime date1 = !string.IsNullOrEmpty(row["INWARDDATE"].ToString().Trim()) ? DateTime.Parse(row["INWARDDATE"].ToString()) : DateTime.Parse(row["ADMITDATE"].ToString());
                    string inDate = date1.ToString("yyyy-MM-dd HH:mm:ss");
                    string inHosDate = inDate.Substring(0, 4) + "年" + inDate.Substring(5, 2) + "月" + inDate.Substring(8, 2) + "日";
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院日期", inHosDate, m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    ///出院日期
                    string outHosDate = string.Empty;
                    if (row["status"].ToString() == "1502" || row["status"].ToString() == "1503")
                    {
                        if (!string.IsNullOrEmpty(row["OUTWARDDATE"].ToString().Trim()) || !string.IsNullOrEmpty(row["OUTHOSDATE"].ToString().Trim()))
                        {
                            DateTime date2 = !string.IsNullOrEmpty(row["OUTHOSDATE"].ToString().Trim()) ? DateTime.Parse(row["OUTHOSDATE"].ToString()) : DateTime.Parse(row["OUTWARDDATE"].ToString());
                            string outDate = date2.ToString("yyyy-MM-dd HH:mm:ss");
                            outHosDate = outDate.Substring(0, 4) + "年" + outDate.Substring(5, 2) + "月" + outDate.Substring(8, 2) + "日";
                        }
                    }
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出院日期", outHosDate, m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///主要诊断
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "主要诊断", row["DIAGNOSIS_NAME"].ToString(), 320, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///地址
                    string address = row["XZZADDRESSUNION"].ToString();
                    string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                    if (!string.IsNullOrEmpty(config))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(config);
                        XmlNodeList nodeList = doc.GetElementsByTagName("IsReadAddressInfo");
                        if (null != nodeList && nodeList.Count > 0)
                        {
                            string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                            if (cfgValue == "0")
                            {//省、市、县
                                address = row["XZZADDRESSUNION"].ToString();
                            }
                            else
                            {//长地址
                                address = row["XZZADDRESS"].ToString();
                            }
                        }
                    }
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "地    址", address, 320, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///转归
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "转    归", row["ZG_NAME"].ToString(), 320, "", m_DefaultFont) + m_ColWidth;

                    defaultPointX = i == 4 ? (defaultPointX + m_CardWidth + 10) : defaultPointX;
                    defaultPointY = i == 4 ? m_PointYStart : pointY + m_RowHeight + 15;
                    #endregion
                }
                ///页码
                string pageStr = "第 " + pageIndex + " 页";
                int pageStrWidth = TextRenderer.MeasureText(pageStr, m_DefaultFont).Width;
                g.DrawString(pageStr, m_DefaultFont, Brushes.Black, new RectangleF((m_Width - pageStrWidth) / 2, m_Height - 20, pageStrWidth, lineHeight + 4), sf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打印当前页(死亡索引卡)
        /// </summary>
        /// <param name="g"></param>
        private void PrintPageOfDeathCard(Graphics g, List<DataRowView> rowList, int pageIndex)
        {
            try
            {
                if (null == rowList || rowList.Count() == 0)
                {
                    return;
                }
                int lineHeight = TextRenderer.MeasureText("高", m_DefaultFont).Height;
                int charWidth = TextRenderer.MeasureText("宽", m_DefaultFont).Height;

                float defaultPointX = m_PointXStart;
                float defaultPointY = m_PointYStart;
                for (int i = 0; i < rowList.Count(); i++)
                {
                    #region 初始化数据
                    DataRowView row = rowList[i];
                    float pointX = defaultPointX;
                    float pointY = defaultPointY;
                    #endregion

                    #region 边框
                    ///上边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth));
                    ///左边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth, pointY - m_ColWidth + m_CardHeight));
                    ///下边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth + m_CardHeight), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth + m_CardHeight));
                    ///右边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth + m_CardHeight));
                    #endregion

                    #region 卡片内容
                    ///死亡索引卡
                    int titleWidth = TextRenderer.MeasureText("死亡索引卡", m_DefaultFont).Width;
                    g.DrawString("死亡索引卡", m_BoldFont, Brushes.Black, new PointF((m_CardWidth - titleWidth) / 2 + pointX, pointY));
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///病案号
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病 案 号", row["patid"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "科    室", row["OUTHOSDEPTNAME"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///姓名
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "姓    名", row["NAME"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    ///性别
                    string sexname = row["SEXID"].ToString() == "1" ? "男" : (row["SEXID"].ToString() == "2" ? "女" : "未知");
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "性    别", sexname, 30, "", m_DefaultFont) + 5;
                    ///年龄
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "年龄", row["AGESTR"].ToString(), 42, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///入院日期
                    DateTime date1 = !string.IsNullOrEmpty(row["INWARDDATE"].ToString().Trim()) ? DateTime.Parse(row["INWARDDATE"].ToString()) : DateTime.Parse(row["ADMITDATE"].ToString());
                    string inDate = date1.ToString("yyyy-MM-dd HH:mm:ss");
                    string inHosDate = inDate.Substring(0, 4) + "年" + inDate.Substring(5, 2) + "月" + inDate.Substring(8, 2) + "日";
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院日期", inHosDate, m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    ///死亡日期
                    string deathDate = !string.IsNullOrEmpty(row["DEATHDATE"].ToString().Trim()) ? DateTime.Parse(row["DEATHDATE"].ToString()).ToString("yyyy-MM-dd") : string.Empty;
                    if (!string.IsNullOrEmpty(row["DEATHDATE"].ToString().Trim()))
                    {
                        string dateStr = DateTime.Parse(row["DEATHDATE"].ToString()).ToString("yyyy-MM-dd");
                        deathDate = dateStr.Substring(0, 4) + "年" + dateStr.Substring(5, 2) + "月" + dateStr.Substring(8, 2) + "日";
                    }
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "死亡日期", deathDate, m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///死亡时间
                    string deathTime = !string.IsNullOrEmpty(row["DEATHTIME"].ToString().Trim()) ? DateTime.Parse(row["DEATHTIME"].ToString()).ToString("HH:mm:ss") : string.Empty;
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "死亡时间", deathTime, m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    ///抢救次数
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "抢救次数", row["EMERGENCY_TIMES"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///主要诊断
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "主要诊断", row["DIAGNOSIS_NAME"].ToString(), 320, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///地址
                    string address = row["XZZADDRESSUNION"].ToString();
                    string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                    if (!string.IsNullOrEmpty(config))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(config);
                        XmlNodeList nodeList = doc.GetElementsByTagName("IsReadAddressInfo");
                        if (null != nodeList && nodeList.Count > 0)
                        {
                            string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                            if (cfgValue == "0")
                            {//省、市、县
                                address = row["XZZADDRESSUNION"].ToString();
                            }
                            else
                            {//长地址
                                address = row["XZZADDRESS"].ToString();
                            }
                        }
                    }
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "地    址", address, 320, "", m_DefaultFont) + m_ColWidth;

                    defaultPointX = i == 4 ? (defaultPointX + m_CardWidth + 10) : defaultPointX;
                    defaultPointY = i == 4 ? m_PointYStart : pointY + m_RowHeight + 15;
                    #endregion
                }
                ///页码
                string pageStr = "第 " + pageIndex + " 页";
                int pageStrWidth = TextRenderer.MeasureText(pageStr, m_DefaultFont).Width;
                g.DrawString(pageStr, m_DefaultFont, Brushes.Black, new RectangleF((m_Width - pageStrWidth) / 2, m_Height - 20, pageStrWidth, lineHeight + 4), sf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打印手术索引卡
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rowList"></param>
        /// <param name="pageIndex"></param>
        private void PrintPageOfOperationCard(Graphics g, List<DataRowView> rowList, int pageIndex)
        {
            try
            {
                if (null == rowList || rowList.Count() == 0)
                {
                    return;
                }
                int lineHeight = TextRenderer.MeasureText("高", m_DefaultFont).Height;
                int charWidth = TextRenderer.MeasureText("宽", m_DefaultFont).Height;

                float defaultPointX = m_PointXStart;
                float defaultPointY = m_PointYStart;
                for (int i = 0; i < rowList.Count(); i++)
                {
                    #region 初始化数据
                    DataRowView row = rowList[i];
                    float pointX = defaultPointX;
                    float pointY = defaultPointY;
                    #endregion

                    #region 边框
                    ///上边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth));
                    ///左边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth, pointY - m_ColWidth + m_CardHeight));
                    ///下边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth + m_CardHeight), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth + m_CardHeight));
                    ///右边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth + m_CardHeight));
                    #endregion

                    #region 卡片内容
                    ///手术索引卡
                    int titleWidth = TextRenderer.MeasureText("手术索引卡", m_DefaultFont).Width;
                    g.DrawString("手术索引卡", m_BoldFont, Brushes.Black, new PointF((m_CardWidth - titleWidth) / 2 + pointX, pointY));
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///病案号
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病 案 号", row["patid"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "科    室", row["OUTHOSDEPTNAME"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///姓名
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "姓    名", row["NAME"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    ///性别
                    string sexname = row["SEXID"].ToString() == "1" ? "男" : (row["SEXID"].ToString() == "2" ? "女" : "未知");
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "性    别", sexname, 30, "", m_DefaultFont) + 5;
                    ///年龄
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "年龄", row["AGESTR"].ToString(), 42, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///入院日期
                    DateTime date1 = !string.IsNullOrEmpty(row["INWARDDATE"].ToString().Trim()) ? DateTime.Parse(row["INWARDDATE"].ToString()) : DateTime.Parse(row["ADMITDATE"].ToString());
                    string inDate = date1.ToString("yyyy-MM-dd HH:mm:ss");
                    string inHosDate = inDate.Substring(0, 4) + "年" + inDate.Substring(5, 2) + "月" + inDate.Substring(8, 2) + "日";
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院日期", inHosDate, m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    ///出院日期
                    string outHosDate = string.Empty;
                    if (row["status"].ToString() == "1502" || row["status"].ToString() == "1503")
                    {
                        if (!string.IsNullOrEmpty(row["OUTWARDDATE"].ToString().Trim()) || !string.IsNullOrEmpty(row["OUTHOSDATE"].ToString().Trim()))
                        {
                            DateTime date2 = !string.IsNullOrEmpty(row["OUTHOSDATE"].ToString().Trim()) ? DateTime.Parse(row["OUTHOSDATE"].ToString()) : DateTime.Parse(row["OUTWARDDATE"].ToString());
                            string outDate = date2.ToString("yyyy-MM-dd HH:mm:ss");
                            outHosDate = outDate.Substring(0, 4) + "年" + outDate.Substring(5, 2) + "月" + outDate.Substring(8, 2) + "日";
                        }
                    }
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出院日期", outHosDate, m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;

                    ///手术名称
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "手术名称", row["operationname"].ToString(), 320, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///地址
                    string address = row["XZZADDRESSUNION"].ToString();
                    string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                    if (!string.IsNullOrEmpty(config))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(config);
                        XmlNodeList nodeList = doc.GetElementsByTagName("IsReadAddressInfo");
                        if (null != nodeList && nodeList.Count > 0)
                        {
                            string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                            if (cfgValue == "0")
                            {//省、市、县
                                address = row["XZZADDRESSUNION"].ToString();
                            }
                            else
                            {//长地址
                                address = row["XZZADDRESS"].ToString();
                            }
                        }
                    }
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "地    址", address, 320, "", m_DefaultFont) + m_ColWidth;

                    defaultPointX = i == 4 ? (defaultPointX + m_CardWidth + 10) : defaultPointX;
                    defaultPointY = i == 4 ? m_PointYStart : pointY + m_RowHeight + 45;
                    #endregion
                }
                ///页码
                string pageStr = "第 " + pageIndex + " 页";
                int pageStrWidth = TextRenderer.MeasureText(pageStr, m_DefaultFont).Width;
                g.DrawString(pageStr, m_DefaultFont, Brushes.Black, new RectangleF((m_Width - pageStrWidth) / 2, m_Height - 20, pageStrWidth, lineHeight + 4), sf);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 打印疾病索引卡
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rowList"></param>
        /// <param name="pageIndex"></param>
        private void PrintPageOfDiseaseCard(Graphics g, List<DataRowView> rowList, int pageIndex)
        {
            try
            {
                if (null == rowList || rowList.Count() == 0)
                {
                    return;
                }
                int lineHeight = TextRenderer.MeasureText("高", m_DefaultFont).Height;
                int charWidth = TextRenderer.MeasureText("宽", m_DefaultFont).Height;

                float defaultPointX = m_PointXStart;
                float defaultPointY = m_PointYStart;
                for (int i = 0; i < rowList.Count(); i++)
                {
                    #region 初始化数据
                    DataRowView row = rowList[i];
                    float pointX = defaultPointX;
                    float pointY = defaultPointY;
                    #endregion

                    #region 边框
                    ///上边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth));
                    ///左边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth, pointY - m_ColWidth + m_CardHeight));
                    ///下边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth, pointY - m_ColWidth + m_CardHeight), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth + m_CardHeight));
                    ///右边框
                    g.DrawLine(solidPen, new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth), new PointF(pointX - m_ColWidth + m_CardWidth, pointY - m_ColWidth + m_CardHeight));
                    #endregion

                    #region 卡片内容
                    ///手术索引卡
                    int titleWidth = TextRenderer.MeasureText("疾病索引卡", m_DefaultFont).Width;
                    g.DrawString("疾病索引卡", m_BoldFont, Brushes.Black, new PointF((m_CardWidth - titleWidth) / 2 + pointX, pointY));
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///病案号
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病 案 号", row["patid"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "科    室", row["OUTHOSDEPTNAME"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///姓名
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "姓    名", row["NAME"].ToString(), m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    ///性别
                    string sexname = row["SEXID"].ToString() == "1" ? "男" : (row["SEXID"].ToString() == "2" ? "女" : "未知");
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "性    别", sexname, 30, "", m_DefaultFont) + 5;
                    ///年龄
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "年龄", row["AGESTR"].ToString(), 42, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///入院日期
                    DateTime date1 = !string.IsNullOrEmpty(row["INWARDDATE"].ToString().Trim()) ? DateTime.Parse(row["INWARDDATE"].ToString()) : DateTime.Parse(row["ADMITDATE"].ToString());
                    string inDate = date1.ToString("yyyy-MM-dd HH:mm:ss");
                    string inHosDate = inDate.Substring(0, 4) + "年" + inDate.Substring(5, 2) + "月" + inDate.Substring(8, 2) + "日";
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院日期", inHosDate, m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    ///出院日期
                    string outHosDate = string.Empty;
                    if (row["status"].ToString() == "1502" || row["status"].ToString() == "1503")
                    {
                        if (!string.IsNullOrEmpty(row["OUTWARDDATE"].ToString().Trim()) || !string.IsNullOrEmpty(row["OUTHOSDATE"].ToString().Trim()))
                        {
                            DateTime date2 = !string.IsNullOrEmpty(row["OUTHOSDATE"].ToString().Trim()) ? DateTime.Parse(row["OUTHOSDATE"].ToString()) : DateTime.Parse(row["OUTWARDDATE"].ToString());
                            string outDate = date2.ToString("yyyy-MM-dd HH:mm:ss");
                            outHosDate = outDate.Substring(0, 4) + "年" + outDate.Substring(5, 2) + "月" + outDate.Substring(8, 2) + "日";
                        }
                    }
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出院日期", outHosDate, m_DefaultLineWidth, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;

                    ///主要诊断
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "主要诊断", row["DIAGNOSIS_NAME"].ToString(), 320, "", m_DefaultFont) + m_ColWidth;
                    pointX = defaultPointX;
                    pointY += m_RowHeight;
                    ///地址
                    string address = row["XZZADDRESSUNION"].ToString();
                    string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                    if (!string.IsNullOrEmpty(config))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(config);
                        XmlNodeList nodeList = doc.GetElementsByTagName("IsReadAddressInfo");
                        if (null != nodeList && nodeList.Count > 0)
                        {
                            string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                            if (cfgValue == "0")
                            {//省、市、县
                                address = row["XZZADDRESSUNION"].ToString();
                            }
                            else
                            {//长地址
                                address = row["XZZADDRESS"].ToString();
                            }
                        }
                    }
                    pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "地    址", address, 320, "", m_DefaultFont) + m_ColWidth;

                    defaultPointX = i == 4 ? (defaultPointX + m_CardWidth + 10) : defaultPointX;
                    defaultPointY = i == 4 ? m_PointYStart : pointY + m_RowHeight + 45;
                    #endregion
                }
                ///页码
                string pageStr = "第 " + pageIndex + " 页";
                int pageStrWidth = TextRenderer.MeasureText(pageStr, m_DefaultFont).Width;
                g.DrawString(pageStr, m_DefaultFont, Brushes.Black, new RectangleF((m_Width - pageStrWidth) / 2, m_Height - 20, pageStrWidth, lineHeight + 4), sf);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 绘制带下划线的元素
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="lineHeight"></param>
        /// <param name="charWidth"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="underLineWidth"></param>
        /// <param name="endName"></param>
        /// <returns></returns>
        private float DrawNameAndValueAndUnderLine(Graphics g, float pointX, float pointY, int lineHeight, int charWidth,
            string name, string value, int underLineWidth, string endName, Font font)
        {
            g.DrawString(name, font, Brushes.Black, new PointF(pointX, pointY));
            int widthName = TextRenderer.MeasureText(name, font).Width;
            int widthValue = underLineWidth;
            pointX = pointX + widthName;

            int valueLength = TextRenderer.MeasureText(value, font).Width;
            if (valueLength >= underLineWidth)
            {
                if (TextRenderer.MeasureText(value, font).Width >= underLineWidth)
                {
                    //g.DrawString(value, font, Brushes.Black, new RectangleF(pointX, pointY - 10, widthValue, lineHeight + 16), sfVertical);
                    g.DrawString(value, m_SmallFont, Brushes.Black, new RectangleF(pointX, pointY, widthValue, lineHeight + 16), sfVertical);
                }
                else
                {
                    g.DrawString(value, font, Brushes.Black, new RectangleF(pointX, pointY, widthValue, lineHeight + 4), sf);
                }
            }
            else
            {
                g.DrawString(value, font, Brushes.Black, new RectangleF(pointX, pointY, widthValue, lineHeight + 4), sf);
            }
            g.DrawLine(pen, new PointF(pointX, pointY + lineHeight), new PointF(pointX + widthValue, pointY + lineHeight));

            pointX = pointX + widthValue;

            if (endName != "")
            {
                g.DrawString(endName, font, Brushes.Black, new PointF(pointX, pointY));
                int widthEndName = TextRenderer.MeasureText(endName, font).Width;
                pointX = pointX + widthEndName + 2;
            }
            else
            {
                pointX = pointX + 2;
            }
            return pointX;
        }
        #endregion

        #region 创建文件夹
        /// <summary>
        /// 在当前程序所在文件创建文件夹
        /// </summary>
        /// <returns></returns>
        private string CreateFolder()
        {
            try
            {
                string folder = AppDomain.CurrentDomain.BaseDirectory;
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                if (!Directory.Exists(folder + "PrintIndexCard\\"))
                {
                    Directory.CreateDirectory(folder + "PrintIndexCard\\");
                }
                DeleteMetaFile(AppDomain.CurrentDomain.BaseDirectory + "PrintIndexCard\\");
                return folder + "PrintIndexCard\\";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="folder"></param>
        private void DeleteMetaFile(string folder)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(folder);
                foreach (FileInfo fi in dirInfo.GetFiles("*.wmf"))
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception ex)
                    { }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion




    }
}
