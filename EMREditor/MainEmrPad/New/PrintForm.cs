using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Print;
using DrectSoft.Service;
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

namespace DrectSoft.Core.MainEmrPad.New
{
    public partial class PrintForm : DevBaseForm
    {
        Inpatient m_CurrentInpatient;
        Inpatient m_CurrentOutInpatient;
        IEmrHost m_App;
        EmrModel m_Model;
        XmlDocument m_ActualEmrDoc = new XmlDocument();//包含修改痕迹的病历
        XmlDocument m_ClearEmrDoc = new XmlDocument();//不含修改痕迹的病历【简洁病历】
        bool m_IsTaoDa = false;//是否是套打
        bool m_IsHideTable = false;//是否是隐藏表格

        PrintDocument m_PrintDocument;//add by ywk 2012年10月24日 10:30:07 

        /// <summary>
        /// 当前界面选中的编辑器
        /// </summary>
        EditorForm m_CurrentEditorForm;

        /// <summary>
        /// 对应转科记录表中的ID，供病程记录使用
        /// </summary>
        public string m_DeptChangeID { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="patUtil"></param>
        /// <param name="model"></param>
        /// <param name="defaultPageIndex"></param>
        /// <param name="inpatient"></param>
        /// <param name="app"></param>
        public PrintForm(EmrModel model, Inpatient inpatient, IEmrHost app, string deptChangeID)
        {
            try
            {
                InitializeComponent();
                navBarGroupLeft.NavigationPaneVisible = false;
                this.StartPosition = FormStartPosition.CenterScreen;
                m_Model = model.Clone();
                m_PrintDocument = new PrintDocument();
                m_CurrentInpatient = inpatient;
                m_App = app;
                m_DeptChangeID = deptChangeID;
                if (m_Model.ModelCatalog == "AC")
                {
                    this.Text = "打印预览--" + "病程记录";
                }
                else
                {
                    this.Text = "打印预览--" + m_Model.ModelName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// .ctor out
        /// </summary>
        /// <param name="patUtil"></param>
        /// <param name="model"></param>
        /// <param name="defaultPageIndex"></param>
        /// <param name="outpatient"></param>
        /// <param name="app"></param>
        public PrintForm(EmrModel model, Inpatient outpatient, IEmrHost app, string deptChangeID)
        {
            try
            {
                InitializeComponent();
                navBarGroupLeft.NavigationPaneVisible = false;
                this.StartPosition = FormStartPosition.CenterScreen;
                m_Model = model.Clone();
                m_PrintDocument = new PrintDocument();
                m_CurrentOutInpatient = outpatient;
                m_App = app;
                m_DeptChangeID = deptChangeID;
                if (m_Model.ModelCatalog == "AC")
                {
                    this.Text = "打印预览--" + "病程记录";
                }
                else
                {
                    this.Text = "打印预览--" + m_Model.ModelName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 指定打印起始页和终止页的初始值和数值范围
        /// </summary>
        private void InitSpinEdit()
        {
            try
            {
                spinEditBegin.Properties.MaxValue = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Pages.Count;
                spinEditEnd.Properties.MaxValue = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Pages.Count;
                spinEditBegin.EditValue = 1;
                spinEditEnd.EditValue = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Pages.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AddEditorForm()
        {
            try
            {
                m_CurrentEditorForm = new EditorForm(m_CurrentInpatient, m_App);
                m_CurrentEditorForm.DeptChangeID = m_DeptChangeID;
                m_CurrentEditorForm.Dock = DockStyle.Fill;
                panelControlPrintMain.Controls.Add(m_CurrentEditorForm);
                if (null != m_Model && null != m_Model.ModelContent && !string.IsNullOrEmpty(m_Model.ModelContent.InnerXml.Trim()))
                {
                    m_CurrentEditorForm.LoadDocument(m_Model.ModelContent.OuterXml);
                }
                if (m_Model.IsReadConfigPageSize)//是否需要读取配置中的页面设置
                {
                    m_CurrentEditorForm.SetDefaultPageSetting();
                    m_CurrentEditorForm.EditorRefresh();
                }
                m_CurrentEditorForm.SetBackColor(Color.White);
                m_CurrentEditorForm.SetCurrentElement(m_Model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitEmrDoc()
        {
            try
            {
                if (null == m_Model || null == m_Model.ModelContent || string.IsNullOrEmpty(m_Model.ModelContent.InnerXml.Trim()))
                {
                    return;
                }

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
                RemoveNoPrintFlag(m_ClearEmrDoc);//Add By wwj 2013-08-05
                RemoveNoPrintspan(m_ClearEmrDoc);
                RemoveNoPrintspan(m_ActualEmrDoc);
                m_Model.ModelContent = m_ClearEmrDoc;//默认不显示修改痕迹
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 移除不打印的Button元素
        /// </summary>
        private void RemoveNoPrintButton(XmlDocument doc)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 移除不打印的标签元素
        /// </summary>
        private void RemoveNoPrintspan(XmlDocument doc)
        {
            try
            {
                XmlNodeList nodelist = doc.GetElementsByTagName("roelement");
                List<XmlNode> list = new List<XmlNode>();
                foreach (XmlNode node in nodelist)
                {
                    XmlAttribute attrType = node.Attributes["type"];
                    XmlAttribute attrPrint = node.Attributes["print"];
                    if (attrType != null && attrPrint != null)
                    {
                        if (attrType.Value.ToLower() == "fixedtext" && attrPrint.Value.ToLower() == "false")
                        {
                            list.Add(node);
                        }
                    }
                }
                int count = list.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    XmlNode node = list[i];
                    node.ParentNode.RemoveChild(node);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 移除打印时不需要占用空间的ZYFLAG Add By wwj 2013-08-05
        /// </summary>
        /// <param name="doc"></param>
        private void RemoveNoPrintFlag(XmlDocument doc)
        {
            try
            {
                XmlNodeList nodelist = doc.GetElementsByTagName("flag");
                List<XmlNode> list = new List<XmlNode>();
                foreach (XmlNode node in nodelist)
                {
                    XmlAttribute attrType = node.Attributes["type"];
                    XmlAttribute attrPrint = node.Attributes["isholdspacewhenprint"];
                    if (attrType.Value.ToLower() == "flag" && attrPrint.Value.ToLower() == "false")//ZYFlag不占用打印时的空间
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 设置编辑器的编辑模式
        /// </summary>
        private void SetEmrDocumentModel()
        {
            try
            {
                if (m_CurrentEditorForm.CurrentEditorControl != null)
                {
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Clear;//默认不显示修改痕迹
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Info.ShowParagraphFlag = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PrintForm_Load(object sender, EventArgs e)
        {
            try
            {
                GetNeedCheckItemCount();
                InitDailyEmrCheckedListBox();
                InitEmrDoc();
                AddEditorForm();
                SetEmrDocumentModel();
                InitSpinEdit();
                InitPrinterList();
                InitComboBoxPage();
                checkEditPrintAll.Checked = true;
                this.WindowState = FormWindowState.Maximized;
                m_PrintDocument.PrintPage += new PrintPageEventHandler(m_PrintDocument_PrintPage);//直接进打印方法 注册事件 
                this.m_CurrentEditorForm.CurrentEditorControl.CurrentPageChanged += new EventHandler(CurrentEditorControl_CurrentPageChanged);
                SetButtonEnable();
                spinEditBegin.ContextMenuStrip = new ContextMenuStrip();
                spinEditEnd.ContextMenuStrip = new ContextMenuStrip();
                spinEditPrintCount.ContextMenuStrip = new ContextMenuStrip();
                spinEditPageStartIndex.ContextMenuStrip = new ContextMenuStrip();
                SetPageIndexForNurseRecord();
                HideWaitDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 针对护理的表单设置页数
        /// </summary>
        private void SetPageIndexForNurseRecord()
        {
            try
            {
                string AutoAddPage = AppConfigReader.GetAppConfig("AutoAddPageForNurseRecord").Config;
                if (AutoAddPage == "1")
                {
                    string pageIndex = GetNurseRecordPageIndex(m_Model.InstanceId.ToString(), m_Model.TempIdentity, m_Model.ModelCatalog, m_CurrentInpatient.NoOfFirstPage.ToString()).ToString();
                    if (pageIndex != "-1")
                    {
                        this.m_CurrentEditorForm.CurrentEditorControl.EMRDoc.PageIndexForNurse = pageIndex;
                        this.m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Refresh();
                        this.m_CurrentEditorForm.CurrentEditorControl.Refresh();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal int GetNurseRecordPageIndex(string recordDetailID, string templateID, string sortID, string noofinpat)
        {
            //'AI'护理记录   'AJ'护理文档   'AK'手术护理记录
            string getSameSortIdEmr = @" select id from recorddetail 
                                          where sortid = '{0}' and valid = 1 and templateid = '{1}' and noofinpat = '{2}'
                                                and sortid in ('AI','AJ','AK') 
                                       order by createtime ";
            DataTable dt = DS_SqlHelper.ExecuteDataTable(string.Format(getSameSortIdEmr, sortID, templateID, noofinpat));
            int pageIndex = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["id"].ToString() == recordDetailID)
                {
                    pageIndex = i + 1;
                    break;
                }
            }
            return pageIndex;
        }

        void CurrentEditorControl_CurrentPageChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_CurrentEditorForm != null && m_CurrentEditorForm.CurrentEditorControl != null && m_CurrentEditorForm.CurrentEditorControl.PageCount >= 1)
                {
                    comboBoxPage.SelectedIndexChanged -= new EventHandler(comboBoxPage_SelectedIndexChanged);
                    comboBoxPage.Text = Convert.ToString(m_CurrentEditorForm.CurrentEditorControl.PageIndex + spinEditPageStartIndex.Value);
                    SetButtonEnable();
                    comboBoxPage.SelectedIndexChanged += new EventHandler(comboBoxPage_SelectedIndexChanged);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 初始化病程复选框列表
        /// </summary>
        private void InitDailyEmrCheckedListBox()
        {
            try
            {
                navBarControlLeft.Visible = false;
                splitterControl1.Visible = false;
                labelControlPageStartIndex.Visible = false;
                spinEditPageStartIndex.Visible = false;

                if (m_Model.ModelCatalog == "AC")
                {
                    navBarControlLeft.Visible = true;
                    splitterControl1.Visible = true;
                    labelControlPageStartIndex.Visible = true;
                    spinEditPageStartIndex.Visible = true;
                    navBarGroupLeft.Caption = "科室病程节点";
                    btnRefresh.Text = "显示勾中的病程记录";
                    btnRefresh.ToolTip = "在右侧的病程预览区中连续显示上方勾中的所有病程记录";
                    SetCheckBoxList(m_CurrentInpatient.NoOfFirstPage.ToString(), m_DeptChangeID, "AC");
                }
                else if (m_Model.ModelCatalog == "AI")
                {
                    string sql = "select qc_code from emrtemplet  where templet_id in (select templateid from recorddetail where id=" + m_Model.InstanceId + ")";
                    DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                    if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["qc_code"].ToString().Trim() == "F01")
                    {
                        navBarControlLeft.Visible = true;
                        splitterControl1.Visible = true;
                        labelControlPageStartIndex.Visible = true;
                        spinEditPageStartIndex.Visible = true;
                        navBarGroupLeft.Caption = "护理记录二页节点";
                        btnRefresh.Text = "显示勾中的护理记录";
                        btnRefresh.ToolTip = "在右侧的护理记录区中连续显示上方勾中的所有护理记录";
                        SetCheckBoxList(m_CurrentInpatient.NoOfFirstPage.ToString(), m_DeptChangeID, "AI");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 初始化打印机列表
        /// </summary>
        private void InitPrinterList()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region CheckEdit
        private void checkEditPrintAll_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void checkEditPrintPartial_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void checkEditPrintSelection_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditPrintSelection.Checked)
                {
                    checkEditHeader.Checked = false;
                    checkEditFooter.Checked = false;
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void checkEditXuDa_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void checkEditPrintCurrentPage_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void checkEditPrintSelectArea_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private void simpleButtonPrint_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(m_CurrentEditorForm.CurrentEditorControl.ToXMLString());
                XmlDocument docClone = (XmlDocument)doc.Clone();

                #region Before Print
                if (checkEditPrintSelection.Checked && (m_IsTaoDa || m_IsHideTable))//add by Ukey 2016-09-01 选择打印选中区域并选择套打或者隐藏表格
                {
                    //add by Ukey 2016-09-01 处理隐藏表格或者套打时，不能打印选中元素的问题。
                    int SelectStart = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Content.SelectStart;
                    int SelectLength = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Content.SelectLength;
                    XmlNodeList nodeList = doc.GetElementsByTagName("table");
                    foreach (XmlNode node in nodeList)
                    {
                        //add by Ukey 2016-09-01 跳过已经隐藏的表格内容
                        if (node.Attributes["hidden-all-border"] != null && node.Attributes["hidden-all-border"].Value != "1")
                        {
                            node.Attributes["hidden-all-border"].Value = "1";
                        }
                    }
                    nodeList = doc.GetElementsByTagName("table-cell");
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.Attributes["border-width-top"] != null && node.Attributes["border-width-top"].Value != "0")
                        {
                            node.Attributes["border-width-top"].Value = "0";
                        }
                        if (node.Attributes["border-width-right"] != null && node.Attributes["border-width-right"].Value != "0")
                        {
                            node.Attributes["border-width-right"].Value = "0";
                        }
                        if (node.Attributes["border-width-bottom"] != null && node.Attributes["border-width-bottom"].Value != "0")
                        {
                            node.Attributes["border-width-bottom"].Value = "0";
                        }
                        if (node.Attributes["border-width-left"] != null && node.Attributes["border-width-left"].Value != "0")
                        {
                            node.Attributes["border-width-left"].Value = "0";
                        }
                    }
                    m_CurrentEditorForm.CurrentEditorControl.LoadXML(doc);
                    //add by Ukey 2016-09-01 处理隐藏表格或者套打时，不能打印选中元素的问题。
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Content.SelectStart = SelectStart;//打印开始点
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Content.SelectLength = SelectLength;//打印长度
                }
                else if (m_IsHideTable || m_IsTaoDa)//add by Ukey 2016-09-01 选中隐藏表格或者套打选项
                {
                    XmlNodeList nodeList = doc.GetElementsByTagName("table");
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.Attributes["hidden-all-border"] != null && node.Attributes["hidden-all-border"].Value != "1")
                        {
                            node.Attributes["hidden-all-border"].Value = "1";
                        }
                    }
                    nodeList = doc.GetElementsByTagName("table-cell");
                    foreach (XmlNode node in nodeList)
                    {
                        if (node.Attributes["border-width-top"] != null && node.Attributes["border-width-top"].Value != "0")
                        {
                            node.Attributes["border-width-top"].Value = "0";
                        }
                        if (node.Attributes["border-width-right"] != null && node.Attributes["border-width-right"].Value != "0")
                        {
                            node.Attributes["border-width-right"].Value = "0";
                        }
                        if (node.Attributes["border-width-bottom"] != null && node.Attributes["border-width-bottom"].Value != "0")
                        {
                            node.Attributes["border-width-bottom"].Value = "0";
                        }
                        if (node.Attributes["border-width-left"] != null && node.Attributes["border-width-left"].Value != "0")
                        {
                            node.Attributes["border-width-left"].Value = "0";
                        }
                    }
                    m_CurrentEditorForm.CurrentEditorControl.LoadXML(doc);
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
                        MyMessageBox.Show("请先设定打印页范围", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
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

                    //pageSetupDialog.Document.DefaultPageSettings.PaperSize = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.PageSettings.PaperSize;

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
                    //        m_CurrentEditorForm.CurrentEditorControl.EMRDoc._PrintPageFromTo( lookUpEditPrinter.EditValue.ToString(),
                    //            Convert.ToInt32(spinEditBegin.EditValue), Convert.ToInt32(spinEditEnd.EditValue), true, new_doc.PrinterSettings.Duplex);
                    //        m_CurrentEditorForm.CurrentEditorControl.Refresh();

                    //        ISDOUBLE = true;
                    //    }
                    #endregion
                    //PageSetupDialog pageSetupDialog = new PageSetupDialog();
                    //pageSetupDialog.Document = m_PrintDocument;
                    //if (pageSetupDialog.Document.DefaultPageSettings.PrinterSettings.CanDuplex)
                    //{
                    //    List<Bitmap> images = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.GeneratePrintImage();

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
                            m_CurrentEditorForm.CurrentEditorControl.EMRDoc._PrintAll(lookUpEditPrinter.EditValue.ToString());
                        }
                        else if (checkEditPrintCurrentPage.Checked)//打印当前页
                        {
                            m_CurrentEditorForm.CurrentEditorControl.EMRDoc._PrintPageFromTo(lookUpEditPrinter.EditValue.ToString(),
                                m_CurrentEditorForm.CurrentEditorControl.CurrentPage.PageIndex,
                                m_CurrentEditorForm.CurrentEditorControl.CurrentPage.PageIndex);
                        }
                        else if (checkEditPrintPartial.Checked)//指定页数打印
                        {
                            //if (Convert.ToInt32(spinEditBegin.EditValue) > Convert.ToInt32(spinEditEnd.EditValue))
                            //{
                            //    int temp = Convert.ToInt32(spinEditBegin.EditValue);
                            //    spinEditBegin.EditValue = spinEditEnd.EditValue;
                            //    spinEditEnd.EditValue = temp;
                            //}
                            if (Convert.ToInt32(spinEditBegin.EditValue) > Convert.ToInt32(spinEditEnd.EditValue))
                            {
                                MessageBox.Show("指定页数的起始页不能大于终止页！");
                                spinEditBegin.Focus();
                                return;
                            }
                            else if (Convert.ToInt32(spinEditBegin.EditValue) > m_CurrentEditorForm.CurrentEditorControl.PageCount + Convert.ToInt32(spinEditPageStartIndex.Value - 1))
                            {
                                MessageBox.Show("指定页数的起始页不能大于病历的总页数！");
                                spinEditBegin.Focus();
                                return;
                            }
                            else if (Convert.ToInt32(spinEditEnd.EditValue) > m_CurrentEditorForm.CurrentEditorControl.PageCount + Convert.ToInt32(spinEditPageStartIndex.Value - 1))
                            {
                                MessageBox.Show("指定页数的终止页不能大于病历的总页数！");
                                spinEditEnd.Focus();
                                return;
                            }
                            else if (Convert.ToInt32(spinEditBegin.EditValue) < Convert.ToInt32(spinEditPageStartIndex.Value))
                            {
                                MessageBox.Show("指定页数的起始页不能小于病历的起始页！");
                                spinEditBegin.Focus();
                                return;
                            }
                            else if (Convert.ToInt32(spinEditEnd.EditValue) < Convert.ToInt32(spinEditPageStartIndex.Value))
                            {
                                MessageBox.Show("指定页数的终止页不能小于病历的起始页！");
                                spinEditEnd.Focus();
                                return;
                            }

                            int beginPageNum = Convert.ToInt32(spinEditBegin.EditValue) - Convert.ToInt32(spinEditPageStartIndex.Value) + 1;
                            int endPageNum = Convert.ToInt32(spinEditEnd.EditValue) - Convert.ToInt32(spinEditPageStartIndex.Value) + 1;

                            m_CurrentEditorForm.CurrentEditorControl.EMRDoc._PrintPageFromTo(lookUpEditPrinter.EditValue.ToString(),
                                //Convert.ToInt32(spinEditBegin.EditValue), Convert.ToInt32(spinEditEnd.EditValue));
                                beginPageNum, endPageNum);
                        }
                        else if (checkEditPrintSelection.Checked)//选中元素打印
                        {
                            if (m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Content.SelectLength == 0)
                            {
                                MyMessageBox.Show("没有选中任何内容", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                                break;
                            }
                            m_CurrentEditorForm.CurrentEditorControl.EMRDoc._PrintSelection(lookUpEditPrinter.EditValue.ToString());
                            m_CurrentEditorForm.CurrentEditorControl.Refresh();
                        }
                        else if (checkEditXuDa.Checked)//续打
                        {
                            m_CurrentEditorForm.CurrentEditorControl.EMRDoc._PrintJump(lookUpEditPrinter.EditValue.ToString());
                        }
                        else if (checkEditPrintSelectArea.Checked)//打印选中区域
                        {
                            m_CurrentEditorForm.CurrentEditorControl.EMRDoc._PrintSelectArea(lookUpEditPrinter.EditValue.ToString());
                        }
                    }
                }
                #endregion

                #endregion

                #region End Print
                if (m_IsTaoDa || m_IsHideTable)
                {
                    m_CurrentEditorForm.CurrentEditorControl.LoadXML(docClone);
                }

                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        //private int ADDIndex = 1; //索引值
        ///// <summary>
        ///// 为双面打印写的打印方法 
        ///// add by ywk  2013年1月24日11:20:13  
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void m_PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        //{
        //    try
        //    {
        //        List<Metafile> images = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.GeneratePrintImage2();
        //        int StartprintPage = Convert.ToInt32(spinEditBegin.EditValue); //开始打印的页数
        //        int EndprintPage = Convert.ToInt32(spinEditEnd.EditValue); //结束打印的页数
        //        e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
        //        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        //        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        for (int k = 0; k < Convert.ToInt32(spinEditPrintCount.EditValue); k++)
        //        {
        //            if (ADDIndex > EndprintPage)
        //            {
        //                ADDIndex = 1;
        //                e.HasMorePages = false;
        //            }
        //            else if (ADDIndex >= 1 && ADDIndex < EndprintPage)
        //            {
        //                e.Graphics.DrawImage(images[StartprintPage + ADDIndex - 2], 
        //                    new RectangleF(0, 0, images[StartprintPage + ADDIndex - 2].Width, images[StartprintPage + ADDIndex - 2].Height));
        //                ADDIndex++;
        //                e.HasMorePages = true;
        //            }
        //            else if (ADDIndex == EndprintPage)//索引循环到了等于终止的页数,直接打印最后一页
        //            {
        //                e.Graphics.DrawImage(images[EndprintPage - 1], new RectangleF(0, 0, images[EndprintPage - 1].Width, images[EndprintPage - 1].Height));
        //                ADDIndex++;
        //                e.HasMorePages = false;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
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
                List<Metafile> images = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.GeneratePrintImage2();
                int StartprintPage = Convert.ToInt32(spinEditBegin.EditValue); //开始打印的页数
                int EndprintPage = Convert.ToInt32(spinEditEnd.EditValue); //结束打印的页数
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                for (int k = 0; k < Convert.ToInt32(spinEditPrintCount.EditValue); k++)
                {
                    if (ADDIndex > EndprintPage)
                    {
                        ADDIndex = 1;
                        e.HasMorePages = false;
                    }
                    else if (ADDIndex >= 1 && StartprintPage + ADDIndex - 1 < EndprintPage)
                    {
                        e.Graphics.DrawImage(images[StartprintPage + ADDIndex - 2],
                            new RectangleF(0, 0, images[StartprintPage + ADDIndex - 2].Width, images[StartprintPage + ADDIndex - 2].Height));
                        ADDIndex++;
                        e.HasMorePages = true;
                    }
                    else if (StartprintPage + ADDIndex - 1 == EndprintPage)//索引循环到了等于终止的页数,直接打印最后一页
                    {
                        e.Graphics.DrawImage(images[EndprintPage - 1], new RectangleF(0, 0, images[EndprintPage - 1].Width, images[EndprintPage - 1].Height));
                        ADDIndex = 1;
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
            try
            {
                if (m_CurrentEditorForm == null) return;
                if (isJumpPrint != m_CurrentEditorForm.CurrentEditorControl.EMRDoc.EnableJumpPrint)
                {
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.EnableJumpPrint = isJumpPrint;
                    m_CurrentEditorForm.CurrentEditorControl.Refresh();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SelectAreaPrint(bool isSelectAreaPrint)
        {
            try
            {
                if (m_CurrentEditorForm == null) return;
                if (isSelectAreaPrint != m_CurrentEditorForm.CurrentEditorControl.EMRDoc.EnableSelectAreaPrint)
                {
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.EnableSelectAreaPrint = isSelectAreaPrint;
                    m_CurrentEditorForm.CurrentEditorControl.Refresh();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void spinEditBegin_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(spinEditBegin.EditValue) <= 0)
                {
                    spinEditBegin.EditValue = 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void spinEditEnd_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(spinEditBegin.EditValue) > m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Pages.Count)
                {
                    spinEditBegin.EditValue = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Pages.Count;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetButtonEnable()
        {
            try
            {
                if (m_CurrentEditorForm.CurrentEditorControl.Pages.Count == 1)
                {
                    simpleButtonFirstPage.Enabled = false;
                    simpleButtonLastPage.Enabled = false;
                    simpleButtonNextPage.Enabled = false;
                    simpleButtonEndPage.Enabled = false;
                }
                //最后一页
                else if (m_CurrentEditorForm.CurrentEditorControl.CurrentPage.PageIndex == m_CurrentEditorForm.CurrentEditorControl.Pages.Count)
                {
                    simpleButtonFirstPage.Enabled = true;
                    simpleButtonLastPage.Enabled = true;
                    simpleButtonNextPage.Enabled = false;
                    simpleButtonEndPage.Enabled = false;
                }
                else if (m_CurrentEditorForm.CurrentEditorControl.CurrentPage.PageIndex == 1)
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
                textEditCurrentPage.Text = "第 " + (m_CurrentEditorForm.CurrentEditorControl.CurrentPage.PageIndex + spinEditPageStartIndex.Value - 1) + " 页/共 "
                    + (m_CurrentEditorForm.CurrentEditorControl.Pages.Count + spinEditPageStartIndex.Value - 1) + " 页";
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void simpleButtonFirstPage_Click(object sender, EventArgs e)
        {
            try
            {
                m_CurrentEditorForm.CurrentEditorControl.FirstPage();
                SetButtonEnable();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void simpleButtonEndPage_Click(object sender, EventArgs e)
        {
            try
            {
                m_CurrentEditorForm.CurrentEditorControl.LastPage();
                SetButtonEnable();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void simpleButtonLastPage_Click(object sender, EventArgs e)
        {
            try
            {
                m_CurrentEditorForm.CurrentEditorControl.PrePage();
                SetButtonEnable();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void simpleButtonNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                m_CurrentEditorForm.CurrentEditorControl.NextPage();
                SetButtonEnable();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void simpleButtonPageSetting_Click(object sender, EventArgs e)
        {
            try
            {
                XPageSettings PageSettings = new XPageSettings();
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc._PageSetting(ref PageSettings);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void simpleButtonClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void checkEditModify_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeModifyShow();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void ChangeModifyShow()
        {
            try
            {
                if (checkEditModify.Checked)//显示修改痕迹
                {
                    m_WaitDialog = new WaitDialogForm("正在显示修改痕迹……", "请稍等。");
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Read;
                    m_CurrentEditorForm.CurrentEditorControl.LoadXML(m_ActualEmrDoc);
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Refresh();
                    m_CurrentEditorForm.CurrentEditorControl.Refresh();
                    HideWaitDialog();
                }
                else//不显示修改痕迹
                {
                    m_WaitDialog = new WaitDialogForm("正在隐藏修改痕迹……", "请稍等。");
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Clear;
                    m_CurrentEditorForm.CurrentEditorControl.LoadXML(m_ClearEmrDoc);
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Refresh();
                    m_CurrentEditorForm.CurrentEditorControl.Refresh();
                    HideWaitDialog();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        #region 等待框
        WaitDialogForm m_WaitDialog;

        public void SetWaitDialogCaption(string caption)
        {
            try
            {
                if (m_WaitDialog != null)
                {
                    if (!m_WaitDialog.Visible)
                    {
                        m_WaitDialog.Visible = true;
                    }
                    m_WaitDialog.Caption = caption;
                }
                else
                {
                    m_WaitDialog = new WaitDialogForm(caption, "提示");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        #endregion

        private void checkEditHeader_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.PrintHeader = checkEditHeader.Checked;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        private void checkEditFooter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.PrintFooter = checkEditFooter.Checked;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 套打，即不打印表格部分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEditTaoDa_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                checkEdit_hidetable.Checked = false;
                m_IsTaoDa = checkEditTaoDa.Checked;
                CheckHeaderAndFooter(!checkEditTaoDa.Checked);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 选择页眉和页脚
        /// </summary>
        /// <param name="check"></param>
        private void CheckHeaderAndFooter(bool check)
        {
            try
            {
                checkEditHeader.Checked = check;
                checkEditFooter.Checked = check;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitComboBoxPage()
        {
            try
            {
                int pageCount = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Pages.Count;
                comboBoxPage.Items.Clear();
                for (int i = 1; i <= pageCount; i++)
                {
                    comboBoxPage.Items.Add(i);
                }
                comboBoxPage.Text = (this.m_CurrentEditorForm.CurrentEditorControl.PageIndex + spinEditPageStartIndex.Value).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetComboBoxPage()
        {
            try
            {
                this.comboBoxPage.SelectedIndexChanged -= new System.EventHandler(this.comboBoxPage_SelectedIndexChanged);
                InitComboBoxPage();
                for (int i = 0; i < comboBoxPage.Items.Count; i++)
                {
                    comboBoxPage.Items[i] = Convert.ToInt32(comboBoxPage.Items[i]) + spinEditPageStartIndex.Value - 1;
                }
                comboBoxPage.Text = (this.m_CurrentEditorForm.CurrentEditorControl.PageIndex + spinEditPageStartIndex.Value).ToString();
                this.comboBoxPage.SelectedIndexChanged += new System.EventHandler(this.comboBoxPage_SelectedIndexChanged);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void comboBoxPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxPage.Text.Trim() != "")
                {
                    int currentPageIndex = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.PageIndex;
                    int pageIndex = Convert.ToInt32(comboBoxPage.Text) - 1 - (Convert.ToInt32(spinEditPageStartIndex.Value) - 1);
                    if (pageIndex < currentPageIndex)
                    {
                        int count = currentPageIndex - pageIndex;
                        for (int i = 0; i < count; i++)
                        {
                            m_CurrentEditorForm.CurrentEditorControl.PrePage();
                        }
                    }
                    else if (pageIndex > currentPageIndex)
                    {
                        int count = pageIndex - currentPageIndex;
                        for (int i = 0; i < count; i++)
                        {
                            m_CurrentEditorForm.CurrentEditorControl.NextPage();
                        }
                    }
                    SetButtonEnable();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
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
            try
            {
                if (checkEditPrintDuplex.Checked)
                {
                    checkEditPrintPartial.CheckedChanged -= new EventHandler(checkEditPrintPartial_CheckedChanged);
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
                    checkEditPrintPartial.CheckedChanged += new EventHandler(checkEditPrintPartial_CheckedChanged);
                }
                else
                {
                    rabLeftRight.Visible = false;
                    rabUpDown.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
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
                string[] fileNames = m_Model.ModelName.Split(' ');
                if (fileNames.Length > 1)
                {
                    if (m_Model.ModelCatalog == "AC")
                    {
                        file.FileName = m_CurrentInpatient.Name + " 的 " + "病程记录";//Add by wwj 2013-04-23
                    }
                    else
                    {
                        //file.FileName = fileNames[0] + " " + fileNames[fileNames.Length - 1];//Modify by wwj 2013-01-18 指定导出PDF的文件名
                        file.FileName = m_CurrentInpatient.Name + " 的 " + fileNames[0];
                    }
                }
                else if (fileNames.Length > 0)
                {
                    file.FileName = fileNames[0];
                }
                else
                {
                    file.FileName = "病历";
                }
                if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    WaitDialogForm m_WaitDialog = new WaitDialogForm("正在导出PDF文件...", "请稍后");
                    List<Bitmap> images = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.GeneratePrintImage();
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
                    MyMessageBox.Show("导出成功", "提示", MyMessageBoxButtons.Yes, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
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

        #region 针对左侧病程树 Add by wwj 2013-03-29

        /// <summary>
        /// 设置复选框列表
        /// </summary>
        public void SetCheckBoxList(string noofinpat, string changeID, string Mr_code)
        {
            try
            {
                if (Mr_code == "AC")
                {
                    SetWaitDialogCaption("正在捞取病程记录，请稍后...");
                    DataTable dt = GetDailyEmrRecords(noofinpat, changeID);
                    SetCheckBoxListSource(dt);
                    DefaultCheckItem();
                    XmlDocument doc = ConcatCheckedEmrContent();
                    m_Model.ModelContent = doc;
                    SetWaitDialogCaption("正在组装病程记录，请稍后...");
                }
                else if (Mr_code == "AI")
                {
                    SetWaitDialogCaption("正在捞取 护理记录二页，请稍后...");
                    DataTable dt = GeHLRecords(noofinpat, changeID);
                    SetCheckBoxListSource(dt);
                    DefaultCheckItem();
                    XmlDocument doc = ConcatCheckedEmrHLContent();
                    m_Model.ModelContent = doc;
                    SetWaitDialogCaption("正在组装护理记录二页，请稍后...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 默认勾中20份病程，意味着FirstCallRecordDetailID周围最近的20份病历在Load页面的时候会加载到预览区中显示
        /// </summary>
        int m_NeedCheckItemCount = 60;

        /// <summary>
        /// 获取指定病人、指定科室的所有病程记录
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="changeID"></param>
        /// <returns></returns>
        private DataTable GetDailyEmrRecords(string noofinpat, string changeID)
        {
            try
            {
                //获取指定病人所在病区的所有病历
                string sqlGetEmrRecord = string.Format(
                    @"select distinct r.id, r.name, r.captiondatetime,'' content, e.isfirstdaily isfirstdaily, e.new_page_flag newPageBegin, e.new_page_end newPageEnd, r.isconfigpagesize
                        from recorddetail r left outer join emrtemplet e on r.templateid = e.templet_id 
                        where r.noofinpat = '{0}' and r.changeid = '{1}' and r.sortid = 'AC' and r.valid = '1' and r.hassubmit<> 4600 
                        order by r.captiondatetime",
                    noofinpat, changeID);
                return DS_SqlHelper.ExecuteDataTable(sqlGetEmrRecord, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取指定病人、指定科室的所有 护理记录
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="changeID"></param>
        /// <returns></returns>
        private DataTable GeHLRecords(string noofinpat, string changeID)
        {
            try
            {
                //获取指定病人所在病区的所有护理记录
                string sqlGetEmrRecord = string.Format(
                    @"select distinct r.id, r.name, r.captiondatetime,'' content, e.isfirstdaily isfirstdaily, e.new_page_flag newPageBegin, e.new_page_end newPageEnd, r.isconfigpagesize
                        from recorddetail r left outer join emrtemplet e on r.templateid = e.templet_id 
                        where r.noofinpat = '{0}' and (r.changeid = '{1}' or '{1}' is null) and e.Qc_code='F01' and r.valid = '1' 
                        order by r.captiondatetime",
                    noofinpat, changeID);
                return DS_SqlHelper.ExecuteDataTable(sqlGetEmrRecord, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="dt"></param>
        private void SetCheckBoxListSource(DataTable dt)
        {
            try
            {
                if (checkedListBoxControlEmrNode.InvokeRequired)
                {
                    checkedListBoxControlEmrNode.Invoke(new Action<DataTable>(SetCheckBoxListSource), dt);
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        checkedListBoxControlEmrNode.Items.Add(new EmrNode(dr), false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 默认选中复选框列表中的指定项目
        /// </summary>
        private void DefaultCheckItem()
        {
            try
            {
                string firstCallRecordDetailID = m_Model.InstanceId.ToString();
                if (checkedListBoxControlEmrNode.InvokeRequired)
                {
                    checkedListBoxControlEmrNode.Invoke(new Action(DefaultCheckItem));
                }
                else
                {
                    //if (checkedListBoxControlEmrNode.Items.Count <= m_NeedCheckItemCount)
                    //{
                    //    checkedListBoxControlEmrNode.CheckAll();
                    //}
                    //else
                    {
                        int index = -1;//FirstCallRecordDetailID中在复选框列表中的索引
                        int needCheckCount = m_NeedCheckItemCount;//在复选框列表中需要选中的项目
                        foreach (CheckedListBoxItem item in checkedListBoxControlEmrNode.Items)
                        {
                            EmrNode emrNode = ((ListBoxItem)item).Value as EmrNode;
                            if (emrNode != null)
                            {
                                if (emrNode.ID == firstCallRecordDetailID.ToString())
                                {
                                    index = checkedListBoxControlEmrNode.Items.IndexOf(item);
                                }
                            }
                        }

                        if (index != -1)
                        {
                            #region 勾中指定病历周围m_NeedCheckItemCount份病历
                            checkedListBoxControlEmrNode.Items[index].CheckState = CheckState.Checked;
                            for (int i = 1; i <= m_NeedCheckItemCount; i++)
                            {
                                if (index + i < checkedListBoxControlEmrNode.Items.Count)
                                {
                                    checkedListBoxControlEmrNode.Items[index + i].CheckState = CheckState.Checked;
                                }

                                if (checkedListBoxControlEmrNode.CheckedItems.Count == m_NeedCheckItemCount) break;

                                if (index - i >= 0)
                                {
                                    checkedListBoxControlEmrNode.Items[index - i].CheckState = CheckState.Checked;
                                }

                                if (checkedListBoxControlEmrNode.CheckedItems.Count == m_NeedCheckItemCount) break;
                            }
                            #endregion

                            checkedListBoxControlEmrNode.SelectedIndex = index;
                            checkedListBoxControlEmrNode.MakeItemVisible(index);
                        }
                        else
                        {
                            int itemCount = checkedListBoxControlEmrNode.Items.Count;
                            int selectIndex = itemCount - m_NeedCheckItemCount;
                            if (selectIndex >= 0)
                            {
                                for (int i = 1; i <= m_NeedCheckItemCount; i++)
                                {
                                    checkedListBoxControlEmrNode.Items[itemCount - i].CheckState = CheckState.Checked;
                                }
                                checkedListBoxControlEmrNode.SelectedIndex = selectIndex;
                                checkedListBoxControlEmrNode.MakeItemVisible(selectIndex);
                            }
                        }

                        if (checkedListBoxControlEmrNode.CheckedItems.Count == checkedListBoxControlEmrNode.Items.Count)
                        {
                            checkEditAll.Checked = true;
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
        /// 拼接选中病历的内容
        /// </summary>
        private XmlDocument ConcatCheckedEmrContent()
        {
            try
            {
                XmlDocument mainDoc = new XmlDocument();
                mainDoc.PreserveWhitespace = true;
                XmlNode nodeBody = null;

                #region 获取需要拼接的病历
                List<EmrNode> emrList = new List<EmrNode>();
                bool isSetPageConfigSize = false;
                foreach (CheckedListBoxItem item in checkedListBoxControlEmrNode.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        EmrNode emrNode = ((ListBoxItem)item).Value as EmrNode;
                        if (emrNode.DataRowItem["content"].ToString() == "")
                        {
                            emrNode.DataRowItem["content"] = GetEmrContentByID(emrNode.ID);
                        }
                        string emrContent = emrNode.DataRowItem["content"].ToString();
                        if (!string.IsNullOrEmpty(emrContent))
                        {
                            emrList.Add(emrNode);
                        }
                    }
                }
                #endregion

                #region 根据第一份病程的设置，决定病历的isconfigpagesize属性值
                if (checkedListBoxControlEmrNode.Items.Count > 0)
                {
                    if (!isSetPageConfigSize)
                    {
                        EmrNode emrNode = ((ListBoxItem)checkedListBoxControlEmrNode.Items[0]).Value as EmrNode;
                        m_Model.IsReadConfigPageSize = emrNode.DataRowItem["isconfigpagesize"].ToString() == "1" ? true : false;
                        isSetPageConfigSize = true;
                    }
                }
                #endregion

                if (emrList.Count > 0)
                {
                    #region 拼接上面获取的病历内容，同时根据病历对应的模板加入分页符

                    bool isHasPageEndInPrevious = false;//在前一份病程中是否包含分页符

                    string emrPrintSetting = DS_SqlService.GetConfigValueByKey("EmrPrintSetting");//lineCountBetweenTwoDailyEmr两份病程间的行数
                    int lineCountBetweenTwoDailyEmr = 0;
                    if (emrPrintSetting != string.Empty)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(emrPrintSetting);
                        XmlNodeList nodeList = doc.SelectNodes("root/lineCountBetweenTwoDailyEmr");
                        if (nodeList.Count > 0)
                        {
                            int.TryParse(nodeList[0].InnerText, out lineCountBetweenTwoDailyEmr);
                        }
                    }

                    for (int i = 0; i < emrList.Count; i++)
                    {
                        try
                        {
                            string emrContent = emrList[i].DataRowItem["content"].ToString();//病历内容
                            bool isFirstDailyEmr = emrList[i].DataRowItem["isfirstdaily"].ToString() == "1" ? true : false;//是否是首次病程
                            bool isNewPageBegin = emrList[i].DataRowItem["newPageBegin"].ToString() == "1" ? true : false;//新页开始
                            bool isNewPageEnd = emrList[i].DataRowItem["newPageEnd"].ToString() == "1" ? true : false;//新页结束

                            if (mainDoc.InnerText == "")
                            {
                                mainDoc.LoadXml(emrContent);
                                nodeBody = mainDoc.SelectSingleNode("document/body");

                                if (isFirstDailyEmr && mainDoc.GetElementsByTagName("pageend").Count > 0)//如果是首程，且包含分页符，则不用插入分页符，这里属于特殊处理，防止在拆分的时候出现问题
                                {
                                    isHasPageEndInPrevious = false;
                                }
                                else if (isNewPageEnd)//如果该病程需要以新页结束，则末尾加上分页符
                                {
                                    isHasPageEndInPrevious = true;
                                    AddPageEnd(mainDoc);
                                }
                                else if (isFirstDailyEmr)//如果是首次病程，根据配置决定是否需要加入分页符
                                {
                                    string config = DS_SqlService.GetConfigValueByKey("EmrSetting");
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(config);
                                    string isPageEnd = doc.SelectSingleNode("Emr/FirstDailyEmr/EmrEndInsertPageEnd").InnerText;
                                    if (isPageEnd == "1")
                                    {
                                        isHasPageEndInPrevious = true;
                                        AddPageEnd(mainDoc);
                                    }
                                    else
                                    {
                                        isHasPageEndInPrevious = false;
                                    }
                                }
                                continue;
                            }
                            else
                            {
                                XmlDocument subDoc = new XmlDocument();
                                subDoc.PreserveWhitespace = true;
                                subDoc.LoadXml(emrContent);
                                AddSaveLogs(mainDoc, subDoc);//在拼接病历内容时，对savelog进行处理
                                if (isNewPageBegin && !isHasPageEndInPrevious)//上一份病程没有以新页结束，且当前病程需要以新页开始
                                {
                                    AddPageEnd(mainDoc);//加入分页符
                                }
                                else
                                {
                                    AddOneParagraph(mainDoc, lineCountBetweenTwoDailyEmr);//加入一行新段落
                                }
                                XmlNode bodyNode = subDoc.SelectSingleNode("document/body");
                                foreach (XmlNode node in bodyNode.ChildNodes)
                                {
                                    XmlNode deepCopyNode = mainDoc.ImportNode(node, true);
                                    nodeBody.AppendChild(deepCopyNode);
                                }
                                if (isNewPageEnd)//当前病程需要以新页结束
                                {
                                    isHasPageEndInPrevious = true;
                                    AddPageEnd(mainDoc);
                                }
                                else
                                {
                                    isHasPageEndInPrevious = false;
                                }
                                continue;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    #endregion


                }
                return mainDoc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 拼接选中病历的内容
        /// </summary>
        private XmlDocument ConcatCheckedEmrHLContent()
        {
            try
            {
                XmlDocument mainDoc = new XmlDocument();
                mainDoc.PreserveWhitespace = true;
                XmlNode nodeBody = null;

                #region 获取需要拼接的病历
                List<EmrNode> emrList = new List<EmrNode>();
                bool isSetPageConfigSize = false;
                foreach (CheckedListBoxItem item in checkedListBoxControlEmrNode.Items)
                {
                    if (item.CheckState == CheckState.Checked)
                    {
                        EmrNode emrNode = ((ListBoxItem)item).Value as EmrNode;
                        if (emrNode.DataRowItem["content"].ToString() == "")
                        {
                            emrNode.DataRowItem["content"] = GetEmrContentByID(emrNode.ID);
                        }
                        string emrContent = emrNode.DataRowItem["content"].ToString();
                        if (!string.IsNullOrEmpty(emrContent))
                        {
                            emrList.Add(emrNode);
                        }
                    }
                }
                #endregion

                #region 根据第一份病程的设置，决定病历的isconfigpagesize属性值
                if (checkedListBoxControlEmrNode.Items.Count > 0)
                {
                    if (!isSetPageConfigSize)
                    {
                        EmrNode emrNode = ((ListBoxItem)checkedListBoxControlEmrNode.Items[0]).Value as EmrNode;
                        m_Model.IsReadConfigPageSize = emrNode.DataRowItem["isconfigpagesize"].ToString() == "1" ? true : false;
                        isSetPageConfigSize = true;
                    }
                }
                #endregion

                if (emrList.Count > 0)
                {
                    #region 拼接上面获取的病历内容，同时根据病历对应的模板加入分页符

                    bool isHasPageEndInPrevious = false;//在前一份病程中是否包含分页符

                    string emrPrintSetting = DS_SqlService.GetConfigValueByKey("EmrPrintSetting");//lineCountBetweenTwoDailyEmr两份病程间的行数
                    int lineCountBetweenTwoDailyEmr = 0;
                    if (emrPrintSetting != string.Empty)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(emrPrintSetting);
                        XmlNodeList nodeList = doc.SelectNodes("root/lineCountBetweenTwoDailyEmr");
                        if (nodeList.Count > 0)
                        {
                            int.TryParse(nodeList[0].InnerText, out lineCountBetweenTwoDailyEmr);
                        }
                    }

                    for (int i = 0; i < emrList.Count; i++)
                    {
                        try
                        {
                            string emrContent = emrList[i].DataRowItem["content"].ToString();//病历内容
                            bool isFirstDailyEmr = emrList[i].DataRowItem["isfirstdaily"].ToString() == "1" ? true : false;//是否是首次病程
                            bool isNewPageBegin = emrList[i].DataRowItem["newPageBegin"].ToString() == "1" ? true : false;//新页开始
                            bool isNewPageEnd = emrList[i].DataRowItem["newPageEnd"].ToString() == "1" ? true : false;//新页结束

                            if (mainDoc.InnerText == "")
                            {
                                mainDoc.LoadXml(emrContent);
                                nodeBody = mainDoc.SelectSingleNode("document/body");

                                if (isFirstDailyEmr && mainDoc.GetElementsByTagName("pageend").Count > 0)//如果是首程，且包含分页符，则不用插入分页符，这里属于特殊处理，防止在拆分的时候出现问题
                                {
                                    isHasPageEndInPrevious = false;
                                }
                                else if (isNewPageEnd)//如果该病程需要以新页结束，则末尾加上分页符
                                {
                                    isHasPageEndInPrevious = true;
                                    AddPageEnd(mainDoc);
                                }
                                else if (isFirstDailyEmr)//如果是首次病程，根据配置决定是否需要加入分页符
                                {
                                    string config = DS_SqlService.GetConfigValueByKey("EmrSetting");
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(config);
                                    string isPageEnd = doc.SelectSingleNode("Emr/FirstDailyEmr/EmrEndInsertPageEnd").InnerText;
                                    if (isPageEnd == "1")
                                    {
                                        isHasPageEndInPrevious = true;
                                        AddPageEnd(mainDoc);
                                    }
                                    else
                                    {
                                        isHasPageEndInPrevious = false;
                                    }
                                }
                                continue;
                            }
                            else
                            {
                                XmlDocument subDoc = new XmlDocument();
                                subDoc.PreserveWhitespace = true;
                                subDoc.LoadXml(emrContent);
                                AddSaveLogs(mainDoc, subDoc);//在拼接病历内容时，对savelog进行处理
                                if (isNewPageBegin && !isHasPageEndInPrevious)//上一份病程没有以新页结束，且当前病程需要以新页开始
                                {
                                    AddPageEnd(mainDoc);//加入分页符
                                }
                                else
                                {
                                    AddOneParagraph(mainDoc, lineCountBetweenTwoDailyEmr);//加入一行新段落
                                }
                                XmlNode bodyNode = subDoc.SelectSingleNode("document/body");
                                foreach (XmlNode node in bodyNode.ChildNodes)
                                {
                                    XmlNode deepCopyNode = mainDoc.ImportNode(node, true);
                                    nodeBody.AppendChild(deepCopyNode);
                                }
                                if (isNewPageEnd)//当前病程需要以新页结束
                                {
                                    isHasPageEndInPrevious = true;
                                    AddPageEnd(mainDoc);
                                }
                                else
                                {
                                    isHasPageEndInPrevious = false;
                                }
                                continue;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    #endregion


                }
                return mainDoc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 在XmlDocument中加入pageend标示符
        /// </summary>
        /// <param name="doc"></param>
        private void AddPageEnd(XmlDocument doc)
        {
            try
            {
                // "<p align=\"0\"><pageend type=\"pageend\" name=\"\" /><eof /></p>"
                XmlNode nodeBody = doc.SelectSingleNode("document/body");

                XmlNode pNode = doc.CreateElement("p");
                XmlAttribute alignAttribute = doc.CreateAttribute("align");
                alignAttribute.Value = "0";
                pNode.Attributes.Append(alignAttribute);

                XmlNode pageendNode = doc.CreateElement("pageend");
                XmlAttribute pageendAttribute = doc.CreateAttribute("pageend");
                pageendNode.Attributes.Append(pageendAttribute);
                XmlNode eofNode = doc.CreateElement("eof");

                pNode.AppendChild(pageendNode);
                pNode.AppendChild(eofNode);

                nodeBody.AppendChild(pNode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 加入一行新段落
        /// </summary>
        /// <param name="?"></param>
        private void AddOneParagraph(XmlDocument doc, int lineCount)
        {
            for (int i = 0; i < lineCount; i++)
            {
                // "<p align=\"0\"><eof /></p>"
                XmlNode nodeBody = doc.SelectSingleNode("document/body");
                XmlNode pNode = doc.CreateElement("p");
                XmlAttribute alignAttribute = doc.CreateAttribute("align");
                alignAttribute.Value = "0";
                pNode.Attributes.Append(alignAttribute);
                XmlNode eofNode = doc.CreateElement("eof");
                pNode.AppendChild(eofNode);
                nodeBody.AppendChild(pNode);
            }
        }

        /// <summary>
        /// 将subDoc中所有的Savelog加到maindoc中Savelogs节点下，作为其子节点
        /// </summary>
        /// <param name="mainDoc"></param>
        /// <param name="subDoc"></param>
        private void AddSaveLogs(XmlDocument mainDoc, XmlDocument subDoc)
        {
            try
            {
                XmlNode mainSavelog = mainDoc.SelectSingleNode("document/savelogs");
                XmlNodeList subSaveLogs = subDoc.SelectNodes("document/savelogs/savelog");
                int mainSaveLogsCount = mainSavelog.ChildNodes.Count;
                foreach (XmlNode node in subSaveLogs)
                {
                    mainSavelog.AppendChild(mainDoc.ImportNode(node, true));
                }
                processElementSaveLog(mainSaveLogsCount, subDoc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将doc下所有的节点中包含属性creator、deleter对应的数值+cnt
        /// </summary>
        /// <param name="cnt"></param>
        /// <param name="doc"></param>
        private void processElementSaveLog(int cnt, XmlNode doc)
        {
            try
            {
                foreach (XmlNode subDoc in doc.ChildNodes)
                {
                    if (subDoc.NodeType == XmlNodeType.Element)
                    {
                        XmlElement subEle = subDoc as XmlElement;
                        if (subEle.HasAttribute("creator"))
                        {
                            subEle.Attributes["creator"].Value = (Convert.ToInt32(subEle.Attributes["creator"].Value) + cnt).ToString();
                        }
                        if (subEle.HasAttribute("deleter"))
                        {
                            subEle.Attributes["deleter"].Value = (Convert.ToInt32(subEle.Attributes["deleter"].Value) + cnt).ToString();
                        }
                        if (subDoc.ChildNodes.Count > 0)
                        {
                            processElementSaveLog(cnt, subDoc);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 通过ID获取病历内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetEmrContentByID(string id)
        {
            try
            {
                string sqlGetEmrContent = string.Format("select content from recorddetail where id = '{0}'", id);
                return DS_SqlHelper.ExecuteScalar(sqlGetEmrContent, CommandType.Text).ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEditAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditAll.Checked)
                {
                    checkedListBoxControlEmrNode.CheckAll();
                }
                else
                {
                    checkedListBoxControlEmrNode.UnCheckAll();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 显示勾中的病程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBoxControlEmrNode.CheckedItems.Count > 0)
                {
                    SetWaitDialogCaption("正在捞取病程记录，请稍后...");
                    XmlDocument doc = ConcatCheckedEmrContent();
                    SetWaitDialogCaption("正在组装病程记录，请稍后...");
                    m_Model.ModelContent = doc;
                    m_CurrentEditorForm.LoadDocument(m_Model.ModelContent.OuterXml);
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Read;
                    if (m_Model.IsReadConfigPageSize)//是否需要读取配置中的页面设置
                    {
                        m_CurrentEditorForm.SetDefaultPageSetting();
                        m_CurrentEditorForm.EditorRefresh();
                    }
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Content.SelectStart = 0;
                    m_CurrentEditorForm.CurrentEditorControl.ScrollViewtopToCurrentElement();
                    HideWaitDialog();
                }
                else
                {


                    MyMessageBox.Show("请勾选需要显示的病程记录", "提示", MyMessageBoxButtons.Yes, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 获取需要选中的病程数目
        /// </summary>
        private void GetNeedCheckItemCount()
        {
            try
            {
                string cnt = BasicSettings.GetStringConfig("PreViewEmrCount").Trim();
                if (!string.IsNullOrEmpty(cnt) && cnt.Split('|').Length == 2)
                {
                    m_NeedCheckItemCount = Convert.ToInt32(cnt.Split('|')[1]);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 设置打印起始页 Add by wwj 2013-04-07
        private void spinEditPageStartIndex_Leave(object sender, EventArgs e)
        {

        }

        private void spinEditPageStartIndex_Validated(object sender, EventArgs e)
        {
            ChangeStartPageIndex();
        }

        private void ChangeStartPageIndex()
        {
            try
            {
                int result;
                if (int.TryParse(spinEditPageStartIndex.Value.ToString(), out result))
                {
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.PageIndexFix = result;
                    m_CurrentEditorForm.CurrentEditorControl.Refresh();
                }
                else
                {
                    MyMessageBox.Show("请输入数字", "提示", MyMessageBoxButtons.Yes, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        private void checkedListBoxControlEmrNode_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    int itemIndex = checkedListBoxControlEmrNode.IndexFromPoint(checkedListBoxControlEmrNode.PointToClient(Cursor.Position));
                    if (itemIndex >= 0)
                    {
                        ListBoxItem item = checkedListBoxControlEmrNode.Items[itemIndex];
                        if (item != null)
                        {
                            EmrNode emrNode = item.Value as EmrNode;
                            if (emrNode != null)
                            {
                                string captionDateTime = emrNode.DataRowItem["captiondatetime"].ToString();
                                this.m_CurrentEditorForm.SetCurrentElement(captionDateTime);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void spinEditPageStartIndex_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeStartPageIndex();
                SetComboBoxPage();
                SetButtonEnable();

                int pageStartNum = Convert.ToInt32(spinEditPageStartIndex.Value);
                spinEditBegin.Properties.MaxValue = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Pages.Count + pageStartNum - 1;
                spinEditEnd.Properties.MaxValue = m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Pages.Count + pageStartNum - 1;
                spinEditBegin.Properties.MinValue = pageStartNum;
                spinEditEnd.Properties.MinValue = pageStartNum;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void spinEditBegin_Validated(object sender, EventArgs e)
        {
            try
            {
                int beginValue = Convert.ToInt32(spinEditBegin.Value);
                int endValue = Convert.ToInt32(spinEditEnd.Value);
                if (beginValue > endValue)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("起始页数不能大于终止页数", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    spinEditBegin.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void spinEditEnd_Validated(object sender, EventArgs e)
        {
            try
            {
                int beginValue = Convert.ToInt32(spinEditBegin.Value);
                int endValue = Convert.ToInt32(spinEditEnd.Value);
                if (endValue < beginValue)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("终止页数不能小于起始页数", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    spinEditEnd.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void checkedListBoxControlEmrNode_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                checkEditAll.CheckedChanged -= new EventHandler(checkEditAll_CheckedChanged);
                if (checkedListBoxControlEmrNode.ItemCount != checkedListBoxControlEmrNode.CheckedItemsCount)
                {
                    checkEditAll.Checked = false;
                }
                else
                {
                    checkEditAll.Checked = true;
                }
                checkEditAll.CheckedChanged += new EventHandler(checkEditAll_CheckedChanged);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.m_CurrentEditorForm.CurrentEditorControl.EMRDoc.SetSelectionColor(Color.White);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.m_CurrentEditorForm.CurrentEditorControl.EMRDoc.SetSelectionColor(Color.Black);
        }

        private void checkEdit_hidetable_CheckedChanged(object sender, EventArgs e)
        {
            checkEditTaoDa.Checked = false;
            m_IsHideTable = checkEdit_hidetable.Checked;
        }
    }

    #region 水印配置类

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

    #endregion
}
