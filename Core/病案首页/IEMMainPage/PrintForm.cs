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

namespace YidanSoft.Core.IEMMainPage
{
    public partial class PrintForm : DevExpress.XtraEditors.XtraForm
    {
        #region Property && Field

        PrintDocument m_PrintDocument;

        int m_PageWidth = 800;//用于设定PictureBox的宽度
        int m_PageHeight = 1050;//用于设定PictureBox的高度

        float m_PointYTitle = 40; //“首页”标题Y轴方向的值
        Font m_DefaultFont = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_DefaultValueFont = new Font("宋体", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_SmallFont = new Font("宋体", 10f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_SmallFont1 = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Pixel);

        float m_PointXPayType = 15; //“医疗付款方式”左上角点的X轴方向的值
        float m_PointYPayType = 120; //“医疗付款方式”左上角点的Y轴方向的值

        StringFormat sf = new StringFormat();
        StringFormat sfVertical = new StringFormat();

        int m_PageIndex = 1;

        /// <summary>
        /// 病案首页实体类
        /// </summary>
        Print_IemMainPageInfo m_IemMainPageEntity = new Print_IemMainPageInfo();

        /// <summary>
        /// 第一页
        /// </summary>
        Metafile mf1;

        /// <summary>
        /// 第二页
        /// </summary>
        Metafile mf2;

        #endregion

        #region .ctor

        public PrintForm(Print_IemMainPageInfo iemMainPageEntity)
        {
            InitializeComponent();
            m_IemMainPageEntity = iemMainPageEntity;
        }

        #endregion

        #region Method

        public void InitPreviewControl()
        {
            m_PrintDocument = new PrintDocument();
            m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 初始化变量
        /// </summary>
        private void InitVariable()
        {
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            sfVertical.LineAlignment = StringAlignment.Center;
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
            Bitmap bmp1 = new Bitmap(m_PageWidth, m_PageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g1 = Graphics.FromImage(bmp1);
            Rectangle rect = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
            string filePath = "C:\\" + Guid.NewGuid().ToString() + ".wmf";
            mf1 = new Metafile(filePath, g1.GetHdc(), rect, MetafileFrameUnit.Pixel);
            g1 = Graphics.FromImage(mf1);
            g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            PrintFirstPage(g1);
            g1.Save();
            g1.Dispose();

            pictureBox1.BackgroundImage = mf1;
            pictureBox1.Width = m_PageWidth;
            pictureBox1.Height = m_PageHeight;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;

            Bitmap bmp2 = new Bitmap(m_PageWidth, m_PageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g2 = Graphics.FromImage(bmp2);
            filePath = "C:\\" + Guid.NewGuid().ToString() + ".wmf";
            mf2 = new Metafile(filePath, g2.GetHdc(), rect, MetafileFrameUnit.Pixel);
            g2 = Graphics.FromImage(mf2);
            g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            PrintSecondPage(g2);
            g2.Save();
            g2.Dispose();

            pictureBox2.BackgroundImage = mf2;
            pictureBox2.Width = m_PageWidth;
            pictureBox2.Height = m_PageHeight;
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
            InitVariable();
            InitPrinterList();
            InitPageSize();
            InitPreviewControl();
            InitMetaFile();
            button1.Focus();
        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            int width = m_PageWidth * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            int height = m_PageHeight * Convert.ToInt32(trackBarControl1.EditValue.ToString().TrimEnd('%')) / 100;
            pictureBox1.Width = width;
            pictureBox1.Height = height;
            pictureBox2.Width = width;
            pictureBox2.Height = height;
            ReLocationPicture();
        }

        private void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            string pageType = lookUpEditPageSize.EditValue.ToString();
            m_PrintDocument.PrinterSettings.PrinterName = lookUpEditPrinter.EditValue.ToString();

            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.Document = m_PrintDocument;
            PaperSize p = null;
            foreach (PaperSize ps in pageSetupDialog.Document.PrinterSettings.PaperSizes)
            {
                if (ps.PaperName.Equals(pageType))//这里设置纸张大小,但必须是定义好的  
                    p = ps;
            }
            pageSetupDialog.Document.DefaultPageSettings.PaperSize = p;
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
            SaveFileDialog file = new SaveFileDialog();
            file.InitialDirectory = "D:\\";
            file.Filter = "Image   Files(*.jpg)|*.jpg";
            file.FileName = "病案首页";
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (mf1 != null)
                {
                    mf1.Save(file.FileName);
                }
            }
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (mf2 != null)
                {
                    mf2.Save(file.FileName);
                }
            }
        }

        void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (m_PageIndex == 1)
            {
                if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                {
                    e.Graphics.DrawImage(mf1, new RectangleF(-10, 0, m_PageWidth, m_PageHeight));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                {
                    e.Graphics.DrawImage(mf1, new RectangleF(-15, 0, m_PageWidth * 0.88f, m_PageHeight * 0.88f));
                }

                m_PageIndex++;
                e.HasMorePages = false;
            }
            else if (m_PageIndex == 2)
            {
                if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.A4)
                {
                    e.Graphics.DrawImage(mf2, new RectangleF(-10, 0, m_PageWidth, m_PageHeight));
                }
                else if (m_PrintDocument.DefaultPageSettings.PaperSize.Kind == PaperKind.B5)
                {
                    e.Graphics.DrawImage(mf2, new RectangleF(-15, 0, m_PageWidth * 0.88f, m_PageHeight * 0.88f));
                }
                m_PageIndex = 1;
                e.HasMorePages = false;
            }
        }

        #endregion

        #region ******************************************************* 绘制病案首页 **************************************************************

        /// <summary>
        /// 绘制第一页
        /// </summary>
        /// <param name="g"></param>
        void PrintFirstPage(Graphics g)
        {
            float pointY;
            DrawTitle(g);
            pointY = PrintPayType(g);
            pointY = PrintPatientBaseInfo(g, pointY + 15);
            pointY = PrintOutHospitalDiaglosis(g, pointY + 25);
            pointY = PrintFristPageOther(g, pointY);
        }

        /// <summary>
        /// 绘制第二页
        /// </summary>
        /// <param name="g"></param>
        void PrintSecondPage(Graphics g)
        {
            float pointY;
            pointY = PrintOperation(g, m_PointYTitle) + 30;
            if (m_IemMainPageEntity.IemObstetricsBaby != null)
            {
                pointY = FuKeChanKeYingEr(g, pointY) + 15;
            }
            PrintSecondPageOther(g, pointY);
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        /// <param name="g"></param>
        private void DrawTitle(Graphics g)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Font font1 = new Font("宋体", 20f, FontStyle.Regular, GraphicsUnit.Pixel);
            Font font2 = new Font("宋体", 25f, FontStyle.Bold, GraphicsUnit.Pixel);
            string hospitalName = m_IemMainPageEntity.IemBasicInfo.HospitalName.Trim();
            g.DrawString(hospitalName, font1, Brushes.Black, new RectangleF(0f, m_PointYTitle, m_PageWidth, 30), sf);
            g.DrawString("住 院 病 案 首 页", font2, Brushes.Black, new RectangleF(0f, m_PointYTitle + 30, m_PageWidth, 50), sf);
        }

        /// <summary>
        /// 绘制医疗付款方式一行
        /// </summary>
        /// <param name="g"></param>
        private float PrintPayType(Graphics g)
        {
            Font font1 = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);

            string payType = m_IemMainPageEntity.IemBasicInfo.PayName.Trim();

            g.DrawString("医疗付款方式：" + payType, font1, Brushes.Red, new PointF(m_PointXPayType, m_PointYPayType));

            string inTime = m_IemMainPageEntity.IemBasicInfo.InCount;
            g.DrawString("第 " + inTime + " 次住院", font1, Brushes.Black, new PointF(355, m_PointYPayType));

            string recordNo = m_IemMainPageEntity.IemBasicInfo.PatNoOfHis;
            g.DrawString("病案号：" + recordNo, font1, Brushes.Black, new PointF(620, m_PointYPayType));

            Pen pen = new Pen(Brushes.Black, 2);
            g.DrawLine(pen, new PointF(m_PointXPayType, m_PointYPayType + 20), new PointF(m_PointXPayType + 770, m_PointYPayType + 20));

            return m_PointYPayType + 21;
        }

        /// <summary>
        /// 绘制病人基本信息
        /// </summary>
        /// <param name="g"></param>
        private float PrintPatientBaseInfo(Graphics g, float pointY)
        {
            Font font = m_DefaultFont;
            int lineHeight = TextRenderer.MeasureText("高", font).Height;
            int charWidth = TextRenderer.MeasureText("宽", font).Height;
            float interval = 38; //行间距

            float pointStartX = m_PointXPayType + 12;
            float pointX = pointStartX;

            #region 1
            //姓名
            string patientName = m_IemMainPageEntity.IemBasicInfo.Name;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "姓名", patientName, 60, "") + 15;

            //性别
            g.DrawString("性别", font, Brushes.Black, new PointF(pointX, pointY));
            string gender = m_IemMainPageEntity.IemBasicInfo.SexID;
            pointX = pointX + TextRenderer.MeasureText("年龄", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, gender, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.男  2.女") + 10;

            //出生
            string birth = m_IemMainPageEntity.IemBasicInfo.Birth;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生", birth, 110, "") + 15;

            //年龄
            string age = m_IemMainPageEntity.IemBasicInfo.Age;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "年龄", age, 50, "") + 15;

            //婚姻
            g.DrawString("婚姻", font, Brushes.Black, new PointF(pointX, pointY));
            string marriage = m_IemMainPageEntity.IemBasicInfo.Marital;
            pointX = pointX + TextRenderer.MeasureText("婚姻", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, marriage, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.未  2.已  3.离  4.丧") + 10;
            #endregion

            #region 2
            pointY += interval;
            pointX = pointStartX;

            //职业
            string jobName = m_IemMainPageEntity.IemBasicInfo.JobName;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "职业", jobName, 60, "") + 15;

            //出生地
            string birthPlace = m_IemMainPageEntity.IemBasicInfo.CountyName;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生地", birthPlace, 150, "") + 15;

            //民族
            string mingZu = m_IemMainPageEntity.IemBasicInfo.NationName;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "民族", mingZu, 50, "") + 15;

            //国籍
            string guoJi = m_IemMainPageEntity.IemBasicInfo.NationalityName;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "国籍", guoJi, 50, "") + 15;

            //身份证号
            string patientID = m_IemMainPageEntity.IemBasicInfo.IDNO;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "身份证号", patientID, 140, "") + 10;
            #endregion

            #region 3
            pointY += interval;
            pointX = pointStartX;

            //工作单位及地址
            string jobAddress = m_IemMainPageEntity.IemBasicInfo.OfficePlace;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "工作单位及地址", jobAddress, 309, "") + 15;

            //电话
            string telNumber = m_IemMainPageEntity.IemBasicInfo.OfficeTEL;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "电话", telNumber, 120, "") + 10;

            //邮政编码
            string jobYouzhenbianma = m_IemMainPageEntity.IemBasicInfo.OfficePost;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "邮政编码", jobYouzhenbianma, 73, "") + 10;
            #endregion

            #region 4
            pointY += interval;
            pointX = pointStartX;

            //户口地址
            string liveAddress = m_IemMainPageEntity.IemBasicInfo.NativeAddress;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "户口地址", liveAddress, 523, "") + 10;

            //邮政编码
            string liveYouzhenbianma = m_IemMainPageEntity.IemBasicInfo.NativePost;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "邮政编码", liveYouzhenbianma, 73, "") + 10;
            #endregion

            #region 5
            pointY += interval;
            pointX = pointStartX;

            //联系人姓名
            string contactName = m_IemMainPageEntity.IemBasicInfo.ContactPerson;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "联系人姓名", contactName, 50, "") + 10;

            //关系
            string relation = m_IemMainPageEntity.IemBasicInfo.Relationship;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "关系", relation, 40, "") + 10;

            //地址
            string contactAddress = m_IemMainPageEntity.IemBasicInfo.ContactAddress;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "地址", contactAddress, 325, "") + 10;

            //电话
            string contactTel = m_IemMainPageEntity.IemBasicInfo.ContactTEL;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "电话", contactTel, 100, "") + 10;

            #endregion

            #region 6
            pointY += interval;
            pointX = pointStartX;

            //入院日期
            string inTime = m_IemMainPageEntity.IemBasicInfo.AdmitDate;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院日期", inTime, 150, "") + 30;

            //入院科别
            string inSection = m_IemMainPageEntity.IemBasicInfo.AdmitDeptName;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院科别", inSection, 115, "") + 10;

            //病区
            string inNurseWard = m_IemMainPageEntity.IemBasicInfo.AdmitWardName;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病区", inNurseWard, 73, "") + 10;

            //转科科别
            string shiftSection = m_IemMainPageEntity.IemBasicInfo.Trans_AdmitDept;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "转科科别", shiftSection, 115, "") + 10;

            #endregion

            #region 7
            pointY += interval;
            pointX = pointStartX;

            //出院日期
            string outTime = m_IemMainPageEntity.IemBasicInfo.OutWardDate;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出院日期", outTime, 150, "") + 30;

            //出院科别
            string outSection = m_IemMainPageEntity.IemBasicInfo.OutHosDeptName;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出院科别", outSection, 115, "") + 10;

            //病区
            string outNurseWard = m_IemMainPageEntity.IemBasicInfo.OutHosWardName;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病区", outNurseWard, 73, "") + 10;

            //实际住院天数
            string InDay = m_IemMainPageEntity.IemBasicInfo.ActualDays;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "实际住院", InDay, 100, "天") + 10;

            #endregion

            #region 8
            pointY += interval;
            pointX = pointStartX;

            //门（急）诊诊断
            string menJiZhenDiaglosis = m_IemMainPageEntity.IemDiagInfo.OutDiag;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "门（急）诊诊断", menJiZhenDiaglosis, 390, "") + 10;

            //入院时情况
            g.DrawString("入院时情况", font, Brushes.Black, new PointF(pointX, pointY));
            string inState = m_IemMainPageEntity.IemDiagInfo.AdmitInfo;
            pointX = pointX + TextRenderer.MeasureText("入院时情况", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, inState, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.危  2.急  3.一般") + 10;
            #endregion

            #region 9
            pointY += interval;
            pointX = pointStartX;

            //入院诊断
            string InDiaglosis = m_IemMainPageEntity.IemDiagInfo.InDiag;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院诊断", InDiaglosis, 432, "") + 10;

            //入院后确诊日期
            string inConfirmTime = m_IemMainPageEntity.IemDiagInfo.In_Check_Date;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院后确诊日期", inConfirmTime, 120, "") + 30;
            #endregion

            return pointY;
        }

        /// <summary>
        /// 出院诊断
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintOutHospitalDiaglosis(Graphics g, float pointY)
        {
            //表格的行高
            float lineHeight = 30f;
            float pointX = m_PointXPayType;
            float firstColumnWidth = 500f;
            Font font = m_SmallFont;
            float offsetX = 12f;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            #region TableHead
            g.DrawString("出  院  诊  断", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, firstColumnWidth, lineHeight * 2), sf);
            g.DrawLine(Pens.Black, new PointF(pointX + firstColumnWidth, pointY), new PointF(pointX + firstColumnWidth, pointY + lineHeight * 10));
            pointX = pointX + firstColumnWidth;
            g.DrawString("出  院  情  况", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, lineHeight * 5, lineHeight), sf);
            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + lineHeight * 5, pointY));//横线

            g.DrawString("1治愈", font, Brushes.Black, new RectangleF(pointX, pointY, lineHeight, lineHeight), sf);
            pointX = pointX + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeight * 9));


            g.DrawString("2好转", font, Brushes.Black, new RectangleF(pointX, pointY, lineHeight, lineHeight), sf);
            pointX = pointX + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeight * 9));

            g.DrawString("3未愈", font, Brushes.Black, new RectangleF(pointX, pointY, lineHeight, lineHeight), sf);
            pointX = pointX + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeight * 9));

            g.DrawString("4死亡", font, Brushes.Black, new RectangleF(pointX, pointY, lineHeight, lineHeight), sf);
            pointX = pointX + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeight * 9));

            g.DrawString("5其他", font, Brushes.Black, new RectangleF(pointX, pointY, lineHeight, lineHeight), sf);
            pointX = pointX + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY - lineHeight), new PointF(pointX, pointY + lineHeight * 9));

            g.DrawString("IDC-10", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY - lineHeight, 100, lineHeight * 2), sf);

            pointX = m_PointXPayType;
            pointY = pointY + lineHeight;

            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));
            #endregion

            #region TableBody

            float pointYTableBodyStart = pointY;

            g.DrawString("主要诊断", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, lineHeight), sfVertical);
            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));
            g.DrawString("其他诊断", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, lineHeight), sfVertical);

            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            g.DrawString("医院感染名称", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, lineHeight), sfVertical);
            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            #endregion

            #region TableBodyValue

            DataTable dtOutHospitalDiagnosis = m_IemMainPageEntity.IemDiagInfo.OutDiagTable;

            int offsetXDiagnosisValue = 100;
            float tableBodyValuePointX;
            pointX = m_PointXPayType;
            float pointYTableBodyStartTemp = pointYTableBodyStart;
            for (int i = 0; i < dtOutHospitalDiagnosis.Rows.Count; i++)
            {
                if (i > 6) break;//出院诊断最多支持7行

                tableBodyValuePointX = pointX + offsetX + offsetXDiagnosisValue;

                //诊断名称
                string diagnosisName = dtOutHospitalDiagnosis.Rows[i]["Diagnosis_Name"].ToString();
                g.DrawString(diagnosisName, m_DefaultValueFont, Brushes.Black,
                    new RectangleF(tableBodyValuePointX, pointYTableBodyStart, firstColumnWidth - offsetX - offsetXDiagnosisValue, lineHeight + 2), sfVertical);

                tableBodyValuePointX = pointX + firstColumnWidth;

                //出院情况
                string status = dtOutHospitalDiagnosis.Rows[i]["Status_id"].ToString();
                int statusID = 0;
                if (status != "")
                {
                    statusID = Convert.ToInt32(status);
                }
                if (statusID >= 1 && statusID <= 5)
                {
                    g.DrawString("√", m_DefaultFont, Brushes.Black,
                        new RectangleF(tableBodyValuePointX + (statusID - 1) * lineHeight, pointYTableBodyStart, lineHeight, lineHeight + 2), sf);
                }

                tableBodyValuePointX = tableBodyValuePointX + lineHeight * 5 + 10;

                //ICD-10
                string icd10 = dtOutHospitalDiagnosis.Rows[i]["Diagnosis_Code"].ToString();

                if (TextRenderer.MeasureText(icd10, m_DefaultValueFont).Width < 100)
                {
                    g.DrawString(icd10, m_DefaultValueFont, Brushes.Black, new RectangleF(tableBodyValuePointX, pointYTableBodyStart, 100, lineHeight), sfVertical);
                }
                else
                {
                    g.DrawString(icd10, m_SmallFont1, Brushes.Black, new RectangleF(tableBodyValuePointX, pointYTableBodyStart, 100, lineHeight), sfVertical);
                }

                pointYTableBodyStart = pointYTableBodyStart + lineHeight;
            }

             #region 医院感染名称

            offsetXDiagnosisValue += 0;
            tableBodyValuePointX = pointX + offsetX + offsetXDiagnosisValue;
            pointYTableBodyStart = pointYTableBodyStartTemp + lineHeight * 7;

            //医院感染名称
            string zymosisName = m_IemMainPageEntity.IemDiagInfo.ZymosisName;
            g.DrawString(zymosisName, m_DefaultValueFont, Brushes.Black, 
                new RectangleF(tableBodyValuePointX, pointYTableBodyStart, firstColumnWidth - offsetX - offsetXDiagnosisValue, lineHeight + 2), sfVertical);

            tableBodyValuePointX = pointX + firstColumnWidth;

            //出院情况
            string zymosisState = m_IemMainPageEntity.IemDiagInfo.ZymosisState;
            int status_ID = 0;
            if (zymosisState != "")
            {
                status_ID = Convert.ToInt32(zymosisState);
            }
            if (status_ID >= 1 && status_ID <= 5)
            {
                g.DrawString("√", m_DefaultValueFont, Brushes.Black,
                    new RectangleF(tableBodyValuePointX + (status_ID - 1) * lineHeight, pointYTableBodyStart, lineHeight, lineHeight + 2), sf);
            }

            tableBodyValuePointX = tableBodyValuePointX + lineHeight * 5 + 10;

            //ICD-10
            string zymosisCode = m_IemMainPageEntity.IemDiagInfo.ZymosisCode;
            g.DrawString(zymosisCode, m_DefaultValueFont, Brushes.Black, new RectangleF(tableBodyValuePointX, pointYTableBodyStart, 100, lineHeight), sfVertical);
            #endregion

            #endregion

            return pointY;
        }

        /// <summary>
        /// 第一页中出院诊断下面的部分
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintFristPageOther(Graphics g, float pointY)
        {
            int RowHeight = 30;
            float pointStartX = m_PointXPayType;
            Font font = m_DefaultFont;
            int offsetY = 9;
            float offsetX = 12f;
            int lineHeight = TextRenderer.MeasureText("高", font).Height;
            int charWidth = TextRenderer.MeasureText("宽", font).Height;

            float pointX = pointStartX;

            //病理诊断
            g.DrawString("病理诊断", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            int valueStartPointX = Convert.ToInt32(pointX + offsetX + TextRenderer.MeasureText("病理诊断", m_DefaultFont).Width + 11);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Pathology_Diagnosis_Name, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 400, RowHeight), sfVertical);
            pointY = pointY + RowHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));


            //损伤、中毒的外部因素：
            pointX = pointStartX;

            g.DrawString("损伤、中毒的外部因素：", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(pointX + offsetX + TextRenderer.MeasureText("损伤、中毒的外部因素：", m_DefaultFont).Width + 10);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Hurt_Toxicosis_Element, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 400, RowHeight), sfVertical);

            pointY = pointY + RowHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));


            //药物过敏
            pointX = pointStartX;

            g.DrawString("药物过敏", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(pointX + offsetX + TextRenderer.MeasureText("药物过敏", m_DefaultFont).Width + 11);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Allergic_Drug, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 240, RowHeight), sfVertical);

            pointX = pointX + 250;

            string HbsAg = m_IemMainPageEntity.IemDiagInfo.Hbsag;
            g.DrawString("   HBsAg  ", m_DefaultFont, Brushes.Black, new RectangleF(pointX + 12, pointY + 2, 400, RowHeight), sfVertical);
            pointX = pointX + TextRenderer.MeasureText("   HBsAg  ", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + offsetY, HbsAg, lineHeight);

            string HCVAb = m_IemMainPageEntity.IemDiagInfo.Hcv_Ab;
            g.DrawString("   HCV-Ab  ", m_DefaultFont, Brushes.Black, new RectangleF(pointX + 12, pointY + 2, 400, RowHeight), sfVertical);
            pointX = pointX + TextRenderer.MeasureText("   HCV-Ab  ", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + offsetY, HCVAb, lineHeight);

            string HIVAb = m_IemMainPageEntity.IemDiagInfo.Hiv_Ab;
            g.DrawString("   HIV-Ab  ", m_DefaultFont, Brushes.Black, new RectangleF(pointX + 12, pointY + 2, 400, RowHeight), sfVertical);
            pointX = pointX + TextRenderer.MeasureText("   HIV-Ab  ", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + offsetY, HIVAb, lineHeight) + 20;

            pointX = DrawSelectItem(g, pointX, pointY + offsetY, "0.未做  1.阴性  2.阳性");
            pointY = pointY + RowHeight;
            pointX = pointStartX;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            //诊断符合情况
            g.DrawString("诊断符合情况", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);

            pointX = pointX + 100;

            string menZhenYuChuYuan = m_IemMainPageEntity.IemDiagInfo.Opd_Ipd_Id;
            g.DrawString("       门诊与出院  ", m_DefaultFont, Brushes.Black, new RectangleF(pointX + 12, pointY + 2, 400, RowHeight), sfVertical);
            pointX = pointX + TextRenderer.MeasureText("       门诊与出院  ", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + offsetY, menZhenYuChuYuan, lineHeight);

            string ruyuanyuchuyuan = m_IemMainPageEntity.IemDiagInfo.In_Out_Inpatinet_Id;
            g.DrawString("       入院与出院  ", m_DefaultFont, Brushes.Black, new RectangleF(pointX + 12, pointY + 2, 400, RowHeight), sfVertical);
            pointX = pointX + TextRenderer.MeasureText("       入院与出院  ", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + offsetY, ruyuanyuchuyuan, lineHeight);

            string suqianyusuhou = m_IemMainPageEntity.IemDiagInfo.Before_After_Or_Id;
            g.DrawString("       术前与术后  ", m_DefaultFont, Brushes.Black, new RectangleF(pointX + 12, pointY + 2, 400, RowHeight), sfVertical);
            pointX = pointX + TextRenderer.MeasureText("       术前与术后  ", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + offsetY, suqianyusuhou, lineHeight);

            string linchuangyubingli = m_IemMainPageEntity.IemDiagInfo.Clinical_Pathology_Id;
            g.DrawString("       临床与病理  ", m_DefaultFont, Brushes.Black, new RectangleF(pointX + 12, pointY + 2, 400, RowHeight), sfVertical);
            pointX = pointX + TextRenderer.MeasureText("       临床与病理  ", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + offsetY, linchuangyubingli, lineHeight);

            pointY = pointY + RowHeight;
            pointX = pointStartX;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointX = pointX + 100;

            string fangsheyubingli = m_IemMainPageEntity.IemDiagInfo.Pacs_Pathology_Id;
            g.DrawString("       放射与病理  ", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            pointX = pointX + TextRenderer.MeasureText("       放射与病理  ", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + offsetY, fangsheyubingli, lineHeight) + 20;

            pointX = DrawSelectItem(g, pointX, pointY + offsetY, "0.未做 1.符合 2.不符合 3.不肯定") + 65;

            //抢救次数
            string saveCount = m_IemMainPageEntity.IemDiagInfo.Save_Times;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + offsetY, lineHeight, charWidth, "抢救", saveCount, 30, "次") + 10;

            //成功次数
            string saveSuccessCount = m_IemMainPageEntity.IemDiagInfo.Success_Times;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + offsetY, lineHeight, charWidth, "成功", saveSuccessCount, 30, "次") + 10;

            pointY = pointY + RowHeight;
            pointX = pointStartX;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            //科主任
            g.DrawString("科主任", font, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("科主任", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Section_Director, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);
            pointX = pointX + 190;
            //主（副主）任医师
            g.DrawString("主（副主）任医师", font, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("主（副主）任医师", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Director, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);
            pointX = pointX + 190;
            //主治医师
            g.DrawString("主治医师", font, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("主治医师", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Vs_Employee_Code, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);
            pointX = pointX + 190;
            //住院医师
            g.DrawString("住院医师", font, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("住院医师", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Resident_Employee_Code, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);

            pointY = pointY + RowHeight;
            pointX = pointStartX;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            //进修医师
            g.DrawString("进修医师", font, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("进修医师", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Refresh_Employee_Code, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);
            pointX = pointX + 190;
            //研究生实习医师
            g.DrawString("研究生实习医师", font, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("研究生实习医师", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Master_Interne, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);
            pointX = pointX + 190;
            //实习医师
            g.DrawString("实习医师", font, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("实习医师", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Interne, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);
            pointX = pointX + 190;
            //编码员
            g.DrawString("编码员", font, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("编码员", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Coding_User, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);
            pointX = pointX + 190;

            pointY = pointY + RowHeight;
            pointX = pointStartX;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            //病案质量
            g.DrawString("病案质量", font, Brushes.Black, new RectangleF(pointX + offsetX, pointY + 2, 400, RowHeight), sfVertical);
            pointX = pointX + TextRenderer.MeasureText("病案质量  ", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + offsetY, m_IemMainPageEntity.IemDiagInfo.Medical_Quality_Id, lineHeight) + 20;

            pointX = DrawSelectItem(g, pointX, pointY + offsetY, "  1.甲  2.乙  3.丙") + 20;

            //质控医师
            g.DrawString("质控医师", font, Brushes.Black, new RectangleF(pointX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("质控医师", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Quality_Control_Doctor, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);
            pointX = pointX + 160;

            //质控护士
            g.DrawString("质控护士", font, Brushes.Black, new RectangleF(pointX, pointY + 2, 400, RowHeight), sfVertical);
            valueStartPointX = Convert.ToInt32(TextRenderer.MeasureText("质控护士", font).Width + 5);
            g.DrawString(m_IemMainPageEntity.IemDiagInfo.Quality_Control_Nurse, m_DefaultValueFont, Brushes.Black,
                new RectangleF(pointX + offsetX + valueStartPointX, pointY + 2, 100, RowHeight), sfVertical);
            pointX = pointX + 160;

            //日期
            string time = m_IemMainPageEntity.IemDiagInfo.Quality_Control_Date;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + offsetY, lineHeight, charWidth, "日期：", time, 110, "") + 15;

            pointY = pointY + RowHeight;
            pointX = pointStartX;
            Pen pen = new Pen(Brushes.Black, 2);
            g.DrawLine(pen, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            return pointY;
        }

        /// <summary>
        /// 手术
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintOperation(Graphics g, float pointY)
        {
            //表格的行高
            float lineHeadHeight = 40f;
            float lineHeight = 30f;
            float pointX = m_PointXPayType;
            Font font = m_DefaultFont;

            int ColumnWidth1 = 100;
            int ColumnWidth2 = 115;
            int ColumnWidth3 = 165;
            int ColumnWidth4 = 180;
            int ColumnWidth5 = 70;
            int ColumnWidth6 = 70;
            int ColumnWidth7 = 70;

            #region 表头

            Pen solidPen = new Pen(Brushes.Black, 2);
            g.DrawLine(solidPen, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            g.DrawString("手术、操作", font, Brushes.Black, new RectangleF(pointX, pointY + 3, ColumnWidth1, lineHeadHeight / 2), sf);
            g.DrawString("编   码", font, Brushes.Black, new RectangleF(pointX, pointY + lineHeadHeight / 2 + 1, ColumnWidth1, lineHeadHeight / 2), sf);

            pointX = pointX + ColumnWidth1;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeadHeight + lineHeight * 4));

            g.DrawString("手术、操作", font, Brushes.Black, new RectangleF(pointX, pointY + 3, ColumnWidth2, lineHeadHeight / 2), sf);
            g.DrawString("日   期", font, Brushes.Black, new RectangleF(pointX, pointY + lineHeadHeight / 2 + 1, ColumnWidth2, lineHeadHeight / 2), sf);

            pointX = pointX + ColumnWidth2;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeadHeight + lineHeight * 4));

            g.DrawString("手术、操作名称", font, Brushes.Black, new RectangleF(pointX, pointY + 3, ColumnWidth3, lineHeadHeight), sf);

            pointX = pointX + ColumnWidth3;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeadHeight + lineHeight * 4));

            g.DrawString("手术、操作医师", font, Brushes.Black, new RectangleF(pointX, pointY + 3, ColumnWidth4, lineHeadHeight / 2), sf);
            g.DrawLine(Pens.Black, new PointF(pointX, pointY + lineHeadHeight / 2), new PointF(pointX + ColumnWidth4, pointY + lineHeadHeight / 2));

            g.DrawString("术者", font, Brushes.Black, new RectangleF(pointX, pointY + lineHeadHeight / 2 + 3, ColumnWidth4 / 3, lineHeadHeight / 2), sf);
            pointX = pointX + ColumnWidth4 / 3;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY + lineHeadHeight / 2), new PointF(pointX, pointY + lineHeadHeight + lineHeight * 4));

            g.DrawString("Ⅰ助", font, Brushes.Black, new RectangleF(pointX, pointY + lineHeadHeight / 2 + 3, ColumnWidth4 / 3, lineHeadHeight / 2), sf);
            pointX = pointX + ColumnWidth4 / 3;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY + lineHeadHeight / 2), new PointF(pointX, pointY + lineHeadHeight + lineHeight * 4));

            g.DrawString("Ⅱ助", font, Brushes.Black, new RectangleF(pointX, pointY + lineHeadHeight / 2 + 3, ColumnWidth4 / 3, lineHeadHeight / 2), sf);
            pointX = pointX + ColumnWidth4 / 3;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeadHeight + lineHeight * 4));

            g.DrawString("麻 醉", font, Brushes.Black, new RectangleF(pointX, pointY + 3, ColumnWidth5, lineHeadHeight / 2), sf);
            g.DrawString("方 式", font, Brushes.Black, new RectangleF(pointX, pointY + lineHeadHeight / 2 + 2, ColumnWidth5, lineHeadHeight / 2), sf);
            pointX = pointX + ColumnWidth5;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeadHeight + lineHeight * 4));

            g.DrawString("切口愈", font, Brushes.Black, new RectangleF(pointX, pointY + 3, ColumnWidth6, lineHeadHeight / 2), sf);
            g.DrawString("合等级", font, Brushes.Black, new RectangleF(pointX, pointY + lineHeadHeight / 2 + 2, ColumnWidth6, lineHeadHeight / 2), sf);
            pointX = pointX + ColumnWidth6;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX, pointY + lineHeadHeight + lineHeight * 4));

            g.DrawString("麻 醉", font, Brushes.Black, new RectangleF(pointX, pointY + 3, ColumnWidth7, lineHeadHeight / 2), sf);
            g.DrawString("医 师", font, Brushes.Black, new RectangleF(pointX, pointY + lineHeadHeight / 2 + 2, ColumnWidth7, lineHeadHeight / 2), sf);

            #endregion

            #region TableBody

            #region 划线
            pointX = m_PointXPayType;
            pointY = pointY + lineHeadHeight;
            float valueTableBodyPointY = pointY;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointX = m_PointXPayType;
            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointX = m_PointXPayType;
            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointX = m_PointXPayType;
            pointY = pointY + lineHeight;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));

            pointX = m_PointXPayType;
            pointY = pointY + lineHeight;
            g.DrawLine(solidPen, new PointF(pointX, pointY), new PointF(pointX + 770, pointY));


            float returnPointY = pointY;
            #endregion

            #region 内容
            pointX = m_PointXPayType;
            pointY = valueTableBodyPointY;
            DataTable dtOperation = m_IemMainPageEntity.IemOperInfo.Operation_Table;
            for (int i = 0; i < dtOperation.Rows.Count; i++)
            {
                if (i > 3) break;

                //手术操作码
                string operationCode = dtOperation.Rows[i]["Operation_Code"].ToString();
                g.DrawString(operationCode, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY + 2, ColumnWidth1, lineHeight), sf);
                pointX += ColumnWidth1;

                //手术操作日期
                string operationDate = dtOperation.Rows[i]["Operation_Date"].ToString();
                g.DrawString(operationDate, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY + 2, ColumnWidth2, lineHeight), sf);
                pointX += ColumnWidth2;

                //手术操作名称
                string operationName = dtOperation.Rows[i]["Operation_Name"].ToString();
                g.DrawString(operationName, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX + 10, pointY + 2, ColumnWidth3, lineHeight), sfVertical);
                pointX += ColumnWidth3;

                //手术操作医师
                string executeUser1Name = dtOperation.Rows[i]["Execute_User1_Name"].ToString();
                g.DrawString(executeUser1Name, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY + 2, ColumnWidth4 / 3, lineHeight), sf);
                pointX += ColumnWidth4 / 3;

                string executeUser2Name = dtOperation.Rows[i]["Execute_User2_Name"].ToString();
                g.DrawString(executeUser2Name, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY + 2, ColumnWidth4 / 3, lineHeight), sf);
                pointX += ColumnWidth4 / 3;

                string executeUser3Name = dtOperation.Rows[i]["Execute_User3_Name"].ToString();
                g.DrawString(executeUser3Name, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY + 2, ColumnWidth4 / 3, lineHeight), sf);
                pointX += ColumnWidth4 / 3;

                //麻醉方式
                string anaesthesiaTypeName = dtOperation.Rows[i]["Anaesthesia_Type_Name"].ToString();
                g.DrawString(anaesthesiaTypeName, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY + 2, ColumnWidth5, lineHeight), sf);
                pointX += ColumnWidth5;

                //切口愈合等级
                string closeLevelName = dtOperation.Rows[i]["Close_Level_Name"].ToString();
                g.DrawString(closeLevelName, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY + 2, ColumnWidth6, lineHeight), sf);
                pointX += ColumnWidth6;

                //麻醉医师
                string anaesthesiaUserName = dtOperation.Rows[i]["Anaesthesia_User_Name"].ToString();
                g.DrawString(anaesthesiaUserName, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY + 2, ColumnWidth7, lineHeight), sf);
                pointX += ColumnWidth7;

                pointX = m_PointXPayType;
                pointY = pointY + lineHeight;
            }

            #endregion

            #endregion

            return returnPointY;
        }

        /// <summary>
        /// 妇科产妇婴儿情况
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float FuKeChanKeYingEr(Graphics g, float pointY)
        {
            float interval = 38; //行间距
            Font font = m_DefaultFont;
            Font font2 = new Font("宋体", 25f, FontStyle.Bold, GraphicsUnit.Pixel);
            Pen solidPen = new Pen(Brushes.Black, 2);
            int lineHeight = TextRenderer.MeasureText("高", font).Height;
            int charWidth = TextRenderer.MeasureText("宽", font).Height;

            float pointStartX = m_PointXPayType + 12;
            float pointX = pointStartX;

            g.DrawString("产 科 产 妇 婴 儿 情 况", font2, Brushes.Black, new RectangleF(10f, pointY, m_PageWidth, 30), sf);
            pointY = pointY + 40;
            g.DrawLine(solidPen, new PointF(pointX - 12, pointY), new PointF(pointX + 770 - 12, pointY));
            pointY = pointY + 30;

            #region 1
            //胎次
            g.DrawString("胎次", font, Brushes.Black, new PointF(pointX, pointY));
            string taiCi = m_IemMainPageEntity.IemObstetricsBaby.TC;
            pointX = pointX + TextRenderer.MeasureText("胎次", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, taiCi, lineHeight) + 10;

            //产次
            g.DrawString("产次", font, Brushes.Black, new PointF(pointX, pointY));
            string chanCi = m_IemMainPageEntity.IemObstetricsBaby.CC;
            pointX = pointX + TextRenderer.MeasureText("产次", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, chanCi, lineHeight) + 10;

            //胎别
            g.DrawString("胎别", font, Brushes.Black, new PointF(pointX, pointY));
            string taiBie = m_IemMainPageEntity.IemObstetricsBaby.TB;
            pointX = pointX + TextRenderer.MeasureText("胎别", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, taiBie, lineHeight) + 10;

            pointX = DrawSelectItem(g, pointX, pointY, "（1.单 2.双 3.多）") + 10;

            //产妇会阴破裂度
            g.DrawString("产妇会阴破裂度", font, Brushes.Black, new PointF(pointX, pointY));
            string poLieLevel = m_IemMainPageEntity.IemObstetricsBaby.CFHYPLD;
            pointX = pointX + TextRenderer.MeasureText("产妇会阴破裂度", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, poLieLevel, lineHeight) + 2;

            pointX = DrawSelectItem(g, pointX, pointY, "（1.Ⅰ 2.Ⅱ 3.Ⅲ）") + 10;

            //接产者
            string acceptUser = m_IemMainPageEntity.IemObstetricsBaby.Midwifery;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "接产者", acceptUser, 70, "") + 15;
            #endregion

            #region 2

            pointY += interval;
            pointX = pointStartX;

            //胎儿性别
            g.DrawString("性别", font, Brushes.Black, new PointF(pointX, pointY));
            string gender = m_IemMainPageEntity.IemObstetricsBaby.Sex;
            pointX = pointX + TextRenderer.MeasureText("性别", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, gender, lineHeight) + 2;

            pointX = DrawSelectItem(g, pointX, pointY, "（1.男  2.女）") + 10;

            //阿帕加评分
            g.DrawString("阿帕加评分", font, Brushes.Black, new PointF(pointX, pointY));
            string apajiapingjia = m_IemMainPageEntity.IemObstetricsBaby.APJ;
            pointX = pointX + TextRenderer.MeasureText("阿帕加评分", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, apajiapingjia, lineHeight) + 2;

            pointX = DrawSelectItem(g, pointX, pointY, "（1—分…A+分）") + 140;

            //身长
            string bodyLength = m_IemMainPageEntity.IemObstetricsBaby.Heigh;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "身长", bodyLength, 50, "CM") + 10;

            //体重
            string bodyWeight = m_IemMainPageEntity.IemObstetricsBaby.Weight;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "体重", bodyWeight, 50, "G") + 10;
            #endregion

            #region 3

            pointY += interval;
            pointX = pointStartX;

            //产出情况
            g.DrawString("产出情况", font, Brushes.Black, new PointF(pointX, pointY));
            string birthStatus = m_IemMainPageEntity.IemObstetricsBaby.CCQK;
            pointX = pointX + TextRenderer.MeasureText("产出情况", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, birthStatus, lineHeight) + 2;

            pointX = DrawSelectItem(g, pointX, pointY, "（1.产活  2.产死  3.死胎  4.畸形）") + 198;

            //出生
            string birth = m_IemMainPageEntity.IemObstetricsBaby.BithDay;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生", birth, 175, "") + 30;

            #endregion

            #region 4

            pointY += interval;
            pointX = pointStartX;

            //分娩方式
            g.DrawString("分娩方式", font, Brushes.Black, new PointF(pointX, pointY));
            string birthType = m_IemMainPageEntity.IemObstetricsBaby.FMFS;
            pointX = pointX + TextRenderer.MeasureText("分娩方式", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, birthType, lineHeight) + 2;

            pointX = DrawSelectItem(g, pointX, pointY, "（1.自然 2.测+吸 3.产钳 4.臂牵引 5.剖宫 6.其他）") + 18;

            //出院情况
            g.DrawString("出院情况", font, Brushes.Black, new PointF(pointX, pointY));
            string outHospitalStatus = m_IemMainPageEntity.IemObstetricsBaby.CYQK;
            pointX = pointX + TextRenderer.MeasureText("出院情况", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, outHospitalStatus, lineHeight) + 2;

            pointX = DrawSelectItem(g, pointX, pointY, "（1.正常  2.有病  3.交叉感染）");

            #endregion

            pointX = pointStartX;
            pointY = pointY + 40;
            g.DrawLine(solidPen, new PointF(pointX - 12, pointY), new PointF(pointX + 770 - 12, pointY));

            return pointY;
        }

        private float PrintSecondPageOther(Graphics g, float pointY)
        {
            float interval = 38; //行间距
            Font font = m_DefaultFont;
            Font font2 = new Font("宋体", 25f, FontStyle.Bold, GraphicsUnit.Pixel);
            Pen solidPen = new Pen(Brushes.Black, 2);
            int lineHeight = TextRenderer.MeasureText("高", font).Height;
            int charWidth = TextRenderer.MeasureText("宽", font).Height;

            float pointStartX = m_PointXPayType + 12;
            float pointX = pointStartX;

            g.DrawLine(solidPen, new PointF(pointX - 12, pointY), new PointF(pointX + 770 - 12, pointY));
            pointY = pointY + 30;

            #region 1
            //住院费用总计（元）
            string allCost = m_IemMainPageEntity.IemFeeInfo.Total.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "住院费用总计（元）", allCost, 62, "");
            //床费
            string bedCost = m_IemMainPageEntity.IemFeeInfo.Bed.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "床费", bedCost, 62, "");
            //护理费
            string nurseCost = m_IemMainPageEntity.IemFeeInfo.Care.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "护理费", nurseCost, 62, "");
            //西药
            string westCost = m_IemMainPageEntity.IemFeeInfo.WMedical.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "西药", westCost, 62, "");
            //中成药
            string zhongChengCost = m_IemMainPageEntity.IemFeeInfo.CPMedical.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "中成药", zhongChengCost, 62, "");
            //中草药
            string zhongCaoCost = m_IemMainPageEntity.IemFeeInfo.CMedical.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "中草药", zhongCaoCost, 62, "");
            #endregion

            #region 2

            pointY += interval;
            pointX = pointStartX;

            //放射
            string pacsCost = m_IemMainPageEntity.IemFeeInfo.Radiate.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "放射", pacsCost, 68, "");
            //化验
            string lisCost = m_IemMainPageEntity.IemFeeInfo.Assay.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "化验", lisCost, 68, "");
            //输氧
            string shuYangCost = m_IemMainPageEntity.IemFeeInfo.Ox.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "输氧", shuYangCost, 68, "");
            //输血
            string bloodCost = m_IemMainPageEntity.IemFeeInfo.Blood.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "输血", bloodCost, 68, "");
            //诊疗
            string DiagnosisCost = m_IemMainPageEntity.IemFeeInfo.Mecical.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "诊疗", DiagnosisCost, 68, "");
            //手术
            string operationCost = m_IemMainPageEntity.IemFeeInfo.Operation.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "手术", operationCost, 68, "");
            //接生
            string birthCost = m_IemMainPageEntity.IemFeeInfo.Accouche.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "接生", birthCost, 67, "");

            #endregion

            #region 3

            pointY += interval;
            pointX = pointStartX;

            //检查
            string checkCost = m_IemMainPageEntity.IemFeeInfo.Ris.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "检查", checkCost, 68, "");
            //麻醉费
            string maZuiCost = m_IemMainPageEntity.IemFeeInfo.Anaesthesia.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "麻醉费", maZuiCost, 68, "");
            //婴儿费
            string yingErCost = m_IemMainPageEntity.IemFeeInfo.Baby.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "婴儿费", yingErCost, 68, "");
            //陪床费
            string peiBedCost = m_IemMainPageEntity.IemFeeInfo.FollwBed.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "陪床费", peiBedCost, 68, "");
            //其他1
            string Other1Cost = m_IemMainPageEntity.IemFeeInfo.Others1.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "其他", Other1Cost, 63, "");
            //其他2
            string Other2Cost = m_IemMainPageEntity.IemFeeInfo.Others2.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "、", Other2Cost, 63, "");
            //其他3
            string Other3Cost = m_IemMainPageEntity.IemFeeInfo.Others3.Trim();
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "、", Other3Cost, 63, "");

            #endregion

            #region 4

            pointY += interval;
            pointX = pointStartX;

            //尸检
            g.DrawString("尸    检", font, Brushes.Black, new PointF(pointX, pointY));
            string shiTiCheck = m_IemMainPageEntity.IemFeeInfo.Ashes_Check;
            pointX = pointX + TextRenderer.MeasureText("尸    检", font).Width + 30;
            pointX = DrawCheckBox(g, pointX, pointY, shiTiCheck, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.是  2.否") + 170;

            //本院第一例
            g.DrawString("手术、治疗、检查、诊断为本院第一例", font, Brushes.Black, new PointF(pointX, pointY));
            string firstCase = m_IemMainPageEntity.IemFeeInfo.IsFirstCase;
            pointX = pointX + TextRenderer.MeasureText("手术、治疗、检查、诊断为本院第一例", font).Width + 30;
            pointX = DrawCheckBox(g, pointX, pointY, firstCase, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.是  2.否");

            #endregion

            #region 5

            pointY += interval;
            pointX = pointStartX;

            g.DrawLine(Pens.Black, new PointF(pointX - 12, pointY - 14), new PointF(pointX + 770 - 12, pointY - 14));

            //随诊
            g.DrawString("随    诊", font, Brushes.Black, new PointF(pointX, pointY));
            string suiZhen = m_IemMainPageEntity.IemFeeInfo.IsFollowing;
            pointX = pointX + TextRenderer.MeasureText("随    诊", font).Width + 30;
            pointX = DrawCheckBox(g, pointX, pointY, suiZhen, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.是  2.否") + 150;

            //随诊期限
            string suiZhenWeek = m_IemMainPageEntity.IemFeeInfo.IsFollowingDay;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "随诊期限", suiZhenWeek, 30, "") + 5;
            string suiZhenMonth = m_IemMainPageEntity.IemFeeInfo.IsFollowingMon;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "周", suiZhenMonth, 30, "") + 5;
            string suiZhenYear = m_IemMainPageEntity.IemFeeInfo.IsFollowingYear;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "月", suiZhenYear, 30, "年") + 5;

            //示教病例
            g.DrawString("示教病例", font, Brushes.Black, new PointF(pointX, pointY));
            string jiaoXueCase = m_IemMainPageEntity.IemFeeInfo.IsTeachingCase;
            pointX = pointX + TextRenderer.MeasureText("示教病例", font).Width + 30;
            pointX = DrawCheckBox(g, pointX, pointY, jiaoXueCase, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.是  2.否");

            #endregion

            #region 6

            pointY += interval;
            pointX = pointStartX;

            g.DrawLine(Pens.Black, new PointF(pointX - 12, pointY - 14), new PointF(pointX + 770 - 12, pointY - 14));

            //血型
            g.DrawString("血    型", font, Brushes.Black, new PointF(pointX, pointY));
            string bloodType = m_IemMainPageEntity.IemFeeInfo.BloodType;
            pointX = pointX + TextRenderer.MeasureText("血    型", font).Width + 30;
            pointX = DrawCheckBox(g, pointX, pointY, bloodType, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.A  2.B  3.AB  4.O  5.其他") + 42;

            //Rh
            g.DrawString("Rh", font, Brushes.Black, new PointF(pointX, pointY));
            string RhType = m_IemMainPageEntity.IemFeeInfo.Rh;
            pointX = pointX + TextRenderer.MeasureText("Rh", font).Width + 30;
            pointX = DrawCheckBox(g, pointX, pointY, RhType, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.阴  2.阳") + 44;

            //输血反应
            g.DrawString("输血反应", font, Brushes.Black, new PointF(pointX, pointY));
            string bloodReflect = m_IemMainPageEntity.IemFeeInfo.BloodReaction;
            pointX = pointX + TextRenderer.MeasureText("输血反应", font).Width + 30;
            pointX = DrawCheckBox(g, pointX, pointY, bloodReflect, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.有  2.无");
            #endregion

            #region 7

            pointY += interval;
            pointX = pointStartX;

            g.DrawLine(Pens.Black, new PointF(pointX - 12, pointY - 14), new PointF(pointX + 770 - 12, pointY - 14));

            //输血品种
            g.DrawString("输血品种", font, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("输血品种", font).Width + 30;

            //红细胞
            string redCell = m_IemMainPageEntity.IemFeeInfo.Rbc;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "1.红细胞", redCell, 40, "单位") + 10;
            //血小板
            string xueXiaoBan = m_IemMainPageEntity.IemFeeInfo.Plt;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "2.血小板", xueXiaoBan, 40, "袋") + 10;
            //血浆
            string xueJiang = m_IemMainPageEntity.IemFeeInfo.Plasma;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "3.血浆", xueJiang, 40, "ml") + 10;
            //全血
            string quanXue = m_IemMainPageEntity.IemFeeInfo.Wb;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "4.全血", quanXue, 40, "ml") + 10;
            //其他
            string otherXue = m_IemMainPageEntity.IemFeeInfo.Others;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "5.其他", otherXue, 40, "ml") + 10;

            pointX = pointStartX;
            pointY = pointY + 30;
            g.DrawLine(solidPen, new PointF(pointX - 12, pointY), new PointF(pointX + 770 - 12, pointY));
            pointY = pointY + 20;
            #endregion

            #region 8

            Font solidFont = new Font("宋体", 14f, FontStyle.Bold, GraphicsUnit.Pixel);
            g.DrawString("说明:", solidFont, Brushes.Black, new PointF(pointX, pointY));
            g.DrawString("医疗付款方式   1、社会基本医疗保险（补充保险、特大病保险） 2、商业保险 3、自费医疗 4、公费医疗", font, Brushes.Black, new PointF(pointX + 50, pointY));

            pointY += interval - 10;
            pointX = pointStartX;

            g.DrawString("5、大病统筹 6、其他        住院费用总计  凡可由计算机提供住院费用清单的，住院昼夜中不可填", font, Brushes.Black, new PointF(pointX + 50, pointY));
            #endregion

            return pointY;
        }

        /// <summary>
        /// 带下划线
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
            string name, string value, int underLineWidth, string endName)
        {
            g.DrawString(name, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            int widthName = TextRenderer.MeasureText(name, m_DefaultFont).Width;
            int widthValue = underLineWidth;
            pointX = pointX + widthName;

            int valueLength = TextRenderer.MeasureText(value, m_DefaultValueFont).Width;
            if (valueLength >= underLineWidth)
            {
                //string firstLine = "";
                //string secondLine = "";
                //GetTwoLineValue(value, underLineWidth, out firstLine, out secondLine);
                //g.DrawString(firstLine, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY - 10, widthValue, lineHeight / 2 + 5), sf);
                //g.DrawString(secondLine, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY + lineHeight / 2 - 5, widthValue, lineHeight / 2 + 5), sf);

                if (TextRenderer.MeasureText(value, m_SmallFont1).Width >= underLineWidth)
                {
                    g.DrawString(value, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY - 10, widthValue, lineHeight + 14), sfVertical);
                }
                else
                {
                    g.DrawString(value, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY, widthValue, lineHeight + 2), sf);
                }
            }
            else
            {
                g.DrawString(value, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY, widthValue, lineHeight + 2), sf);
            }
            g.DrawLine(Pens.Black, new PointF(pointX, pointY + lineHeight), new PointF(pointX + widthValue, pointY + lineHeight));

            pointX = pointX + widthValue;

            if (endName != "")
            {
                g.DrawString(endName, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
                int widthEndName = TextRenderer.MeasureText(endName, m_DefaultFont).Width;
                pointX = pointX + widthEndName + 2;
            }
            else
            {
                pointX = pointX + 2;
            }
            return pointX;
        }

        private void GetTwoLineValue(string value, int lineWidth, out string firstLine, out string secondLine)
        {
            StringBuilder sb = new StringBuilder();
            bool isFirstLineTemp = true;
            bool isSecondLineTemp = false;
            firstLine = "";
            secondLine = "";
            for (int i = 0; i < value.Length; i++ )
            {
                if (isFirstLineTemp)
                {
                    if (TextRenderer.MeasureText(sb.Append(value[i]).ToString(), m_DefaultValueFont).Width > lineWidth)
                    {
                        firstLine = sb.ToString().Substring(0, sb.Length - 1);
                        sb = new StringBuilder();
                        isFirstLineTemp = false;
                        isSecondLineTemp = true;
                        i--;
                    }
                }
                else if (isSecondLineTemp)
                {
                    secondLine = value.Substring(firstLine.Length);
                }
            }
        }

        /// <summary>
        /// 不带下划线
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
        private float DrawNameAndValueWithoutUnderLine2(Graphics g, float pointX, float pointY, int lineHeight, int charWidth, 
            string name, string value, int underLineWidth, string endName)
        {
            g.DrawString(name, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            int widthName = TextRenderer.MeasureText(name, m_DefaultFont).Width;
            int widthValue = underLineWidth;
            pointX = pointX + widthName;
            string patientName = value;
            g.DrawString(patientName, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY, widthValue, lineHeight + 2), sf);

            pointX = pointX + widthValue;

            if (endName != "")
            {
                g.DrawString(endName, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
                int widthEndName = TextRenderer.MeasureText(endName, m_DefaultFont).Width;
                pointX = pointX + widthEndName + 2;
            }
            else
            {
                pointX = pointX + 2;
            }
            return pointX;
        }

        /// <summary>
        /// 绘制方框和方框中的值
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="value"></param>
        /// <param name="lineHeight"></param>
        /// <returns></returns>
        private float DrawCheckBox(Graphics g, float pointX, float pointY, string value, int lineHeight)
        {
            Rectangle rect = new Rectangle((int)pointX, (int)pointY, lineHeight - 2, lineHeight - 2);
            g.DrawRectangle(Pens.Black, rect);
            RectangleF rectF = new RectangleF(pointX - 0.5f, pointY - 0.5f, lineHeight, lineHeight + 0.5f);
            g.DrawString(value, m_DefaultFont, Brushes.Black, rectF, sf);
            return pointX + lineHeight + 5;
        }

        /// <summary>
        /// 绘制选项
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="itemList"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        private float DrawSelectItem(Graphics g, float pointX, float pointY, string allSelectItem/*所有选项,用空格隔开*/)
        {
            g.DrawString(allSelectItem, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            return pointX + TextRenderer.MeasureText(allSelectItem, m_DefaultFont).Width;
        }

        #endregion
    }
}