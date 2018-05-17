using System;
using System.Drawing;
using System.Xml;

namespace DrectSoft.DrawDriver
{
    public class DrawTable
    {
        private int int_cols_max;
        private int int_rows_max;
        private bool[,] IsOccupySS;
        private int start_left;
        private int start_top;
        private int x;
        private int y;
        private int left;
        private int top;
        private Graphics g;
        private XmlNode TableHeadNode;
        public int GetWidth(int i)
        {
            int result;
            try
            {
                result = 100;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int GetHeight(int i)
        {
            int result;
            try
            {
                result = 50;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int GetLeft(int i)
        {
            int result;
            try
            {
                result = 10 + this.GetWidth(i) * i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int GetTop(int i)
        {
            int result;
            try
            {
                result = 10 + this.GetHeight(i) * i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public DrawTable(Graphics _g, XmlNode _TableHeadNode)
        {
            try
            {
                this.int_cols_max = 1;
                this.int_rows_max = 1;
                this.g = _g;
                this.TableHeadNode = _TableHeadNode;
                this.SetIsOccupySS();
                this.SetWidthHeight();
                this.start_left = 10;
                this.start_top = 10;
                this.x = 0;
                this.y = 0;
                this.left = this.start_left;
                this.top = this.start_top;
                this.StartDraw();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetIsOccupySS()
        {
            try
            {
                int num = 0;
                int num2 = 0;
                foreach (XmlNode xmlNode in this.TableHeadNode.ChildNodes)
                {
                    if (xmlNode.Name.Trim().ToLower().Equals("tr"))
                    {
                        num2++;
                        int num3 = 0;
                        foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                        {
                            if (xmlNode2.Name.Trim().ToLower().Equals("th"))
                            {
                                int num4 = 1;
                                XmlElement xmlElement = (XmlElement)xmlNode2;
                                foreach (XmlAttribute xmlAttribute in xmlElement.Attributes)
                                {
                                    if (xmlAttribute.Name.ToLower().Equals("colspan".ToLower()))
                                    {
                                        num4 = CommonMethods.StringToInt32(xmlAttribute.Value, 1);
                                    }
                                    if (xmlAttribute.Name.ToLower().Equals("rowspan".ToLower()))
                                    {
                                        int num5 = CommonMethods.StringToInt32(xmlAttribute.Value, 1);
                                    }
                                }
                                num3 += num4;
                            }
                        }
                        if (num3 > num)
                        {
                            num = num3;
                        }
                    }
                }
                this.int_cols_max = num;
                this.int_rows_max = num2;
                this.IsOccupySS = new bool[this.int_rows_max, this.int_cols_max];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetWidthHeight()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetOccupySS(int xxx, int yyy, int colspan, int rowspan)
        {
            try
            {
                for (int i = yyy; i < yyy + rowspan; i++)
                {
                    this.IsOccupySS[i, xxx] = true;
                }
                for (int i = xxx; i < xxx + colspan; i++)
                {
                    this.IsOccupySS[yyy, i] = true;
                }
                for (int i = yyy; i < yyy + rowspan; i++)
                {
                    for (int j = xxx; j < xxx + colspan; j++)
                    {
                        this.IsOccupySS[i, j] = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void StartDraw()
        {
            try
            {
                Pen pen = new Pen(Color.Black, 1f);
                int num = 0;
                foreach (XmlNode xmlNode in this.TableHeadNode.ChildNodes)
                {
                    if (xmlNode.Name.Trim().ToLower().Equals("tr"))
                    {
                        int num2 = 0;
                        foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                        {
                            if (xmlNode2.Name.ToLower().Equals("th"))
                            {
                                while (this.IsOccupySS[num, num2])
                                {
                                    num2++;
                                }
                                int num3 = 1;
                                int num4 = 1;
                                XmlElement xmlElement = (XmlElement)xmlNode2;
                                foreach (XmlAttribute xmlAttribute in xmlElement.Attributes)
                                {
                                    if (xmlAttribute.Name.ToLower().Equals("colspan".ToLower()))
                                    {
                                        num3 = CommonMethods.StringToInt32(xmlAttribute.Value, 1);
                                    }
                                    if (xmlAttribute.Name.ToLower().Equals("rowspan".ToLower()))
                                    {
                                        num4 = CommonMethods.StringToInt32(xmlAttribute.Value, 1);
                                    }
                                }
                                this.SetOccupySS(num2, num, num3, num4);
                                string innerText = xmlNode2.InnerText;
                                this.g.DrawRectangle(pen, new Rectangle(this.GetLeft(num2), this.GetTop(num), this.GetWidth(num2) * num3, this.GetHeight(num) * num4));
                                Font font = new Font(new FontFamily("宋体"), 15f, FontStyle.Regular, GraphicsUnit.Pixel);
                                StringFormat stringFormat = new StringFormat();
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Center;
                                this.g.DrawString(innerText, font, new SolidBrush(Color.Black), new Rectangle(this.GetLeft(num2), this.GetTop(num), this.GetWidth(num2) * num3, this.GetHeight(num) * num4), stringFormat);
                                num2 += num3;
                            }
                        }
                        num++;
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
