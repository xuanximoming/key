using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using DevExpress.XtraEditors;
using DrectSoft.Emr.Util;
using System.Drawing.Printing;
using System.Xml;
using DevExpress.Utils;
using DrectSoft.Core;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class PrintForm : DevExpress.XtraEditors.XtraForm
    {
        PatRecUtil m_PatUtil;
        EmrModel m_Model;
        int m_DefaultPageIndex;
        XmlDocument m_ActualEmrDoc = new XmlDocument();//包含修改痕迹的病历
        XmlDocument m_ClearEmrDoc = new XmlDocument();//不含修改痕迹的病历【简洁病历】
        bool m_IsTaoDa = false;//是否是套打
        string m_noofinpat = string.Empty;

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
            for(int i = count - 1; i >= 0; i--)
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
            #endregion

            #region End Print
            if (m_IsTaoDa)
            {
                CurrentForm.zyEditorControl1.LoadXML(docClone);
            }
            #endregion
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
            CurrentForm.zyEditorControl1.EMRDoc._PageSetting();
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
    }
}