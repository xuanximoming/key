using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using System.Drawing.Printing;
using System.Xml;

using System.Reflection;
namespace DrectSoft.Core.QCReport
{
    /// <summary>
    /// xll 2013-01-10 自定义打印的预览与打印 非报表工具
    /// </summary>
    public partial class PrintForm : DevBaseForm
    {
        int pageEndRecordIndex = 0;  //当前页打印截止的记录序号  
        bool isFirstPrintPage = true;//是否是打印第一页
        PaperSize paperSize = new PaperSize("A4", 827, 1169);
        Font fontDraw = new Font("新宋体", 12);
        bool Landscape = true;
        Margins margin =null;
        int lineHeight = 50;//默认行高
        DataTable dt = null;
        private Dictionary<string, DataColumn> dic_columns;
        private XmlNode reportNode = null;
        PrintDocument printDocumentPrintNow = null;

        #region 滚动需要的字段
        private Type _t;
        private FieldInfo _Position;
        private MethodInfo _SetPositionMethod;
        #endregion

        public PrintForm(DataTable _dt, List<DataColumn> _columns, XmlNode _reportNode)
        {
            try
            {
                InitializeComponent();

                #region 滚动私有方法提取,从printpreviewControl中提取
                _t = typeof(System.Windows.Forms.PrintPreviewControl);
                _Position = _t.GetField("position", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);
                _SetPositionMethod = _t.GetMethod("SetPositionNoInvalidate", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.ExactBinding);
                #endregion
                //注册事件
                printPreviewControl1.MouseWheel += new MouseEventHandler(printPreviewControl1_MouseWheel);  

                InitPageSize();
                dt = _dt;
                dic_columns = CommonMethods.DataColumnsToDictionary(_columns);
                reportNode = _reportNode;
                InitPrintView();
                printDocumentPrintNow = new PrintDocument();
                printDocumentPrintNow.DefaultPageSettings.PaperSize = paperSize;
                printDocumentPrintNow.DefaultPageSettings.Landscape = Landscape;
                printDocumentPrintNow.PrintPage += new PrintPageEventHandler(printDocumentPrintNow_PrintPage);
                printPreviewControl1.Document = printDocumentPrintNow;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                printPreviewControl1.Zoom = Convert.ToDouble(trackBarControl1.Value) / 100;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
            
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintNow_Click(object sender, EventArgs e)
        {
            try
            {
                pageEndRecordIndex = 0;  //当前页打印截止的记录序号  
                isFirstPrintPage = true;//是否是打印第一页
                printDocumentPrintNow.Print();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        void printDocumentPrintNow_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                bool isPrintTableHeader = true;
                string pageType = lookUpEditPageSize.EditValue.ToString();
                PageSetupDialog pageSetupDialog = new PageSetupDialog();
                pageSetupDialog.Document = printDocumentPrintNow;
                foreach (PaperSize ps in pageSetupDialog.Document.PrinterSettings.PaperSizes)
                {
                    if (ps.PaperName.Equals(pageType))
                    {
                        paperSize = ps;
                        break;
                    }
                }

                if (paperSize != null)
                {
                    pageSetupDialog.Document.DefaultPageSettings.PaperSize = paperSize;
                }
                pageSetupDialog.PageSettings.Margins = new Margins(CalculateMarginLeft(), CalculateMarginLeft(), margin.Top, margin.Bottom);

                int pageHeight = pageSetupDialog.PageSettings.Margins.Top;
                //打印业务
                Font font = null;
                int printPageHeight = 0;
                if (Landscape)
                {
                    printPageHeight = paperSize.Width - pageSetupDialog.PageSettings.Margins.Top - pageSetupDialog.PageSettings.Margins.Bottom;
                }
                else
                {
                    printPageHeight = paperSize.Width - pageSetupDialog.PageSettings.Margins.Top - pageSetupDialog.PageSettings.Margins.Bottom;
                }

                for (int i = pageEndRecordIndex; i < dt.Rows.Count; i++)
                {
                    if (printPageHeight - pageHeight-lineHeight <= 0)
                    {
                        pageEndRecordIndex = i;
                        printPreviewControl1.Rows++;
                        e.HasMorePages = true;
                        break;
                    }
                    if (isFirstPrintPage)
                    {
                        XmlNodeList nodeList = reportNode.SelectNodes("Title");
                        foreach (XmlNode nodeItem in nodeList)
                        {
                            string text = nodeItem.Attributes == null || nodeItem.Attributes["text"] == null
                                        || nodeItem.Attributes["text"].Value.Trim() == "" ? "" : nodeItem.Attributes["text"].Value;
                            int width = int.Parse(nodeItem.Attributes["width"].Value.Trim());
                            int height = int.Parse(nodeItem.Attributes["height"].Value.Trim());
                            int x = int.Parse(nodeItem.Attributes["x"].Value.Trim());
                            int y = int.Parse(nodeItem.Attributes["y"].Value.Trim());
                            int fontSize = int.Parse(nodeItem.Attributes["fontsize"].Value.Trim());
                            string bold = nodeItem == null || nodeItem.Attributes == null || nodeItem.Attributes["bold"] == null ||
                                        nodeItem.Attributes["bold"].Value == "" ? "0" : nodeItem.Attributes["bold"].Value.Trim();
                            if (bold.Equals("1"))
                            {
                                font = new System.Drawing.Font("新宋体", fontSize, FontStyle.Bold);
                            }
                            else 
                            {
                                font = new System.Drawing.Font("新宋体", fontSize, FontStyle.Regular);
                            }
                            Rectangle rectangle = new Rectangle(x, y, width, height);
                            
                            g.DrawString(text, font, Brushes.Black, rectangle);
                            pageHeight = pageHeight + height;

                        }
                        isFirstPrintPage = false;
                    }

                    if (isPrintTableHeader)
                    {
                        DrawTableHeader(g, pageSetupDialog.PageSettings.Margins.Left, pageHeight);
                        pageHeight = pageHeight + lineHeight;
                        isPrintTableHeader = false;
                    }
                    Rectangle rec = new Rectangle();
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    rec.X = pageSetupDialog.PageSettings.Margins.Left;
                    rec.Y = pageHeight;
                    foreach (DataColumn item in dic_columns.Values)
                    {
                        rec.Height = lineHeight;
                        rec.Width = int.Parse(item.width);
                        if (dt.Columns.Contains(item.datafield))
                        {
                            g.DrawRectangle(Pens.Black, rec);
                            g.DrawString(dt.Rows[i][item.datafield].ToString(), fontDraw, Brushes.Black, rec, sf);
                            rec.Offset((int)rec.Width, 0);
                        }
                        else
                        {
                            g.DrawRectangle(Pens.Black, rec);
                            g.DrawString("", fontDraw, Brushes.Black, rec, sf);
                            rec.Offset((int)rec.Width, 0);
                        }
                    }
                    pageHeight = pageHeight + lineHeight;

                    if (i >= dt.Rows.Count - 1)
                    {
                        e.HasMorePages = false;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void InitPrintView() 
        {
            try
            {
                if (reportNode != null && reportNode.Attributes != null && reportNode.Attributes["landspace"] != null&&reportNode.Attributes["landspace"].Value.Trim()!="")
                {
                    if (reportNode.Attributes["landspace"].Value.ToString().Trim().ToLower() == "false")
                    {
                        Landscape = false;
                    }
                    else
                    {
                        Landscape = true;
                    }
                }
                else 
                {
                    Landscape = true;
                }

                if (reportNode != null && reportNode.Attributes != null && reportNode.Attributes["margin"] != null&&reportNode.Attributes["margin"].Value.Trim() != "")
                {
                    string[] margins = reportNode.Attributes["margin"].Value.Trim().Split(',');
                    if (margins.Length == 4)
                    {
                        margin = new Margins(int.Parse(margins[0]), int.Parse(margins[1]), int.Parse(margins[2]), int.Parse(margins[3]));
                    }
                    else 
                    {
                        margin = new Margins(0, 0, 0, 0);
                    }
                }
                else
                {
                    margin = new Margins(0, 0, 0, 0);
                }


                if (reportNode != null && reportNode.Attributes != null && reportNode.Attributes["lineHeight"] != null && reportNode.Attributes["lineHeight"].Value.Trim() != "")
                {
                    lineHeight = int.Parse(reportNode.Attributes["lineHeight"].Value.Trim());
                }
                else 
                {
                    lineHeight = 50;
                }

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 初始化页面大小
        /// </summary>
        /// 
        private void InitPageSize()
        {
            try
            {
                DataTable dataTablePageSize = new DataTable();
                dataTablePageSize.Columns.Add("PageName");
                dataTablePageSize.Columns.Add("PageValue");

                DataRow dr = dataTablePageSize.NewRow();
                dr["PageName"] = "A4";
                dr["PageValue"] = "A4";
                dataTablePageSize.Rows.Add(dr);

                dr = dataTablePageSize.NewRow();
                dr["PageName"] = "16K";
                dr["PageValue"] = "16K";
                dataTablePageSize.Rows.Add(dr);

                lookUpEditPageSize.Properties.DataSource = dataTablePageSize;
                lookUpEditPageSize.Properties.DisplayMember = "PageName";
                lookUpEditPageSize.Properties.ValueMember = "PageValue";


                lookUpEditPageSize.EditValue = "A4";
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private int CalculateMarginLeft() 
        {
            try
            {
                int marginLeft = 0;
                int sumColumn = 0;
                foreach (DataColumn itemColumn in dic_columns.Values)
                {
                    sumColumn = sumColumn + int.Parse(itemColumn.width);
                }
                if (Landscape)
                {
                    return marginLeft = (paperSize.Height - sumColumn) / 2;
                }
                else 
                {
                    return marginLeft = (paperSize.Width - sumColumn) / 2;
                }
                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void DrawTableHeader(Graphics g,int x,int y) 
        {
            try
            {
                if (dic_columns != null)
                {
                    Rectangle rect = new Rectangle();
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    rect.X = x;
                    rect.Y = y;
                    foreach (DataColumn itemColumn in dic_columns.Values)
                    {
                        rect.Height = lineHeight;//dic_columns[];
                        rect.Width = int.Parse(itemColumn.width);
                        g.DrawRectangle(Pens.Black, rect);
                        g.DrawString(itemColumn.caption.ToString(), fontDraw, Brushes.Black, rect,sf);
                        rect.Offset((int)rect.Width, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        void printPreviewControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                //处理鼠标滚动事件
                if (!SystemInformation.MouseWheelPresent)
                {
                    //没有安装鼠标滚轮
                    return;
                }

                int scrollAmount;
                float amount = Math.Abs(e.Delta) / SystemInformation.MouseWheelScrollDelta;
                amount *= SystemInformation.MouseWheelScrollLines;
                amount *= 12;//行高
                amount *= (float)printPreviewControl1.Zoom;//缩放量

                if (e.Delta < 0)
                {
                    scrollAmount = (int)amount;
                }
                else
                {
                    scrollAmount = -(int)amount;
                }

                Point curPos = (Point)(_Position.GetValue(printPreviewControl1));

                _SetPositionMethod.Invoke(printPreviewControl1, new object[] { new       

            Point (curPos.X + 0, curPos.Y + scrollAmount) });
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void printPreviewControl1_Click(object sender, EventArgs e)
        {
            try
            {
                printPreviewControl1.Select();
                printPreviewControl1.Focus();

            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
