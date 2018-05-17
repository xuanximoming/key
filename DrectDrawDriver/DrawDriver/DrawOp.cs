using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.DrawDriver
{
    public class DrawOp
    {
        private static List<DrawObject> drawObjectsList = new List<DrawObject>();

        private static Dictionary<string, DataSet> dic_dataSet = new Dictionary<string, DataSet>();

        private static List<ShowObject> showTypeObject = new List<ShowObject>();

        private static List<Metafile> listPages = null;

        public static int printPageNowPer = 0;

        public static PageSetupDialog pageSetupDialogs = new PageSetupDialog();

        public static bool AddDrawObject(DrawObject _drawobject)
        {
            bool result;
            try
            {
                if (_drawobject == null)
                {
                    result = false;
                }
                else
                {
                    DrawOp.drawObjectsList.Add(_drawobject);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DeleteDrawObject(DrawObject _drawobject)
        {
            bool result;
            try
            {
                if (_drawobject == null || DrawOp.drawObjectsList.Count == 0)
                {
                    result = false;
                }
                else
                {
                    DrawOp.drawObjectsList.Remove(_drawobject);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool SetParamaterValue(string _paramaername, string _value)
        {
            bool result;
            try
            {
                XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname("Parameters");
                if (xmlNodeByTagname == null || xmlNodeByTagname.Length == 0)
                {
                    result = false;
                }
                else
                {
                    XmlNode xmlNode = xmlNodeByTagname[0];
                    XmlCommomOp.SetXmlNodeByParamName(_paramaername, _value);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool SetDataSource(DataSet[] _value)
        {
            bool result;
            try
            {
                DrawOp.dic_dataSet.Clear();
                for (int i = 0; i < _value.Length; i++)
                {
                    DataSet dataSet = _value[i];
                    DrawOp.dic_dataSet.Add(dataSet.DataSetName, dataSet);
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static DataSet[] GetDataSets()
        {
            DataSet[] result;
            try
            {
                result = (from kv in DrawOp.dic_dataSet
                          select kv.Value).ToArray<DataSet>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static DataSet GetAnDataSetByName(string _name)
        {
            DataSet result;
            try
            {
                foreach (KeyValuePair<string, DataSet> current in DrawOp.dic_dataSet)
                {
                    if (current.Equals(_name))
                    {
                        result = current.Value;
                        return result;
                    }
                }
                result = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool MakeAnLocalXml(XmlDocument _doc)
        {
            bool result;
            try
            {
                XmlNode[] array = DrawOp.SearchTableNode(_doc);
                XmlNode[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    XmlNode curNode = array2[i];
                    DrawOp.FormateXMLElementPosition(curNode, 10);
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static void FormateXMLElementPosition(XmlNode curNode, int dataSourceItemsCount)
        {
            try
            {
                int num = int.Parse(curNode.SelectSingleNode("TableHead/Th").Attributes["height"].Value) + int.Parse(curNode.SelectSingleNode("TableBody/Tr").Attributes["height"].Value) * dataSourceItemsCount;
                curNode.Attributes["height"].Value = num.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static XmlNode[] SearchTableNode(XmlDocument _doc)
        {
            XmlNode[] result;
            try
            {
                XmlNode[] xmlNodeByTagname = XmlCommomOp.GetXmlNodeByTagname("Table");
                result = xmlNodeByTagname;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool TranslationXmlDocument(Graphics g, XmlDocument _doc)
        {
            bool result;
            try
            {
                DrawDriverXmlOp.SetRowsAttributeByXmlNode(_doc);
                XmlNodeList childNodes = _doc.SelectSingleNode("PrintDocument/Body").ChildNodes;
                int num = 100;
                foreach (XmlNode parent in childNodes)
                {
                    Stack stack = new Stack();
                    stack.Push(num);
                    DrawOp.TranslationLayout(g, parent, ref stack);
                    num = int.Parse(stack.Peek().ToString());
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static string GetValueByAttributeCurrentToTop(XmlNode _CurrentNode, string _AttributeName)
        {
            string result;
            try
            {
                XmlNode xmlNode = _CurrentNode;
                string valueByAttribute = XmlCommomOp.GetValueByAttribute(xmlNode, _AttributeName);
                if (valueByAttribute != null)
                {
                    result = valueByAttribute;
                }
                else
                {
                    while (xmlNode.ParentNode != null)
                    {
                        if (XmlCommomOp.GetValueByAttribute(xmlNode, _AttributeName) != null)
                        {
                            result = XmlCommomOp.GetValueByAttribute(xmlNode, _AttributeName);
                            return result;
                        }
                        xmlNode = xmlNode.ParentNode;
                    }
                    result = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static string GetParentValueByAttribute(XmlNode _currentNode, string _attributeName)
        {
            string result;
            try
            {
                XmlNode xmlNode = _currentNode;
                while (xmlNode.ParentNode != null)
                {
                    if (XmlCommomOp.GetValueByAttribute(xmlNode.ParentNode, _attributeName) != null)
                    {
                        result = XmlCommomOp.GetValueByAttribute(xmlNode.ParentNode, _attributeName);
                        return result;
                    }
                    xmlNode = xmlNode.ParentNode;
                }
                result = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private static void TranslationLayout(Graphics g, XmlNode _parent, ref Stack _stack)
        {
            try
            {
                if (_parent.HasChildNodes && (_parent.ChildNodes.Count != 1 || _parent.ChildNodes[0].NodeType != XmlNodeType.Text) && !_parent.Name.ToLower().Equals("table"))
                {
                    CommonMethods.InnerXmlTextAddSpan(_parent);
                    if (!_parent.ParentNode.Name.ToLower().Equals("body"))
                    {
                        int num = int.Parse((_stack.Peek() == null) ? "0" : _stack.Peek().ToString());
                        _stack.Push(num);
                    }
                    foreach (XmlNode parent in _parent.ChildNodes)
                    {
                        DrawOp.TranslationLayout(g, parent, ref _stack);
                    }
                }
                else
                {
                    int num2 = int.Parse((_stack.Peek() == null) ? "0" : _stack.Peek().ToString());
                    _stack.Push(num2);
                    string text = _parent.Name.ToLower();
                    if (text != null)
                    {
                        if (!(text == "p") && !(text == "span") && !(text == "i") && !(text == "b") && !(text == "u"))
                        {
                            if (text == "table")
                            {
                                int num3 = int.Parse(_stack.Peek().ToString());
                                int num4 = int.Parse((_parent.Attributes["height"] == null) ? _parent.ParentNode.Attributes["height"].Value : _parent.Attributes["height"].Value);
                                if (_parent.Attributes["top"] == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "top", string.Concat(num3));
                                }
                                if (_parent.Attributes["height"] == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "height", string.Concat(num4));
                                }
                                int num5 = 0;
                                int num6 = 0;
                                if (_parent.Attributes["left"] == null)
                                {
                                    num5 = CommonMethods.StringToInt32(DrawOp.GetValueByAttributeCurrentToTop(_parent, "left"), 20);
                                }
                                if (_parent.Attributes["left"] != null)
                                {
                                    int num7 = CommonMethods.StringToInt32(_parent.Attributes["left"].Value, 0);
                                    num5 += num7;
                                    int num8 = CommonMethods.StringToInt32(_parent.Attributes["left"].Value, 0);
                                }
                                if (_parent.Attributes["width"] == null)
                                {
                                    num6 = CommonMethods.StringToInt32(DrawOp.GetValueByAttributeCurrentToTop(_parent, "width"), 0);
                                }
                                if (_parent.Attributes["left"] == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "left", string.Concat(num5));
                                }
                                if (_parent.Attributes["width"] == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "width", string.Concat(num6));
                                }
                                DrawInfoString drawInfoString = new DrawInfoString();
                                drawInfoString.Left = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Left");
                                drawInfoString.Top = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Top");
                                drawInfoString.Width = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Width");
                                drawInfoString.Height = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Height");
                                drawInfoString.Align = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Align");
                                drawInfoString.Valign = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Valign");
                                drawInfoString.FontColor = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Font");
                                drawInfoString.BgColor = DrawOp.GetValueByAttributeCurrentToTop(_parent, "BgColor");
                                drawInfoString.Border = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Border");
                                if (XmlCommomOp.GetValueByAttribute(_parent, "Align") == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "Align", drawInfoString.Align ?? "");
                                }
                                if (XmlCommomOp.GetValueByAttribute(_parent, "Valign") == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "Valign", drawInfoString.Valign ?? "");
                                }
                                if (XmlCommomOp.GetValueByAttribute(_parent, "FontColor") == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "FontColor", drawInfoString.FontColor ?? "");
                                }
                                if (XmlCommomOp.GetValueByAttribute(_parent, "BgColor") == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "BgColor", drawInfoString.BgColor ?? "");
                                }
                                if (XmlCommomOp.GetValueByAttribute(_parent, "Border") == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "Border", drawInfoString.Border ?? "");
                                }
                            }
                        }
                        else
                        {
                            string innerText = _parent.InnerText;
                            int num3 = CommonMethods.StringToInt32(_stack.Peek().ToString());
                            int num4 = CommonMethods.StringToInt32(DrawOp.GetValueByAttributeCurrentToTop(_parent.ParentNode, "height"), 32);
                            if (_parent.Attributes["top"] == null)
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "top", string.Concat(num3));
                            }
                            if (_parent.Attributes["height"] == null)
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "height", string.Concat(num4));
                            }
                            int num5 = 0;
                            int num6 = 0;
                            if (_parent.Attributes["left"] == null)
                            {
                                num5 = CommonMethods.StringToInt32(DrawOp.GetValueByAttributeCurrentToTop(_parent, "left"), 20);
                            }
                            int num8 = 0;
                            if (_parent.Attributes["left"] != null)
                            {
                                int num7 = CommonMethods.StringToInt32(_parent.Attributes["left"].Value, 0);
                                num5 += num7;
                                num8 = CommonMethods.StringToInt32(_parent.Attributes["left"].Value, 0);
                            }
                            if (_parent.Attributes["width"] == null)
                            {
                                num6 = CommonMethods.StringToInt32(DrawOp.GetValueByAttributeCurrentToTop(_parent, "width"), 0);
                            }
                            if (_parent.Name.ToLower().Equals("p"))
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "width", string.Concat(num6));
                                if (_parent.Attributes["left"] == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "left", string.Concat(num5));
                                }
                            }
                            else if (_parent.Name.ToLower().Equals("span") || _parent.Name.ToLower().Equals("b") || _parent.Name.ToLower().Equals("i") || _parent.Name.ToLower().Equals("u"))
                            {
                                System.Drawing.Font font = new System.Drawing.Font(new FontFamily("新宋体"), 12f, FontStyle.Regular, GraphicsUnit.Pixel);
                                string valueByAttribute = XmlCommomOp.GetValueByAttribute(_parent, "Font");
                                FontColor fontColor;
                                if (valueByAttribute == null)
                                {
                                    fontColor = new FontColor();
                                    fontColor.Font = font;
                                }
                                else
                                {
                                    fontColor = CommonMethods.StringToFontColor(valueByAttribute, null);
                                    if (fontColor == null)
                                    {
                                        fontColor.Font = font;
                                    }
                                }
                                System.Drawing.Font font2 = fontColor.Font;
                                if (_parent.Name.ToLower().Equals("b"))
                                {
                                    font2 = new System.Drawing.Font(fontColor.Font.FontFamily, fontColor.Font.Size, FontStyle.Bold, GraphicsUnit.Pixel);
                                }
                                else if (_parent.Name.ToLower().Equals("i"))
                                {
                                    font2 = new System.Drawing.Font(fontColor.Font.FontFamily, fontColor.Font.Size, FontStyle.Italic, GraphicsUnit.Pixel);
                                }
                                else if (_parent.Name.ToLower().Equals("u"))
                                {
                                    font2 = new System.Drawing.Font(fontColor.Font.FontFamily, fontColor.Font.Size, FontStyle.Underline, GraphicsUnit.Pixel);
                                }
                                fontColor.Font = font2;
                                int num9 = (int)g.MeasureString(innerText, fontColor.Font).Width + 1;
                                XmlCommomOp.SetXmlAttribute(_parent, "width", string.Concat(num9));
                                if (_parent.ParentNode != null)
                                {
                                    int num10 = 0;
                                    if (_parent.ParentNode.Attributes["temp_width"] != null)
                                    {
                                        num10 = CommonMethods.StringToInt32(_parent.ParentNode.Attributes["temp_width"].Value, 0);
                                    }
                                    if (num10 == 0)
                                    {
                                        num9 += num8;
                                    }
                                    if (_parent.Attributes["left"] == null)
                                    {
                                        XmlCommomOp.SetXmlAttribute(_parent, "left", string.Concat(num10));
                                    }
                                    num10 += num9;
                                    XmlCommomOp.SetXmlAttribute(_parent.ParentNode, "temp_width", string.Concat(num10));
                                }
                                else if (_parent.Attributes["left"] == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "left", string.Concat(num5));
                                }
                            }
                            DrawInfoString drawInfoString = new DrawInfoString();
                            drawInfoString.Left = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Left");
                            drawInfoString.Top = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Top");
                            drawInfoString.Width = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Width");
                            drawInfoString.Height = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Height");
                            drawInfoString.Align = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Align");
                            drawInfoString.Valign = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Valign");
                            drawInfoString.FontColor = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Font");
                            drawInfoString.BgColor = DrawOp.GetValueByAttributeCurrentToTop(_parent, "BgColor");
                            drawInfoString.Border = DrawOp.GetValueByAttributeCurrentToTop(_parent, "Border");
                            if (XmlCommomOp.GetValueByAttribute(_parent, "Align") == null)
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "Align", drawInfoString.Align ?? "");
                            }
                            if (XmlCommomOp.GetValueByAttribute(_parent, "Valign") == null)
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "Valign", drawInfoString.Valign ?? "");
                            }
                            if (XmlCommomOp.GetValueByAttribute(_parent, "Font") == null)
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "Font", drawInfoString.FontColor ?? "");
                            }
                            if (XmlCommomOp.GetValueByAttribute(_parent, "BgColor") == null)
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "BgColor", drawInfoString.BgColor ?? "");
                            }
                            if (XmlCommomOp.GetValueByAttribute(_parent, "Border") == null)
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "Border", drawInfoString.Border ?? "");
                            }
                        }
                    }
                    text = _parent.Name.ToLower();
                    if (text != null)
                    {
                        if (!(text == "p") && !(text == "table"))
                        {
                            if (text == "span")
                            {
                                int num11 = int.Parse(_stack.Pop().ToString());
                                if (_parent.ParentNode.Name.ToLower() != "span" && _parent.NextSibling == null)
                                {
                                    _stack.Push(num11 + int.Parse((_parent.Attributes["height"] == null) ? _parent.ParentNode.Attributes["height"].Value : _parent.Attributes["height"].Value));
                                }
                            }
                        }
                        else
                        {
                            int num11 = int.Parse(_stack.Pop().ToString());
                            _stack.Push(num11 + int.Parse(_parent.Attributes["height"].Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DrawDocument(ref List<Metafile> pages, XmlDocument _doc)
        {
            Graphics graphics = null;
            try
            {
                graphics = Graphics.FromImage(pages[0]);
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                DrawDriverXmlOp.SetRowsAttributeByXmlNode(_doc);
                XmlNode xmlNode = _doc.SelectSingleNode("PrintDocument/Body");
                int num = 30;
                int num2 = 0;
                Stack stack = new Stack();
                stack.Push(num);
                Stack stack2 = new Stack();
                foreach (XmlNode xmlNode2 in xmlNode)
                {
                    if (xmlNode2.Name.ToLower().Equals("pageend"))
                    {
                        graphics.Save();
                        graphics.Dispose();
                        Bitmap image = new Bitmap(1446, 876);
                        Graphics graphics2 = Graphics.FromImage(image);
                        Metafile metafile = new Metafile(new MemoryStream(), graphics2.GetHdc());
                        graphics = Graphics.FromImage(metafile);
                        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.Clear(Color.White);
                        pages.Add(metafile);
                        stack.Clear();
                        stack.Push(num);
                        stack2 = new Stack();
                        num2 = 0;
                    }
                    else
                    {
                        DrawOp.DrawLayer(graphics, xmlNode2, ref stack, ref stack2, ref num2);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                graphics.Save();
                graphics.Dispose();
            }
        }

        private static void DrawLayout(Graphics g, XmlNode _parent, ref Stack _stack, string op_type)
        {
            try
            {
                if (_parent.HasChildNodes && (_parent.ChildNodes.Count != 1 || _parent.ChildNodes[0].NodeType != XmlNodeType.Text) && !_parent.Name.ToLower().Equals("table"))
                {
                    if (!_parent.ParentNode.Name.ToLower().Equals("body"))
                    {
                        int num = int.Parse((_stack.Peek() == null) ? "0" : _stack.Peek().ToString());
                        _stack.Push(num);
                    }
                    foreach (XmlNode parent in _parent.ChildNodes)
                    {
                        DrawOp.DrawLayout(g, parent, ref _stack, op_type);
                    }
                }
                else
                {
                    int num2 = int.Parse((_stack.Peek() == null) ? "0" : _stack.Peek().ToString());
                    _stack.Push(num2);
                    FontColor fontColor = new FontColor();
                    fontColor.Color = Color.Red;
                    fontColor.Font = new System.Drawing.Font("新宋体", 10f, FontStyle.Regular);
                    int num3 = 0;
                    int num4 = 0;
                    string text = _parent.Name.ToLower();
                    if (text != null)
                    {
                        if (!(text == "p") && !(text == "span"))
                        {
                            if (!(text == "table"))
                            {
                            }
                        }
                        else
                        {
                            string innerText = _parent.InnerText;
                            int num5 = int.Parse(_stack.Peek().ToString());
                            int num6 = int.Parse((_parent.Attributes["height"] == null) ? _parent.ParentNode.Attributes["height"].Value : _parent.Attributes["height"].Value);
                            if (_parent.Attributes["top"] == null)
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "top", string.Concat(num5));
                            }
                            if (_parent.Attributes["height"] == null)
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "height", string.Concat(num6));
                            }
                            int num7 = 0;
                            int num8 = 0;
                            if (_parent.Attributes["left"] == null)
                            {
                                num7 = CommonMethods.StringToInt32(DrawOp.GetValueByAttributeCurrentToTop(_parent, "left"), 20);
                            }
                            int num9 = 0;
                            if (_parent.Attributes["left"] != null)
                            {
                                int num10 = CommonMethods.StringToInt32(_parent.Attributes["left"].Value, 0);
                                num7 += num10;
                                num9 = CommonMethods.StringToInt32(_parent.Attributes["left"].Value, 0);
                            }
                            if (_parent.Attributes["width"] == null)
                            {
                                num8 = CommonMethods.StringToInt32(DrawOp.GetValueByAttributeCurrentToTop(_parent, "width"), 0);
                            }
                            if (_parent.Name.ToLower().Equals("p"))
                            {
                                XmlCommomOp.SetXmlAttribute(_parent, "width", string.Concat(num8));
                                if (_parent.Attributes["left"] == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "left", string.Concat(num7));
                                }
                            }
                            else if (_parent.Name.ToLower().Equals("span"))
                            {
                                int num11;
                                if (_parent.Attributes["font-size"] != null)
                                {
                                    num11 = CommonMethods.StringToInt32(_parent.Attributes["font-size"].Value.Replace("px", ""), 1);
                                }
                                else
                                {
                                    num11 = 12;
                                }
                                string name;
                                if (_parent.Attributes["font-family"] != null)
                                {
                                    name = _parent.Attributes["font-family"].Value;
                                }
                                else
                                {
                                    name = "新宋体";
                                }
                                System.Drawing.Font font = new System.Drawing.Font(new FontFamily(name), (float)num11, FontStyle.Regular, GraphicsUnit.Pixel);
                                int num12 = (int)g.MeasureString(innerText, font).Width + 1;
                                XmlCommomOp.SetXmlAttribute(_parent, "width", string.Concat(num12));
                                if (_parent.ParentNode != null)
                                {
                                    int num13 = 0;
                                    if (_parent.ParentNode.Attributes["temp_width"] != null)
                                    {
                                        num13 = CommonMethods.StringToInt32(_parent.ParentNode.Attributes["temp_width"].Value, 0);
                                    }
                                    if (num13 == 0)
                                    {
                                        num12 += num9;
                                    }
                                    if (_parent.Attributes["left"] == null)
                                    {
                                        XmlCommomOp.SetXmlAttribute(_parent, "left", string.Concat(num13));
                                    }
                                    num13 += num12;
                                    XmlCommomOp.SetXmlAttribute(_parent.ParentNode, "temp_width", string.Concat(num13));
                                }
                                else if (_parent.Attributes["left"] == null)
                                {
                                    XmlCommomOp.SetXmlAttribute(_parent, "left", string.Concat(num7));
                                }
                            }
                            num7 = ((_parent.Attributes["left"] == null || string.IsNullOrEmpty(_parent.Attributes["left"].Value)) ? 20 : int.Parse(_parent.Attributes["left"].Value));
                            CommonDrawOp.DrawString(g, _parent.InnerText, num7, num5, num8, num6, fontColor, AlignType.Left, ValignType.Middle, out num3, out num4);
                        }
                    }
                    text = _parent.Name.ToLower();
                    if (text != null)
                    {
                        if (!(text == "p") && !(text == "table"))
                        {
                            if (text == "span")
                            {
                                int num14 = int.Parse(_stack.Pop().ToString());
                                if (_parent.ParentNode.Name.ToLower() != "span" && _parent.NextSibling == null)
                                {
                                    _stack.Push(num14 + int.Parse((_parent.Attributes["height"] == null) ? _parent.ParentNode.Attributes["height"].Value : _parent.Attributes["height"].Value));
                                }
                            }
                        }
                        else
                        {
                            int num14 = int.Parse(_stack.Pop().ToString());
                            _stack.Push(num14 + int.Parse(_parent.Attributes["height"].Value));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Metafile> MakeImagesByXmlDocument(XmlDocument _doc)
        {
            Graphics graphics = null;
            List<Metafile> result;
            try
            {
                int width = int.Parse(_doc.SelectSingleNode("PrintDocument/PageSize").Attributes["Width"].Value);
                int height = int.Parse(_doc.SelectSingleNode("PrintDocument/PageSize").Attributes["Height"].Value);
                Bitmap image = new Bitmap(width, height);
                Graphics graphics2 = Graphics.FromImage(image);
                System.Drawing.Rectangle frameRect = new System.Drawing.Rectangle(0, 0, width, height);
                Metafile metafile = new Metafile(new MemoryStream(), graphics2.GetHdc(), frameRect, MetafileFrameUnit.Pixel);
                graphics = Graphics.FromImage(metafile);
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                List<Metafile> list = new List<Metafile>();
                list.Add(metafile);
                DrawDriverXmlOp.SetRowsAttributeByXmlNode(_doc);
                XmlNode xmlNode = _doc.SelectSingleNode("PrintDocument/Body");
                int num = 0;
                int num2 = 0;
                Stack stack = new Stack();
                stack.Push(num);
                Stack stack2 = new Stack();
                int num3 = 0;
                XmlNodeList xmlNodeList;
                if (_doc.SelectSingleNode("PrintDocument/SpecialDrawRules/SpecialShowValue") == null)
                {
                    xmlNodeList = null;
                }
                else
                {
                    xmlNodeList = _doc.SelectSingleNode("PrintDocument/SpecialDrawRules/SpecialShowValue").ChildNodes;
                }
                DrawOp.showTypeObject.Clear();
                if (xmlNodeList != null && xmlNodeList.Count > 0)
                {
                    foreach (XmlNode node2 in xmlNodeList)
                    {
                        int num4 = (node2.Attributes == null || node2.Attributes["DrawType"] == null || node2.Attributes["DrawType"].Value == "" || node2.Attributes["DrawType"].Value == null) ? 0 : int.Parse(node2.Attributes["DrawType"].Value);
                        string parameter = (node2.Attributes == null || node2.Attributes["ParameterName"] == null || node2.Attributes["ParameterName"].Value == null) ? "" : node2.Attributes["ParameterName"].Value;
                        DrawOp.showTypeObject.Add(new ShowObject(num4.ToString(), parameter));
                    }
                }
                foreach (XmlNode xmlNode3 in xmlNode)
                {
                    if (xmlNode3.Name.ToLower().Equals("pageend"))
                    {
                        num3++;
                        XmlNodeList childNodes = _doc.SelectSingleNode("PrintDocument/SpecialDrawRules/SpecialDrawline").ChildNodes;
                        foreach (XmlNode xmlNode2 in childNodes)
                        {
                            int num5 = (xmlNode2.Attributes == null || xmlNode2.Attributes["PageIndex"] == null || xmlNode2.Attributes["PageIndex"].Value == "") ? 0 : int.Parse(xmlNode2.Attributes["PageIndex"].Value);
                            if (num3 == num5)
                            {
                                XmlNodeList childNodes2 = xmlNode2.ChildNodes;
                                foreach (XmlNode xmlNode4 in childNodes2)
                                {
                                    DrawOp.DrawSpecialLine(xmlNode4, graphics);
                                }
                            }
                        }
                        graphics.Save();
                        graphics.Dispose();
                        image = new Bitmap(width, height);
                        graphics2 = Graphics.FromImage(image);
                        metafile = new Metafile(new MemoryStream(), graphics2.GetHdc(), frameRect, MetafileFrameUnit.Pixel);
                        graphics = Graphics.FromImage(metafile);
                        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.Clear(Color.White);
                        list.Add(metafile);
                        stack.Clear();
                        stack.Push(num);
                        stack2 = new Stack();
                        num2 = 0;
                    }
                    else
                    {
                        DrawOp.DrawLayer(graphics, xmlNode3, ref stack, ref stack2, ref num2);
                    }
                }
                list.RemoveAt(list.Count - 1);
                result = list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                graphics.Save();
                graphics.Dispose();
            }
            return result;
        }

        private static void DrawLayer(Graphics g, XmlNode _node, ref Stack _stack, ref Stack _curStack, ref int _left)
        {
            try
            {
                if (_node.Name.ToLower().Equals("p") || _node.Name.ToLower().Equals("div") || _node.Name.ToLower().Equals("table") || _node.Name.ToLower().Equals("line"))
                {
                    _left = 0;
                    if (_node.PreviousSibling != null)
                    {
                        if (_node.PreviousSibling.Attributes == null || _node.PreviousSibling.Attributes["height"] == null || string.IsNullOrEmpty(_node.PreviousSibling.Attributes["height"].Value))
                        {
                            _stack.Push(int.Parse(_stack.Pop().ToString()));
                            _curStack.Push(int.Parse(_node.Attributes["height"].Value));
                        }
                        else
                        {
                            _stack.Push(int.Parse(_stack.Pop().ToString()) + int.Parse(_node.PreviousSibling.Attributes["height"].Value));
                            _curStack.Pop();
                            _curStack.Push(int.Parse(_node.Attributes["height"].Value));
                        }
                    }
                    else
                    {
                        _curStack.Push(int.Parse(_node.Attributes["height"].Value));
                    }
                }
                if (_node.HasChildNodes && !_node.Name.ToLower().Equals("span") && !_node.Name.ToLower().Equals("table"))
                {
                    foreach (XmlNode node in _node.ChildNodes)
                    {
                        DrawOp.DrawLayer(g, node, ref _stack, ref _curStack, ref _left);
                    }
                }
                else if (_node.Name.ToLower().Equals("span"))
                {
                    int num = int.Parse(_stack.Peek().ToString());
                    DrawOp.DrawSpanNode(g, _node, ref _left, num, (int)_curStack.Peek());
                }
                else if (_node.Name.ToLower().Equals("table"))
                {
                    int num = int.Parse(_stack.Peek().ToString());
                    CommonDrawOp.DrawTable(_node, g, _left, num);
                }
                else
                {
                    int num = int.Parse(_stack.Peek().ToString());
                    float width = (_node.Attributes == null || _node.Attributes["height"] == null || _node.Attributes["height"].Value == "") ? 1f : float.Parse(_node.Attributes["height"].Value);
                    Pen pen = new Pen(Brushes.Black, width);
                    g.DrawLine(pen, _left, num, _left + int.Parse(_node.Attributes["width"].Value), num);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void DrawSpanNode(Graphics g, XmlNode spanNode, ref int left, int top, int height)
        {
            try
            {
                FontColor fontColor = new FontColor();
                FontColor fontColor2 = CommonMethods.StringToFontColor(DrawOp.GetParentValueByAttribute(spanNode, "font"));
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                fontColor = CommonMethods.StringToFontColor(DrawOp.GetValueByAttributeCurrentToTop(spanNode, "font"));
                Border border = CommonMethods.StringToBorder(DrawOp.GetValueByAttributeCurrentToTop(spanNode, "border"));
                string valueByAttributeCurrentToTop = DrawOp.GetValueByAttributeCurrentToTop(spanNode, "format");
                if (spanNode.ParentNode.Attributes == null)
                {
                    num3 = (int)g.MeasureString(" ", fontColor.Font).Width;
                }
                else if (spanNode.Attributes["width"] == null || string.IsNullOrEmpty(spanNode.Attributes["width"].Value))
                {
                    num3 = (int)g.MeasureString(" ", fontColor.Font).Width;
                }
                else
                {
                    num3 = int.Parse(spanNode.Attributes["width"].Value);
                }
                if (border != null)
                {
                    int height2 = int.Parse(spanNode.ParentNode.Attributes["height"].Value);
                    CommonDrawOp.DrawBorder(g, left, top, num3, height2, border, out num, out num2);
                }
                if (spanNode.HasChildNodes)
                {
                    foreach (XmlNode xmlNode in spanNode.ChildNodes)
                    {
                        if (xmlNode.NodeType == XmlNodeType.Text)
                        {
                            string innerText = xmlNode.InnerText;
                            fontColor = CommonMethods.StringToFontColor(DrawOp.GetValueByAttributeCurrentToTop(xmlNode.ParentNode, "font"));
                            border = CommonMethods.StringToBorder(DrawOp.GetValueByAttributeCurrentToTop(xmlNode.ParentNode, "border"));
                            string text = innerText.Replace(" ", "^");
                            if (xmlNode.ParentNode.Attributes == null)
                            {
                                num3 = (int)g.MeasureString(text, fontColor.Font).Width;
                            }
                            else if (xmlNode.ParentNode.Attributes["width"] == null || string.IsNullOrEmpty(xmlNode.ParentNode.Attributes["width"].Value))
                            {
                                num3 = (int)g.MeasureString(text, fontColor.Font).Width;
                            }
                            else
                            {
                                num3 = int.Parse(xmlNode.ParentNode.Attributes["width"].Value);
                            }
                            int num4 = (int)g.MeasureString(text, fontColor.Font).Height;
                            int num5 = 0;
                            if (height != 0)
                            {
                                num5 = height - num4;
                            }
                            if (string.IsNullOrEmpty(valueByAttributeCurrentToTop))
                            {
                                AlignType align = AlignType.Center;
                                if (xmlNode.ParentNode.Attributes["Align"] != null)
                                {
                                    if (xmlNode.ParentNode.Attributes["Align"].Value.ToString() == "Center")
                                    {
                                        align = AlignType.Center;
                                    }
                                    if (xmlNode.ParentNode.Attributes["Align"].Value.ToString() == "Left")
                                    {
                                        align = AlignType.Left;
                                    }
                                    if (xmlNode.ParentNode.Attributes["Align"].Value.ToString() == "Right")
                                    {
                                        align = AlignType.Right;
                                    }
                                }
                                else
                                {
                                    align = AlignType.Center;
                                }
                                CommonDrawOp.DrawString(g, innerText, left, top + num5 / 2, num3, height, fontColor, align, ValignType.Middle, out num, out num2);
                            }
                            else
                            {
                                foreach (ShowObject current in DrawOp.showTypeObject)
                                {
                                    if (current.ParameterName.Equals(valueByAttributeCurrentToTop))
                                    {
                                        switch ((DrawStyle)Enum.Parse(typeof(DrawStyle), current.DrawType))
                                        {
                                            case DrawStyle.Normal:
                                                CommonDrawOp.DrawString(g, innerText, left, top + num5, num3, (int)g.MeasureString(text, fontColor.Font).Height, fontColor, AlignType.Center, ValignType.Middle, out num, out num2);
                                                break;
                                            case DrawStyle.mark:
                                                CommonDrawOp.DrawSpecialValue(g, innerText, left, top + num5, num3, (int)g.MeasureString(text, fontColor.Font).Height, fontColor, AlignType.Center, ValignType.Middle, out num, out num2);
                                                break;
                                            case DrawStyle.sdecimal:
                                                break;
                                            default:
                                                CommonDrawOp.DrawString(g, innerText, left, top + num5, num3, (int)g.MeasureString(text, fontColor.Font).Height, fontColor, AlignType.Center, ValignType.Middle, out num, out num2);
                                                break;
                                        }
                                        break;
                                    }
                                }
                            }
                            left += num3;
                        }
                        else
                        {
                            string text2 = xmlNode.Name.ToLower();
                            if (text2 != null)
                            {
                                if (!(text2 == "b") && !(text2 == "u") && !(text2 == "i"))
                                {
                                    if (!(text2 == "checkbox"))
                                    {
                                        if (text2 == "c")
                                        {
                                            int num6 = 0;
                                            int num7 = 0;
                                            fontColor = CommonMethods.StringToFontColor(DrawOp.GetValueByAttributeCurrentToTop(xmlNode, "font"));
                                            fontColor2 = CommonMethods.StringToFontColor(DrawOp.GetParentValueByAttribute(spanNode, "font"));
                                            int num4 = (int)g.MeasureString(xmlNode.InnerText, fontColor2.Font).Height;
                                            int num5 = 0;
                                            if (height != 0)
                                            {
                                                num5 = height - num4;
                                            }
                                            CommonDrawOp.DrawCircleString(g, xmlNode.InnerText, left + 5, top + num5, 10, 10, fontColor, Color.Black, out num6, out num7, DrawType.Rectangle);
                                            left += (int)g.MeasureString(xmlNode.InnerText.Replace(" ", "^"), fontColor.Font).Width;
                                        }
                                    }
                                    else
                                    {
                                        CommonDrawOp.DrawFillRectangle(g, left + 2, top, 10, 10, Color.Black, false, out num, out num2);
                                        if (xmlNode.Attributes == null || xmlNode.Attributes["checked"] == null)
                                        {
                                            break;
                                        }
                                        if (xmlNode.Attributes["checked"].Value == "true")
                                        {
                                            fontColor = CommonMethods.StringToFontColor(DrawOp.GetValueByAttributeCurrentToTop(xmlNode, "font"));
                                            g.DrawString("√", fontColor.Font, Brushes.Black, (float)left, (float)top + 5f);
                                        }
                                        left += 18;
                                        string innerText = xmlNode.InnerText;
                                        g.DrawString(innerText, fontColor.Font, Brushes.Black, (float)left, (float)top + 2f);
                                        left += (int)g.MeasureString(innerText.Replace(" ", "^"), fontColor.Font).Width;
                                    }
                                }
                                else
                                {
                                    string innerText = xmlNode.InnerText;
                                    FontStyle style = FontStyle.Regular;
                                    DrawOp.SetSpecialCharacter(xmlNode, ref style);
                                    fontColor = CommonMethods.StringToFontColor(DrawOp.GetValueByAttributeCurrentToTop(xmlNode, "font"));
                                    System.Drawing.Font font = new System.Drawing.Font("新宋体", fontColor.Font.Size, style);
                                    string text = innerText.Replace(" ", "^");
                                    if (xmlNode.ParentNode.Attributes == null)
                                    {
                                        num3 = (int)g.MeasureString(text, fontColor.Font).Width;
                                    }
                                    else if (xmlNode.ParentNode.Attributes["width"] == null || string.IsNullOrEmpty(xmlNode.ParentNode.Attributes["width"].Value))
                                    {
                                        num3 = (int)g.MeasureString(text.Replace(" ", "^"), font).Width;
                                    }
                                    else
                                    {
                                        num3 = int.Parse(xmlNode.ParentNode.Attributes["width"].Value);
                                    }
                                    int num4 = (int)g.MeasureString(text, fontColor.Font).Height;
                                    int num5 = 0;
                                    if (height != 0)
                                    {
                                        num5 = height - num4;
                                    }
                                    g.DrawString(innerText.Replace("\u3000", "  ").Replace(" ", "\0"), font, Brushes.Black, (float)left, (float)top + (float)num5);
                                    left += num3;
                                }
                            }
                        }
                    }
                }
                else
                {
                    left += num3;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void SetSpecialCharacter(XmlNode n, ref FontStyle fs)
        {
            try
            {
                if (n.HasChildNodes)
                {
                    string text = n.Name.ToLower();
                    if (text != null)
                    {
                        if (!(text == "u"))
                        {
                            if (!(text == "i"))
                            {
                                if (text == "b")
                                {
                                    fs |= FontStyle.Bold;
                                }
                            }
                            else
                            {
                                fs |= FontStyle.Italic;
                            }
                        }
                        else
                        {
                            fs |= FontStyle.Underline;
                        }
                    }
                    foreach (XmlNode n2 in n.ChildNodes)
                    {
                        DrawOp.SetSpecialCharacter(n2, ref fs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DrawSpecialLine(XmlNode xmlNode, Graphics g)
        {
            try
            {
                float x = 0f;
                float y = 0f;
                float x2 = 0f;
                float y2 = 0f;
                string text = (xmlNode.Attributes == null || xmlNode.Attributes["StartPosition"] == null || xmlNode.Attributes["StartPosition"].Value == "") ? "" : xmlNode.Attributes["StartPosition"].Value;
                string text2 = (xmlNode.Attributes == null || xmlNode.Attributes["EndPosition"] == null || xmlNode.Attributes["EndPosition"].Value == "") ? "" : xmlNode.Attributes["EndPosition"].Value;
                string[] array = text.Split(new char[]
				{
					' '
				});
                string[] array2 = text2.Split(new char[]
				{
					' '
				});
                Pen pen = new Pen(Brushes.Black);
                if (array.Count<string>() == 2 || array2.Count<string>() == 2)
                {
                    x = float.Parse(array[0]);
                    y = float.Parse(array[1]);
                    x2 = float.Parse(array2[0]);
                    y2 = float.Parse(array2[1]);
                }
                g.DrawLine(pen, x, y, x2, y2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ExportToPDf(List<Metafile> pages, string filePath)
        {
            try
            {
                if (!filePath.Contains(".pdf"))
                {
                    filePath += ".pdf";
                }
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.OpenOrCreate));
                document.Open();
                for (int i = 0; i < pages.Count; i++)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    pages[i].Save(memoryStream, ImageFormat.Png);
                    iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(memoryStream.ToArray());
                    instance.Alignment = 1;
                    instance.ScalePercent(70f);
                    document.Add(instance);
                }
                document.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void PrintView(List<Metafile> pages)
        {
            try
            {
                new PrintForm(pages)
                {
                    TopMost = true
                }.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Print(List<Metafile> pages)
        {
            try
            {
                DrawOp.listPages = pages;
                PrintDocument printDocument = new PrintDocument();
                DrawOp.pageSetupDialogs.Document = printDocument;
                printDocument.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";
                DrawOp.pageSetupDialogs.PageSettings.PaperSize = new PaperSize("A4", 827, 1146);
                DrawOp.pageSetupDialogs.PageSettings.Margins = new Margins(0, 0, 15, 15);
                printDocument.DefaultPageSettings.PaperSize = DrawOp.pageSetupDialogs.PageSettings.PaperSize;
                printDocument.PrintPage += new PrintPageEventHandler(DrawOp.printDocument_PrintPag);
                printDocument.Print();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void printDocument_PrintPag(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (DrawOp.printPageNowPer < DrawOp.listPages.Count)
                {
                    e.Graphics.DrawImage(DrawOp.listPages[DrawOp.printPageNowPer], new System.Drawing.Rectangle((DrawOp.pageSetupDialogs.PageSettings.PaperSize.Width - DrawOp.listPages[DrawOp.printPageNowPer].Width) / 2, 0, Convert.ToInt32(DrawOp.listPages[DrawOp.printPageNowPer].Width), Convert.ToInt32(DrawOp.listPages[DrawOp.printPageNowPer].Height)), 0, 0, DrawOp.listPages[DrawOp.printPageNowPer].Width, DrawOp.listPages[DrawOp.printPageNowPer].Height, GraphicsUnit.Pixel);
                    DrawOp.printPageNowPer++;
                    e.HasMorePages = true;
                }
                else
                {
                    DrawOp.printPageNowPer = 0;
                    e.HasMorePages = false;
                }
                if (DrawOp.printPageNowPer == DrawOp.listPages.Count)
                {
                    DrawOp.printPageNowPer = 0;
                    e.HasMorePages = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
