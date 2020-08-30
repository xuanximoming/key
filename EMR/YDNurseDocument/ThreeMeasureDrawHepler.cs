using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Xml;

namespace DrectSoft.Core.NurseDocument
{
    public class ThreeMeasureDrawHepler
    {
        private Graphics gph;
        public DataLoader dataLoader;//数据记载类
        private LegendIconSource legendIcon;

        public ThreeMeasureDrawHepler(decimal currInpatient, DataLoader _dataLoader)
        {
            try
            {
                dataLoader = _dataLoader;
                ConfigInfo.LoadTableConfig(MethodSet.App, currInpatient);
                dataLoader = _dataLoader;
                legendIcon = new LegendIconSource();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 画图不涉及数据处理
        /// </summary>
        /// <param name="g">画布</param>
        public void DrawBlankTable(Graphics g)
        {
            try
            {
                gph = g;
                int height, width;
                gph.Clear(Color.White);
                gph.SmoothingMode = SmoothingMode.AntiAlias;
                //gph.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                //gph.PixelOffsetMode = PixelOffsetMode.Default;
                gph.CompositingQuality = CompositingQuality.HighQuality;
                gph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle rec = new Rectangle();

                #region 绘制空表格
                for (int i = 0; i < ConfigInfo.m_rowList.Count; i++)
                {
                    rec.X = 0;
                    rec.Y = (int)ConfigInfo.m_rowList[i].startY;
                    rec.Height = (int)ConfigInfo.m_rowList[i].height;
                    for (int j = 0; j < ConfigInfo.m_columnList.Count; j++)
                    {
                        rec.Width = (int)ConfigInfo.m_columnList[j].width;
                        gph.DrawRectangle(Pens.Black, rec);
                        rec.Offset((int)rec.Width, 0);
                    }
                }
                #endregion

                #region 绘制表格行标题
                RectangleF recf = new RectangleF();
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Far;
                Font fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);
                foreach (XmlNode node in ConfigInfo.m_specialNode.ChildNodes)
                {
                    recf.X = Int32.Parse(node.Attributes["x"].Value);
                    recf.Y = Int32.Parse(node.Attributes["y"].Value);
                    recf.Height = Int32.Parse(node.Attributes["height"].Value);
                    recf.Width = Int32.Parse(node.Attributes["width"].Value);
                    gph.DrawString(node.Attributes["text"].Value, fontText, Brushes.Black, recf, sf);
                }
                #endregion

                #region 绘制时段
                XmlNode nodeElement = ConfigInfo.xmlDoc.GetElementsByTagName("HourOfday")[0];
                XmlNodeList childnodelist = nodeElement.ChildNodes;
                int subColCount = Int32.Parse(nodeElement.Attributes["subColumnCount"].Value);//几个时段 6
                int startColumn = Int32.Parse(nodeElement.Attributes["startColumn"].Value);// 开始显示时段的大列的索引 一般为第2列
                int startRow = Int32.Parse(nodeElement.Attributes["startRow"].Value);// 开始显示时段的行的索引 一般为第4行
                int repeatColCount = Int32.Parse(nodeElement.Attributes["columnRepeatCount"].Value.ToString());//几天 一般为7天
                int subColwidth = (int)ConfigInfo.m_columnList[startColumn - 1].width / subColCount;//时段所在区域列宽
                int startX = Int32.Parse(nodeElement.Attributes["startX"].Value);
                int startY = Int32.Parse(nodeElement.Attributes["startY"].Value);
                Font fontHourText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_hourcaptionSize, FontStyle.Regular);
                Rectangle singleColRec = new Rectangle(startX, startY, subColwidth, (int)ConfigInfo.m_rowList[startRow - 1].height);
                sf.LineAlignment = StringAlignment.Center;
                for (int i = 0; i < subColCount * repeatColCount; i++)
                {
                    gph.DrawString(childnodelist[i % subColCount].InnerText.ToLower(), fontHourText, ConfigInfo.GetColorBrush(childnodelist[i % subColCount].Attributes["numberColor"].Value), (float)singleColRec.X + 7, (float)singleColRec.Y + 10, sf);
                    gph.DrawRectangle(Pens.Black, singleColRec);
                    singleColRec.Offset(subColwidth, 0);
                }

                #endregion

                #region 合并单元格
                nodeElement = ConfigInfo.xmlDoc.GetElementsByTagName("GridClear")[0];
                childnodelist = nodeElement.ChildNodes;
                sf.LineAlignment = StringAlignment.Center;
                Font fontRegionText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);
                for (int i = 0; i < childnodelist.Count; i++)
                {
                    width = Int32.Parse(childnodelist[i].Attributes["width"].Value);
                    height = Int32.Parse(childnodelist[i].Attributes["height"].Value);
                    startX = Int32.Parse(childnodelist[i].Attributes["startX"].Value);
                    startY = Int32.Parse(childnodelist[i].Attributes["startY"].Value);
                    gph.FillRectangle(Brushes.White, startX, startY, width, height);
                    gph.DrawRectangle(Pens.Black, startX, startY, width, height);
                    if (childnodelist[i].Attributes["text"].Value != "")
                        gph.DrawString(childnodelist[i].Attributes["text"].Value, fontRegionText, Brushes.Black, new RectangleF(startX, startY, width, height), sf);
                }
                #endregion

                #region 拆分单元格
                nodeElement = ConfigInfo.xmlDoc.GetElementsByTagName("gridDisassembly")[0];
                childnodelist = nodeElement.ChildNodes;
                for (int i = 0; i < childnodelist.Count; i++)
                {
                    width = Int32.Parse(childnodelist[i].Attributes["width"].Value);
                    height = Int32.Parse(childnodelist[i].Attributes["height"].Value);
                    startX = Int32.Parse(childnodelist[i].Attributes["startX"].Value);
                    startY = Int32.Parse(childnodelist[i].Attributes["startY"].Value);
                    int colsCount = Int32.Parse(childnodelist[i].Attributes["colsCount"].Value);
                    int subColWidth = width / colsCount;
                    rec.X = startX;
                    rec.Y = startY;
                    rec.Width = subColWidth;
                    rec.Height = height;
                    for (int j = 0; j < colsCount - 1; j++)
                    {
                        gph.DrawRectangle(Pens.Black, rec);
                        rec.Offset(subColWidth, 0);
                    }
                }
                #endregion

                #region 绘制坐标
                //左侧坐标
                ConfigInfo.dicVerticalCoordinate.Clear();

                nodeElement = ConfigInfo.xmlDoc.GetElementsByTagName("zuobiao")[0];
                childnodelist = nodeElement.ChildNodes;
                int interval = Int32.Parse(nodeElement.Attributes["interval"].Value);
                startX = 0;
                startY = 0;
                recf = new Rectangle();
                sf.LineAlignment = StringAlignment.Far;
                fontHourText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);
                for (int i = 0; i < childnodelist.Count; i++)
                {
                    string fieldName = string.Empty;
                    string caption = string.Empty;

                    startX = Int32.Parse(childnodelist[i].Attributes["startX"].Value);
                    startY = Int32.Parse(childnodelist[i].Attributes["startY"].Value);
                    caption = childnodelist[i].Attributes["text"].Value;
                    fieldName = childnodelist[i].Attributes["datafield"].Value;
                    string[] data = childnodelist[i].InnerText.Split(',');//坐标数值点
                    recf.X = float.Parse(childnodelist[i].Attributes["startX"].Value);
                    recf.Y = float.Parse(childnodelist[i].Attributes["startY"].Value);
                    recf.Width = Int32.Parse(childnodelist[i].Attributes["width"].Value);
                    recf.Height = 0;
                    if (childnodelist[i].Attributes["hide"].Value == "0")//坐标熟悉Hide隐藏设为"0"时 才绘制
                    {
                        for (int j = 0; j < data.Length; j++)
                        {
                            if (j > 0 && j < 2)
                            {
                                recf.Y += ConfigInfo.subMove;
                            }
                            gph.DrawString(data[data.Length - j - 1], fontHourText, ConfigInfo.GetColorBrush(childnodelist[i].Attributes["numberColor"].Value), recf, sf);
                            recf.Offset(0, interval);
                        }
                        recf.Offset(0, 45);
                        gph.DrawString(caption, fontHourText, ConfigInfo.GetColorBrush(childnodelist[i].Attributes["labelColor"].Value), recf, sf);
                    }
                    //保存坐标
                    if (fieldName.Contains(","))
                    {
                        string[] fieldNames = fieldName.Split(',');
                        for (int index = 0; index < fieldNames.Count(); index++)
                        {
                            ConfigInfo.dicVerticalCoordinate.Add(fieldNames[index], new VerticalCoordinate(fieldNames[index], float.Parse(childnodelist[i].Attributes["max"].Value), float.Parse(childnodelist[i].Attributes["min"].Value), float.Parse(childnodelist[i].Attributes["endY"].Value), float.Parse(childnodelist[i].Attributes["startY"].Value), childnodelist[i].Attributes["hide"].Value));
                        }
                    }
                    else
                    {
                        ConfigInfo.dicVerticalCoordinate.Add(fieldName, new VerticalCoordinate(fieldName, float.Parse(childnodelist[i].Attributes["max"].Value), float.Parse(childnodelist[i].Attributes["min"].Value), float.Parse(childnodelist[i].Attributes["endY"].Value), float.Parse(childnodelist[i].Attributes["startY"].Value), childnodelist[i].Attributes["hide"].Value));
                    }
                }
                #endregion

                #region 绘制图例
                ConfigInfo.dicVitalSignsLineColor.Clear();
                ConfigInfo.dicVitalSignsLineType.Clear();
                nodeElement = ConfigInfo.xmlDoc.GetElementsByTagName("legend")[0];
                childnodelist = nodeElement.ChildNodes;
                for (int i = 0; i < childnodelist.Count; i++)
                {
                    startX = Int32.Parse(childnodelist[i].Attributes["startX"].Value);
                    startY = Int32.Parse(childnodelist[i].Attributes["startY"].Value);
                    width = Int32.Parse(childnodelist[i].Attributes["width"].Value);
                    height = Int32.Parse(childnodelist[i].Attributes["height"].Value);
                    rec.Width = width;
                    rec.Height = height;
                    rec.X = startX;
                    rec.Y = startY;

                    if (!childnodelist[i].Attributes["show"].Value.Equals("0"))
                    {
                        gph.DrawString(childnodelist[i].Attributes["text"].Value, fontHourText, Brushes.Black, rec, sf);
                    }
                    string iconName = childnodelist[i].Attributes["datafield"].Value;

                    if (!ConfigInfo.dicVitalSignsLineColor.Keys.Contains(childnodelist[i].Attributes["datafield"].Value))
                        ConfigInfo.dicVitalSignsLineColor.Add(iconName, childnodelist[i].Attributes["linecolor"].Value);//保存线条颜色
                    if (!ConfigInfo.dicVitalSignsLineType.Keys.Contains(childnodelist[i].Attributes["datafield"].Value))
                        ConfigInfo.dicVitalSignsLineType.Add(iconName, childnodelist[i].Attributes["lineType"].Value);

                    if (!childnodelist[i].Attributes["show"].Value.Equals("0"))
                    {
                        if (iconName != "" && iconName == DataLoader.TEMPERATURE)
                        {
                            switch (childnodelist[i].Attributes["text"].Value)
                            {
                                case "口表":
                                    iconName = DataLoader.KOUWEN;
                                    break;
                                case "腋表":
                                    iconName = DataLoader.YEWEN;
                                    break;
                                case "肛表":
                                    iconName = DataLoader.GANGWEN;
                                    break;
                                case "口温":
                                    iconName = DataLoader.KOUWEN;
                                    break;
                                case "腋温":
                                    iconName = DataLoader.YEWEN;
                                    break;
                                case "肛温":
                                    iconName = DataLoader.GANGWEN;
                                    break;
                                default:
                                    iconName = DataLoader.KOUWEN;
                                    break;
                            }
                            if (iconName == DataLoader.KOUWEN)
                                gph.DrawImage(LegendIconSource.dic_legendIcon[iconName], rec.X + 38, rec.Y + 5, 12, 12);
                            else
                                gph.DrawImage(LegendIconSource.dic_legendIcon[iconName], rec.X + 38, rec.Y + 5, 8, 8);
                        }
                        else
                        {
                            gph.DrawImage(LegendIconSource.dic_legendIcon[iconName], rec.X + 38, rec.Y + 5, 8, 8);
                        }
                    }
                    //rec.Offset(0, itemInertval);
                }
                #endregion

                #region 绘制数据网格
                nodeElement = ConfigInfo.xmlDoc.GetElementsByTagName("smalldatagrid")[0];
                int colCount = Int32.Parse(nodeElement.Attributes["colCount"].Value);
                int rowCount = Int32.Parse(nodeElement.Attributes["rowCount"].Value);
                int xInterval = Int32.Parse(nodeElement.Attributes["xInterval"].Value);
                string color = nodeElement == null || nodeElement.Attributes == null || nodeElement.Attributes["color"] == null || nodeElement.Attributes["color"].Value == "" ? "Gray" : nodeElement.Attributes["color"].Value.Trim().ToString();
                ConfigInfo.smallGridWidth = xInterval;
                int yInterval = Int32.Parse(nodeElement.Attributes["yInterval"].Value);
                ConfigInfo.smallGridHeight = yInterval;
                startX = Int32.Parse(nodeElement.Attributes["startX"].Value);
                startY = Int32.Parse(nodeElement.Attributes["startY"].Value);
                ConfigInfo.smallGridBound = new Rectangle(startX, startY, colCount * xInterval, rowCount * yInterval);//保存数据小方格的坐标范围
                for (int i = 0; i < colCount; i++)
                {
                    gph.DrawLine(color.Equals("Gray") == false ? Pens.LightBlue : Pens.Gray, startX + i * xInterval, startY, startX + i * xInterval, startY + rowCount * yInterval);
                }
                for (int i = 0; i < rowCount; i++)
                {
                    gph.DrawLine(color.Equals("Gray") == false ? Pens.LightBlue : Pens.Gray, startX, startY + i * yInterval, startX + colCount * xInterval, startY + i * yInterval);
                }
                #endregion

                #region 二次加工特殊线条
                nodeElement = ConfigInfo.xmlDoc.GetElementsByTagName("specialLineColor")[0];
                childnodelist = nodeElement.ChildNodes;
                int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                float penwidth = 0;
                for (int i = 0; i < childnodelist.Count; i++)
                {
                    x1 = Int32.Parse(childnodelist[i].Attributes["x1"].Value);
                    y1 = Int32.Parse(childnodelist[i].Attributes["y1"].Value);
                    x2 = Int32.Parse(childnodelist[i].Attributes["x2"].Value);
                    y2 = Int32.Parse(childnodelist[i].Attributes["y2"].Value);
                    penwidth = float.Parse(childnodelist[i].Attributes["size"].Value);
                    if (childnodelist[i].Attributes["color"].Value.Equals("红"))
                    {
                        gph.DrawLine(new Pen(Brushes.Red, penwidth), x1, y1, x2, y2);
                    }
                    if (childnodelist[i].Attributes["color"].Value.Equals("黑"))
                    {
                        gph.DrawLine(new Pen(Brushes.Black, penwidth), x1, y1, x2, y2);
                    }
                }
                #endregion

                #region 绘制非数据文本
                nodeElement = ConfigInfo.xmlDoc.GetElementsByTagName("ThemeText")[0];
                childnodelist = nodeElement.ChildNodes;
                for (int i = 0; i < childnodelist.Count; i++)
                {
                    fontHourText = new Font(ConfigInfo.m_fontName, Int32.Parse(childnodelist[i].Attributes["size"].Value), childnodelist[i].Attributes["blod"].Value == "0" ? FontStyle.Regular : FontStyle.Bold);
                    startX = Int32.Parse(childnodelist[i].Attributes["startX"].Value);
                    startY = Int32.Parse(childnodelist[i].Attributes["startY"].Value);
                    rec.Width = Int32.Parse(childnodelist[i].Attributes["width"].Value);
                    rec.Height = Int32.Parse(childnodelist[i].Attributes["height"].Value);
                    rec.X = startX;
                    rec.Y = startY;
                    switch (childnodelist[i].Attributes["align"].Value)
                    {
                        case "left":
                            sf.Alignment = StringAlignment.Near;
                            gph.DrawString(childnodelist[i].Attributes["text"].Value, fontHourText, Brushes.Black, rec, sf);
                            break;
                        case "right":
                            sf.Alignment = StringAlignment.Far;
                            gph.DrawString(childnodelist[i].Attributes["text"].Value, fontHourText, Brushes.Black, rec, sf);
                            break;
                        default:
                            sf.Alignment = StringAlignment.Center;
                            gph.DrawString(childnodelist[i].Attributes["text"].Value, fontHourText, Brushes.Black, rec, sf);
                            break;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绘制数据点
        /// </summary>
        /// <param name="dataList"></param>
        public void FillVitalSignsData(DataCollection dataList)
        {
            try
            {
                string fieldName = dataList.FieldName;//字段名称
                foreach (DataPoint dp in dataList)
                {
                    if (string.IsNullOrEmpty(dp.value.Trim()))//过滤输入多个空格引起画图出错问题
                    {
                        continue;
                    }
                    string iconName = string.Empty;
                    PointF pos = new PointF();
                    pos.X = dp.ToXCoordinate();
                    pos.Y = dp.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[fieldName]);

                    if (fieldName.Equals(DataLoader.TEMPERATURE)) //如是温度
                    {
                        if (float.Parse(dp.value) < 35)
                        {
                            //小于35度，画体温不升标志
                            DataPoint dpmax = new DataPoint();
                            dpmax.value = "35";
                            float maxY = dpmax.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[fieldName]);

                            DataPoint dpmin = new DataPoint();
                            dpmin.value = "34.6";
                            float minY = (int)dpmin.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[fieldName]);
                            Font fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);

                            StringFormat sf = new StringFormat();
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Center;

                            System.Drawing.Drawing2D.AdjustableArrowCap lineCap = new System.Drawing.Drawing2D.AdjustableArrowCap(2, 2, true);
                            Pen redArrowPen = new Pen(Color.Blue, 2);
                            redArrowPen.CustomEndCap = lineCap;

                            gph.DrawLine(redArrowPen, pos.X, maxY, pos.X, minY);
                        }

                        switch (dp.temperatureType)
                        {
                            case 8801:
                                iconName = DataLoader.YEWEN;
                                break;
                            case 8802:
                                iconName = DataLoader.KOUWEN;
                                break;
                            case 8803:
                                iconName = DataLoader.GANGWEN;
                                break;
                            default:
                                iconName = DataLoader.YEWEN;
                                break;
                        }
                        if (iconName == DataLoader.KOUWEN)
                        {
                            gph.FillRectangle(Brushes.White, pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width + 2, ConfigInfo.m_lineIconSize.Height + 2);
                            gph.DrawImage(LegendIconSource.dic_legendIcon[iconName], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width + 2, ConfigInfo.m_lineIconSize.Height + 2);
                        }
                        else
                        {
                            gph.FillRectangle(Brushes.White, pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width, ConfigInfo.m_lineIconSize.Height - 2);
                            gph.DrawImage(LegendIconSource.dic_legendIcon[iconName], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width, ConfigInfo.m_lineIconSize.Height);
                        }
                    }
                    else if (fieldName.Equals(DataLoader.BREATHE))//如是"呼吸"
                    {
                        if (dp.value != null && dp.value.ToLower().Equals("r"))//是呼吸机
                        {
                            gph.FillRectangle(Brushes.White, pos.X - 3, pos.Y - 3, ConfigInfo.m_lineIconSize.Width, ConfigInfo.m_lineIconSize.Height);
                            gph.DrawImage(LegendIconSource.dic_legendIcon[DataLoader._BREATHE], pos.X - 3, pos.Y - 3, ConfigInfo.m_lineIconSize.Width, ConfigInfo.m_lineIconSize.Height);
                        }
                        else
                        {
                            gph.FillRectangle(Brushes.White, pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width, ConfigInfo.m_lineIconSize.Height - 2);
                            gph.DrawImage(LegendIconSource.dic_legendIcon[fieldName], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width, ConfigInfo.m_lineIconSize.Height);
                        }
                    }
                    else
                    {
                        gph.FillRectangle(Brushes.White, pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width, ConfigInfo.m_lineIconSize.Height - 2);
                        gph.DrawImage(LegendIconSource.dic_legendIcon[fieldName], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width, ConfigInfo.m_lineIconSize.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绘制物理升温点
        /// </summary>
        /// <param name="dataList">升温点</param>
        /// <param name="dataList1">体温点</param>
        public void FillVitalSignsData(DataCollection dataList, DataCollection dataList1)
        {
            try
            {
                string fieldName = dataList.FieldName;//字段名称
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].value == "")
                    {
                        continue;
                    }
                    string iconName = string.Empty;
                    PointF pos = new PointF();
                    pos.X = dataList[i].ToXCoordinate();
                    pos.Y = dataList[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[fieldName]);

                    switch (dataList1[i].temperatureType)//当前对应的体温点类型
                    {
                        case 8801:
                            iconName = DataLoader.YEWEN;
                            break;
                        case 8802:
                            iconName = DataLoader.KOUWEN;
                            break;
                        case 8803:
                            iconName = DataLoader.GANGWEN;
                            break;
                        default:
                            iconName = DataLoader.YEWEN;
                            break;
                    }
                    if (iconName == DataLoader.KOUWEN)
                    {
                        gph.DrawImage(LegendIconSource.dic_legendIcon[iconName], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width + 2, ConfigInfo.m_lineIconSize.Height + 2);
                    }
                    else
                    {
                        gph.DrawImage(LegendIconSource.dic_legendIcon[iconName], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width, ConfigInfo.m_lineIconSize.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据体温单配置文件patientstatefilter----breakLinkLine字段判断连线是否断开
        /// modify by ukey 20200816
        /// </summary>
        /// <param name="dp">体温</param>
        /// <returns></returns>
        private bool CheckDataPointFilter(DataPoint dp)
        {
            try
            {
                if (string.IsNullOrEmpty(ConfigInfo.breakLinkLine) || ConfigInfo.breakLinkLine.Equals("0"))
                {
                    return false;
                }
                foreach (KeyValuePair<DataPoint, string> item in ConfigInfo.StateDataListFilter)
                {
                    //同一天且同一时段
                    if (DateTime.Parse(item.Key.date).Date == DateTime.Parse(dp.date).Date && ConfigInfo.GetHourDuringIndex(item.Key.date) == ConfigInfo.GetTimelotIndex(dp.timeslot))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 解决一天没有体征数据单有特殊状态时连线不断开问题
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="preDp"></param>
        /// <returns></returns>
        private bool CheckDataPointFilter(DataPoint dp, DataPoint preDp)
        {
            try
            {
                if (string.IsNullOrEmpty(ConfigInfo.breakLinkLine) || ConfigInfo.breakLinkLine.Equals("0"))
                {
                    return false;
                }
                if (preDp != null)
                {
                    int day = (DateTime.Parse((DateTime.Parse(dp.date)).ToString("yyyy-MM-dd")) - DateTime.Parse((DateTime.Parse(preDp.date)).ToString("yyyy-MM-dd"))).Days;
                    if (day > 1)
                    {
                        foreach (KeyValuePair<DataPoint, string> item in ConfigInfo.StateDataListFilter)
                        {
                            for (int i = 1; i < day; i++)
                            {
                                //同一天且同一时段
                                if (DateTime.Parse(item.Key.date).Date == DateTime.Parse(preDp.date).AddDays(i).Date)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 是否是特殊病人状态
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        private bool CheckSpecialPatientState(DataPoint dp)
        {
            try
            {
                foreach (FilterState item in ConfigInfo.StateValueTextList)
                {
                    //包含此状态
                    if (item.stateName == dp.value && !item.position.Equals("0"))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 垂直连接不同类数据点
        /// </summary>
        /// <param name="dataList1">升温数据点/降温数据点</param>
        /// <param name="dataList2">体温数据点</param>
        /// <param name="color"></param>
        /// <param name="linkNextTemperature">是否连接下一个温度点</param>
        private void LinkDifferentTypeDataPoint(DataCollection dataList1, DataCollection dataList2, Brush color, DashStyle dashStyle)
        {
            try
            {
                Pen pen = new Pen(color, 1);
                pen.DashStyle = dashStyle;
                if (pen.DashStyle == DashStyle.Custom)
                {
                    pen.DashPattern = new float[] { 6, 2 };//设置短划线和空白部分的数组
                }
                for (int i = 0; i < dataList1.Count; i++)
                {
                    if (dataList1[i].value == "" || dataList2[i].value == "") continue;
                    if (dataList1[i].timeslot == dataList2[i].timeslot)
                    {
                        gph.DrawLine(pen, (int)dataList1[i].ToXCoordinate() + ConfigInfo.lineMoveX, (int)dataList1[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[dataList1.FieldName]) + 1, (int)dataList2[i].ToXCoordinate() + ConfigInfo.lineMoveX, dataList2[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[dataList2.FieldName]) + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 脉搏心率连线
        /// 同一时间段有值且脉搏小于心率时
        /// </summary>
        /// <param name="dataListPulse">脉搏点集合</param>
        /// <param name="dataListHeartRate">心率集合</param>
        /// <param name="color">画笔颜色</param>
        /// <param name="dashStyle">画笔虚实样式</param>
        private void LinkPulseAndHeartRate(DataCollection dataListPulse, DataCollection dataListHeartRate, Brush color, DashStyle dashStyle)
        {
            try
            {
                Pen pen = new Pen(color);
                pen.DashStyle = dashStyle;
                for (int i = 0; i < dataListPulse.Count; i++)
                {
                    if (dataListPulse[i].value == "" || dataListHeartRate[i].value == "") continue;
                    if (dataListPulse[i].timeslot == dataListHeartRate[i].timeslot && float.Parse(dataListPulse[i].value) < float.Parse(dataListHeartRate[i].value))
                    {
                        gph.DrawLine(pen, (int)dataListPulse[i].ToXCoordinate() + ConfigInfo.lineMoveX, (int)dataListPulse[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[dataListPulse.FieldName]) + 1, (int)dataListHeartRate[i].ToXCoordinate() + ConfigInfo.lineMoveX, dataListHeartRate[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[dataListHeartRate.FieldName]) + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数据点之间连线
        /// Modify by xlb 2013-05-24 解决空字符引起的异常
        /// </summary>
        /// <param name="dataList">数据集合</param>
        private void LinkDataPoints(DataCollection dataList)
        {
            try
            {
                string filedName = dataList.FieldName;
                Pen pen = ConfigInfo.GetPen(filedName);
                bool temp = false;//该体征点是否需要过滤
                bool _temp = false;//解决一天没有体征数据单有特殊状态时连线不断开问题
                DataPoint preDataPoint = null;
                foreach (DataPoint dp in dataList)
                {
                    temp = CheckDataPointFilter(dp);
                    _temp = CheckDataPointFilter(dp, preDataPoint);
                    if (_temp) //解决一天没有体征数据单有特殊状态时连线不断开问题
                    {
                        preDataPoint = null;
                    }
                    if (temp)  //该时段存在"外出"等病人状态
                    {
                        dp.value = "";
                        preDataPoint = null;
                    }
                    if (string.IsNullOrEmpty(dp.value.Trim()))
                    {
                        continue;
                    }
                    if (preDataPoint != null)
                    {
                        gph.DrawLine(pen, (int)preDataPoint.ToXCoordinate() + ConfigInfo.lineMoveX, (int)preDataPoint.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[filedName]) + 1, (int)dp.ToXCoordinate() + ConfigInfo.lineMoveX, (int)dp.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[filedName]) + 1);
                    }
                    preDataPoint = dp;
                    if (temp)  //该时段存在"外出"等病人状态
                    {
                        preDataPoint = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 体温,升温点间连线
        /// 解决空字符引起的异常
        /// Add by xlb 2013-05-24
        /// </summary>
        /// <param name="dataList">升温点</param>
        /// <param name="dataList2">体温点</param>
        private void LinkDataPoints(DataCollection dataListHotting, ref DataCollection dataListTemperature)
        {
            try
            {
                string filedName = dataListTemperature.FieldName;
                Pen pen = ConfigInfo.GetColorPen(ConfigInfo.dicVitalSignsLineColor[filedName]);
                if (filedName == DataLoader.HEARTRATE)
                {
                    pen.DashStyle = DashStyle.Dot;
                }
                bool temp = false;//该体征点是否需要过滤
                bool _temp = false;//解决一天没有体征数据单有特殊状态时连线不断开问题
                DataPoint preDataPoint = null;
                for (int i = 0; i < dataListTemperature.Count; i++)
                {
                    temp = CheckDataPointFilter(dataListTemperature[i]);
                    _temp = CheckDataPointFilter(dataListTemperature[i], preDataPoint);
                    if (_temp) //解决一天没有体征数据单有特殊状态时连线不断开问题
                    {
                        preDataPoint = null;
                    }
                    if (temp)  //该时段存在"外出"等病人状态
                    {
                        dataListTemperature[i].value = "";
                        preDataPoint = null;
                    }
                    if (string.IsNullOrEmpty(dataListTemperature[i].value.Trim()))
                    {
                        continue;
                    }
                    if (preDataPoint != null)
                    {
                        gph.DrawLine(pen, (int)preDataPoint.ToXCoordinate() + ConfigInfo.lineMoveX, (int)preDataPoint.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[filedName]) + 1, (int)dataListTemperature[i].ToXCoordinate() + ConfigInfo.lineMoveX, (int)dataListTemperature[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[filedName]) + 1);
                    }
                    preDataPoint = dataListTemperature[i];
                    if (dataListHotting[i].value != "")
                    {
                        preDataPoint = dataListHotting[i];
                    }
                    if (temp)  //该时段存在"外出"等病人状态
                    {
                        preDataPoint = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 脉搏，心率数据点值补空
        /// Modify by xlb 2013-05-30
        /// 解决脉搏和心率连线同时存在虚线和实线情况
        /// </summary>
        /// <param name="pulseDataList"></param>
        /// <param name="heartRateDataList"></param>
        private void FormatePulseHeartRateDataPointsAndLink(DataCollection pulseDataList, DataCollection heartRateDataList)
        {
            try
            {
                LinkPulseAndHeartRate(pulseDataList, heartRateDataList, Brushes.Red, DashStyle.Solid);
                string pulse = pulseDataList.FieldName;
                string heartRate = heartRateDataList.FieldName;
                if (ConfigInfo.pulseHeartRate == 1)
                {
                    //Pen pen = ConfigInfo.GetColorPen(ConfigInfo.dicVitalSignsLineColor[pulse]);
                    //Modify by xlb 2013-05-30 配置项lineType无效 预防出现个性需求控制线条
                    Pen pen = ConfigInfo.GetPen(pulse);
                    bool temp = false;//该体征点是否需要过滤
                    bool _temp = false;//解决一天没有体征数据单有特殊状态时连线不断开问题
                    DataPoint preDataPoint = null;
                    bool prePointRepalce = false;//上一个数据点是否是置换点
                    for (int i = 0; i < pulseDataList.Count; i++)
                    {
                        temp = CheckDataPointFilter(pulseDataList[i]);
                        _temp = CheckDataPointFilter(pulseDataList[i], preDataPoint);
                        if (_temp) //解决一天没有体征数据单有特殊状态时连线不断开问题
                        {
                            preDataPoint = null;
                        }
                        if (temp)  //该时段存在"外出"等病人状态
                        {
                            pulseDataList[i].value = "";//Add by xlb 2013-06-27  特殊状态清空点值
                            preDataPoint = null;//2013-1-5 by tj 
                        }
                        if (string.IsNullOrEmpty(pulseDataList[i].value))//脉搏值为空值，用当前点的心率值充当
                        {
                            if (pulseDataList[i].date == heartRateDataList[i].date && pulseDataList[i].timeslot == heartRateDataList[i].timeslot)
                            {
                                if (string.IsNullOrEmpty(heartRateDataList[i].value))
                                {
                                    continue;
                                }
                                pulseDataList[i].value = heartRateDataList[i].value;
                                //脉搏点连线
                                if (temp)  //该时段存在"外出"等病人状态
                                {
                                    pulseDataList[i].value = "";
                                }
                                if (string.IsNullOrEmpty(pulseDataList[i].value))
                                {
                                    continue;
                                }
                                if (preDataPoint != null && prePointRepalce == false)
                                {
                                    gph.DrawLine(pen, (int)preDataPoint.ToXCoordinate() + ConfigInfo.lineMoveX, (int)preDataPoint.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[pulse]) + 1, (int)pulseDataList[i].ToXCoordinate() + ConfigInfo.lineMoveX, (int)pulseDataList[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[pulse]) + 1);
                                }
                                preDataPoint = pulseDataList[i];
                                if (temp)  //该时段存在"外出"等病人状态
                                {
                                    preDataPoint = null;
                                }
                                //---------
                                prePointRepalce = true;
                            }
                        }
                        else
                        {
                            //脉搏点连线
                            if (prePointRepalce == true)
                            {
                                prePointRepalce = false;
                            }
                            _temp = CheckDataPointFilter(pulseDataList[i]);
                            temp = CheckDataPointFilter(pulseDataList[i], preDataPoint);
                            if (_temp) //解决一天没有体征数据单有特殊状态时连线不断开问题
                            {
                                preDataPoint = null;
                            }
                            if (temp)  //该时段存在"外出"等病人状态
                            {
                                pulseDataList[i].value = "";
                                preDataPoint = null;
                            }
                            if (string.IsNullOrEmpty(pulseDataList[i].value))
                            {
                                continue;
                            }
                            if (preDataPoint != null && prePointRepalce == false)
                            {
                                gph.DrawLine(pen, (int)preDataPoint.ToXCoordinate() + ConfigInfo.lineMoveX, (int)preDataPoint.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[pulse]) + 1, (int)pulseDataList[i].ToXCoordinate() + ConfigInfo.lineMoveX, (int)pulseDataList[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[pulse]) + 1);
                            }
                            preDataPoint = pulseDataList[i];
                            if (temp)  //该时段存在"外出"等病人状态
                            {
                                preDataPoint = null;
                            }
                            //---------
                            prePointRepalce = false;
                        }
                    }
                }
                //-------------------------------------------------------------------------
                if (ConfigInfo.heartRatePulse == 1)
                {
                    Pen pen = ConfigInfo.GetPen(heartRate);//MOdify by xlb 2013-05-30
                    bool temp = false;//该体征点是否需要过滤
                    bool _temp = false;//解决一天没有体征数据单有特殊状态时连线不断开问题
                    DataPoint preDp = null;
                    bool _prePointRepalce = false;//上一个数据点是否是置换点
                    for (int i = 0; i < heartRateDataList.Count; i++)
                    {
                        temp = CheckDataPointFilter(heartRateDataList[i]);
                        _temp = CheckDataPointFilter(pulseDataList[i], preDp);
                        if (_temp) //解决一天没有体征数据单有特殊状态时连线不断开问题
                        {
                            preDp = null;
                        }
                        if (temp)  //该时段存在"外出"等病人状态
                        {
                            preDp = null;
                        }
                        if (string.IsNullOrEmpty(heartRateDataList[i].value))//心率值为空值，用当前点的脉搏值充当
                        {
                            if (string.IsNullOrEmpty(pulseDataList[i].value))
                            {
                                continue;
                            }
                            if (pulseDataList[i].date == heartRateDataList[i].date && pulseDataList[i].timeslot == heartRateDataList[i].timeslot)
                            {
                                heartRateDataList[i].value = pulseDataList[i].value;
                                //脉搏点连线

                                if (temp)  //该时段存在"外出"等病人状态
                                {
                                    heartRateDataList[i].value = "";
                                }
                                if (string.IsNullOrEmpty(heartRateDataList[i].value))
                                {
                                    continue;
                                }
                                if (preDp != null && _prePointRepalce == false)
                                {
                                    //pen.DashStyle = DashStyle.Custom;
                                    //pen.DashPattern = new float[] { 6, 2 };
                                    gph.DrawLine(pen, (int)preDp.ToXCoordinate() + ConfigInfo.lineMoveX, (int)preDp.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[heartRate]) + 1, (int)heartRateDataList[i].ToXCoordinate() + ConfigInfo.lineMoveX, (int)heartRateDataList[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[heartRate]) + 1);
                                }
                                preDp = heartRateDataList[i];
                                if (temp)  //该时段存在"外出"等病人状态
                                {
                                    preDp = null;
                                }
                                //---------
                                _prePointRepalce = true;
                            }
                        }
                        else
                        {
                            if (_prePointRepalce == true)
                            {
                                _prePointRepalce = false;
                            }
                            //心率点连线
                            temp = CheckDataPointFilter(heartRateDataList[i]);
                            _temp = CheckDataPointFilter(heartRateDataList[i], preDp);
                            if (_temp)//解决一天没有体征数据单有特殊状态时连线不断开问题
                            {
                                preDp = null;
                            }
                            if (temp)  //该时段存在"外出"等病人状态
                            {
                                heartRateDataList[i].value = "";
                                preDp = null;
                            }
                            if (string.IsNullOrEmpty(heartRateDataList[i].value))
                            {

                                continue;
                            }
                            if (preDp != null && _prePointRepalce == false)
                            {
                                //pen.DashStyle = DashStyle.Custom;
                                //pen.DashPattern = new float[] { 6, 2 };
                                pen = ConfigInfo.GetPen(heartRate);
                                gph.DrawLine(pen, (int)preDp.ToXCoordinate() + ConfigInfo.lineMoveX, (int)preDp.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[heartRate]) + 1, (int)heartRateDataList[i].ToXCoordinate() + ConfigInfo.lineMoveX, (int)heartRateDataList[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[heartRate]) + 1);
                            }
                            preDp = heartRateDataList[i];
                            if (temp)  //该时段存在"外出"等病人状态
                            {
                                preDp = null;
                            }
                            //---------
                            _prePointRepalce = false;
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void FillNullForPulseAndHeartrate(DataCollection datalistMain, DataCollection dataList)
        {
            try
            {
                Pen pen = ConfigInfo.GetColorPen(ConfigInfo.dicVitalSignsLineColor[datalistMain.FieldName]);
                bool temp = false;//该体征点是否需要过滤
                bool _temp = false;//解决一天没有体征数据单有特殊状态时连线不断开问题
                DataPoint preDataPoint = null;
                bool prePointRepalce = false;//上一个数据点是否是置换点
                for (int i = 0; i < datalistMain.Count; i++)
                {
                    if (string.IsNullOrEmpty(datalistMain[i].value))//脉搏值为空值，用当前点的心率值充当
                    {
                        if (datalistMain[i].date == dataList[i].date && datalistMain[i].timeslot == dataList[i].timeslot)
                        {
                            if (string.IsNullOrEmpty(dataList[i].value)) continue;
                            datalistMain[i].value = dataList[i].value;
                            //脉搏点连线
                            temp = CheckDataPointFilter(datalistMain[i]);
                            _temp = CheckDataPointFilter(datalistMain[i], preDataPoint);
                            if (_temp)
                            {
                                preDataPoint = null;
                            }
                            if (temp)  //该时段存在"外出"等病人状态
                            {
                                datalistMain[i].value = "";
                                preDataPoint = null;
                            }
                            if (string.IsNullOrEmpty(datalistMain[i].value))
                            {
                                continue;
                            }
                            if (preDataPoint != null && prePointRepalce == false)
                            {
                                gph.DrawLine(pen, (int)preDataPoint.ToXCoordinate() + ConfigInfo.lineMoveX, (int)preDataPoint.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[datalistMain.FieldName]) + 1, (int)datalistMain[i].ToXCoordinate() + ConfigInfo.lineMoveX, (int)datalistMain[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[datalistMain.FieldName]) + 1);
                            }
                            preDataPoint = datalistMain[i];
                            if (temp)  //该时段存在"外出"等病人状态
                            {
                                preDataPoint = null;
                            }
                            //---------
                            prePointRepalce = true;
                        }
                    }
                    else
                    {
                        //脉搏点连线
                        if (prePointRepalce == true)
                        {
                            prePointRepalce = false;
                        }
                        temp = CheckDataPointFilter(datalistMain[i]);
                        _temp = CheckDataPointFilter(datalistMain[i], preDataPoint);
                        if (_temp)
                        {
                            preDataPoint = null;
                        }
                        if (temp)  //该时段存在"外出"等病人状态
                        {
                            datalistMain[i].value = "";
                            preDataPoint = null;
                        }
                        if (string.IsNullOrEmpty(datalistMain[i].value))
                        {
                            continue;
                        }
                        if (preDataPoint != null && prePointRepalce == false)
                        {
                            gph.DrawLine(pen, (int)preDataPoint.ToXCoordinate() + ConfigInfo.lineMoveX, (int)preDataPoint.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[datalistMain.FieldName]) + 1, (int)datalistMain[i].ToXCoordinate() + ConfigInfo.lineMoveX, (int)datalistMain[i].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[datalistMain.FieldName]) + 1);
                        }
                        preDataPoint = datalistMain[i];
                        if (temp)  //该时段存在"外出"等病人状态
                        {
                            preDataPoint = null;
                        }
                        //---------
                        prePointRepalce = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绘制非体征数据点
        /// </summary>
        /// <param name="dataList">一周数据</param>
        private void FillData(DataCollection dataList)
        {
            try
            {
                string fieldName = dataList.FieldName;//字段名称
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                RectangleF recf = ConfigInfo.dicDataFieldStartPositin[fieldName].StartBound;

                bool updown = ConfigInfo.dicDataFieldStartPositin[fieldName].updown; ;//控制文本下交替,默认第一个数值居上
                Font fontText;
                if (ConfigInfo.dicDataFieldStartPositin[fieldName].FontSize == 0)
                {
                    fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);
                }
                else  //此行绘制文本大小单独定义
                {
                    fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.dicDataFieldStartPositin[fieldName].FontSize, FontStyle.Regular);
                }
                foreach (DataPoint dp in dataList)
                {
                    //------------次处处理监测呼吸时使用呼吸机的情况，呼吸值用“®”表示
                    if (dataList.FieldName != null && dataList.FieldName.Equals(DataLoader.BREATHE))
                    {

                        if (dp.value.ToLower().Equals("r"))
                        {
                            fontText = new Font(ConfigInfo.m_fontName, 12, FontStyle.Regular);
                            dp.value = "®";
                        }
                        else
                        {
                            fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.dicDataFieldStartPositin[fieldName].FontSize, FontStyle.Regular);
                        }
                    }
                    //---------------------------------------------------------------------

                    //------------如果只有一个血压 无论上午下午均居中显示 同时去除竖线
                    if (dataList.FieldName != null && dataList.FieldName.Equals(DataLoader.BLOODPRESSURE))
                    {
                        string[] bloodPressure = dp.value.Split('|');
                        if (bloodPressure.Count() == 2)
                        {
                            if (bloodPressure[0].ToString().Trim() == "" && bloodPressure[1].ToString().Trim() == "")
                            {
                                dp.value = "";
                            }
                            else if (bloodPressure[0].ToString().Trim() == "" && bloodPressure[1].ToString().Trim() != "")
                            {
                                dp.value = bloodPressure[1].ToString().Trim();
                            }
                            else if (bloodPressure[0].ToString().Trim() != "" && bloodPressure[1].ToString().Trim() == "")
                            {
                                dp.value = bloodPressure[0].ToString().Trim();
                            }
                            else
                            {
                                dp.value = dp.value;
                            }
                        }

                    }
                    //---------------------------------------------------------------------

                    Brush brush = ConfigInfo.GetColorBrush(ConfigInfo.dicDataFieldStartPositin[fieldName].textColor);
                    if (ConfigInfo.dicSpecialRowCellDisplay.Keys.Contains(fieldName)) //显示值是否需要特殊处理 这里处理文本颜色
                    {
                        if (dp.value.Contains(ConfigInfo.dicSpecialRowCellDisplay[fieldName].ValueKey))
                            brush = ConfigInfo.GetColorBrush(ConfigInfo.dicSpecialRowCellDisplay[fieldName].TextColor);
                        fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.dicSpecialRowCellDisplay[fieldName].TextSize, FontStyle.Regular);
                    }
                    if (ConfigInfo.dicDataFieldStartPositin[fieldName].DataShowType == 0) //此行按时段数据显示
                    {
                        recf.Offset(dp.GetOffsetDays() * ConfigInfo.m_columnList[1].width + (ConfigInfo.m_columnList[1].width / ConfigInfo.m_subHourCountOfDay) * (ConfigInfo.GetTimelotIndex(dp.timeslot) - 1), 0);
                        recf.Width = ConfigInfo.m_columnList[1].width / ConfigInfo.m_subHourCountOfDay;
                        if (ConfigInfo.dicDataFieldStartPositin[fieldName].VerAlign.ToString() == "1") //水平上下交替
                        {
                            //  bool updown = ConfigInfo.dicDataFieldStartPositin[fieldName].updown;
                            if (string.IsNullOrEmpty(dp.value))
                            {
                                updown = !updown;
                            }
                            if (updown)
                            {
                                sf.LineAlignment = StringAlignment.Near;
                            }
                            else
                            {
                                sf.LineAlignment = StringAlignment.Far;
                            }
                            updown = !updown;
                        }
                        else
                        {
                            sf.LineAlignment = StringAlignment.Center;
                        }
                        gph.DrawString(dp.value, fontText, brush, recf, sf);
                        recf.Offset(-(dp.GetOffsetDays() * ConfigInfo.m_columnList[1].width + (ConfigInfo.m_columnList[1].width / ConfigInfo.m_subHourCountOfDay) * (ConfigInfo.GetTimelotIndex(dp.timeslot) - 1)), 0);
                    }
                    else
                    {
                        recf.Offset(dp.GetOffsetDays() * (ConfigInfo.m_columnList[1].width), 0);
                        recf.Width = ConfigInfo.m_columnList[1].width;
                        gph.DrawString(dp.value, fontText, brush, recf, sf);
                        recf.Offset(-dp.GetOffsetDays() * (ConfigInfo.m_columnList[1].width), 0);
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 绘制日期
        /// </summary>
        private void FillDate()
        {
            try
            {
                DataTable dt = dataLoader.GetDateTimeForColumns(dataLoader.m_currentDate);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                Font fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);
                RectangleF recf = ConfigInfo.dicDataFieldStartPositin[DataLoader.DATEOFSURVEY].StartBound;
                Brush brush = ConfigInfo.GetColorBrush(ConfigInfo.dicDataFieldStartPositin[DataLoader.DATEOFSURVEY].textColor);
                recf.Width = ConfigInfo.m_columnList[1].width;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    gph.DrawString(dt.Rows[i][1].ToString(), fontText, brush, recf, sf);
                    recf.Offset(ConfigInfo.m_columnList[1].width, 0);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 绘制带有占位符的数据  如 病人信息
        /// </summary>
        private void FillVocateData()
        {
            try
            {
                DataRow dr = null;
                DataTable dt = dataLoader.GetPatientInfoForThreeMeasureTable();
                if (dt.Rows.Count < 1)
                    return;
                else
                    dr = dt.Rows[0];

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                Font fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);
                VocateText obj = null;
                string temp = string.Empty;
                foreach (KeyValuePair<string, VocateText> pair in ConfigInfo.dicVocateValue)
                {
                    if (dr["isbaby"].ToString().Equals("1"))
                    {
                        dr["PATID"] = dataLoader.GetMotherPatid(dr["mother"].ToString());
                        MethodSet.PatID = dr["PATID"].ToString(); ;
                    }
                    obj = pair.Value;
                    if (obj.Caption.Contains("@"))
                    {
                        temp = obj.Caption.Replace("@", dr[pair.Key].ToString());
                    }
                    gph.DrawString(temp, fontText, Brushes.Black, new Rectangle(obj.X, obj.Y, obj.Width, obj.Height), sf);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
        private DataCollection FillEventData(DataCollection dataList)
        {
            try
            {
                DataCollection specialPatientStateDataPoints = new DataCollection();//存储特殊的病人状态事件点

                string fieldName = dataList.FieldName;//字段名称
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Near;
                Font fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);
                RectangleF recf = new RectangleF();
                recf.Y = ConfigInfo.m_eventYStart;
                recf.Height = ConfigInfo.m_eventYEnd - ConfigInfo.m_eventYStart;
                recf.X = ConfigInfo.smallGridBound.X;
                int hourDuringIndex = 0;//时段
                DataPoint preEventDataPoint = null;//保存上一个事件状态数据点
                int temphourDuringIndex = -1;//保存上一个点的时段
                StringBuilder sb = new StringBuilder("");//用于连接状态事件文本
                int index = 0; //累加计数器
                bool specialState = false;
                foreach (DataPoint dp in dataList)
                {
                    //收集特殊病人状态事件点 add by tj 2012-12-24 15:46
                    index++;
                    specialState = CheckSpecialPatientState(dp);
                    if (specialState)
                    {
                        specialPatientStateDataPoints.Add(dp);
                        if (index < dataList.Count)
                        {
                            continue;
                        }
                    }
                    //---------------------------------------------------
                    hourDuringIndex = ConfigInfo.GetHourDuringIndex(dp.date) - 1;//一天中时段序号  ;减1的目的是为了绘图
                    bool isShowtime = true, isIntercept = true;
                    foreach (FilterState item in ConfigInfo.StateValueTextList)
                    {
                        if (item.stateName == dp.value)
                        {
                            if (item.ShowTime.Equals("0"))
                            {
                                isShowtime = false;
                            }
                            if (item.Intercept.Equals("1"))
                            {
                                isIntercept = false;
                            }
                        }
                    }
                    if (isIntercept)
                        hourDuringIndex += 1;
                    if (preEventDataPoint != null)
                    {
                        //与上一个的数据点比较 如果不是同时段 或者 不是同一天 清空暂存数据
                        if (DateTime.Parse(preEventDataPoint.date).Date != DateTime.Parse(dp.date).Date || temphourDuringIndex != hourDuringIndex)
                        {
                            recf.Offset(preEventDataPoint.GetOffsetDays_1() * (ConfigInfo.m_columnList[1].width) + ConfigInfo.smallGridWidth * temphourDuringIndex, 0);
                            recf.Width = ConfigInfo.smallGridWidth;
                            string showcontent1 = string.Empty;
                            #region Modified by wwj 2013-07-25 解决事件名称的颜色与Config.xml文件中的设置不符的情况
                            //二次更改此处根据dp取颜色是不对的 by ywk 2013年9月13日 11:49:09
                            if (sb.ToString().Contains('|'))
                            {
                                showcontent1 = sb.ToString().Split('|')[0];
                            }
                            gph.DrawString(sb.ToString(), fontText, ConfigInfo.GetColorByStateName(showcontent1), recf, sf);
                            #endregion

                            recf.Offset(-(preEventDataPoint.GetOffsetDays_1() * (ConfigInfo.m_columnList[1].width) + ConfigInfo.smallGridWidth * temphourDuringIndex), 0);

                            sb.Remove(0, sb.Length);//若是次时段，清空
                            temphourDuringIndex = -1;
                        }
                    }
                    recf.Offset(dp.GetOffsetDays_1() * (ConfigInfo.m_columnList[1].width) + ConfigInfo.smallGridWidth * hourDuringIndex, 0);
                    recf.Width = ConfigInfo.smallGridWidth;

                    //保存当前值
                    preEventDataPoint = dp;
                    temphourDuringIndex = hourDuringIndex;

                    string showcontent = string.Empty;
                    if (index == dataList.Count && specialState)
                    {
                        showcontent = sb.ToString();
                        //画颜色根据状态进行查询，add by ywk 2013年7月29日 11:10:50
                        if (sb.ToString().Contains('|'))
                        {
                            showcontent = showcontent.Split('|')[0];
                        }
                        gph.DrawString(sb.ToString(), fontText, ConfigInfo.GetColorByStateName(showcontent), recf, sf);
                        return specialPatientStateDataPoints;
                    }

                    if (isShowtime)
                    {
                        sb.Append(dp.value + "|" + MethodSet.NumerricTimeToString(dp.date) + "\0");
                    }
                    else
                    {
                        sb.Append(dp.value + "\0");
                    }

                    if (index == dataList.Count)
                    {

                        //add byy ywk 2013年7月24日 15:04:02 传入状态名称获取颜色
                        gph.DrawString(sb.ToString(), fontText, ConfigInfo.GetColorByStateName(dp.value), recf, sf);

                        return specialPatientStateDataPoints;
                    }
                    recf.Offset(-(dp.GetOffsetDays_1() * (ConfigInfo.m_columnList[1].width) + ConfigInfo.smallGridWidth * hourDuringIndex), 0);
                }
                return specialPatientStateDataPoints;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillSpecialEventData(DataCollection dataList)
        {
            try
            {
                string fieldName = dataList.FieldName;//字段名称
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Near;
                Font fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);
                RectangleF recf = new RectangleF();
                recf.Y = ConfigInfo.m_specialEventYStart;
                recf.Height = ConfigInfo.m_specialEventYEnd - ConfigInfo.m_specialEventYStart;
                recf.X = ConfigInfo.smallGridBound.X;
                int hourDuringIndex = 0;//时段
                DataPoint preEventDataPoint = null;//保存上一个事件状态数据点
                int temphourDuringIndex = -1;//保存上一个点的时段
                StringBuilder sb = new StringBuilder("");//用于连接状态事件文本
                int index = 0; //累加计数器
                foreach (DataPoint dp in dataList)
                {
                    index++;
                    hourDuringIndex = ConfigInfo.GetHourDuringIndex(dp.date) - 1;//一天中时段序号  ;减1的目的是为了绘图
                    if (preEventDataPoint != null)
                    {
                        //与上一个的数据点比较 如果不是同时段 或者 不是同一天 清空暂存数据
                        if (DateTime.Parse(preEventDataPoint.date).Date != DateTime.Parse(dp.date).Date || temphourDuringIndex != hourDuringIndex)
                        {
                            recf.Offset(preEventDataPoint.GetOffsetDays_1() * (ConfigInfo.m_columnList[1].width) + ConfigInfo.smallGridWidth * temphourDuringIndex, 0);
                            recf.Width = ConfigInfo.smallGridWidth;
                            //add by ywk 2013年4月17日9:53:42  
                            string[] states = sb.ToString().Split('\0');
                            Rectangle itemRec = new Rectangle();//记录每个事件状态项的区域
                            itemRec.X = (int)recf.X; itemRec.Width = (int)recf.Width; itemRec.Y = (int)recf.Y; itemRec.Height = (int)recf.Height;
                            int preStateRecHeight = 0;//上一个事件项的文本高度
                            foreach (string item in states)
                            {
                                foreach (FilterState items in ConfigInfo.StateValueTextList)
                                {
                                    if (item.Equals(items.stateName))
                                    {
                                        //SizeF textSize=gph.MeasureString(item.ToString(), fontText);//文本所占画布的区域大小
                                        itemRec.Y += preStateRecHeight;
                                        gph.DrawString(item.ToString(), fontText, ConfigInfo.GetColorByStateName(items.stateName), itemRec, sf);
                                        preStateRecHeight = (item.Length + 1) * (int)ConfigInfo.smallGridHeight;
                                    }
                                }
                            }
                            recf.Offset(-(preEventDataPoint.GetOffsetDays_1() * (ConfigInfo.m_columnList[1].width) + ConfigInfo.smallGridWidth * temphourDuringIndex), 0);

                            sb.Remove(0, sb.Length);//若是次时段，清空
                            temphourDuringIndex = -1;
                        }
                    }

                    recf.Offset(dp.GetOffsetDays_1() * (ConfigInfo.m_columnList[1].width) + ConfigInfo.smallGridWidth * hourDuringIndex, 0);
                    recf.Width = ConfigInfo.smallGridWidth;

                    //保存当前值
                    preEventDataPoint = dp;
                    temphourDuringIndex = hourDuringIndex;
                    bool isShowtime = true;
                    foreach (FilterState item in ConfigInfo.StateValueTextList)
                    {
                        if (item.stateName == dp.value)
                        {
                            if (item.ShowTime.Equals("0"))
                            {
                                isShowtime = false;
                            }
                        }
                    }
                    if (isShowtime)
                    {
                        sb.Append(dp.value + "|" + MethodSet.NumerricTimeToString(dp.date) + "\0");
                    }
                    else
                    {
                        sb.Append(dp.value + "\0");
                    }

                    if (index == dataList.Count)
                    {
                        //add by ywk 2013年4月17日9:53:42  
                        foreach (FilterState item in ConfigInfo.StateValueTextList)
                        {
                            gph.DrawString(sb.ToString(), fontText, ConfigInfo.GetColorByStateName(dp.value), recf, sf);
                        }
                    }
                    recf.Offset(-(dp.GetOffsetDays_1() * (ConfigInfo.m_columnList[1].width) + ConfigInfo.smallGridWidth * hourDuringIndex), 0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 画页号
        /// </summary>
        private void DrawPageIndex()
        {
            try
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                Font fontText = new Font(ConfigInfo.m_fontName, ConfigInfo.m_captionSize, FontStyle.Regular);
                gph.DrawString((DataLoader.WeekIndex + 1).ToString(), fontText, Brushes.Black, ConfigInfo.m_pageBound, sf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 解决桑植医院特殊状态断开时不连线录入值画点需求
        /// Modify by xlb 2013-06-24
        /// </summary>
        /// <param name="g"></param>
        public void DrawDataImage(Graphics g)
        {
            try
            {
                DrawBlankTable(g);//绘制空表

                DataTable dt = dataLoader.GetDateTimeForColumns(dataLoader.m_currentDate);
                DateTime dte = Convert.ToDateTime(dt.Rows[0][0].ToString());
                DateTime endDte = dte.AddDays(ConfigInfo.m_Days - 1);

                //绘制病人状态事件
                DateTime endDteStates = endDte.AddDays(1);
                //endDte = endDte.AddDays(1);   xll 绘制数据时会绘制8天的数据 调整为 病人状态的加1天 原有不变
                DataCollection VitalSignsDataPointsDaily = new DataCollection(); //天数据点集合
                //DataCollection patientState = dataLoader.GetPatStates(dte, endDte);
                DataCollection patientState = dataLoader.GetPatStates(dte, endDteStates, dataLoader.CurrentPat.ToString());
                ConfigInfo.StateDataListFilter.Clear();
                foreach (DataPoint dp in patientState)
                {
                    foreach (FilterState fs in ConfigInfo.StateValueTextList)
                    {
                        if (fs.stateName.Equals(dp.value) && fs.BreakLine.Equals("1"))
                        {
                            ConfigInfo.StateDataListFilter.Add(dp, fs.position);
                        }

                    }
                }
                DataCollection dataList = FillEventData(patientState);
                if (dataList.Count > 0)
                {
                    FillSpecialEventData(dataList);
                }

                #region 体征数据
                dataLoader.GetNursingRecordByDate(dte.ToShortDateString(), endDte.ToShortDateString());
                //--------------------------------------------------加载体征数据--------------------------------------------------
                DataCollection VitalSignsDataPointsTEMPERATURE = null;//温度点集合
                DataCollection VitalSignsDataPointsPHYSICALHOTTING = null;//升温点集合
                DataCollection VitalSignsDataPointsPHYSICALCOOLING = null;//降温点集合
                DataCollection VitalSignsDataPointsBREATH = null;//呼吸点集合
                DataCollection VitalSignsDataPointsHEARTRATE = null;//心率点集合
                DataCollection VitalSignsDataPointsPULSE = null;//脉搏点集合

                if (MainNursingMeasure.m_showTempareture && ConfigInfo.dicVerticalCoordinate[DataLoader.TEMPERATURE].hide == "0")//显示体温线
                {
                    VitalSignsDataPointsTEMPERATURE = dataLoader.GetVitalSignsDataPoints(DataLoader.TEMPERATURE);//体温
                    FillVitalSignsData(VitalSignsDataPointsTEMPERATURE);//绘制点                   
                    /*物理降温点集合*/
                    VitalSignsDataPointsPHYSICALCOOLING = dataLoader.GetVitalSignsDataPoints(DataLoader.PHYSICALCOOLING);//物理降温
                    FillVitalSignsData(VitalSignsDataPointsPHYSICALCOOLING);
                    Brush brush = null;
                    /*通过配置决定体温物理降温连线画笔颜色*/
                    brush = ConfigInfo.GetColorBrush(ConfigInfo.temperatureChangedNode.Attributes["lineColorDown"] == null ? "红" : ConfigInfo.temperatureChangedNode.Attributes["lineColorDown"].Value);
                    /*降温点体温点连线*/
                    LinkDifferentTypeDataPoint(VitalSignsDataPointsPHYSICALCOOLING/*物理降温点集合*/, VitalSignsDataPointsTEMPERATURE/*温度点集合*/, brush/*画笔颜色*/, DashStyle.Custom);
                    /*物理升温点集合*/
                    VitalSignsDataPointsPHYSICALHOTTING = dataLoader.GetVitalSignsDataPoints(DataLoader.PHYSICALHOTTING);//物理升温
                    /*绘制物理升温点*/
                    FillVitalSignsData(VitalSignsDataPointsPHYSICALHOTTING, VitalSignsDataPointsTEMPERATURE);

                    brush = ConfigInfo.GetColorBrush(ConfigInfo.temperatureChangedNode.Attributes["lineColorUp"] == null ? "蓝" : ConfigInfo.temperatureChangedNode.Attributes["lineColorUp"].Value);
                    //------Add by xlb 2013-06-24解决桑植断开时录入值则正常画图标-------------------------------
                    LinkDifferentTypeDataPoint(VitalSignsDataPointsPHYSICALHOTTING, VitalSignsDataPointsTEMPERATURE, brush, DashStyle.Custom);
                    bool linkNextTemperature = ConfigInfo.temperatureChangedNode.Attributes["linkNextTemperature"] == null ? false : ConfigInfo.temperatureChangedNode.Attributes["linkNextTemperature"].Value == "1" ? true : false;
                    if (linkNextTemperature)
                    {
                        //绘制体温点和升温连线
                        LinkDataPoints(VitalSignsDataPointsPHYSICALHOTTING, ref VitalSignsDataPointsTEMPERATURE);
                    }
                    else
                    {
                        LinkDataPoints(VitalSignsDataPointsTEMPERATURE);//体温点连线
                    }
                }
                //-------------------------------显示呼吸-----------------------------------
                if (MainNursingMeasure.m_showBreath && ConfigInfo.dicVerticalCoordinate[DataLoader.BREATHE].hide == "0")
                {
                    VitalSignsDataPointsBREATH = dataLoader.GetVitalSignsDataPoints(DataLoader.BREATHE);
                    FillVitalSignsData(VitalSignsDataPointsBREATH);
                    LinkDataPoints(VitalSignsDataPointsBREATH);
                }
                //--------------------------------显示脉搏 心率---------------------------------------
                if (MainNursingMeasure.m_showPulse && ConfigInfo.dicVerticalCoordinate[DataLoader.PULSE].hide == "0")
                {
                    VitalSignsDataPointsPULSE = dataLoader.GetVitalSignsDataPoints(DataLoader.PULSE);
                    //-- FillVitalSignsData(VitalSignsDataPointsPULSE);

                    VitalSignsDataPointsHEARTRATE = dataLoader.GetVitalSignsDataPoints(DataLoader.HEARTRATE);
                    if (ConfigInfo.PluseAndHeartRate.Equals("1"))
                    {
                        FillVitalSignsData(VitalSignsDataPointsHEARTRATE);
                        FillVitalSignsData(VitalSignsDataPointsPULSE);//Add by xlb 2013-05-27
                    }
                    else
                    {
                        FillVitalSignsData(VitalSignsDataPointsPULSE);
                        FillVitalSignsData(VitalSignsDataPointsHEARTRATE);
                    }
                    //-- FillVitalSignsData(VitalSignsDataPointsHEARTRATE);
                    if (ConfigInfo.pulseHeartRate == 0 && ConfigInfo.heartRatePulse == 0)
                    {
                        LinkDataPoints(VitalSignsDataPointsPULSE);
                        LinkDataPoints(VitalSignsDataPointsHEARTRATE);
                    }
                    else
                    {
                        FormatePulseHeartRateDataPointsAndLink(VitalSignsDataPointsPULSE, VitalSignsDataPointsHEARTRATE);
                    }

                }
                else
                {
                    //-------------------------------显示心率---------------------------------------
                    //VitalSignsDataPointsHEARTRATE = dataLoader.GetVitalSignsDataPoints(DataLoader.HEARTRATE);
                    //FillVitalSignsData(VitalSignsDataPointsHEARTRATE);
                    //LinkDataPoints(VitalSignsDataPointsHEARTRATE);
                }
                #endregion

                #region Add by xlb 处理重合点 断开时录入点仍正常绘制故取点没清空前的值
                VitalSignsDataPointsTEMPERATURE = dataLoader.GetVitalSignsDataPoints(DataLoader.TEMPERATURE);//体温
                //脉搏点集合
                VitalSignsDataPointsPULSE = dataLoader.GetVitalSignsDataPoints(DataLoader.PULSE);
                VitalSignsDataPointsBREATH = dataLoader.GetVitalSignsDataPoints(DataLoader.BREATHE);
                #endregion
                ConfigInfo.ReclosingAssemble.Clear();
                int index = 0;
                if (!(VitalSignsDataPointsTEMPERATURE == null))
                {
                    foreach (DataPoint dp in VitalSignsDataPointsTEMPERATURE)
                    {
                        if (!string.IsNullOrEmpty(dp.value.Trim()))
                        {
                            if (!(VitalSignsDataPointsPULSE == null))
                            {
                                if (dp.ToXCoordinate() == VitalSignsDataPointsPULSE[index].ToXCoordinate() &&
                                    dp.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[VitalSignsDataPointsTEMPERATURE.FieldName]) == VitalSignsDataPointsPULSE[index].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[VitalSignsDataPointsPULSE.FieldName]))
                                {
                                    if (!(VitalSignsDataPointsBREATH == null))
                                    {
                                        if (dp.ToXCoordinate() == VitalSignsDataPointsBREATH[index].ToXCoordinate() &&
                                    dp.ToYCoordinate(ConfigInfo.dicVerticalCoordinate[VitalSignsDataPointsTEMPERATURE.FieldName]) == VitalSignsDataPointsBREATH[index].ToYCoordinate(ConfigInfo.dicVerticalCoordinate[VitalSignsDataPointsBREATH.FieldName]))
                                        {
                                            ConfigInfo.ReclosingAssemble.Add(dp, EnumReclos.NursTiWenHuXiMaiBo);
                                        }
                                        else
                                        {
                                            switch (dp.temperatureType.ToString().Trim())
                                            {
                                                case "8801":
                                                    ConfigInfo.ReclosingAssemble.Add(dp, EnumReclos.NursMaiBoTiWenYe);
                                                    break;
                                                case "8802":
                                                    ConfigInfo.ReclosingAssemble.Add(dp, EnumReclos.NursMaiBoTiWenKou);
                                                    break;
                                                case "8803":
                                                    ConfigInfo.ReclosingAssemble.Add(dp, EnumReclos.NursMaiBoTiWenGang);
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        switch (dp.temperatureType.ToString().Trim())
                                        {
                                            case "8801":
                                                ConfigInfo.ReclosingAssemble.Add(dp, EnumReclos.NursMaiBoTiWenYe);
                                                break;
                                            case "8802":
                                                ConfigInfo.ReclosingAssemble.Add(dp, EnumReclos.NursMaiBoTiWenKou);
                                                break;
                                            case "8803":
                                                ConfigInfo.ReclosingAssemble.Add(dp, EnumReclos.NursMaiBoTiWenGang);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        index++;
                    }
                }
                DrawReclosing();
                //加载非体征数据-天数据
                foreach (KeyValuePair<string, DataFieldRowConfigInfo> pair in ConfigInfo.dicDataFieldStartPositin)
                {
                    if (pair.Key == DataLoader.DATEOFSURVEY) continue;//日期单独处理
                    try
                    {
                        VitalSignsDataPointsDaily = dataLoader.GetDailyDataPoints(pair.Key);
                        FillData(VitalSignsDataPointsDaily);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                //绘制带有占位符的信息  如 病人信息
                FillVocateData();
                //绘制日期
                FillDate();
                //绘制页号
                DrawPageIndex();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        //二次加工重合点
        public void DrawReclosing()
        {
            try
            {
                foreach (DataPoint dp in ConfigInfo.ReclosingAssemble.Keys)
                {
                    PointF pos = new PointF();
                    pos.X = dp.ToXCoordinate();
                    pos.Y = dp.ToYCoordinate(ConfigInfo.dicVerticalCoordinate["TEMPERATURE"]);
                    switch (ConfigInfo.ReclosingAssemble[dp])
                    {
                        case EnumReclos.NursMaiBoTiWenGang:
                            gph.DrawImage(LegendIconSource.dic_legendIcon["NursMaiBoTiWenGang"], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width + 2, ConfigInfo.m_lineIconSize.Height + 2);
                            break;
                        case EnumReclos.NursMaiBoTiWenKou:
                            gph.DrawImage(LegendIconSource.dic_legendIcon["NursMaiBoTiWenKou"], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width + 2, ConfigInfo.m_lineIconSize.Height + 2);
                            break;
                        case EnumReclos.NursMaiBoTiWenYe:
                            gph.DrawImage(LegendIconSource.dic_legendIcon["NursMaiBoTiWenYe"], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width + 2, ConfigInfo.m_lineIconSize.Height + 2);
                            break;
                        case EnumReclos.NursTiWenHuXiMaiBo:
                            gph.DrawImage(LegendIconSource.dic_legendIcon["NursTiWenHuXiMaiBo"], pos.X + ConfigInfo.subMoveX, pos.Y - 3, ConfigInfo.m_lineIconSize.Width + 2, ConfigInfo.m_lineIconSize.Height + 2);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}




