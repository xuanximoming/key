using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>
    /// xll 2013-01-10 自定义打印的预览与打印 非报表工具
    /// </summary>
    public partial class PrintForm1 : DevBaseForm
    {
        public PrintInCommonView m_printInCommonView;
        IPrintNurse iPrintNurse;
        int printPageNowPer = 0;  //当前打印预览页  
        int printEnd = 0;
        PaperSize paperSize;
        bool Landscape = true;
        public PrintForm1(PrintInCommonView printInCommonView)
        {
            try
            {
                //<PrintPageSize width="1654" height="1169" type="A3"></PrintPageSize>
                InitializeComponent();
                InitPrintView(printInCommonView);
                speNum.Value = GetSpitValue();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="printInCommonView"></param>
        private void InitPrintView(PrintInCommonView printInCommonView)
        {
            printPageNowPer = 0;
            m_printInCommonView = printInCommonView;
            printPreviewControl1.Rows = 1;
            iPrintNurse = AbstractorFactry.GetNurseRecord(printInCommonView.PrintFileName);
            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"Report\" + printInCommonView.PrintFileName + ".xml";
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fileName);
            iPrintNurse.PageRecordCount = int.Parse(CommonMethods.GetElementAttribute("ImageSize", "rowCount", xmldoc));
            string PaperType = CommonMethods.GetElementAttribute("PrintPageSize", "type", xmldoc);
            int width;
            int heigth;
            int.TryParse(CommonMethods.GetElementAttribute("PrintPageSize", "width", xmldoc), out width);
            int.TryParse(CommonMethods.GetElementAttribute("PrintPageSize", "height", xmldoc), out heigth);
            Landscape = CommonMethods.GetElementAttribute("PrintPageSize", "Landscape", xmldoc).ToUpper() == "TRUE" ? true : false;

            PrintDocument printDocumentPre = new PrintDocument();
            PrintDialog MySettings = new PrintDialog();
            MySettings.Document = printDocumentPre;

            foreach (PaperSize ps in MySettings.Document.PrinterSettings.PaperSizes)
            {
                if (ps.Kind.ToString().ToUpper().Equals(PaperType.ToUpper()))//这里设置纸张大小,但必须是定义好的  
                {
                    paperSize = ps;
                    break;
                }
            }
            if (paperSize == null)
            {
                paperSize = new PaperSize();
                paperSize.PaperName = PaperType;
                paperSize.Width = width;
                paperSize.Height = heigth;
            }
            MySettings.Document.DefaultPageSettings.PaperSize = paperSize;
            MySettings.Document.DefaultPageSettings.Landscape = Landscape;

            //printDocumentPre.DefaultPageSettings.PaperSize = paperSize;

            int PageRecordCount = iPrintNurse.PageRecordCount; //每张的行数
            var PrintInCommonItemViewList = m_printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
            if (PrintInCommonItemViewList == null)
                PrintInCommonItemViewList = new List<PrintInCommonItemView>();
            int allcount = PrintInCommonItemViewList.Count;
            int pageCount = (allcount + PageRecordCount - 1) / PageRecordCount;  //总页数
            if (pageCount == 0)
                pageCount = 1;
            printEnd = pageCount;

            printDocumentPre.PrintPage += new PrintPageEventHandler(printDocumentPre_PrintPage);
            printPreviewControl1.Document = printDocumentPre;
            printPreviewControl1.Zoom = 1;

        }

        private int GetSpitValue()
        {
            try
            {
                string sql = string.Format("select * from incommonprintfrompage i where i.incommonnoteflow='{0}'", m_printInCommonView.IncommonNoteflow);
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt == null || dt.Rows == null || dt.Rows.Count <= 0) return 1;
                else
                {
                    return Convert.ToInt32(dt.Rows[0]["PAGEFROM"].ToString());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 打印全部事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printDocumentPre_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (m_printInCommonView == null
                       || m_printInCommonView.PrintInCommonTabViewList1 == null)
                {
                    return;
                }
                if (m_printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList == null)
                {
                    m_printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList = new List<PrintInCommonItemView>();
                }
                int PageRecordCount = iPrintNurse.PageRecordCount; //每张的行数
                var PrintInCommonItemViewList = m_printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
                int allcount = PrintInCommonItemViewList.Count;
                int pageCount = (allcount + PageRecordCount - 1) / PageRecordCount;  //总页数
                if (pageCount == 0)
                    pageCount = 1;
                List<PrintInCommonItemView> printInCommonItemViewEven = new List<PrintInCommonItemView>();
                for (int j = printPageNowPer * PageRecordCount; j < ((printPageNowPer + 1) * PageRecordCount); j++)
                {
                    if (j < allcount)
                    {
                        PrintInCommonItemViewList[j].RowNum = j + 1;
                        InCommonNoteBiz.ConvertForImgRec(PrintInCommonItemViewList[j]);
                        printInCommonItemViewEven.Add(PrintInCommonItemViewList[j]);
                    }
                }
                InCommonNoteBiz.ConvertForDateTime(printInCommonItemViewEven);
                InCommonNoteBiz.SetRowEnd(printInCommonItemViewEven, m_printInCommonView.PrintFileName);
                RecordPrintView recordPrintView = ConvertPrintView(m_printInCommonView);
                recordPrintView.PrintInpatientView.ListCount = printInCommonItemViewEven.Count.ToString();
                int Addpage = Convert.ToInt32(speNum.Value);
                recordPrintView.PrintInpatientView.CurrPage = (printPageNowPer + Addpage).ToString();
                recordPrintView.PrintInCommonItemViewList = printInCommonItemViewEven;
                iPrintNurse.GetPreview(recordPrintView, e.Graphics);
                if (printPageNowPer < pageCount - 1 && printPageNowPer < printEnd - 1)
                {
                    e.HasMorePages = true;
                    printPreviewControl1.Rows += 1;
                    printPageNowPer++;
                    return;
                }
                else
                {
                    spinEditPage.Properties.MinValue = (int)speNum.Value;
                    spinEditPage.Properties.MaxValue = pageCount + (int)speNum.Value - 1;
                    printPageNowPer = 0;
                    e.HasMorePages = false;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            printPreviewControl1.Zoom = Convert.ToDouble(trackBarControl1.Value) / 100;
        }

        /// <summary>
        /// 打印全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDocument printDocumentPrint = new PrintDocument();
                printDocumentPrint.PrintPage += new PrintPageEventHandler(printDocumentPre_PrintPage);
                PrintDialog MySettings = new PrintDialog();
                MySettings.AllowSomePages = true;
                MySettings.AllowCurrentPage = false;
                MySettings.Document = printDocumentPrint;
                MySettings.Document.DefaultPageSettings.PaperSize = paperSize;
                MySettings.Document.DefaultPageSettings.Landscape = Landscape;
                //xll 2013-06-17 添加双面打印支持
                if (cboDuplex.Text == "上下翻转双面打印")
                {
                    MySettings.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
                }
                else if (cboDuplex.Text == "左右翻转双面打印")
                {
                    MySettings.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                }
                else
                {
                    MySettings.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                }

                MySettings.Document.DefaultPageSettings.PrinterSettings.FromPage = Convert.ToInt32(spinEditPage.Properties.MinValue);
                MySettings.Document.DefaultPageSettings.PrinterSettings.MinimumPage = Convert.ToInt32(spinEditPage.Properties.MinValue);
                MySettings.Document.DefaultPageSettings.PrinterSettings.MaximumPage = Convert.ToInt32(spinEditPage.Properties.MaxValue);
                MySettings.Document.DefaultPageSettings.PrinterSettings.ToPage = Convert.ToInt32(spinEditPage.Properties.MaxValue);

                if (MySettings.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                if (MySettings.Document.DefaultPageSettings.PrinterSettings.PrintRange == PrintRange.SomePages)
                {
                    printPageNowPer = MySettings.PrinterSettings.FromPage - (int)speNum.Value;
                    printEnd = MySettings.PrinterSettings.ToPage - (int)speNum.Value + 1;
                }
                else if (MySettings.Document.DefaultPageSettings.PrinterSettings.PrintRange == PrintRange.AllPages)
                {
                    printPageNowPer = (int)spinEditPage.Properties.MinValue - (int)speNum.Value;
                    printEnd = (int)spinEditPage.Properties.MaxValue - (int)speNum.Value + 1;
                }
                printPreviewControl1.Rows = 1;
                //printDocumentPrint.DefaultPageSettings.PaperSize = paperSize;
                //printDocumentPrint.DefaultPageSettings.Landscape = Landscape;
                // printDocumentPrint.PrinterSettings = MySettings.PrinterSettings;
                printDocumentPrint.Print();
                int startPage = Convert.ToInt32(spinEditPage.Properties.MinValue); ;
                int endPage = Convert.ToInt32(spinEditPage.Properties.MaxValue);
                if (MySettings.Document.DefaultPageSettings.PrinterSettings.PrintRange == PrintRange.SomePages)
                {
                    startPage = MySettings.Document.DefaultPageSettings.PrinterSettings.FromPage;
                    endPage = MySettings.Document.DefaultPageSettings.PrinterSettings.ToPage;
                }
                AddPrintHistory(startPage, endPage, MySettings.Document.DefaultPageSettings.PrinterSettings.Copies);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 打印指定列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintNow_Click(object sender, EventArgs e)
        {

            try
            {
                PrintDocument printDocumentPrintNow = new PrintDocument();
                printDocumentPrintNow.PrintPage += new PrintPageEventHandler(printDocumentPrintNow_PrintPage);
                printDocumentPrintNow.DefaultPageSettings.PaperSize = paperSize;
                printDocumentPrintNow.DefaultPageSettings.Landscape = Landscape;
                printDocumentPrintNow.Print();
                AddPrintHistory(Convert.ToInt32(spinEditPage.Value), Convert.ToInt32(spinEditPage.Value), 1);

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message + ex.StackTrace);
            }
        }


        /// <summary>
        /// xll 添加历史打印记录
        /// </summary>
        /// <param name="startpage"></param>
        /// <param name="endpage"></param>
        /// <param name="printpages"></param>
        private void AddPrintHistory(int startpage, int endpage, int printpages)
        {
            try
            {
                PrintHistoryEntity printHistoryEntity = new PrintHistoryEntity();
                printHistoryEntity.PrintRecordFlow = m_printInCommonView.IncommonNoteflow;
                printHistoryEntity.StartPage = startpage;
                printHistoryEntity.EndPage = endpage;
                printHistoryEntity.PrintPages = printpages;
                printHistoryEntity.PrintType = "1";
                DrectSoft.Common.PrintHistoryHistory.AddrintHistory(printHistoryEntity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 打印指定列事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void printDocumentPrintNow_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                if (m_printInCommonView == null
                       || m_printInCommonView.PrintInCommonTabViewList1 == null)
                {
                    return;
                }
                if (m_printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList == null)
                {
                    m_printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList = new List<PrintInCommonItemView>();
                }
                var PrintInCommonItemViewList = m_printInCommonView.PrintInCommonTabViewList1.PrintInCommonItemViewList;
                int allcount = PrintInCommonItemViewList.Count;
                int PageRecordCount = iPrintNurse.PageRecordCount; //每张的行数
                int pageIndex = (int)spinEditPage.Value - (int)speNum.Value + 1;  //实际打印页码需要重新计算
                List<PrintInCommonItemView> printInCommonItemViewEven = new List<PrintInCommonItemView>();
                for (int j = (pageIndex - 1) * PageRecordCount; j < (pageIndex * PageRecordCount); j++)
                {
                    if (j < allcount)
                    {
                        PrintInCommonItemViewList[j].RowNum = j + 1;
                        printInCommonItemViewEven.Add(PrintInCommonItemViewList[j]);
                    }
                }
                InCommonNoteBiz.ConvertForDateTime(printInCommonItemViewEven);
                InCommonNoteBiz.SetRowEnd(printInCommonItemViewEven, m_printInCommonView.PrintFileName);
                RecordPrintView recordPrintView = ConvertPrintView(m_printInCommonView);
                recordPrintView.PrintInpatientView.ListCount = printInCommonItemViewEven.Count.ToString();

                int Addpage = Convert.ToInt32(speNum.Value);
                recordPrintView.PrintInpatientView.CurrPage = (pageIndex + Addpage - 1).ToString();

                recordPrintView.PrintInCommonItemViewList = printInCommonItemViewEven;
                iPrintNurse.GetPreview(recordPrintView, e.Graphics);
                e.HasMorePages = false;

            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message + ex.StackTrace);
            }
        }



        /// <summary>
        /// 装换对象RecordPrintView
        /// </summary>
        /// <param name="printInCommonView"></param>
        /// <returns></returns>
        RecordPrintView ConvertPrintView(PrintInCommonView printInCommonView)
        {
            RecordPrintView recordPrintView = new RecordPrintView();
            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"Report\" + printInCommonView.PrintFileName + ".xml";
            recordPrintView.PrintFileName = fileName;
            recordPrintView.RecordName = printInCommonView.RecordName;
            recordPrintView.PrintInpatientView = printInCommonView.PrintInpatientView;
            recordPrintView.PrintInCommonTabView = printInCommonView.PrintInCommonTabViewList1;
            if (printInCommonView.PrintInCommonTabViewList2 != null
                && printInCommonView.PrintInCommonTabViewList2.PrintInCommonItemViewList != null
                && printInCommonView.PrintInCommonTabViewList2.PrintInCommonItemViewList.Count > 0)
            {
                recordPrintView.PrintInCommonItemViewOther = printInCommonView.PrintInCommonTabViewList2.PrintInCommonItemViewList[0];
            }
            if (recordPrintView.PrintInCommonItemViewList == null)
            {
                recordPrintView.PrintInCommonItemViewList = new List<PrintInCommonItemView>();
            }
            if (recordPrintView.PrintInCommonItemViewOther == null)
            {
                recordPrintView.PrintInCommonItemViewOther = new PrintInCommonItemView();
            }

            return recordPrintView;
        }

        private void speNum_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshView();
                AddOrModPageFrom();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 插入选中页码
        /// </summary>
        private void AddOrModPageFrom()
        {
            try
            {
                SqlParameter[] sps ={
                                   new SqlParameter("@incommonnoteflow",SqlDbType.VarChar,50),
                                   new SqlParameter("@PageFrom",SqlDbType.VarChar,50)
                                   };
                sps[0].Value = m_printInCommonView.IncommonNoteflow;
                sps[1].Value = ((int)speNum.Value).ToString();
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery("emr_commonnote.usp_AddOrModIncommPagefrom", sps, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void RefreshView()
        {
            try
            {
                InitPrintView(m_printInCommonView);
                printPreviewControl1.InvalidatePreview();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnPrintHistory_Click(object sender, EventArgs e)
        {
            try
            {
                PrintHistoryForm printHistoryForm = new PrintHistoryForm(m_printInCommonView.IncommonNoteflow, "1");
                printHistoryForm.ShowDialog();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
