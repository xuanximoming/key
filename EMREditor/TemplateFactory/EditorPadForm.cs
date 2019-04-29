using DevExpress.XtraVerticalGrid;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Library.EmrEditor.Src.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZYTextDocumentLib;


namespace DrectSoft.Emr.TemplateFactory
{
    public partial class EditorPadForm : UserControl
    {


        public PropertyGridControl propertyGrid;

        private IEmrHost m_app;
        RecordDal m_RecordDal;
        private TPTextCell m_cell;


        public EditorPadForm(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_RecordDal = new RecordDal(app.SqlHelper);
            InitEditor();
            this.zyEditorControl1.MouseDown += new MouseEventHandler(CurrentEditorControl_MouseDown);
        }

        private void InitEditor()
        {

            this.zyEditorControl1.InitializeDocument();
            this.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Design;
            this.zyEditorControl1.MouseDragScroll = false;



            //委托按钮事件,在这里写具体事件名字
            //先清空

            //替换按钮事件
            DataTable replaceItemDataTable = m_RecordDal.GetReplaceItem(string.Empty);
            //诊断按钮事件
            DataTable diagItemDataTable = m_RecordDal.GetDiagButtonEvent();

            if (replaceItemDataTable.Rows.Count > 0)
            {
                ZYEditorControl.helpEventList.Clear();

                //增加可替换项按钮事件
                foreach (DataRow dr in replaceItemDataTable.Rows)
                {
                    string replaceItem = dr["dest_itemname"].ToString();
                    if (!ZYEditorControl.helpEventList.Contains(replaceItem))
                    {
                        ZYEditorControl.helpEventList.Add(replaceItem);
                    }
                }

                //增加诊断按钮事件
                foreach (DataRow dr in diagItemDataTable.Rows)
                {
                    string diagname = dr["diagname"].ToString();
                    if (!ZYEditorControl.helpEventList.Contains(diagname))
                    {
                        ZYEditorControl.helpEventList.Add(diagname);
                    }
                }

                //增加几个特殊节点
                //ZYEditorControl.helpEventList.Add("初步诊断");
                //ZYEditorControl.helpEventList.Add("补充诊断");
                //ZYEditorControl.helpEventList.Add("修正诊断");
                //ZYEditorControl.helpEventList.Add("中医诊断");
                //ZYEditorControl.helpEventList.Add("入院诊断");
                //ZYEditorControl.helpEventList.Add("鉴别诊断");
                //ZYEditorControl.helpEventList.Add("诊疗计划");
            }
            else
            {
                ZYEditorControl.helpEventList.Clear();
                ZYEditorControl.helpEventList.Add("初步诊断");
                ZYEditorControl.helpEventList.Add("补充诊断");
                ZYEditorControl.helpEventList.Add("修正诊断");
                ZYEditorControl.helpEventList.Add("中医诊断");
                ZYEditorControl.helpEventList.Add("入院诊断");
                ZYEditorControl.helpEventList.Add("鉴别诊断");
                ZYEditorControl.helpEventList.Add("诊疗计划");
                ZYEditorControl.helpEventList.Add("并发症");
                ZYEditorControl.helpEventList.Add("手术记录");
                ZYEditorControl.helpEventList.Add("辅助检查");
                ZYEditorControl.helpEventList.Add("病危通知");
                ZYEditorControl.helpEventList.Add("操作记录");
                ZYEditorControl.helpEventList.Add("诊断依据");
                ZYEditorControl.helpEventList.Add("出院小结");
                ZYEditorControl.helpEventList.Add("护理记录");
                ZYEditorControl.helpEventList.Add("主诉");
                ZYEditorControl.helpEventList.Add("既往史");
                ZYEditorControl.helpEventList.Add("现病史");
                ZYEditorControl.helpEventList.Add("手术外伤史");
                ZYEditorControl.helpEventList.Add("输血史");
                ZYEditorControl.helpEventList.Add("过敏史");
                ZYEditorControl.helpEventList.Add("个人史");
                ZYEditorControl.helpEventList.Add("月经史");
                ZYEditorControl.helpEventList.Add("家族史");
                ZYEditorControl.helpEventList.Add("体格检查");
                ZYEditorControl.helpEventList.Add("专科情况(体检)");
                ZYEditorControl.helpEventList.Add("体征");

            }

            foreach (Wordbook.WordbookInfo info in Wordbook.WordbookStaticHandle.WordbookList)
            {
                ZYEditorControl.WordBookList.Add(info.Name);
            }

            zyEditorControl1.SetTitle(m_app.CurrentHospitalInfo.Name);
            //设置患者ID号，用于阻止不同患者之前的数据粘贴
            //zyEditorControl1.EMRDoc.Info.PatientID = "abc";

            //委托外部按钮事件
            zyEditorControl1.OnHelpButtonClick += new ZYEditorControl.HelpButtonClick(pnlText_OnHelpButtonClick);
            //选择改变事件
            zyEditorControl1.EMRDoc.OnSelectionChanged += new EventHandler(EMRDoc_OnSelectionChanged);

            //委托从外部获得xml文档
            //zyEditorControl1.OnGetOuterData += new ZYEditorControl.GetOuterData(pnlText_OnGetOuterData);
            zyEditorControl1.OnGetOuterDataBinary += new ZYEditorControl.GetOuterDataBinary(pnlText_OnGetOuterDataBinary);


            //鼠标单击事件
            zyEditorControl1.MouseClick += new MouseEventHandler(pnlText_MouseClick);

            //鼠标双击事件
            zyEditorControl1.MouseDoubleClick += new MouseEventHandler(pnlText_MouseDoubleClick);

            //zyEditorControl1.ContentChanged += new EventHandler(pnlText_ContentChanged);

        }


        void CurrentEditorControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    ZYTextElement element = this.zyEditorControl1.GetElementByPos(e.X, e.Y);
                    if (!(element is ZYTextImage))
                    {
                        popupMenuRightMouse.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        void EMRDoc_OnSelectionChanged(object sender, EventArgs e)
        {
            //设置选择属性，选中设置
            ArrayList eal = zyEditorControl1.EMRDoc.Content.GetSelectElements();
            ArrayList pal = new ArrayList();

            ////如果没有选中字符，则设置段属性
            if (eal.Count == 0)
            {
                foreach (object o in this.zyEditorControl1.EMRDoc.Content.GetSelectParagraph())
                {
                    if (o is ZYTextParagraph)
                        pal.Add(new PropertyParagraph(o));
                }
            }
            else
            {
                foreach (object o in eal)
                {
                    if (o is ZYTextChar)
                        pal.Add(new PropertyChar(o));
                    if (o is ZYTextImage)
                        pal.Add(new PropertyImage(o));

                }

            }
            if (propertyGrid != null)
                propertyGrid.SelectedObjects = pal.ToArray();


            ////设置工具条按钮
            ////判断当前所选择内容的格式，并相应设置字体，字号，粗体、斜体、下划线、 上下标，段落的对齐方式 工具条按钮的样式
            ////如果当前有选择元素，获得当前选 中元素的格式
            Dictionary<string, Object> dict = new Dictionary<string, object>();

            string[] format = { ZYTextConst.c_FontBold, ZYTextConst.c_FontItalic, ZYTextConst.c_FontUnderLine, ZYTextConst.c_Sup, ZYTextConst.c_Sub };
            string[] format2 = { ZYTextConst.c_FontName, ZYTextConst.c_FontSize };
            ArrayList elements = this.zyEditorControl1.EMRDoc.Content.GetSelectElements();
            if (elements.Count == 0)
            {
                //如果当前无选中元素，获得插入符前一个字符的格式
                elements.Add(this.zyEditorControl1.EMRDoc.Content.PreElement);

            }
            foreach (ZYTextElement ele in elements)
            {
                if (ele == null) return;

                foreach (string attr in format)
                {
                    if (!dict.ContainsKey(attr))
                        dict.Add(attr, ele.Attributes[attr] != null ? ele.Attributes[attr].Value : null);
                    if (dict[attr] != null && ele.Attributes[attr] != null && (bool)dict[attr] != (bool)ele.Attributes[attr].Value)
                    {
                        dict[attr] = null;
                    }
                }

                foreach (string attr in format2)
                {

                    if (!dict.ContainsKey(attr))
                        dict.Add(attr, ele.Attributes[attr] != null ? ele.Attributes[attr].Value : null);
                    if (dict[attr] != null && ele.Attributes[attr] != null && dict[attr].ToString() != (string)ele.Attributes[attr].Value.ToString())
                    {
                        dict[attr] = null;
                    }
                }

            }
            //todo 

            //mainFrm.粗体toolStripButton5.Checked = GetBoolFromDict(dict, ZYTextConst.c_FontBold);
            //mainFrm.斜体toolStripButton7.Checked = GetBoolFromDict(dict, ZYTextConst.c_FontItalic);
            //mainFrm.下划线toolStripButton6.Checked = GetBoolFromDict(dict, ZYTextConst.c_FontUnderLine);
            //mainFrm.上标toolStripButton13.Checked = GetBoolFromDict(dict, ZYTextConst.c_Sup);
            //mainFrm.下标toolStripButton15.Checked = GetBoolFromDict(dict, ZYTextConst.c_Sub);
            //if (GetFontSizeNameFromDict(dict, ZYTextConst.c_FontSize).Length > 0)
            //{
            //    mainFrm.toolStripComboBoxSize.SelectedItem = GetFontSizeNameFromDict(dict, ZYTextConst.c_FontSize);
            //    mainFrm.toolStripComboBoxSize.Text = GetFontSizeNameFromDict(dict, ZYTextConst.c_FontSize);
            //}
            //else
            //{
            //    mainFrm.toolStripComboBoxSize.Text = "";
            //}

            //if (GetTextFontNameFromDict(dict, ZYTextConst.c_FontName).Length > 0)
            //{
            //    mainFrm.toolStripComboBoxFont.SelectedItem = GetTextFontNameFromDict(dict, ZYTextConst.c_FontName);
            //    mainFrm.toolStripComboBoxFont.Text = GetTextFontNameFromDict(dict, ZYTextConst.c_FontName);
            //}
            //else
            //{
            //    mainFrm.toolStripComboBoxFont.Text = "";
            //}

            ////段落对齐方式
            //elements = this.pnlText.EMRDoc.Content.GetSelectParagraph();

            //int intAlign = -1;
            //foreach (ZYTextParagraph myP in elements)
            //{
            //    if (intAlign == -1)
            //        intAlign = (int)myP.Align;

            //    if (intAlign != (int)myP.Align)
            //    {
            //        intAlign = -1;
            //        break;
            //    }
            //}

            //ResetParagraphButton();
            //switch (intAlign)
            //{
            //    case 0:
            //        mainFrm.左对齐toolStripButton10.Checked = true;
            //        break;
            //    case 1:
            //        mainFrm.右对齐toolStripButton12.Checked = true;
            //        break;
            //    case 2:
            //        mainFrm.居中对齐toolStripButton11.Checked = true;
            //        break;
            //    case 3:

            //        break;
            //    case 4:
            //        mainFrm.两端对齐toolStripButton.Checked = true;
            //        break;
            //    case 5:

            //        break;

            //}

        }

        void pnlText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            if (this.zyEditorControl1.ActiveEditArea != null)
            {
                Point p = this.zyEditorControl1.ClientPointToView(e.Location);
                if (this.zyEditorControl1.ActiveEditArea.TopElement.RealTop < p.Y && p.Y < this.zyEditorControl1.ActiveEditArea.End)
                {
                    ZYTextElement ele = this.zyEditorControl1.GetElementByPos(e.Location.X, e.Location.Y);
                    if (ele.Parent is ZYText)
                    {
                        if ((ele.Parent as ZYText).Name == "记录日期" || (ele.Parent as ZYText).Name == "标题")
                        {
                            MessageBox.Show("弹出编辑时间与标题的窗口");
                        }
                    }
                }
            }

        }

        byte[] pnlText_OnGetOuterDataBinary(ElementType type, string name)
        {
            try
            {
                DataTable table = m_app.SqlHelper.ExecuteDataTable("select item_doc_new ITEM_DOC from emrtemplet_item  WHERE MR_NAME='" + name + "'");
                //找不到子模板不能这样转换的add by ywk 2013年4月3日16:16:40 
                if (table.Rows.Count < 1)
                {
                    //object obj = "子模板不存在";
                    //return (byte[])obj;
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("此子模板不存在！");
                }

                return System.Text.Encoding.Default.GetBytes(RecordDal.UnzipEmrXml(table.Rows[0]["ITEM_DOC"].ToString()));

            }
            catch (Exception)
            {

                throw;
            }


        }



        //void pnlText_ContentChanged(object sender, EventArgs e)
        //{
        //if (!this.splitContainer1.Panel1Collapsed)
        //{
        //    this.zyEditorControl1.EMRDoc.GetBrief(this.treeView1);
        //}
        //throw new NotImplementedException();
        //}

        //XmlDocument pnlText_OnGetOuterData(ElementType type, string name)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load("c:\\" + name + ".xml");
        //    return doc;
        //}


        /// <summary>
        /// 在这里根据事件名字具体处理过程
        /// </summary>
        /// <param name="name"></param>
        string pnlText_OnHelpButtonClick(string name)
        {
            //MessageBox.Show("编辑器控件中按钮事件 " + name);

            //string[] text = new string[] { "aaa", "bbb", "ccc" };
            this.zyEditorControl1.InsertResultPar("4.当主诊断时只有一条时，无编号；如果有多条时，有编号。编号格式1.2.3.分行排列，左端对齐");
            //this.zyEditorControl1.InsertResultPar(text);

            //this.zyEditorControl1.InsertTextElementAfterButton(text);

            //this.zyEditorControl1.InsertStringAfterButton("\nhello world");

            return "编辑器控件中按钮事件 " + name;
        }

        public event EventHandler<EventOjbArgs> PropertyObjChanged;

        private void OnPropetyChanged(EventOjbArgs args)
        {
            if (PropertyObjChanged != null)
            {


                PropertyObjChanged(this, args);
            }

        }

        //之所以在这里委托了一些鼠标事件，而不写在editorcontrol ，是因为要设置属性窗口，而editorcontrol 、porpertyFrm 它们互不知道对方。只有在更高的层次才行
        void pnlText_MouseClick(object sender, MouseEventArgs e)
        {
            //设置可选元素属性，点击设置属性
            ZYTextElement element = zyEditorControl1.GetElementByPos(e.X, e.Y);

            if (element == null)
            {
                return;
            }

            object ele = ZYEditorControl.CreatePropertyObj(element);

            if (ele != null)
            {
                //propertyGrid.SelectedObject = ele;
                OnPropetyChanged(new EventOjbArgs(ele));
            }
        }

        private void btn_CellBorderWidth_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TPTextCell cell = this.zyEditorControl1.EMRDoc.GetCurrentCell();
                if (cell != null)
                {
                    if (m_cell == null) m_cell = cell;
                    TableCellSetting cellSetting = new TableCellSetting(cell, m_cell);
                    cellSetting.StartPosition = FormStartPosition.CenterScreen;
                    DialogResult DialogResult = cellSetting.ShowDialog();
                    m_cell = cellSetting.p_cell;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
    }

    public class EventOjbArgs : EventArgs
    {
        public object PropertyObj { get; set; }

        public EventOjbArgs(object obj)
        {
            PropertyObj = obj;
        }
    }
}
