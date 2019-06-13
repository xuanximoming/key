using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Xml;

namespace DrectSoft.Core.IEMMainPage
{
    public class DrawMainPageUtil
    {
        #region Property && Field

        PrintDocument m_PrintDocument;

        public int m_PageWidth = 800;//用于设定PictureBox的宽度
        public int m_PageHeight = 1100;//用于设定PictureBox的高度1100

        float m_PointYTitle = 40; //“首页”标题Y轴方向的值
        //Font m_DefaultFont = new Font("宋体", 15f, FontStyle.Regular, GraphicsUnit.Pixel);//各个小标题
        Font m_DefaultFont = new Font("宋体", 15f, FontStyle.Bold, GraphicsUnit.Pixel);     //王冀 2012 12 10
        Font m_DefaultValueFont = new Font("宋体", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_SmallFont = new Font("宋体", 10f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_SmallFont1 = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Pixel);

        float m_PointXPayType = 15; //“医疗付款方式”左上角点的X轴方向的值
        float m_PointYPayType = 120; //“医疗付款方式”左上角点的Y轴方向的值

        float m_PointXOrganizationCode = 550; //“组织机构代码”在右角点的X轴方向的值
        float m_PointYOrganizationCode = 20; //“组织机构代码”在右上角的Y轴方向的值
        private string m_OrganizationCode = string.Empty;//“组织机构代码”

        StringFormat sf = new StringFormat();
        StringFormat sfVertical = new StringFormat();
        StringFormat sfTop = new StringFormat();

        int m_PageIndex = 1;

        /// <summary>
        /// 病案首页实体类
        /// </summary>
        IemMainPageInfo m_IemMainPageEntity = new IemMainPageInfo();

        Metafile mf1;
        /// <summary>
        /// 第一页
        /// </summary>
        public Metafile MF1
        {
            get
            {
                return mf1;
            }
        }

        Metafile mf2;
        /// <summary>
        /// 第二页
        /// </summary>
        public Metafile MF2
        {
            get
            {
                return mf2;
            }
        }
        Pen pen = new Pen(Brushes.Black, 2);

        Pen pen2 = new Pen(Brushes.White, 3);
        string m_FilePath1 = string.Empty;
        /// <summary>
        /// 第一页的地址
        /// </summary>
        public string FilePath1
        {
            get
            {
                return m_FilePath1;
            }
        }

        string m_FilePath2 = string.Empty;
        /// <summary>
        /// 第二页的地址
        /// </summary>
        public string FilePath2
        {
            get
            {
                return m_FilePath2;
            }
        }

        DataHelper dHelper = new DataHelper();
        string m_AddressShowType = string.Empty;

        #endregion
        //public bool ShowDiagContorl = false;//是否显示诊断符合的栏位 
        public DrawMainPageUtil(IemMainPageInfo iemMainPageEntity)
        {
            m_IemMainPageEntity = iemMainPageEntity;
            InitVariable();
            InitMetaFile();
        }

        /// <summary>
        /// 初始化变量
        /// </summary>
        private void InitVariable()
        {
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sfVertical.LineAlignment = StringAlignment.Center;
            sfTop.LineAlignment = StringAlignment.Near;
        }

        private void InitMetaFile()
        {
            string folder = CreateFolder();
            string cansee = dHelper.GetConfigValueByKey("EmrInputConfig");
            XmlDocument doc1 = new XmlDocument();
            doc1.LoadXml(cansee);
            int m_AnOtherHeight = 0;
            if (doc1.GetElementsByTagName("IemPageContorlVisable")[0].InnerText == "1")//可见诊断符合的一些栏位
            {
                m_PageHeight = 1139;
                m_AnOtherHeight = 1000;
            }
            else
            {
                m_AnOtherHeight = 1000;
            }


            string temp = "";
            temp = doc1.GetElementsByTagName("ShowOrganizationCode")[0] == null ? "" : doc1.GetElementsByTagName("ShowOrganizationCode")[0].InnerText;//病案首页显示组织机构代码 2012/10/22 by tj
            if (string.IsNullOrEmpty(temp))
            {
                m_OrganizationCode = "";
            }
            else
            {
                m_OrganizationCode = temp;
            }

            Bitmap bmp1 = new Bitmap(m_PageWidth, m_PageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g1 = Graphics.FromImage(bmp1);
            Rectangle rect = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
            m_FilePath1 = folder + Guid.NewGuid().ToString() + ".wmf";
            mf1 = new Metafile(m_FilePath1, g1.GetHdc(), rect, MetafileFrameUnit.Pixel);
            g1 = Graphics.FromImage(mf1);
            g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g1.Clear(Color.White);
            PrintFirstPage(g1);
            g1.Save();
            g1.Dispose();

            Bitmap bmp2 = new Bitmap(m_PageWidth, m_AnOtherHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g2 = Graphics.FromImage(bmp2);
            m_FilePath2 = folder + Guid.NewGuid().ToString() + ".wmf";
            mf2 = new Metafile(m_FilePath2, g2.GetHdc(), rect, MetafileFrameUnit.Pixel);
            g2 = Graphics.FromImage(mf2);
            g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g2.Clear(Color.White);
            PrintSecondPage(g2);
            g2.Save();
            g2.Dispose();
        }

        /// <summary>
        /// 在当前程序所在文件创建文件夹
        /// </summary>
        /// <returns></returns>
        private string CreateFolder()
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (!Directory.Exists(folder + "PrintImage\\"))
            {
                Directory.CreateDirectory(folder + "PrintImage\\");
            }
            DeleteMetaFileAll();
            return folder + "PrintImage\\";
        }

        private void DeleteMetaFileAll()
        {
            //删除原先保存在C盘的文件
            DeleteMetaFileAllInner(@"C:\");

            //删除打印需要的矢量文件
            DeleteMetaFileAllInner(AppDomain.CurrentDomain.BaseDirectory + "PrintImage\\");
        }

        private void DeleteMetaFileAllInner(string folder)
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

        /// <summary>
        /// 删除使用后的矢量文件
        /// </summary>
        private void DeleteMetaFile()
        {
            if (File.Exists(m_FilePath1))
            {
                mf1.Dispose();
                File.Delete(m_FilePath1);
            }
            if (File.Exists(m_FilePath2))
            {
                mf2.Dispose();
                File.Delete(m_FilePath2);
            }
        }

        #region ******************************************************* 绘制病案首页 **************************************************************

        #region 【打印第一页】

        void PrintFirstPage(Graphics g)
        {
            float pointY;
            DrawTitle(g);
            DrawOrganaqzaitionCode(g);
            pointY = PrintPayType(g);
            pointY = PrintPatientBaseInfo(g, pointY + 15);
            pointY = PrintOutHospitalDiaglosis(g, pointY + 18);
            pointY = PrintFristPageOther(g, pointY);
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
            Font font1 = new Font("宋体", 31f, FontStyle.Bold, GraphicsUnit.Pixel);
            Font font2 = new Font("宋体", 24f, FontStyle.Bold, GraphicsUnit.Pixel);

            //g.DrawString(m_IemMainPageEntity.IemBasicInfo.HospitalName, font1, Brushes.Black, new RectangleF(0f, m_PointYTitle - 5, m_PageWidth, 35), sf);
            g.DrawString(DrectSoft.MainFrame.PublicClass.getHosName(), font1, Brushes.Black, new RectangleF(0f, m_PointYTitle - 5, m_PageWidth, 35), sf);
            g.DrawString("住院病案首页", font2, Brushes.Black, new RectangleF(0f, m_PointYTitle + 20, m_PageWidth, 50), sf);
        }

        /// <summary>
        /// 绘制组织机构代码 2012/10/22 by tj
        /// </summary>
        /// <param name="g"></param>
        private void DrawOrganaqzaitionCode(Graphics g)
        {
            if (string.IsNullOrEmpty(m_OrganizationCode)) return;
            Font font1 = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
            //Font font2 = new Font("宋体", 20f, FontStyle.Regular, GraphicsUnit.Pixel);
            //g.DrawString(m_OrganizationCode, font1, Brushes.Black, new RectangleF(0f, m_PointYTitle, m_PageWidth, 30), sf);
            //g.DrawString("", font1, Brushes.Red, new PointF(m_PointXOrganizationCode, m_PointYOrganizationCode));
            g.DrawString("(" + m_OrganizationCode + ")", font1, Brushes.Black, new PointF(m_PointXOrganizationCode + 10, m_PointYOrganizationCode + 30));
        }

        /// <summary>
        /// 绘制医疗付款方式一行
        /// </summary>
        /// <param name="g"></param>
        private float PrintPayType(Graphics g)
        {
            Font font = m_DefaultFont;
            Font font1 = new Font("宋体", 15f, FontStyle.Regular, GraphicsUnit.Pixel);
            int lineHeight = TextRenderer.MeasureText("高", font).Height;

            float pointX = m_PointXPayType;

            //医疗付款方式
            g.DrawString("医疗付款方式:", font1, Brushes.Black, new PointF(pointX, m_PointYPayType - 25));
            string payType = m_IemMainPageEntity.IemBasicInfo.PayID; //todo
            pointX = pointX + TextRenderer.MeasureText("医疗付款方式:", font1).Width;
            pointX = DrawCheckBox(g, pointX, m_PointYPayType - 25, payType, lineHeight);
            pointX = DrawSelectItem(g, pointX, m_PointYPayType - 25, "") + 20;

            //健康卡号
            string healthCarCode = m_IemMainPageEntity.IemBasicInfo.CardNumber;//todo
            g.DrawString("健康卡号:" + healthCarCode, font1, Brushes.Black, new PointF(m_PointXPayType, m_PointYPayType));

            //入院次数
            string inTime = m_IemMainPageEntity.IemBasicInfo.InCount;//todo
            g.DrawString("第 " + inTime + " 次住院", font1, Brushes.Black, new PointF(355, m_PointYPayType));

            //病案号
            string recordNo = string.Empty;
            if (m_IemMainPageEntity.IemBasicInfo.IsBaby == "1")//是婴儿
            {
                recordNo = m_IemMainPageEntity.IemBasicInfo.MotherPatOfHis;
                //IemMainPageManger.GetPatData(m_IemMainPageEntity.IemBasicInfo.Mother, m_SqlHelper).Rows[0]["patnoofhis"].ToString();
            }
            else
            {
                recordNo = m_IemMainPageEntity.IemBasicInfo.PatNoOfHis;//todo
            }
            //中心医院病案室需求：病案号加粗加大 edit by ywk 2012年8月10日 10:51:11
            //Font font22 = new Font("宋体", 30f, FontStyle.Bold, GraphicsUnit.Pixel);
            //g.DrawString("病案号:" + recordNo, font22, Brushes.Black, new PointF(620, m_PointYPayType));
            g.DrawString("病案号:", font1, Brushes.Black, new PointF(600, m_PointYPayType));
            //中心医院 需求只留后面六位数字 edit by ywk 2012年8月16日 10:51:10
            //if (recordNo.Length >= 6)
            //{
            //    g.DrawString(recordNo.Substring(recordNo.Length - 6), font22, Brushes.Black, new PointF(650, m_PointYPayType - 10));
            //}
            //else
            //{
            g.DrawString(recordNo, font1, Brushes.Black, new PointF(660, m_PointYPayType));
            //}

            Pen pen = new Pen(Brushes.Black, 2);
            g.DrawLine(pen, new PointF(m_PointXPayType - 5, m_PointYPayType + 20), new PointF(m_PointXPayType + 770, m_PointYPayType + 20));

            return m_PointYPayType + 21;
        }
        public string M_TIPHIGHT = string.Empty;
        /// <summary>
        /// 画竖线
        /// </summary>
        /// <param name="pen">笔</param>
        /// <param name="pointX">左边起点X坐标</param>
        /// <param name="pointY">左边起点Y坐标</param>
        /// <param name="interval">列宽</param>
        private void PrintYLine(Graphics g, float pointX, float pointY, float interval)
        {
            Pen pen = new Pen(Brushes.Black, 2);
            g.DrawLine(pen, new PointF(pointX - 2, pointY), new PointF(pointX - 2, pointY + interval));
            g.DrawLine(pen, new PointF(pointX + 770, pointY), new PointF(pointX + 770, pointY + interval));
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
            float interval = 27; //行间距

            float pointStartX = m_PointXPayType - 2;//
            float pointX = pointStartX;

            #region 1
            //姓名

            Pen pen = new Pen(Brushes.Black, 2);
            g.DrawLine(pen, new PointF(pointX - 2, pointY - 15), new PointF(pointX - 2, pointY + interval * 34));
            g.DrawLine(pen, new PointF(pointX + 770, pointY - 15), new PointF(pointX + 770, pointY + interval * 34));
            string patientName = m_IemMainPageEntity.IemBasicInfo.Name;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "姓名", patientName, 90, "") + 5;

            //性别
            g.DrawString("性别", font, Brushes.Black, new PointF(pointX, pointY));
            string gender = m_IemMainPageEntity.IemBasicInfo.SexID; //todo
            pointX = pointX + TextRenderer.MeasureText("年龄：", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, gender, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.男  2.女") + 5;

            //出生
            string birth = m_IemMainPageEntity.IemBasicInfo.BirthPrint;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生日期", birth, 120, "") + 5;

            //年龄
            string age = m_IemMainPageEntity.IemBasicInfo.Age;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "年龄", age, 80, "") + 5;

            //国籍
            string nationality = m_IemMainPageEntity.IemBasicInfo.NationalityName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "国籍", nationality, 100, "") + 15;

            #endregion

            #region 2
            pointY += interval;
            pointX = pointStartX;

            //（年龄不足1周岁的） 年龄
            g.DrawString("（年龄不足1周岁的）", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            //pointX = pointX + TextRenderer.MeasureText("（年龄不足1周岁的）", font).Width + 5;
            pointX = TextRenderer.MeasureText("（年龄不足1周岁的）", m_DefaultValueFont).Width + 5;
            string babyMonth = m_IemMainPageEntity.IemBasicInfo.MonthAge;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, " 年龄", babyMonth, 80, "月") + 20;

            //新生儿出生体重
            string babyBirthWeight = m_IemMainPageEntity.IemBasicInfo.Weight;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "新生儿出生体重", babyBirthWeight, 80, "克") + 20;

            //新生儿入院体重
            string babyInHospitalWeight = m_IemMainPageEntity.IemBasicInfo.InWeight;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "新生儿入院体重", babyInHospitalWeight, 80, "克");
            #endregion

            #region 3
            pointY += interval;
            pointX = pointStartX;


            //读取配置，绘制病案首页的有关几个地址的显示情况（ywk 2012年7月11日 09:32:30）
            string config = dHelper.GetConfigValueByKey("EmrInputConfig");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(config);
            m_AddressShowType = doc.GetElementsByTagName("IsReadAddressInfo")[0].InnerText;
            #region 对出生地的控制绘制
            //出生地 ywk
            if (m_AddressShowType == "0")//还是按原来的那样绘制
            {
                string birthPlaceProvince = m_IemMainPageEntity.IemBasicInfo.CSD_ProvinceName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生地", birthPlaceProvince, 60, "省(区、市)");
                string birthPlaceCity = m_IemMainPageEntity.IemBasicInfo.CSD_CityName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", birthPlaceCity, 60, "市");
                string birthPlaceCounty = m_IemMainPageEntity.IemBasicInfo.CSD_DistrictName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", birthPlaceCounty, 60, "县") + 5;
            }
            if (m_AddressShowType == "1")//按地址为一个名称的显示
            {
                //string CSDAddress = "...出生地的具体地址...";
                string CSDAddress = m_IemMainPageEntity.IemBasicInfo.CSDAddress;
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, " 出生地", CSDAddress, 240, "") + 5;
            }

            #endregion

            #region 对籍贯地址的控制绘制
            if (m_AddressShowType == "0")//还是按原来的那样绘制
            {
                //籍贯 ywk
                string jiGuanProvince = m_IemMainPageEntity.IemBasicInfo.JG_ProvinceName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "籍贯", jiGuanProvince, 60, "省(区、市)");
                string jiGuanCity = m_IemMainPageEntity.IemBasicInfo.JG_CityName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", jiGuanCity, 60, "市") + 5;
            }
            if (m_AddressShowType == "1")//按地址一个名称的显示
            {
                //string JGAddress = "...籍贯的具体地址...";
                string JGAddress = m_IemMainPageEntity.IemBasicInfo.JGAddress;
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "籍贯", JGAddress, 255, "") + 5;
            }

            #endregion

            //民族
            string mingZu = m_IemMainPageEntity.IemBasicInfo.NationName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "民族", mingZu, 73, "") + 10;
            #endregion

            #region 4
            pointY += interval;
            pointX = pointStartX;

            //身份证号
            string patientID = m_IemMainPageEntity.IemBasicInfo.IDNO;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "身份证号", patientID, 140, "") + 5;

            //职业
            string jobName = m_IemMainPageEntity.IemBasicInfo.JobName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "职业", jobName, 90, "") + 5;//120

            //婚姻
            g.DrawString("婚姻", font, Brushes.Black, new PointF(pointX, pointY));
            string marriage = m_IemMainPageEntity.IemBasicInfo.Marital; //todo
            pointX = pointX + TextRenderer.MeasureText("婚姻", font).Width + 12;
            pointX = DrawCheckBox(g, pointX, pointY, marriage, lineHeight) + 40;
            //pointX = DrawSelectItem(g, pointX, pointY, "1.未婚 2.已婚 3.丧偶 4.离婚 9.其他") + 10;
            pointX = DrawSelectItem(g, pointX, pointY, "0.未婚 1.已婚 2.离婚 3.丧偶 5.再婚 9.其他") + 10;//edit by ywk 2012年3月6日14:39:51
            #endregion

            #region 5
            pointY += interval;
            pointX = pointStartX;


            #region 对现住址的绘制控制
            if (m_AddressShowType == "0")//还是按原来的那样绘制
            {
                //现住址 ywk
                string addressProvince = m_IemMainPageEntity.IemBasicInfo.XZZ_ProvinceName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "现在住址", addressProvince, 70, "省(区、市)");
                string addressCity = m_IemMainPageEntity.IemBasicInfo.XZZ_CityName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", addressCity, 70, "市");
                string addressCounty = m_IemMainPageEntity.IemBasicInfo.XZZ_DistrictName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", addressCounty, 70, "县") + 15;
            }
            if (m_AddressShowType == "1")//按照全名称绘制
            {
                //string CSDAddress = "...现住址的具体地址...";
                string XZZAddress = m_IemMainPageEntity.IemBasicInfo.XZZAddress;
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, " 现在住址", XZZAddress, 265, "") + 10;
            }

            #endregion

            //电话
            string addressTelNumber = m_IemMainPageEntity.IemBasicInfo.XZZ_TEL;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "电话", addressTelNumber, 136, "") + 10;

            //编码
            string addressPostCode = m_IemMainPageEntity.IemBasicInfo.XZZ_Post;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "邮编", addressPostCode, 110, "") + 10;

            #endregion

            #region 6
            pointY += interval;
            pointX = pointStartX;

            #region 对户口住址的控制绘制
            if (m_AddressShowType == "0")//还是按原来的那样绘制 ywk
            {
                string hukouProvince = m_IemMainPageEntity.IemBasicInfo.HKDZ_ProvinceName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "户口地址", hukouProvince, 70, "省(区、市)");
                string hukouCity = m_IemMainPageEntity.IemBasicInfo.HKDZ_CityName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", hukouCity, 70, "市");
                string hukouCounty = m_IemMainPageEntity.IemBasicInfo.HKDZ_DistrictName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", hukouCounty, 70, "县") + 202;
            }
            if (m_AddressShowType == "1")//按照全名称显示绘制
            {
                string HKZZAddress = m_IemMainPageEntity.IemBasicInfo.HKZZAddress;
                pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, " 户口住址", HKZZAddress, 265, "") + 25;
            }

            #endregion

            //邮编
            string hukouPostCode = m_IemMainPageEntity.IemBasicInfo.HKDZ_Post;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "邮编", hukouPostCode, 110, "") + 10;

            #endregion

            #region 7
            pointY += interval;
            pointX = pointStartX;

            //工作单位及地址
            string jobAddress = m_IemMainPageEntity.IemBasicInfo.OfficePlace;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "工作单位及地址", jobAddress, 268, "") + 5;

            //单位电话
            string jobTelNumber = m_IemMainPageEntity.IemBasicInfo.OfficeTEL;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "单位电话", jobTelNumber, 137, "") + 10;

            //邮编
            string jobPostCode = m_IemMainPageEntity.IemBasicInfo.OfficePost;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "邮编", jobPostCode, 110, "") + 10;

            #endregion

            #region 8
            pointY += interval;
            pointX = pointStartX;

            //联系人姓名
            string contactName = m_IemMainPageEntity.IemBasicInfo.ContactPerson;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "联系人姓名", contactName, 90, "") + 5;

            //关系
            string relation = m_IemMainPageEntity.IemBasicInfo.RelationshipName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "关系", relation, 90, "") + 5;

            //地址
            string contactAddress = m_IemMainPageEntity.IemBasicInfo.ContactAddress;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "地址", contactAddress, 247, "") + 5;

            //电话
            string contactTel = m_IemMainPageEntity.IemBasicInfo.ContactTEL;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "电话", contactTel, 111, "") + 10;

            #endregion

            #region 9
            pointY += interval;
            pointX = pointStartX;

            //入院途径
            g.DrawString("入院途径", font, Brushes.Black, new PointF(pointX, pointY));
            string inType = m_IemMainPageEntity.IemBasicInfo.InHosType; //todo
            pointX = pointX + TextRenderer.MeasureText("入院途径", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, inType, lineHeight) + 10;
            pointX = DrawSelectItem(g, pointX, pointY, "1.急诊   2.门诊   3.其他医疗机构转入   9.其他") + 20;

            //在入院途径后面加入院病情 add by ywk 2012年6月26日 08:53:42（新增内容首页不显示）
            ////Graphics g1 =;
            //Bitmap bmp11 = new Bitmap(m_PageWidth, m_PageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //Graphics g11 = Graphics.FromImage(bmp11);

            //g.DrawString("入院病情", font, Brushes.Black, new PointF(pointX, pointY));
            //string inType1 = m_IemMainPageEntity.IemBasicInfo.AdmitInfo; //todo
            ////string inType1 = "2";
            //pointX = pointX + TextRenderer.MeasureText("入院病情", font).Width + 5;
            //pointX = DrawCheckBox(g, pointX, pointY, inType1, lineHeight) + 10;
            //pointX = DrawSelectItem(g, pointX, pointY, "1.危   2.重   3.一般   4.急") + 20;

            #endregion

            #region 10
            pointY += interval;
            pointX = pointStartX;

            //入院时间
            string inTime = m_IemMainPageEntity.IemBasicInfo.AdmitDatePrint;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院时间", inTime, 160, "") + 5;

            //入院科别
            string inSection = m_IemMainPageEntity.IemBasicInfo.AdmitDeptName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院科别", inSection, 120, "") + 5;

            //病区
            string inNurseWard = m_IemMainPageEntity.IemBasicInfo.AdmitWardName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病区", inNurseWard, 102, "") + 5;

            //转科科别
            string shiftSection = m_IemMainPageEntity.IemBasicInfo.Trans_AdmitDeptName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "转科科别", shiftSection, 108, "") + 10;

            #endregion

            #region 11
            pointY += interval;
            pointX = pointStartX;

            //出院日期
            string outTime = m_IemMainPageEntity.IemBasicInfo.OutWardDatePrint;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出院日期", outTime, 160, "") + 5;

            //出院科别
            string outSection = m_IemMainPageEntity.IemBasicInfo.OutHosDeptName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出院科别", outSection, 120, "") + 5;

            //病区
            string outNurseWard = m_IemMainPageEntity.IemBasicInfo.OutHosWardName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病区", outNurseWard, 102, "") + 5;

            //实际住院天数
            string InDay = m_IemMainPageEntity.IemBasicInfo.ActualDays;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "实际住院", InDay, 93, "天") + 10;

            #region 王冀 2012 12 10  注释
            //读取系统参数配置的可见性 add by ywk 
            //string cansee = dHelper.GetConfigValueByKey("EmrInputConfig");
            //XmlDocument doc1 = new XmlDocument();
            //doc1.LoadXml(cansee);
            //string TipValue = doc.GetElementsByTagName("IemPageContorlVisable")[0].InnerText;//标识高度
            //M_TIPHIGHT = TipValue;
            //if (TipValue == "1")//可见诊断符合的一些栏位
            //{
            //    pointY += interval;
            //    pointX = pointStartX;
            //    //新增的几个诊断符合情况的显示在首页基本信息中
            //    //门诊和住院
            //    g.DrawString("门诊与住院：", font, Brushes.Black, new PointF(pointX, pointY));
            //    string MenAndInHop = m_IemMainPageEntity.IemBasicInfo.MenAndInHop; //todo
            //    pointX = pointX + TextRenderer.MeasureText("门诊与住院：", font).Width + 5;
            //    pointX = DrawCheckBox(g, pointX, pointY, MenAndInHop, lineHeight) + 10;
            //    pointX = DrawSelectItem(g, pointX, pointY, "0.未作  1.确诊  2.误诊  3.不肯定") + 30;
            //    //入院和出院
            //    g.DrawString("入院和出院：", font, Brushes.Black, new PointF(pointX, pointY));
            //    string InHopAndOutHop = m_IemMainPageEntity.IemBasicInfo.InHopAndOutHop; //todo
            //    pointX = pointX + TextRenderer.MeasureText("入院与出院：", font).Width + 5;
            //    pointX = DrawCheckBox(g, pointX, pointY, InHopAndOutHop, lineHeight) + 10;
            //    pointX = DrawSelectItem(g, pointX, pointY, "0.未作  1.确诊  2.误诊  3.不肯定") + 30;

            //    pointY += interval;
            //    pointX = pointStartX;

            //    //术前与术后
            //    g.DrawString("术前与术后：", font, Brushes.Black, new PointF(pointX, pointY));
            //    string BeforeAndAfter = m_IemMainPageEntity.IemBasicInfo.BeforeOpeAndAfterOper; //todo
            //    pointX = pointX + TextRenderer.MeasureText("术前与术后：", font).Width + 5;
            //    pointX = DrawCheckBox(g, pointX, pointY, BeforeAndAfter, lineHeight) + 10;
            //    pointX = DrawSelectItem(g, pointX, pointY, "0.未作  1.确诊  2.误诊  3.不肯定") + 30;

            //    //临床与病理
            //    g.DrawString("临床与病理：", font, Brushes.Black, new PointF(pointX, pointY));
            //    string LinAndBingLi = m_IemMainPageEntity.IemBasicInfo.LinAndBingLi; //todo
            //    pointX = pointX + TextRenderer.MeasureText("临床与病理：", font).Width + 5;
            //    pointX = DrawCheckBox(g, pointX, pointY, LinAndBingLi, lineHeight) + 10;
            //    pointX = DrawSelectItem(g, pointX, pointY, "0.未作  1.确诊  2.误诊  3.不肯定") + 30;

            //    pointY += interval;
            //    pointX = pointStartX;
            //    //入院三日内
            //    g.DrawString("入院三日内：", font, Brushes.Black, new PointF(pointX, pointY));
            //    string InHopThree = m_IemMainPageEntity.IemBasicInfo.InHopThree; //todo
            //    pointX = pointX + TextRenderer.MeasureText("入院三日内：", font).Width + 5;
            //    pointX = DrawCheckBox(g, pointX, pointY, InHopThree, lineHeight) + 10;
            //    pointX = DrawSelectItem(g, pointX, pointY, "0.未作  1.确诊  2.误诊  3.不肯定") + 30;
            //    //放射与病理
            //    g.DrawString("放射与病理：", font, Brushes.Black, new PointF(pointX, pointY));
            //    string FangAndBingLi = m_IemMainPageEntity.IemBasicInfo.FangAndBingLi; //todo
            //    pointX = pointX + TextRenderer.MeasureText("放射与病理：", font).Width + 5;
            //    pointX = DrawCheckBox(g, pointX, pointY, FangAndBingLi, lineHeight) + 10;
            //    pointX = DrawSelectItem(g, pointX, pointY, "0.未作  1.确诊  2.误诊  3.不肯定") + 30;

            //}
            #endregion
            #endregion
            #region 12
            pointY += interval;
            pointX = pointStartX;

            //门（急）诊诊断
            string menJiZhenDiaglosis = m_IemMainPageEntity.IemDiagInfo.OutDiagName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "门（急）诊诊断", menJiZhenDiaglosis, 370, "") + 5;

            //疾病编码
            string deseaseCode = m_IemMainPageEntity.IemDiagInfo.OutDiagID;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "疾病编码", deseaseCode, 194, "") + 10;

            ////入院时情况
            //g.DrawString("入院时情况", font, Brushes.Black, new PointF(pointX, pointY));
            //string inState = "8"; //todo
            //pointX = pointX + TextRenderer.MeasureText("入院时情况", font).Width + 5;
            //pointX = DrawCheckBox(g, pointX, pointY, inState, lineHeight);
            //pointX = DrawSelectItem(g, pointX, pointY, "1.危  2.急  3.一般") + 10;
            #endregion

            #region 9
            //pointY += interval;
            //pointX = pointStartX;

            ////入院诊断
            //string InDiaglosis = "我要发我要发我要发我要发我要发我要发我要发我要发";//todo
            //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院诊断", InDiaglosis, 432, "") + 10;

            ////入院后确诊日期
            //string inConfirmTime = "2011年10月20日";//todo
            //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院后确诊日期", inConfirmTime, 120, "") + 30;
            #endregion

            return pointY + 2;
        }
        DataTable newoutDiagTable = null;
        /// <summary>
        /// 出院诊断
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintOutHospitalDiaglosis(Graphics g, float pointY)
        {
            //表格的行高
            float lineHeight = 28f;
            float pointX = m_PointXPayType;
            float firstColumnWidth = 240f;//第一列宽度
            float secondColumnWidth = 100f;//第二列宽度
            float thirdColumnWidth = 43f;//第三列宽度

            float firstLineHeight = 36f;

            Font font = m_SmallFont;
            float offsetX = 12f;
            int lineWidth = 770;//需要保证 (firstColumnWidth + secondColumnWidth + thirdColumnWidth) == lineWidth

            #region TableHead

            //第一列

            g.DrawRectangle(pen, new Rectangle(new Point((int)pointX - 4, (int)pointY), new Size((int)firstColumnWidth + 4, (int)firstLineHeight)));
            //g.DrawLine(pen2, new Point((int)pointX - 1, (int)pointY + 1), new Point((int)pointX - 1, (int)pointY + (int)firstLineHeight - 1));
            g.DrawString("出院诊断", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, firstColumnWidth, firstLineHeight), sf);

            pointX += firstColumnWidth;

            //第二列
            g.DrawRectangle(pen, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)secondColumnWidth, (int)firstLineHeight)));
            g.DrawString("疾病编码", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, secondColumnWidth, firstLineHeight), sf);

            pointX += secondColumnWidth;

            //第三列
            g.DrawRectangle(pen, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)thirdColumnWidth, (int)firstLineHeight)));
            g.DrawString("入院情况", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY + 2, thirdColumnWidth, firstLineHeight), sf);

            pointX += thirdColumnWidth;

            //第四列
            g.DrawRectangle(pen, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)firstColumnWidth, (int)firstLineHeight)));
            g.DrawString("出院诊断", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, firstColumnWidth, firstLineHeight), sf);

            pointX += firstColumnWidth;

            //第五列
            g.DrawRectangle(pen, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)secondColumnWidth, (int)firstLineHeight)));
            g.DrawString("疾病编码", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, secondColumnWidth, firstLineHeight), sf);

            pointX += secondColumnWidth;

            //第六列
            g.DrawRectangle(pen, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)thirdColumnWidth, (int)firstLineHeight)));
            g.DrawLine(Pens.White, new Point((int)pointX + (int)thirdColumnWidth, (int)pointY), new Point((int)pointX + (int)thirdColumnWidth, (int)pointY + (int)firstLineHeight));
            g.DrawString("入院情况", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY + 2, thirdColumnWidth, firstLineHeight), sf);

            pointX = m_PointXPayType;
            pointY = pointY + firstLineHeight;
            #endregion

            #region TableBody

            float pointYTableBodyStart = pointY;
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;

            DataTable outDiagTable = m_IemMainPageEntity.IemDiagInfo.OutDiagTable;
            if (newoutDiagTable == null)
            {
                newoutDiagTable = outDiagTable.Copy();//声明一个用于存放去除了门急诊诊断的数据 add by ywk2012年6月4日 11:52:49
            }

            if (outDiagTable.Rows.Count > 0)
            {
                outDiagTable.DefaultView.RowFilter = " Diagnosis_Type_ID<>'13'";
                newoutDiagTable = outDiagTable.DefaultView.ToTable();
            }

            //【出院主诊断】
            //主诊断
            g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)firstColumnWidth, (int)lineHeight)));
            g.DrawLine(Pens.White, new Point((int)pointX, (int)pointYTableBodyStart), new Point((int)pointX, (int)pointYTableBodyStart + (int)lineHeight));
            g.DrawString("主要诊断:", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart + 2, firstColumnWidth, lineHeight), sf);
            //主诊断value
            //此处抓数据的时候把门急诊的也抓出来了，要排除掉 eidt by ywk 2012年6月4日 11:38:28
            if (newoutDiagTable.Rows.Count > 0)//todo
            {
                int stringWidth = TextRenderer.MeasureText("主要诊断:", m_DefaultFont).Width;
                //DrawStringInCell(g, outDiagTable.Rows[0]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                //    pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
                //outDiagTable.DefaultView.RowFilter = " Diagnosis_Type_ID<>'13'";
                DrawStringInCell(g, newoutDiagTable.Rows[0]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                   pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultValueFont);
            }

            pointX += firstColumnWidth;

            //疾病编码
            g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)secondColumnWidth, (int)lineHeight)));
            //疾病编码value
            if (newoutDiagTable.Rows.Count > 0)//todo
            {
                DrawStringInCell2(g, newoutDiagTable.Rows[0]["Diagnosis_Code"].ToString(), pointX,
                   pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultValueFont);
            }
            //    DrawStringInCell2(g, outDiagTable.Rows[0]["Diagnosis_Code"].ToString(), pointX,
            //        pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultFont);
            //}

            pointX += secondColumnWidth;

            //入院病情
            g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)thirdColumnWidth, (int)lineHeight)));
            //入院病情value
            if (newoutDiagTable.Rows.Count > 0)//todo
            {
                DrawStringInCell2(g, newoutDiagTable.Rows[0]["Status_Id"].ToString(), pointX,
                    pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultValueFont);
                //DrawStringInCell2(g, outDiagTable.Rows[0]["Status_Id"].ToString(), pointX,
                //    pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultFont);
            }

            pointX += thirdColumnWidth;

            //【其他诊断】

            for (int i = 0; i < 25; i++)//最多可以填写25个其他诊断(仁和诊断部分显示不全 edit by ywk 25)  new23
            {
                //【其他诊断 1-10】
                if (i <= 10)//i <= 10 ywk 
                {
                    if (i == 0)
                    {
                        pointX = m_PointXPayType;
                        pointYTableBodyStart = pointYTableBodyStart + lineHeight;
                        g.DrawString("其他诊断:", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth, lineHeight), sf);

                        //其他诊断value
                        //if (outDiagTable.Rows.Count > 1)ywk
                        if (newoutDiagTable.Rows.Count > 1)
                        {
                            int stringWidth = TextRenderer.MeasureText("其他诊断:", m_DefaultFont).Width;
                            DrawStringInCell(g, newoutDiagTable.Rows[1]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                              pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultValueFont);
                            //DrawStringInCell(g, outDiagTable.Rows[1]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                            //    pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
                        }
                    }

                    //其他诊断value
                    if (i > 0 && newoutDiagTable.Rows.Count > i + 1)
                    //if (i > 0 && outDiagTable.Rows.Count > i + 1)ywk
                    {
                        //DrawStringInCell(g, outDiagTable.Rows[i + 1]["Diagnosis_Name"].ToString(), pointX + offsetX,
                        //    pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultFont);
                        DrawStringInCell(g, newoutDiagTable.Rows[i + 1]["Diagnosis_Name"].ToString(), pointX + offsetX,
                           pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultValueFont);
                    }

                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)firstColumnWidth, (int)lineHeight)));
                    g.DrawLine(Pens.White, new Point((int)pointX, (int)pointYTableBodyStart), new Point((int)pointX, (int)pointYTableBodyStart + (int)lineHeight));

                    pointX += firstColumnWidth;
                    //疾病编码
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)secondColumnWidth, (int)lineHeight)));
                    //疾病编码value
                    if (newoutDiagTable.Rows.Count > i + 1)//todo
                    //if (outDiagTable.Rows.Count > i + 1)//todo----------ywk
                    {
                        DrawStringInCell2(g, newoutDiagTable.Rows[i + 1]["Diagnosis_Code"].ToString(), pointX,
                            pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultValueFont);
                    }

                    pointX += secondColumnWidth;
                    //入院病情
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)thirdColumnWidth, (int)lineHeight)));
                    //入院病情value
                    //if (outDiagTable.Rows.Count > i + 1)//todo   ywk
                    if (newoutDiagTable.Rows.Count > i + 1)//todo
                    {
                        //DrawStringInCell2(g, outDiagTable.Rows[i + 1]["Status_Id"].ToString(), pointX,
                        //    pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultFont);
                        DrawStringInCell2(g, newoutDiagTable.Rows[i + 1]["Status_Id"].ToString(), pointX,
                            pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultValueFont);
                    }

                    pointX = m_PointXPayType;
                    pointYTableBodyStart += lineHeight;
                }
                else if (i == 11)//11 ywk  9
                {
                    //【入院病情说明】
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart),
                        new Size((int)firstColumnWidth + (int)secondColumnWidth + (int)thirdColumnWidth, (int)lineHeight)));
                    g.DrawLine(Pens.White, new Point((int)pointX, (int)pointYTableBodyStart), new Point((int)pointX, (int)pointYTableBodyStart + (int)lineHeight));
                    g.DrawString("入院病情: 1.有  2.临床未确定  3.情况不明  4.无 ", m_DefaultValueFont, Brushes.Black,
                        new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth + secondColumnWidth + thirdColumnWidth, lineHeight), sf);
                }
                //【其他诊断 11-21】
                if (i >= 12)//12 ywk     -------12
                {
                    if (i == 12)//12 ywk  ------ 12
                    {
                        pointX = m_PointXPayType + firstColumnWidth + secondColumnWidth + thirdColumnWidth;
                        pointYTableBodyStart = pointY;
                        g.DrawString("其他诊断:", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth, lineHeight), sf);

                        //其他诊断value
                        //if (outDiagTable.Rows.Count > i)
                        if (newoutDiagTable.Rows.Count > i)
                        {
                            int stringWidth = TextRenderer.MeasureText("其他诊断:", m_DefaultFont).Width;
                            DrawStringInCell(g, newoutDiagTable.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                                pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultValueFont);
                        }
                    }

                    //其他诊断value
                    if (i > 12 && newoutDiagTable.Rows.Count > i)//12  ------ 12
                    {
                        DrawStringInCell(g, newoutDiagTable.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX,
                            pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultValueFont);
                    }

                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)firstColumnWidth, (int)lineHeight)));

                    pointX += firstColumnWidth;
                    //疾病编码
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)secondColumnWidth, (int)lineHeight)));
                    //疾病编码value
                    if (newoutDiagTable.Rows.Count > i)//todo
                    {
                        DrawStringInCell2(g, newoutDiagTable.Rows[i]["Diagnosis_Code"].ToString(), pointX,
                            pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultValueFont);
                    }

                    pointX += secondColumnWidth;
                    //入院病情
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)thirdColumnWidth, (int)lineHeight)));
                    //入院病情value
                    if (newoutDiagTable.Rows.Count > i)//todo
                    {
                        DrawStringInCell2(g, newoutDiagTable.Rows[i]["Status_Id"].ToString(), pointX,
                            pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultValueFont);
                    }

                    g.DrawLine(Pens.White, new Point((int)pointX + (int)thirdColumnWidth, (int)pointYTableBodyStart),
                        new Point((int)pointX + (int)thirdColumnWidth, (int)pointYTableBodyStart + (int)lineHeight));

                    pointX = m_PointXPayType + firstColumnWidth + secondColumnWidth + thirdColumnWidth;
                    pointYTableBodyStart += lineHeight;
                }

            }
            #region 原来的
            //for (int i = 0; i < 23; i++)//最多可以填写25个其他诊断(仁和诊断部分显示不全 edit by ywk 25)
            //{
            //    //【其他诊断 1-10】
            //    if (i <= 8)//i <= 10 ywk 
            //    {
            //        if (i == 0)
            //        {
            //            pointX = m_PointXPayType;
            //            pointYTableBodyStart = pointYTableBodyStart + lineHeight;
            //            g.DrawString("其他诊断:", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth, lineHeight), sf);

            //            //其他诊断value
            //            //if (outDiagTable.Rows.Count > 1)ywk
            //            if (newoutDiagTable.Rows.Count > 1)
            //            {
            //                int stringWidth = TextRenderer.MeasureText("其他诊断:", m_DefaultFont).Width;
            //                DrawStringInCell(g, newoutDiagTable.Rows[1]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
            //                  pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
            //                //DrawStringInCell(g, outDiagTable.Rows[1]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
            //                //    pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
            //            }
            //        }

            //        //其他诊断value
            //        if (i > 0 && newoutDiagTable.Rows.Count > i + 1)
            //        //if (i > 0 && outDiagTable.Rows.Count > i + 1)ywk
            //        {
            //            //DrawStringInCell(g, outDiagTable.Rows[i + 1]["Diagnosis_Name"].ToString(), pointX + offsetX,
            //            //    pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultFont);
            //            DrawStringInCell(g, newoutDiagTable.Rows[i + 1]["Diagnosis_Name"].ToString(), pointX + offsetX,
            //               pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultFont);
            //        }

            //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)firstColumnWidth, (int)lineHeight)));
            //        g.DrawLine(Pens.White, new Point((int)pointX, (int)pointYTableBodyStart), new Point((int)pointX, (int)pointYTableBodyStart + (int)lineHeight));

            //        pointX += firstColumnWidth;
            //        //疾病编码
            //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)secondColumnWidth, (int)lineHeight)));
            //        //疾病编码value
            //        if (newoutDiagTable.Rows.Count > i + 1)//todo
            //        //if (outDiagTable.Rows.Count > i + 1)//todo----------ywk
            //        {
            //            DrawStringInCell2(g, newoutDiagTable.Rows[i + 1]["Diagnosis_Code"].ToString(), pointX,
            //                pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultFont);
            //        }

            //        pointX += secondColumnWidth;
            //        //入院病情
            //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)thirdColumnWidth, (int)lineHeight)));
            //        //入院病情value
            //        //if (outDiagTable.Rows.Count > i + 1)//todo   ywk
            //        if (newoutDiagTable.Rows.Count > i + 1)//todo
            //        {
            //            //DrawStringInCell2(g, outDiagTable.Rows[i + 1]["Status_Id"].ToString(), pointX,
            //            //    pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultFont);
            //            DrawStringInCell2(g, newoutDiagTable.Rows[i + 1]["Status_Id"].ToString(), pointX,
            //                pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultFont);
            //        }

            //        pointX = m_PointXPayType;
            //        pointYTableBodyStart += lineHeight;
            //    }
            //    else if (i == 9)//11 ywk
            //    {
            //        //【入院病情说明】
            //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart),
            //            new Size((int)firstColumnWidth + (int)secondColumnWidth + (int)thirdColumnWidth, (int)lineHeight)));
            //        g.DrawLine(Pens.White, new Point((int)pointX, (int)pointYTableBodyStart), new Point((int)pointX, (int)pointYTableBodyStart + (int)lineHeight));
            //        g.DrawString("入院病情: 1.有  2.临床未确定  3.情况不明  4.无 ", m_DefaultFont, Brushes.Black,
            //            new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth + secondColumnWidth + thirdColumnWidth, lineHeight), sf);
            //    }
            //    //【其他诊断 11-21】
            //    if (i >= 12)//12 ywk
            //    {
            //        if (i == 12)//12 ywk
            //        {
            //            pointX = m_PointXPayType + firstColumnWidth + secondColumnWidth + thirdColumnWidth;
            //            pointYTableBodyStart = pointY;
            //            g.DrawString("其他诊断:", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth, lineHeight), sf);

            //            //其他诊断value
            //            //if (outDiagTable.Rows.Count > i)
            //            if (newoutDiagTable.Rows.Count > i)
            //            {
            //                int stringWidth = TextRenderer.MeasureText("其他诊断:", m_DefaultFont).Width;
            //                DrawStringInCell(g, newoutDiagTable.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
            //                    pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
            //            }
            //        }

            //        //其他诊断value
            //        if (i > 12 && newoutDiagTable.Rows.Count > i)//12
            //        {
            //            DrawStringInCell(g, newoutDiagTable.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX,
            //                pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultFont);
            //        }

            //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)firstColumnWidth, (int)lineHeight)));

            //        pointX += firstColumnWidth;
            //        //疾病编码
            //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)secondColumnWidth, (int)lineHeight)));
            //        //疾病编码value
            //        if (newoutDiagTable.Rows.Count > i)//todo
            //        {
            //            DrawStringInCell2(g, newoutDiagTable.Rows[i]["Diagnosis_Code"].ToString(), pointX,
            //                pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultFont);
            //        }

            //        pointX += secondColumnWidth;
            //        //入院病情
            //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)thirdColumnWidth, (int)lineHeight)));
            //        //入院病情value
            //        if (newoutDiagTable.Rows.Count > i)//todo
            //        {
            //            DrawStringInCell2(g, newoutDiagTable.Rows[i]["Status_Id"].ToString(), pointX,
            //                pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultFont);
            //        }

            //        g.DrawLine(Pens.White, new Point((int)pointX + (int)thirdColumnWidth, (int)pointYTableBodyStart),
            //            new Point((int)pointX + (int)thirdColumnWidth, (int)pointYTableBodyStart + (int)lineHeight));

            //        pointX = m_PointXPayType + firstColumnWidth + secondColumnWidth + thirdColumnWidth;
            //        pointYTableBodyStart += lineHeight;
            //    }

            //}
            #endregion
            #endregion

            return pointYTableBodyStart;
        }

        /// <summary>
        /// 第一页中出院诊断下面的部分
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintFristPageOther(Graphics g, float pointY)
        {
            //表格的行高
            float rowHeight = 25f;//24f
            float pointStartX = m_PointXPayType;
            Font font = m_DefaultFont;
            int lineHeight = TextRenderer.MeasureText("高", font).Height;
            int charWidth = TextRenderer.MeasureText("宽", font).Height;

            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            int lineWidth = 770;
            int xOffset = 12;

            #region 1
            float pointX = pointStartX + xOffset;
            //损伤、中毒的外部原因
            string outsideReason = m_IemMainPageEntity.IemDiagInfo.Hurt_Toxicosis_Element;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "损伤、中毒的外部原因", outsideReason, 339, "") + 10;

            //疾病编码
            string outsideReasondeseaseCode = m_IemMainPageEntity.IemDiagInfo.Hurt_Toxicosis_ElementID;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "疾病编码", outsideReasondeseaseCode, 160, "") + 10;

            pointY += rowHeight;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));
            #endregion

            #region 2
            pointX += xOffset;

            //病理诊断
            string pathologyDiagnisis = m_IemMainPageEntity.IemDiagInfo.Pathology_Diagnosis_Name;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "病理诊断", pathologyDiagnisis, 435, "") + 10;

            //疾病编码
            string pathologyDiagnisisDeseaseCode = m_IemMainPageEntity.IemDiagInfo.Pathology_Diagnosis_ID;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "疾病编码", pathologyDiagnisisDeseaseCode, 160, "") + 10;
            //g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

            pointY += rowHeight;
            pointX = pointStartX;



            string bingliNo = m_IemMainPageEntity.IemDiagInfo.Pathology_Observation_Sn;
            pointX = DrawNameAndValueAndUnderLine(g, pointX + lineWidth - 225, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "病理号", bingliNo, 160, "");

            pointY += rowHeight;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(pointX, pointY - 5), new PointF(pointX + lineWidth - 252, pointY - 5));
            g.DrawLine(pen, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

            #endregion

            #region 3
            pointX += xOffset;

            //药物过敏
            g.DrawString("过敏药物", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
            //string drugAllergy = string.IsNullOrEmpty(m_IemMainPageEntity.IemDiagInfo.Allergic_Drug.Trim()) ? "1" : "2" ; //todo
            string drugAllergy = m_IemMainPageEntity.IemDiagInfo.Allergic_Flag.Trim(); //edit by ywk 2012年3月6日14:36:31
            pointX = pointX + TextRenderer.MeasureText("过敏药物", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, drugAllergy, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.无 2.有");

            string drugAllergyName = m_IemMainPageEntity.IemDiagInfo.Allergic_Drug; //todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, " 过敏药物", drugAllergyName, 250, "") + 5;

            //死亡患者尸检
            g.DrawString("死亡患者尸检", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
            string isCheckDeadBody = m_IemMainPageEntity.IemBasicInfo.Autopsy_Flag; //todo
            pointX = pointX + TextRenderer.MeasureText("死亡患者尸检", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, isCheckDeadBody, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.是  2.否");

            pointY += rowHeight;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

            #endregion

            #region 4
            pointX += xOffset;

            //血型
            g.DrawString("血型", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
            string blood = m_IemMainPageEntity.IemDiagInfo.BloodType; //todo
            pointX = pointX + TextRenderer.MeasureText("血型", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, blood, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.A  2.B  3.O  4.AB  5.不详  6.未查") + 10;

            //Rh
            g.DrawString("Rh", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
            string bloodRh = m_IemMainPageEntity.IemDiagInfo.Rh; //todo
            pointX = pointX + TextRenderer.MeasureText("Rh", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, bloodRh, lineHeight);
            //pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.A  2.B  3.O  4.AB  5.不详  6.未查");
            pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.阴  2.阳  3.不详  4.未查");//edit by ywk 2012年3月6日15:04:45

            pointY += rowHeight;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

            #endregion

            #region 5
            pointX += xOffset;

            //科主任
            string deptDirecter = m_IemMainPageEntity.IemDiagInfo.Section_DirectorName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "科主任", deptDirecter, 85, "") + 5;

            //主任（副主任）医师
            string archiater = m_IemMainPageEntity.IemDiagInfo.DirectorName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "主任（副主任）医师", archiater, 95, "") + 5;

            //主治医师
            string attend = m_IemMainPageEntity.IemDiagInfo.Vs_EmployeeName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "主治医师", attend, 95, "") + 5;

            //住院医师
            string resident = m_IemMainPageEntity.IemDiagInfo.Resident_EmployeeName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "住院医师", resident, 100, "") + 5;

            pointY += rowHeight;
            pointX = pointStartX;

            #endregion

            #region 6

            pointX += xOffset;

            //责任护士
            string dutyNurse = m_IemMainPageEntity.IemDiagInfo.Duty_NurseName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "责任护士", dutyNurse, 90, "") + 63;

            //进修医师
            string furtherStudyDoctor = m_IemMainPageEntity.IemDiagInfo.Refresh_EmployeeName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "进修医师", furtherStudyDoctor, 95, "") + 5;

            //实习医师
            string practiceDoctor = m_IemMainPageEntity.IemDiagInfo.InterneName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "实习医师", practiceDoctor, 95, "") + 21;

            //编码员
            //edit by cyq 2012-11-23 固定员工(依据配置)
            string coder = GetFixedEncoderName();
            if (string.IsNullOrEmpty(coder.Trim()))
            {
                coder = m_IemMainPageEntity.IemDiagInfo.Coding_UserName;//todo
            }
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "编码员", coder, 100, "");

            pointY += rowHeight;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

            #endregion

            #region 7

            pointX += xOffset;

            //病案质量
            g.DrawString("病案质量", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
            string medicalRecordQuality = m_IemMainPageEntity.IemDiagInfo.Medical_Quality_Id; //todo
            pointX = pointX + TextRenderer.MeasureText("病案质量", font).Width + 8;
            pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, medicalRecordQuality, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.甲  2.乙  3.丙") + 10;

            //质控医师
            string qualityDoctor = m_IemMainPageEntity.IemDiagInfo.Quality_Control_DoctorName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "质控医师", qualityDoctor, 95, "") + 5;

            //质控护士
            string qualityNurse = m_IemMainPageEntity.IemDiagInfo.Quality_Control_NurseName;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "质控护士", qualityNurse, 95, "") + 5;

            //质控日期
            string qualityDateTime = m_IemMainPageEntity.IemDiagInfo.Quality_Control_DatePrint;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "质控日期", qualityDateTime, 100, "") + 15;

            pointY += rowHeight;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(pointX-4, pointY), new PointF(pointX + lineWidth, pointY));


            #endregion

            //if (M_TIPHIGHT == "1")
            //{
            //    pointY -= 2005f;
            //}
            //else
            //{
            //    pointY += rowHeight;
            //}


            //绘制页数
            //g.DrawString("第 1 页", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, lineWidth, rowHeight), sf);

            return pointY;
        }

        #endregion

        #region 【打印第二页】

        void PrintSecondPage(Graphics g)
        {
            float pointY;

            pointY = PrintOperation(g, m_PointYTitle) + 30;
            pointY = PrintSecondPageOther(g, pointY) + 20;
            //if (m_IemMainPageEntity.IemObstetricsBaby != null)
            //    pointY = FuKeChanKeYingEr(g, pointY) + 20;
            pointY = PrintFee(g, pointY) + 20;
            pointY = PrintNote(g, pointY) + 20;
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
            int firstLineHieght = 38;
            int lineHeight = 28;
            float pointX = m_PointXPayType;//15m_PointXPayType
            Font font = m_DefaultFont;
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            Pen pen = new Pen(Brushes.Black, 2);
            g.DrawLine(pen, new PointF(pointX - 2, pointY), new PointF(pointX - 2, pointY + lineHeight * 34));
            g.DrawLine(pen, new PointF(pointX + 770, pointY), new PointF(pointX + 770, pointY + lineHeight * 34));

            //int lineHeight = TextRenderer.MeasureText("高", font).Height;
            //int charWidth = TextRenderer.MeasureText("宽", font).Height;

            //int columnWidth1 = 90;//手术及操作编码
            //int columnWidth2 = 100;//手术及操作日期

            //宽度调整 edit by ywk 2012年4月18日13:32:46
            int columnWidth1 = 74;//手术及操作编码
            int columnWidth2 = 120;//手术及操作日期

            int columnWidth3 = 39;//手术级别
            int columnWidth4 = 128;//手术及操作名称
            int columnWidth5 = 210;//手术及操作医师 术者60 Ⅰ助60 Ⅱ助60
            int columnWidth6 = 59;//切口愈合等级
            int columnWidth7 = 69;//麻醉方式
            int columnWidth8 = 69;//麻醉医师

            int lineWidth = 770; //ColumnWidth1 + ColumnWidth2 + ...... = lineWidth
            int xOffset = 12;

            Pen solidPen = new Pen(Brushes.Black, 2);
            g.DrawLine(solidPen, new PointF(pointX-3, pointY), new PointF(pointX + lineWidth, pointY));

            #region Table Header
            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth1, firstLineHieght));
            g.DrawString("手术及", font, Brushes.Black, new RectangleF(pointX, pointY + 3, columnWidth1, firstLineHieght / 2), sf);
            g.DrawString("操作编码", font, Brushes.Black, new RectangleF(pointX, pointY + firstLineHieght / 2 + 2, columnWidth1, firstLineHieght / 2), sf);
            g.DrawLine(Pens.White, new Point((int)pointX, (int)pointY), new Point((int)pointX, (int)pointY + firstLineHieght));

            pointX += columnWidth1;

            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth2, firstLineHieght));
            g.DrawString("手术及", font, Brushes.Black, new RectangleF(pointX, pointY + 3, columnWidth2, firstLineHieght / 2), sf);
            g.DrawString("操作日期", font, Brushes.Black, new RectangleF(pointX, pointY + firstLineHieght / 2 + 2, columnWidth2, firstLineHieght / 2), sf);

            pointX += columnWidth2;

            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth3, firstLineHieght));
            g.DrawString("手术", font, Brushes.Black, new RectangleF(pointX, pointY + 3, columnWidth3, firstLineHieght / 2), sf);
            g.DrawString("级别", font, Brushes.Black, new RectangleF(pointX, pointY + firstLineHieght / 2 + 2, columnWidth3, firstLineHieght / 2), sf);

            pointX += columnWidth3;

            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth4, firstLineHieght));
            g.DrawString("手术及操作名称", font, Brushes.Black, new RectangleF(pointX, pointY + 2, columnWidth4, firstLineHieght), sf);

            pointX += columnWidth4;

            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5, firstLineHieght / 2));
            g.DrawString("手术及操作医师", font, Brushes.Black, new RectangleF(pointX, pointY + 3, columnWidth5, firstLineHieght / 2), sf);
            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY + firstLineHieght / 2, columnWidth5 / 3, firstLineHieght / 2));
            g.DrawString("术者", font, Brushes.Black, new RectangleF(pointX, pointY + firstLineHieght / 2 + 3, columnWidth5 / 3, firstLineHieght / 2), sf);

            pointX += columnWidth5 / 3;

            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY + firstLineHieght / 2, columnWidth5 / 3, firstLineHieght / 2));
            g.DrawString("Ⅰ助", font, Brushes.Black, new RectangleF(pointX, pointY + firstLineHieght / 2 + 3, columnWidth5 / 3, firstLineHieght / 2), sf);

            pointX += columnWidth5 / 3;

            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY + firstLineHieght / 2, columnWidth5 / 3, firstLineHieght / 2));
            g.DrawString("Ⅱ助", font, Brushes.Black, new RectangleF(pointX, pointY + firstLineHieght / 2 + 3, columnWidth5 / 3, firstLineHieght / 2), sf);

            pointX += columnWidth5 / 3;

            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth6, firstLineHieght));
            g.DrawString("切口愈", font, Brushes.Black, new RectangleF(pointX, pointY + 3, columnWidth6, firstLineHieght / 2), sf);
            g.DrawString("合等级", font, Brushes.Black, new RectangleF(pointX, pointY + firstLineHieght / 2 + 2, columnWidth6, firstLineHieght / 2), sf);

            pointX += columnWidth6;

            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth7, firstLineHieght));
            g.DrawString("麻醉方式", font, Brushes.Black, new RectangleF(pointX, pointY + 2, columnWidth7, firstLineHieght), sf);

            pointX += columnWidth7;

            g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth8, firstLineHieght));
            g.DrawString("麻醉医师", font, Brushes.Black, new RectangleF(pointX, pointY + 2, columnWidth8, firstLineHieght), sf);
            g.DrawLine(Pens.White, new Point((int)pointX + columnWidth8, (int)pointY), new Point((int)pointX + columnWidth8, (int)pointY + firstLineHieght));

            #endregion

            #region Table Body
            pointX = m_PointXPayType;
            pointY += firstLineHieght;
            DataTable operationTable = m_IemMainPageEntity.IemOperInfo.Operation_Table;

            for (int i = 0; i < 8; i++)
            {
                //手术及操作编码
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth1, lineHeight));
                g.DrawLine(Pens.White, new Point((int)pointX, (int)pointY), new Point((int)pointX, (int)pointY + lineHeight));
                //手术及操作编码value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Operation_Code"].ToString(), pointX, pointY + 2, columnWidth1, lineHeight, m_DefaultFont);
                }

                pointX += columnWidth1;

                //手术及操作日期
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth2, lineHeight));
                //手术及操作日期value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Operation_Date"].ToString(), pointX, pointY + 2, columnWidth2, lineHeight, m_DefaultValueFont);
                }

                pointX += columnWidth2;

                //手术级别
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth3, lineHeight));
                //手术级别value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Operation_Level"].ToString(), pointX, pointY + 2, columnWidth3, lineHeight, m_DefaultFont);
                }

                pointX += columnWidth3;

                //手术及操作名称
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth4, lineHeight));
                //手术及操作名称value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Operation_Name"].ToString(), pointX, pointY + 2, columnWidth4, lineHeight, m_DefaultFont);
                }

                pointX += columnWidth4;

                //手术及操作医师【术者】
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5 / 3, lineHeight));
                //手术及操作医师【术者】value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Execute_User1_Name"].ToString(), pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                }

                pointX += columnWidth5 / 3;

                //手术及操作医师【Ⅰ助】
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5 / 3, lineHeight));
                //手术及操作医师【Ⅰ助】value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Execute_User2_Name"].ToString(), pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                }

                pointX += columnWidth5 / 3;

                //手术及操作医师【Ⅱ助】
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5 / 3, lineHeight));
                //手术及操作医师【Ⅱ助】value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Execute_User3_Name"].ToString(), pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                }

                pointX += columnWidth5 / 3;

                //切口愈合等级
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth6, lineHeight));
                //切口愈合等级value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Close_Level_Name"].ToString(), pointX, pointY + 2, columnWidth6, lineHeight, m_DefaultFont);
                }

                pointX += columnWidth6;

                //麻醉方式
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth7, lineHeight));
                //切口愈合等级value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Anaesthesia_Type_Name"].ToString(), pointX, pointY + 2, columnWidth7, lineHeight, m_DefaultFont);
                }

                pointX += columnWidth7;

                //麻醉医师
                g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth8, lineHeight));
                //麻醉医师value
                if (operationTable.Rows.Count > i)
                {
                    DrawStringInCell2(g, operationTable.Rows[i]["Anaesthesia_User_Name"].ToString(), pointX, pointY + 2, columnWidth8, lineHeight, m_DefaultFont);
                }

                g.DrawLine(Pens.White, new Point((int)pointX + columnWidth8, (int)pointY), new Point((int)pointX + columnWidth8, (int)pointY + lineHeight));

                pointX = m_PointXPayType;
                if (i == 7)
                {
                    g.DrawLine(solidPen, new Point((int)pointX, (int)pointY + lineHeight), new Point((int)pointX + lineWidth, (int)pointY + lineHeight));
                    pointY += 6;
                }
                else
                {
                    pointY += lineHeight;
                }
            }
            #endregion

            return pointY;
        }

        /// <summary>
        /// 打印第二页手术和妇科之间的部分
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintSecondPageOther(Graphics g, float pointY)
        {
            int rowHeight = 24;
            Font font = m_DefaultFont;
            Pen solidPen = new Pen(Brushes.Black, 2);
            int lineHeight = TextRenderer.MeasureText("高", font).Height;
            int charWidth = TextRenderer.MeasureText("宽", font).Height;

            int lineWidth = 770;
            int offsetX = 12;

            float pointX = m_PointXPayType + offsetX;

            //转归
            //g.DrawString("转归：", font, Brushes.Black, new PointF(pointX, pointY));
            //string zg = m_IemMainPageEntity.IemBasicInfo.ZG_FLAG; //todo
            //pointX = pointX + TextRenderer.MeasureText("转归：", font).Width + 5;
            //pointX = DrawCheckBox(g, pointX, pointY, zg, lineHeight);
            //pointX = DrawSelectItem(g, pointX, pointY, "1.治愈 2.好转 3.未愈 4.死亡 5.其他");

            //pointY += rowHeight;
            //pointX = m_PointXPayType + offsetX;

            //并发症 add by cyq 2012-10-16
            string cansee = dHelper.GetConfigValueByKey("EmrInputConfig");
            XmlDocument doc1 = new XmlDocument();
            doc1.LoadXml(cansee);
            if (doc1.GetElementsByTagName("IemPageContorlVisable")[0].InnerText == "1")//可见手术符合的一些栏位
            {
                g.DrawString("并发症：", font, Brushes.Black, new PointF(pointX, pointY));
                pointX = pointX + TextRenderer.MeasureText("并发症：", font).Width + 5;

                List<string> codeList = m_IemMainPageEntity.IemOperInfo.Operation_Table.AsEnumerable().Select(p => p["opercomplication_code"].ToString()).ToList();
                string sqlStr = @" select * from Complications where valid != 0 ";
                string condition = GetSqlConditionStrByList(codeList);
                if (!string.IsNullOrEmpty(condition))
                {
                    sqlStr += @" and ICD_Code in(" + condition + ") ";
                    sqlStr += @" order by SortIndex ";
                    DataTable dt = dHelper.GetDataTableBySql(sqlStr);
                    string bfzStr = string.Empty;
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        int i = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            i++;
                            bfzStr += i.ToString() + "." + dr["NAME"].ToString() + " ";
                        }
                    }
                    pointX = DrawSelectItem(g, pointX, pointY, bfzStr);
                }
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;
            }

            //离院方式
            g.DrawString("离院方式：", font, Brushes.Black, new PointF(pointX, pointY));
            string leaveType = m_IemMainPageEntity.IemBasicInfo.OutHosType; //todo
            pointX = pointX + TextRenderer.MeasureText("离院方式:", font).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, leaveType, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.医嘱离院  2.医嘱转院,拟接收医疗机构名称:");

            //拟接收医疗机构名称
            string medicalName = m_IemMainPageEntity.IemBasicInfo.ReceiveHosPital;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", medicalName, 360, "");

            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //医嘱转社区卫生服务机构/乡镇卫生院
            string healthOrganizations = m_IemMainPageEntity.IemBasicInfo.ReceiveHosPital2;//todo
            pointX = DrawSelectItem(g, pointX, pointY, "3.医嘱转社区卫生服务机构/乡镇卫生院，拟接收医疗机构名称:");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", healthOrganizations, 180, " ");
            pointX = DrawSelectItem(g, pointX, pointY, "4.非医嘱离院 5.死亡 9.其他");
            pointY += rowHeight / 2 + 8;
            pointX = m_PointXPayType;

            g.DrawLine(pen, new Point((int)pointX, (int)pointY), new Point((int)pointX + lineWidth, (int)pointY));

            pointY += rowHeight / 2 - 4;
            pointX += offsetX;

            //是否有出院31天内再住院计划
            g.DrawString("是否有出院31天内再住院计划：", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            string inHospitalAgain = m_IemMainPageEntity.IemBasicInfo.AgainInHospital; //todo
            pointX = pointX + TextRenderer.MeasureText("是否有出院31天内再住院计划：", m_DefaultValueFont).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, inHospitalAgain, lineHeight);
            pointX = DrawSelectItem(g, pointX, pointY, "1.无  2.有，目的:");

            //目的
            string purpose = m_IemMainPageEntity.IemBasicInfo.AgainInHospitalReason;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", purpose, 410, "");

            pointY += rowHeight / 2 + 8;
            pointX = m_PointXPayType;

            g.DrawLine(pen, new Point((int)pointX, (int)pointY), new Point((int)pointX + lineWidth, (int)pointY));

            pointY += rowHeight / 2 - 4;
            pointX += offsetX;

            //颅脑损伤患者昏迷时间
            pointX = DrawSelectItem(g, pointX, pointY, "颅脑损伤患者昏迷时间： 入院前");
            string sleepDayPre = m_IemMainPageEntity.IemBasicInfo.BeforeHosComaDay;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepDayPre, 40, "");
            pointX = DrawSelectItem(g, pointX, pointY, "天");
            string sleepHourPre = m_IemMainPageEntity.IemBasicInfo.BeforeHosComaHour;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepHourPre, 40, "");
            pointX = DrawSelectItem(g, pointX, pointY, "小时");
            string sleepMinutePre = m_IemMainPageEntity.IemBasicInfo.BeforeHosComaMinute;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepMinutePre, 40, "");
            pointX = DrawSelectItem(g, pointX, pointY, "分钟") + 20;


            //颅脑损伤患者昏迷时间
            pointX = DrawSelectItem(g, pointX, pointY, "入院后");
            string sleepDayAfter = m_IemMainPageEntity.IemBasicInfo.LaterHosComaDay;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepDayAfter, 40, "");
            pointX = DrawSelectItem(g, pointX, pointY, "天");
            string sleepHourAfter = m_IemMainPageEntity.IemBasicInfo.LaterHosComaHour;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepHourAfter, 40, "");
            pointX = DrawSelectItem(g, pointX, pointY, "小时");
            string sleepMinuteAfter = m_IemMainPageEntity.IemBasicInfo.LaterHosComaMinute;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepMinuteAfter, 40, "");
            pointX = DrawSelectItem(g, pointX, pointY, "分钟");

            pointY += rowHeight / 2 + 8;
            pointX = m_PointXPayType;

            g.DrawLine(solidPen, new Point((int)pointX, (int)pointY), new Point((int)pointX + lineWidth, (int)pointY));

            return pointY;
        }
        #region 注释的产妇婴儿情况
        /// <summary>
        /// 妇科产妇婴儿情况   产妇婴儿暂不显示 add by ywk 2012年11月28日13:34:02 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        //private float FuKeChanKeYingEr(Graphics g, float pointY)
        //{
        //float interval = 25; //行间距
        //Font font = m_DefaultFont;
        //Font font2 = new Font("宋体", 25f, FontStyle.Bold, GraphicsUnit.Pixel);
        //Pen solidPen = new Pen(Brushes.Black, 2);
        //int lineHeight = TextRenderer.MeasureText("高", font).Height;
        //int charWidth = TextRenderer.MeasureText("宽", font).Height;

        //float pointStartX = m_PointXPayType + 12;
        //float pointX = pointStartX;

        //int lineWidth = 770;
        //int offsetX = 12;

        //g.DrawString("产 科 产 妇 婴 儿 情 况", font2, Brushes.Black, new RectangleF(m_PointXPayType, pointY, lineWidth, 30), sf);
        //pointY = pointY + 40;
        //g.DrawLine(solidPen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + lineWidth, pointY));
        //pointY = pointY + 10;

        //#region 1
        ////胎次
        //g.DrawString("胎次：", font, Brushes.Black, new PointF(pointX, pointY));
        //string taiCi = m_IemMainPageEntity.IemObstetricsBaby.TC; //todo
        //pointX = pointX + TextRenderer.MeasureText("胎次：", font).Width;
        //pointX = DrawCheckBox(g, pointX, pointY, taiCi, lineHeight) + 5;

        ////产次
        //g.DrawString("产次：", font, Brushes.Black, new PointF(pointX, pointY));
        //string chanCi = m_IemMainPageEntity.IemObstetricsBaby.CC; //todo
        //pointX = pointX + TextRenderer.MeasureText("产次：", font).Width;
        //pointX = DrawCheckBox(g, pointX, pointY, chanCi, lineHeight) + 5;

        ////胎别
        //g.DrawString("胎别：", font, Brushes.Black, new PointF(pointX, pointY));
        //string taiBie = m_IemMainPageEntity.IemObstetricsBaby.TB; //todo
        //pointX = pointX + TextRenderer.MeasureText("胎别：", font).Width;
        //pointX = DrawCheckBox(g, pointX, pointY, taiBie, lineHeight);

        //pointX = DrawSelectItem(g, pointX, pointY, "（1.单 2.双 3.多）") + 5;

        ////产妇会阴破裂度
        //g.DrawString("产妇会阴破裂度：", font, Brushes.Black, new PointF(pointX, pointY));
        //string poLieLevel = m_IemMainPageEntity.IemObstetricsBaby.CFHYPLD; //todo
        //pointX = pointX + TextRenderer.MeasureText("产妇会阴破裂度：", font).Width;
        //pointX = DrawCheckBox(g, pointX, pointY, poLieLevel, lineHeight);

        //pointX = DrawSelectItem(g, pointX, pointY, "（1.Ⅰ 2.Ⅱ 3.Ⅲ）")+5;

        ////接产者
        //string acceptUser = m_IemMainPageEntity.IemObstetricsBaby.Midwifery;//todo
        //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "接产者：", acceptUser, 60, "");
        //#endregion

        //#region 2

        //pointY += interval;
        //pointX = pointStartX;

        ////胎儿性别
        //g.DrawString("性别：", font, Brushes.Black, new PointF(pointX, pointY));
        //string gender = m_IemMainPageEntity.IemObstetricsBaby.Sex; //todo
        //pointX = pointX + TextRenderer.MeasureText("性别：", font).Width;
        //pointX = DrawCheckBox(g, pointX, pointY, gender, lineHeight) + 5;

        //pointX = DrawSelectItem(g, pointX, pointY, "（1.男  2.女）") + 5;

        ////阿帕加评分
        //g.DrawString("阿帕加评分：", font, Brushes.Black, new PointF(pointX, pointY));
        //string apajiapingjia = m_IemMainPageEntity.IemObstetricsBaby.APJ; //todo
        //pointX = pointX + TextRenderer.MeasureText("阿帕加评分：", font).Width;
        //pointX = DrawCheckBox(g, pointX, pointY, apajiapingjia, lineHeight) + 5;

        //pointX = DrawSelectItem(g, pointX, pointY, "（1—分…A+分）") + 100;

        ////身长
        //string bodyLength = m_IemMainPageEntity.IemObstetricsBaby.Heigh;//todo
        //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "身长：", bodyLength, 50, "CM") + 5;

        ////体重
        //string bodyWeight = m_IemMainPageEntity.IemObstetricsBaby.Weight;//todo
        //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "体重：", bodyWeight, 50, "G") + 5;
        //#endregion

        //#region 3

        //pointY += interval;
        //pointX = pointStartX;

        ////产出情况
        //g.DrawString("产出情况：", font, Brushes.Black, new PointF(pointX, pointY));
        //string birthStatus = m_IemMainPageEntity.IemObstetricsBaby.CCQK; //todo
        //pointX = pointX + TextRenderer.MeasureText("产出情况：", font).Width;
        //pointX = DrawCheckBox(g, pointX, pointY, birthStatus, lineHeight) + 5;

        //pointX = DrawSelectItem(g, pointX, pointY, "（1.产活  2.产死  3.死胎  4.畸形）") + 165;

        ////出生
        //string birth = m_IemMainPageEntity.IemObstetricsBaby.BithDayPrint;//todo
        //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生：", birth, 175, "") + 30;

        //#endregion

        //#region 4

        //pointY += interval;
        //pointX = pointStartX;

        ////分娩方式
        //g.DrawString("分娩方式：", font, Brushes.Black, new PointF(pointX, pointY));
        //string birthType = m_IemMainPageEntity.IemObstetricsBaby.FMFS; //todo
        //pointX = pointX + TextRenderer.MeasureText("分娩方式：", font).Width;
        //pointX = DrawCheckBox(g, pointX, pointY, birthType, lineHeight)+5;

        //pointX = DrawSelectItem(g, pointX, pointY, "（1.自然 2.测+吸 3.产钳 4.臂牵引 5.剖宫 6.其他）") + 5;

        ////出院情况
        //g.DrawString("出院情况：", font, Brushes.Black, new PointF(pointX, pointY));
        //string outHospitalStatus = m_IemMainPageEntity.IemObstetricsBaby.CYQK; //todo
        //pointX = pointX + TextRenderer.MeasureText("出院情况：", font).Width;
        //pointX = DrawCheckBox(g, pointX, pointY, outHospitalStatus, lineHeight) + 5;

        //pointX = DrawSelectItem(g, pointX, pointY, "（1.正常  2.有病  3.交叉感染）");

        //#endregion

        //pointX = pointStartX;
        //pointY = pointY + 25;
        //g.DrawLine(solidPen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + lineWidth, pointY));

        //return pointY;
        //}
        #endregion
        /// <summary>
        /// 打印费用
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintFee(Graphics g, float pointY)
        {
            Font font = m_DefaultFont;
            int lineHeight = TextRenderer.MeasureText("高", font).Height;
            int charWidth = TextRenderer.MeasureText("宽", font).Height;
            float rowHeight = 26; //行间距
            float pointX = m_PointXPayType;
            Pen solidPen = new Pen(Brushes.Black, 2);
            int lineWidth = 770;
            int offsetX = 12;

            //g.DrawLine(solidPen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + lineWidth, pointY));
            //pointY = pointY + 10;

            #region 1
            pointX = m_PointXPayType + offsetX;

            //住院费用（元）：总费用
            string totalFee = m_IemMainPageEntity.IemFeeInfo.Total;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "住院费用（元）：总费用", totalFee, 150, "");

            //自付金额
            string selfFee = m_IemMainPageEntity.IemFeeInfo.OwnFee;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（自付金额：", selfFee, 150, ")");

            #endregion

            #region 2
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //一般医疗服务费
            g.DrawString("1.综合医疗服务类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("1.综合医疗服务类:", m_DefaultFont).Width + 5;
            g.DrawString("（1）一般医疗服务费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（1）一般医疗服务费:", m_DefaultValueFont).Width + 5;
            string normalMedicalServices = m_IemMainPageEntity.IemFeeInfo.YBYLFY;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", normalMedicalServices, 65, "") + 2;

            //一般治疗操作费
            g.DrawString("（2）一般治疗操作费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（2）一般治疗操作费:", m_DefaultValueFont).Width + 5;
            string normalOperateServices = m_IemMainPageEntity.IemFeeInfo.YBZLFY;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", normalOperateServices, 65, "") + 2;

            //护理费
            g.DrawString("（3）护理费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（3）护理费:", m_DefaultValueFont).Width + 5;
            string nurseServices = m_IemMainPageEntity.IemFeeInfo.Care;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", nurseServices, 65, "") + 5;
            #endregion

            #region 3
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //其他费用
            g.DrawString("（4）其他费用:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（4）其他费用:", m_DefaultValueFont).Width + 5;
            string otherServices = m_IemMainPageEntity.IemFeeInfo.ZHQTFY;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", otherServices, 100, "") + 5;
            #endregion

            #region 4
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //病理诊断费
            g.DrawString("2.诊断类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("2.诊断类:", m_DefaultFont).Width + 5;
            g.DrawString("（5）病理诊断费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（5）病理诊断费:", m_DefaultValueFont).Width + 5;
            string pathologyDiagnisis = m_IemMainPageEntity.IemFeeInfo.BLZDF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", pathologyDiagnisis, 82, "") + 5;

            //实验室诊断费
            g.DrawString("（6）实验室诊断费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（6）实验室诊断费:", m_DefaultValueFont).Width + 5;
            string libDiagnisis = m_IemMainPageEntity.IemFeeInfo.SYSZDF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", libDiagnisis, 82, "") + 5;

            //影像学诊断费
            g.DrawString("（7）影像学诊断费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（7）影像学诊断费:", m_DefaultValueFont).Width + 5;
            string imagingDiagnisis = m_IemMainPageEntity.IemFeeInfo.YXXZDF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", imagingDiagnisis, 82, "") + 5;
            #endregion

            #region 5
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //临床诊断项目费
            g.DrawString("（8）临床诊断项目费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（8）临床诊断项目费:", m_DefaultValueFont).Width + 5;
            string clinicDiagnisis = m_IemMainPageEntity.IemFeeInfo.LCZDF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", clinicDiagnisis, 100, "") + 5;
            #endregion

            #region 6
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //非手术治疗项目费
            g.DrawString("3.治疗类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("3.治疗类:", m_DefaultFont).Width + 5;
            g.DrawString("（9）非手术治疗项目费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（9）非手术治疗项目费:", m_DefaultValueFont).Width + 5;
            string unOperationFee = m_IemMainPageEntity.IemFeeInfo.FSSZLF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", unOperationFee, 100, "");

            //临床物理治疗费
            g.DrawString("（临床物理治疗费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（临床物理治疗费:", m_DefaultValueFont).Width + 5;
            string clinicPhysicsFee = m_IemMainPageEntity.IemFeeInfo.LCWLZLF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", clinicPhysicsFee, 100, "") + 5;
            g.DrawString("）", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            #endregion

            #region 7
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //手术治疗费
            g.DrawString("（10）手术治疗费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（10）手术治疗费:", m_DefaultValueFont).Width + 5;
            string operationTreatFee = m_IemMainPageEntity.IemFeeInfo.SSZLF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", operationTreatFee, 100, "");

            //麻醉费
            g.DrawString("（麻醉费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（麻醉费:", m_DefaultValueFont).Width + 5;
            string anesthesiaFee = m_IemMainPageEntity.IemFeeInfo.MZF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", anesthesiaFee, 100, "");

            //手术费
            g.DrawString("手术费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("手术费:", m_DefaultValueFont).Width + 5;
            string operationFee = m_IemMainPageEntity.IemFeeInfo.SSF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", operationFee, 100, "");
            g.DrawString("）", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));

            #endregion

            #region 8
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //康复费
            g.DrawString("4.康复类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("4.康复类:", m_DefaultFont).Width + 5;
            g.DrawString("（11）康复费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（11）康复费:", m_DefaultValueFont).Width + 5;
            string recoveryFee = m_IemMainPageEntity.IemFeeInfo.KFF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", recoveryFee, 100, "");
            #endregion

            #region 9
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //中医治疗费
            g.DrawString("5.中医类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("5.中医类:", m_DefaultFont).Width + 5;
            g.DrawString("（12）中医治疗费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（12）中医治疗费:", m_DefaultValueFont).Width + 5;
            string chineseMedicine = m_IemMainPageEntity.IemFeeInfo.ZYZLF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", chineseMedicine, 100, "");
            #endregion

            #region 10
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //西药费
            g.DrawString("6.西药类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("6.西药类:", m_DefaultFont).Width + 5;
            g.DrawString("（13）西药费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（13）西药费:", m_DefaultValueFont).Width + 5;
            string westDrug = m_IemMainPageEntity.IemFeeInfo.XYF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", westDrug, 100, "");

            //抗菌药物费用
            g.DrawString("（抗菌药物费用:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（抗菌药物费用:", m_DefaultValueFont).Width + 5;
            string antibiosisDrug = m_IemMainPageEntity.IemFeeInfo.KJYWF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", antibiosisDrug, 100, "");
            g.DrawString("）", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            #endregion

            #region 11
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //中成药费
            g.DrawString("7.中药类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("7.中药类:", m_DefaultFont).Width + 5;
            g.DrawString("（14）中成药费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（14）中成药费:", m_DefaultValueFont).Width + 5;
            string zhongChenDrug = m_IemMainPageEntity.IemFeeInfo.CPMedical;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", zhongChenDrug, 100, "");

            //中草药费
            g.DrawString("（15）中草药费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（15）中草药费:", m_DefaultValueFont).Width + 5;
            string zhongCaoDrug = m_IemMainPageEntity.IemFeeInfo.CMedical;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", zhongCaoDrug, 100, "");

            #endregion

            #region 12
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //血费
            g.DrawString("8.血液和血液制品类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("8.血液和血液制品类:", m_DefaultFont).Width + 5;
            g.DrawString("（16）血费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（16）血费:", m_DefaultValueFont).Width + 5;
            string bloodFee = m_IemMainPageEntity.IemFeeInfo.BloodFee;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", bloodFee, 58, "");

            //白蛋白类制品费
            g.DrawString("（17）白蛋白类制品费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（17）白蛋白类制品费:", m_DefaultValueFont).Width + 5;
            string baiDanBaiFee = m_IemMainPageEntity.IemFeeInfo.XDBLZPF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", baiDanBaiFee, 58, "");

            //球蛋白类制品费
            g.DrawString("（18）球蛋白类制品费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（18）球蛋白类制品费:", m_DefaultValueFont).Width + 5;
            string qiuDanBaiFee = m_IemMainPageEntity.IemFeeInfo.QDBLZPF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", qiuDanBaiFee, 58, "");

            #endregion

            #region 13
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //凝血因子类制品费
            g.DrawString("（19）凝血因子类制品费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（19）凝血因子类制品费:", m_DefaultValueFont).Width + 5;
            string ninXueFee = m_IemMainPageEntity.IemFeeInfo.NXYZLZPF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", ninXueFee, 100, "");

            //细胞因子类制品费
            g.DrawString("（20）细胞因子类制品费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（20）细胞因子类制品费:", m_DefaultValueFont).Width + 5;
            string cellFee = m_IemMainPageEntity.IemFeeInfo.XBYZLZPF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", cellFee, 100, "");

            #endregion

            #region 14
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //检查用一次性医用材料费
            g.DrawString("9.耗材类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("9.耗材类:", m_DefaultFont).Width + 5;
            g.DrawString("（21）检查用一次性医用材料费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（21）检查用一次性医用材料费:", m_DefaultValueFont).Width + 5;
            string checkOnceFee = m_IemMainPageEntity.IemFeeInfo.JCYYCXCLF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", checkOnceFee, 100, "");

            //治疗用一次性医用材料费
            g.DrawString("（22）治疗用一次性医用材料费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（22）治疗用一次性医用材料费:", m_DefaultValueFont).Width + 5;
            string treatOnceFee = m_IemMainPageEntity.IemFeeInfo.ZLYYCXCLF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", treatOnceFee, 100, "");
            #endregion

            #region 15
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //手术用一次性医用材料费
            g.DrawString("（23）手术用一次性医用材料费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（23）手术用一次性医用材料费:", m_DefaultValueFont).Width + 5;
            string operateOnceFee = m_IemMainPageEntity.IemFeeInfo.SSYYCXCLF;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", operateOnceFee, 100, "");
            #endregion

            #region 16
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            g.DrawString("10.其他类:", m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("10.其他类:", m_DefaultFont).Width + 5;
            g.DrawString("（24）其他费:", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("（24）其他费:", m_DefaultValueFont).Width + 5;
            string otherFee = m_IemMainPageEntity.IemFeeInfo.OtherFee;//todo
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", otherFee, 100, "");
            #endregion

            pointY = pointY + 22;
            g.DrawLine(solidPen, new PointF(m_PointXPayType-3, pointY), new PointF(m_PointXPayType + lineWidth, pointY));

            #region 主要诊断 抢救 临床路径 add by 王冀 2012 12 4
            pointY += 22;
            pointX = m_PointXPayType + offsetX;

            g.DrawString("主要诊断治愈好转情况：", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            string maindiagnosis = m_IemMainPageEntity.IemOthers.Main_Diagnosis_Curecondition;
            pointX = pointX + TextRenderer.MeasureText("主要诊断治愈好转情况：", m_DefaultValueFont).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, maindiagnosis, lineHeight) + 20;
            pointX = DrawSelectItem(g, pointX, pointY, "1.治愈  2.好转  3.未愈  4.死亡  5.其他");

            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            g.DrawString("诊断符合情况：1.门诊与住院", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            string dia1 = m_IemMainPageEntity.IemOthers.Diagnosis_conditions1;
            pointX = pointX + TextRenderer.MeasureText("诊断符合情况：1.门诊与住院", m_DefaultValueFont).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, dia1, lineHeight) + 20;

            g.DrawString("2.入院与出院", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            string dia2 = m_IemMainPageEntity.IemOthers.Diagnosis_conditions2;
            pointX = pointX + TextRenderer.MeasureText("2.入院与出院", m_DefaultValueFont).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, dia2, lineHeight) + 20;

            g.DrawString("3.术前与术后", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            string dia3 = m_IemMainPageEntity.IemOthers.Diagnosis_conditions3;
            pointX = pointX + TextRenderer.MeasureText("3.术前与术后", m_DefaultValueFont).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, dia3, lineHeight) + 20;

            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;
            pointX = pointX + TextRenderer.MeasureText("诊断符合情况：", m_DefaultValueFont).Width - 7;

            g.DrawString("4.临床与病理", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            string dia4 = m_IemMainPageEntity.IemOthers.Diagnosis_conditions4;
            pointX = pointX + TextRenderer.MeasureText("4.临床与病理", m_DefaultValueFont).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, dia4, lineHeight) + 20;

            g.DrawString("5.放射与病理", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            string dia5 = m_IemMainPageEntity.IemOthers.Diagnosis_conditions5;
            pointX = pointX + TextRenderer.MeasureText("5.放射与病理", m_DefaultValueFont).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, dia5, lineHeight) + 20;

            pointX = DrawSelectItem(g, pointX, pointY, "（0.未做  1.符合  2.不符合  3.不确定）");

            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            //g.DrawString("抢救情况：抢救", font, Brushes.Black, new PointF(pointX, pointY));
            g.DrawString("抢救情况：抢救", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("抢救情况：抢救", m_DefaultValueFont).Width + 5;
            string EMERGENCY_TIMES = m_IemMainPageEntity.IemOthers.Emergency_times;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", EMERGENCY_TIMES, 40, "");
            g.DrawString("次", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + 50;

            g.DrawString("成功", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("成功", m_DefaultValueFont).Width + 5;
            string EMERGENCY_SUCCESSFUL_TIMES = m_IemMainPageEntity.IemOthers.Emergency_Successful_times;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", EMERGENCY_SUCCESSFUL_TIMES, 40, "");
            g.DrawString("次", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointY += rowHeight;
            pointX = m_PointXPayType + offsetX;

            string cpstatus = m_IemMainPageEntity.IemOthers.CP_status;
            g.DrawString("临床路径管理：", m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY));
            pointX = pointX + TextRenderer.MeasureText("临床路径管理：", m_DefaultValueFont).Width + 5;
            pointX = DrawCheckBox(g, pointX, pointY, cpstatus, lineHeight) + 20;

            pointX = DrawSelectItem(g, pointX, pointY, "1.完成  2.变异  3.不退出  4.未入");

            pointX = m_PointXPayType + offsetX;
            pointY = pointY + 22;
            g.DrawLine(solidPen, new PointF(m_PointXPayType-3, pointY), new PointF(m_PointXPayType + lineWidth, pointY));



            #endregion
            return pointY;
        }


        private float PrintNote(Graphics g, float pointY)
        {
            Font font = m_DefaultValueFont;
            int lineHeight = TextRenderer.MeasureText("高", font).Height;
            int charWidth = TextRenderer.MeasureText("宽", font).Height;
            float rowHeight = 25; //行间距
            Pen solidPen = new Pen(Brushes.Black, 2);
            int lineWidth = 760;
            int offsetX = 12;
            float pointX = m_PointXPayType + offsetX;

            g.DrawString("说明:(一)医疗付费方式 1.城镇职工基本医疗保险 2.城镇居民基本医疗保险 3.新型农村合作医疗 4.贫困救助", font, Brushes.Black, new PointF(pointX, pointY));
            pointY += 20;
            g.DrawString("5.商业医疗保险 6.全公费 7.全自费 8.其他社会保险 9.其他", font, Brushes.Black, new PointF(pointX, pointY));
            pointY += 20;

            int width = TextRenderer.MeasureText("说明:", font).Width;

            g.DrawString("(二)凡可由医院信息系统提供住院费用清单的，住院病案首页中可不填写“住院费用”。", font, Brushes.Black, new PointF(pointX + width - 8, pointY));



            return pointY;
        }

        /// <summary>
        /// 将List拼接成SQL中in条件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-17</date>
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string GetSqlConditionStrByList(List<string> list)
        {
            string condition = string.Empty;
            if (null != list && list.Count() > 0)
            {
                foreach (string str in list)
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        condition += "'" + str + "',";
                    }
                }
                if (!string.IsNullOrEmpty(condition))
                {
                    condition = condition.Substring(0, condition.Length - 1);
                }
            }
            return condition;
        }

        #endregion

        #region 【绘制界面元素】

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
            string name, string value, int underLineWidth, string endName)
        {
            g.DrawString(name, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            int widthName = TextRenderer.MeasureText(name, m_DefaultFont).Width;
            int widthValue = underLineWidth;
            pointX = pointX + widthName;

            int valueLength = TextRenderer.MeasureText(value, m_DefaultValueFont).Width;
            if (valueLength >= underLineWidth)
            {
                if (TextRenderer.MeasureText(value, m_SmallFont1).Width >= underLineWidth)
                {
                    g.DrawString(value, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY - 10, widthValue, lineHeight + 16), sfVertical);
                }
                else
                {
                    g.DrawString(value, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY, widthValue, lineHeight + 4), sf);
                }
            }
            else
            {
                g.DrawString(value, m_DefaultValueFont, Brushes.Black, new RectangleF(pointX, pointY, widthValue, lineHeight + 4), sf);
            }
            g.DrawLine(pen, new PointF(pointX, pointY + lineHeight), new PointF(pointX + widthValue, pointY + lineHeight));

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
            Rectangle rect = new Rectangle((int)pointX, (int)pointY + 1, lineHeight - 2, lineHeight - 2);
            g.DrawRectangle(Pens.Black, rect);
            RectangleF rectF = new RectangleF(pointX - 0.5f, pointY + 1 - 0.5f, lineHeight, lineHeight + 0.5f);
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
            //g.DrawString(allSelectItem, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            g.DrawString(allSelectItem, m_DefaultValueFont, Brushes.Black, new PointF(pointX, pointY)); //王冀 2012 12 10 修改画笔
            return pointX + TextRenderer.MeasureText(allSelectItem, m_DefaultValueFont).Width;
        }

        /// <summary>
        /// 绘制单元格中的字符串，如果指定的宽度不够则缩小字体，如果缩小后还不够则换行显示，对于超过2行的数据则不显示
        /// </summary>
        /// <param name="g"></param>
        /// <param name="value"></param>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="font"></param>
        private void DrawStringInCell(Graphics g, string value, float pointX, float pointY, int width, int height, Font font)
        {
            int textWidth = TextRenderer.MeasureText(value, font).Width;
            if (textWidth < width)//显示一行
            {
                g.DrawString(value, font, Brushes.Black, new RectangleF(pointX, pointY, width, height), sfVertical);
            }
            else//一行显示有可能不够
            {
                if (TextRenderer.MeasureText(value, m_SmallFont1).Width >= width)
                {
                    g.DrawString(value, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY, width, height), sfTop);
                }
                else
                {
                    g.DrawString(value, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY, width, height), sfVertical);
                }
            }
        }

        /// <summary>
        /// 绘制单元格中的字符串，如果指定的宽度不够则缩小字体，如果缩小后还不够则换行显示，对于超过2行的数据则不显示
        /// </summary>
        /// <param name="g"></param>
        /// <param name="value"></param>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="font"></param>
        private void DrawStringInCell2(Graphics g, string value, float pointX, float pointY, int width, int height, Font font)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            width += 4;

            int textWidth = TextRenderer.MeasureText(value, font).Width;
            if (textWidth < width)//显示一行
            {
                g.DrawString(value, font, Brushes.Black, new RectangleF(pointX, pointY, width, height), sf);
            }
            else//一行显示有可能不够
            {
                if (TextRenderer.MeasureText(value, m_SmallFont1).Width >= width)
                {
                    g.DrawString(value, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY, width, height), sfTop);
                }
                else
                {
                    g.DrawString(value, m_SmallFont1, Brushes.Black, new RectangleF(pointX, pointY, width, height), sf);
                }
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// 获取固定编码员名称(配置)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-23</date>
        /// </summary>
        public string GetFixedEncoderName()
        {
            try
            {
                string encoder = string.Empty;
                string encoderConfig = dHelper.GetConfigValueByKey("FixedEncoder");
                if (!string.IsNullOrEmpty(encoderConfig.Trim()))
                {
                    string encoderID = string.Empty;
                    if (encoderConfig.Contains(","))
                    {
                        string[] str = encoderConfig.Split(',');
                        encoderID = str[0];
                    }
                    else
                    {
                        encoderID = encoderConfig;
                    }
                    //员工工号不满6位，则不足6位
                    if (!string.IsNullOrEmpty(encoderID.Trim()) && encoderID.Length < 6)
                    {
                        int length = encoderID.Length;
                        for (int i = 0; i < 6 - length; i++)
                        {
                            encoderID = "0" + encoderID;
                        }
                    }
                    //根据员工工号获取员工姓名
                    encoder = dHelper.GetUserNameByID(encoderID);
                }
                return encoder;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
