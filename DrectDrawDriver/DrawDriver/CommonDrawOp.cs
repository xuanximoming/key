using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml;

namespace DrectSoft.DrawDriver
{
    public class CommonDrawOp
    {
        public static bool DrawString(Graphics g, string _content, int _left, int _top, int _width, int _height, FontColor _fontcolor, AlignType _align, ValignType _valign, out int _newleft, out int _newtop)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                if (_content == null)
                {
                    throw new NullReferenceException();
                }
                if (_fontcolor == null)
                {
                    throw new NullReferenceException();
                }
                _newleft = _left;
                _newtop = _top;
                StringFormat stringFormat = new StringFormat();
                if (_align == AlignType.Center)
                {
                    stringFormat.Alignment = StringAlignment.Center;
                }
                else if (_align == AlignType.Left)
                {
                    stringFormat.Alignment = StringAlignment.Near;
                }
                else if (_align == AlignType.Right)
                {
                    stringFormat.Alignment = StringAlignment.Far;
                }
                else
                {
                    stringFormat.Alignment = StringAlignment.Center;
                }
                if (_valign == ValignType.Middle)
                {
                    stringFormat.LineAlignment = StringAlignment.Center;
                }
                else if (_valign == ValignType.Top)
                {
                    stringFormat.LineAlignment = StringAlignment.Near;
                }
                else if (_valign == ValignType.Bottom)
                {
                    stringFormat.LineAlignment = StringAlignment.Far;
                }
                else
                {
                    stringFormat.LineAlignment = StringAlignment.Center;
                }
                g.DrawString(_content, _fontcolor.Font, new SolidBrush(_fontcolor.Color), new Rectangle(_left, _top, _width, _height), stringFormat);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DrawSpecialValue(Graphics g, string _content, int _left, int _top, int _width, int _height, FontColor _fontcolor, AlignType _align, ValignType _valign, out int _newleft, out int _newtop)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                if (_content == null)
                {
                    throw new NullReferenceException();
                }
                if (_fontcolor == null)
                {
                    throw new NullReferenceException();
                }
                _newleft = _left;
                _newtop = _top;
                StringFormat stringFormat = new StringFormat();
                if (_align == AlignType.Center)
                {
                    stringFormat.Alignment = StringAlignment.Center;
                }
                else if (_align == AlignType.Left)
                {
                    stringFormat.Alignment = StringAlignment.Near;
                }
                else if (_align == AlignType.Right)
                {
                    stringFormat.Alignment = StringAlignment.Far;
                }
                else
                {
                    stringFormat.Alignment = StringAlignment.Center;
                }
                if (_valign == ValignType.Middle)
                {
                    stringFormat.LineAlignment = StringAlignment.Center;
                }
                else if (_valign == ValignType.Top)
                {
                    stringFormat.LineAlignment = StringAlignment.Near;
                }
                else if (_valign == ValignType.Bottom)
                {
                    stringFormat.LineAlignment = StringAlignment.Far;
                }
                else
                {
                    stringFormat.LineAlignment = StringAlignment.Center;
                }
                string[] array = _content.Split(new char[]
				{
					','
				});
                if (array == null || array.Length == 1 || array.Length == 2)
                {
                    g.DrawString(_content, _fontcolor.Font, new SolidBrush(_fontcolor.Color), new RectangleF((float)_left, (float)_top, (float)_width, (float)_height), stringFormat);
                }
                else
                {
                    int num = (int)g.MeasureString(array[0], _fontcolor.Font).Width;
                    Bitmap image = new Bitmap(_width, _height);
                    Graphics graphics = Graphics.FromImage(image);
                    g.DrawString(array[0], _fontcolor.Font, new SolidBrush(_fontcolor.Color), new Point(_left + (_width / 3 - 3), _top + _height / 2), stringFormat);
                    if (array[1].Trim() != "0")
                    {
                        g.DrawString(array[1], _fontcolor.Font, new SolidBrush(_fontcolor.Color), new PointF((float)(_left + _width / 2), (float)_top), stringFormat);
                        g.DrawLine(new Pen(_fontcolor.Color), new Point(_left + _width / 3, _top + _height / 2 - 3), new Point(_left + (_width / 2 + num), _top + _height / 2 - 3));
                        g.DrawString(array[2], _fontcolor.Font, new SolidBrush(_fontcolor.Color), new PointF((float)(_left + _width / 2), (float)(_top + _height - 5)), stringFormat);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DrawLine(Graphics g, int _startleft, int _starttop, int _endleft, int _endtop, Color _color, LineType _linetype, int _linewidth, out int _newleft, out int _newtop)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                //bool flag = 0 == 0;
                _newleft = _startleft;
                _newtop = _starttop;
                Pen pen = new Pen(_color, (float)_linewidth);
                if (_linetype == LineType.Solid)
                {
                    pen.DashStyle = DashStyle.Solid;
                }
                else if (_linetype == LineType.Dash)
                {
                    pen.DashStyle = DashStyle.Dash;
                }
                else if (_linetype == LineType.DashDot)
                {
                    pen.DashStyle = DashStyle.DashDot;
                }
                else if (_linetype == LineType.DashDotDot)
                {
                    pen.DashStyle = DashStyle.DashDotDot;
                }
                else if (_linetype == LineType.Dot)
                {
                    pen.DashStyle = DashStyle.Dot;
                }
                g.DrawLine(pen, _startleft, _starttop, _endleft, _endtop);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DrawBorder(Graphics g, int _left, int _top, int _width, int _height, Border _border, out int _newleft, out int _newtop)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                if (_border == null)
                {
                    throw new NullReferenceException();
                }
                _newleft = _left;
                _newtop = _top;
                if (_border.IsDrawTop)
                {
                    CommonDrawOp.DrawLine(g, _left, _top, _left + _width, _top, _border.TopColor, _border.TopStyle, _border.TopWidth, out _newleft, out _newtop);
                }
                if (_border.IsDrawRight)
                {
                    CommonDrawOp.DrawLine(g, _left + _width, _top, _left + _width, _top + _height, _border.RightColor, _border.RightStyle, _border.RightWidth, out _newleft, out _newtop);
                }
                if (_border.IsDrawBottom)
                {
                    CommonDrawOp.DrawLine(g, _left, _top + _height, _left + _width, _top + _height, _border.BottomColor, _border.BottomStyle, _border.BottomWidth, out _newleft, out _newtop);
                }
                if (_border.IsDrawLeft)
                {
                    CommonDrawOp.DrawLine(g, _left, _top, _left, _top + _height, _border.LeftColor, _border.LeftStyle, _border.LeftWidth, out _newleft, out _newtop);
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DrawFillRectangle(Graphics g, int _left, int _top, int _width, int _height, Color _color, bool isfill, out int _newleft, out int _newtop)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                //bool flag = 0 == 0;
                _newleft = _left;
                _newtop = _top;
                if (!isfill)
                {
                    g.DrawRectangle(new Pen(_color, 1f), new Rectangle(_left, _top, 10, 10));
                }
                else
                {
                    g.FillRectangle(new SolidBrush(_color), new Rectangle(_left, _top, 10, 10));
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DrawCircle(Graphics g, int _x, int _y, int _r, Color _color, bool isfill, out int _newleft, out int _newtop)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                //bool flag = 0 == 0;
                _newleft = _x;
                _newtop = _y;
                if (!isfill)
                {
                    g.DrawEllipse(new Pen(_color, 1f), new Rectangle(_x, _y, _r, _r));
                }
                else
                {
                    g.FillEllipse(new SolidBrush(_color), new Rectangle(_x, _y, _r, _r));
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DrawCircleString(Graphics g, string _content, int _left, int _top, int _width, int _height, FontColor _fontcolor, Color _circlecolor, out int _newleft, out int _newtop)
        {
            return CommonDrawOp.DrawCircleString(g, _content, _left, _top, _width, _height, _fontcolor, _circlecolor, out _newleft, out _newtop, DrawType.Circle);
        }

        public static bool DrawCircleString(Graphics g, string _content, int _left, int _top, int _width, int _height, FontColor _fontcolor, Color _circlecolor, out int _newleft, out int _newtop, DrawType _drawtype)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                if (_fontcolor == null)
                {
                    throw new NullReferenceException();
                }
                //bool flag = 0 == 0;
                _newleft = _left;
                _newtop = _top;
                SizeF sizeF = g.MeasureString(_content, _fontcolor.Font);
                if (_drawtype == DrawType.Rectangle)
                {
                    g.DrawRectangle(new Pen(_circlecolor, 1f), new Rectangle(_left, _top, _width + 1, _height + 1));
                }
                else
                {
                    g.DrawEllipse(new Pen(_circlecolor, 1f), new Rectangle(_left, _top, _width + 1, _height + 1));
                }
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                g.DrawString(_content, _fontcolor.Font, new SolidBrush(_fontcolor.Color), new Rectangle(_left, _top, _width + 3, _height + 3), stringFormat);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DrawImage(Graphics g, Image _image, int _left, int _top, int _width, int _height, out int _newleft, out int _newtop)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                if (_image == null)
                {
                    throw new NullReferenceException();
                }
                _newleft = _left;
                _newtop = _top;
                g.DrawImage(_image, _left, _top, _width, _height);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DrawSupText(Graphics g, string _content, int _left, int _top, int _width, int _height, FontColor _parentfontcolor, out int _newleft, out int _newtop)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                if (_content == null)
                {
                    throw new NullReferenceException();
                }
                if (_parentfontcolor == null)
                {
                    throw new NullReferenceException();
                }
                _newleft = _left;
                _newtop = _top;
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Near;
                Font font = _parentfontcolor.Font;
                g.DrawString(_content, new Font(font.FontFamily, font.Size / 2f, font.Style, font.Unit), new SolidBrush(_parentfontcolor.Color), new Rectangle(_left, _top, _width, _height), stringFormat);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool DrawSubText(Graphics g, string _content, int _left, int _top, int _width, int _height, FontColor _parentfontcolor, out int _newleft, out int _newtop)
        {
            bool result;
            try
            {
                if (g == null)
                {
                    throw new NullReferenceException();
                }
                if (_content == null)
                {
                    throw new NullReferenceException();
                }
                if (_parentfontcolor == null)
                {
                    throw new NullReferenceException();
                }
                _newleft = _left;
                _newtop = _top;
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Near;
                stringFormat.LineAlignment = StringAlignment.Far;
                Font font = _parentfontcolor.Font;
                g.DrawString(_content, new Font(font.FontFamily, font.Size / 2f, font.Style, font.Unit), new SolidBrush(_parentfontcolor.Color), new Rectangle(_left, _top, _width, _height), stringFormat);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static void DrawTable(XmlNode tableNode, Graphics g, int x, int y)
        {
            try
            {
                CommonDrawOp.DrawHead(tableNode, g, x, y);
                XmlCommomOp.SetXmlAttribute(tableNode, "left", string.Concat(x));
                XmlCommomOp.SetXmlAttribute(tableNode, "top", string.Concat(y));
                int num = y;
                XmlNode xmlNode = tableNode.SelectNodes("TableHead")[0];
                XmlNodeList childNodes = xmlNode.ChildNodes;
                for (int i = 0; i < childNodes.Count; i++)
                {
                    num += int.Parse(childNodes[i].Attributes["height"].Value);
                }
                CommonDrawOp.DrawTableBody(tableNode, g, x, num);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void DrawHead(XmlNode xmlNode, Graphics g, int x, int y)
        {
            try
            {
                Dictionary<int, List<int>> tableModel = XmlCommomOp.GetTableModel(xmlNode);
                Dictionary<int, XmlNode> column = XmlCommomOp.GetColumn(xmlNode);
                XmlNode xmlNode2 = xmlNode.SelectNodes("TableHead")[0];
                XmlNodeList childNodes = xmlNode2.ChildNodes;
                int count = childNodes.Count;
                int num = 0;
                foreach (XmlNode xmlNode3 in childNodes[0])
                {
                    if (xmlNode3.Attributes["colspan"] != null)
                    {
                        num += int.Parse(xmlNode3.Attributes["colspan"].Value);
                    }
                    else
                    {
                        num++;
                    }
                }
                Rectangle rectangle = default(Rectangle);
                rectangle.Y = y;
                rectangle.X = x;
                for (int i = 1; i <= childNodes.Count; i++)
                {
                    int num2 = int.Parse(childNodes[i - 1].Attributes["height"].Value);
                    rectangle.X = x;
                    rectangle.Height = num2;
                    for (int j = 0; j < num; j++)
                    {
                        int width = int.Parse(column[j + 1].Attributes["width"].Value);
                        rectangle.Width = width;
                        g.DrawRectangle(Pens.Black, rectangle);
                        rectangle.Offset(rectangle.Width, 0);
                    }
                    rectangle.Y += num2;
                }
                rectangle = default(Rectangle);
                rectangle.Y = y;
                for (int i = 1; i <= count; i++)
                {
                    int num3 = 0;
                    int num4 = tableModel[i][num3];
                    int num5 = int.Parse(childNodes[i - 1].Attributes["height"].Value);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    foreach (XmlNode xmlNode4 in childNodes[i - 1].ChildNodes)
                    {
                        int num6 = (xmlNode4.Attributes["rowspan"] == null || xmlNode4.Attributes["rowspan"].Value == "") ? 1 : int.Parse(xmlNode4.Attributes["rowspan"].Value);
                        int num7 = (xmlNode4.Attributes["colspan"] == null || xmlNode4.Attributes["colspan"].Value == "") ? 1 : int.Parse(xmlNode4.Attributes["colspan"].Value);
                        int arg_34C_0 = (xmlNode4.Attributes["width"] == null || xmlNode4.Attributes["width"].Value == "") ? 1 : int.Parse(xmlNode4.Attributes["width"].Value);
                        string str = (xmlNode4.Attributes["Font"] == null || xmlNode4.Attributes["Font"].Value == "") ? "" : xmlNode4.Attributes["Font"].Value;
                        string text = (xmlNode4.Attributes["IsBlod"] == null || xmlNode4.Attributes["IsBlod"].Value == "") ? "false" : xmlNode4.Attributes["IsBlod"].Value;
                        string text2 = (xmlNode4.Attributes["Align"] == null || xmlNode4.Attributes["Align"].Value == "") ? "Center" : xmlNode4.Attributes["Align"].Value;
                        string text3 = text2.ToLower().Trim().ToString();
                        if (text3 == null)
                        {
                            goto IL_4C9;
                        }
                        if (!(text3 == "left"))
                        {
                            if (!(text3 == "right"))
                            {
                                if (!(text3 == "center"))
                                {
                                    goto IL_4C9;
                                }
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Center;
                            }
                            else
                            {
                                stringFormat.LineAlignment = StringAlignment.Center;
                                stringFormat.Alignment = StringAlignment.Far;
                            }
                        }
                        else
                        {
                            stringFormat.LineAlignment = StringAlignment.Center;
                            stringFormat.Alignment = StringAlignment.Near;
                        }
                    IL_4DD:
                        FontColor fontColor = CommonMethods.StringToFontColor(str);
                        FontStyle style = fontColor.Font.Style;
                        Font font;
                        if (text.ToLower().Trim().Equals("true"))
                        {
                            font = new Font(fontColor.Font.Name, fontColor.Font.Size, fontColor.Font.Style | FontStyle.Bold);
                        }
                        else
                        {
                            font = new Font(fontColor.Font.Name, fontColor.Font.Size, fontColor.Font.Style);
                        }
                        string innerText = xmlNode4.InnerText;
                        rectangle.X = x;
                        rectangle.Height = 0;
                        rectangle.Width = 0;
                        for (int j = 1; j < num4; j++)
                        {
                            rectangle.X += int.Parse(column[j].Attributes["width"].Value);
                        }
                        for (int k = i; k < i + num6; k++)
                        {
                            rectangle.Height += int.Parse(childNodes[k - 1].Attributes["height"].Value);
                        }
                        for (int l = num4; l < num4 + num7; l++)
                        {
                            rectangle.Width += int.Parse(column[l].Attributes["width"].Value);
                        }
                        g.FillRectangle(Brushes.White, rectangle);
                        g.DrawRectangle(Pens.Black, rectangle);
                        g.DrawString(innerText, font, new SolidBrush(fontColor.Color), rectangle, stringFormat);
                        num3 += num7;
                        if (num3 < tableModel[i].Count)
                        {
                            num4 = tableModel[i][num3];
                        }
                        continue;
                    IL_4C9:
                        stringFormat.Alignment = StringAlignment.Center;
                        stringFormat.LineAlignment = StringAlignment.Center;
                        goto IL_4DD;
                    }
                    rectangle.Y += num5;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void DrawTableBody(XmlNode xmlNode, Graphics g, int x, int y)
        {
            try
            {
                XmlNode xmlNode2 = xmlNode.SelectNodes("TableBody")[0];
                Dictionary<int, XmlNode> column = XmlCommomOp.GetColumn(xmlNode);
                XmlNodeList childNodes = xmlNode2.ChildNodes;
                int count = childNodes.Count;
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                Rectangle rectangle = default(Rectangle);
                rectangle.Y = y;
                rectangle.Height = int.Parse(xmlNode2.FirstChild.Attributes["height"].Value);
                string str = (xmlNode2.Attributes["Font"] == null || xmlNode2.Attributes["Font"].Value == "") ? "" : xmlNode2.Attributes["Font"].Value;
                FontColor fontColor = CommonMethods.StringToFontColor(str);
                foreach (XmlNode xmlNode3 in childNodes)
                {
                    rectangle.X = x;
                    int num = 1;
                    foreach (XmlNode xmlNode4 in xmlNode3.ChildNodes)
                    {
                        rectangle.Width = int.Parse(column[num].Attributes["width"].Value);
                        string text = (column[num].Attributes["ContentAlign"] == null || column[num].Attributes["ContentAlign"].Value == "") ? "left" : column[num].Attributes["ContentAlign"].Value;
                        string text2 = text.ToLower().Trim().ToString();
                        if (text2 == null)
                        {
                            goto IL_22E;
                        }
                        if (!(text2 == "left"))
                        {
                            if (!(text2 == "right"))
                            {
                                if (!(text2 == "center"))
                                {
                                    goto IL_22E;
                                }
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Center;
                            }
                            else
                            {
                                stringFormat.LineAlignment = StringAlignment.Center;
                                stringFormat.Alignment = StringAlignment.Far;
                            }
                        }
                        else
                        {
                            stringFormat.LineAlignment = StringAlignment.Center;
                            stringFormat.Alignment = StringAlignment.Near;
                        }
                    IL_242:
                        string innerText = xmlNode4.InnerText;
                        g.DrawRectangle(Pens.Black, rectangle);
                        g.DrawString(innerText, fontColor.Font, new SolidBrush(fontColor.Color), rectangle, stringFormat);
                        rectangle.X += rectangle.Width;
                        num++;
                        continue;
                    IL_22E:
                        stringFormat.LineAlignment = StringAlignment.Center;
                        stringFormat.Alignment = StringAlignment.Near;
                        goto IL_242;
                    }
                    rectangle.Y += rectangle.Height;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
