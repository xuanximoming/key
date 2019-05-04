using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>
    /// 针对具有重复列表单的绘制
    /// </summary>
    public class MultiRepeatColumnDrawer
    {
        Dictionary<string, LogicColumn> dic_ColumnsList = new Dictionary<string, LogicColumn>();
        Dictionary<string, LogicColumn> dic_ColumnsBound = new Dictionary<string, LogicColumn>();
        private int totalWidth = 0, totalHeight = 0, startX = 0, startY = 0;
        private float preColumnLeft = 0;
        private Graphics gra = null;
        private int PrintPageSizeWidth = 0;
        private int PrintPageSizeHeight = 0;
        Font f = null;
        private static StringFormat sf = new StringFormat();
        XmlDocument doc = null;
        int horRepeat = 0;
        RecordPrintView recordPrintView = null;

        public MultiRepeatColumnDrawer(Graphics g)
        {
            try
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                gra = g;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 画护理单表格
        /// </summary>
        /// <param name="_recordPrintView">表格内容</param>
        public void Draw(RecordPrintView _recordPrintView)
        {
            try
            {
                recordPrintView = _recordPrintView;
                doc = new XmlDocument();
                doc.Load(recordPrintView.PrintFileName);
                PrintPageSizeWidth = int.Parse(CommonMethods.GetElementAttribute("PrintPageSize", "width", doc));
                PrintPageSizeHeight = int.Parse(CommonMethods.GetElementAttribute("PrintPageSize", "height", doc));
                startY = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "dataRegionStartY", doc));
                f = new Font(CommonMethods.GetElementAttribute("DataValueStyle", "fontName", doc), float.Parse(CommonMethods.GetElementAttribute("DataValueStyle", "fontSize", doc)), FontStyle.Regular);
                DrawColumns();
                DrawLines();
                FillData(recordPrintView.PrintInCommonItemViewList);
                FillTitle();
                FillPatientInfo(recordPrintView.PrintInpatientView);
                FillOtherInfo(recordPrintView.PrintInCommonItemViewOther);
                DrawSpecialLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化Columns部分数据
        /// </summary>
        public void DrawColumns()
        {
            try
            {
                dic_ColumnsList.Clear();
                XmlNode column = CommonMethods.GetElementByTagName("Columns", doc);
                int align = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "align", doc));
                if (column == null || column.ChildNodes.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请检查<Columns>节点");
                    return;
                }
                horRepeat = int.Parse(column.Attributes["horRepeat"] == null || column.Attributes["horRepeat"].Value == "" ? "0" : column.Attributes["horRepeat"].Value);//列水平重复 次数
                XmlNodeList columns = column.ChildNodes;
                for (int i = 0; i < columns.Count; i++)
                {
                    GetTotalWidth(columns[i]);
                }
                if (horRepeat >= 0)
                {
                    totalWidth += totalWidth * horRepeat;
                }
                if (columns.Count > 0)
                {
                    for (int index = 0; index <= horRepeat; index++)
                    {
                        if (align == 1)
                        {
                            preColumnLeft = startX = (PrintPageSizeWidth - totalWidth) / 2;
                        }
                        preColumnLeft += totalWidth / (horRepeat + 1) * index;
                        for (int i = 0; i < columns.Count; i++)
                        {
                            ScanColumn(columns[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 画ImageSize部分
        /// </summary>
        public void DrawLines()
        {
            try
            {
                dic_ColumnsBound.Clear();
                Rectangle temp;
                int rowHeight = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "rowHeight", doc));
                int tablerowCount = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "tableRowcount", doc));
                for (int i = 0; i <= horRepeat; i++)
                {
                    foreach (KeyValuePair<string, LogicColumn> kv in dic_ColumnsList)
                    {
                        temp = new Rectangle(kv.Value.Bound.X + totalWidth / (horRepeat + 1) * i, kv.Value.Bound.Bottom, kv.Value.Bound.Width, 0);
                        if (!dic_ColumnsBound.Keys.Contains(kv.Key))
                        {
                            dic_ColumnsBound.Add(kv.Key, new LogicColumn(temp, kv.Value.HorAlign));
                        }
                        temp.Height = rowHeight * tablerowCount;
                        gra.DrawRectangle(Pens.Black, temp);
                    }
                }
                Rectangle rec = new Rectangle(startX, startY, totalWidth, rowHeight);
                for (int i = 0; i < tablerowCount; i++)
                {
                    rec.Offset(0, rowHeight);
                    gra.DrawLine(Pens.Black, rec.X, rec.Y + (PrintPageSizeHeight - totalHeight) / 2, rec.X + rec.Width, rec.Y + (PrintPageSizeHeight - totalHeight) / 2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawSpecialLine()
        {
            try
            {
                StringFormat sf = new StringFormat();
                XmlNode staticTextNode = CommonMethods.GetElementByTagName("SpecialLines", doc);
                foreach (XmlNode node in staticTextNode.ChildNodes)
                {
                    int x1 = int.Parse(node.Attributes["x1"].Value == "" ? "0" : node.Attributes["x1"].Value);
                    int y1 = int.Parse(node.Attributes["y1"].Value == "" ? "0" : node.Attributes["y1"].Value);
                    int x2 = int.Parse(node.Attributes["x2"].Value == "" ? "0" : node.Attributes["x2"].Value);
                    int y2 = int.Parse(node.Attributes["y2"].Value == "" ? "0" : node.Attributes["y2"].Value);
                    int fontsize = int.Parse(node.Attributes["size"].Value == "" ? "1" : node.Attributes["size"].Value);
                    gra.DrawLine(Pens.Black, x1, y1, x2, y2);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetTotalWidth(XmlNode node)
        {
            try
            {
                if (node.Attributes["datafield"].Value != "")
                {
                    int w = int.Parse(node.Attributes["width"].Value == "" ? "0" : node.Attributes["width"].Value);
                    totalWidth += w;
                }
                totalHeight = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "height", doc));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 开始画Column部分数据
        /// </summary>
        /// <param name="node">XML文档Column节点</param>
        private void ScanColumn(XmlNode node)
        {
            try
            {

                float x;
                x = float.Parse(node.Attributes["offsetXPreCol"].Value) + preColumnLeft;
                preColumnLeft = x;
                float y = float.Parse(node.Attributes["y"].Value) + (PrintPageSizeHeight - totalHeight) / 2;
                float w = float.Parse(node.Attributes["width"].Value);
                float h = float.Parse(node.Attributes["height"].Value);
                gra.DrawRectangle(Pens.Black, x, y, w, h);
                string text = node.Attributes["text"].Value;
                string[] texts = text.Split('-');
                if (string.IsNullOrEmpty(text))
                {
                    text = recordPrintView.PrintInCommonTabView.GetCaptionName(node.Attributes["datafield"].Value);
                }
                int line = int.Parse(node.Attributes["line"].Value);
                string align = node.Attributes["align"].Value;
                switch (line)
                {
                    case 0:
                        switch (align)
                        {
                            case "-1":
                                sf.Alignment = StringAlignment.Near;
                                sf.LineAlignment = StringAlignment.Center;
                                break;
                            case "0":
                            case "":
                                sf.Alignment = StringAlignment.Center;
                                sf.LineAlignment = StringAlignment.Center;
                                break;
                            case "1":
                                sf.Alignment = StringAlignment.Far;
                                sf.LineAlignment = StringAlignment.Center;
                                break;
                        }
                        gra.DrawString(text, f, Brushes.Black, new RectangleF(x, y, w, h), sf);
                        break;
                    case 1:

                        gra.DrawLine(Pens.Black, x, y, x + w, y + h);
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Center;
                        gra.DrawString(texts[0], f, Brushes.Black, new RectangleF(x, y, w, h / 2), sf);

                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
                        gra.DrawString(texts[1], f, Brushes.Black, new RectangleF(x, y + h / 2, w, h / 2), sf);
                        break;
                    case 2:
                        gra.DrawLine(Pens.Black, x + w / 2, y, x + w, y + h);
                        gra.DrawLine(Pens.Black, x, y + h / 2, x + w, y + h);
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Center;
                        gra.DrawString(texts[0], f, Brushes.Black, new RectangleF(x, y, w, h / 3), sf);

                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        gra.DrawString(texts[1], f, Brushes.Black, new RectangleF(x, y + h / 3, w, h / 3), sf);

                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
                        gra.DrawString(texts[2], f, Brushes.Black, new RectangleF(x, y + h / 3 * 2, w, h / 3), sf);
                        break;
                    case -1:
                        gra.DrawLine(Pens.Black, x + w, y, x, y + h);
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
                        gra.DrawString(texts[0], f, Brushes.Black, new RectangleF(x, y, w, h / 2), sf);

                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Center;
                        gra.DrawString(texts[1], f, Brushes.Black, new RectangleF(x, y + h / 2, w, h / 2), sf);
                        break;
                }
                if (node.Attributes["datafield"].Value != "" && !dic_ColumnsList.Keys.Contains(node.Attributes["datafield"].Value))
                {
                    dic_ColumnsList.Add(node.Attributes["datafield"].Value, new LogicColumn(new Rectangle((int)x, (int)y, (int)w, (int)h), align));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FillData(List<PrintInCommonItemView> printInCommonItemViewList)
        {
            try
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                int rowHeight = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "rowHeight", doc));
                int tablerowCount = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "tableRowcount", doc));
                Rectangle rec = new Rectangle(0, 0, 0, 0);
                int cunrRecordIndex = 0;
                for (int i = 0; i < printInCommonItemViewList.Count; i++)//记录条数
                {
                    if (cunrRecordIndex >= tablerowCount)
                    {
                        cunrRecordIndex = 0;
                    }
                    Type type = printInCommonItemViewList[i].GetType();
                    PropertyInfo pro = null;
                    StringBuilder sb = new StringBuilder();//存储多属性值
                    foreach (KeyValuePair<string, LogicColumn> kv in dic_ColumnsBound)
                    {
                        int colGroupIndex = i / tablerowCount;
                        rec.X = kv.Value.Bound.Left + totalWidth / (horRepeat + 1) * colGroupIndex;
                        rec.Y = kv.Value.Bound.Bottom;
                        rec.Offset(0, cunrRecordIndex * rowHeight);
                        rec.Width = kv.Value.Bound.Width;
                        rec.Height = rowHeight;
                        sb.Remove(0, sb.Length);
                        //签名列
                        if (kv.Key.Equals("RecordDoctorImgbyte"))
                        {
                            pro = type.GetProperty(kv.Key);
                            if (pro == null)
                            {
                                continue;
                            }
                            object val = pro.GetValue(printInCommonItemViewList[i], null);
                            if (val != null)
                            {
                                byte[] bytes = val as byte[];
                                Bitmap map = new Bitmap(new MemoryStream(bytes));
                                gra.DrawImage(map, rec.X + 1, rec.Y + 1, rec.Width - 2, rec.Height - 2);
                            }
                        }
                        else  //若此列需要显示多属性值
                        {
                            string[] fields = kv.Key.Split(';');
                            foreach (string k in fields)
                            {
                                pro = type.GetProperty(k);
                                if (pro == null)
                                {
                                    continue;
                                }
                                //----------------------统计-----------------------
                                PropertyInfo proSum = type.GetProperty("IsZongLiang");
                                if (proSum != null)
                                {
                                    object obj = proSum.GetValue(printInCommonItemViewList[i], null);
                                    if (obj != null && obj.ToString() != "" && obj.ToString() != "0")
                                    {
                                        Pen p = null;
                                        switch (obj.ToString())
                                        {
                                            case "1":
                                                p = new Pen(Brushes.Red, 2f);
                                                break;
                                            case "2":
                                                p = new Pen(Brushes.Blue, 2f);
                                                break;
                                        }
                                        gra.DrawRectangle(p, new Rectangle(startX, rec.Y, totalWidth, rec.Height - 1));
                                    }
                                }

                                {
                                    object val = pro.GetValue(printInCommonItemViewList[i], null);
                                    if (val == null)
                                    {
                                        val = "";
                                    }
                                    sb.Append(val.ToString());
                                }
                            }
                        }

                        switch (kv.Value.HorAlign)
                        {
                            case "-1":
                                sf.Alignment = StringAlignment.Near;
                                sf.LineAlignment = StringAlignment.Center;
                                break;
                            case "0":
                            case "":
                                sf.Alignment = StringAlignment.Center;
                                sf.LineAlignment = StringAlignment.Center;
                                break;
                            case "1":
                                sf.Alignment = StringAlignment.Far;
                                sf.LineAlignment = StringAlignment.Center;
                                break;
                        }
                        gra.DrawString(sb == null ? "" : sb.ToString(), new Font("新宋体", 9, FontStyle.Regular), Brushes.Black, rec, sf);
                        rec.Offset(0, -cunrRecordIndex * rowHeight);
                    }
                    cunrRecordIndex++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FillTitle()
        {
            try
            {
                StringFormat sf = new StringFormat();
                XmlNode staticTextNode = CommonMethods.GetElementByTagName("StaticText", doc);
                foreach (XmlNode node in staticTextNode.ChildNodes)
                {
                    int x = int.Parse(node.Attributes["x"].Value == "" ? "0" : node.Attributes["x"].Value);
                    int y = int.Parse(node.Attributes["y"].Value == "" ? "0" : node.Attributes["y"].Value) + (PrintPageSizeHeight - totalHeight) / 2;
                    int w = int.Parse(node.Attributes["width"].Value == "" ? totalWidth.ToString() : node.Attributes["width"].Value);
                    int h = int.Parse(node.Attributes["height"].Value == "" ? "0" : node.Attributes["height"].Value);
                    string text = node.Attributes["text"].Value;
                    int fontsize = int.Parse(node.Attributes["fontsize"].Value == "" ? "0" : node.Attributes["fontsize"].Value);
                    FontStyle fs = node.Attributes["bold"].Value == "1" ? FontStyle.Bold : FontStyle.Regular;
                    if (node.Attributes["align"].Value == "" || node.Attributes["align"].Value == "0")
                    {
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                    }
                    else if (node.Attributes["align"].Value == "-1")
                    {
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Near;
                    }
                    else
                    {
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Far;
                    }
                    Font f = new Font("新宋体", fontsize, fs);
                    gra.DrawString(text, f, Brushes.Black, new Rectangle(x, y, w, h), sf);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FillPatientInfo(PrintInpatientView printInpatientView)
        {
            try
            {
                XmlNode patientInfoNode = CommonMethods.GetElementByTagName("PatientInfo", doc);
                Type type = printInpatientView.GetType();
                PropertyInfo pro = null;
                foreach (XmlNode node in patientInfoNode.ChildNodes)
                {
                    int x = int.Parse(node.Attributes["x"].Value == "" ? "0" : node.Attributes["x"].Value);
                    int y = int.Parse(node.Attributes["y"].Value == "" ? "0" : node.Attributes["y"].Value);
                    int w = int.Parse(node.Attributes["width"].Value == "" ? totalWidth.ToString() : node.Attributes["width"].Value);
                    int h = int.Parse(node.Attributes["height"].Value == "" ? "0" : node.Attributes["height"].Value);
                    string text = node.Attributes["text"].Value;
                    int fontsize = int.Parse(node.Attributes["fontsize"].Value == "" ? "0" : node.Attributes["fontsize"].Value);
                    FontStyle fs = node.Attributes["bold"].Value == "1" ? FontStyle.Bold : FontStyle.Regular;
                    string dataField = node.Attributes["datafield"].Value;
                    pro = type.GetProperty(dataField);
                    Font f = new Font("新宋体", fontsize, fs);
                    string horAlign = node.Attributes["align"].Value;
                    switch (horAlign)
                    {
                        case "-1":
                            sf.Alignment = StringAlignment.Near;
                            sf.LineAlignment = StringAlignment.Center;
                            break;
                        case "0":
                        case "":
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Center;
                            break;
                        case "1":
                            sf.Alignment = StringAlignment.Far;
                            sf.LineAlignment = StringAlignment.Near;
                            break;
                    }
                    if (pro == null || pro.GetValue(printInpatientView, null) == null)
                    {
                        gra.DrawString(text.Replace("@", ""), f, Brushes.Black, new Rectangle(x, y, w, h), sf);
                    }
                    else
                    {
                        gra.DrawString(text.Replace("@", pro.GetValue(printInpatientView, null).ToString()), f, Brushes.Black, new Rectangle(x, y, w, h), sf);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FillOtherInfo(PrintInCommonItemView printInCommonItemViewOther)
        {
            try
            {
                if (printInCommonItemViewOther == null)
                {
                    return;
                }
                XmlNode otherInfoNode = CommonMethods.GetElementByTagName("OtherInfo", doc);
                Type type = printInCommonItemViewOther.GetType();
                PropertyInfo pro = null;
                foreach (XmlNode node in otherInfoNode.ChildNodes)
                {
                    int x = int.Parse(node.Attributes["x"].Value == "" ? "0" : node.Attributes["x"].Value);
                    int y = int.Parse(node.Attributes["y"].Value == "" ? "0" : node.Attributes["y"].Value);
                    int w = int.Parse(node.Attributes["width"].Value == "" ? totalWidth.ToString() : node.Attributes["width"].Value);
                    int h = int.Parse(node.Attributes["height"].Value == "" ? "0" : node.Attributes["height"].Value);
                    string text = node.Attributes["text"].Value;
                    int fontsize = int.Parse(node.Attributes["fontsize"].Value == "" ? "0" : node.Attributes["fontsize"].Value);
                    FontStyle fs = node.Attributes["bold"].Value == "1" ? FontStyle.Bold : FontStyle.Regular;
                    string dataField = node.Attributes["datafield"].Value;
                    pro = type.GetProperty(dataField);
                    Font f = new Font("新宋体", fontsize, fs);
                    string horAlign = node.Attributes["align"].Value;
                    switch (horAlign)
                    {
                        case "-1":
                            sf.Alignment = StringAlignment.Near;
                            sf.LineAlignment = StringAlignment.Center;
                            break;
                        case "0":
                        case "":
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Center;
                            break;
                        case "1":
                            sf.Alignment = StringAlignment.Far;
                            sf.LineAlignment = StringAlignment.Near;
                            break;
                    }
                    if (pro == null || pro.GetValue(printInCommonItemViewOther, null) == null)
                    {
                        gra.DrawString(text.Replace("@", ""), f, Brushes.Black, new Rectangle(x, y, w, h), sf);
                    }
                    else
                    {
                        gra.DrawString(text.Replace("@", pro.GetValue(printInCommonItemViewOther, null).ToString()), f, Brushes.Black, new Rectangle(x, y, w, h), sf);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    struct LogicColumn
    {
        public Rectangle Bound;
        public string HorAlign;

        public LogicColumn(Rectangle _bound, string _horAlign)
        {
            Bound = _bound;
            HorAlign = _horAlign;
        }
    }
}
