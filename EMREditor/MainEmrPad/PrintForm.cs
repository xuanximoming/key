using DevExpress.Utils;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.Util;
using DrectSoft.Library.EmrEditor.Src.Print;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class PrintForm : DevBaseForm
    {
        PatRecUtil m_PatUtil;
        EmrModel m_Model;
        int m_DefaultPageIndex;
        XmlDocument m_ActualEmrDoc = new XmlDocument();//包含修改痕迹的病历
        XmlDocument m_ClearEmrDoc = new XmlDocument();//不含修改痕迹的病历【简洁病历】
        bool m_IsTaoDa = false;//是否是套打
        string m_noofinpat = string.Empty;

        PrintDocument m_PrintDocument;//add by ywk 2012年10月24日 10:30:07 

        /// <summary>
        /// 当前界面选中的编辑器
        /// </summary>
        PadForm CurrentForm
        {
            get
            {
                if (panelControlPrintReview.Controls.Count > 0)
                {
                    return panelControlPrintReview.Controls[0] as PadForm;
                }
                return null;
            }
        }

        public PrintForm(PatRecUtil patUtil, EmrModel model, int defaultPageIndex, string noofinpat)
        {
            InitializeComponent();
            //this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;
            m_PatUtil = patUtil;
            m_Model = model;
            AddPadForm();
            InitSpinEdit();
            m_DefaultPageIndex = defaultPageIndex;
            m_noofinpat = noofinpat;
            m_PrintDocument = new PrintDocument();

            //m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);
            //checkEditPrintSelectArea.Visible = false;
        }

        private void InitSpinEdit()
        {
            spinEditBegin.Properties.MaxValue = CurrentForm.zyEditorControl1.EMRDoc.Pages.Count;
            spinEditEnd.Properties.MaxValue = CurrentForm.zyEditorControl1.EMRDoc.Pages.Count;
            spinEditBegin.EditValue = 1;
            spinEditEnd.EditValue = CurrentForm.zyEditorControl1.EMRDoc.Pages.Count;
        }

        private void AddPadForm()
        {
            PadForm pad = new PadForm(m_PatUtil, null);
            pad.Dock = DockStyle.Fill;
            //pad.BackColor = Color.White;//打印还是白色
            pad.SetBackColor();
            InitEmrDoc();
            pad.LoadDocment(m_Model);
            this.Text = "打印预览--" + m_Model.ModelName;
            panelControlPrintReview.Controls.Add(pad);
            SetEmrDocumentModel();
        }

        private void InitEmrDoc()
        {
            m_ActualEmrDoc = m_Model.ModelContent;

            m_ClearEmrDoc = new XmlDocument();
            m_ClearEmrDoc.PreserveWhitespace = true;
            m_ClearEmrDoc.LoadXml(m_Model.ModelContent.InnerXml);
            var xmlnode = m_ClearEmrDoc.GetElementsByTagName("span");
            for (int i = 0; i < xmlnode.Count; i++)
            {
                if (((XmlElement)xmlnode[i]).HasAttribute("deleter"))
                {
                    ((XmlElement)xmlnode[i]).RemoveAll();
                }
            }
            RemoveNoPrintButton(m_ClearEmrDoc);
            m_Model.ModelContent = m_ClearEmrDoc;//默认不显示修改痕迹
        }

        /// <summary>
        /// 移除不打印的Button元素
        /// </summary>
        private void RemoveNoPrintButton(XmlDocument doc)
        {
            XmlNodeList nodelist = doc.GetElementsByTagName("btnelement");
            List<XmlNode> list = new List<XmlNode>();
            foreach (XmlNode node in nodelist)
            {
                XmlAttribute attrType = node.Attributes["type"];
                XmlAttribute attrPrint = node.Attributes["print"];
                if (attrType.Value.ToLower() == "button" && attrPrint.Value.ToLower() == "false")
                {
                    list.Add(node);
                }
            }
            int count = list.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                XmlNode node = list[i];
                node.ParentNode.RemoveChild(node);
            }
        }

        /// <summary>
        /// 设置编辑器的编辑模式
        /// </summary>
        private void SetEmrDocumentModel()
        {
            if (CurrentForm != null)
            {
                CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Clear;//默认不显示修改痕迹
                //CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Read;
                CurrentForm.zyEditorControl1.EMRDoc.Info.ShowParagraphFlag = false;
            }
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            InitPrinterList();
            InitComboBoxPage();
            checkEditPrintAll.Checked = true;
            this.WindowState = FormWindowState.Maximized;
            CurrentForm.zyEditorControl1.CurrentPage = CurrentForm.zyEditorControl1.Pages[m_DefaultPageIndex - 1];

            //直接进打印方法 注册事件 
            m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);

            SetButtonEnable();
            SetPageIndexForNurseRecord();
        }

        /// <summary>
        /// 初始化打印机列表
        /// </summary>
        private void InitPrinterList()
        {
            DataTable dataTablePrinter = new DataTable();
            dataTablePrinter.Columns.Add("PrintName");

            PrintDocument prtdoc = new PrintDocument();
            string strDefaultPrinter = prtdoc.PrinterSettings.PrinterName;//獲取默認的打印機名
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                DataRow dr = dataTablePrinter.NewRow();
                dr["PrintName"] = strPrinter;
                dataTablePrinter.Rows.Add(dr);
            }
            lookUpEditPrinter.Properties.DataSource = dataTablePrinter;
            lookUpEditPrinter.Properties.DisplayMember = "PrintName";
            lookUpEditPrinter.Properties.ValueMember = "PrintName";
            lookUpEditPrinter.EditValue = strDefaultPrinter;
        }

        #region CheckEdit
        private void checkEditPrintAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditPrintAll.Checked)
            {
                checkEditPrintPartial.Checked = false;
                spinEditBegin.Enabled = false;
                spinEditEnd.Enabled = false;
                checkEditPrintSelection.Checked = false;
                checkEditXuDa.Checked = false;
                checkEditPrintCurrentPage.Checked = false;
                checkEditPrintSelectArea.Checked = false;
                checkEditPrintDuplex.Checked = false;//双面打印去除
                JumpPrint(false);
                SelectAreaPrint(false);
            }
        }

        private void checkEditPrintPartial_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditPrintPartial.Checked)
            {
                checkEditPrintAll.Checked = false;
                spinEditBegin.Enabled = true;
                spinEditEnd.Enabled = true;
                checkEditPrintSelection.Checked = false;
                checkEditXuDa.Checked = false;
                checkEditPrintCurrentPage.Checked = false;
                checkEditPrintSelectArea.Checked = false;
                checkEditPrintDuplex.Checked = false;//双面打印去除
                JumpPrint(false);
                SelectAreaPrint(false);
            }
            else
            {
                spinEditBegin.Enabled = false;
                spinEditEnd.Enabled = false;
            }
        }

        private void checkEditPrintSelection_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditPrintSelection.Checked)
            {
                checkEditPrintAll.Checked = false;
                checkEditPrintPartial.Checked = false;
                spinEditBegin.Enabled = false;
                spinEditEnd.Enabled = false;
                checkEditXuDa.Checked = false;
                checkEditPrintCurrentPage.Checked = false;
                checkEditPrintSelectArea.Checked = false;
                checkEditPrintDuplex.Checked = false;//双面打印去除
                JumpPrint(false);
                SelectAreaPrint(false);
            }
        }

        private void checkEditXuDa_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditXuDa.Checked)
            {
                checkEditPrintAll.Checked = false;
                checkEditPrintPartial.Checked = false;
                spinEditBegin.Enabled = false;
                spinEditEnd.Enabled = false;
                checkEditPrintSelection.Checked = false;
                checkEditPrintCurrentPage.Checked = false;
                checkEditPrintSelectArea.Checked = false;
                checkEditPrintDuplex.Checked = false;//双面打印去除
                JumpPrint(true);
                SelectAreaPrint(false);
            }
            else
            {
                JumpPrint(false);
            }
        }

        private void checkEditPrintCurrentPage_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditPrintCurrentPage.Checked)
            {
                checkEditPrintAll.Checked = false;
                checkEditPrintPartial.Checked = false;
                spinEditBegin.Enabled = false;
                spinEditEnd.Enabled = false;
                checkEditPrintSelection.Checked = false;
                checkEditXuDa.Checked = false;
                checkEditPrintSelectArea.Checked = false;
                checkEditPrintDuplex.Checked = false;//双面打印去除
                JumpPrint(false);
                SelectAreaPrint(false);
            }
        }

        private void checkEditPrintSelectArea_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditPrintSelectArea.Checked)
            {
                checkEditPrintAll.Checked = false;
                checkEditPrintPartial.Checked = false;
                spinEditBegin.Enabled = false;
                spinEditEnd.Enabled = false;
                checkEditPrintSelection.Checked = false;
                checkEditPrintCurrentPage.Checked = false;
                checkEditXuDa.Checked = false;
                checkEditPrintDuplex.Checked = false;//双面打印去除
                JumpPrint(false);
                SelectAreaPrint(true);
            }
            else
            {
                SelectAreaPrint(false);
            }
        }
        #endregion


        private void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(CurrentForm.zyEditorControl1.ToXMLString());
            XmlDocument docClone = (XmlDocument)doc.Clone();

            #region Before Print
            if (m_IsTaoDa)
            {
                XmlNodeList nodeList = doc.GetElementsByTagName("table");
                foreach (XmlNode node in nodeList)
                {
                    if (node.Attributes["hidden-all-border"] != null)
                    {
                        node.Attributes["hidden-all-border"].Value = "1";
                    }
                }
                nodeList = doc.GetElementsByTagName("table-cell");
                foreach (XmlNode node in nodeList)
                {
                    if (node.Attributes["border-width-top"] != null)
                    {
                        node.Attributes["border-width-top"].Value = "0";
                    }
                    if (node.Attributes["border-width-right"] != null)
                    {
                        node.Attributes["border-width-right"].Value = "0";
                    }
                    if (node.Attributes["border-width-bottom"] != null)
                    {
                        node.Attributes["border-width-bottom"].Value = "0";
                    }
                    if (node.Attributes["border-width-left"] != null)
                    {
                        node.Attributes["border-width-left"].Value = "0";
                    }
                }
                CurrentForm.zyEditorControl1.LoadXML(doc);
            }
            #endregion

            #region Print

            #region 双面打印
            bool ISDOUBLE = false;//是否双面打印
            //增加双面打印选项 add by ywk 2012年10月24日 10:26:30
            if (checkEditPrintDuplex.Checked)
            {
                if (!checkEditPrintPartial.Checked)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请先设定打印页范围！");
                    return;
                }

                m_PrintDocument.PrinterSettings.PrinterName = lookUpEditPrinter.EditValue.ToString();
                PageSetupDialog pageSetupDialog = new PageSetupDialog();
                pageSetupDialog.Document = m_PrintDocument;
                pageSetupDialog.Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);

                if (checkEditPrintDuplex.Checked)
                {
                    //if (pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.CanDuplex)
                    //{
                    //}
                    //else
                    //{
                    //    MessageBox.Show("此打印机不支持双面打印!", "警告");
                    //    checkEditPrintDuplex.Checked = false;
                    //    return;
                    //}
                    ISDOUBLE = true;

                    //int StartprintPage = Convert.ToInt32(spinEditBegin.EditValue); //开始打印的页数
                    //int EndprintPage = Convert.ToInt32(spinEditEnd.EditValue); //结束打印的页数

                    if (rabLeftRight.Checked)//左右翻转
                    {
                        pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
                        m_PrintDocument.Print();
                    }
                    else if (rabUpDown.Checked)//上下翻转
                    {
                        pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                        m_PrintDocument.Print();
                    }
                    else//什么都不选中，默认
                    {
                        pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                        m_PrintDocument.Print();
                    }
                }

                #region 双面打印更换方法，先转换成图片 ,参照病案首页的逻辑 add by ywk 2013年1月24日10:29:21
                //PageSetupDialog pageSetupDialog = new PageSetupDialog();
                //pageSetupDialog.Document = m_PrintDocument;

                //pageSetupDialog.Document.DefaultPageSettings.PaperSize = CurrentForm.zyEditorControl1.EMRDoc.PageSettings.PaperSize;

                ////双面打印逻辑放在指定页数的前面  add by ywk 2013年1月7日10:45:27 
                ////pageSetupDialog.Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                //if (pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.CanDuplex)
                //{
                //    XDesignerPrinting.XPrintDocument new_doc = new XDesignerPrinting.XPrintDocument();

                //    //add by ywk 2012年10月23日 19:59:34  要求左右上下反转打印
                //    if (rabLeftRight.Checked)//左右翻转
                //    {
                //        new_doc.PrinterSettings.Duplex = Duplex.Vertical;
                //    }
                //    else if (rabUpDown.Checked)//上下翻转
                //    {
                //        new_doc.PrinterSettings.Duplex = Duplex.Horizontal;
                //    }
                //    else//一个都不选择就是默认
                //    {
                //        new_doc.PrinterSettings.Duplex = Duplex.Simplex;
                //    }
                //    //MessageBox.Show("第一次翻转方向:" + new_doc.PrinterSettings.Duplex.ToString());
                //    if (Convert.ToInt32(spinEditBegin.EditValue) > Convert.ToInt32(spinEditEnd.EditValue))
                //    {
                //        int temp = Convert.ToInt32(spinEditBegin.EditValue);
                //        spinEditBegin.EditValue = spinEditEnd.EditValue;
                //        spinEditEnd.EditValue = temp;
                //    }
                //    for (int j = 0; j < Convert.ToInt32(spinEditPrintCount.EditValue); j++)
                //    {
                //        //不用指定页数的打印
                //        CurrentForm.zyEditorControl1.EMRDoc._PrintPageFromTo( lookUpEditPrinter.EditValue.ToString(),
                //            Convert.ToInt32(spinEditBegin.EditValue), Convert.ToInt32(spinEditEnd.EditValue), true, new_doc.PrinterSettings.Duplex);
                //        CurrentForm.zyEditorControl1.Refresh();

                //        ISDOUBLE = true;
                //    }
                #endregion
                //PageSetupDialog pageSetupDialog = new PageSetupDialog();
                //pageSetupDialog.Document = m_PrintDocument;
                //if (pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.CanDuplex)
                //{
                //    List<Bitmap> images = CurrentForm.zyEditorControl1.EMRDoc.GeneratePrintImage();

                //    List<Metafile> mers = new List<Metafile>();
                //    string folder = CreateFolder();
                //    Graphics[] grs = new Graphics[images.Count];
                //    for (int j = 0; j < images.Count; j++)
                //    {
                //        grs[j] = Graphics.FromImage(images[j]);
                //        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, images[j].Width, images[j].Height);
                //        mers.Add(new Metafile(folder + Guid.NewGuid().ToString() + ".wmf", grs[j].GetHdc(), rect, MetafileFrameUnit.Pixel));
                //        grs[j] = Graphics.FromImage(mers[j]);
                //        grs[j].SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //        //grs[j].Clear(Color.White);
                //        grs[j].Save();
                //        //grs[j].Dispose();
                //    }
                //    int StartprintPage = Convert.ToInt32(spinEditBegin.EditValue); //开始打印的页数
                //    int EndprintPage = Convert.ToInt32(spinEditEnd.EditValue); //结束打印的页数
                //}
                //                //}
                //else
                //{
                //    //MessageBox.Show("此打印机不支持双面打印!", "警告");
                //    Common.Ctrs.DLG.MessageBox.Show("此打印机不支持双面打印！");
                //    checkEditPrintDuplex.Checked = false;
                //    return;
                //}
            }
            #endregion
            #region 无双面打印
            if (!ISDOUBLE)//没进行双面打印
            {
                for (int i = 0; i < Convert.ToInt32(spinEditPrintCount.EditValue); i++)
                {
                    if (checkEditPrintAll.Checked)//全部打印
                    {
                        CurrentForm.zyEditorControl1.EMRDoc._PrintAll(lookUpEditPrinter.EditValue.ToString());
                    }
                    else if (checkEditPrintCurrentPage.Checked)//打印当前页
                    {
                        CurrentForm.zyEditorControl1.EMRDoc._PrintPageFromTo(lookUpEditPrinter.EditValue.ToString(),
                            CurrentForm.zyEditorControl1.CurrentPage.PageIndex,
                            CurrentForm.zyEditorControl1.CurrentPage.PageIndex);
                    }
                    #region 注释掉双面打印
                    ////增加双面打印选项 add by ywk 2012年10月24日 10:26:30
                    //else if (checkEditPrintDuplex.Checked)
                    //{
                    //    if (!checkEditPrintPartial.Checked)
                    //    {
                    //        Common.Ctrs.DLG.MessageBox.Show("请先设定打印页范围！");
                    //        return;
                    //    }
                    //    PageSetupDialog pageSetupDialog = new PageSetupDialog();
                    //    pageSetupDialog.Document = m_PrintDocument;

                    //    pageSetupDialog.Document.DefaultPageSettings.PaperSize = CurrentForm.zyEditorControl1.EMRDoc.PageSettings.PaperSize;
                    //    //双面打印逻辑放在指定页数的前面  add by ywk 2013年1月7日10:45:27 
                    //    //pageSetupDialog.Document.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
                    //    if (pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.CanDuplex)
                    //    {
                    //        //add by ywk 2012年10月23日 19:59:34  要求左右上下反转打印
                    //        if (rabLeftRight.Checked)//左右翻转
                    //        {
                    //            pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;

                    //            //CurrentForm.zyEditorControl1.EMRDoc._Print();//(lookUpEditPrinter.EditValue.ToString());
                    //            //CurrentForm.zyEditorControl1.EMRDoc._PrintSelection(lookUpEditPrinter.EditValue.ToString());

                    //            //CurrentForm.zyEditorControl1.Refresh();
                    //        }
                    //        else if (rabUpDown.Checked)//上下翻转
                    //        {
                    //            pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                    //        }
                    //        else//一个都不选择就是默认
                    //        {
                    //            pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;

                    //        }
                    //        if (Convert.ToInt32(spinEditBegin.EditValue) > Convert.ToInt32(spinEditEnd.EditValue))
                    //        {
                    //            int temp = Convert.ToInt32(spinEditBegin.EditValue);
                    //            spinEditBegin.EditValue = spinEditEnd.EditValue;
                    //            spinEditEnd.EditValue = temp;
                    //        }
                    //        for (int j   = 0; j < Convert.ToInt32(spinEditPrintCount.EditValue); j++)
                    //        {
                    //            if (rabLeftRight.Checked)//左右翻转
                    //            {
                    //                pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Vertical;
                    //            }
                    //            else if (rabUpDown.Checked)//上下翻转
                    //            {
                    //                pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Horizontal;
                    //            }
                    //            else//什么都不选中，默认
                    //            {
                    //                pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.Duplex = Duplex.Simplex;
                    //            }


                    //            //不用指定页数的打印
                    //            CurrentForm.zyEditorControl1.EMRDoc._PrintPageFromTo(lookUpEditPrinter.EditValue.ToString(),
                    //                Convert.ToInt32(spinEditBegin.EditValue), Convert.ToInt32(spinEditEnd.EditValue));
                    //            CurrentForm.zyEditorControl1.Refresh();
                    //        }
                    //        //不用指定页数的打印
                    //        //CurrentForm.zyEditorControl1.EMRDoc._PrintPageFromTo(lookUpEditPrinter.EditValue.ToString(),
                    //        //    Convert.ToInt32(spinEditBegin.EditValue), Convert.ToInt32(spinEditEnd.EditValue));
                    //        //CurrentForm.zyEditorControl1.Refresh();
                    //    }
                    //    else
                    //    {
                    //        //MessageBox.Show("此打印机不支持双面打印!", "警告");
                    //        Common.Ctrs.DLG.MessageBox.Show("此打印机不支持双面打印！");
                    //        checkEditPrintDuplex.Checked = false;
                    //        return;
                    //    }
                    //}
                    #endregion
                    else if (checkEditPrintPartial.Checked)//指定页数打印
                    {
                        if (Convert.ToInt32(spinEditBegin.EditValue) > Convert.ToInt32(spinEditEnd.EditValue))
                        {
                            int temp = Convert.ToInt32(spinEditBegin.EditValue);
                            spinEditBegin.EditValue = spinEditEnd.EditValue;
                            spinEditEnd.EditValue = temp;
                        }
                        CurrentForm.zyEditorControl1.EMRDoc._PrintPageFromTo(lookUpEditPrinter.EditValue.ToString(),
                            Convert.ToInt32(spinEditBegin.EditValue), Convert.ToInt32(spinEditEnd.EditValue));
                    }
                    else if (checkEditPrintSelection.Checked)//选中元素打印
                    {
                        if (CurrentForm.zyEditorControl1.EMRDoc.Content.SelectLength == 0)
                        {
                            m_PatUtil.m_app.CustomMessageBox.MessageShow("没有选中任何内容!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                            break;
                        }
                        CurrentForm.zyEditorControl1.EMRDoc._PrintSelection(lookUpEditPrinter.EditValue.ToString());
                        CurrentForm.zyEditorControl1.Refresh();
                    }
                    else if (checkEditXuDa.Checked)//续打
                    {
                        CurrentForm.zyEditorControl1.EMRDoc._PrintJump(lookUpEditPrinter.EditValue.ToString());
                    }
                    else if (checkEditPrintSelectArea.Checked)//打印选中区域
                    {
                        CurrentForm.zyEditorControl1.EMRDoc._PrintSelectArea(lookUpEditPrinter.EditValue.ToString());
                    }
                }
            }
            #endregion

            #endregion

            #region End Print
            if (m_IsTaoDa)
            {
                CurrentForm.zyEditorControl1.LoadXML(docClone);
            }
            #endregion
        }

        private int ADDIndex = 1; //索引值
        /// <summary>
        /// 为双面打印写的打印方法 
        /// add by ywk  2013年1月24日11:20:13  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                List<Metafile> images = CurrentForm.zyEditorControl1.EMRDoc.GeneratePrintImage2();

                int StartprintPage = Convert.ToInt32(spinEditBegin.EditValue); //开始打印的页数
                int EndprintPage = Convert.ToInt32(spinEditEnd.EditValue); //结束打印的页数
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                for (int k = 0; k < Convert.ToInt32(spinEditPrintCount.EditValue); k++)
                {
                    if (ADDIndex > EndprintPage)
                    {
                        //ADDIndex = EndprintPage;
                        ADDIndex = 1;
                        e.HasMorePages = false;
                    }
                    else if (ADDIndex >= 1 && ADDIndex < EndprintPage)
                    {

                        e.Graphics.DrawImage(images[StartprintPage + ADDIndex - 2], new RectangleF(0, 0, images[StartprintPage + ADDIndex - 2].Width, images[StartprintPage + ADDIndex - 2].Height));
                        ADDIndex++;
                        e.HasMorePages = true;
                    }

                    else if (ADDIndex == EndprintPage)//索引循环到了等于终止的页数,直接打印最后一页
                    {

                        e.Graphics.DrawImage(images[EndprintPage - 1], new RectangleF(0, 0, images[EndprintPage - 1].Width, images[EndprintPage - 1].Height));
                        ADDIndex++;
                        e.HasMorePages = false;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void JumpPrint(bool isJumpPrint)
        {
            if (CurrentForm == null) return;
            if (isJumpPrint != CurrentForm.zyEditorControl1.EMRDoc.EnableJumpPrint)
            {
                CurrentForm.zyEditorControl1.EMRDoc.EnableJumpPrint = isJumpPrint;
                CurrentForm.zyEditorControl1.Refresh();
            }
        }

        private void SelectAreaPrint(bool isSelectAreaPrint)
        {
            if (CurrentForm == null) return;
            if (isSelectAreaPrint != CurrentForm.zyEditorControl1.EMRDoc.EnableSelectAreaPrint)
            {
                CurrentForm.zyEditorControl1.EMRDoc.EnableSelectAreaPrint = isSelectAreaPrint;
                CurrentForm.zyEditorControl1.Refresh();
            }
        }

        private void spinEditBegin_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(spinEditBegin.EditValue) <= 0)
            {
                spinEditBegin.EditValue = 1;
            }
        }

        private void spinEditEnd_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(spinEditBegin.EditValue) > CurrentForm.zyEditorControl1.EMRDoc.Pages.Count)
            {
                spinEditBegin.EditValue = CurrentForm.zyEditorControl1.EMRDoc.Pages.Count;
            }
        }

        public void SetButtonEnable()
        {
            if (CurrentForm.zyEditorControl1.Pages.Count == 1)
            {
                simpleButtonFirstPage.Enabled = false;
                simpleButtonLastPage.Enabled = false;
                simpleButtonNextPage.Enabled = false;
                simpleButtonEndPage.Enabled = false;
            }
            //最后一页
            else if (CurrentForm.zyEditorControl1.CurrentPage.PageIndex == CurrentForm.zyEditorControl1.Pages.Count)
            {
                simpleButtonFirstPage.Enabled = true;
                simpleButtonLastPage.Enabled = true;
                simpleButtonNextPage.Enabled = false;
                simpleButtonEndPage.Enabled = false;
            }
            else if (CurrentForm.zyEditorControl1.CurrentPage.PageIndex == 1)
            {
                simpleButtonFirstPage.Enabled = false;
                simpleButtonLastPage.Enabled = false;
                simpleButtonNextPage.Enabled = true;
                simpleButtonEndPage.Enabled = true;
            }
            else
            {
                simpleButtonFirstPage.Enabled = true;
                simpleButtonLastPage.Enabled = true;
                simpleButtonNextPage.Enabled = true;
                simpleButtonEndPage.Enabled = true;
            }
            textEditCurrentPage.Text = "第 " + CurrentForm.zyEditorControl1.CurrentPage.PageIndex.ToString() + " 页/共 " + CurrentForm.zyEditorControl1.Pages.Count + " 页";
        }

        private void simpleButtonFirstPage_Click(object sender, EventArgs e)
        {
            //CurrentForm.zyEditorControl1.CurrentPage = CurrentForm.zyEditorControl1.Pages[0];
            CurrentForm.zyEditorControl1.FirstPage();
            SetButtonEnable();
        }

        private void simpleButtonEndPage_Click(object sender, EventArgs e)
        {
            //CurrentForm.zyEditorControl1.CurrentPage = CurrentForm.zyEditorControl1.Pages[CurrentForm.zyEditorControl1.Pages.Count - 1];
            CurrentForm.zyEditorControl1.LastPage();
            SetButtonEnable();
        }

        private void simpleButtonLastPage_Click(object sender, EventArgs e)
        {
            //if (CurrentForm.zyEditorControl1.CurrentPage.PageIndex > 1)
            //{
            //    CurrentForm.zyEditorControl1.CurrentPage = CurrentForm.zyEditorControl1.Pages[CurrentForm.zyEditorControl1.CurrentPage.PageIndex - 2];
            //}
            CurrentForm.zyEditorControl1.PrePage();
            SetButtonEnable();
        }

        private void simpleButtonNextPage_Click(object sender, EventArgs e)
        {
            //if (CurrentForm.zyEditorControl1.CurrentPage.PageIndex < CurrentForm.zyEditorControl1.Pages.Count)
            //{
            //    CurrentForm.zyEditorControl1.CurrentPage = CurrentForm.zyEditorControl1.Pages[CurrentForm.zyEditorControl1.CurrentPage.PageIndex];
            //}
            CurrentForm.zyEditorControl1.NextPage();
            SetButtonEnable();
        }

        private void simpleButtonPageSetting_Click(object sender, EventArgs e)
        {
            XPageSettings PageSettings = new XPageSettings();
            CurrentForm.zyEditorControl1.EMRDoc._PageSetting(ref PageSettings);
        }

        private void simpleButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkEditModify_CheckedChanged(object sender, EventArgs e)
        {
            ChangeModifyShow();
        }

        WaitDialogForm m_WaitDialog;
        private void ChangeModifyShow()
        {
            if (checkEditModify.Checked)//显示修改痕迹
            {
                m_WaitDialog = new WaitDialogForm("正在显示修改痕迹……", "请稍等。");
                CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Read;
                CurrentForm.zyEditorControl1.LoadXML(m_ActualEmrDoc);
                CurrentForm.zyEditorControl1.EMRDoc.Refresh();
                CurrentForm.zyEditorControl1.Refresh();
                HideWaitDialog();
            }
            else//不显示修改痕迹
            {
                m_WaitDialog = new WaitDialogForm("正在隐藏修改痕迹……", "请稍等。");
                CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Clear;
                CurrentForm.zyEditorControl1.LoadXML(m_ClearEmrDoc);
                CurrentForm.zyEditorControl1.EMRDoc.Refresh();
                CurrentForm.zyEditorControl1.Refresh();
                HideWaitDialog();
            }
        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }

        private void checkEditHeader_CheckedChanged(object sender, EventArgs e)
        {
            CurrentForm.zyEditorControl1.EMRDoc.PrintHeader = checkEditHeader.Checked;
        }

        private void checkEditFooter_CheckedChanged(object sender, EventArgs e)
        {
            CurrentForm.zyEditorControl1.EMRDoc.PrintFooter = checkEditFooter.Checked;
        }

        /// <summary>
        /// 套打，即不打印表格部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEditTaoDa_CheckedChanged(object sender, EventArgs e)
        {
            m_IsTaoDa = checkEditTaoDa.Checked;
            CheckHeaderAndFooter(!checkEditTaoDa.Checked);
        }

        /// <summary>
        /// 选择页眉和页脚
        /// </summary>
        /// <param name="check"></param>
        private void CheckHeaderAndFooter(bool check)
        {
            checkEditHeader.Checked = check;
            checkEditFooter.Checked = check;
        }

        /// <summary>
        /// 针对护理的表单设置页数
        /// </summary>
        private void SetPageIndexForNurseRecord()
        {
            string AutoAddPage = AppConfigReader.GetAppConfig("AutoAddPageForNurseRecord").Config;
            if (AutoAddPage == "1")
            {
                string pageIndex = m_PatUtil.GetNurseRecordPageIndex(m_Model.InstanceId.ToString(), m_Model.TempIdentity, m_Model.ModelCatalog, m_noofinpat).ToString();
                if (pageIndex != "-1")
                {
                    CurrentForm.zyEditorControl1.EMRDoc.PageIndexForNurse = pageIndex;
                    CurrentForm.zyEditorControl1.EMRDoc.Refresh();
                    CurrentForm.zyEditorControl1.Refresh();
                }
            }
        }

        private void InitComboBoxPage()
        {
            int pageCount = CurrentForm.zyEditorControl1.EMRDoc.Pages.Count;
            for (int i = 1; i <= pageCount; i++)
            {
                comboBoxPage.Items.Add(i);
            }
        }

        private void comboBoxPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPage.Text.Trim() != "")
            {
                int currentPageIndex = CurrentForm.zyEditorControl1.EMRDoc.PageIndex;
                int pageIndex = Convert.ToInt32(comboBoxPage.Text) - 1;
                if (pageIndex < currentPageIndex)
                {
                    int count = currentPageIndex - pageIndex;
                    for (int i = 0; i < count; i++)
                    {
                        CurrentForm.zyEditorControl1.PrePage();
                    }
                }
                else if (pageIndex > currentPageIndex)
                {
                    int count = pageIndex - currentPageIndex;
                    for (int i = 0; i < count; i++)
                    {
                        CurrentForm.zyEditorControl1.NextPage();
                    }
                }
                SetButtonEnable();
            }
        }
        /// <summary>
        /// 选中双面打印，控制选择的是上下还是左右
        /// add byy ywk2012年10月24日 10:27:49
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEditPrintDuplex_CheckedChanged(object sender, EventArgs e)
        {
            //if (checkEditPrintDuplex.Checked)
            //{
            //    rabLeftRight.Visible = true;
            //    rabUpDown.Visible = true;
            //}
            //else
            //{
            //    rabLeftRight.Visible = false;
            //    rabUpDown.Visible = false;
            //}

            if (checkEditPrintDuplex.Checked)
            {
                checkEditPrintAll.Checked = false;
                checkEditPrintPartial.Checked = true;
                spinEditBegin.Enabled = true;
                spinEditEnd.Enabled = true;
                checkEditPrintSelection.Checked = false;
                checkEditXuDa.Checked = false;
                checkEditPrintSelectArea.Checked = false;
                rabLeftRight.Visible = true;
                rabUpDown.Visible = true;
                JumpPrint(false);
                SelectAreaPrint(false);
            }
            else
            {
                rabLeftRight.Visible = false;
                rabUpDown.Visible = false;
            }
        }

        /// <summary>
        /// 导出PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog file = new SaveFileDialog();
                file.InitialDirectory = "D:\\";
                file.Filter = "Pdf Files(*.pdf)|*.pdf";
                file.FileName = m_Model.ModelName.Split(' ')[0];//Modify by wwj 2013-01-18 指定导出PDF的文件名
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    WaitDialogForm m_WaitDialog = new WaitDialogForm("正在导出PDF文件...", "请稍后");
                    List<Bitmap> images = CurrentForm.zyEditorControl1.EMRDoc.GeneratePrintImage();
                    AddWaterMark(images);//Add by wwj 2013-01-18 图片上增加水印
                    Document doc = new Document();
                    PdfWriter.GetInstance(doc, new FileStream(file.FileName, FileMode.OpenOrCreate));
                    doc.Open();
                    foreach (Bitmap img in images)
                    {
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance((System.Drawing.Image)img, BaseColor.WHITE);
                        image.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                        image.ScalePercent(70);
                        doc.Add(image);
                    }
                    doc.Close();
                    m_WaitDialog.Close();
                    m_WaitDialog.Dispose();
                    MessageBox.Show("导出成功！");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 图片文件添加水印 
        /// Add by wwj 2013-01-18
        /// </summary>
        /// <param name="bmpList"></param>
        private void AddWaterMark(List<Bitmap> bmpList)
        {
            try
            {
                WaterMarkConfig waterMarkConfig = new WaterMarkConfig();
                string waterMark = waterMarkConfig.Content;//水印内容
                if (!string.IsNullOrEmpty(waterMark))
                {
                    System.Drawing.Font font = new System.Drawing.Font(waterMarkConfig.FontFamily, waterMarkConfig.FontSize);//水印字体
                    bmpList.ForEach((bmp) =>
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            //第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
                            Color c = Color.FromArgb(waterMarkConfig.ColorAlpha, waterMarkConfig.ColorRed, waterMarkConfig.ColorGreen, waterMarkConfig.ColorBlue);
                            //更改坐标系统的原点
                            g.TranslateTransform(waterMarkConfig.TranslateX, waterMarkConfig.TranslateY);
                            //水印文字倾斜方向
                            Matrix transform = g.Transform;
                            transform.Shear(waterMarkConfig.ShearX, waterMarkConfig.ShearY);
                            g.Transform = transform;
                            //绘制水印文字
                            g.DrawString(waterMark, font, new SolidBrush(c), new PointF());
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// 水印配置
    /// Add by wwj 2013-01-18
    /// </summary>
    public class WaterMarkConfig
    {
        /// <summary>
        /// 字体类型
        /// </summary>
        public string FontFamily { get; set; }
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; }
        /// <summary>
        /// 显示的水印内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 文字起始位置-X轴
        /// </summary>
        public float TranslateX { get; set; }
        /// <summary>
        /// 文字起始位置-Y轴
        /// </summary>
        public float TranslateY { get; set; }
        /// <summary>
        /// 文字方向-X轴
        /// </summary>
        public float ShearX { get; set; }
        /// <summary>
        /// 文字方向-Y轴
        /// </summary>
        public float ShearY { get; set; }
        /// <summary>
        /// 透明度
        /// </summary>
        public int ColorAlpha { get; set; }
        public int ColorRed { get; set; }
        public int ColorGreen { get; set; }
        public int ColorBlue { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public WaterMarkConfig()
        {
            try
            {
                DataTable dt = DS_SqlHelper.ExecuteDataTable("select value from appcfg where configkey = 'EmrPDFWaterMark'", CommandType.Text);
                if (dt.Rows.Count == 0) return;
                string xml = dt.Rows[0][0].ToString();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                FontFamily = doc.GetElementsByTagName("font-family")[0].InnerText;
                FontSize = int.Parse(doc.GetElementsByTagName("font-size")[0].InnerText);
                Content = doc.GetElementsByTagName("content")[0].InnerText.Trim();
                TranslateX = float.Parse(doc.GetElementsByTagName("translate-x")[0].InnerText);
                TranslateY = float.Parse(doc.GetElementsByTagName("translate-y")[0].InnerText);
                ShearX = float.Parse(doc.GetElementsByTagName("shear-x")[0].InnerText);
                ShearY = float.Parse(doc.GetElementsByTagName("shear-y")[0].InnerText);
                ColorAlpha = int.Parse(doc.GetElementsByTagName("alpha")[0].InnerText);
                ColorRed = int.Parse(doc.GetElementsByTagName("red")[0].InnerText);
                ColorGreen = int.Parse(doc.GetElementsByTagName("green")[0].InnerText);
                ColorBlue = int.Parse(doc.GetElementsByTagName("blue")[0].InnerText);
            }
            catch (Exception ex)
            {
                throw new Exception("水印配置有误！  " + ex.Message);
            }
        }
    }
}