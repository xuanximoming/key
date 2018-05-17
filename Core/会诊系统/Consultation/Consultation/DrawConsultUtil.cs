using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Core.Consultation
{
    class DrawConsultUtil
    {

        ConsultationEntity m_ConsultationEntity;


        public DrawConsultUtil(ConsultationEntity entity)
        {
            m_ConsultationEntity = entity;

            InitVariable();
            InitMetaFile();
        }

        int m_PageWidth = 800;//用于设定PictureBox的宽度
        public int PageWidth
        {
            get
            {
                return m_PageWidth;
            }
        }

        int m_PageHeight = 1050;//用于设定PictureBox的高度
        public int PageHeight
        {
            get
            {
                return m_PageHeight;
            }
        }

        float m_PointYTitle = 55; // 标题Y轴方向的值
        Font m_DefaultFont = new Font("宋体", 15f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_DefaultValueFont = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_SmallFont1 = new Font("宋体", 11f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_SmallFont2 = new Font("宋体", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        Font m_SmallFont3 = new Font("宋体", 13f, FontStyle.Regular, GraphicsUnit.Pixel);

        float m_PointXPayType = 40; //“姓名”左上角点的X轴方向的值

        StringFormat sf = new StringFormat();
        StringFormat sfVertical = new StringFormat();

        int m_LineHeight = 40; //每行的高度

        const int m_LineCountBingQin = 7;
        const int m_LineCountPurpos = 4;
        const int m_LineCountResult = 6;

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
        /// 第一页
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
        /// 第一页的地址
        /// </summary>
        public string FilePath2
        {
            get
            {
                return m_FilePath2;
            }
        }

        /// <summary>
        /// 初始化变量
        /// </summary>
        private void InitVariable()
        {
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sfVertical.LineAlignment = StringAlignment.Center;

            sfVertical = new StringFormat();
            sfVertical.Alignment = StringAlignment.Center;
        }

        private void InitMetaFile()
        {
            Bitmap bmp1 = new Bitmap(m_PageWidth, m_PageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g1 = Graphics.FromImage(bmp1);
            Rectangle rect = new Rectangle(0, 0, bmp1.Width, bmp1.Height);
            m_FilePath1 = "C:\\" + Guid.NewGuid().ToString() + ".wmf";
            mf1 = new Metafile(m_FilePath1, g1.GetHdc(), rect, MetafileFrameUnit.Pixel);
            g1 = Graphics.FromImage(mf1);
            g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g1.Clear(Color.White);
            DrawConsultRecord(g1);
            g1.Save();
            g1.Dispose();
            Bitmap bmp2 = new Bitmap(m_PageWidth, m_PageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g2 = Graphics.FromImage(bmp2);

            m_FilePath2 = "C:\\" + Guid.NewGuid().ToString() + ".wmf";
            mf2 = new Metafile(m_FilePath2, g2.GetHdc(), rect, MetafileFrameUnit.Pixel);
            g2 = Graphics.FromImage(mf2);
            g2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g2.Clear(Color.White);
            DrawConsultRecord(g2);
            g2.Save();
            g2.Dispose();
        }

        public void DeleteMetaFile()
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

        //***************************************绘制会诊单*************************************************
        /// <summary>
        /// 绘制会诊单
        /// </summary>
        /// <param name="g"></param>
        /// <returns>是否绘制完毕 true:绘制完成 false:未绘制完</returns>
        private void DrawConsultRecord(Graphics g)
        {
            float pointY = DrawTitle(g) + 70;
            pointY = DrawPatientInfo(g, pointY) + 20;
            pointY = DrawBingQin(g, pointY) + 10;
            pointY = DrawPurpos(g, pointY) + 10;
            pointY = DrawConsultInfo(g, pointY) + 40;
            pointY = DrawResult(g, pointY) + 10;
            DrawBottom(g, pointY);
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        /// <param name="g"></param>
        private float DrawTitle(Graphics g)
        {
            Font font1 = new Font("宋体", 20f, FontStyle.Regular, GraphicsUnit.Pixel);
            Font font2 = new Font("宋体", 25f, FontStyle.Bold, GraphicsUnit.Pixel);
            Font font3 = new Font("宋体", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
            string hospitalName = m_ConsultationEntity.HospitalName;
            //string hospitalName = DrectSoft.MainFrame.PublicClass.getHosName();// 
            g.DrawString(hospitalName, font1, Brushes.Black, new RectangleF(0f, m_PointYTitle, m_PageWidth, 30), sf);
            g.DrawString("会 诊 记 录", font2, Brushes.Black, new RectangleF(0f, m_PointYTitle + 30, m_PageWidth, 35), sf);

            int pointX = Convert.ToInt32(m_PageWidth - 200);
            int pointY = Convert.ToInt32(m_PointYTitle + 45);
            //g.DrawString("紧急度", font3, Brushes.Red, new PointF(pointX, pointY));
            //string urgencyTypeID = m_ConsultationEntity.UrgencyTypeID; //todo
            //pointX = pointX + TextRenderer.MeasureText("紧急度", font3).Width + 5;
            //pointX = DrawCheckBox(g, pointX, pointY, urgencyTypeID, 20);
            //pointX = DrawSelectItem(g, pointX, pointY, "1.男  2.女") + 20;

            //紧急度
            bool isUrgency = false;
            bool isNormal = false;
            if (m_ConsultationEntity.UrgencyTypeID == Convert.ToString((int)UrgencyType.Normal))
            {
                isNormal = true;
            }
            else
            {
                isUrgency = true;
            }
            g.DrawString("紧急度", font3, Brushes.Red, new PointF(pointX, pointY));

            g.DrawRectangle(Pens.Red, new Rectangle(pointX + 60, pointY + 1, 11, 11));
            if (isNormal)
            {
                g.DrawString("√", font3, Brushes.Red, new RectangleF(pointX + 60, pointY + 2, 12, 12), sf);
            }
            g.DrawString("普通", font3, Brushes.Red, new PointF(pointX + 75, pointY));

            g.DrawRectangle(Pens.Red, new Rectangle(pointX + 110, pointY + 1, 11, 11));
            if (isUrgency)
            {
                g.DrawString("√", font3, Brushes.Red, new RectangleF(pointX + 110, pointY + 2, 12, 12), sf);
            }
            g.DrawString("紧急", font3, Brushes.Red, new PointF(pointX + 125, pointY));

            return m_PointYTitle + 25;
        }

        /// <summary>
        /// 病人基本信息
        /// </summary>
        /// <param name="g"></param>
        private float DrawPatientInfo(Graphics g, float pointY)
        {
            int lineHeight = 15;
            float pointX = DrawNameAndValueAndUnderLine(g, m_PointXPayType, pointY, lineHeight, "姓名", m_ConsultationEntity.Name, 65, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "性别", m_ConsultationEntity.SexName, 35, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "年龄", m_ConsultationEntity.Age, 45, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "科别", m_ConsultationEntity.DeptName, 90, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "病区", m_ConsultationEntity.WardName, 50, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "床号", m_ConsultationEntity.Bed, 40, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "住院号", m_ConsultationEntity.PatNoOfHIS, 100, "");
            return pointY + 20;
        }

        private float DrawBingQin(Graphics g, float pointY)
        {
            List<string> list = new List<string>();

            #region 计算需要绘制的每行的数据
            float fontHeight = TextRenderer.MeasureText("测试", m_DefaultValueFont).Height + 5;
            string bingQin = m_ConsultationEntity.Abstract.Replace("\r\n", "");

            Font font;
            string titleName = "病情及治疗情况：";
            list = ComputeNeedDrawStringList(bingQin, out font, m_LineCountBingQin, titleName);
            float width = m_PageWidth - 2 * m_PointXPayType;
            float widthTitle = TextRenderer.MeasureText(titleName, m_DefaultFont).Width + 2;
            g.DrawString(titleName, m_DefaultFont, Brushes.Black, new PointF(m_PointXPayType, pointY));
            #endregion

            #region 绘制下划线
            float pointX = widthTitle + m_PointXPayType;
            float w = width - widthTitle;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY + fontHeight), new PointF(pointX + w - 10, pointY + fontHeight));

            float pointYTemp = pointY;

            for (int i = 0; i < m_LineCountBingQin - 1; i++)
            {
                pointYTemp += m_LineHeight;
                g.DrawLine(Pens.Black, new PointF(m_PointXPayType, pointYTemp + fontHeight), new PointF(m_PointXPayType + width - 10, pointYTemp + fontHeight));
            }
            #endregion

            #region 绘制病情及治疗情况
            if (list.Count > 0)
            {
                g.DrawString(list[0], font, Brushes.Black, new PointF(pointX, pointY));
            }

            if (list.Count > 1)
            {
                for (int i = 1; i < list.Count; i++)
                {
                    if (i > m_LineCountBingQin - 1) break;
                    pointY += m_LineHeight;
                    g.DrawString(list[i], font, Brushes.Black, new PointF(m_PointXPayType, pointY));
                }
            }
            #endregion

            return pointYTemp + m_LineHeight;
        }

        private float DrawPurpos(Graphics g, float pointY)
        {
            List<string> list = new List<string>();

            #region 计算需要绘制的每行的数据
            float fontHeight = TextRenderer.MeasureText("测试", m_DefaultValueFont).Height + 5;
            string purpos = m_ConsultationEntity.Purpose.Replace("\r\n", "");

            Font font;
            string titleName = "申请会诊理由和目的：";
            list = ComputeNeedDrawStringList(purpos, out font, m_LineCountPurpos, titleName);
            float width = m_PageWidth - 2 * m_PointXPayType;
            float widthTitle = TextRenderer.MeasureText(titleName, m_DefaultFont).Width;
            g.DrawString(titleName, m_DefaultFont, Brushes.Black, new PointF(m_PointXPayType, pointY));
            #endregion

            #region 绘制下划线
            float pointX = widthTitle + m_PointXPayType;
            float w = width - widthTitle;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY + fontHeight), new PointF(pointX + w - 10, pointY + fontHeight));

            float pointYTemp = pointY;

            for (int i = 0; i < m_LineCountPurpos - 1; i++)
            {
                pointYTemp += m_LineHeight;
                g.DrawLine(Pens.Black, new PointF(m_PointXPayType, pointYTemp + fontHeight), new PointF(m_PointXPayType + width - 10, pointYTemp + fontHeight));
            }
            #endregion

            #region 绘制病情及治疗情况
            if (list.Count > 0)
            {
                g.DrawString(list[0], font, Brushes.Black, new PointF(pointX, pointY));
            }

            if (list.Count > 1)
            {
                for (int i = 1; i < list.Count; i++)
                {
                    if (i > m_LineCountPurpos - 1) break;
                    pointY += m_LineHeight;
                    g.DrawString(list[i], font, Brushes.Black, new PointF(m_PointXPayType, pointY));
                }
            }
            #endregion

            return pointYTemp + m_LineHeight;
        }

        private float DrawConsultInfo(Graphics g, float pointY)
        {
            int lineHeight = 15;
            float pointX = DrawNameAndValueAndUnderLine(g, m_PointXPayType, pointY, lineHeight, "会诊科室", m_ConsultationEntity.ConsultDeptName, 420, "") + 3;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "申请科室", m_ConsultationEntity.ApplyDeptName, 150, "");

            pointY += m_LineHeight;
            pointX = m_PointXPayType;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "申请医师签名", m_ConsultationEntity.ApplyUserName, 120, "") + 80;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "申请时间", m_ConsultationEntity.ApplyYear, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "年", m_ConsultationEntity.ApplyMonth, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "月", m_ConsultationEntity.ApplyDay, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "日", m_ConsultationEntity.ApplyHour, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "时", m_ConsultationEntity.ApplyMinute, 50 - 4, "分");
            return pointY;
        }

        private float DrawResult(Graphics g, float pointY)
        {
            List<string> list = new List<string>();

            #region 计算需要绘制的每行的数据
            float fontHeight = TextRenderer.MeasureText("测试", m_DefaultValueFont).Height + 5;
            string result = m_ConsultationEntity.ConsultSuggestion.Replace("\r\n", "");
            Font font;
            string titleName = "会诊意见及建议：";
            list = ComputeNeedDrawStringList(result, out font, m_LineCountResult, titleName);
            float width = m_PageWidth - 2 * m_PointXPayType;
            float widthTitle = TextRenderer.MeasureText(titleName, m_DefaultFont).Width;
            g.DrawString(titleName, m_DefaultFont, Brushes.Black, new PointF(m_PointXPayType, pointY));
            #endregion

            #region 绘制下划线
            float pointX = widthTitle + m_PointXPayType;
            float w = width - widthTitle;
            g.DrawLine(Pens.Black, new PointF(pointX, pointY + fontHeight), new PointF(pointX + w - 10, pointY + fontHeight));

            float pointYTemp = pointY;

            for (int i = 0; i < m_LineCountResult - 1; i++)
            {
                pointYTemp += m_LineHeight;
                g.DrawLine(Pens.Black, new PointF(m_PointXPayType, pointYTemp + fontHeight), new PointF(m_PointXPayType + width - 10, pointYTemp + fontHeight));
            }
            #endregion

            #region 绘制病情及治疗情况
            if (list.Count > 0)
            {
                g.DrawString(list[0], font, Brushes.Black, new PointF(pointX, pointY));
            }

            if (list.Count > 1)
            {
                for (int i = 1; i < list.Count; i++)
                {
                    if (i > m_LineCountResult - 1) break;
                    pointY += m_LineHeight;
                    g.DrawString(list[i], font, Brushes.Black, new PointF(m_PointXPayType, pointY));
                }
            }
            #endregion

            return pointYTemp + m_LineHeight;
        }

        private void DrawBottom(Graphics g, float pointY)
        {
            int lineHeight = 15;

            float pointX = DrawNameAndValueAndUnderLine(g, m_PointXPayType, pointY, lineHeight, "会诊医院", m_ConsultationEntity.ConsultHospitalName, 172, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "会诊科室", m_ConsultationEntity.ConsultDeptName2, 400, "");

            pointY += m_LineHeight;
            pointX = m_PointXPayType;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "会诊医师签名", m_ConsultationEntity.ConsultUserName, 180, "") + 20;
            //原来的怎么注释了？？？ add by ywk 
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "会诊时间", m_ConsultationEntity.ConsultYear, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "年", m_ConsultationEntity.ConsultMonth, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "月", m_ConsultationEntity.ConsultDay, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "日", m_ConsultationEntity.ConsultHour, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "时", m_ConsultationEntity.ConsultMinute, 50 - 4, "分");
            pointY += m_LineHeight;
            pointX = m_PointXPayType;
            pointX = DrawNameAndValueAndUnderLine(g, m_PointXPayType, pointY, lineHeight, "会诊医院", m_ConsultationEntity.ConsultHospitalName, 172, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "会诊科室", m_ConsultationEntity.ConsultDeptName2, 400, "");

            pointY += m_LineHeight;
            pointX = m_PointXPayType;
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "会诊医师签名", m_ConsultationEntity.ConsultUserName, 180, "") + 20;
            //原来的怎么注释了？？？ add by ywk 
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "会诊时间", m_ConsultationEntity.ConsultYear, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "年", m_ConsultationEntity.ConsultMonth, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "月", m_ConsultationEntity.ConsultDay, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "日", m_ConsultationEntity.ConsultHour, 50 - 4, "");
            pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "时", m_ConsultationEntity.ConsultMinute, 50 - 4, "分");
            //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "会诊时间", "", 50 - 4, "");
            //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "年", "", 50 - 4, "");
            //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "月", "", 50 - 4, "");
            //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "日", "", 50 - 4, "");
            //pointX = DrawNameAndValueAndUnderLine(g, pointX, pointY, lineHeight, "时", "", 50 - 4, "分");
        }

        /// <summary>
        /// 带下划线
        /// Modify by xlb 2013-05-16
        /// 解决显示不全的问题
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
        private float DrawNameAndValueAndUnderLine(Graphics g, float pointX, float pointY, int lineHeight,
            string name, string value, int underLineWidth, string endName)
        {
            g.DrawString(name, m_DefaultFont, Brushes.Black, new PointF(pointX, pointY));
            int widthName = TextRenderer.MeasureText(name, m_DefaultFont).Width;
            int widthValue = underLineWidth;
            pointX = pointX + widthName;

            Font font = ComputeNeedFont(value, underLineWidth);
            int valueLength = TextRenderer.MeasureText(value, font).Width;
            if (valueLength >= underLineWidth)
            {
                g.DrawString(value, font, Brushes.Black, new RectangleF(pointX, pointY - 10, valueLength, lineHeight + 14), sfVertical);
            }
            else
            {
                g.DrawString(value, font, Brushes.Black, new RectangleF(pointX, pointY, widthValue, lineHeight + 2), sf);
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
        /// 计算需要绘制的字体大小以及绘制的行数
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        private Font ComputeNeedFont(string value, int underLineWidth)
        {
            int valueLength = TextRenderer.MeasureText(value, m_DefaultValueFont).Width;
            if (valueLength < underLineWidth)
            {
                return m_DefaultValueFont;
            }
            valueLength = TextRenderer.MeasureText(value, m_SmallFont3).Width;
            if (valueLength < underLineWidth)
            {
                return m_SmallFont3;
            }
            valueLength = TextRenderer.MeasureText(value, m_SmallFont2).Width;
            if (valueLength < underLineWidth)
            {
                return m_SmallFont2;
            }
            return m_SmallFont1;
        }

        /// <summary>
        /// 动态判断输入字符应该使用的字体大小
        /// Modify by xlb 2013-05-22
        /// </summary>
        /// <param name="bingQin"></param>
        /// <param name="font"></param>
        /// <param name="LineCount"></param>
        /// <param name="titleName"></param>
        /// <returns></returns>
        private List<string> ComputeNeedDrawStringList(string bingQin, out Font font, int LineCount, string titleName)
        {
            try
            {
                List<string> list = new List<string>();
                StringBuilder sb = new StringBuilder();
                float width = m_PageWidth - 2 * m_PointXPayType;
                float widthTitle = TextRenderer.MeasureText(titleName, m_DefaultFont).Width;
                float widthTemp = 0;

                string bingQinTemp = bingQin;
                bool isChangeFont = false;//是否改变字体
                Font valueFont = m_DefaultValueFont;
                bool isBreak = false;
                for (int j = 0; j < 4; j++)
                {
                    if (isBreak) break;
                    if (j == 0) valueFont = m_DefaultValueFont;
                    if (j == 1) valueFont = m_SmallFont3;
                    if (j == 2) valueFont = m_SmallFont2;
                    if (j == 3) valueFont = m_SmallFont1;
                    bingQin = bingQinTemp.Replace("\r\n", "");
                    string[] strEnterLst = bingQin.Split('\n');
                    //换行符 edit by cyq 2012-10-19
                    for (int t = 0; t < strEnterLst.Length; t++)//以换行分组遍历
                    {
                        bingQin = strEnterLst[t];
                        //空行自动缩进 edit by cyq 2012-10-19
                        if (string.IsNullOrEmpty(strEnterLst[t]) || strEnterLst[t] == "\r")
                        {
                            continue;
                        }
                        sb = new StringBuilder();
                        for (int i = 0; i < bingQin.Length; i++)
                        {
                            sb.Append(bingQin[i]);//追加文本计算当前文本长度大于一行剩余长度开始截取
                            widthTemp = TextRenderer.MeasureText(sb.ToString(), valueFont).Width;
                            if (list.Count == 0 && widthTemp >= width - widthTitle || list.Count > 0 && widthTemp >= width)
                            {
                                string value = null == sb ? "" : sb.ToString();
                                value = value.Length == 0 ? "" : value.Substring(0, value.Length - 1);
                                list.Add(value);
                                bingQin = bingQin.Substring(value.Length);
                                sb = new StringBuilder();
                                i = 0;
                                if (list.Count >= LineCount)//当集合中行数大于页面行数且字体号没超过定义
                                {
                                    if (j < 3)
                                    {
                                        list = new List<string>();//清空集合
                                        isChangeFont = true;//表示需要改变字体
                                        break;
                                    }
                                    continue;
                                }
                            }
                            else if (i == bingQin.Length - 1)
                            {//当前内容的最后一行 edit by cyq 2012-10-19
                                if (t == strEnterLst.Length - 1)
                                {//所有内容的最后一行 edit by cyq 2012-10-19
                                    string value = sb.ToString();
                                    list.Add(value);
                                    isBreak = true;
                                    isChangeFont = false;
                                    break;
                                }
                                else
                                {
                                    string value = sb.ToString();
                                    list.Add(value);
                                    isChangeFont = false;
                                    continue;
                                }
                            }
                        }
                        if (isChangeFont)
                        {
                            break;
                        }
                    }
                }
                font = valueFont;
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
    }
}
