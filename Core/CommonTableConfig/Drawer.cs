using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using DrectSoft.Core.CommonTableConfig.CommonNoteUse;

namespace DrectSoft.Core.CommonTableConfig
{
    public class Drawer
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

        public Drawer(Graphics g)
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

        public void Draw(RecordPrintView recordPrintView)
        {
            try
            {
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
                DrawSpecialLine();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawColumns()
        {
            try
            {
                dic_ColumnsList.Clear();
                XmlNode column = CommonMethods.GetElementByTagName("Columns", doc);
                if (column == null || column.ChildNodes.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请检查<Columns>节点");
                    return;
                }
                XmlNodeList columns = column.ChildNodes;
                if (columns.Count > 0)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        GetTotalWidth(columns[i]);
                    }
                    preColumnLeft = (PrintPageSizeWidth - totalWidth) / 2;
                    startX = (PrintPageSizeWidth - totalWidth) / 2;
                    for (int i = 0; i < columns.Count; i++)
                    {
                        ScanColumn(columns[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DrawLines()
        {
            try
            {
                dic_ColumnsBound.Clear();
                Rectangle temp;
                int index = 0;
                int rowHeight = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "rowHeight", doc));
                int rowCount = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "rowCount", doc));
                foreach (KeyValuePair<string, LogicColumn> kv in dic_ColumnsList)
                {
                    index++;
                    temp = new Rectangle(kv.Value.Bound.X, kv.Value.Bound.Bottom, kv.Value.Bound.Width, 0);
                    dic_ColumnsBound.Add(kv.Key, new LogicColumn(temp, kv.Value.HorAlign));
                    temp.Height = rowHeight * rowCount;
                    gra.DrawRectangle(Pens.Black, temp);
                }
                Rectangle rec = new Rectangle(startX, startY, totalWidth, rowHeight);
                for (int i = 0; i < rowCount; i++)
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
                    int x1 = int.Parse(node.Attributes["x1"] == null || node.Attributes["x1"].Value == "" ? "0" : node.Attributes["x1"].Value);
                    int y1 = int.Parse(node.Attributes["y1"] == null || node.Attributes["y1"].Value == "" ? "0" : node.Attributes["y1"].Value);
                    int x2 = int.Parse(node.Attributes["x2"] == null || node.Attributes["x2"].Value == "" ? "0" : node.Attributes["x2"].Value);
                    int y2 = int.Parse(node.Attributes["y2"] == null || node.Attributes["y2"].Value == "" ? "0" : node.Attributes["y2"].Value);
                    int fontsize = int.Parse(node.Attributes["size"] == null || node.Attributes["size"].Value == "" ? "1" : node.Attributes["size"].Value);
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
                if (node.Attributes["datafield"] != null && node.Attributes["datafield"].Value != "")
                {
                    int w = int.Parse(node.Attributes["width"] == null || node.Attributes["width"].Value == "" ? "0" : node.Attributes["width"].Value);
                    totalWidth += w;
                }
                totalHeight = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "height", doc) == "" ? "22" : CommonMethods.GetElementAttribute("ImageSize", "height", doc));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                string text = node.Attributes["text"].Value;
                string align = node.Attributes["align"].Value;
                gra.DrawRectangle(Pens.Black, x, y, w, h);
                gra.DrawString(text, f, Brushes.Black, new RectangleF(x, y, w, h), sf);
                if (node.Attributes["datafield"].Value != "")
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
                int rowCount = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "rowCount", doc));
                Rectangle rec = new Rectangle(0, 0, 0, 0);
                for (int i = 0; i < printInCommonItemViewList.Count; i++)
                {
                    Type type = printInCommonItemViewList[i].GetType();
                    foreach (KeyValuePair<string, LogicColumn> kv in dic_ColumnsBound)
                    {
                        rec.X = kv.Value.Bound.Left;
                        rec.Y = kv.Value.Bound.Bottom + (i * rowHeight);
                        rec.Width = kv.Value.Bound.Width;
                        rec.Height = rowHeight;
                        PropertyInfo pro = type.GetProperty(kv.Key);
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
                        //-----------------------若【签名】列----------------------
                        object val = pro.GetValue(printInCommonItemViewList[i], null);

                        if (pro.Name.Equals("RecordDoctorImgbyte"))
                        {
                            if (val != null)
                            {
                                byte[] bytes = val as byte[];
                                Bitmap map = new Bitmap(new MemoryStream(bytes));
                                gra.DrawImage(map, rec.X + 1, rec.Y + 1, rec.Width - 2, rec.Height - 2);
                            }
                        }
                        else
                        {
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
                            gra.DrawString(val == null ? "" : val.ToString(), new Font("新宋体", 8, FontStyle.Regular), Brushes.Black, rec, sf);
                        }
                    }
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
                        sf.LineAlignment = StringAlignment.Center;
                    }
                    else
                    {
                        sf.Alignment = StringAlignment.Far;
                        sf.LineAlignment = StringAlignment.Center;
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
                    gra.DrawString(text.Replace("@", pro.GetValue(printInpatientView, null).ToString()), f, Brushes.Black, new Rectangle(x, y, w, h), sf);
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
