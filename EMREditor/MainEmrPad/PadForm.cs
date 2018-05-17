using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Library.EmrEditor.Src.Gui;
using System.Data.OracleClient;
using System.Xml;
using DrectSoft.Emr.Util;
using DrectSoft.Core.MainEmrPad.HelpForm;
using DrectSoft.Common.Eop;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTab;
using DrectSoft.Core.MainEmrPad.DailyForm;
using DrectSoft.Library.EmrEditor.Src.Print;
using DevExpress.XtraEditors;
using System.Drawing.Printing;
using DrectSoft.Core;
using DrectSoft.Service;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.MainEmrPad
{
    /// <summary>
    /// add try...catch by cyq 2013-03-08
    /// </summary>
    public partial class PadForm : UserControl
    {
        EmrModel m_CurrentModel;

        PatRecUtil m_Util;
        RecordDal m_RecordDal;
        public static XPageSettings m_DefaultPageSetting;
        public XPageSettings m_ActualPageSetting;
        public static float s_LineSpace = 1.5f;
        public static Color s_ElementColor = Color.FromArgb(0x99, 0xCC, 0xFF);
        public static string s_ElementStyle = "背景色";
        //public static int s_LineHeight = 60;
        List<string> m_ExtraEvents;

        /// <summary>
        /// 显示的名称
        /// </summary>
        public string ShowDocName
        {
            get
            {
                return m_ShowDocName;
            }
            set
            {
                m_ShowDocName = value;
            }
        }
        private string m_ShowDocName;

        /// <summary>
        /// 诊断逻辑
        /// </summary>
        DiagFormLogic m_DiagFormLogic;

        public PadForm(PatRecUtil util, RecordDal recordDal)
        {
            try
            {
                InitializeComponent();
                m_Util = util;
                m_RecordDal = recordDal;
                InitEditor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 设置背景色 
        /// add by ywk 2012年12月13日20:03:13
        /// </summary>
        public void SetBackColor()
        {
            try
            {
                this.zyEditorControl1.PageBackColor = Color.White;//打印还是白色
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitEditor()
        {
            try
            {
                //设置文书录入背景色 为三色值的配置add by ywk  2012年12月13日19:51:45 
                //R202  G225  B230
                string emrSettingBackColor = BasicSettings.GetStringConfig("SetPageColor");
                string[] colors = emrSettingBackColor.Split(',');
                if (colors.Length == 3)
                {
                    int IntR = Int32.Parse(colors[0].ToString());
                    int IntG = Int32.Parse(colors[1].ToString());
                    int IntB = Int32.Parse(colors[2].ToString());
                    this.zyEditorControl1.PageBackColor = Color.FromArgb(IntR, IntG, IntB);
                }


                m_DiagFormLogic = new DiagFormLogic(m_Util.m_app);
                this.zyEditorControl1.InitializeDocument();
               //  zyEditorControl1.EMRDoc.Info.PatientID = m_Util.m_app.CurrentPatientInfo.NoOfHisFirstPage;
                this.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                this.zyEditorControl1.MouseDragScroll = false;
                PatRecUtil.TempEditorControl = zyEditorControl1;
                zyEditorControl1.Dock = DockStyle.Fill;
                zyEditorControl1.VerticalScroll.Value = 0;
                zyEditorControl1.VerticalScroll.Value = 0;
                zyEditorControl1.HorizontalScroll.Value = 0;

                //行间距
                zyEditorControl1.SetLineSpace(s_LineSpace);

                ZYEditorControl.ElementBackColor = s_ElementColor;
                ZYEditorControl.ElementStyle = s_ElementStyle;

                //由于设置行高会影响图片，所以这里删掉
                //zyEditorControl1.EMRDoc.EnableFixedLineHeigh = true;
                //zyEditorControl1.EMRDoc.FixedLineHeigh = s_LineHeight;
                //zyEditorControl1.EMRDoc.ContentChanged();
                //zyEditorControl1.EMRDoc.EndUpdate();

                m_ExtraEvents = new List<string>();


                //委托按钮事件,在这里写具体事件名字
                //先清空
                //ZYEditorControl.helpEventList.Clear();
                //ZYEditorControl.helpEventList.Add("初步诊断");
                //ZYEditorControl.helpEventList.Add("补充诊断");
                //ZYEditorControl.helpEventList.Add("修正诊断");
                //ZYEditorControl.helpEventList.Add("中医诊断");
                //ZYEditorControl.helpEventList.Add("入院诊断");
                //ZYEditorControl.helpEventList.Add("鉴别诊断");
                //ZYEditorControl.helpEventList.Add("诊疗计划");
                //ZYEditorControl.helpEventList.Add("并发症");
                //ZYEditorControl.helpEventList.Add("手术记录");
                //ZYEditorControl.helpEventList.Add("辅助检查");
                //ZYEditorControl.helpEventList.Add("病危通知");
                //ZYEditorControl.helpEventList.Add("操作记录");
                //ZYEditorControl.helpEventList.Add("诊断依据");
                //ZYEditorControl.helpEventList.Add("出院小结");
                //ZYEditorControl.helpEventList.Add("护理记录");
                //ZYEditorControl.helpEventList.Add("主诉");
                //ZYEditorControl.helpEventList.Add("既往史");
                //ZYEditorControl.helpEventList.Add("体格检查");
                //ZYEditorControl.helpEventList.Add("专科情况");
                //ZYEditorControl.helpEventList.Add("既往史");
                //ZYEditorControl.helpEventList.Add("现病史");
                //ZYEditorControl.helpEventList.Add("手术外伤史");
                //ZYEditorControl.helpEventList.Add("输血史");
                //ZYEditorControl.helpEventList.Add("过敏史");
                //ZYEditorControl.helpEventList.Add("个人史");
                //ZYEditorControl.helpEventList.Add("月经史");
                //ZYEditorControl.helpEventList.Add("家族史");
                //ZYEditorControl.helpEventList.Add("体格检查");
                //ZYEditorControl.helpEventList.Add("专科情况(体检)");
                //ZYEditorControl.helpEventList.Add("体征");

                zyEditorControl1.SetTitle(m_Util.m_app.CurrentHospitalInfo.Name);
                //zyEditorControl1.SetSubTitle(m_Util.m_app.CurrentHospitalInfo.Subname);

                //设置患者ID号，用于阻止不同患者之前的数据粘贴
                zyEditorControl1.EMRDoc.Info.PatientID = m_Util.m_app.CurrentPatientInfo.NoOfHisFirstPage;
                zyEditorControl1.EMRDoc.setPatientid (m_Util.m_app.CurrentPatientInfo.NoOfHisFirstPage);
                //委托外部按钮事件
                zyEditorControl1.OnHelpButtonClick += new ZYEditorControl.HelpButtonClick(pnlText_OnHelpButtonClick);
                //选择改变事件
                //zyEditorControl1.EMRDoc.OnSelectionChanged += new EventHandler(EMRDoc_OnSelectionChanged);

                //委托从外部获得xml文档
                //zyEditorControl1.OnGetOuterData += new ZYEditorControl.GetOuterData(pnlText_OnGetOuterData);
                zyEditorControl1.OnGetOuterDataBinary += new ZYEditorControl.GetOuterDataBinary(pnlText_OnGetOuterDataBinary);


                //鼠标单击事件
                zyEditorControl1.MouseClick += new MouseEventHandler(pnlText_MouseClick);

                //鼠标双击事件
                zyEditorControl1.MouseDoubleClick += new MouseEventHandler(pnlText_MouseDoubleClick);

                //zyEditorControl1.ContentChanged += new EventHandler(pnlText_ContentChanged);

                zyEditorControl1.GotFocus += new EventHandler(zyEditorControl1_GotFocus);

                this.VisibleChanged += new EventHandler(PadForm_VisibleChanged);

                zyEditorControl1.EMRDoc.OnShowFindForm += new ShowFindFormHandler(EMRDoc_OnShowFindForm);
                zyEditorControl1.EMRDoc.OnSaveEMR += new SaveEMRHandler(EMRDoc_OnSaveEMR);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void SetReplaceEvent()
        {
            try
            {
                if (m_CurrentModel == null) return;
                if (m_ExtraEvents.Count > 0) return;
                if (m_RecordDal != null)
                {
                    //获得当前病历所有的替换项
                    DataTable replaceItemDataTable = m_RecordDal.GetReplaceItem(m_CurrentModel.ModelName);
                    m_ExtraEvents.Clear();
                    if (replaceItemDataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in replaceItemDataTable.Rows)
                        {
                            string replaceItem = dr["dest_itemname"].ToString();
                            if (!m_ExtraEvents.Contains(replaceItem))
                            {
                                m_ExtraEvents.Add(replaceItem);
                            }
                        }
                    }
                    else
                    {
                        m_ExtraEvents.Add("主诉");
                        m_ExtraEvents.Add("既往史");
                        m_ExtraEvents.Add("体格检查");
                        m_ExtraEvents.Add("专科情况");
                        m_ExtraEvents.Add("既往史");
                        m_ExtraEvents.Add("手术外伤史");
                        m_ExtraEvents.Add("输血史");
                        m_ExtraEvents.Add("过敏史");
                        m_ExtraEvents.Add("个人史");
                        m_ExtraEvents.Add("月经史");
                        m_ExtraEvents.Add("家族史");
                        m_ExtraEvents.Add("体格检查");
                        m_ExtraEvents.Add("专科情况(体检)");
                        m_ExtraEvents.Add("体征");
                        m_ExtraEvents.Add("现病史");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void EMRDoc_OnSaveEMR()
        {
            try
            {
                if (this.Parent is XtraTabPage && this.Parent.Parent is XtraTabControl && this.Parent.Parent.Parent is UCEmrInput)
                {
                    UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;
                    form.SaveDocumentPublic();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void EMRDoc_OnShowFindForm()
        {
            try
            {
                UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;
                if (form != null)
                {
                    form.find();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void PadForm_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Visible == true)
                {
                    GotFocusNotTriggerEvent();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        #region 权限相关
        /// <summary>
        /// 二次修改，实现个人修改痕迹功能
        /// edit by ywk 2012年12月24日13:36:373
        /// </summary>
        /// <param name="model"></param>
        private void ResetEditMode(EmrModel model)
        {
            try
            {
                bool isModified = zyEditorControl1.EMRDoc.Modified;
                if (model.CreatorXH == m_Util.m_app.User.DoctorId)
                {
                    isModified = true;
                }
                //zyEditorControl1.SetTitle("南京市中西医结合医院");
                zyEditorControl1.SetTitle(m_Util.m_app.CurrentHospitalInfo.Name);
                //zyEditorControl1.SetSubTitle(m_Util.m_app.CurrentHospitalInfo.Subname);
                if (m_ActualPageSetting != null)
                {
                    zyEditorControl1.EMRDoc.Pages.PageSettings = m_ActualPageSetting;
                }
                else
                {
                    zyEditorControl1.EMRDoc.Pages.PageSettings = m_DefaultPageSetting;
                }

                //Add By wwj 2012-02-16 设置页眉页脚高度
                zyEditorControl1.EMRDoc.DocumentHeaderHeight = m_DefaultPageSetting.HeaderHeight;
                zyEditorControl1.EMRDoc.DocumentFooterHeight = m_DefaultPageSetting.FooterHeight;

                zyEditorControl1.SetLineSpace(s_LineSpace);
                zyEditorControl1.EMRDoc.Modified = isModified;
                zyEditorControl1.EMRDoc.RefreshLine();
                zyEditorControl1.EMRDoc.RefreshPages();
                zyEditorControl1.EMRDoc.OwnerControl.UpdatePages();
                zyEditorControl1.EMRDoc.Refresh();


                zyEditorControl1.EMRDoc.Info.LogicDelete = true;
                zyEditorControl1.EMRDoc.Info.AutoLogicDelete = true;
                zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                IEmrModelPermision modelPermision;
                bool b;
                DrectSoft.Common.Eop.Employee empl = new DrectSoft.Common.Eop.Employee(m_Util.m_app.User.Id);
                empl.ReInitializeProperties();
                modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Audit, empl);
                b = modelPermision.CanDo(model);
                #region 原处理三级检诊痕迹的功能(ywk )
                //string isopenThreeLevelAudit = string.Empty;//是否开放了三级检诊功能

                //bool isopenThreeLevelAudit;
                //string isopenOwnerSign = string.Empty;//是否开放了个人书写留痕功能
                //isopenOwnerSign = YD_SqlService.GetConfigValueByKey("IsOpenOwnerSign");
                //UCEmrInput UC1 = new UCEmrInput();
                //isopenThreeLevelAudit = UC1.IsOpenThreeLevelAudit();
                //if (isopenThreeLevelAudit == true && isopenOwnerSign == "1")
                //{
                //    Common.Ctrs.DLG.MessageBox.Show("对不起，三级检诊功能和设置个人修改痕迹不可同时开放");
                //    return;
                //}
                //开放了三级检诊走下面的流程
                //if (b)
                //{
                    //若b为true，则代表文件已经提交，处于审核模式
                    if (b) // 文件已提交，处于审核模式
                    {
                        // 审核——已提交（未归档），符合级别要求
                        //记录修改痕迹
                        zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                        zyEditorControl1.EMRDoc.Info.LogicDelete = true;
                        zyEditorControl1.EMRDoc.Info.AutoLogicDelete = true;
                        zyEditorControl1.EMRDoc.Info.ShowMark = true;
                        zyEditorControl1.EMRDoc.Info.ShowAll = true;
                        zyEditorControl1.EMRDoc.Info.ShowMark = true;
                        //zyEditorControl1.InsertMode = false;//记录修改痕迹
                        zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.UserName = m_Util.m_app.User.Name; //这里传入用户名
                        //todo 针对性设置修改痕迹
                        if (empl.DoctorGradeNumber == 0)//住院医生
                        {

                            zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.Level = 0;         //这里传入用户级别
                            zyEditorControl1.EMRDoc.Content.UserLevel = 1;                     //这里还是同样的用户级别
                            //editorfrm.pnlText.Refresh();
                        }
                        else if (empl.DoctorGradeNumber == 1)//主治医生
                        {

                            zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.Level = 1;         //这里传入用户级别
                            zyEditorControl1.EMRDoc.Content.UserLevel = 2;                     //这里还是同样的用户级别

                        }
                        else if (empl.DoctorGradeNumber > 1)//主任医师
                        {
                            zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.Level = 2;         //这里传入用户级别
                            zyEditorControl1.EMRDoc.Content.UserLevel = 9;                     //这里还是同样的用户级别
                        }

                        //m_ActionAuditAll.Enabled = b;
                    }
                    else
                    { }

                //}
                #region  留痕暂不在此处实现 ywk
                ////开放了修改个人痕迹显示的功能,走下面流程
                //if (isopenOwnerSign == "1")
                //{
                //    zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Read;
                //    zyEditorControl1.EMRDoc.Info.LogicDelete = true;
                //    zyEditorControl1.EMRDoc.Info.AutoLogicDelete = true;
                //    zyEditorControl1.EMRDoc.Info.ShowMark = true;
                //    zyEditorControl1.EMRDoc.Info.ShowAll = true;
                //    //zyEditorControl1.EMRDoc.Info.ShowMark = true;
                //    //zyEditorControl1.InsertMode = false;//记录修改痕迹
                //    zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.UserName = m_Util.m_app.User.Name; //这里传入用户名
                //    //todo 针对性设置修改痕迹


                //    if (MessageBox.Show("您确认此时并开启留痕吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //    {
                //        zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                //        zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.Level = 1;         //这里传入用户级别
                //        zyEditorControl1.EMRDoc.Content.UserLevel = 1;                     //这里还是同样的用户级别
                //    }
                //    else
                //    {
                //        zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.Level = 0;         //这里传入用户级别
                //        zyEditorControl1.EMRDoc.Content.UserLevel = 0;                     //这里还是同样的用户级别
                //    }

                //}
                #endregion
                #endregion
            }
            catch (Exception)
            {
                throw new Exception("ResetEditMode 方法出错");
            }
        }

        #endregion

        private void InitEMRDocExtraInfo()
        {
            //zyEditorControl1.SetTitle("南京市中西医结合医院");
            ////zyEditorControl1.SetSubTitle("南京中医药大学附属医院");
            //zyEditorControl1.EMRDoc.Info.LogicDelete = true;
            //zyEditorControl1.EMRDoc.Info.AutoLogicDelete = true;
            //zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
            //todo 判断权限
            //zyEditorControl1.EMRDoc.Info.ShowMark = true;
            //zyEditorControl1.EMRDoc.Info.ShowAll = true;
            //zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.UserName = "name"; //这里传入用户名
            //zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.Level = 0;         //这里传入用户级别
            //zyEditorControl1.EMRDoc.Content.UserLevel = 0;
        }


        void pnlText_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.zyEditorControl1.ActiveEditArea != null)
                {
                    Point p = this.zyEditorControl1.ClientPointToView(e.Location);
                    if (this.zyEditorControl1.ActiveEditArea.TopElement.RealTop < p.Y && p.Y < this.zyEditorControl1.ActiveEditArea.End)
                    {
                        ZYTextElement ele = this.zyEditorControl1.GetElementByPos(e.Location.X, e.Location.Y);
                        if (ele.Parent is ZYText)
                        {
                            if ((ele.Parent as ZYText).Name == "记录日期")
                            {
                                UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;

                                TreeListNode node = form.GetFocusNode();
                                DailyFormDisplayDateTime formDateTime = new DailyFormDisplayDateTime((ele.Parent as ZYText).ToEMRString(), m_Util.m_app, node);
                                formDateTime.StartPosition = FormStartPosition.CenterParent;

                                if (formDateTime.ShowDialog() == DialogResult.Yes)
                                {
                                    //修改病程的記錄日期,加粗顯示
                                    ZYText date = new ZYText();
                                    date.Name = (ele.Parent as ZYText).Name;
                                    date.Attributes.SetValue(ZYTextConst.c_FontBold, true);
                                    date.Text = formDateTime.CommitDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                                    //todo 此处病历刷新有问题，待确认修改 周辉 2011-08-21
                                    //zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                                    zyEditorControl1.EMRDoc._InsertBlock(date);
                                    zyEditorControl1.DeleteElement(ele.Parent);
                                    //zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Read;
                                    zyEditorControl1.EMRDoc.Refresh();
                                    ResetDailyDateTime((ele.Parent as ZYText).text, null, date.text, null);
                                }


                            }
                            else if ((ele.Parent as ZYText).Name == "标题")
                            {
                                DailyFormTitle formTitle = new DailyFormTitle((ele.Parent as ZYText).ToEMRString(), m_Util.m_app);
                                formTitle.StartPosition = FormStartPosition.CenterParent;
                                if (formTitle.ShowDialog() == DialogResult.Yes)
                                {
                                    //修改病程的標題
                                    ZYText caption = new ZYText();
                                    caption.Name = (ele.Parent as ZYText).Name;
                                    caption.Text = formTitle.GetTitle();

                                    //todo 此处病历刷新有问题，待确认修改 周辉 2011-08-21
                                    //zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                                    zyEditorControl1.EMRDoc._InsertBlock(caption);
                                    zyEditorControl1.DeleteElement(ele.Parent);
                                    //zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Read;
                                    zyEditorControl1.EMRDoc.Refresh();
                                    //zyEditorControl1.Refresh();


                                    ResetDailyDateTime(null, (ele.Parent as ZYText).text, null, caption.text);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void ResetDailyDateTime(string oldDateTime, string oldTitle, string newDateTime, string newTitle)
        {
            try
            {
                UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;
                form.ReSetTreeListNodeAndTabPageName(oldDateTime, oldTitle, newDateTime, newTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ModifyTitle(ZYText element, string title)
        {
            try
            {
                ZYText caption = new ZYText();
                caption.Name = "标题";
                caption.Text = title;
                this.zyEditorControl1.EMRDoc._InsertBlock(caption);
                this.zyEditorControl1.DeleteElement(element);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        byte[] pnlText_OnGetOuterDataBinary(ElementType type, string name)
        {
            try
            {
                //throw new NotImplementedException();
                //XmlDocument doc = new XmlDocument();

                //改为从当前程序的根目录下获取

                return m_Util.GetTempleteItem(name);

            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                //弹出诊断窗口 name="初步诊断"、确诊诊断、补充诊断、修正诊断、中医诊断、
                //弹出临床诊断库 name='鉴别诊断'、诊断计划、
                //弹出其他相应窗口 name=''
                if (m_DiagFormLogic.ShowDialog(name))
                {
                    m_DiagFormLogic.GetDiag(this);
                    return name;
                }
                //特殊的病历信息抓取，比如首次病程中包括一部分入院记录中的数据
                if (m_ExtraEvents.Contains(name))
                {
                    zyEditorControl1.EMRDoc._InserString(zyEditorControl1.EMRDoc.GetReplaceText(name));

                }

                return "编辑器控件中按钮事件 " + name;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
                return "编辑器控件中按钮事件 " + name;
            }
        }

        //之所以在这里委托了一些鼠标事件，而不写在editorcontrol ，是因为要设置属性窗口，而editorcontrol 、porpertyFrm 它们互不知道对方。只有在更高的层次才行
        void pnlText_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                /*
                //设置可选元素属性，点击设置属性
                ZYTextElement element = zyEditorControl1.GetElementByPos(e.X, e.Y);

                if (element == null)
                {
                    return;
                }

                object ele = ZYEditorControl.CreatePropertyObj(element);

                //if (ele != null)
                //{
                //    porpertyFrm.propertyGrid1.SelectedObject = ele;
                //}
                 */

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (this.Parent is XtraTabPage && this.Parent.Parent is XtraTabControl && this.Parent.Parent.Parent is UCEmrInput)
                    {
                        UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;

                        if (!form.RightMouseButtonFlag) return;

                        barButtonItemSave.Enabled = form.GetSaveButtonEnable();
                        barButtonItemDelete.Enabled = form.GetDeleteButtonEnable();
                        barButtonItemSubmit.Enabled = form.GetSubmitButtonEnable();
                        barButtonItemAudit.Enabled = form.GetAuditButtonEnable();
                        barButtonItemCancelAudit.Enabled = form.GetCancelAuditButtonEnable();
                        barButtonItemLocate.Enabled = form.GetLocateButtonEnable();

                        popupMenuRightMouse.ShowPopup(this.PointToScreen(new Point(e.Location.X + 5, e.Location.Y + 5)));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据内容加载进界面
        /// add by ywk 2012年12月21日15:55:26 世界末日修改
        /// </summary>
        /// <param name="model"></param>
        public void LoadDocment(EmrModel model)
        {
            try
            {

                string pageconfig = BasicSettings.GetStringConfig("PageConfigIsAffectBC") == "" ? "0" : BasicSettings.GetStringConfig("PageConfigIsAffectBC");
                m_CurrentModel = model;
                SetReplaceEvent();

                if (model.ModelContent == null)
                {
                    //model.ModelContent = zyEditorControl1.BinaryToXml(m_Util.GetTemplateContent(model.TempIdentity));
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    doc.LoadXml(m_Util.GetTemplateContent(model.TempIdentity, model.PersonalTemplate));
                    model.ModelContent = doc;
                }
                if (pageconfig == "1")//病程记录也跟着变 add by ywk 2012年12月21日16:00:04世界末日
                {
                    if (model.ModelContent != null && !model.IsReadConfigPageSize)
                    {
                        SetPageSize(model.ModelContent);
                    }
                }
                if (pageconfig == "0")
                {
                    if (model.ModelContent != null && !model.IsReadConfigPageSize && model.ModelCatalog != ContainerCatalog.BingChengJiLu)
                    {
                        SetPageSize(model.ModelContent);
                    }
                }
                //if (model.ModelContent != null && !model.IsReadConfigPageSize && model.ModelCatalog != ContainerCatalog.BingChengJiLu)
                //if (model.ModelContent != null && !model.IsReadConfigPageSize)
                //{
                //    SetPageSize(model.ModelContent);
                //}

                zyEditorControl1.EMRDoc.ClearContent();
                zyEditorControl1.LoadXML(model.ModelContent);
                SetSubTitleFont();

                //设置当前病人
                this.zyEditorControl1.EMRDoc.Info.PatientID = m_Util.CurrentInpatient.NoOfFirstPage.ToString();

                //判断当前模型状态
                ResetEditMode(model);

                //  zyEditorControl1.EMRDoc.Modified = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetPageSize(XmlDocument doc)
        {
            try
            {
                XPageSettings ps = new XPageSettings();

                string pageKind = ((XmlElement)doc.GetElementsByTagName("page")[0]).GetAttribute("kind").ToString();
                string pageWidth = ((XmlElement)doc.GetElementsByTagName("page")[0]).GetAttribute("width").ToString();
                string pageHeight = ((XmlElement)doc.GetElementsByTagName("page")[0]).GetAttribute("height").ToString();
                string marginLeft = ((XmlElement)doc.GetElementsByTagName("margins")[0]).GetAttribute("left").ToString();
                string marginRight = ((XmlElement)doc.GetElementsByTagName("margins")[0]).GetAttribute("right").ToString();
                string marginTop = ((XmlElement)doc.GetElementsByTagName("margins")[0]).GetAttribute("top").ToString();
                string marginBottom = ((XmlElement)doc.GetElementsByTagName("margins")[0]).GetAttribute("bottom").ToString();
                string landScape = ((XmlElement)doc.GetElementsByTagName("landscape")[0]).GetAttribute("value").ToString();

                ps.PaperSize.Kind = (PaperKind)Enum.Parse(typeof(PaperKind), pageKind);

                if (ps.PaperSize.Kind == PaperKind.Custom)
                {
                    ps.PaperSize.Height = int.Parse(pageWidth);
                    ps.PaperSize.Width = int.Parse(pageHeight);
                }

                ps.Margins.Left = int.Parse(marginLeft);
                ps.Margins.Right = int.Parse(marginRight);
                ps.Margins.Top = int.Parse(marginTop);
                ps.Margins.Bottom = int.Parse(marginBottom);

                ps.Landscape = bool.Parse(landScape);
                m_ActualPageSetting = ps;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重置编辑器状态
        /// </summary>
        /// <param name="model"></param>
        public void ResetEditModeState(EmrModel model)
        {
            try
            {
                ResetEditMode(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void SetDocmentExamineState(EmrModel model)
        {
            try
            {
                //如果

                if (model.State == DrectSoft.Common.Eop.ExamineState.NotSubmit)
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 替换首次病程中的病程名称
        /// </summary>
        /// <param name="model"></param>
        private void ReplaceFirstDailyContent(EmrModel model)
        {
            try
            {
                ArrayList al = new ArrayList();
                this.zyEditorControl1.EMRDoc.GetAllSpecElement(al, this.zyEditorControl1.EMRDoc.RootDocumentElement, ElementType.FixedText, "T病程记录1");

                //把光标定位到它处
                ZYFixedText find = null;
                //已经有项目了
                if (al.Count > 0)
                {
                    foreach (ZYFixedText ele in al)
                    {
                        find = ele;
                    }
                }

                this.zyEditorControl1.EMRDoc.Content.AutoClearSelection = true;

                if (find != null)  //用于自动更换首程内容
                {
                    this.zyEditorControl1.EMRDoc.Content.MoveSelectStart(find.FirstElement);
                    //删除要替换的节点
                    this.zyEditorControl1.DeleteElement(find);
                }

                //插入日期，标题
                ZYText date = new ZYText();
                date.Name = "记录日期";
                date.Attributes.SetValue(ZYTextConst.c_FontBold, true);
                date.Text = model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");

                ZYText caption = new ZYText();
                caption.Name = "标题";
                caption.Text = model.Description.Trim() == "" ? "   " : model.Description;

                this.zyEditorControl1.EMRDoc._InsertBlock(date);

                InsertSpace(6, null);

                this.zyEditorControl1.EMRDoc._InsertBlock(caption);
                if (!model.RealFirstDaily)
                {
                    this.zyEditorControl1.EMRDoc._InsertChar('\r');
                }

                //设置编辑区域
                this.zyEditorControl1.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将病程插入到编辑器的界面中
        /// </summary>
        /// <param name="model"></param>
        public void InsertDailyInfo(EmrModel model)
        {
            try
            {
                m_CurrentModel = model;
                if (model.ModelContent != null)
                {
                    if (model.ModelContent.InnerText.Trim() != "")
                    {
                        //将未保存的病历显示在编辑器中
                        LoadDocment(model);
                        //在时间和标题中增加空格
                        InsertSpaceBetweenDateTimeAndTitle();
                        //todo 确认
                        ResetEditMode(model);
                        zyEditorControl1.EMRDoc.Modified = false;
                    }
                    return;
                }

                ArrayList al = new ArrayList();

                if (model.ModelContentHistory != null)
                {
                    model.ModelContent = (XmlDocument)model.ModelContentHistory.Clone();
                    model.ModelContentHistory = null;
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    doc.LoadXml(m_Util.GetTemplateContent(model.TempIdentity, model.PersonalTemplate));
                    //model.ModelContent = zyEditorControl1.BinaryToXml(m_Util.GetTemplateContent(model.TempIdentity));
                    model.ModelContent = doc;
                }

                if (model.FirstDailyEmrModel)//加载科室首个病程
                {
                    if (model.ModelContent.InnerText.Trim() != "")
                    {
                        LoadDocment(model);
                        ReplaceFirstDailyContent(model);
                        //在时间和标题中增加空格
                        InsertSpaceBetweenDateTimeAndTitle();
                        //换页
                        if (model.RealFirstDaily && m_Util.IsInsertPageEndOnFirstDailyEmr)
                        {
                            InsertPageEndToEmrEnd();
                        }
                        //todo 确认
                        ResetEditMode(model);
                    }
                    return;
                }     //xll病程首页进行处理

                //插入除首程外的其他病程
                this.zyEditorControl1.ActiveEditArea = null;
                this.zyEditorControl1.EMRDoc.GetAllSpecElement(al, this.zyEditorControl1.EMRDoc.RootDocumentElement, ElementType.Text, "记录日期");

                //把光标定位到它处
                ZYTextElement find = null;
                //已经有项目了
                if (al.Count > 0)
                {
                    foreach (ZYText ele in al)
                    {
                        //if (DateTime.Parse(ele.Text) == model.DisplayTime)
                        //{
                        //    MessageBox.Show("当前日期已经存在");
                        //    return;
                        //}
                        //else 
                        if (DateTime.Parse(ele.Text) > model.DisplayTime)
                        {
                            find = ele;
                            break;
                        }
                    }
                }

                if (find == null)
                {
                    find = this.zyEditorControl1.EMRDoc.Content.Elements[this.zyEditorControl1.EMRDoc.Content.Elements.Count - 1] as ZYTextElement;
                }

                //插入日期，标题
                ZYText date = new ZYText();
                date.Name = "记录日期";
                date.Attributes.SetValue(ZYTextConst.c_FontBold, true);
                date.Text = model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");

                ZYText caption = new ZYText();
                caption.Name = "标题";
                caption.Text = model.Description.Trim() == "" ? "   " : model.Description;

                if (find is ZYTextBlock)
                {
                    this.zyEditorControl1.EMRDoc.Content.CurrentElement = (find as ZYText).FirstElement;
                    //edit by cyq 2013-01-08
                    //this.zyEditorControl1.EMRDoc.Content.CurrentElement = this.zyEditorControl1.EMRDoc.Content.PreElement;
                }
                else
                {
                    this.zyEditorControl1.EMRDoc.Content.CurrentElement = find;
                }

                if (!model.RealFirstDaily)
                {
                    this.zyEditorControl1.EMRDoc._InsertChar('\r');
                }
                //todo 确认
                ResetEditMode(model);


                //左对齐
                this.zyEditorControl1.EMRDoc.SetAlign(ParagraphAlignConst.Left);

                //********************判断光标前是否存在分页符**********************
                if (model.IsNewPage)
                {
                    InsertPageEndLogic("BACK");
                }
                //****************************************************************


                this.zyEditorControl1.EMRDoc._InsertBlock(date);

                InsertSpace(6, null);

                this.zyEditorControl1.EMRDoc._InsertBlock(caption);
                this.zyEditorControl1.EMRDoc._InsertChar('\r');

                this.zyEditorControl1.EMRDoc.InsetDaliyTemplate(model.ModelContent);

                //********************判断光标后是否存在分页符**********************
                if (model.NewPageEnd)
                {
                    InsertPageEndLogic("FRONT");
                }
                //****************************************************************
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入换页符逻辑
        /// </summary>
        /// <param name="model">病历模板</param>
        /// <param name="direct">判断方向 front：往下找 back：往上找</param>
        private void InsertPageEndLogic(string direct)
        {
            try
            {
                int selectIndex = this.zyEditorControl1.EMRDoc.Content.SelectStart;
                bool isNeedInsertPageEnd = true;

                int elementsCount = this.zyEditorControl1.EMRDoc.Content.Elements.Count;

                if (direct.ToUpper() == "BACK")
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        if (selectIndex - i >= 0)
                        {
                            if (this.zyEditorControl1.EMRDoc.Content.Elements[selectIndex - i] is ZYPageEnd)
                            {
                                isNeedInsertPageEnd = false;
                                break;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (direct.ToUpper() == "FRONT")
                {
                    if (selectIndex == elementsCount - 1)
                    {
                        isNeedInsertPageEnd = true;
                    }
                    else
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            if (selectIndex + i < elementsCount)
                            {
                                if (this.zyEditorControl1.EMRDoc.Content.Elements[selectIndex + i] is ZYPageEnd)
                                {
                                    isNeedInsertPageEnd = false;
                                    break;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }

                //换页，如果当前页不存在换页符号，则插入换页符
                if (isNeedInsertPageEnd) InsertPageEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除指定日期的病程
        /// </summary>
        /// <param name="captionTime">病程设置的日期</param>
        public void DeleteDailyEmr(string captionTime)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(captionTime);
                //【1】设置模式为编辑
                ReadOnly2EditMode(dt);
                //【2】删除对应的病程，病程只能从后往前删除
                DeleteDailyEmrInner();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除选中的病程
        /// </summary>
        private void DeleteDailyEmrInner()
        {
            try
            {
                if (this.zyEditorControl1.ActiveEditArea != null)
                {
                    ZYTextElement topEle = this.zyEditorControl1.ActiveEditArea.TopElement;
                    ZYTextElement endEle = this.zyEditorControl1.ActiveEditArea.EndElement;

                    int start = this.zyEditorControl1.EMRDoc.Content.IndexOf(topEle);
                    int end = 0;
                    if (this.zyEditorControl1.ActiveEditArea.EndElement != null)
                    {
                        end = this.zyEditorControl1.EMRDoc.Content.IndexOf(endEle);
                    }
                    else
                    {
                        end = this.zyEditorControl1.EMRDoc.Content.Elements.Count - 1;
                    }
                    this.zyEditorControl1.EMRDoc.Content.SetSelection(start, end - start + 1);
                    this.zyEditorControl1.EMRDoc.Content.DeleteSeleciton(true);

                    this.zyEditorControl1.ActiveEditArea = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置编辑模式
        /// </summary>
        /// <param name="startTime"></param>
        private void ReadOnly2EditMode(DateTime startTime)
        {
            try
            {
                //MessageBox.Show(this.toolStripComboBox1.Text);
                //找到与选项相同的格式化数字项
                ArrayList al = new ArrayList();
                this.zyEditorControl1.EMRDoc.GetAllSpecElement(al, this.zyEditorControl1.EMRDoc.RootDocumentElement, ElementType.Text, "记录日期");


                //把光标定位到它处
                ZYText find = null;
                foreach (ZYText ele in al)
                {
                    if (DateTime.Parse(ele.Text) == startTime)
                    {
                        //MessageBox.Show("找到了 " );

                        find = ele;
                        break;
                    }
                }
                if (find == null)
                {
                    find = this.zyEditorControl1.EMRDoc.Content.Elements[this.zyEditorControl1.EMRDoc.Content.Elements.Count - 1] as ZYText;
                }
                if (find != null)
                {
                    this.zyEditorControl1.EMRDoc.Content.AutoClearSelection = true;
                    this.zyEditorControl1.EMRDoc.Content.MoveSelectStart(find.FirstElement);
                    this.zyEditorControl1.ActiveEditArea = new ActiveEditArea();
                    this.zyEditorControl1.ActiveEditArea.TopElement = find.FirstElement;
                    ZYText end = null;
                    //如果是最后一个
                    if (al.IndexOf(find) == al.Count - 1)
                    {
                        this.zyEditorControl1.ActiveEditArea.EndElement = null;// this.zyEditorControl1.EMRDoc.Content.Elements[this.zyEditorControl1.EMRDoc.Content.Elements.Count - 1] as ZYTextElement;
                    }
                    else
                    {
                        end = al[al.IndexOf(find) + 1] as ZYText;
                        this.zyEditorControl1.ActiveEditArea.EndElement = end.FirstElement;
                    }

                    this.zyEditorControl1.ScrollViewtopToCurrentElement();//Add By wwj 2012-03-06

                    //设置编辑区域
                    this.zyEditorControl1.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RefreshActiveEditArea(DateTime dt)
        {
            try
            {
                this.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;

                ArrayList al = new ArrayList();
                this.zyEditorControl1.EMRDoc.GetAllSpecElement(al, this.zyEditorControl1.EMRDoc.RootDocumentElement, ElementType.Text, "记录日期");

                //把光标定位到它处
                ZYText find = null;
                foreach (ZYText ele in al)
                {
                    if (DateTime.Parse(ele.Text) == dt)
                    {

                        find = ele;
                        break;
                    }
                }
                if (find != null)
                {
                    this.zyEditorControl1.EMRDoc.Content.AutoClearSelection = true;
                    this.zyEditorControl1.EMRDoc.Content.MoveSelectStart(find.FirstElement);
                    this.zyEditorControl1.ActiveEditArea = new ActiveEditArea();
                    this.zyEditorControl1.ActiveEditArea.TopElement = find.FirstElement;
                    ZYText end = null;
                    //如果是最后一个
                    if (al.IndexOf(find) == al.Count - 1)
                    {
                        this.zyEditorControl1.ActiveEditArea.EndElement = null;
                    }
                    else
                    {
                        end = al[al.IndexOf(find) + 1] as ZYText;
                        this.zyEditorControl1.ActiveEditArea.EndElement = end.FirstElement;
                    }


                    //设置编辑区域

                    this.zyEditorControl1.Refresh();


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 在病历编辑器中定位病程记录
        /// </summary>
        /// <param name="node"></param>
        public void LocateDailyEmrMode(TreeListNode node)
        {
            try
            {
                if (node.Tag is EmrModel)
                {
                    EmrModel model = node.Tag as EmrModel;
                    string displayDateTime = model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");

                    this.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;

                    ArrayList al = new ArrayList();
                    this.zyEditorControl1.EMRDoc.GetAllSpecElement(al, this.zyEditorControl1.EMRDoc.RootDocumentElement, ElementType.Text, "记录日期");

                    //把光标定位到它处
                    ZYText find = null;
                    foreach (ZYText ele in al)
                    {
                        if (ele.Text == displayDateTime)
                        {
                            find = ele;
                            break;
                        }
                    }
                    if (find != null)
                    {
                        this.zyEditorControl1.EMRDoc.Content.AutoClearSelection = true;
                        this.zyEditorControl1.EMRDoc.Content.MoveSelectStart(find.FirstElement);
                        this.zyEditorControl1.ScrollViewtopToCurrentElement();

                        //设置编辑区域
                        this.zyEditorControl1.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入指定个数的空格
        /// </summary>
        /// <param name="count"></param>
        private void InsertSpace(int count, ZYText find)
        {
            try
            {
                //if (find != null && find.FirstElement != null)
                //{
                //    //判断病程的时间和名称之间有没有空格，如果有空格则退出
                //    if (find.FirstElement.Parent != null && find.FirstElement.Parent.Parent != null && find.FirstElement.Parent.Parent.ChildElements.Count > 1)
                //    {
                //        if (find.FirstElement.Parent.Parent.ChildElements[1] is ZYTextChar)
                //        {
                //            if (((ZYTextChar)find.FirstElement.Parent.Parent.ChildElements[1]).Char == ' ')
                //            {
                //                return;
                //            }
                //        }
                //    }
                //}

                //在病程的时间和名称之间插入空格
                for (int i = 0; i < count; i++)
                {
                    this.zyEditorControl1.EMRDoc._InsertChar(' ');
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertSpaceBetweenDateTimeAndTitle()
        {
            try
            {
                DocumentModel documentModel = this.zyEditorControl1.EMRDoc.Info.DocumentModel;
                this.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;

                ArrayList al = new ArrayList();
                this.zyEditorControl1.EMRDoc.GetAllSpecElement(al, this.zyEditorControl1.EMRDoc.RootDocumentElement, ElementType.Text, "标题");

                //把光标定位到它处
                ZYText find = null;
                //foreach (ZYText ele in al)
                for (int i = 0; i < al.Count; i++)
                {
                    //if (al.Count > 1 && i == 0) return;

                    ZYText ele = al[i] as ZYText;
                    find = ele;
                    if (find != null)
                    {
                        this.zyEditorControl1.EMRDoc.Content.AutoClearSelection = true;
                        this.zyEditorControl1.EMRDoc.Content.MoveSelectStart(find.FirstElement);

                        // InsertSpace(6, find);
                    }
                }

                //设置编辑区域
                this.zyEditorControl1.Refresh();
                this.zyEditorControl1.EMRDoc.Info.DocumentModel = documentModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置病程的编辑区
        /// </summary>
        /// <param name="node"></param>
        public void SetDailyEmrEditArea(TreeListNode node)
        {
            try
            {
                if (node == null) return;
                EmrModel model = node.Tag as EmrModel;
                if (model != null)
                {
                    //诊断病程的处理
                    if (model.DailyEmrModel)
                    {
                        //todo此次为何设置为只读 zhouhui why？
                        //this.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                        this.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Read;
                        DateTime dt = Convert.ToDateTime(model.DisplayTime);
                        ReadOnly2EditMode(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.Parent is XtraTabPage && this.Parent.Parent is XtraTabControl && this.Parent.Parent.Parent is UCEmrInput)
                {
                    UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;
                    form.SaveDocumentPublic();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.Parent is XtraTabPage && this.Parent.Parent is XtraTabControl && this.Parent.Parent.Parent is UCEmrInput)
                {
                    UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;
                    form.DeleteDocumentPublic();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSubmit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.Parent is XtraTabPage && this.Parent.Parent is XtraTabControl && this.Parent.Parent.Parent is UCEmrInput)
                {
                    UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;
                    form.SubmitDocumentPublic();
                    form.GetStateImage();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.Parent is XtraTabPage && this.Parent.Parent is XtraTabControl && this.Parent.Parent.Parent is UCEmrInput)
                {
                    UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;
                    form.AuditDocumentPublic();
                    form.GetStateImage();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 取消审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemCancelAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.Parent is XtraTabPage && this.Parent.Parent is XtraTabControl && this.Parent.Parent.Parent is UCEmrInput)
                {
                    UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;
                    form.CancelAuditPublic();
                    form.GetStateImage();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 得到焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void zyEditorControl1_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (this.Parent is XtraTabPage && this.Parent.Parent is XtraTabControl && this.Parent.Parent.Parent is UCEmrInput)
                {
                    UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;

                    //在编辑器获得焦点的时候不需要定位病程
                    //bool isLocateDaily = form.IsLocateDaily;
                    //form.IsLocateDaily = false;
                    bool isNeedActiveEditArea = form.IsNeedActiveEditArea;
                    form.IsNeedActiveEditArea = false;

                    form.SelectedPageChanged(this.Parent as XtraTabPage);

                    //form.IsLocateDaily = isLocateDaily;
                    form.IsNeedActiveEditArea = isNeedActiveEditArea;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        public void GotFocusNotTriggerEvent()
        {
            try
            {
                zyEditorControl1.GotFocus -= new EventHandler(zyEditorControl1_GotFocus);
                zyEditorControl1.Focus();
                zyEditorControl1.GotFocus += new EventHandler(zyEditorControl1_GotFocus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddGotFocusEvent()
        {
            try
            {
                zyEditorControl1.GotFocus += new EventHandler(zyEditorControl1_GotFocus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RemoveGotFocusEvent()
        {
            try
            {
                zyEditorControl1.GotFocus -= new EventHandler(zyEditorControl1_GotFocus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 設置子標題
        /// </summary>
        public void SetSubTitleFont()
        {
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(zyEditorControl1.EMRDoc.HeadString);
            //XmlNodeList xmlNodeList = xmlDoc.GetElementsByTagName("span");
            //xmlNodeList.Item(1).Attributes["fontname"].Value = xmlNodeList.Item(0).Attributes["fontname"].Value;
            //xmlNodeList.Item(1).Attributes["fontsize"].Value = xmlNodeList.Item(0).Attributes["fontsize"].Value;
            //xmlNodeList.Item(1).Attributes.RemoveNamedItem("fontbold");
            //zyEditorControl1.EMRDoc.HeadString = xmlDoc.InnerXml;
        }

        /// <summary>
        /// 光标移动到病历最后，插入换页符
        /// </summary>
        private void InsertPageEndToEmrEnd()
        {
            try
            {
                zyEditorControl1.EMRDoc.Content.MoveSelectStart(zyEditorControl1.ToXMLString().Length);
                this.zyEditorControl1.EMRDoc._InsertChar('\r');
                ZYPageEnd pageEnd = new ZYPageEnd();
                zyEditorControl1.EMRDoc._InsertElement(pageEnd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertPageEnd()
        {
            try
            {
                ZYPageEnd pageEnd = new ZYPageEnd();
                zyEditorControl1.EMRDoc._InsertElement(pageEnd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void barButtonItemLocate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.Parent is XtraTabPage && this.Parent.Parent is XtraTabControl && this.Parent.Parent.Parent is UCEmrInput)
                {
                    UCEmrInput form = this.Parent.Parent.Parent as UCEmrInput;
                    LocateDailyEmrByCursor(form);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 在编辑器中定位对应的病程，如果对于此病程有编辑权限则同时设置编辑区域
        /// </summary>
        /// <param name="form"></param>
        private void LocateDailyEmrByCursor(UCEmrInput form)
        {
            try
            {
                TreeListNodes nodes = form.GetACTreeListNodes();
                if (nodes == null) return;

                int selectIndex = this.zyEditorControl1.EMRDoc.Content.SelectStart;
                //把光标定位到它处
                ZYText find = null;
                for (int i = selectIndex; i >= 0; i--)
                {
                    ZYTextElement ele = this.zyEditorControl1.EMRDoc.Content.Elements[i] as ZYTextElement;
                    if (ele != null && ele.Parent != null && ele.Parent is ZYText && ((ZYText)ele.Parent).Name == "记录日期")
                    {
                        find = ele.Parent as ZYText;
                        break;
                    }
                }
                if (find != null && nodes.Count > 0)
                {
                    foreach (TreeListNode node in nodes)
                    {
                        EmrModel emrModel = node.Tag as EmrModel;
                        if (emrModel != null)
                        {
                            if (emrModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss").IndexOf(find.text) >= 0)
                            {
                                form.FocusedNodeChanged(node);
                                break;
                            }
                        }
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
