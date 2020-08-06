using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Core.MainEmrPad.DailyForm;
using DrectSoft.Core.MainEmrPad.HelpForm;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Common;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Library.EmrEditor.Src.Gui;
using DrectSoft.Library.EmrEditor.Src.Print;
using DrectSoft.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using ZYTextDocumentLib;


namespace DrectSoft.Core.MainEmrPad.New
{
    /// <summary>
    /// 编辑器封装类
    /// </summary>
    public partial class EditorForm : UserControl
    {
        #region Field && Property

        IEmrHost m_App;
        Inpatient m_CurrentInpatient;

        /// <summary>
        /// 编辑器封装类数据访问层
        /// </summary>
        private EditorFormDal m_PadFormDal;

        /// <summary>
        /// 默认页面设置，即通过配置表m_Appcfg中获取的统一页面配置
        /// </summary>
        public XPageSettings DefaultPageSetting;

        /// <summary>
        /// 实际页面设置，即病历内容的Xml中实际存储的页面配置
        /// </summary>
        public XPageSettings ActualPageSetting;

        /// <summary>
        /// 行间距
        /// </summary>
        public float LineSpace = 1.5f;

        /// <summary>
        /// 元素的背景色
        /// </summary>
        public static Color s_ElementColor = Color.FromArgb(0x99, 0xCC, 0xFF);
        public static string s_ElementStyle = "背景色";

        /// <summary>
        /// 诊断逻辑类
        /// </summary>
        DiagFormLogic m_DiagFormLogic;

        List<string> m_ExtraEvents;

        /// <summary>
        /// 编辑器
        /// </summary>
        public ZYEditorControl CurrentEditorControl
        {
            get
            {
                return zyEditorControl1;
            }
        }

        /// <summary>
        /// 当前用户的信息
        /// </summary>
        Employee _doctorEmployye;
        Employee DoctorEmployee
        {
            get
            {
                if (_doctorEmployye == null)
                {
                    _doctorEmployye = new Employee(DS_Common.currentUser.Id);
                    _doctorEmployye.ReInitializeProperties();
                }
                _doctorEmployye.CurrentDept.Code = DS_Common.currentUser.CurrentDeptId;
                _doctorEmployye.CurrentWard.Code = DS_Common.currentUser.CurrentWardId;
                return _doctorEmployye;
            }
        }

        /// <summary>
        /// 转科ID
        /// </summary>
        public string DeptChangeID { get; set; }

        #endregion

        #region .ctor
        public EditorForm(Inpatient inpatient, IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_CurrentInpatient = inpatient;
                m_App = app;
                Init();
                this.CurrentEditorControl.MouseDown += new MouseEventHandler(CurrentEditorControl_MouseDown);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Init

        #region 初始化
        private void Init()
        {
            try
            {
                InitVariable();
                InitEmrPageSetting();
                InitEditor();
                SetBackColorDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 初始化变量
        private void InitVariable()
        {
            try
            {
                m_DiagFormLogic = new DiagFormLogic(m_App);
                m_PadFormDal = new EditorFormDal(m_App);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 获取统一的页面设置，即从m_Appcfg表中捞取的页面配置的Xml
        /// <summary>
        /// 获取统一的页面设置，即从m_Appcfg表中捞取的页面配置的Xml
        /// </summary>
        private void InitEmrPageSetting()
        {
            try
            {
                string xmldoc = BasicSettings.GetStringConfig("PageSetting");
                System.Xml.XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(xmldoc);
                XmlNode pagesetting = doc.ChildNodes[0].SelectSingleNode("pagesettings");
                if (pagesetting != null)
                {
                    XmlElement ele = (pagesetting as XmlElement).SelectSingleNode("page") as XmlElement;
                    XPageSettings ps = new XPageSettings();
                    ps.PaperSize.Kind = (PaperKind)Enum.Parse(typeof(PaperKind), ele.GetAttribute("kind"));
                    if (ps.PaperSize.Kind == PaperKind.Custom)
                    {
                        ps.PaperSize.Height = int.Parse(ele.GetAttribute("height"));
                        ps.PaperSize.Width = int.Parse(ele.GetAttribute("width"));
                    }
                    ele = (pagesetting as XmlElement).SelectSingleNode("margins") as XmlElement;
                    ps.Margins.Left = int.Parse(ele.GetAttribute("left"));
                    ps.Margins.Right = int.Parse(ele.GetAttribute("right"));
                    ps.Margins.Top = int.Parse(ele.GetAttribute("top"));
                    ps.Margins.Bottom = int.Parse(ele.GetAttribute("bottom"));

                    ele = (pagesetting as XmlElement).SelectSingleNode("landscape") as XmlElement;
                    ps.Landscape = bool.Parse(ele.GetAttribute("value"));
                    ele = (pagesetting as XmlElement).SelectSingleNode("header") as XmlElement;
                    ps.HeaderHeight = int.Parse(ele.GetAttribute("height"));
                    ele = (pagesetting as XmlElement).SelectSingleNode("footer") as XmlElement;
                    ps.FooterHeight = int.Parse(ele.GetAttribute("height"));
                    DefaultPageSetting = ps;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 初始化编辑器

        private void InitEditor()
        {
            try
            {
                InitEmrDefaultSetting();
                m_DiagFormLogic = new DiagFormLogic(m_App);
                m_ExtraEvents = new List<string>();
                CurrentEditorControl.InitializeDocument();
                //  CurrentEditorControl.EMRDoc.Info.PatientID = m_App.CurrentPatientInfo.NoOfHisFirstPage;
                CurrentEditorControl.MouseDragScroll = false;
                CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Read;
                CurrentEditorControl.Dock = DockStyle.Fill;
                CurrentEditorControl.VerticalScroll.Value = 0;
                CurrentEditorControl.VerticalScroll.Value = 0;
                CurrentEditorControl.HorizontalScroll.Value = 0;
                CurrentEditorControl.SetLineSpace(LineSpace);//设置行间距
                ZYEditorControl.ElementBackColor = s_ElementColor;
                ZYEditorControl.ElementStyle = s_ElementStyle;
                CurrentEditorControl.EMRDoc.Info.LogicDelete = true;
                CurrentEditorControl.EMRDoc.Info.AutoLogicDelete = true;
                CurrentEditorControl.OnHelpButtonClick += new ZYEditorControl.HelpButtonClick(CurrentEditorControl_OnHelpButtonClick);//委托外部按钮事件
                CurrentEditorControl.OnGetOuterDataBinary += new ZYEditorControl.GetOuterDataBinary(CurrentEditorControl_OnGetOuterDataBinary);//委托从外部获得xml文档
                CurrentEditorControl.MouseClick += new MouseEventHandler(CurrentEditorControl_MouseClick);//鼠标单击事件
                CurrentEditorControl.MouseDoubleClick += new MouseEventHandler(CurrentEditorControl_MouseDoubleClick);//鼠标双击事件
                CurrentEditorControl.GotFocus += new EventHandler(CurrentEditorControl_GotFocus);//获取焦点事件
                CurrentEditorControl.EMRDoc.OnShowFindForm += new ShowFindFormHandler(EMRDoc_OnShowFindForm);
                CurrentEditorControl.EMRDoc.OnSaveEMR += new SaveEMRHandler(EMRDoc_OnSaveEMR);
                this.VisibleChanged += new EventHandler(PadForm_VisibleChanged);
                CurrentEditorControl.SetTitle(m_App.CurrentHospitalInfo.Name);//设置主标题

                SetDefaultFontSize(m_App.EmrDefaultSettings.FontSize);
                //zyEditorControl1.SetSubTitle(m_Util.m_App.CurrentHospitalInfo.Subname);//设置副标题
                zyEditorControl1.EMRDoc.Info.PatientID = m_App.CurrentPatientInfo.NoOfHisFirstPage;//设置患者ID号，用于阻止不同患者之前的数据粘贴
                zyEditorControl1.EMRDoc.setPatientid(m_App.CurrentPatientInfo.NoOfHisFirstPage);
                //zyEditorControl1.ContentChanged += new EventHandler(pnlText_ContentChanged);//病历内容改变事件
                //zyEditorControl1.EMRDoc.OnSelectionChanged += new EventHandler(EMRDoc_OnSelectionChanged);//选择改变事件
                //zyEditorControl1.OnGetOuterData += new ZYEditorControl.GetOuterData(pnlText_OnGetOuterData);//委托从外部获得xml文档
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置编辑器属性
        /// </summary>
        private void SetEditorProperty()
        {
            try
            {
                CurrentEditorControl.SetTitle(m_App.CurrentHospitalInfo.Name);//设置主标题
                CurrentEditorControl.SetLineSpace(LineSpace);//设置行间距
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
                UCEmrInputBody inputBody = Util.GetParentUserControl<UCEmrInputBody>(this);
                inputBody.Save();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 病历编辑器默认设置
        /// </summary>
        private void InitEmrDefaultSetting()
        {
            try
            {
                if (m_App.EmrDefaultSettings.LineSpace != "")
                {
                    LineSpace = float.Parse(m_App.EmrDefaultSettings.LineSpace);
                }
                else
                {
                    LineSpace = 1.5f;
                }

                s_ElementColor = m_App.EmrDefaultSettings.EleColor;
                s_ElementStyle = m_App.EmrDefaultSettings.EleStyle;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 设置编辑器的默认背景色
        /// <summary>
        /// 设置编辑器的默认背景色
        /// </summary>
        public void SetBackColorDefault()
        {
            try
            {
                //设置文书录入默认背景色
                string emrSettingBackColor = BasicSettings.GetStringConfig("SetPageColor");
                string[] colors = emrSettingBackColor.Split(',');
                if (colors.Length == 3)
                {
                    int IntR = Int32.Parse(colors[0].ToString());
                    int IntG = Int32.Parse(colors[1].ToString());
                    int IntB = Int32.Parse(colors[2].ToString());
                    this.CurrentEditorControl.PageBackColor = Color.FromArgb(IntR, IntG, IntB);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Event

        private void EditorForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }



        void EMRDoc_OnShowFindForm()
        {
            try
            {
                SearchFrm searchfrm = new SearchFrm(m_App, CurrentEditorControl);
                searchfrm.Show(this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void PadForm_VisibleChanged(object sender, EventArgs e)
        { }

        void CurrentEditorControl_GotFocus(object sender, EventArgs e)
        {
            try
            {
                UCEmrInputBody emrInputBody = Util.GetParentUserControl<UCEmrInputBody>(this);
                if (emrInputBody != null)
                {
                    emrInputBody.CurrentForm_GotFocus();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        void CurrentEditorControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.CurrentEditorControl.EMRDoc.Info.DocumentModel != DocumentModel.Edit)
                {
                    return;
                }

                ZYTextElement ele = this.CurrentEditorControl.GetElementByPos(e.Location.X, e.Location.Y);
                if (ele != null && ele.Parent != null && ele.Parent is ZYText)
                {
                    if ((ele.Parent as ZYText).Name == "记录日期")
                    {
                        UCEmrInputBody emrInputBody = Util.GetParentUserControl<UCEmrInputBody>(this);
                        DailyFormDisplayDateTime formDateTime = new DailyFormDisplayDateTime(m_App, emrInputBody);
                        formDateTime.StartPosition = FormStartPosition.CenterParent;

                        if (formDateTime.ShowDialog() == DialogResult.Yes)
                        {
                            ZYText date = ele.Parent as ZYText;
                            date.Text = formDateTime.CommitDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                            ////修改病程的记录日期,加粗显示
                            RefreshElement();

                            //修改节点和tab页标题
                            emrInputBody.ReSetTreeListNodeAndTabPageName((ele.Parent as ZYText).text, date.text, null, null);
                            emrInputBody.SetTreeNodeIndex(emrInputBody.CurrentTreeList.FocusedNode, 0);
                        }
                    }
                    else if ((ele.Parent as ZYText).Name == "标题")
                    {
                        DailyFormTitle formTitle = new DailyFormTitle((ele.Parent as ZYText).ToEMRString(), m_App);
                        formTitle.StartPosition = FormStartPosition.CenterParent;
                        if (formTitle.ShowDialog() == DialogResult.Yes)
                        {
                            ZYText caption = ele.Parent as ZYText;
                            caption.Text = formTitle.GetTitle();

                            ////修改病程的标题
                            RefreshElement();

                            //修改标题描述
                            UCEmrInputBody emrInputBody = Util.GetParentUserControl<UCEmrInputBody>(this);
                            emrInputBody.ReSetTreeListNodeAndTabPageName(null, null, (ele.Parent as ZYText).text, caption.text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 刷新元素，在替换宏，替换可替换元素，替换病程中的标题和时间  后调用此方法属性页面
        /// </summary>
        private void RefreshElement()
        {
            try
            {
                this.CurrentEditorControl.EMRDoc.RefreshSize();
                this.CurrentEditorControl.EMRDoc.ContentChanged();
                this.CurrentEditorControl.EMRDoc.OwnerControl.Refresh();
                this.CurrentEditorControl.EMRDoc.UpdateCaret();
            }
            catch (Exception)
            {
                throw;
            }
        }

        byte[] CurrentEditorControl_OnGetOuterDataBinary(ElementType type, string name)
        {
            try
            {
                return m_PadFormDal.GetTempleteItem(name);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
                return null;
            }
        }

        void CurrentEditorControl_MouseClick(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// 诊断逻辑
        /// </summary>
        string CurrentEditorControl_OnHelpButtonClick(string name)
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
                    #region Modified By wwj 2013-08-09 与复用项目数据捞取的方法保持一致
                    //zyEditorControl1.EMRDoc._InserString(zyEditorControl1.EMRDoc.GetReplaceText(name));
                    string replaceItemText = ZYEditorControl.GetReplaceTextByName(zyEditorControl1.ToXMLString(), name);
                    zyEditorControl1.EMRDoc._InserString(replaceItemText);
                    #endregion
                }

                return "编辑器控件中按钮事件 " + name;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
                return "编辑器控件中按钮事件 " + name;
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 设置默认的页面设置
        /// </summary>
        public void SetDefaultPageSetting()
        {
            try
            {
                //设置页眉页脚高度
                CurrentEditorControl.EMRDoc.DocumentHeaderHeight = DefaultPageSetting.HeaderHeight;
                CurrentEditorControl.EMRDoc.DocumentFooterHeight = DefaultPageSetting.FooterHeight;
                CurrentEditorControl.EMRDoc.Pages.PageSettings = DefaultPageSetting;
                //一定要刷新，否则界面显示有误
                this.EditorRefresh();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 设置实际的页面设置
        /// </summary>
        public void SetActualPageSetting()
        {
            try
            {
                CurrentEditorControl.EMRDoc.Pages.PageSettings = ActualPageSetting;
                //一定要刷新，否则界面显示有误
                this.EditorRefresh();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 设置编辑器的默认字体大小，当以新的空行开始输入字符的时候此属性生效
        /// </summary>
        /// <param name="fontSize">字体大小 如： "五号"、"小五"、"六号"</param>
        public void SetDefaultFontSize(string fontSize)
        {
            try
            {
                if (FontCommon.allFontSizeName.Contains(fontSize))
                {
                    ZYEditorControl.DefaultFontSize = fontSize;
                }
                else
                {
                    ZYEditorControl.DefaultFontSize = "10.5";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 设置编辑器的背景色
        /// </summary>
        public void SetBackColor(Color color)
        {
            try
            {
                this.CurrentEditorControl.PageBackColor = color;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑器是否可以编辑
        /// </summary>
        /// <param name="isCanEdit"></param>
        public void CanEdit(bool isCanEdit)
        {
            try
            {
                if (isCanEdit)
                {
                    this.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Edit;//开放编辑
                    EditorRefresh();
                }
                else
                {
                    this.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Read;//只读，不可编辑
                    EditorRefresh();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetClearModel()
        {
            try
            {
                this.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Clear;//开放编辑
                this.CurrentEditorControl.EMRDoc.Info.ShowParagraphFlag = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刷新编辑器
        /// </summary>
        public void EditorRefresh()
        {
            try
            {
                CurrentEditorControl.EMRDoc.RefreshLine();
                CurrentEditorControl.EMRDoc.RefreshPages();
                CurrentEditorControl.EMRDoc.OwnerControl.UpdatePages();
                CurrentEditorControl.EMRDoc.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改痕迹功能
        /// </summary>
        /// <param name="model"></param>
        public void ResetEditModeState(EmrModel model)
        {
            try
            {
                bool isModified = CurrentEditorControl.EMRDoc.Modified;
                if (model.CreatorXH == DS_Common.currentUser.DoctorId)
                {
                    isModified = true;
                }

                //CurrentEditorControl.SetTitle(m_App.CurrentHospitalInfo.Name);
                //CurrentEditorControl.EMRDoc.Pages.PageSettings = ActualPageSetting != null ? ActualPageSetting : DefaultPageSetting;
                //设置页眉页脚高度
                //CurrentEditorControl.EMRDoc.DocumentHeaderHeight = DefaultPageSetting.HeaderHeight;
                //CurrentEditorControl.EMRDoc.DocumentFooterHeight = DefaultPageSetting.FooterHeight;
                //CurrentEditorControl.SetLineSpace(LineSpace);

                CurrentEditorControl.EMRDoc.Modified = isModified;
                CurrentEditorControl.EMRDoc.RefreshLine();
                CurrentEditorControl.EMRDoc.RefreshPages();
                CurrentEditorControl.EMRDoc.OwnerControl.UpdatePages();
                CurrentEditorControl.EMRDoc.Refresh();

                CurrentEditorControl.EMRDoc.Info.LogicDelete = true;
                CurrentEditorControl.EMRDoc.Info.AutoLogicDelete = true;
                CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                IEmrModelPermision modelPermision;

                //DrectSoft.Common.Eop.Employee empl = new DrectSoft.Common.Eop.Employee(YD_Common.currentUser.Id);
                //empl.ReInitializeProperties();
                modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Audit, DoctorEmployee);

                if (modelPermision.CanDo(model)) // 文件已提交，处于审核模式
                {
                    SetEditorAuditState(DoctorEmployee.Grade);
                }
            }
            catch (Exception)
            {
                throw new Exception("ResetEditModeState 方法出错");
            }
        }

        /// <summary>
        /// 审核病历时，编辑器的设置
        /// </summary>
        /// <param name="model"></param>
        public void SetEditorAuditState(string grade)
        {
            try
            {
                DoctorGrade doctorGrade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), grade);

                //记录修改痕迹
                CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                CurrentEditorControl.EMRDoc.Info.LogicDelete = true;
                CurrentEditorControl.EMRDoc.Info.AutoLogicDelete = true;
                CurrentEditorControl.EMRDoc.Info.ShowMark = true;
                CurrentEditorControl.EMRDoc.Info.ShowAll = true;
                CurrentEditorControl.EMRDoc.Info.ShowMark = true;
                CurrentEditorControl.EMRDoc.SaveLogs.CurrentSaveLog.UserName = m_App.User.Name;

                switch (doctorGrade)
                {
                    case DoctorGrade.Resident://住院医生
                        CurrentEditorControl.EMRDoc.SaveLogs.CurrentSaveLog.Level = 0;
                        CurrentEditorControl.EMRDoc.Content.UserLevel = 1;
                        break;
                    case DoctorGrade.Attending://主治医生
                        CurrentEditorControl.EMRDoc.SaveLogs.CurrentSaveLog.Level = 1;
                        CurrentEditorControl.EMRDoc.Content.UserLevel = 2;
                        break;
                    case DoctorGrade.AssociateChief://副主任医师
                    case DoctorGrade.Chief://主任医师
                        CurrentEditorControl.EMRDoc.SaveLogs.CurrentSaveLog.Level = 2;
                        CurrentEditorControl.EMRDoc.Content.UserLevel = 9;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载病历内容
        /// </summary>
        /// <param name="emrContent"></param>
        public void LoadDocument(string emrContent)
        {
            try
            {
                SetPageSize(emrContent);
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(emrContent);
                FillModelMacro(doc);
                CurrentEditorControl.LoadXML(doc);
                SetEditorProperty();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 在编辑器中查找当前光标所在的病程记录
        /// </summary>
        /// <param name="form"></param>
        public string LocateDailyEmrByCursor()
        {
            try
            {
                int selectIndex = CurrentEditorControl.EMRDoc.Content.SelectStart;
                //把光标定位到它处
                ZYText find = null;
                for (int i = selectIndex; i >= 0; i--)
                {
                    ZYTextElement ele = CurrentEditorControl.EMRDoc.Content.Elements[i] as ZYTextElement;
                    if (ele != null && ele.Parent != null && ele.Parent is ZYText && ((ZYText)ele.Parent).Name == "记录日期")
                    {
                        find = ele.Parent as ZYText;
                        break;
                    }
                }
                if (find != null)
                {
                    return find.text;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 在编辑器中定位指定的位置
        /// </summary>
        /// <param name="captionDatetime"></param>
        public void SetCurrentElement(string captionDatetime)
        {
            try
            {
                ArrayList al = new ArrayList();
                this.CurrentEditorControl.EMRDoc.GetAllSpecElement(al, CurrentEditorControl.EMRDoc.RootDocumentElement, ElementType.Text, "记录日期");
                //把光标定位到它处
                ZYTextElement find = null;
                //已经有项目了
                if (al.Count > 0)
                {
                    foreach (ZYText ele in al)
                    {
                        if (ele.Text.Trim() == captionDatetime)
                        {
                            find = ele;
                            break;
                        }
                    }
                    if (find != null)
                    {
                        if (find is ZYTextBlock)
                        {
                            CurrentEditorControl.EMRDoc.Content.CurrentElement = (find as ZYText).FirstElement;
                        }
                        else
                        {
                            CurrentEditorControl.EMRDoc.Content.CurrentElement = find;
                        }
                        CurrentEditorControl.ScrollViewtopToCurrentElement();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 在编辑器中定位与指定位置最近的位置
        /// </summary>
        /// <param name="captionDatetime"></param>
        public string SetCurrentElementNearest(string captionDateStr)
        {
            try
            {
                DateTime captionDateTime = new DateTime();
                if (!DateTime.TryParse(captionDateStr, out captionDateTime))
                {
                    return "";
                }

                ArrayList al = new ArrayList();
                this.CurrentEditorControl.EMRDoc.GetAllSpecElement(al, CurrentEditorControl.EMRDoc.RootDocumentElement, ElementType.Text, "记录日期");
                //已经有项目了
                if (al.Count > 0)
                {
                    ZYTextElement findElement = null;

                    foreach (ZYText ele in al)
                    {
                        DateTime currentElementDateTime = new DateTime();
                        if (DateTime.TryParse(ele.Text.Trim(), out currentElementDateTime))
                        {
                            if ((currentElementDateTime - captionDateTime).TotalSeconds > 0)
                            {
                                continue;
                            }
                            else
                            {
                                findElement = ele;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (findElement != null)
                    {
                        if (findElement is ZYTextBlock)
                        {
                            CurrentEditorControl.EMRDoc.Content.CurrentElement = (findElement as ZYText).FirstElement;
                            CurrentEditorControl.ScrollViewtopToCurrentElement();
                            return (findElement as ZYText).text;
                        }
                        else
                        {
                            CurrentEditorControl.EMRDoc.Content.CurrentElement = findElement;
                            CurrentEditorControl.ScrollViewtopToCurrentElement();
                        }
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 填充可替换项（复用项目）的内容<replace></replace>
        /// </summary>
        /// <param name="model"></param>
        public void ReplaceModelMacro(string modelName)
        {
            try
            {
                ReplaceModelMacroInner(modelName);
                //new Thread(new ParameterizedThreadStart(ReplaceModelMacroInner)).Start(modelName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 复用项目替换其中的内容
        /// </summary>
        /// <param name="name"></param>
        private void ReplaceModelMacroInner(object name)
        {
            try
            {
                string modelName = name.ToString();

                //循环每个可替换元素，根据可替换元素的Name属性，查询并赋值Text属性
                //获得病历中所有可替换元素列表
                ArrayList al = new ArrayList();
                ZYTextDocument doc = CurrentEditorControl.EMRDoc;
                doc.GetAllSpecElement(al, doc.RootDocumentElement, ElementType.Replace, null);
                bool isModified = CurrentEditorControl.EMRDoc.Modified;

                //无可替换元素则退出
                if (al.Count < 0) return;

                Dictionary<string, string> padFormDictionary = new Dictionary<string, string>();

                //获得当前病历需要替换的项目需要从哪些病历中提取
                DataTable dtReplaceItem = m_PadFormDal.GetReplaceItem(modelName);

                //源病历的列表
                List<string> sourceEmrList = (from DataRow dr in dtReplaceItem.Rows
                                              select dr["source_emrname"].ToString()).Distinct().ToList();

                foreach (string sourceEmr in sourceEmrList)
                {
                    //PadForm padForm = GetSourcePadFormForReplaceItem(sourceEmr);
                    //padFormDictionary.Add(sourceEmr, padForm);

                    string emrContent = GetSourceEditorFormForReplaceItem(sourceEmr);
                    if (emrContent != "")
                    {
                        padFormDictionary.Add(sourceEmr, emrContent);
                    }
                }

                if (padFormDictionary.Count == 0) return;

                //替换当前病历中的项目
                foreach (ZYReplace m in al)
                {
                    List<string> destList = GetSourceEMRByDestItemAndDestEmrName(modelName, m.Name, dtReplaceItem);
                    if (destList.Count == 0) continue;
                    if (destList.Count == 2)
                    {
                        string source_emrname = destList[0];
                        string source_itemname = destList[1];

                        if (padFormDictionary.ContainsKey(source_emrname))
                        {
                            string emrContent = padFormDictionary[source_emrname];
                            if (emrContent != "")
                            {
                                //Modified By wwj 2013-08-09 与复用项目数据捞取的方法保持一致
                                string replaceText = ZYEditorControl.GetReplaceTextByName(emrContent, source_itemname);
                                //string replaceText = this.zyEditorControl1.EMRDoc.GetReplaceText(source_itemname);
                                if (replaceText.Trim() != "" && m.Text != replaceText)
                                {
                                    m.Text = replaceText;
                                    // zyEditorControl1.EMRDoc._InserString(replaceText); 
                                    SetElementText(replaceText, m);
                                }
                            }
                        }
                    }
                }

                RefreshEditor(doc, isModified);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }



        private void SetElementText(string text, ZYReplace replaceElement)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, ZYReplace>(SetElementText), text, replaceElement);
            }
            else
            {
                replaceElement.Text = text;
            }
        }

        private void RefreshEditor(ZYTextDocument doc, bool isModified)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<ZYTextDocument, bool>(RefreshEditor), doc, isModified);
            }
            else
            {
                doc.RefreshSize();
                doc.ContentChanged();
                doc.OwnerControl.Refresh();
                doc.UpdateCaret();
                doc.Modified = isModified;
            }
        }

        /// <summary>
        /// 重新填充宏元素
        /// </summary>
        public void FillModelMacro()
        {
            try
            {
                if (this != null)
                {
                    bool isModified = this.CurrentEditorControl.EMRDoc.Modified;

                    //替换文档中的宏
                    //获得所有宏元素列表
                    ArrayList al = new ArrayList();
                    ZYTextDocument doc = this.CurrentEditorControl.EMRDoc;

                    doc.GetAllSpecElement(al, doc.RootDocumentElement, ElementType.Macro, null);

                    //循环每个宏元素，根据宏元素的Name属性，查询并赋值线Text属性
                    foreach (ZYMacro m in al)
                    {
                        if (m.Name == "科室" && m.Text.Trim() != "科室")
                            continue;
                        if (m.Name == "入院科室" && m.Text.Trim() != "入院科室")
                            continue;
                        if (m.Name == "病区" && m.Text.Trim() != "病区")
                            continue;
                        if (m.Name == "医师签名" && m.text.Trim() != "医师签名")
                            continue;
                        if (m.Name == "当前日期" && m.text.Trim() != "当前日期")
                            continue;
                        if (m.Name == "当前时间" && m.text.Trim() != "当前时间")
                            continue;

                        //Add by wwj 2013-05-14 解决中心医院“医生签名”宏元素名称设置不正确而导致医师的名字被替换的问题
                        if (m.Name == "主任医师" && m.text.Trim() != "主任医师")
                            continue;
                        if (m.Name == "主治医师" && m.text.Trim() != "主治医师")
                            continue;
                        if (m.Name == "住院医师" && m.text.Trim() != "住院医师")
                            continue;
                        if (m.Name == "管床医生" && m.text.Trim() != "管床医生")
                            continue;

                        m.Text = m_PadFormDal.GetDataByNameForMacro(m_CurrentInpatient.NoOfFirstPage.ToString(), m.Name);
                    }

                    doc.RefreshSize();
                    doc.ContentChanged();
                    doc.OwnerControl.Refresh();
                    doc.UpdateCaret();

                    this.CurrentEditorControl.EMRDoc.Modified = isModified;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 替换首次病程中内容的标题 在加载完病程（即调用LoadDocument）之后调用
        /// </summary>
        /// <param name="model"></param>
        public bool ReplaceFirstDailyTitle(string displayTime, string title)
        {
            try
            {
                ArrayList al = new ArrayList();
                this.CurrentEditorControl.EMRDoc.GetAllSpecElement(al, this.CurrentEditorControl.EMRDoc.RootDocumentElement, ElementType.FixedText, "T病程记录1");

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
                else
                {
                    return false;
                }

                //用于自动更换首程内容
                if (find != null)
                {
                    this.zyEditorControl1.EMRDoc.Content.MoveSelectStart(find.FirstElement);
                    //删除要替换的节点
                    this.zyEditorControl1.DeleteElement(find);
                }

                //插入日期，标题
                bool isDateBold = true;
                bool isTitleBold = false;
                int spaceCount = 6;
                float dateFontSize = 12;
                float titleFontSize = 12;
                string config = DS_SqlService.GetConfigValueByKey("EmrDateAndTitle");
                if (!string.IsNullOrEmpty(config))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(config);
                    XmlNodeList dateBoldList = doc.GetElementsByTagName("DateBold");
                    if (null != dateBoldList && dateBoldList.Count > 0)
                    {
                        string dateBoldValue = null == dateBoldList[0].InnerText ? "" : dateBoldList[0].InnerText.Trim();
                        isDateBold = dateBoldValue == "1";
                    }
                    XmlNodeList titleBoldList = doc.GetElementsByTagName("TitleBold");
                    if (null != titleBoldList && titleBoldList.Count > 0)
                    {
                        string titleBoldValue = null == titleBoldList[0].InnerText ? "" : titleBoldList[0].InnerText.Trim();
                        isTitleBold = titleBoldValue == "1";
                    }
                    XmlNodeList spaceCountList = doc.GetElementsByTagName("SpaceCounts");
                    if (null != spaceCountList && spaceCountList.Count > 0)
                    {
                        string spaceCountValue = null == spaceCountList[0].InnerText ? "" : spaceCountList[0].InnerText.Trim();
                        if (!string.IsNullOrEmpty(spaceCountValue) && Tool.IsNumeric(spaceCountValue))
                        {
                            spaceCount = int.Parse(spaceCountValue);
                        }
                    }
                    XmlNodeList dateFontSizeList = doc.GetElementsByTagName("DateFontSize");
                    if (null != dateFontSizeList && dateFontSizeList.Count > 0)
                    {
                        string fontSizeValue = null == dateFontSizeList[0].InnerText ? "" : dateFontSizeList[0].InnerText.Trim();
                        if (!string.IsNullOrEmpty(fontSizeValue) && Tool.IsNumeric(fontSizeValue))
                        {
                            dateFontSize = float.Parse(fontSizeValue);
                        }
                    }
                    XmlNodeList titleFontSizeList = doc.GetElementsByTagName("TitleFontSize");
                    if (null != titleFontSizeList && titleFontSizeList.Count > 0)
                    {
                        string fontSizeValue = null == titleFontSizeList[0].InnerText ? "" : titleFontSizeList[0].InnerText.Trim();
                        if (!string.IsNullOrEmpty(fontSizeValue) && Tool.IsNumeric(fontSizeValue))
                        {
                            titleFontSize = float.Parse(fontSizeValue);
                        }
                    }
                }

                ZYText date = new ZYText();
                date.Name = "记录日期";
                if (isDateBold)
                {
                    date.Attributes.SetValue(ZYTextConst.c_FontBold, isDateBold);
                }
                date.Attributes.SetValue(ZYTextConst.c_FontSize, dateFontSize);
                date.Text = displayTime;

                ZYText caption = new ZYText();
                caption.Name = "标题";
                if (isTitleBold)
                {
                    caption.Attributes.SetValue(ZYTextConst.c_FontBold, isTitleBold);
                }
                caption.Attributes.SetValue(ZYTextConst.c_FontSize, titleFontSize);
                caption.Text = title.Trim() == "" ? "   " : title;

                this.zyEditorControl1.EMRDoc._InsertBlock(date);
                InsertSpace(spaceCount);
                this.zyEditorControl1.EMRDoc._InsertBlock(caption);

                //刷新编辑区域
                this.zyEditorControl1.Refresh();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入日常病程的时间和标题 在加载完病程（即调用LoadDocument）之前调用
        /// </summary>
        /// <param name="displayTime"></param>
        /// <param name="title"></param>
        public void InsertDisplayTimeAndTitle(XmlDocument doc, string displayTime, string title)
        {
            try
            {
                //<p align="0">
                //  <text fontbold="1" type="text" name="记录日期">2011-09-08 13:30:48</text>
                //  <span fontbold="1" creator="22">      </span>
                //  <text type="text" name="标题">术后病程</text>
                //  <eof />
                //</p>

                //已经存在<text>...</text>标签的不需要再次插入
                if (doc.GetElementsByTagName("text").Cast<XmlNode>().Count(node =>
                {
                    if (((XmlElement)node).HasAttribute("type") && ((XmlElement)node).HasAttribute("name"))
                    {
                        if (node.Attributes["type"].Value == "text" && node.Attributes["name"].Value == "记录日期")
                        {
                            return true;
                        }
                    }
                    return false;
                }) > 0)
                {
                    return;
                }

                XmlNodeList nodeList = doc.SelectSingleNode("document/body").ChildNodes;
                XmlNode bodyNode = doc.SelectSingleNode("document/body");
                if (nodeList.Count > 0 && bodyNode != null)
                {
                    XmlElement pElement = doc.CreateElement("p");
                    XmlAttribute pAlignAttr = doc.CreateAttribute("align");
                    pAlignAttr.Value = "0";

                    bool isDateBold = true;
                    bool isTitleBold = false;
                    int spaceCount = 6;
                    float dateFontSize = 9;
                    float titleFontSize = 9;
                    string config = DS_SqlService.GetConfigValueByKey("EmrDateAndTitle");
                    if (!string.IsNullOrEmpty(config))
                    {
                        XmlDocument cigDoc = new XmlDocument();
                        cigDoc.LoadXml(config);
                        XmlNodeList dateBoldList = cigDoc.GetElementsByTagName("DateBold");
                        if (null != dateBoldList && dateBoldList.Count > 0)
                        {
                            string dateBoldValue = null == dateBoldList[0].InnerText ? "" : dateBoldList[0].InnerText.Trim();
                            isDateBold = dateBoldValue == "1";
                        }
                        XmlNodeList titleBoldList = cigDoc.GetElementsByTagName("TitleBold");
                        if (null != titleBoldList && titleBoldList.Count > 0)
                        {
                            string titleBoldValue = null == titleBoldList[0].InnerText ? "" : titleBoldList[0].InnerText.Trim();
                            isTitleBold = titleBoldValue == "1";
                        }
                        XmlNodeList spaceCountList = cigDoc.GetElementsByTagName("SpaceCounts");
                        if (null != spaceCountList && spaceCountList.Count > 0)
                        {
                            string spaceCountValue = null == spaceCountList[0].InnerText ? "" : spaceCountList[0].InnerText.Trim();
                            if (!string.IsNullOrEmpty(spaceCountValue) && Tool.IsNumeric(spaceCountValue))
                            {
                                spaceCount = int.Parse(spaceCountValue);
                            }
                        }
                        XmlNodeList dateFontSizeList = cigDoc.GetElementsByTagName("DateFontSize");
                        if (null != dateFontSizeList && dateFontSizeList.Count > 0)
                        {
                            string fontSizeValue = null == dateFontSizeList[0].InnerText ? "" : dateFontSizeList[0].InnerText.Trim();
                            if (!string.IsNullOrEmpty(fontSizeValue) && Tool.IsNumeric(fontSizeValue))
                            {
                                dateFontSize = float.Parse(fontSizeValue);
                            }
                        }
                        XmlNodeList titleFontSizeList = cigDoc.GetElementsByTagName("TitleFontSize");
                        if (null != titleFontSizeList && titleFontSizeList.Count > 0)
                        {
                            string fontSizeValue = null == titleFontSizeList[0].InnerText ? "" : titleFontSizeList[0].InnerText.Trim();
                            if (!string.IsNullOrEmpty(fontSizeValue) && Tool.IsNumeric(fontSizeValue))
                            {
                                titleFontSize = float.Parse(fontSizeValue);
                            }
                        }
                    }

                    XmlElement textElementCaptionDatetime = doc.CreateElement("text");
                    XmlAttribute textTypeDatatimeAttr = doc.CreateAttribute("type");
                    XmlAttribute textNameDatatimeAttr = doc.CreateAttribute("name");
                    textTypeDatatimeAttr.Value = "text";
                    textNameDatatimeAttr.Value = "记录日期";
                    if (isDateBold)
                    {
                        XmlAttribute textFontboldDatatimeAttr = doc.CreateAttribute("fontbold");
                        textFontboldDatatimeAttr.Value = "1";
                        textElementCaptionDatetime.Attributes.Append(textFontboldDatatimeAttr);
                    }

                    XmlAttribute dateFontSizeAttr = doc.CreateAttribute("fontsize");
                    dateFontSizeAttr.Value = dateFontSize.ToString();
                    textElementCaptionDatetime.Attributes.Append(dateFontSizeAttr);

                    textElementCaptionDatetime.Attributes.Append(textTypeDatatimeAttr);
                    textElementCaptionDatetime.Attributes.Append(textNameDatatimeAttr);

                    if (WeiWenProcess.weiwen)
                    {
                        string strDateTime = "";
                        displayTime = displayTime.Trim();
                        for (int i = 0; i < displayTime.Length; i++)
                        {
                            strDateTime += displayTime[displayTime.Length - 1 - i];
                        }
                        displayTime = strDateTime;
                    }
                    textElementCaptionDatetime.InnerText = displayTime;

                    XmlElement spanElement = doc.CreateElement("span");
                    XmlAttribute spanFontboldAttr = doc.CreateAttribute("fontbold");
                    spanFontboldAttr.Value = "1";
                    spanElement.Attributes.Append(spanFontboldAttr);

                    XmlAttribute spanFontSizeAttr = doc.CreateAttribute("fontsize");
                    spanFontSizeAttr.Value = dateFontSize.ToString();
                    spanElement.Attributes.Append(spanFontSizeAttr);

                    string spaceStr = string.Empty;
                    for (int i = 0; i < spaceCount; i++)
                    {
                        spaceStr += " ";
                    }
                    spanElement.InnerText = spaceStr;

                    XmlElement textElementTitle = doc.CreateElement("text");
                    XmlAttribute textTypeTitleAttr = doc.CreateAttribute("type");
                    XmlAttribute textNameTitleAttr = doc.CreateAttribute("name");
                    textTypeTitleAttr.Value = "text";
                    textNameTitleAttr.Value = "标题";
                    if (isTitleBold)
                    {
                        XmlAttribute textFontboldTitleAttr = doc.CreateAttribute("fontbold");
                        textFontboldTitleAttr.Value = "1";
                        textElementTitle.Attributes.Append(textFontboldTitleAttr);
                    }

                    XmlAttribute titleFontSizeAttr = doc.CreateAttribute("fontsize");
                    titleFontSizeAttr.Value = titleFontSize.ToString();
                    textElementTitle.Attributes.Append(titleFontSizeAttr);

                    textElementTitle.Attributes.Append(textTypeTitleAttr);
                    textElementTitle.Attributes.Append(textNameTitleAttr);
                    textElementTitle.InnerText = title.Trim() == "" ? "  " : title;

                    XmlElement eofElement = doc.CreateElement("eof");

                    pElement.AppendChild(textElementCaptionDatetime);
                    pElement.AppendChild(spanElement);
                    pElement.AppendChild(textElementTitle);
                    pElement.AppendChild(eofElement);

                    bodyNode.InsertBefore(pElement, nodeList[0]);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Private Method

        /// <summary>
        /// 设置页面大小
        /// </summary>
        /// <param name="emrContent"></param>
        private void SetPageSize(string emrContent)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(emrContent);

                string pageKind = ((XmlElement)doc.GetElementsByTagName("page")[0]).GetAttribute("kind").ToString();
                string pageWidth = ((XmlElement)doc.GetElementsByTagName("page")[0]).GetAttribute("width").ToString();
                string pageHeight = ((XmlElement)doc.GetElementsByTagName("page")[0]).GetAttribute("height").ToString();
                string marginLeft = ((XmlElement)doc.GetElementsByTagName("margins")[0]).GetAttribute("left").ToString();
                string marginRight = ((XmlElement)doc.GetElementsByTagName("margins")[0]).GetAttribute("right").ToString();
                string marginTop = ((XmlElement)doc.GetElementsByTagName("margins")[0]).GetAttribute("top").ToString();
                string marginBottom = ((XmlElement)doc.GetElementsByTagName("margins")[0]).GetAttribute("bottom").ToString();
                string landScape = ((XmlElement)doc.GetElementsByTagName("landscape")[0]).GetAttribute("value").ToString();

                XPageSettings ps = new XPageSettings();
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
                ActualPageSetting = ps;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 填充宏元素的内容<macro></macro>
        /// </summary>
        private void FillModelMacro(XmlDocument emrDoc)
        {
            try
            {
                #region 替换文档中的宏

                //获得所有宏元素列表
                XmlNodeList nodes = emrDoc.GetElementsByTagName("macro");

                string deptName = string.Empty;
                string wardName = string.Empty;
                string bedID = string.Empty;

                if (!string.IsNullOrEmpty(DeptChangeID))
                {
                    DataTable dtDeptInfo = m_PadFormDal.GetDeptChangeInfo(DeptChangeID);
                    if (dtDeptInfo.Rows.Count > 0)
                    {
                        foreach (DataColumn dc in dtDeptInfo.Columns)
                        {
                            switch (dc.ColumnName.ToLower())
                            {
                                case "deptname": deptName = dtDeptInfo.Rows[0]["deptname"].ToString();
                                    break;
                                case "wardname": wardName = dtDeptInfo.Rows[0]["wardname"].ToString();
                                    break;
                                //case "bedid": bedID = dtDeptInfo.Rows[0]["bedid"].ToString();
                                //    break;
                            }
                        }
                    }
                }

                SetErroMaro(nodes);

                //循环每个宏元素，根据宏元素的Name属性，查询并赋值线Text属性
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["name"].Value.Trim() == "科室" && node.InnerText.Trim() == "科室" && !string.IsNullOrEmpty(deptName))
                    {
                        node.InnerText = deptName;
                        continue;
                    }
                    if (node.Attributes["name"].Value.Trim() == "病区" && node.InnerText.Trim() == "病区" && !string.IsNullOrEmpty(wardName))
                    {
                        node.InnerText = wardName;
                        continue;
                    }

                    //if (node.Attributes["name"].Value.Trim() == "床号" && node.InnerText.Trim() == "床号" && !string.IsNullOrEmpty(bedID))
                    //{
                    //    node.InnerText = bedID;
                    //    continue;
                    //}

                    if (node.Attributes["name"].Value.Trim() == "科室" && node.InnerText.Trim() != "科室")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "入院科室" && node.InnerText.Trim() != "入院科室")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "病区" && node.InnerText.Trim() != "病区")
                        continue;
                    //if (node.Attributes["name"].Value.Trim() == "床号" && node.InnerText.Trim() != "床号")
                    //    continue;
                    if (node.Attributes["name"].Value.Trim() == "住院号" && node.InnerText.Trim() != "住院号")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "医师签名" && node.InnerText.Trim() != "医师签名")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "当前日期" && node.InnerText.Trim() != "当前日期")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "当前时间" && node.InnerText.Trim() != "当前时间")
                        continue;

                    //Add by wwj 2013-05-14 解决中心医院“医生签名”宏元素名称设置不正确而导致医师的名字被替换的问题
                    if (node.Attributes["name"].Value.Trim() == "主任医师" && node.InnerText.Trim() != "主任医师")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "主治医师" && node.InnerText.Trim() != "主治医师")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "住院医师" && node.InnerText.Trim() != "住院医师")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "管床医生" && node.InnerText.Trim() != "管床医生")
                        continue;

                    node.InnerText = m_PadFormDal.GetDataByNameForMacro(m_CurrentInpatient.NoOfFirstPage.ToString(), node.Attributes["name"].Value);
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// xll 20130624 清除另存为个人模板和科室模板时 早期宏没有替换
        /// 在加载的时候如果住院号不一样 提换下面几个的宏 为当前最新的宏
        /// </summary>
        /// <param name="nodes"></param>
        private void SetErroMaro(XmlNodeList nodes)
        {
            foreach (XmlNode nodec in nodes)
            {

                if (nodec.Attributes["name"].Value.Trim() == "住院号" && nodec.InnerText.Trim() != "住院号")
                {
                    if (m_CurrentInpatient.RecordNoOfHospital.Trim() != nodec.InnerText.Trim())
                    {
                        //住院号宏对象中内容不等于住院号 并且值与当前病人住院号不一致时，替换掉原有的
                        foreach (XmlNode node in nodes)
                        {
                            if (node.Attributes["name"].Value.Trim() == "科室")
                            {
                                node.InnerText = "科室";
                                continue;
                            }
                            if (node.Attributes["name"].Value.Trim() == "入院科室")
                            {
                                node.InnerText = "入院科室";
                                continue;
                            }
                            if (node.Attributes["name"].Value.Trim() == "病区")
                            {
                                node.InnerText = "病区";
                                continue;
                            }
                            if (node.Attributes["name"].Value.Trim() == "床号")
                            {
                                node.InnerText = "床号";
                                continue;
                            }
                            if (node.Attributes["name"].Value.Trim() == "住院号")
                            {
                                node.InnerText = "住院号";
                                continue;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通过病历名称获取病历编辑窗体
        /// </summary>
        /// <param name="sourceEMR"></param>
        /// <returns></returns>
        public string GetSourceEditorFormForReplaceItem(string sourceEMR)
        {
            try
            {
                DataTable dtEmrContent = m_PadFormDal.GetEmrContentByName(sourceEMR, m_CurrentInpatient.NoOfFirstPage.ToString());
                if (dtEmrContent.Rows.Count > 0)
                {
                    string emrContent = dtEmrContent.Rows[0]["content"].ToString();
                    //EditorForm form = new EditorForm(m_CurrentInpatient, m_App);
                    //form.LoadDocument(emrContent);
                    //return form;
                    return emrContent;
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过目标项和目标病历获得源病历
        /// </summary>
        /// <param name="destEmrModelName"></param>
        /// <param name="destItem"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<string> GetSourceEMRByDestItemAndDestEmrName(string destEmrModelName, string destItem, DataTable dt)
        {
            try
            {
                List<string> list = new List<string>();
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["dest_itemname"].ToString() == destItem)
                        {
                            string source_emrname = dr["source_emrname"].ToString();
                            string source_itemname = dr["source_itemname"].ToString();
                            list.Add(source_emrname);
                            list.Add(source_itemname);
                            break;
                        }
                    }
                }
                return list;
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
        private void InsertSpace(int count)
        {
            try
            {
                for (int i = 0; i < count; i++)
                {
                    this.CurrentEditorControl.EMRDoc._InsertChar(' ');
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 设置插入当前病历内容的按钮事件
        /// </summary>
        /// <param name="modelName"></param>
        public void SetReplaceEvent(string modelName)
        {
            try
            {
                if (m_ExtraEvents.Count > 0) return;

                //获得当前病历所有的替换项
                DataTable replaceItemDataTable = m_PadFormDal.GetReplaceItem(modelName);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void popupMenuRightMouse_BeforePopup(object sender, CancelEventArgs e)
        {
            try
            {
                UCEmrInputPreViewInner parentUserControl = Util.GetParentUserControl<UCEmrInputPreViewInner>(this);
                if (parentUserControl != null)
                {
                    btnItemSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnItemDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnItemFind.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnItemCopy.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnItemPaste.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    return;
                }

                if (CurrentEditorControl.EMRDoc.Info.DocumentModel == DocumentModel.Edit)
                {
                    btnItemSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnItemDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnItemFind.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnItemCopy.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    btnItemPaste.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    return;
                }
                else
                {
                    btnItemSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnItemDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnItemFind.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnItemCopy.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnItemPaste.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    return;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        void CurrentEditorControl_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    ZYTextElement element = this.CurrentEditorControl.GetElementByPos(e.X, e.Y);
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

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItemFind_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CurrentEditorControl.EMRDoc.ShowFindForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItemCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                zyEditorControl1.EMRDoc._Copy();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        /// <summary>
        /// 粘贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItemPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                zyEditorControl1.EMRDoc._Paste();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        /// <summary>
        /// 开放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItemReBack_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //CurrentEditorControl.EMRDoc.ShowFindForm();
                // if(  CurrentEditorControl.EMRDoc.)
                if (CurrentEditorControl.EMRDoc.CanModify())
                { }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存病历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItemSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CurrentEditorControl.EMRDoc.SaveEMR();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除病历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                UCEmrInputBody emrInputBody = Util.GetParentUserControl<UCEmrInputBody>(this);
                if (null == emrInputBody)
                {
                    return;
                }
                emrInputBody.DeleteDocument();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 调用外部“删除病历”的功能
        /// </summary>
        public event SaveEMRHandler OnDeleteEMR = null;
        private void DeleteEMR()
        {
            try
            {
                if (OnDeleteEMR != null)
                {
                    OnDeleteEMR();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnItemclean_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DrectSoft.Library.EmrEditor.Src.Clipboard.EmrClipboard.Data = null;
                DrectSoft.Library.EmrEditor.Src.Clipboard.EmrClipboard.PatientID = null;
                Clipboard.SetData(System.Windows.Forms.DataFormats.UnicodeText, null);
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("清空成功");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string getImgStr = @"select userpic from userpicsign where userid='{0}' and valide='1';";
                DataTable dtImage = m_App.SqlHelper.ExecuteDataTable(string.Format(getImgStr, m_App.User.Id), CommandType.Text);
                if (dtImage == null || dtImage.Rows.Count <= 0) return;
                string imgStr = dtImage.Rows[0]["USERPIC"].ToString();
                byte[] bytes = Convert.FromBase64String(imgStr);
                this.CurrentEditorControl.EMRDoc._InsertImage(bytes);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请确认你所选文件是否为图片格式。" + ex.Message);
            }
        }
    }
}