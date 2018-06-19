using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace DrectSoft.Core.BirthProcess
{
    /// <summary>
    /// 绘制产程图片
    /// </summary>
    class DrawHepler
    {
        private XmlDocument xmlDoc = new XmlDocument();
        private DataLoader dataLoader;//数据记载类

        public static Rectangle smallGridBound;//数据小方格范围区域
        private static RectangleF m_patientInfoBound;//病人数据显示区域
        private static float smallGridHeight = 0, smallGridWidth = 0;//数据小方格高度，宽度
        public static Bitmap m_dataImage;//显示数据的图片
        public static Size dataIamgeSize;//显示数据的图片的Size
        public static Graphics gph;
        private static float m_eventYStart = 0, m_eventYEnd = 0;//绘制病人状态文本的起始,结束纵坐标

        #region 配置参考信息的集合 如“坐标”“行数据代表的字段”等   Key:表字段名称
        public static Dictionary<string, VerticalCoordinate> dicVerticalCoordinate = new Dictionary<string, VerticalCoordinate>();//纵坐标集合 字段名称->坐标对象
        //public static Dictionary<string, DataFieldRowConfigInfo> dicDataFieldStartPositin = new Dictionary<string, DataFieldRowConfigInfo>();//存放将要绘制的字段值的起点位置坐标
        public static Dictionary<string, string> dicVitalSignsLineColor = new Dictionary<string, string>();//点间连线的颜色
        public static Dictionary<string, string> dicPatientValue = new Dictionary<string, string>();//病人信息字段值

        private List<Column> m_columnList = new List<Column>();
        private List<Row> m_rowList = new List<Row>();
        private Dictionary<int, string[]> dic_HourOfDay = new Dictionary<int, string[]>();//存储时段配置信息
        #endregion

        #region 常量
        public static int m_Days = 7;//每周七天
        private static int subMove = 12;//微调像素
        private const string m_fontName = "新宋体";
        private const float m_captionSize = 8;
        private const float m_hourcaptionSize = 7;
        #endregion

        public DrawHepler()
        {
            Stream strm = Assembly.GetExecutingAssembly().GetManifestResourceStream("DrectSoft.Core.BirthProcess.config_BirthProcess.xml");
            xmlDoc.Load(strm);
            LoadTableCofnig();
        }

        //加载配置数据病保存至内存
        private void LoadTableCofnig()
        {
            XmlNodeList nodeList = null;
            XmlNode parent = null;

            parent = xmlDoc.GetElementsByTagName("Size")[0];
            dataIamgeSize = new Size(Int32.Parse(parent.Attributes["width"].Value), Int32.Parse(parent.Attributes["height"].Value));

            //dicDataFieldStartPositin.Clear();
            dicPatientValue.Clear();
            //----------------------------病人基本信息-------------------------------
            parent = xmlDoc.GetElementsByTagName("patient")[0];
            nodeList = parent.ChildNodes;
            m_patientInfoBound = new Rectangle();
            m_patientInfoBound.X = Int32.Parse(parent.Attributes["startX"].Value);
            m_patientInfoBound.Y = Int32.Parse(parent.Attributes["startY"].Value);
            m_patientInfoBound.Width = Int32.Parse(parent.Attributes["width"].Value);
            m_patientInfoBound.Height = Int32.Parse(parent.Attributes["height"].Value);
            foreach (XmlNode node in nodeList)
            {
                if (!node.Attributes["datafield"].Value.Equals(""))
                {
                    dicPatientValue.Add(node.Attributes["datafield"].Value, node.InnerText);
                }
            }
        }

        private void DrawBlankTable(Size size)
        {
            XmlNodeList nodeList = null;
            XmlNode parent = null;
            int startX, startY;
            RectangleF recf;
            Font textFont = new Font("宋体", 18, FontStyle.Bold);
            Font textFontCaption = new Font("宋体", 10, FontStyle.Regular);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            try
            {
                m_dataImage = new Bitmap(dataIamgeSize.Width, dataIamgeSize.Height); //用于绘制数据表单
                gph = Graphics.FromImage(m_dataImage);
                gph.Clear(Color.White);
                gph.SmoothingMode = SmoothingMode.AntiAlias;
                gph.TextRenderingHint = TextRenderingHint.SystemDefault;
                Rectangle rec = new Rectangle();

                #region 绘制数据网格
                parent = xmlDoc.GetElementsByTagName("smalldatagrid")[0];
                int colCount = Int32.Parse(parent.Attributes["colCount"].Value);
                int rowCount = Int32.Parse(parent.Attributes["rowCount"].Value);
                int xInterval = Int32.Parse(parent.Attributes["xInterval"].Value);
                smallGridWidth = xInterval;
                int yInterval = Int32.Parse(parent.Attributes["yInterval"].Value);
                smallGridHeight = yInterval;
                startX = Int32.Parse(parent.Attributes["startX"].Value);
                startY = Int32.Parse(parent.Attributes["startY"].Value);
                smallGridBound = new Rectangle(startX, startY, colCount * xInterval, rowCount * yInterval);//保存数据小方格的坐标范围
                for (int i = 0; i < colCount; i++)
                {
                    gph.DrawLine(Pens.Gray, startX + i * xInterval, startY, startX + i * xInterval, startY + rowCount * yInterval);
                }
                for (int i = 0; i < rowCount; i++)
                {
                    gph.DrawLine(Pens.Gray, startX, startY + i * yInterval, startX + colCount * xInterval, startY + i * yInterval);
                }
                #endregion

                #region 二次加工特殊线条
                parent = xmlDoc.GetElementsByTagName("specialLineColor")[0];
                nodeList = parent.ChildNodes;
                int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                float penwidth = 0;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    x1 = Int32.Parse(nodeList[i].Attributes["x1"].Value);
                    y1 = Int32.Parse(nodeList[i].Attributes["y1"].Value);
                    x2 = Int32.Parse(nodeList[i].Attributes["x2"].Value);
                    y2 = Int32.Parse(nodeList[i].Attributes["y2"].Value);
                    penwidth = float.Parse(nodeList[i].Attributes["size"].Value);
                    gph.DrawLine(new Pen(GetColorBrush(nodeList[i].Attributes["color"].Value), penwidth), x1, y1, x2, y2);
                }
                #endregion

                #region 绘制非数据文本
                parent = xmlDoc.GetElementsByTagName("ThemeText")[0];
                nodeList = parent.ChildNodes;

                for (int i = 0; i < nodeList.Count; i++)
                {

                    startX = Int32.Parse(nodeList[i].Attributes["startX"].Value);
                    startY = Int32.Parse(nodeList[i].Attributes["startY"].Value);
                    rec.Width = Int32.Parse(nodeList[i].Attributes["width"].Value);
                    rec.Height = Int32.Parse(nodeList[i].Attributes["height"].Value);
                    float fontSize = float.Parse(nodeList[i].Attributes["size"].Value);
                    FontStyle fs = nodeList[i].Attributes["fontStyle"].Value == "0" ? FontStyle.Regular : FontStyle.Bold;//文本是否加粗
                    Font f = new Font("宋体", fontSize, fs);
                    rec.X = startX;
                    rec.Y = startY;
                    //bool textOrientation = nodeList[i].Attributes["orientation"].Value == "0" ? true : false;// true：表示是水平;反之是垂直
                    switch (nodeList[i].Attributes["align"].Value)
                    {
                        case "left":
                            sf.Alignment = StringAlignment.Near;
                            gph.DrawString(nodeList[i].Attributes["text"].Value, f, Brushes.Black, rec, sf);
                            break;
                        case "right":
                            sf.Alignment = StringAlignment.Far;
                            gph.DrawString(nodeList[i].Attributes["text"].Value, f, Brushes.Black, rec, sf);
                            break;
                        default:
                            sf.Alignment = StringAlignment.Center;
                            gph.DrawString(nodeList[i].Attributes["text"].Value, f, Brushes.Black, rec, sf);
                            break;
                    }
                }
            }
            catch (Exception) { }
                #endregion

            #region 绘制坐标
            dicVerticalCoordinate.Clear();

            parent = xmlDoc.GetElementsByTagName("zuobiao")[0];
            nodeList = parent.ChildNodes;
            int interval = Int32.Parse(parent.Attributes["interval"].Value);
            recf = new RectangleF();
            for (int i = 0; i < nodeList.Count; i++)
            {
                bool orientation = nodeList[i].Attributes["orientation"].Value == "0" ? true : false;// true：表示是水平坐标 ;反之是垂直坐标
                startX = Int32.Parse(nodeList[i].Attributes["startX"].Value);
                startY = Int32.Parse(nodeList[i].Attributes["startY"].Value);
                string caption = nodeList[i].Attributes["text"].Value;
                string fieldName = nodeList[i].Attributes["datafield"].Value;
                string[] data = nodeList[i].InnerText.Split(',');//坐标数值点
                if (orientation)//水平坐标
                {
                    recf.X = float.Parse(nodeList[i].Attributes["startX"].Value) + subMove;
                    recf.Y = float.Parse(nodeList[i].Attributes["startY"].Value);
                    recf.Height = Int32.Parse(nodeList[i].Attributes["height"].Value); ;
                    recf.Width = 0;
                    for (int j = 0; j < data.Length; j++)
                    {
                        gph.DrawString(data[j], textFontCaption, GetColorBrush(nodeList[i].Attributes["numberColor"].Value), recf, sf);
                        recf.Offset(interval, 0);
                    }
                }
                else
                {
                    recf.X = float.Parse(nodeList[i].Attributes["startX"].Value);
                    recf.Y = float.Parse(nodeList[i].Attributes["startY"].Value);
                    recf.Width = Int32.Parse(nodeList[i].Attributes["width"].Value);
                    recf.Height = 0;
                    for (int j = 0; j < data.Length; j++)
                    {
                        gph.DrawString(data[data.Length - j - 1], textFontCaption, GetColorBrush(nodeList[i].Attributes["numberColor"].Value), recf, sf);
                        recf.Offset(0, interval);
                    }
                }
                gph.DrawString(caption, textFontCaption, GetColorBrush(nodeList[i].Attributes["labelColor"].Value), recf, sf);

                #region 保存坐标
                if (fieldName.Contains(","))
                {
                    string[] fieldNames = fieldName.Split(',');
                    for (int index = 0; index < fieldNames.Count(); index++)
                    {
                        dicVerticalCoordinate.Add(fieldNames[index], new VerticalCoordinate(fieldNames[index], float.Parse(nodeList[i].Attributes["max"].Value), float.Parse(nodeList[i].Attributes["min"].Value), float.Parse(nodeList[i].Attributes["endY"].Value), float.Parse(nodeList[i].Attributes["startY"].Value), orientation));
                    }
                }
                else
                {
                    dicVerticalCoordinate.Add(fieldName, new VerticalCoordinate(fieldName, float.Parse(nodeList[i].Attributes["max"].Value), float.Parse(nodeList[i].Attributes["min"].Value), float.Parse(nodeList[i].Attributes["endY"].Value), float.Parse(nodeList[i].Attributes["startY"].Value), orientation));
                }
                #endregion
            }
            #endregion
        }


        /// <summary>
        /// 绘制体征数据点
        /// </summary>
        /// <param name="dataList">一周数据</param>
        public void FillVitalSignsData(DataCollection dataList, bool linked)
        {
            try
            {
                //string fieldName = dataList.FieldName;//字段名称
                //DataPoint preDataPoint = null;//保存上一个数据点，用于点间连线
                //foreach (DataPoint dp in dataList)
                //{
                //    if (dp.value == "") continue;
                //    string iconName = string.Empty;
                //    PointF pos = new PointF();
                //    pos.X = dp.ToXCoordinate();
                //    pos.Y = dp.ToYCoordinate(dicVerticalCoordinate[fieldName]);

                //    if (fieldName.Equals(DataLoader.TEMPERATURE)) //如是温度
                //    {
                //        switch (dp.temperatureType)
                //        {
                //            case 8801:
                //                iconName = DataLoader.KOUWEN;
                //                break;
                //            case 8802:
                //                iconName = DataLoader.YEWEN;
                //                break;
                //            case 8803:
                //                iconName = DataLoader.GANGWEN;
                //                break;
                //            default:
                //                iconName = DataLoader.KOUWEN;
                //                break;
                //        }
                //        if (iconName == DataLoader.KOUWEN)
                //            gph.DrawImage(LegendIconSource.dic_legendIcon[iconName], pos.X, pos.Y - 3, 8, 8);
                //        else
                //            gph.DrawImage(LegendIconSource.dic_legendIcon[iconName], pos.X, pos.Y - 3, 6, 6);
                //    }
                //    else
                //    {
                //        gph.DrawImage(LegendIconSource.dic_legendIcon[fieldName], pos.X, pos.Y - 3, 6, 6);
                //    }
                //    if (linked)
                //    {
                //        if (preDataPoint != null)
                //            gph.DrawLine(GetColorPen(dicVitalSignsLineColor[fieldName]), preDataPoint.ToXCoordinate() + 4, preDataPoint.ToYCoordinate(dicVerticalCoordinate[fieldName]) + 2, dp.ToXCoordinate() + 4, dp.ToYCoordinate(dicVerticalCoordinate[fieldName]) + 2);
                //    }
                //    preDataPoint = dp;
                //}
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 绘制非体征数据点
        /// </summary>
        /// <param name="dataList">一周数据</param>
        public void FillData(DataCollection dataList)
        {
            try
            {
                //string fieldName = dataList.FieldName;//字段名称
                //StringFormat sf = new StringFormat();
                //sf.Alignment = StringAlignment.Center;
                //sf.LineAlignment = StringAlignment.Center;
                //Font fontText = new Font("m_fontName", m_captionSize, FontStyle.Regular);
                //RectangleF recf = dicDataFieldStartPositin[fieldName].StartBound;
                //foreach (DataPoint dp in dataList)
                //{
                //    recf.Offset(dp.GetOffsetDays() * (m_columnList[1].width), 0);
                //    recf.Width = m_columnList[1].width;
                //    gph.DrawString(dp.value, fontText, Brushes.Black, recf, sf);
                //    recf.Offset(-dp.GetOffsetDays() * (m_columnList[1].width), 0);
                //}
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 绘制病人数据
        /// </summary>
        public void FillPatientData()
        {
            try
            {
                //DataRow dr = null;
                //DataTable dt = dataLoader.GetPatientInfoForThreeMeasureTable();
                //if (dt.Rows.Count < 1)
                //    return;
                //else
                //    dr = dt.Rows[0];
                //StringBuilder sb = new StringBuilder("");
                //foreach (KeyValuePair<string, string> pair in dicPatientValue)
                //{
                //    string temp = pair.Value;
                //    if (temp.Contains("@"))
                //    {
                //        temp = temp.Replace("@", dr[pair.Key].ToString());
                //    }
                //    sb.Append(temp + "      ");
                //}
                //StringFormat sf = new StringFormat();
                //sf.Alignment = StringAlignment.Near;
                //sf.LineAlignment = StringAlignment.Near;
                //Font fontText = new Font("m_fontName", m_captionSize, FontStyle.Regular);
                //gph.DrawString(sb.ToString(), fontText, Brushes.Black, m_patientInfoBound, sf);
            }
            catch (Exception ex)
            { throw ex; }
        }

        //绘制数据图片
        public void DrawDataImage()
        {
            try
            {
                DrawBlankTable(dataIamgeSize);//绘制空表

                //DataTable dt = dataLoader.GetDateTimeForColumns(DataLoader.m_currentDate);
                //DateTime dte = Convert.ToDateTime(dt.Rows[0][0].ToString());
                //DateTime endDte = dte.AddDays(m_Days - 1);
                //dataLoader.GetNursingRecordByDate(dte.ToShortDateString(), endDte.ToShortDateString());
                ////--------------------------------------------------加载体征数据--------------------------------------------------
                //DataCollection VitalSignsDataPoints = null;
                //if (MainNursingMeasure.m_showTempareture)//显示体温线
                //{
                //    VitalSignsDataPoints = dataLoader.GetVitalSignsDataPoints(DataLoader.TEMPERATURE);//体温
                //    FillVitalSignsData(VitalSignsDataPoints, true);

                //    DataCollection VitalSignsDataPointsForPHYSICALCOOLING = dataLoader.GetVitalSignsDataPoints(DataLoader.PHYSICALCOOLING);//物理降温
                //    FillVitalSignsData(VitalSignsDataPointsForPHYSICALCOOLING, false);
                //    LinkDifferentTypeDataPoint(VitalSignsDataPoints, DataLoader.TEMPERATURE, VitalSignsDataPointsForPHYSICALCOOLING, DataLoader.TEMPERATURE, Brushes.Red);

                //    DataCollection VitalSignsDataPointsForPHYSICALHOTTING = dataLoader.GetVitalSignsDataPoints(DataLoader.PHYSICALHOTTING);//物理升温
                //    FillVitalSignsData(VitalSignsDataPointsForPHYSICALHOTTING, false);
                //    LinkDifferentTypeDataPoint(VitalSignsDataPoints, DataLoader.TEMPERATURE, VitalSignsDataPointsForPHYSICALHOTTING, DataLoader.TEMPERATURE, Brushes.Blue);
                //}
                ////----------------------------------------------------------------------
                //if (MainNursingMeasure.m_showBreath)
                //{
                //    VitalSignsDataPoints = dataLoader.GetVitalSignsDataPoints(DataLoader.BREATHE);
                //    FillVitalSignsData(VitalSignsDataPoints, true);
                //}
                ////-------------------------------------------------
                //DataCollection tempForPulse = null;
                //if (MainNursingMeasure.m_showPulse)
                //{
                //    tempForPulse = dataLoader.GetVitalSignsDataPoints(DataLoader.PULSE);
                //    FillVitalSignsData(tempForPulse, true);
                //}
                //DataCollection tempForHeartRate = dataLoader.GetVitalSignsDataPoints(DataLoader.HEARTRATE);
                //FillVitalSignsData(tempForHeartRate, true);

                //if (MainNursingMeasure.m_showPulse)
                //    LinkDifferentTypeDataPoint(tempForPulse, DataLoader.PULSE, tempForHeartRate, DataLoader.HEARTRATE, Brushes.Red);//心率和脉搏之间连线

                ////绘制病人状态事件
                //endDte = endDte.AddDays(1);
                //DataCollection patientState = dataLoader.GetPatStates(dte, endDte);
                //FillEventData(patientState);

                ////加载非体征数据
                //foreach (KeyValuePair<string, DataFieldRowConfigInfo> pair in ThreeMeasureDrawHepler.dicDataFieldStartPositin)
                //{
                //    if (pair.Key == DataLoader.DATEOFSURVEY) continue;//日期单独处理
                //    try
                //    {
                //        VitalSignsDataPoints = dataLoader.GetDailyDataPoints(pair.Key);
                //        FillData(VitalSignsDataPoints);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw ex;
                //    }
                //}

                ////绘制病人信息
                //FillPatientData();
                ////绘制日期
                //FillDate();
                ////绘制页号
                //DrawPageIndex();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                gph.Dispose();
            }
        }

        private Brush GetColorBrush(string color)
        {
            switch (color)
            {
                case "红":
                    return Brushes.Red;
                case "蓝":
                    return Brushes.Blue;
                case "黑":
                    return Brushes.Black;
                case "绿":
                    return Brushes.Green;
                default:
                    return Brushes.Red;
            }
        }

        private Pen GetColorPen(string color)
        {
            switch (color)
            {
                case "红":
                    return Pens.Red;
                case "蓝":
                    return Pens.Blue;
                case "黑":
                    return Pens.Black;
                case "绿":
                    return Pens.Green;
                default:
                    return Pens.Red;
            }
        }

        //获得时段的Index  1-6
        private int GetHourDuringIndex(string datetime)
        {
            try
            {
                DateTime _datetime = DateTime.Parse(datetime);
                string hour = _datetime.Hour.ToString();
                foreach (KeyValuePair<int, string[]> pair in dic_HourOfDay)
                {
                    if (pair.Value.Contains(hour))
                        return pair.Key;
                }
            }
            catch (Exception ex) { throw ex; }
            return -1;
        }
    }

    public struct Column
    {
        public int index;
        public float width;
    }

    public struct Row
    {
        public int index;
        public float height;
        public float startY;
    }

    /// <summary>
    /// 纵坐标轴
    /// </summary>
    public struct VerticalCoordinate
    {
        public string name;//名称
        public float maxValue;//最小值
        public float minValue;//最小值
        public float startY;//起始纵坐标
        public float endY;//结束纵坐标
        public bool orientation;//坐标方向 true:水平坐标



        public VerticalCoordinate(string _name, float _maxValue, float _minValue, float _startY, float _endY, bool _orientation)
        {
            this.name = _name;
            this.maxValue = _maxValue;
            this.minValue = _minValue;
            this.startY = _startY;
            this.endY = _endY;
            this.orientation = _orientation;
        }
    }
}
