using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.IEMMainPage
{
    public class DrawMainPageEnUtil
    {
        #region Property && Field

        PrintDocument m_PrintDocument;

        public int m_PageWidth = 800;//用于设定PictureBox的宽度
        public int m_PageHeight = 1120;//用于设定PictureBox的高度

        float m_PointYTitle = 40; //“首页”标题Y轴方向的值
        Font m_DefaultFont = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_DefaultValueFont = new Font("宋体", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_SmallFont = new Font("宋体", 10f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_SmallFont1 = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Pixel);

        float m_PointXPayType = 15; //“医疗付款方式”左上角点的X轴方向的值
        float m_PointYPayType = 120; //“医疗付款方式”左上角点的Y轴方向的值

        StringFormat sf = new StringFormat();
        StringFormat sfVertical = new StringFormat();
        StringFormat sfTop = new StringFormat();

        private string m_OrganizationCode = string.Empty;//“组织机构代码”
        float m_PointXOrganizationCode = 550; //“组织机构代码”在右角点的X轴方向的值
        float m_PointYOrganizationCode = 40; //“组织机构代码”在右上角的Y轴方向的值

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

        #endregion

        public DrawMainPageEnUtil(IemMainPageInfo iemMainPageEntity)
        {
            try
            {
                m_IemMainPageEntity = iemMainPageEntity;
                InitVariable();
                InitMetaFile();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 初始化变量
        /// </summary>
        private void InitVariable()
        {
            try
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sfVertical.LineAlignment = StringAlignment.Center;
                sfTop.LineAlignment = StringAlignment.Near;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void InitMetaFile()
        {
            try
            {
                string folder = CreateFolder();

                string cansee = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                if (!string.IsNullOrEmpty(cansee))
                {
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(cansee);
                    m_OrganizationCode = doc1.GetElementsByTagName("ShowOrganizationCode")[0] == null ? "" : doc1.GetElementsByTagName("ShowOrganizationCode")[0].InnerText;//病案首页显示组织机构代码 2012/10/22 by tj
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

                Bitmap bmp2 = new Bitmap(m_PageWidth, m_PageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
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
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// 绘制组织机构代码 2013/4/18 by xll
        /// </summary>
        /// <param name="g"></param>
        private void DrawOrganaqzaitionCode(Graphics g)
        {
            if (string.IsNullOrEmpty(m_OrganizationCode)) return;
            Font font1 = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
            Font font2 = new Font("宋体", 20f, FontStyle.Regular, GraphicsUnit.Pixel);
            g.DrawString("组织机构代码:", font1, Brushes.Black, new PointF(m_PointXOrganizationCode, m_PointYOrganizationCode));
            g.DrawString(m_OrganizationCode, font2, Brushes.Black, new PointF(m_PointXOrganizationCode + 90, m_PointYOrganizationCode - 4));
        }

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
                if (!Directory.Exists(folder + "PrintImage\\"))
                {
                    Directory.CreateDirectory(folder + "PrintImage\\");
                }
                DeleteMetaFileAll();
                return folder + "PrintImage\\";
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void DeleteMetaFileAll()
        {
            try
            {
                //删除原先保存在C盘的文件
                DeleteMetaFileAllInner(@"C:\");

                //删除打印需要的矢量文件
                DeleteMetaFileAllInner(AppDomain.CurrentDomain.BaseDirectory + "PrintImage\\");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void DeleteMetaFileAllInner(string folder)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(folder);
                foreach (FileInfo fi in dirInfo.GetFiles("*.wmf"))
                {
                    fi.Delete();
                }
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// 删除使用后的矢量文件
        /// </summary>
        private void DeleteMetaFile()
        {
            try
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
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #region ******************************************************* 绘制病案首页 **************************************************************

        #region 【打印第一页】

        void PrintFirstPage(Graphics g)
        {
            try
            {
                float pointY;
                DrawTitle(g);
                DrawOrganaqzaitionCode(g);
                pointY = PrintPayType(g);
                pointY = PrintPatientBaseInfo(g, pointY + 15);
                pointY = PrintOutHospitalDiaglosis(g, pointY + 18);
                pointY = PrintFristPageOther(g, pointY);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #region 绘制标题
        /// <summary>
        /// 绘制标题
        /// </summary>
        /// <param name="g"></param>
        private void DrawTitle(Graphics g)
        {
            try
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                Font font1 = new Font("宋体", 20f, FontStyle.Regular, GraphicsUnit.Pixel);
                Font font2 = new Font("宋体", 25f, FontStyle.Bold, GraphicsUnit.Pixel);

                g.DrawString("医疗机构：" + m_IemMainPageEntity.IemBasicInfo.HospitalName, font1, Brushes.Black, new RectangleF(0f, m_PointYTitle, 700, 30), sf);
                g.DrawString("住 院 病 案 首 页", font2, Brushes.Black, new RectangleF(0f, m_PointYTitle + 30, m_PageWidth, 50), sf);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region 绘制医疗付款方式一行
        /// <summary>
        /// 绘制医疗付款方式一行
        /// </summary>
        /// <param name="g"></param>
        private float PrintPayType(Graphics g)
        {
            try
            {
                Font font = m_DefaultFont;
                Font font1 = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
                int lineHeight = TextRenderer.MeasureText("高", font).Height;

                float pointX = m_PointXPayType;



                //健康卡号
                string healthCarCode = m_IemMainPageEntity.IemBasicInfo.CardNumber;//todo
                g.DrawString("健康卡号:" + healthCarCode, font1, Brushes.Black, new PointF(m_PointXPayType, m_PointYPayType));

                //入院次数
                string inTime = m_IemMainPageEntity.IemBasicInfo.InCount;//todo
                g.DrawString("第 " + inTime + " 次住院", font1, Brushes.Black, new PointF(230, m_PointYPayType));

                //病案号
                string recordNo = string.Empty;
                if (m_IemMainPageEntity.IemBasicInfo.IsBaby == "1")//是婴儿
                {
                    recordNo = m_IemMainPageEntity.IemBasicInfo.MotherPatOfHis;
                }
                else
                {
                    recordNo = m_IemMainPageEntity.IemBasicInfo.NOOFRECORD;
                }
                g.DrawString("病案号:" + recordNo, font1, Brushes.Black, new PointF(420, m_PointYPayType));


                //医疗付款方式
                g.DrawString("医疗付费方式:", font1, Brushes.Black, new PointF(630, m_PointYPayType));
                string payType = m_IemMainPageEntity.IemBasicInfo.PayID; //todo
                //pointX = 500 + TextRenderer.MeasureText("医疗付款方式:", font1).Width;
                pointX = DrawCheckBox(g, 630 + TextRenderer.MeasureText("医疗付款方式:", font1).Width, m_PointYPayType, payType, lineHeight);
                pointX = DrawSelectItem(g, 630 + TextRenderer.MeasureText("医疗付款方式:", font1).Width, m_PointYPayType, "") + 20;


                Pen pen = new Pen(Brushes.Black, 2);
                g.DrawLine(pen, new PointF(m_PointXPayType, m_PointYPayType + 20), new PointF(m_PointXPayType + 770, m_PointYPayType + 20));

                return m_PointYPayType + 21;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 绘制病人基本信息
        /// <summary>
        /// 绘制病人基本信息
        /// </summary>
        /// <param name="g"></param>
        private float PrintPatientBaseInfo(Graphics g, float pointY)
        {
            try
            {
                Font font = m_DefaultFont;
                int lineHeight = TextRenderer.MeasureText("高", font).Height;
                int charWidth = TextRenderer.MeasureText("宽", font).Height;
                float interval = 28; //行间距

                float pointStartX = m_PointXPayType + 12;
                float pointX = pointStartX;

                #region 1
                //姓名
                string patientName = m_IemMainPageEntity.IemBasicInfo.Name;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "姓名", patientName, 90, "") + 25;

                //性别
                g.DrawString("性别", font, Brushes.Black, new PointF(pointX, pointY));
                string gender = m_IemMainPageEntity.IemBasicInfo.SexID; //todo
                pointX = pointX + TextRenderer.MeasureText("年龄", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, gender, lineHeight);
                pointX = DrawSelectItem(g, pointX, pointY, "1.男  2.女") + 20;

                //出生
                string birth = m_IemMainPageEntity.IemBasicInfo.BirthPrint;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生日期", birth, 120, "") + 25;

                //年龄
                string age = m_IemMainPageEntity.IemBasicInfo.Age;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "年龄", age, 65, "") + 25;

                //国籍
                string nationality = m_IemMainPageEntity.IemBasicInfo.NationalityName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "国籍", nationality, 65, "") + 15;

                #endregion

                #region 2
                pointY += interval;
                pointX = pointStartX;

                //（年龄不足1周岁的） 年龄
                string babyMonth = m_IemMainPageEntity.IemBasicInfo.MonthAge;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（年龄不足1周岁的） 年龄", babyMonth, 60, "月") + 50;

                //新生儿出生体重
                string babyBirthWeight = m_IemMainPageEntity.IemBasicInfo.Weight;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "新生儿出生体重", babyBirthWeight, 60, "克") + 50;

                //新生儿入院体重
                string babyInHospitalWeight = m_IemMainPageEntity.IemBasicInfo.InWeight;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "新生儿入院体重", babyInHospitalWeight, 60, "克") + 30;
                #endregion

                #region 3
                pointY += interval;
                pointX = pointStartX;

                //出生地
                string birthPlaceProvince = m_IemMainPageEntity.IemBasicInfo.CSD_ProvinceName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生地", birthPlaceProvince, 250, "");
                //string birthPlaceProvince = m_IemMainPageEntity.IemBasicInfo.CSD_ProvinceName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生地", birthPlaceProvince, 65, "省(区、市)");
                //string birthPlaceCity = m_IemMainPageEntity.IemBasicInfo.CSD_CityName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", birthPlaceCity, 65, "市");
                //string birthPlaceCounty = m_IemMainPageEntity.IemBasicInfo.CSD_DistrictName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", birthPlaceCounty, 65, "县") + 10;

                //籍贯
                string jiGuanProvince = m_IemMainPageEntity.IemBasicInfo.JG_ProvinceName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "籍贯", jiGuanProvince, 250, "");
                //string jiGuanProvince = m_IemMainPageEntity.IemBasicInfo.JG_ProvinceName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "籍贯", jiGuanProvince, 65, "省(区、市)");
                //string jiGuanCity = m_IemMainPageEntity.IemBasicInfo.JG_CityName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", jiGuanCity, 65, "市") + 15;
                //民族
                string mingZu = m_IemMainPageEntity.IemBasicInfo.NationName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "民族", mingZu, 65, "") + 10;
                #endregion

                #region 4
                pointY += interval;
                pointX = pointStartX;

                //身份证号
                string patientID = m_IemMainPageEntity.IemBasicInfo.IDNO;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "身份证号", patientID, 200, "") + 10;

                //职业
                string jobName = m_IemMainPageEntity.IemBasicInfo.JobName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "职业", jobName, 120, "") + 10;

                //婚姻
                g.DrawString("婚姻", font, Brushes.Black, new PointF(pointX, pointY));
                string marriage = m_IemMainPageEntity.IemBasicInfo.Marital; //todo
                pointX = pointX + TextRenderer.MeasureText("婚姻", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, marriage, lineHeight);
                //pointX = DrawSelectItem(g, pointX, pointY, "1.未婚 2.已婚 3.丧偶 4.离婚 9.其他") + 10;
                pointX = DrawSelectItem(g, pointX, pointY, "1.未婚 2.已婚 3.丧偶 4.离婚 9.其他") + 10;

                #endregion

                #region 5
                pointY += interval;
                pointX = pointStartX;

                //现住址
                #region 0604修改
                string addressProvince = m_IemMainPageEntity.IemBasicInfo.XZZ_ProvinceName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "现在住址", addressProvince, 270, "");
                //string addressProvince = m_IemMainPageEntity.IemBasicInfo.XZZ_ProvinceName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "现在住址", addressProvince, 70, "省(区、市)");
                //string addressCity = m_IemMainPageEntity.IemBasicInfo.XZZ_CityName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", addressCity, 70, "市");
                //string addressCounty = m_IemMainPageEntity.IemBasicInfo.XZZ_DistrictName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", addressCounty, 70, "县") + 18;
                #endregion
                //电话
                string addressTelNumber = m_IemMainPageEntity.IemBasicInfo.XZZ_TEL;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "电话", addressTelNumber, 120, "") + 28;

                //编码
                string addressPostCode = m_IemMainPageEntity.IemBasicInfo.XZZ_Post;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "邮编", addressPostCode, 120, "") + 10;

                #endregion

                #region 6
                pointY += interval;
                pointX = pointStartX;

                //户口地址
                #region 0604修改
                string hukouProvince = m_IemMainPageEntity.IemBasicInfo.HKDZ_ProvinceName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "户口地址", hukouProvince, 270, "");
                //string hukouProvince = m_IemMainPageEntity.IemBasicInfo.HKDZ_ProvinceName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "户口地址", hukouProvince, 70, "省(区、市)");
                //string hukouCity = m_IemMainPageEntity.IemBasicInfo.HKDZ_CityName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", hukouCity, 70, "市");
                //string hukouCounty = m_IemMainPageEntity.IemBasicInfo.HKDZ_DistrictName;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "", hukouCounty, 70, "县") + 203;
                #endregion
                //邮编
                string hukouPostCode = m_IemMainPageEntity.IemBasicInfo.HKDZ_Post;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "邮编", hukouPostCode, 120, "") + 10;

                #endregion

                #region 7
                pointY += interval;
                pointX = pointStartX;

                //工作单位及地址
                string jobAddress = m_IemMainPageEntity.IemBasicInfo.OfficePlace;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "工作单位及地址", jobAddress, 255, "") + 16;

                //单位电话
                string jobTelNumber = m_IemMainPageEntity.IemBasicInfo.OfficeTEL;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "单位电话", jobTelNumber, 120, "") + 28;

                //邮编
                string jobPostCode = m_IemMainPageEntity.IemBasicInfo.OfficePost;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "邮编", jobPostCode, 120, "") + 10;

                #endregion

                #region 8
                pointY += interval;
                pointX = pointStartX;

                //联系人姓名
                string contactName = m_IemMainPageEntity.IemBasicInfo.ContactPerson;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "联系人姓名", contactName, 80, "") + 15;

                //关系
                string relation = m_IemMainPageEntity.IemBasicInfo.RelationshipName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "关系", relation, 80, "") + 15;

                //地址
                string contactAddress = m_IemMainPageEntity.IemBasicInfo.ContactAddress;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "地址", contactAddress, 233, "") + 15;

                //电话
                string contactTel = m_IemMainPageEntity.IemBasicInfo.ContactTEL;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "电话", contactTel, 120, "") + 10;

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
                #endregion

                #region 10
                pointY += interval;
                pointX = pointStartX;

                //入院时间
                string inTime = m_IemMainPageEntity.IemBasicInfo.AdmitDatePrint;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院时间", inTime, 150, "") + 20;

                //入院科别
                string inSection = m_IemMainPageEntity.IemBasicInfo.AdmitDeptName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院科别", inSection, 115, "") + 20;

                //病区
                string inNurseWard = m_IemMainPageEntity.IemBasicInfo.AdmitWardName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病房", inNurseWard, 71, "") + 20;

                //转科科别
                string shiftSection = m_IemMainPageEntity.IemBasicInfo.Trans_AdmitDeptName;//todo
                if (shiftSection.Trim() == "")
                {
                    shiftSection = "无";
                }
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "转科科别", shiftSection, 120, "") + 10;

                #endregion

                #region 11
                pointY += interval;
                pointX = pointStartX;

                //出院日期
                string outTime = m_IemMainPageEntity.IemBasicInfo.OutWardDatePrint;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出院日期", outTime, 150, "") + 20;

                //出院科别
                string outSection = m_IemMainPageEntity.IemBasicInfo.OutHosDeptName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出院科别", outSection, 115, "") + 20;

                //病区
                string outNurseWard = m_IemMainPageEntity.IemBasicInfo.OutHosWardName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "病房", outNurseWard, 71, "") + 20;

                //实际住院天数
                string InDay = m_IemMainPageEntity.IemBasicInfo.ActualDays;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "实际住院", InDay, 100, "天") + 10;

                #endregion

                #region 12
                pointY += interval;
                pointX = pointStartX;

                //门（急）诊诊断（西医诊断）
                string menJiZhenDiaglosisWest = m_IemMainPageEntity.IemBasicInfo.MZXYZD_NAME;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "门（急）诊诊断", menJiZhenDiaglosisWest, 200, "") + 25;

                //疾病编码
                string deseaseCodeWest = m_IemMainPageEntity.IemBasicInfo.MZXYZD_CODE;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "疾病编码", deseaseCodeWest, 120, "") + 10;

                ////入院时情况
                //g.DrawString("入院时情况", font, Brushes.Black, new PointF(pointX, pointY));
                //string inState = "8"; //todo
                //pointX = pointX + TextRenderer.MeasureText("入院时情况", font).Width + 5;
                //pointX = DrawCheckBox(g, pointX, pointY, inState, lineHeight);
                //pointX = DrawSelectItem(g, pointX, pointY, "1.危  2.急  3.一般") + 10;
                #endregion




                return pointY + 2;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 出院诊断
        /// <summary>
        /// 出院诊断
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintOutHospitalDiaglosis(Graphics g, float pointY)
        {
            try
            {
                //表格的行高
                float lineHeight = 30f;
                float pointX = m_PointXPayType;
                float firstColumnWidth = 240f;//第一列宽度
                float secondColumnWidth = 100f;//第二列宽度
                float thirdColumnWidth = 45f;//第三列宽度

                float firstLineHeight = 34f;

                Font font = m_SmallFont;
                float offsetX = 12f;

                #region TableHead

                //第一列
                g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)firstColumnWidth, (int)firstLineHeight)));
                g.DrawLine(Pens.White, new Point((int)pointX, (int)pointY), new Point((int)pointX, (int)pointY + (int)firstLineHeight));
                g.DrawString("出院诊断", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, firstColumnWidth, firstLineHeight), sf);

                pointX += firstColumnWidth;

                //第二列
                g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)secondColumnWidth, (int)firstLineHeight)));
                g.DrawString("疾病编码", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, secondColumnWidth, firstLineHeight), sf);

                pointX += secondColumnWidth;

                //第三列
                g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)thirdColumnWidth, (int)firstLineHeight)));
                g.DrawString("入院病情", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY + 2, thirdColumnWidth, firstLineHeight), sf);

                pointX += thirdColumnWidth;

                //第四列
                g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)firstColumnWidth, (int)firstLineHeight)));
                g.DrawString("出院诊断", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, firstColumnWidth, firstLineHeight), sf);

                pointX += firstColumnWidth;

                //第五列
                g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)secondColumnWidth, (int)firstLineHeight)));
                g.DrawString("疾病编码", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, secondColumnWidth, firstLineHeight), sf);

                pointX += secondColumnWidth;

                //第六列
                g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointY), new Size((int)thirdColumnWidth, (int)firstLineHeight)));
                g.DrawLine(Pens.White, new Point((int)pointX + (int)thirdColumnWidth, (int)pointY), new Point((int)pointX + (int)thirdColumnWidth, (int)pointY + (int)firstLineHeight));
                g.DrawString("入院病情", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY + 2, thirdColumnWidth, firstLineHeight), sf);

                pointX = m_PointXPayType;
                pointY = pointY + firstLineHeight;
                #endregion

                #region TableBody

                float pointYTableBodyStart = pointY;
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;

                //西医出院诊断
                DataTable outDiagTableWest = m_IemMainPageEntity.IemDiagInfo.OutDiagTable.Copy();
                //中医出院诊断
                //’DataTable outDiagTableChinese = m_IemMainPageEntity.IemDiagInfo.OutDiagTable.Copy();

                for (int i = outDiagTableWest.Rows.Count - 1; i >= 0; i--)
                {
                    if (outDiagTableWest.Rows[i]["Type"].ToString() != "1")
                    {
                        outDiagTableWest.Rows.RemoveAt(i);
                    }
                }
                //for (int i = outDiagTableChinese.Rows.Count - 1; i >= 0; i--)
                //{
                //    if (outDiagTableChinese.Rows[i]["Type"].ToString() != "2")
                //    {
                //        outDiagTableChinese.Rows.RemoveAt(i);
                //    }
                //}
                outDiagTableWest.AcceptChanges();
                //outDiagTableChinese.AcceptChanges();

                //左侧诊断
                for (int i = 0; i < 11; i++)
                {
                    if (i == 0)
                    {
                        //【出院诊断 主要诊断】
                        g.DrawString("主要诊断：", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart + 2, firstColumnWidth, lineHeight), sf);
                        //主要诊断value
                        if (outDiagTableWest.Rows.Count > i)//todo
                        {
                            int stringWidth = TextRenderer.MeasureText("主要诊断：", m_DefaultFont).Width;
                            DrawStringInCell(g, outDiagTableWest.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                                pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
                        }
                    }
                    if (i == 1)
                    {
                        //【出院诊断 其他诊断】
                        g.DrawString("其他诊断：", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart + 2, firstColumnWidth, lineHeight), sf);
                        //主要诊断value
                        if (outDiagTableWest.Rows.Count > i)//todo
                        {
                            int stringWidth = TextRenderer.MeasureText("其他诊断：", m_DefaultFont).Width;
                            DrawStringInCell(g, outDiagTableWest.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                                pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
                        }
                    }
                    //左列剩余其他诊断value
                    if (i > 1 && outDiagTableWest.Rows.Count > i)//todo
                    {
                        DrawStringInCell(g, outDiagTableWest.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX,
                            pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultFont);
                    }
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)firstColumnWidth, (int)lineHeight)));
                    g.DrawLine(Pens.White, new Point((int)pointX, (int)pointYTableBodyStart), new Point((int)pointX, (int)pointYTableBodyStart + (int)lineHeight));
                    pointX += firstColumnWidth;


                    //疾病编码
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)secondColumnWidth, (int)lineHeight)));
                    //疾病编码value
                    if (outDiagTableWest.Rows.Count > i)
                        DrawStringInCell2(g, outDiagTableWest.Rows[i]["Diagnosis_Code"].ToString(), pointX,
                            pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultFont);
                    pointX += secondColumnWidth;


                    //入院病情
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)thirdColumnWidth, (int)lineHeight)));
                    //入院病情value
                    if (outDiagTableWest.Rows.Count > i)
                        DrawStringInCell2(g, outDiagTableWest.Rows[i]["Status_Id"].ToString(), pointX,
                            pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultFont);


                    pointX = m_PointXPayType;
                    pointYTableBodyStart += lineHeight;
                }

                //【入院病情说明】
                g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart),
                    new Size((int)firstColumnWidth + (int)secondColumnWidth + (int)thirdColumnWidth, (int)lineHeight)));
                g.DrawLine(Pens.White, new Point((int)pointX, (int)pointYTableBodyStart), new Point((int)pointX, (int)pointYTableBodyStart + (int)lineHeight));
                g.DrawString("入院病情: 1.有  2.临床未确定  3.情况不明  4.无 ", m_DefaultFont, Brushes.Black,
                    new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth + secondColumnWidth + thirdColumnWidth, lineHeight), sf);

                pointX = m_PointXPayType + firstColumnWidth + secondColumnWidth + thirdColumnWidth;
                pointYTableBodyStart = pointY;
                //右侧诊断
                for (int i = 11; i < 23; i++)
                {
                    if (i == 11)
                    {
                        //【出院诊断 其他诊断】
                        g.DrawString("其他诊断：", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart + 2, firstColumnWidth, lineHeight), sf);
                        //主要诊断value
                        if (outDiagTableWest.Rows.Count > i)//todo
                        {
                            int stringWidth = TextRenderer.MeasureText("其他诊断：", m_DefaultFont).Width;
                            DrawStringInCell(g, outDiagTableWest.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                                pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
                        }
                    }
                    //右侧剩余其他诊断value
                    if (i > 11 && outDiagTableWest.Rows.Count > i)//todo
                    {
                        DrawStringInCell(g, outDiagTableWest.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX,
                            pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultFont);
                    }
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)firstColumnWidth, (int)lineHeight)));
                    pointX += firstColumnWidth;


                    //疾病编码
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)secondColumnWidth, (int)lineHeight)));
                    //疾病编码value
                    if (outDiagTableWest.Rows.Count > i)
                        DrawStringInCell2(g, outDiagTableWest.Rows[i]["Diagnosis_Code"].ToString(), pointX,
                            pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultFont);
                    pointX += secondColumnWidth;


                    //入院病情
                    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)thirdColumnWidth, (int)lineHeight)));
                    //入院病情value
                    if (outDiagTableWest.Rows.Count > i)
                        DrawStringInCell2(g, outDiagTableWest.Rows[i]["Status_Id"].ToString(), pointX,
                            pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultFont);

                    g.DrawLine(Pens.White, new Point((int)pointX + (int)thirdColumnWidth, (int)pointYTableBodyStart), new Point((int)pointX + (int)thirdColumnWidth, (int)pointYTableBodyStart + (int)lineHeight));
                    pointX = m_PointXPayType + firstColumnWidth + secondColumnWidth + thirdColumnWidth;
                    pointYTableBodyStart += lineHeight;

                }
                ////【主证】
                //for (int i = 0; i <= 8; i++)
                //{
                //    //【主证 1-8】
                //    if (i <= 7)
                //    {
                //        if (i == 0)
                //        {
                //            pointX = m_PointXPayType;
                //            pointYTableBodyStart = pointYTableBodyStart + lineHeight;
                //            g.DrawString("其他诊断：", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth, lineHeight), sf);

                //            //主证
                //            if (outDiagTableChinese.Rows.Count > 1)
                //            {
                //                int stringWidth = TextRenderer.MeasureText("主证:", m_DefaultFont).Width;
                //                DrawStringInCell(g, outDiagTableChinese.Rows[1]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                //                    pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
                //            }
                //        }

                //        //其他主证
                //        if (i > 0 && outDiagTableChinese.Rows.Count > i + 1)
                //        {
                //            DrawStringInCell(g, outDiagTableChinese.Rows[i + 1]["Diagnosis_Name"].ToString(), pointX + offsetX,
                //                pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultFont);
                //        }

                //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)firstColumnWidth, (int)lineHeight)));
                //        g.DrawLine(Pens.White, new Point((int)pointX, (int)pointYTableBodyStart), new Point((int)pointX, (int)pointYTableBodyStart + (int)lineHeight));

                //        pointX += firstColumnWidth;
                //        //疾病编码
                //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)secondColumnWidth, (int)lineHeight)));
                //        //疾病编码value
                //        if (outDiagTableChinese.Rows.Count > i + 1)//todo
                //        {
                //            DrawStringInCell2(g, outDiagTableChinese.Rows[i + 1]["Diagnosis_Code"].ToString(), pointX,
                //                pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultFont);
                //        }

                //        pointX += secondColumnWidth;
                //        //入院病情
                //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)thirdColumnWidth, (int)lineHeight)));
                //        //入院病情value
                //        if (outDiagTableChinese.Rows.Count > i + 1)//todo
                //        {
                //            DrawStringInCell2(g, outDiagTableChinese.Rows[i + 1]["Status_Id"].ToString(), pointX,
                //                pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultFont);
                //        }

                //        pointX = m_PointXPayType;
                //        pointYTableBodyStart += lineHeight;
                //    }
                //    else if (i == 8)
                //    {
                //        //【入院病情说明】
                //        g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart),
                //            new Size((int)firstColumnWidth + (int)secondColumnWidth + (int)thirdColumnWidth, (int)lineHeight)));
                //        g.DrawLine(Pens.White, new Point((int)pointX, (int)pointYTableBodyStart), new Point((int)pointX, (int)pointYTableBodyStart + (int)lineHeight));
                //        g.DrawString("入院病情: 1.有  2.临床未确定  3.情况不明  4.无 ", m_DefaultFont, Brushes.Black,
                //            new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth + secondColumnWidth + thirdColumnWidth, lineHeight), sf);
                //    }
                //}

                //出院西医诊断
                //for (int i = 0; i <= 9; i++)
                //{
                //    if (i == 0)
                //    {
                //        pointX = m_PointXPayType + firstColumnWidth + secondColumnWidth + thirdColumnWidth;
                //        pointYTableBodyStart = pointY;
                //        g.DrawString("其他诊断:", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth, lineHeight), sf);

                //        //主要诊断value
                //        if (outDiagTableWest.Rows.Count > i)
                //        {
                //            int stringWidth = TextRenderer.MeasureText("其他诊断:", m_DefaultFont).Width;
                //            DrawStringInCell(g, outDiagTableWest.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                //                pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
                //        }
                //    }
                //    else if (i == 1)
                //    {
                //        //g.DrawString("其他诊断:", m_DefaultFont, Brushes.Black, new RectangleF(pointX + offsetX, pointYTableBodyStart, firstColumnWidth, lineHeight), sf);

                //        //其他诊断value
                //        if (outDiagTableWest.Rows.Count > i)
                //        {
                //            int stringWidth = TextRenderer.MeasureText("其他诊断:", m_DefaultFont).Width;
                //            DrawStringInCell(g, outDiagTableWest.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX + stringWidth,
                //                pointYTableBodyStart + 2, (int)(firstColumnWidth - (offsetX + stringWidth)), (int)lineHeight, m_DefaultFont);
                //        }
                //    }
                //    //其他诊断value
                //    else if (i > 1 && outDiagTableWest.Rows.Count > i)
                //    {
                //        DrawStringInCell(g, outDiagTableWest.Rows[i]["Diagnosis_Name"].ToString(), pointX + offsetX,
                //            pointYTableBodyStart + 2, (int)(firstColumnWidth - offsetX), (int)lineHeight, m_DefaultFont);
                //    }

                //    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)firstColumnWidth, (int)lineHeight)));

                //    pointX += firstColumnWidth;
                //    //疾病编码
                //    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)secondColumnWidth, (int)lineHeight)));
                //    //疾病编码value
                //    if (outDiagTableWest.Rows.Count > i)//todo
                //    {
                //        DrawStringInCell2(g, outDiagTableWest.Rows[i]["Diagnosis_Code"].ToString(), pointX,
                //            pointYTableBodyStart + 2, (int)secondColumnWidth, (int)lineHeight, m_DefaultFont);
                //    }

                //    pointX += secondColumnWidth;
                //    //入院病情
                //    g.DrawRectangle(Pens.Black, new Rectangle(new Point((int)pointX, (int)pointYTableBodyStart), new Size((int)thirdColumnWidth, (int)lineHeight)));
                //    //入院病情value
                //    if (outDiagTableWest.Rows.Count > i)//todo
                //    {
                //        DrawStringInCell2(g, outDiagTableWest.Rows[i]["Status_Id"].ToString(), pointX,
                //            pointYTableBodyStart + 2, (int)thirdColumnWidth, (int)lineHeight, m_DefaultFont);
                //    }

                //    g.DrawLine(Pens.White, new Point((int)pointX + (int)thirdColumnWidth, (int)pointYTableBodyStart),
                //        new Point((int)pointX + (int)thirdColumnWidth, (int)pointYTableBodyStart + (int)lineHeight));

                //    pointX = m_PointXPayType + firstColumnWidth + secondColumnWidth + thirdColumnWidth;
                //    pointYTableBodyStart += lineHeight;

                //}
                #endregion

                return pointYTableBodyStart;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region  第一页中出院诊断下面的部分
        /// <summary>
        /// 第一页中出院诊断下面的部分
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintFristPageOther(Graphics g, float pointY)
        {
            try
            {
                //行高
                float rowHeight = 26f;
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
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "损伤、中毒的外部原因", outsideReason, 385, "") + 30;

                //疾病编码
                string outsideReasondeseaseCode = m_IemMainPageEntity.IemDiagInfo.Hurt_Toxicosis_ElementID;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "疾病编码", outsideReasondeseaseCode, 120, "");

                pointY += rowHeight;
                pointX = pointStartX;
                g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));
                #endregion

                #region 2
                pointX += xOffset;

                //病理诊断
                string pathologyDiagnisis = m_IemMainPageEntity.IemDiagInfo.Pathology_Diagnosis_Name;//todo
                string path1 = string.Empty;
                string path2 = string.Empty;
                if (pathologyDiagnisis.Length > 36)
                {
                    path1 = pathologyDiagnisis.Substring(0, 36);
                    path2 = pathologyDiagnisis.Substring(36);
                }
                else
                {
                    path1 = pathologyDiagnisis;

                }
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "病理诊断:", path1, 480, "") + 15;

                //疾病编码
                string pathologyDiagnisisDeseaseCode = m_IemMainPageEntity.IemDiagInfo.Pathology_Diagnosis_ID;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "疾病编码", pathologyDiagnisisDeseaseCode, 120, "");

                pointY += rowHeight;
                pointX = pointStartX;
                //g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, " ", path2, 540, "") + 15;

                string bingliNo = m_IemMainPageEntity.IemDiagInfo.Pathology_Observation_Sn;
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "病理号", bingliNo, 120, "");
                pointY += rowHeight;
                pointX = pointStartX;
                g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

                #endregion

                #region 3
                pointX += xOffset;

                //药物过敏
                g.DrawString("药物过敏", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
                //string drugAllergy = string.IsNullOrEmpty(m_IemMainPageEntity.IemDiagInfo.Allergic_Drug.Trim()) ? "2" : "1"; //todo
                string drugAllergy = m_IemMainPageEntity.IemDiagInfo.Allergic_Flag.Trim(); //by ywk 2012年3月6日14:36:31
                pointX = pointX + TextRenderer.MeasureText("药物过敏", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, drugAllergy, lineHeight);
                pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.无 2.有");

                string drugAllergyName = m_IemMainPageEntity.IemDiagInfo.Allergic_Drug; //todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, " 过敏药物：", drugAllergyName, 291, "") + 30;

                //死亡患者尸检
                g.DrawString("死亡患者尸检", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
                string isCheckDeadBody = m_IemMainPageEntity.IemBasicInfo.Autopsy_Flag; //todo
                pointX = pointX + TextRenderer.MeasureText("死亡患者尸检", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, isCheckDeadBody, lineHeight);
                pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.是  2.否");

                pointY += rowHeight;
                pointX = pointStartX;
                g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

                #endregion

                #region 4
                pointX += xOffset;

                //血型
                g.DrawString("血型", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
                string blood = m_IemMainPageEntity.IemDiagInfo.BloodType; //todo
                pointX = pointX + TextRenderer.MeasureText("血型", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, blood, lineHeight);
                pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.A  2.B  3.O  4.AB  5.不详  6.未查") + 40;

                //Rh
                g.DrawString("Rh", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
                string bloodRh = m_IemMainPageEntity.IemDiagInfo.Rh; //todo
                pointX = pointX + TextRenderer.MeasureText("Rh", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, bloodRh, lineHeight);
                //pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.A  2.B  3.O  4.AB  5.不详  6.未查");
                pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.阴  2.阳  3.不详  4.未查");

                pointY += rowHeight;
                pointX = pointStartX;
                g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

                #endregion

                #region 5
                pointX += xOffset;

                //科主任
                string deptDirecter = m_IemMainPageEntity.IemDiagInfo.Section_DirectorName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "科主任", deptDirecter, 95, "") + 20;

                //主任（副主任）医师
                string archiater = m_IemMainPageEntity.IemDiagInfo.DirectorName;
                // m_IemMainPageEntity.IemDiagInfo.DirectorName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "主任（副主任）医师", archiater, 95, "") + 20;

                //主治医师
                string attend = m_IemMainPageEntity.IemDiagInfo.Vs_EmployeeName;
                //m_IemMainPageEntity.IemDiagInfo.Vs_EmployeeName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "主治医师", attend, 95, "") + 20;

                //住院医师
                string resident = m_IemMainPageEntity.IemDiagInfo.Resident_EmployeeName;
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "住院医师", resident, 95, "") + 20;

                pointY += rowHeight;
                pointX = pointStartX;

                #endregion

                #region 6

                pointX += xOffset;

                //责任护士
                string dutyNurse = m_IemMainPageEntity.IemDiagInfo.Duty_NurseName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "责任护士", dutyNurse, 95, "") + 43;

                //进修医师
                string furtherStudyDoctor = m_IemMainPageEntity.IemDiagInfo.Refresh_EmployeeName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "进修医师", furtherStudyDoctor, 95, "") + 43;

                //实习医师
                string practiceDoctor = m_IemMainPageEntity.IemDiagInfo.InterneName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "实习医师", practiceDoctor, 95, "") + 44;

                //编码员
                string coder = m_IemMainPageEntity.IemDiagInfo.Coding_UserName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "编码员", coder, 95, "");

                pointY += rowHeight;
                pointX = pointStartX;
                g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

                #endregion

                #region 7

                pointX += xOffset;

                //病案质量
                g.DrawString("病案质量", font, Brushes.Black, new PointF(pointX, pointY + (rowHeight - lineHeight) / 2));
                string medicalRecordQuality = m_IemMainPageEntity.IemDiagInfo.Medical_Quality_Id; //todo
                if (medicalRecordQuality == "")                 // wangji   edit 2013 1 12 去掉占位符
                {
                    medicalRecordQuality = " ";
                }
                pointX = pointX + TextRenderer.MeasureText("病案质量", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY + (rowHeight - lineHeight) / 2, medicalRecordQuality, lineHeight);
                pointX = DrawSelectItem(g, pointX, pointY + (rowHeight - lineHeight) / 2, "1.甲  2.乙  3.丙") + 15;

                //质控医师
                string qualityDoctor = m_IemMainPageEntity.IemDiagInfo.Quality_Control_DoctorName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "质控医师", qualityDoctor, 95, "") + 15;

                //质控护士
                string qualityNurse = m_IemMainPageEntity.IemDiagInfo.Quality_Control_NurseName;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "质控护士", qualityNurse, 95, "") + 15;

                //质控日期
                string qualityDateTime = m_IemMainPageEntity.IemDiagInfo.Quality_Control_DatePrint;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY + (rowHeight - lineHeight) / 2, lineHeight, charWidth, "质控日期", qualityDateTime, 120, "") + 15;

                pointY += rowHeight;
                pointX = pointStartX;
                g.DrawLine(Pens.Black, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));


                #endregion

                pointY += rowHeight;

                //绘制页数
                //g.DrawString("第 1 页", m_DefaultFont, Brushes.Black, new RectangleF(pointX, pointY, lineWidth, rowHeight), sf);

                return pointY;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


        #endregion 【打印第一页】

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
            try
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
            catch (Exception ex)
            {

                throw ex;
            }
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
            try
            {
                Rectangle rect = new Rectangle((int)pointX, (int)pointY + 1, lineHeight - 2, lineHeight - 2);
                g.DrawRectangle(Pens.Black, rect);
                RectangleF rectF = new RectangleF(pointX - 0.5f, pointY + 1 - 0.5f, lineHeight, lineHeight + 0.5f);
                if (string.IsNullOrEmpty(value))
                {
                    g.DrawString("/", m_DefaultFont, Brushes.Black, rectF, sf);
                }
                else
                {
                    g.DrawString(value, m_DefaultFont, Brushes.Black, rectF, sf);
                }

                return pointX + lineHeight + 5;
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
            try
            {
                g.DrawString(allSelectItem, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
                return pointX + TextRenderer.MeasureText(allSelectItem, m_DefaultFont).Width;
            }
            catch (Exception ex)
            {

                throw ex;
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
        private void DrawStringInCell(Graphics g, string value, float pointX, float pointY, int width, int height, Font font)
        {
            try
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
            catch (Exception ex)
            {

                throw ex;
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
            try
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region 【打印第二页】

        void PrintSecondPage(Graphics g)
        {
            try
            {
                float pointY;
                pointY = PrintOperation(g, m_PointYTitle) + 30;
                pointY = PrintSecondPageOther(g, pointY) + 20;
                if (m_IemMainPageEntity.IemObstetricsBaby != null)
                    pointY = FuKeChanKeYingEr(g, pointY) + 20;
                pointY = PrintFee(g, pointY) + 10;
                pointY = PrintNote(g, pointY) + 10;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region 手术
        /// <summary>
        /// 手术
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintOperation(Graphics g, float pointY)
        {
            try
            {
                //表格的行高
                int firstLineHieght = 38;
                int lineHeight = 26;
                float pointX = m_PointXPayType;
                Font font = m_DefaultFont;
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;


                //宽度调整 edit by ywk 2012年4月18日13:32:46
                int columnWidth1 = 75;//手术及操作编码
                int columnWidth2 = 120;//手术及操作日期
                int columnWidth3 = 40;//手术级别
                int columnWidth4 = 130;//手术及操作名称
                int columnWidth5 = 210;//手术及操作医师 术者60 Ⅰ助60 Ⅱ助60
                int columnWidth6 = 60;//切口愈合等级
                int columnWidth7 = 70;//麻醉方式
                int columnWidth8 = 70;//麻醉医师

                int lineWidth = 770; //ColumnWidth1 + ColumnWidth2 + ...... = lineWidth
                int xOffset = 12;

                Pen solidPen = new Pen(Brushes.Black, 2);
                g.DrawLine(solidPen, new PointF(pointX, pointY), new PointF(pointX + lineWidth, pointY));

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

                if (operationTable.Rows.Count > 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        //手术及操作编码
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth1, lineHeight));
                        g.DrawLine(Pens.White, new Point((int)pointX, (int)pointY), new Point((int)pointX, (int)pointY + lineHeight));
                        //手术及操作编码value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth1, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
                        {
                            DrawStringInCell2(g, operationTable.Rows[i]["Operation_Code"].ToString(), pointX, pointY + 2, columnWidth1, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth1;

                        //手术及操作日期
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth2, lineHeight));
                        //手术及操作日期value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth1, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
                        {
                            DrawStringInCell2(g, operationTable.Rows[i]["Operation_Date"].ToString(), pointX, pointY + 2, columnWidth2, lineHeight, m_DefaultValueFont);
                        }


                        pointX += columnWidth2;

                        //手术级别
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth3, lineHeight));
                        //手术级别value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth3, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
                        {
                            DrawStringInCell2(g, operationTable.Rows[i]["Operation_Level"].ToString(), pointX, pointY + 2, columnWidth3, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth3;

                        //手术及操作名称
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth4, lineHeight));
                        //手术及操作名称value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth4, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
                        {
                            DrawStringInCell2(g, operationTable.Rows[i]["Operation_Name"].ToString(), pointX, pointY + 2, columnWidth4, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth4;

                        //手术及操作医师【术者】
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5 / 3, lineHeight));
                        //手术及操作医师【术者】value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
                        {
                            DrawStringInCell2(g, operationTable.Rows[i]["Execute_User1_Name"].ToString(), pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth5 / 3;

                        //手术及操作医师【Ⅰ助】
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5 / 3, lineHeight));
                        //手术及操作医师【Ⅰ助】value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
                        {
                            DrawStringInCell2(g, operationTable.Rows[i]["Execute_User2_Name"].ToString(), pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth5 / 3;

                        //手术及操作医师【Ⅱ助】
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5 / 3, lineHeight));
                        //手术及操作医师【Ⅱ助】value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
                        {
                            DrawStringInCell2(g, operationTable.Rows[i]["Execute_User3_Name"].ToString(), pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth5 / 3;

                        //切口愈合等级
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth6, lineHeight));
                        //切口愈合等级value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth6, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
                        {
                            DrawStringInCell2(g, operationTable.Rows[i]["Close_Level_Name"].ToString(), pointX, pointY + 2, columnWidth6, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth6;

                        //麻醉方式
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth7, lineHeight));
                        //切口愈合等级value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth7, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
                        {
                            DrawStringInCell2(g, operationTable.Rows[i]["Anaesthesia_Type_Name"].ToString(), pointX, pointY + 2, columnWidth7, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth7;

                        //麻醉医师
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth8, lineHeight));
                        //麻醉医师value
                        if (operationTable.Rows.Count <= 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth8, lineHeight, m_DefaultFont);
                        }
                        else if (operationTable.Rows.Count > i)
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

                }//edit zyx 没有数据时第一行画“/”
                else
                {
                    for (int i = 0; i < 8; i++)
                    {
                        //手术及操作编码
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth1, lineHeight));
                        g.DrawLine(Pens.White, new Point((int)pointX, (int)pointY), new Point((int)pointX, (int)pointY + lineHeight));
                        //手术及操作编码value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth1, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth1, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth1;

                        //手术及操作日期
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth2, lineHeight));
                        //手术及操作日期value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth1, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth2, lineHeight, m_DefaultValueFont);
                        }


                        pointX += columnWidth2;

                        //手术级别
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth3, lineHeight));
                        //手术级别value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth3, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth3, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth3;

                        //手术及操作名称
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth4, lineHeight));
                        //手术及操作名称value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth4, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth4, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth4;

                        //手术及操作医师【术者】
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5 / 3, lineHeight));
                        //手术及操作医师【术者】value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth5 / 3;

                        //手术及操作医师【Ⅰ助】
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5 / 3, lineHeight));
                        //手术及操作医师【Ⅰ助】value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth5 / 3;

                        //手术及操作医师【Ⅱ助】
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth5 / 3, lineHeight));
                        //手术及操作医师【Ⅱ助】value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth5 / 3, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth5 / 3;

                        //切口愈合等级
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth6, lineHeight));
                        //切口愈合等级value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth6, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth6, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth6;

                        //麻醉方式
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth7, lineHeight));
                        //切口愈合等级value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth7, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth7, lineHeight, m_DefaultFont);
                        }


                        pointX += columnWidth7;

                        //麻醉医师
                        g.DrawRectangle(Pens.Black, new Rectangle((int)pointX, (int)pointY, columnWidth8, lineHeight));
                        //麻醉医师value
                        if (i == 0)
                        {
                            DrawStringInCell2(g, "/", pointX, pointY + 2, columnWidth8, lineHeight, m_DefaultFont);
                        }
                        else
                        {
                            DrawStringInCell2(g, "", pointX, pointY + 2, columnWidth8, lineHeight, m_DefaultFont);
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
                }

                return pointY;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 打印第二页手术和妇科之间的部分
        /// <summary>
        /// 打印第二页手术和妇科之间的部分
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintSecondPageOther(Graphics g, float pointY)
        {
            try
            {
                int rowHeight = 24;
                Font font = m_DefaultFont;
                Pen solidPen = new Pen(Brushes.Black, 2);
                int lineHeight = TextRenderer.MeasureText("高", font).Height;
                int charWidth = TextRenderer.MeasureText("宽", font).Height;

                int lineWidth = 770;
                int offsetX = 12;

                float pointX = m_PointXPayType + offsetX;

                //离院方式
                g.DrawString("离院方式", font, Brushes.Black, new PointF(pointX, pointY));
                string leaveType = m_IemMainPageEntity.IemBasicInfo.OutHosType; //todo
                pointX = pointX + TextRenderer.MeasureText("离院方式", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, leaveType, lineHeight);
                pointX = DrawSelectItem(g, pointX, pointY, "1.医嘱离院  2.医嘱转院,");

                //拟接收医疗机构名称
                string medicalName = m_IemMainPageEntity.IemBasicInfo.ReceiveHosPital;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "拟接收医疗机构名称", medicalName, 300, "") + 25;

                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //医嘱转社区卫生服务机构/乡镇卫生院
                string healthOrganizations = m_IemMainPageEntity.IemBasicInfo.ReceiveHosPital2;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "3.医嘱转社区卫生服务机构/乡镇卫生院，拟接收医疗机构名称:", healthOrganizations, 145, " 4.非医嘱离院 5.死亡 9.其他");

                pointY += rowHeight / 2 + 8;
                pointX = m_PointXPayType;

                g.DrawLine(Pens.Black, new Point((int)pointX, (int)pointY), new Point((int)pointX + lineWidth, (int)pointY));

                pointY += rowHeight / 2 - 4;
                pointX += offsetX;

                //是否有出院31天内再住院计划
                g.DrawString("是否有出院31天内再住院计划", font, Brushes.Black, new PointF(pointX, pointY));
                string inHospitalAgain = m_IemMainPageEntity.IemBasicInfo.AgainInHospital; //todo
                pointX = pointX + TextRenderer.MeasureText("是否有出院31天内再住院计划", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, inHospitalAgain, lineHeight);
                pointX = DrawSelectItem(g, pointX, pointY, "1.无  2.有，");

                //目的
                string purpose = m_IemMainPageEntity.IemBasicInfo.AgainInHospitalReason;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "目的:", purpose, 200, "");

                pointY += rowHeight / 2 + 8;
                pointX = m_PointXPayType;

                g.DrawLine(Pens.Black, new Point((int)pointX, (int)pointY), new Point((int)pointX + lineWidth, (int)pointY));

                pointY += rowHeight / 2 - 4;
                pointX += offsetX;

                //颅脑损伤患者昏迷时间
                string sleepDayPre = m_IemMainPageEntity.IemBasicInfo.BeforeHosComaDay;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "颅脑损伤患者昏迷时间： 入院前", sleepDayPre, 40, "天");
                string sleepHourPre = m_IemMainPageEntity.IemBasicInfo.BeforeHosComaHour;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepHourPre, 40, "小时");
                string sleepMinutePre = m_IemMainPageEntity.IemBasicInfo.BeforeHosComaMinute;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepMinutePre, 40, "分钟") + 40;


                //颅脑损伤患者昏迷时间
                string sleepDayAfter = m_IemMainPageEntity.IemBasicInfo.LaterHosComaDay;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "入院后", sleepDayAfter, 40, "天");
                string sleepHourAfter = m_IemMainPageEntity.IemBasicInfo.LaterHosComaHour;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepHourAfter, 40, "小时");
                string sleepMinuteAfter = m_IemMainPageEntity.IemBasicInfo.LaterHosComaMinute;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "", sleepMinuteAfter, 40, "分钟");

                pointY += rowHeight / 2 + 8;
                pointX = m_PointXPayType;

                g.DrawLine(solidPen, new Point((int)pointX, (int)pointY), new Point((int)pointX + lineWidth, (int)pointY));

                return pointY;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 妇科产妇婴儿情况
        /// <summary>
        /// 妇科产妇婴儿情况
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float FuKeChanKeYingEr(Graphics g, float pointY)
        {
            try
            {
                float interval = 25; //行间距
                Font font = m_DefaultFont;
                Font font2 = new Font("宋体", 25f, FontStyle.Bold, GraphicsUnit.Pixel);
                Pen solidPen = new Pen(Brushes.Black, 2);
                int lineHeight = TextRenderer.MeasureText("高", font).Height;
                int charWidth = TextRenderer.MeasureText("宽", font).Height;

                float pointStartX = m_PointXPayType + 12;
                float pointX = pointStartX;

                int lineWidth = 770;
                int offsetX = 12;

                g.DrawString("产 科 产 妇 婴 儿 情 况", font2, Brushes.Black, new RectangleF(m_PointXPayType, pointY, lineWidth, 30), sf);
                pointY = pointY + 40;
                g.DrawLine(solidPen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + lineWidth, pointY));
                pointY = pointY + 10;

                #region 1
                //胎次
                g.DrawString("胎次", font, Brushes.Black, new PointF(pointX, pointY));
                string taiCi = m_IemMainPageEntity.IemObstetricsBaby.TC; //todo
                pointX = pointX + TextRenderer.MeasureText("胎次", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, taiCi, lineHeight) + 10;

                //产次
                g.DrawString("产次", font, Brushes.Black, new PointF(pointX, pointY));
                string chanCi = m_IemMainPageEntity.IemObstetricsBaby.CC; //todo
                pointX = pointX + TextRenderer.MeasureText("产次", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, chanCi, lineHeight) + 10;

                //胎别
                g.DrawString("胎别", font, Brushes.Black, new PointF(pointX, pointY));
                string taiBie = m_IemMainPageEntity.IemObstetricsBaby.TB; //todo
                pointX = pointX + TextRenderer.MeasureText("胎别", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, taiBie, lineHeight) + 10;

                pointX = DrawSelectItem(g, pointX, pointY, "（1.单 2.双 3.多）") + 10;

                //产妇会阴破裂度
                g.DrawString("产妇会阴破裂度", font, Brushes.Black, new PointF(pointX, pointY));
                string poLieLevel = m_IemMainPageEntity.IemObstetricsBaby.CFHYPLD; //todo
                pointX = pointX + TextRenderer.MeasureText("产妇会阴破裂度", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, poLieLevel, lineHeight) + 2;

                pointX = DrawSelectItem(g, pointX, pointY, "（1.Ⅰ 2.Ⅱ 3.Ⅲ）") + 10;

                //接产者
                string acceptUser = m_IemMainPageEntity.IemObstetricsBaby.Midwifery;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "接产者", acceptUser, 70, "") + 15;
                #endregion

                #region 2

                pointY += interval;
                pointX = pointStartX;

                //胎儿性别
                g.DrawString("性别", font, Brushes.Black, new PointF(pointX, pointY));
                string gender = m_IemMainPageEntity.IemObstetricsBaby.Sex; //todo
                pointX = pointX + TextRenderer.MeasureText("性别", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, gender, lineHeight) + 2;

                pointX = DrawSelectItem(g, pointX, pointY, "（1.男  2.女）") + 10;

                //阿帕加评分
                g.DrawString("阿帕加评分", font, Brushes.Black, new PointF(pointX, pointY));
                string apajiapingjia = m_IemMainPageEntity.IemObstetricsBaby.APJ; //todo
                pointX = pointX + TextRenderer.MeasureText("阿帕加评分", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, apajiapingjia, lineHeight) + 2;

                pointX = DrawSelectItem(g, pointX, pointY, "（1—分…A+分）") + 140;

                //身长
                string bodyLength = m_IemMainPageEntity.IemObstetricsBaby.Heigh;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "身长", bodyLength, 50, "CM") + 10;

                //体重
                string bodyWeight = m_IemMainPageEntity.IemObstetricsBaby.Weight;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "体重", bodyWeight, 50, "G") + 10;
                #endregion

                #region 3

                pointY += interval;
                pointX = pointStartX;

                //产出情况
                g.DrawString("产出情况", font, Brushes.Black, new PointF(pointX, pointY));
                string birthStatus = m_IemMainPageEntity.IemObstetricsBaby.CCQK; //todo
                pointX = pointX + TextRenderer.MeasureText("产出情况", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, birthStatus, lineHeight) + 2;

                pointX = DrawSelectItem(g, pointX, pointY, "（1.产活  2.产死  3.死胎  4.畸形）") + 198;

                //出生
                string birth = m_IemMainPageEntity.IemObstetricsBaby.BithDayPrint;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "出生", birth, 175, "") + 30;

                #endregion

                #region 4

                pointY += interval;
                pointX = pointStartX;

                //分娩方式
                g.DrawString("分娩方式", font, Brushes.Black, new PointF(pointX, pointY));
                string birthType = m_IemMainPageEntity.IemObstetricsBaby.FMFS; //todo
                pointX = pointX + TextRenderer.MeasureText("分娩方式", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, birthType, lineHeight) + 2;

                pointX = DrawSelectItem(g, pointX, pointY, "（1.自然 2.测+吸 3.产钳 4.臂牵引 5.剖宫 6.其他）") + 18;

                //出院情况
                g.DrawString("出院情况", font, Brushes.Black, new PointF(pointX, pointY));
                string outHospitalStatus = m_IemMainPageEntity.IemObstetricsBaby.CYQK; //todo
                pointX = pointX + TextRenderer.MeasureText("出院情况", font).Width + 5;
                pointX = DrawCheckBox(g, pointX, pointY, outHospitalStatus, lineHeight) + 2;

                pointX = DrawSelectItem(g, pointX, pointY, "（1.正常  2.有病  3.交叉感染）");

                #endregion

                pointX = pointStartX;
                pointY = pointY + 25;
                g.DrawLine(solidPen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + lineWidth, pointY));

                return pointY;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region 打印费用
        /// <summary>
        /// 打印费用
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintFee(Graphics g, float pointY)
        {
            try
            {
                Font font = m_DefaultFont;
                int lineHeight = TextRenderer.MeasureText("高", font).Height;
                int charWidth = TextRenderer.MeasureText("宽", font).Height;
                float rowHeight = 25; //行间距
                float pointX = m_PointXPayType;
                Pen solidPen = new Pen(Brushes.Black, 2);
                int lineWidth = 770;
                int offsetX = 12;

                g.DrawLine(solidPen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + lineWidth, pointY));
                pointY = pointY + 10;

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
                string normalMedicalServices = m_IemMainPageEntity.IemFeeInfo.YBYLFY;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "1.综合医疗服务类:（1）一般医疗服务费:", normalMedicalServices, 60, "") + 5;

                //一般治疗操作费
                string ybzlczf = m_IemMainPageEntity.IemFeeInfo.YBZLFY;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（2）一般治疗操作费：", ybzlczf, 55, "") + 5;

                //护理费
                string hlfy = m_IemMainPageEntity.IemFeeInfo.Care;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（3）护理费：", hlfy, 55, "") + 5;
                #endregion

                #region 3
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //其他费用
                string otherServices = m_IemMainPageEntity.IemFeeInfo.ZHQTFY;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（4）其他费用:", otherServices, 100, "") + 5;
                #endregion

                #region 4
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //病理诊断费
                string pathologyDiagnisis = m_IemMainPageEntity.IemFeeInfo.BLZDF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "2.诊断类:（5）病理诊断费:", pathologyDiagnisis, 93, "") + 5;

                //实验室诊断费
                string libDiagnisis = m_IemMainPageEntity.IemFeeInfo.SYSZDF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（6）实验室诊断费:", libDiagnisis, 93, "") + 6;

                //影像学诊断费
                string imagingDiagnisis = m_IemMainPageEntity.IemFeeInfo.YXXZDF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（7）影像学诊断费:", imagingDiagnisis, 93, "") + 5;
                #endregion

                #region 5
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //临床诊断项目费
                string clinicDiagnisis = m_IemMainPageEntity.IemFeeInfo.LCZDF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（8）临床诊断项目费:", clinicDiagnisis, 100, "") + 5;
                #endregion

                #region 6
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //非手术治疗项目费
                string unOperationFee = m_IemMainPageEntity.IemFeeInfo.FSSZLF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "3.治疗类：（9）非手术治疗项目费:", unOperationFee, 100, "");

                //临床物理治疗费
                string clinicPhysicsFee = m_IemMainPageEntity.IemFeeInfo.LCWLZLF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（临床物理治疗费:", clinicPhysicsFee, 100, ")") + 5;
                #endregion

                #region 7
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //手术治疗费
                string operationTreatFee = m_IemMainPageEntity.IemFeeInfo.SSZLF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（10）手术治疗费:", operationTreatFee, 100, "");

                //麻醉费
                string anesthesiaFee = m_IemMainPageEntity.IemFeeInfo.MZF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（麻醉费:", anesthesiaFee, 100, "");

                //手术费
                string operationFee = m_IemMainPageEntity.IemFeeInfo.SSF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "手术费:", operationFee, 100, ")");

                #endregion

                #region 8
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //康复费
                string recoveryFee = m_IemMainPageEntity.IemFeeInfo.KFF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "4.康复类:（11）康复费:", recoveryFee, 100, "");
                #endregion

                #region 9
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //中医治疗费
                string zyzdf = m_IemMainPageEntity.IemFeeInfo.ZYZDF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "5.中医类：（12）中医治疗费：", zyzdf, 60, "");

                #region 屏蔽，改成国家统一发布版本
                ////中医诊断费
                //string zyzlf = m_IemMainPageEntity.IemFeeInfo.ZYZLF;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（13）中医治疗", zyzlf, 60, "");

                ////中医外治费
                //string zywz = m_IemMainPageEntity.IemFeeInfo.ZYWZ;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（中医外治:", zywz, 60, "");

                //pointY += rowHeight;
                //pointX = m_PointXPayType + offsetX;

                ////中医骨伤
                //string zygs = m_IemMainPageEntity.IemFeeInfo.ZYGS;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "中医骨伤:", zygs, 55, "");

                ////针刺与灸法
                //string zcyjf = m_IemMainPageEntity.IemFeeInfo.ZCYJF;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "针刺与灸法:", zcyjf, 55, "");

                ////中医推拿治疗
                //string zytnzl = m_IemMainPageEntity.IemFeeInfo.ZYTNZL;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "中医推拿治疗:", zytnzl, 55, "");

                ////中医肛肠治疗
                //string zygczl = m_IemMainPageEntity.IemFeeInfo.ZYGCZL;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "中医肛肠治疗:", zygczl, 55, "");

                ////中医特殊治疗
                //string zytszl = m_IemMainPageEntity.IemFeeInfo.ZYTSZL;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "中医特殊治疗:", zytszl, 55, "）");

                //pointY += rowHeight;
                //pointX = m_PointXPayType + offsetX;

                ////中医其他
                //string zyqt = m_IemMainPageEntity.IemFeeInfo.ZYQT;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（14）中医其他：", zyqt, 60, "");

                ////中药特殊调配加工
                //string zytstpjg = m_IemMainPageEntity.IemFeeInfo.ZYTSTPJG;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（中药特殊调配加工：", zytstpjg, 60, "");

                ////辨证施膳
                //string bzss = m_IemMainPageEntity.IemFeeInfo.BZSS;//todo
                //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "辨证施膳：", bzss, 60, "）");
                #endregion
                #endregion

                #region 10
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //西药费
                string westDrug = m_IemMainPageEntity.IemFeeInfo.XYF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "6.西药类:（13）西药费:", westDrug, 100, "");

                //抗菌药物费用
                string antibiosisDrug = m_IemMainPageEntity.IemFeeInfo.KJYWF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（抗菌药物费用:", antibiosisDrug, 100, ")");
                #endregion

                #region 11
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //中成药费
                string zhongChenDrug = m_IemMainPageEntity.IemFeeInfo.CPMedical;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "7.中药类:（14）中成药费:", zhongChenDrug, 100, "");

                //中草药费
                string zhongCaoDrug = m_IemMainPageEntity.IemFeeInfo.CMedical;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（15）中草药费:", zhongCaoDrug, 100, "");

                #endregion

                #region 12
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //血费
                string bloodFee = m_IemMainPageEntity.IemFeeInfo.BloodFee;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "8.血液和血液制品类:（16）血费:", bloodFee, 71, "");

                //白蛋白类制品费
                string baiDanBaiFee = m_IemMainPageEntity.IemFeeInfo.XDBLZPF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（17）白蛋白类制品费:", baiDanBaiFee, 71, "");

                //球蛋白类制品费
                string qiuDanBaiFee = m_IemMainPageEntity.IemFeeInfo.QDBLZPF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（18）球蛋白类制品费:", qiuDanBaiFee, 70, "");

                #endregion

                #region 13
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //凝血因子类制品费
                string ninXueFee = m_IemMainPageEntity.IemFeeInfo.NXYZLZPF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（19）凝血因子类制品费:", ninXueFee, 100, "");

                //细胞因子类制品费
                string cellFee = m_IemMainPageEntity.IemFeeInfo.XBYZLZPF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（20）细胞因子类制品费:", cellFee, 100, "");

                #endregion

                #region 14
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //检查用一次性医用材料费
                string checkOnceFee = m_IemMainPageEntity.IemFeeInfo.JCYYCXCLF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "9.耗材类:（21）检查用一次性医用材料费:", checkOnceFee, 100, "");

                //治疗用一次性医用材料费
                string treatOnceFee = m_IemMainPageEntity.IemFeeInfo.ZLYYCXCLF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（22）治疗用一次性医用材料费:", treatOnceFee, 100, "");
                #endregion

                #region 15
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                //手术用一次性医用材料费
                string operateOnceFee = m_IemMainPageEntity.IemFeeInfo.SSYYCXCLF;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（23）手术用一次性医用材料费:", operateOnceFee, 100, "");
                #endregion

                #region 16
                pointY += rowHeight;
                pointX = m_PointXPayType + offsetX;

                string otherFee = m_IemMainPageEntity.IemFeeInfo.OtherFee;//todo
                pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "10.其他类:（24）其他费:", otherFee, 100, "");
                #endregion

                pointY = pointY + 22;
                g.DrawLine(solidPen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + lineWidth, pointY));

                return pointY;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region 打印最后一页  提示部分
        /// <summary>
        ///打印最后一页  提示部分
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintNote(Graphics g, float pointY)
        {
            try
            {
                Font font = m_DefaultFont;
                int lineHeight = TextRenderer.MeasureText("高", font).Height;
                int charWidth = TextRenderer.MeasureText("宽", font).Height;
                float rowHeight = 25; //行间距
                Pen solidPen = new Pen(Brushes.Black, 2);
                int lineWidth = 770;
                int offsetX = 12;
                float pointX = m_PointXPayType + offsetX;

                g.DrawString("说明：（一）医疗付费方式  1.城镇职工基本医疗保险  2.城镇居民基本医疗保险 3.新型农村合作医疗 4.贫困救助", font, Brushes.Black, new PointF(pointX, pointY));
                pointY += 20;
                g.DrawString("5.商业医疗保险  6.全公费  7.全自费  8.其他社会保险  9.其他", font, Brushes.Black, new PointF(pointX, pointY));
                pointY += 20;

                int width = TextRenderer.MeasureText("说明：", font).Width;

                g.DrawString("（二）凡可由医院信息系统提供住院费用清单的，住院病案首页中可不填写“住院费用”。", font, Brushes.Black, new PointF(pointX + width - 7, pointY));



                return pointY;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #endregion

        #endregion 绘制病案首页
    }
}
