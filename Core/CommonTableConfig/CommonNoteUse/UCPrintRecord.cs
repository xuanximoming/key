using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using XDesigner.Report;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    public partial class UCPrintRecord : DevExpress.XtraEditors.XtraUserControl
    {
        PrintInCommonView m_printInCommonView;  //用于打印的对象
        public UCPrintRecord()
        {
            try
            {

                InitializeComponent();
                SetMainToopStripFalse();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 打印控件按钮进行控制
        /// </summary>
        private void SetMainToopStripFalse()
        {
            XDesigner.Report.XPrintControlExt.ControlButtons butts = xPrint.Buttons;
            butts.MainToolStrip.Visible = true;
            butts.cmdExport.Visible = false;
            butts.cmdRefresh.Visible = false;
            butts.lblVersion2.Visible = false;
            butts.btnPageSettings.Visible = false;
            butts.cmdJumpPrint.Visible = true;
            butts.cmdPrint.Visible = true;


         
        }


        //根据PrintInCommonView进行判断调用哪个打印模板
        public void LoadPrint(PrintInCommonView printInCommonView)
        {
            try
            {
                m_printInCommonView = printInCommonView;

                PrintInCommonView printInCommonViewClone = AddLineCount();

                XDesigner.Report.DataBaseReportBuilder builder = new XDesigner.Report.DataBaseReportBuilder();
                builder.SetVariable("PrintInCommonView", new object[] { printInCommonViewClone });
                string path = Application.StartupPath.ToString();
                path += @"\Report\" + printInCommonViewClone.PrintFileName + ".xrp";
                if (path == null) return;
                bool hasfile = File.Exists(path);
                if (hasfile)
                {
                    builder.Load(path);
                    builder.Refresh();
                    xPrint.Document = builder.ReportDocument;
                    xPrint.RefreshView();
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("打印模板文件不存在");
                }
              
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 从界面获取空行添加
        /// </summary>
        private PrintInCommonView AddLineCount()
        {
            //复制一个一样的对象
            PrintInCommonView printInCommonViewClone = CommonTabHelper.Clone<PrintInCommonView>(m_printInCommonView);
            int linecount = Convert.ToInt32(spinEditLineCount.Value);
            if (linecount > 0)
            {
                if (printInCommonViewClone.PrintInCommonTabViewList1 == null)
                { printInCommonViewClone.PrintInCommonTabViewList1 = new PrintInCommonTabView(); }
                if (printInCommonViewClone.PrintInCommonTabViewList1.PrintInCommonItemViewList == null)
                {

                    printInCommonViewClone.PrintInCommonTabViewList1.PrintInCommonItemViewList = new List<PrintInCommonItemView>();
                }
                for (int i = 0; i < linecount; i++)
                {
                    printInCommonViewClone.PrintInCommonTabViewList1.PrintInCommonItemViewList.Add(new PrintInCommonItemView());
                }
                #region 原先错误的添加行
                //int maxColCount = InCommonNoteBiz.GetReportxmlCol(printInCommonViewClone);
                //if (maxColCount > 1 && maxColCount <= 5)
                //{
                //    if (maxColCount >= 2)
                //    {
                //        if (printInCommonViewClone.PrintInCommonTabViewList10 == null)
                //        { printInCommonViewClone.PrintInCommonTabViewList10 = new PrintInCommonTabView(); }
                //        if (printInCommonViewClone.PrintInCommonTabViewList10.PrintInCommonItemViewList == null)
                //        {

                //            printInCommonViewClone.PrintInCommonTabViewList10.PrintInCommonItemViewList = new List<PrintInCommonItemView>();
                //        }
                //        for (int i = 0; i < linecount; i++)
                //        {
                //            printInCommonViewClone.PrintInCommonTabViewList10.PrintInCommonItemViewList.Add(new PrintInCommonItemView());
                //        }
                //    }
                //    if (maxColCount >= 3)
                //    {
                //        if (printInCommonViewClone.PrintInCommonTabViewList9 == null)
                //        { printInCommonViewClone.PrintInCommonTabViewList9 = new PrintInCommonTabView(); }
                //        if (printInCommonViewClone.PrintInCommonTabViewList9.PrintInCommonItemViewList == null)
                //        {

                //            printInCommonViewClone.PrintInCommonTabViewList9.PrintInCommonItemViewList = new List<PrintInCommonItemView>();
                //        }
                //        for (int i = 0; i < linecount; i++)
                //        {
                //            printInCommonViewClone.PrintInCommonTabViewList9.PrintInCommonItemViewList.Add(new PrintInCommonItemView());
                //        }
                //    }
                //    if (maxColCount >= 4)
                //    {
                //        if (printInCommonViewClone.PrintInCommonTabViewList8 == null)
                //        { printInCommonViewClone.PrintInCommonTabViewList8 = new PrintInCommonTabView(); }
                //        if (printInCommonViewClone.PrintInCommonTabViewList8.PrintInCommonItemViewList == null)
                //        {

                //            printInCommonViewClone.PrintInCommonTabViewList8.PrintInCommonItemViewList = new List<PrintInCommonItemView>();
                //        }
                //        for (int i = 0; i < linecount; i++)
                //        {
                //            printInCommonViewClone.PrintInCommonTabViewList8.PrintInCommonItemViewList.Add(new PrintInCommonItemView());
                //        }
                //    } if (maxColCount >= 5)
                //    {
                //        if (printInCommonViewClone.PrintInCommonTabViewList7 == null)
                //        { printInCommonViewClone.PrintInCommonTabViewList7 = new PrintInCommonTabView(); }
                //        if (printInCommonViewClone.PrintInCommonTabViewList7.PrintInCommonItemViewList == null)
                //        {

                //            printInCommonViewClone.PrintInCommonTabViewList7.PrintInCommonItemViewList = new List<PrintInCommonItemView>();
                //        }
                //        for (int i = 0; i < linecount; i++)
                //        {
                //            printInCommonViewClone.PrintInCommonTabViewList7.PrintInCommonItemViewList.Add(new PrintInCommonItemView());
                //        }
                //  }
                //}
#endregion
            }
            return printInCommonViewClone;
        }


        /// <summary>
        /// 获取要添加的空行数，并刷新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPrint(m_printInCommonView);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            xPrint.InnerPreviewControl.PrintCurrentPage(true);
        }
    }
}
