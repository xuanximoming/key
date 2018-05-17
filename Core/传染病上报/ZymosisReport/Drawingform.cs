using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public class Drawingform
    {
        #region 属性

        PrintDocument m_PrintDocument;

        public int m_PageWidth = 800;//用于设定PictureBox的宽度
        public int m_PageHeight = 1120;//用于设定PictureBox的高度
        readonly ZymosisReportEntity m_ZymosisReportEntity = new ZymosisReportEntity();
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

        int m_PageIndex = 1;
        Pen pen = new Pen(Brushes.Black, 1);
        Pen pen1 = new Pen(Brushes.Black, 1);

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

        #region 方法
        public Drawingform(ZymosisReportEntity m_ZymosisReportEntitys)
        {
            m_ZymosisReportEntity = m_ZymosisReportEntitys;
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




        void PrintFirstPage(Graphics g)
        {
            float pointY;
            DrawTitle(g);
            pointY = PrintPayType(g);
            pointY = PrintPatientBaseInfo(g, pointY + 15);
            //pointY = PrintOutHospitalDiaglosis(g, pointY + 18);
            //pointY = PrintFristPageOther(g, pointY);
        }
        //第二页
        private void PrintSecondPage(Graphics g)
        {
            float pointY;
            SecondTitle(g);
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
            Font font1 = new Font("宋体", 25f, FontStyle.Bold, GraphicsUnit.Pixel);

            g.DrawString("中华人民共和国传染病报告卡", font1, Brushes.Black, new RectangleF(0f, m_PointYTitle + 15, m_PageWidth, 30), sf);
        }

        /// <summary>
        /// 第一行头列
        /// </summary>
        /// <param name="g"></param>
        private float PrintPayType(Graphics g)
        {
            Font font = m_DefaultFont;
            Font font1 = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
            string reportNo = m_ZymosisReportEntity.ReportNo;//传染病卡号
            g.DrawString("卡片编号:", font1, Brushes.Black, new PointF(m_PointXPayType + 20, m_PointYPayType));
            g.DrawString("卡片类别:", font1, Brushes.Black, new PointF(500, m_PointYPayType));
            string type = m_ZymosisReportEntity.ReportType;
            string type1 = "";//1初次报告2订正报告
            string type2 = "";
            if (type == "1")
            {
                type1 = "√";
            }
            else
            {
                type2 = "√";
            }

            DrawCheckBox(g, 575, m_PointYPayType, type1, 15);
            g.DrawString("1.初次报告 ", font1, Brushes.Black, new PointF(590, m_PointYPayType));//2.订正报告
            DrawCheckBox(g, 670, m_PointYPayType, type2, 15);
            g.DrawString("2.订正报告 ", font1, Brushes.Black, new PointF(685, m_PointYPayType));
            g.DrawLine(pen, new PointF(m_PointXPayType, m_PointYPayType + 20), new PointF(m_PointXPayType + 770, m_PointYPayType + 20));
            return m_PointYPayType + 21;
        }




        /// <summary>
        /// 绘制第一页详细信息
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pointY"></param>
        /// <returns></returns>
        private float PrintPatientBaseInfo(Graphics g, float pointY)
        {

            Font font = m_DefaultFont;
            int lineHeight = TextRenderer.MeasureText("高", font).Height;
            int charWidth = TextRenderer.MeasureText("宽", font).Height;
            float interval = 31; //行间距
            Font font1 = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
            float pointStartX = m_PointXPayType + 12;
            float pointX = pointStartX;

            g.DrawLine(pen1, pointX - 12, pointY - 15, pointX - 12, m_PageHeight - 15);
            g.DrawLine(pen1, m_PageWidth - pointX + 12, pointY - 15, m_PageWidth - pointX + 12, m_PageHeight - 15);
            //姓名
            string name = m_ZymosisReportEntity.Name;//患者姓名
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "患者姓名*:", name, 150, "");
            string parentname = m_ZymosisReportEntity.Parentname;//家长姓名
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, charWidth, "（患儿家长姓名：", parentname, 150, ")") + 25;
            //身份证号
            pointY += interval;
            pointX = pointStartX;
            string idno = m_ZymosisReportEntity.Idno;//身份证号
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "身份证号:", idno, 500, "");
            //性别
            string sex = m_ZymosisReportEntity.Sex;
            string m = "";
            string w = "";
            if (sex == "2")
            {
                w = "√";
            }
            else
            {
                m = "√";
            }

            g.DrawString("性别*：", font1, Brushes.Black, new PointF(pointX + 10, pointY));
            DrawCheckBox(g, pointX + 70, pointY, m, 15);
            g.DrawString("男", font1, Brushes.Black, new PointF(pointX + 85, pointY));
            DrawCheckBox(g, pointX + 105, pointY, w, 15);
            g.DrawString("女", font1, Brushes.Black, new PointF(pointX + 120, pointY));

            pointY += interval;
            pointX = pointStartX;
            //出生日期
            DateTime birth = DateTime.Parse(m_ZymosisReportEntity.Birth.ToString());//日期
            string y;
            string M;
            string d;
            if (m_ZymosisReportEntity.Birth.ToString() == "1900-01-01")
            {
                y = "";
                M = "";
                d = "";
            }
            else
            {
                y = birth.ToString("yyyy");
                M = birth.ToString("MM");
                d = birth.ToString("dd");
            }

            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "出身日期*:", y, 90, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "年", M, 60, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "月", d, 60, "");
            string age = m_ZymosisReportEntity.Age;  //年龄
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "日(如初日期不详，实足年龄：", age, 40, "年龄单位：");
            string ageunit = m_ZymosisReportEntity.AgeUnit;
            string ageunit1 = "";//岁
            string ageunit2 = "";//月
            string ageunit3 = "";//天
            if (ageunit == "1")
            {
                ;
                ageunit1 = "√";
            }
            if (ageunit == "2")
            {
                ageunit2 = "√";
            }
            else
            {
                ageunit3 = "√";
            }
            DrawCheckBox(g, pointX, pointY, ageunit1, 15);
            g.DrawString("岁", font1, Brushes.Black, new PointF(pointX + 15, pointY));
            DrawCheckBox(g, pointX + 40, pointY, ageunit2, 15);
            g.DrawString("月", font1, Brushes.Black, new PointF(pointX + 55, pointY));
            DrawCheckBox(g, pointX + 80, pointY, ageunit3, 15);
            g.DrawString("天)", font1, Brushes.Black, new PointF(pointX + 95, pointY));

            pointY += interval;
            pointX = pointStartX;
            //工作单位联系电话
            string organization = m_ZymosisReportEntity.Organization;//工作单位
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "工作单位：", organization, 400, "");
            string officetel = m_ZymosisReportEntity.Officetel;//单位电话
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "联系电话：", officetel, 150, "");

            pointY += interval;
            pointX = pointStartX;
            //病人属于
            string checkstring = "";//本县
            string checkstring1 = "";//本市及其他地区
            string checkstring2 = "";//本省其他地区
            string checkstring3 = "";//省外
            string checkstring4 = "";//港澳台
            string checkstring5 = "";//外籍
            string address = m_ZymosisReportEntity.Addresstype;
            if (address == "1")
            {
                checkstring = "√";
            }
            if (address == "2")
            {
                checkstring1 = "√";
            }
            if (address == "3")
            {
                checkstring2 = "√";
            }
            if (address == "4")
            {
                checkstring3 = "√";
            }
            if (address == "5")
            {
                checkstring4 = "√";
            }
            if (address == "6")
            {
                checkstring5 = "√";
            }
            g.DrawString("病人属于：", font1, Brushes.Black, new PointF(pointX - 5, pointY));
            DrawCheckBox(g, pointX += 82, pointY, checkstring, 15);
            g.DrawString("本县区", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 102, pointY, checkstring1, 15);
            g.DrawString("本市其他地区", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 102, pointY, checkstring2, 15);
            g.DrawString("本省其他地区", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 102, pointY, checkstring3, 15);
            g.DrawString("外省", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 87, pointY, checkstring4, 15);
            g.DrawString("港澳台", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 87, pointY, checkstring5, 15);
            g.DrawString("外籍", font1, Brushes.Black, new PointF(pointX += 15, pointY));

            pointY += interval;
            pointX = pointStartX;
            //现住址
            string add = m_ZymosisReportEntity.Address;//居住详细地址
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "现住址(详填)*：", add, 600, "");
            pointY += interval;
            pointX = pointStartX;
            //患者职业
            string job = m_ZymosisReportEntity.Jobid;
            string job1 = "";//幼托儿童
            string job2 = "";//散居儿童
            string job3 = "";//学生
            string job4 = "";//教师
            string job5 = "";//保育员及保姆
            string job6 = "";//餐饮食品业
            string job7 = "";//商业服务
            string job8 = "";//医务人员
            string job9 = "";//工人
            string job10 = "";//民工
            string job11 = "";//农民
            string job12 = "";//牧民
            string job13 = "";//渔民
            string job14 = "";//干部职工
            string job15 = "";//离退人员
            string job16 = "";//家务及其待业
            string job17 = "";//其他
            string job18 = "";//不详
            if (job == "1")
            {
                job1 = "√";
            }
            if (job == "2")
            {
                job2 = "√";
            }
            if (job == "3")
            {
                job3 = "√";
            }
            if (job == "4")
            {
                job4 = "√";
            }
            if (job == "5")
            {
                job5 = "√";
            }
            if (job == "6")
            {
                job6 = "√";
            }
            if (job == "7")
            {
                job7 = "√";
            }
            if (job == "8")
            {
                job8 = "√";
            }
            if (job == "9")
            {
                job9 = "√";
            }
            if (job == "10")
            {
                job10 = "√";
            }
            if (job == "11")
            {
                job11 = "√";
            }
            if (job == "12")
            {
                job12 = "√";
            }
            if (job == "13")
            {
                job13 = "√";
            }
            if (job == "14")
            {
                job14 = "√";
            }
            if (job == "15")
            {
                job15 = "√";
            }
            if (job == "16")
            {
                job16 = "√";
            }
            if (job == "17")
            {
                job17 = "√";
            }
            if (job == "18")
            {
                job18 = "√";
            }
            g.DrawString("患者职业*：", font1, Brushes.Black, new PointF(pointX - 5, pointY));
            pointY += interval;
            pointX = pointStartX;
            DrawCheckBox(g, pointX - 5, pointY, job1, 15);
            g.DrawString("幼托儿童、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 77, pointY, job2, 15);
            g.DrawString("散居儿童、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 77, pointY, job3, 15);
            g.DrawString("学生(大中小学)、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 117, pointY, job4, 15);
            g.DrawString("教师、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 57, pointY, job5, 15);
            g.DrawString("保育员及保姆、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 97, pointY, job6, 15);
            g.DrawString("餐饮食品业、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 97, pointY, "", 15);
            g.DrawString("公共场所服务员、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            pointY += interval;
            pointX = pointStartX;
            DrawCheckBox(g, pointX - 5, pointY, job7, 15);
            g.DrawString("商业服务、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 75, pointY, job8, 15);
            g.DrawString("医务人员、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 75, pointY, job9, 15);
            g.DrawString("工人、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 55, pointY, job10, 15);
            g.DrawString("民工、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 55, pointY, job11, 15);
            g.DrawString("农民、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 55, pointY, job12, 15);
            g.DrawString("牧民、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 55, pointY, job13, 15);
            g.DrawString("渔民、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 55, pointY, "", 15);
            g.DrawString("海员及长途驾驶员、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 128, pointY, job14, 15);
            g.DrawString("干部职员、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            pointY += interval;
            pointX = pointStartX;
            DrawCheckBox(g, pointX - 5, pointY, job15, 15);
            g.DrawString("离退人员、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 70, pointY, job16, 15);
            g.DrawString("家务及待业、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 85, pointY, job17, 15);
            g.DrawString("其他（         ）、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 140, pointY, job18, 15);
            g.DrawString("不详", font1, Brushes.Black, new PointF(pointX += 15, pointY));

            pointY += interval;
            pointX = pointStartX;
            //病例分类
            string recordtype1 = m_ZymosisReportEntity.Recordtype1;//病理分类1
            string record1 = "";//疑似病例
            string record2 = "";//临床诊断病例
            string record3 = "";//实验室确诊病例
            string record4 = "";//病原携带者
            if (recordtype1 == "1")
            {
                record1 = "√";
            }
            if (recordtype1 == "2")
            {
                record2 = "√";
            }
            if (recordtype1 == "3")
            {
                record3 = "√";
            }
            if (recordtype1 == "4")
            {
                record4 = "√";
            }
            g.DrawString("病例分类*：  (1)", font1, Brushes.Black, new PointF(pointX - 5, pointY));
            DrawCheckBox(g, pointX += 120, pointY, record3, 15);
            g.DrawString("实验室确诊病例、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 120, pointY, record2, 15);
            g.DrawString("临床诊断病例、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 100, pointY, record1, 15);
            g.DrawString("疑似病例、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 100, pointY, record4, 15);
            g.DrawString("病原携带者、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 100, pointY, "", 15);//该选项占无
            g.DrawString("阳性检测", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            pointY += interval;
            pointX = pointStartX;
            string recordtype2 = m_ZymosisReportEntity.Recordtype2;//病理分类2
            string record5 = "";//急性
            string record6 = "";//慢性
            if (recordtype2 == "1")
            {
                record5 = "√";
            }
            if (recordtype2 == "2")
            {
                record6 = "√";
            }
            g.DrawString("(2)", font1, Brushes.Black, new PointF(pointX + 86, pointY));
            DrawCheckBox(g, pointX += 120, pointY, record5, 15);
            g.DrawString("急性、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 70, pointY, record6, 15);
            g.DrawString("慢性、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 100, pointY, "", 15);
            g.DrawString("未分类(乙型肝炎  吸血虫病填写)", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            pointY += interval;
            pointX = pointStartX;
            //发病日期
            string att = m_ZymosisReportEntity.Attackdate;//发病日期
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "发病日期*：", att, 400, "(病原携带者填初检日期或就诊时间)");
            pointY += interval;
            pointX = pointStartX;
            //就诊日期
            string dia = m_ZymosisReportEntity.Diagdate;//就诊日期
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "就诊日期*：", dia, 400, "");
            pointY += interval;
            pointX = pointStartX;
            //死亡日期
            string die = m_ZymosisReportEntity.Diedate;//死亡日期
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "死亡日期*：", die, 400, "");
            pointY += interval + 5;
            pointX = pointStartX;

            g.DrawLine(pen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + 770, pointY));
            //甲种传染病
            pointY += interval;
            pointX = pointStartX;
            string DiagicdName = m_ZymosisReportEntity.Diagname.ToString();
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "上报传染病:", DiagicdName, 600, "");

            //string diagicd = m_ZymosisReportEntity.Diagicd10;//ICD-10病理编码
            //string diagicd1 = "";//鼠疫A20.901
            //string diagicd2 = "";//霍乱A00.901
            //if (diagicd == "A20.901")
            //{
            //    diagicd1 = "√";
            //}
            //if (diagicd == "A00.901")
            //{
            //    diagicd2 = "√";
            //}
            //g.DrawString("甲种传染病*：  ", font1, Brushes.Black, new PointF(pointX - 5, pointY-5));
            //DrawCheckBox(g, pointX += 120, pointY-5, diagicd1, 15);
            //g.DrawString("鼠疫、", font1, Brushes.Black, new PointF(pointX += 15, pointY-5));
            //DrawCheckBox(g, pointX += 70, pointY-5, diagicd2, 15);
            //g.DrawString("霍乱", font1, Brushes.Black, new PointF(pointX += 15, pointY-5));
            //pointY += interval + 5;
            //pointX = pointStartX;
            //g.DrawLine(pen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + 770, pointY));
            ////乙种传染病
            //pointY += interval;
            //pointX = pointStartX;
            //g.DrawString("乙种传染病*：  ", font1, Brushes.Black, new PointF(pointX - 5, pointY-5));
            //pointY += interval;
            //pointX = pointStartX;
            //string diagicd3 = "";//传染性非典型肺炎U04.9
            //string diagicd4 = "";//B19.902病毒性肝炎B24..01艾滋病
            //string diagicd5 = "";//甲B15.001 乙型 B16.905 丙B17.101 戌 B17.201 未B19
            //string diagicd6 = "";//甲B15.001 乙型 B16.905 丙B17.101 戌 B17.201 未B19
            //string diagicd7 = "";//甲B15.001 乙型 B16.905 丙B17.101 戌 B17.201 未B19
            //string diagicd8 = "";//甲B15.001 乙型 B16.905 丙B17.101 戌 B17.201 未B19
            //string diagicd9 = "";
            //if (diagicd == "U04.9")
            //{
            //    diagicd3 = "√";
            //}
            //if (diagicd == "B19.902" || diagicd == "B24..01")
            //{
            //    diagicd4 = "√";
            //}
            //if (diagicd == "B15.001")
            //{
            //    diagicd4 = "√";
            //    diagicd5 = "√";
            //}
            //if (diagicd == "B16.905")
            //{
            //    diagicd4 = "√";
            //    diagicd6 = "√";
            //}
            //if (diagicd == "B17.101")
            //{
            //    diagicd4 = "√";
            //    diagicd7 = "√";
            //}
            //if (diagicd == "B17.201")
            //{
            //    diagicd4 = "√";
            //    diagicd8 = "√";
            //}
            //if (diagicd == "B19")
            //{
            //    diagicd4 = "√";
            //    diagicd9 = "√";
            //}
            //DrawCheckBox(g, pointX - 5, pointY, diagicd3, 15);////U04.9
            //g.DrawString("传染性非典型肺炎、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 140, pointY, diagicd4, 15);
            //g.DrawString("艾滋病、病毒性肝炎（", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 164, pointY, diagicd5, 15);
            //g.DrawString("甲型、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 63, pointY, diagicd6, 15);
            //g.DrawString("乙型、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 63, pointY, diagicd7, 15);
            //g.DrawString("丙型、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 63, pointY, diagicd8, 15);
            //g.DrawString("戌型、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 63, pointY, diagicd9, 15);
            //g.DrawString("未分型  ）、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //pointY += interval;
            //pointX = pointStartX;
            //string diagicd10 = "";//脊髓灰质炎 A80.901
            //string diagicd11 = "";//人感染高致病性禽流感
            //string diagicd12 = "";//甲型H1N1流感
            //string diagicd13 = "";//麻疹
            //string diagicd14 = "";//流行性出血热A98.502+
            //string diagicd15 = "";//狂犬病A92.901
            //if (diagicd == "A80.901")
            //{
            //    diagicd10 = "√";
            //}
            //if (diagicd == "B05.001" || diagicd == "B05.201" || diagicd == "B05.301" || diagicd == "B05.801" || diagicd == "B05.901")
            //{
            //    diagicd13 = "√";
            //}
            //if (diagicd == "A98.502+")
            //{
            //    diagicd14 = "√";
            //}
            //if (diagicd == "A82.901")
            //{
            //    diagicd15 = "√";
            //}
            //DrawCheckBox(g, pointX - 5, pointY, diagicd10, 15);
            //g.DrawString("脊髓灰质炎、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 107, pointY, diagicd11, 15);//占无
            //g.DrawString("人感染高致病性禽流感、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 177, pointY, diagicd12, 15);//占无
            //g.DrawString("甲型H1N1流感、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 127, pointY, diagicd13, 15);
            //g.DrawString("麻疹、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 77, pointY, diagicd14, 15);
            //g.DrawString("流行性出血热、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 117, pointY, diagicd15, 15);
            //g.DrawString("狂犬病、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //pointY += interval;
            //pointX = pointStartX;
            //string diagicd16 = "";//流行性乙型脑炎A93.001
            //string diagicd17 = "";//登革热A90，炭疽热A22.901，痢疾，肺结核，伤寒A01.001
            //string diagicd18 = "";//肺炭疽A22.101
            //string diagicd19 = "";//皮肤炭疽
            //string diagicd20 = "";//未分型
            //string diagicd21 = "";//细菌痢疾A03.901
            //string diagicd22 = "";//阿米巴痢疾
            //string diagicd23 = "";//涂阳
            //string diagicd24 = "";//仅墙阳
            //string diagicd25 = "";//菌阴
            //string diagicd26 = "";//未痰检
            //string diagicd27 = "";//伤寒A01.001
            //string diagicd28 = "";//副伤寒A01.401  
            //if (diagicd == "A83.001")
            //{
            //    diagicd16 = "√";
            //}
            //if (diagicd == "A90" || diagicd == "A22.901" || diagicd == "A01.001")
            //{
            //    diagicd17 = "√";
            //}
            //if (diagicd == "A22.101")
            //{
            //    diagicd17 = "√";
            //    diagicd18 = "√";
            //}
            //if (diagicd == "A03.901")
            //{
            //    diagicd17 = "√";
            //    diagicd21 = "√";
            //}
            //if (diagicd == "A06.003")
            //{
            //    diagicd17 = "√";
            //    diagicd22 = "√";
            //}
            //if (diagicd == "A01.001")
            //{
            //    diagicd17 = "√";
            //    diagicd27 = "√";
            //}
            //if (diagicd == "A01.401")
            //{
            //    diagicd17 = "√";
            //    diagicd28 = "√";
            //}
            //DrawCheckBox(g, pointX - 5, pointY, diagicd16, 15);
            //g.DrawString("流行性乙型脑炎、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 127, pointY, diagicd17, 15);
            //g.DrawString("登革热，炭疽（", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 107, pointY, diagicd18, 15);
            //g.DrawString("肺炭疽、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 67, pointY, diagicd19, 15);//占无
            //g.DrawString("皮肤炭疽、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 72, pointY, diagicd20, 15);//占无
            //g.DrawString("未分型）、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //g.DrawString("痢疾(", font1, Brushes.Black, new PointF(pointX += 70, pointY));
            //DrawCheckBox(g, pointX += 47, pointY, diagicd21, 15);
            //g.DrawString("细菌性、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 77, pointY, diagicd22, 15);
            //g.DrawString("阿米巴性）、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //pointY += interval;
            //pointX = pointStartX;
            //g.DrawString("肺结核(", font1, Brushes.Black, new PointF(pointX - 5, pointY));
            //DrawCheckBox(g, pointX += 67, pointY, diagicd23, 15);//占无
            //g.DrawString("涂阳、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 47, pointY, diagicd24, 15);//占无
            //g.DrawString("仅墙阳、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 67, pointY, diagicd25, 15);//占无
            //g.DrawString("菌阴、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 67, pointY, diagicd26, 15);//占无
            //g.DrawString("未痰检）、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //g.DrawString("伤寒(", font1, Brushes.Black, new PointF(pointX += 80, pointY));
            //DrawCheckBox(g, pointX += 47, pointY, diagicd27, 15);
            //g.DrawString("伤寒、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 57, pointY, diagicd28, 15);
            //g.DrawString("副伤寒)、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //string diagicd29 = "";//流行性脑脊髓膜炎A83.002
            //if (diagicd == "A83.002")
            //{
            //    diagicd29 = "√";
            //}
            //DrawCheckBox(g, pointX += 90, pointY, diagicd29, 15);
            //g.DrawString("流行性脑脊髓膜炎、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //pointY += interval;
            //pointX = pointStartX;
            //string diagicd30 = "";//百日咳A37.901
            //string diagicd31 = "";//白喉A36.901
            //string diagicd32 = "";//新生儿破伤风A33..01
            //string diagicd33 = "";//猩红热A38..01
            //string diagicd34 = "";//布鲁氏菌病A23
            //string diagicd35 = "";//淋病A54.901 梅毒A53.901
            //string diagicd36 = "";//早起梅毒A51
            //string diagicd37 = "";//二期梅毒A51.401
            //string diagicd38 = "";//晚期梅毒A52.901
            //string diagicd39 = "";//
            //string diagicd40 = "";//隐性梅毒A53.001
            //string diagicd41 = "";//钩端螺旋体病 A27.903
            //string diagicd42 = "";//吸血虫病B65.202
            ////string diagicd43 = "";//恶性疟疾B50 ，间日疟疾B51 未指定B54
            //if (diagicd == "A37.901")
            //{
            //    diagicd30 = "√";
            //}
            //if (diagicd == "A36.901")
            //{
            //    diagicd31 = "√";
            //}
            //if (diagicd == "A33..01")
            //{
            //    diagicd32 = "√";
            //}
            //if (diagicd == "A38..01")
            //{
            //    diagicd33 = "√";
            //}
            //if (diagicd == "A23")
            //{
            //    diagicd34 = "√";
            //}
            //if (diagicd == "A54.901" || diagicd == "A53.901")
            //{
            //    diagicd35 = "√";
            //}

            //if (diagicd == "A51")
            //{
            //    diagicd35 = "√";
            //    diagicd36 = "√";
            //}
            //if (diagicd == "A51.401")
            //{
            //    diagicd35 = "√";
            //    diagicd37 = "√";
            //}
            //if (diagicd == "A52.901")
            //{
            //    diagicd35 = "√";
            //    diagicd38 = "√";
            //}
            //if (diagicd == "A53.001")
            //{
            //    diagicd35 = "√";
            //    diagicd40 = "√";
            //}

            //if (diagicd == "  A27.903")
            //{
            //    diagicd41 = "√";
            //}
            //if (diagicd == "B65.202" || diagicd == "B50" || diagicd == "B51" || diagicd == "B54")
            //{
            //    diagicd42 = "√";
            //}

            //DrawCheckBox(g, pointX - 5, pointY, diagicd30, 15);
            //g.DrawString("百日咳、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 65, pointY, diagicd31, 15);
            //g.DrawString("白喉、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 65, pointY, diagicd32, 15);
            //g.DrawString("新生儿破伤风、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 105, pointY, diagicd33, 15);
            //g.DrawString("猩红热、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 65, pointY, diagicd34, 15);
            //g.DrawString("布鲁氏菌病、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 85, pointY, diagicd35, 15);
            //g.DrawString("淋病,梅毒(", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 80, pointY, diagicd36, 15);
            //g.DrawString("I期", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 45, pointY, diagicd37, 15);
            //g.DrawString("II期", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 45, pointY, diagicd38, 15);
            //g.DrawString("III期", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //pointY += interval;
            //pointX = pointStartX;
            //DrawCheckBox(g, pointX - 5, pointY, diagicd39, 15);//占无
            //g.DrawString("胎传", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 40, pointY, diagicd40, 15);
            //g.DrawString("隐性）、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 60, pointY, diagicd41, 15);
            //g.DrawString("钩端螺旋体病、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 110, pointY, diagicd42, 15);
            //g.DrawString("吸血虫病,疟疾", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //pointY += interval + 5;
            //pointX = pointStartX;
            //g.DrawLine(pen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + 770, pointY));

            ////乙种传染病
            //pointY += interval;
            //pointX = pointStartX;
            //g.DrawString("丙种传染病*：  ", font1, Brushes.Black, new PointF(pointX - 5, pointY-5));
            //pointY += interval;
            //pointX = pointStartX;
            //string diagicd43 = "";//流行性感冒;J11
            //string diagicd44 = "";//流行性腮腺炎;B26.901
            //string diagicd45 = "";//风疹;B06
            //string diagicd46 = "";//急性出血性结膜炎;B30.301+
            //string diagicd47 = "";//麻风A30
            //string diagicd48 = "";//流行性和地方性斑疹伤寒A75.001流行性 A75.201地方性
            //if (diagicd == "J11")
            //{
            //    diagicd43 = "√";
            //}
            //if (diagicd == "B26.901")
            //{
            //    diagicd44 = "√";
            //}
            //if (diagicd == "B06")
            //{
            //    diagicd45 = "√";
            //}
            //if (diagicd == "B30.301+")
            //{
            //    diagicd46 = "√";
            //}
            //if (diagicd == "A30")
            //{
            //    diagicd47 = "√";
            //}
            //if (diagicd == "A75.001" || diagicd == "A75.201")
            //{
            //    diagicd48 = "√";
            //}
            //DrawCheckBox(g, pointX - 5, pointY, diagicd43, 15);
            //g.DrawString("流行性感冒、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 100, pointY, diagicd44, 15);
            //g.DrawString("流行性腮腺炎、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 110, pointY, diagicd45, 15);
            //g.DrawString("风疹、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 70, pointY, diagicd46, 15);
            //g.DrawString("急性出血性结膜炎、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 135, pointY, diagicd47, 15);
            //g.DrawString("麻风病、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 80, pointY, diagicd48, 15);
            //g.DrawString("流行性和地方性斑疹伤寒、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //pointY += interval;
            //pointX = pointStartX;
            //string diagicd49 = "";//黑热病B55.001
            //string diagicd50 = "";//包虫病B67.901
            //string diagicd51 = "";//丝虫病B74.905
            //string diagicd52 = "";//除霍乱，细菌性和阿米巴性痢疾，伤寒和副伤寒以外的感染型腹泻病
            //if (diagicd == "B55.001")
            //{
            //    diagicd49 = "√";
            //}
            //if (diagicd == "B67.901")
            //{
            //    diagicd50 = "√";
            //}
            //if (diagicd == "B74.905")
            //{
            //    diagicd51 = "√";
            //}
            //DrawCheckBox(g, pointX - 5, pointY, diagicd49, 15);
            //g.DrawString("黑热病、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 75, pointY, diagicd50, 15);
            //g.DrawString("包虫病、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 75, pointY, diagicd51, 15);
            //g.DrawString("丝虫病、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //DrawCheckBox(g, pointX += 75, pointY, diagicd52, 15);
            //g.DrawString("除霍乱，细菌性和阿米巴性痢疾，伤寒和副伤寒以外的感染型腹泻病、", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            //pointY += interval;
            //pointX = pointStartX;
            //string diagicd53 = "";//手足口B08.401
            //if (diagicd == "B08.401")
            //{
            //    diagicd53 = "√";
            //}
            //DrawCheckBox(g, pointX - 5, pointY, diagicd53, 15);
            //g.DrawString("手足口病", font1, Brushes.Black, new PointF(pointX += 15, pointY));

            pointY += interval + 5;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + 770, pointY));

            //其他法定管理和重点监测传染病
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("其他法定管理和重点监测传染病*：  ", font1, Brushes.Black, new PointF(pointX - 5, pointY - 10));
            pointY += interval;
            pointX = pointStartX;
            string oth = m_ZymosisReportEntity.OtherDiag;//其他法定管理和重点监测传染病
            //g.DrawString(oth, font1, Brushes.Black, new PointF(pointX + 10, pointY));

            DrawStringInCell(g, oth, pointX + 20, pointY - 10, 720, 30, font1);
            pointY += interval + 5;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + 770, pointY));
            //密切接触者有无相同症状
            pointY += interval;
            pointX = pointStartX;
            string infe = m_ZymosisReportEntity.InfectotherFlag;
            string infe1 = "";//有
            string infe2 = "";//无
            if (infe == "0")
            {
                infe1 = "√";
            }
            else
            {
                infe2 = "√";
            }
            g.DrawString("密切接触者有无相同症状：  ", font1, Brushes.Black, new PointF(pointX - 5, pointY));
            DrawCheckBox(g, pointX += 170, pointY, infe1, 15);
            g.DrawString("有", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            DrawCheckBox(g, pointX += 40, pointY, infe2, 15);
            g.DrawString("无", font1, Brushes.Black, new PointF(pointX += 15, pointY));
            pointY += interval + 5;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + 770, pointY));

            //其他
            pointY += interval;
            pointX = pointStartX;
            string correctName = m_ZymosisReportEntity.CorrectName;//订正病名
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "订正病名：", correctName, 250, "");
            string reason = m_ZymosisReportEntity.CancelReason;//退卡原因
            pointX = DrawNameAndValueAndUnderLine(g, pointX + 50, pointY, lineHeight, charWidth, "退卡原因：", reason, 250, "");
            pointY += interval;
            pointX = pointStartX;
            string reportdeptname = m_ZymosisReportEntity.Reportdeptname;//报告单位
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "报告单位：", reportdeptname, 250, "");
            string doctortel = m_ZymosisReportEntity.Doctortel;//联系电话
            pointX = DrawNameAndValueAndUnderLine(g, pointX + 50, pointY, lineHeight, charWidth, "联系电话：", doctortel, 250, "");
            pointY += interval;
            pointX = pointStartX;
            string reportdocname = m_ZymosisReportEntity.Reportdocname;//报告医生
            pointX = DrawNameAndValueAndUnderLine(g, pointX - 5, pointY, lineHeight, charWidth, "报告医生：", reportdocname, 250, "");
            string reportDate = m_ZymosisReportEntity.ReportDate;//填卡日期
            pointX = DrawNameAndValueAndUnderLine(g, pointX + 50, pointY, lineHeight, charWidth, "填卡日期：", reportDate, 250, "");
            pointY += interval + 5;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(m_PointXPayType, pointY), new PointF(m_PointXPayType + 770, pointY));
            //备注
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("备注：  ", font1, Brushes.Black, new PointF(pointX - 5, pointY - 5));
            pointY += interval;
            pointX = pointStartX;
            string memo = m_ZymosisReportEntity.Memo;//备注
            //g.DrawString(memo, font1, Brushes.Black, new PointF(pointX + 20, pointY));

            DrawStringInCell(g, memo, pointX + 30, pointY, 720, 30, font1);
            pointY += interval + 5;
            pointX = pointStartX;
            g.DrawLine(pen, new PointF(m_PointXPayType, pointY + 20), new PointF(m_PointXPayType + 770, pointY + 20));
            return pointY - 2;
        }

        /// <summary>
        /// 绘制第二页标题
        /// </summary>
        /// <param name="g"></param>
        private void SecondTitle(Graphics g)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Font font1 = new Font("宋体", 25f, FontStyle.Bold, GraphicsUnit.Pixel);
            g.DrawString("《中华人民共和国传染病报告卡》填卡说明", font1, Brushes.Black, new RectangleF(0f, m_PointYTitle + 30, m_PageWidth, 30), sf);
            Font font = m_DefaultFont;
            Font font2 = new Font("宋体", 14f, FontStyle.Bold, GraphicsUnit.Pixel);
            Font font3 = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
            ////医疗付款方式
            //g.DrawString("医疗付款方式:", font1, Brushes.Black, new PointF(pointX, m_PointYPayType - 25));
            //string payType = m_IemMainPageEntity.IemBasicInfo.PayID; //todo
            //pointX = pointX + TextRenderer.MeasureText("医疗付款方式:", font1).Width;
            //pointX = DrawCheckBox(g, pointX, m_PointYPayType - 25, payType, lineHeight);
            //pointX = DrawSelectItem(g, pointX, m_PointYPayType - 25, "") + 20;
            float interval = 25; //行间距
            float pointStartX = m_PointXPayType + 12;
            float pointX = pointStartX;
            float pointY = m_PointYPayType;
            g.DrawString("卡片编码：", font2, Brushes.Black, new PointF(pointX, pointY));
            g.DrawString("由报告单位自行编制填写。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("姓   名:", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("填写患者或献血员的名字（性病/AIDS 等可填写代号）,如果登记身份证号码，则姓名应该和身份证上的姓名", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("        一致。", font3, Brushes.Black, new PointF(pointX + 2, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("家长姓名：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("14 岁以下的患儿要求填写患者家长姓名。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("身份证号：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("既可填写15位身份证号，也可填写18 位身份证号。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("性    别：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("在相应的性别前打√。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("出生日期：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("出生日期与年龄栏只要选择一栏填写即可，不必既填出生日期，又填年龄。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("实足年龄：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("对出生日期不详的用户填写年龄。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("年龄单位：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("对于新生儿和只有月龄的儿童请注意选择年龄单位，默认为岁。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("工作单位：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("填写患者的工作单位，如果无工作单位则可不填写。学生、幼托儿童、工人、干部职员、民工等职业相对应", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("的工作单位设为必填项，其中学生、幼托儿童工作单位填写其所在的学校或托幼机构、民工填写其所工作的工", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("地或工厂。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("联系电话：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("填写患者的联系方式,14岁以下的患儿家长联系电话为必填项。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("病例属于：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("在相应的类别前打√，用于标识病人现住地址与就诊医院所在地区的关系。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("现住地址：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("至少须详细填写到乡镇（街道）。现住址的填写，原则是指病人发病时的居住地，不是户藉所在地址。如献", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("血员不能提供本人现住地址，则填写该采供血机构地址。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("职   业：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("在相应的职业名前打√。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("病例分类：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("在相应的类别前打√。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("需报告“病原携带者”的法定传染病病种包括“霍乱、脊髓灰质炎、艾滋病。非法定报告传染病按照当地", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("相关要求填报。采供血机构报告填写献血员阳性检测结果，病种是HIV时病例分类才能选择阳性监测，别的病种", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("不允许选择。“梅毒”“淋病”的病例分类只能为“实验室诊断病例”和“疑似病例”“尖锐湿疣”“生殖", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("器疱疹”的病例分类只能为“临床诊断病例”和“实验室诊断病例”。乙肝、血吸虫病例须分急性或慢性填写。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("发病日期：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("本次发病日期；病原携带者填初检日期或就诊时间；采供血机构报告填写献血员献血日期。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("诊断日期：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("本次诊断日期；采供血机构报告填写HIV第二次初筛阳性结果检出日期。“诊断时间”的小时设为必填项。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("死亡日期：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("死亡病例或死亡订正时填入。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("疾病名称：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("在作出诊断的病名前打√。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("其他传染病：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("如有，则分别填写病种名称，也可填写不明原因传染病和新发传染病名称。", font3, Brushes.Black, new PointF(pointX += 85, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("订正病名：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("直接填写订正后的病种名称。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("退卡原因： ", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("填写卡片填报不合格的原因。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("报告单位： ", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("填写报告传染病的单位。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("填卡医生： ", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("填卡医生设为必填项。 ", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("填卡日期：", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("填写本卡日期。 ", font3, Brushes.Black, new PointF(pointX += 70, pointY));
            pointY += interval;
            pointX = pointStartX;
            g.DrawString("备    注:", font2, Brushes.Black, new PointF(pointX + 2, pointY));
            g.DrawString("用户可填写一些文字信息，如传染途径、最后确诊非传染病病名等。", font3, Brushes.Black, new PointF(pointX += 70, pointY));
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
            g.DrawString(allSelectItem, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            return pointX + TextRenderer.MeasureText(allSelectItem, m_DefaultFont).Width;
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
    }
}

