using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Emr.Util;
using System.Collections;
using System.Data;
using System.Xml;
using DevExpress.XtraTreeList.Nodes;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using System.Drawing;
using System.Runtime.InteropServices;
using DrectSoft.Core;
using DevExpress.XtraNavBar;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class UCEmrInput
    {
        #region 设置病人信息
        private void SetPatientInfo(int noofinpat)
        {
            DataTable dt = m_RecordDal.GetPatientInfo(noofinpat);
            barStatic_Age.Caption = dt.Rows[0]["age"].ToString();
            barStatic_Bed.Caption = dt.Rows[0]["bed"].ToString();
            barStatic_Blh.Caption = dt.Rows[0]["patid"].ToString();
            barStatic_Dept.Caption = dt.Rows[0]["deptname"].ToString() + ":" + dt.Rows[0]["wardname"].ToString();
            barStatic_InWardDate.Caption = dt.Rows[0]["inwarddate"].ToString();
            barStaticPatName.Caption = dt.Rows[0]["patname"].ToString();
            barStaticSex.Caption = dt.Rows[0]["sexname"].ToString();
            barButtonItem_InCount.Caption = "入院次数:" + m_RecordDal.GetHistoryInCount(noofinpat);
            barButtonItem_InCount.Appearance.ForeColor = Color.Blue;
        }
        #endregion

        #region 可替换元素赋值

        /// <summary>
        /// 是否开启只有编辑区范围内可替换的开关 
        /// true:  编辑区范围内可替换，编辑区范围外不可替换
        /// false: 编辑区范围内可替换，编辑区范围外也可替换
        /// </summary>
        private const bool c_IsOpenEditAreaCanReplaceFlag = true;

        /// <summary>
        /// 获得编辑区域的索引值  
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="startIndex">索引起始点</param>
        /// <param name="endIndex">索引终止点</param>
        private void GetEditAreaIndex(ZYTextDocument doc, out int startIndex, out int endIndex)
        {
            startIndex = -1;
            endIndex = -1;
            if (doc.OwnerControl.ActiveEditArea != null)
            {
                ZYTextElement topEle = doc.OwnerControl.ActiveEditArea.TopElement;
                ZYTextElement endEle = doc.OwnerControl.ActiveEditArea.EndElement;

                int start = doc.Content.IndexOf(topEle);
                int end = 0;
                if (doc.OwnerControl.ActiveEditArea.EndElement != null)
                {
                    end = doc.Content.IndexOf(endEle);
                }
                else
                {
                    end = doc.Content.Elements.Count - 1;
                }

                startIndex = start;
                endIndex = end;
            }
        }

        private void ReplaceModelMacro(EmrModel model)
        {
            //循环每个可替换元素，根据可替换元素的Name属性，查询并赋值Text属性
            //获得病历中所有可替换元素列表
            ArrayList al = new ArrayList();
            ZYTextDocument doc = CurrentForm.zyEditorControl1.EMRDoc;
            doc.GetAllSpecElement(al, doc.RootDocumentElement, ElementType.Replace, null);

            //无可替换元素则退出
            if (al.Count < 0) return;

            Dictionary<string, PadForm> padFormDictionary = new Dictionary<string, PadForm>();

            //获得当前病历需要替换的项目需要从哪些病历中提取
            DataTable dtReplaceItem = m_RecordDal.GetReplaceItem(model.ModelName);

            //源病历的列表
            List<string> sourceEmrList = m_RecordDal.GetSourceEMRByDestEmrName(model.ModelName, dtReplaceItem);

            foreach (string sourceEmr in sourceEmrList)
            {
                PadForm padForm = GetSourcePadFormForReplaceItem(sourceEmr);
                padFormDictionary.Add(sourceEmr, padForm);
            }

            if (padFormDictionary.Count == 0) return;

            int startIndex;
            int endIndex;
            GetEditAreaIndex(doc, out startIndex, out endIndex);

            DocumentModel docModel = CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel;

            //替换当前病历中的项目
            foreach (ZYReplace m in al)
            {
                //排除编辑范围之外的可替换元素
                if (c_IsOpenEditAreaCanReplaceFlag && docModel == DocumentModel.Read && startIndex != -1 && endIndex != -1)
                {
                    int zyReplaceIndex = CurrentForm.zyEditorControl1.EMRDoc.Content.IndexOf(m.FirstElement);
                    if (zyReplaceIndex < startIndex || zyReplaceIndex > endIndex)
                    {
                        continue;
                    }
                }

                List<string> destList = m_RecordDal.GetSourceEMRByDestItemAndDestEmrName(model.ModelName, m.Name, dtReplaceItem);
                if (destList.Count == 0) continue;
                if (destList.Count == 2)
                {
                    string source_emrname = destList[0];
                    string source_itemname = destList[1];
                    PadForm padForm = padFormDictionary[source_emrname];
                    if (padForm != null)
                    {
                        m.Text = padForm.zyEditorControl1.EMRDoc.GetReplaceText(source_itemname);
                    }
                }
            }
            doc.RefreshSize();
            doc.ContentChanged();
            doc.OwnerControl.Refresh();
            doc.UpdateCaret();
        }

        #region 通过配置文件找到目标文件
        private PadForm GetSourcePadFormForReplaceItem(string sourceEMR)
        {
            m_EmrSourceModel = null;
            GetEmrDestModel(treeList1.Nodes, sourceEMR);

            if (m_EmrSourceModel != null)
            {
                PadForm pad = new PadForm(m_patUtil, m_RecordDal);
                InsertEmrModelContent(pad, m_EmrSourceModel);
                return pad;
            }
            else
            {
                return null;
            }
        }

        private EmrModel m_EmrSourceModel;

        /// <summary>
        /// 通过源病历的名称，找到病历的内容
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="sourceEMR"></param>
        private void GetEmrDestModel(TreeListNodes nodes, string sourceEMR)
        {
            if (m_EmrSourceModel != null) return;
            if (nodes != null)
            {
                foreach (TreeListNode node in nodes)
                {
                    if (node.Nodes.Count > 0)
                    {
                        GetEmrDestModel(node.Nodes, sourceEMR);
                    }
                    else
                    {
                        EmrModel model = node.Tag as EmrModel;
                        if (model != null && model.ModelName.IndexOf(sourceEMR) == 0)
                        {
                            if (model.DailyEmrModel)//如果是病程，需要找到首次病程
                            {
                                m_EmrSourceModel = (from TreeListNode n in nodes
                                                    where ((EmrModel)n.Tag).FirstDailyEmrModel
                                                    select (EmrModel)n.Tag).FirstOrDefault();
                            }
                            else
                            {
                                m_EmrSourceModel = model.Clone();
                            }
                            if (m_EmrSourceModel != null && m_EmrSourceModel.ModelContent == null)
                            {
                                m_EmrSourceModel = m_RecordDal.LoadModelInstance(model.InstanceId).Clone();
                            }
                            return;
                        }
                    }
                }
            }
        }
        #endregion

        #endregion

        #region 宏元素赋值

        private void FillModelMacro(PadForm padForm)
        {
            if (padForm != null)
            {
                bool isModified = padForm.zyEditorControl1.EMRDoc.Modified;

                //根据病历的不同状态，调用程序在此处初始化宏的值。
                //替换标题中的宏
                XmlDocument headerdoc = new XmlDocument();
                headerdoc.LoadXml(padForm.zyEditorControl1.EMRDoc.HeadString);
                XmlNodeList nodes = headerdoc.SelectNodes("header/p/macro");
                foreach (XmlNode node in nodes) //为默认值的时候进行替换
                {
                    if (node.Attributes["name"].Value.Trim() == "科室" && node.InnerText.Trim() != "科室")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "病区" && node.InnerText.Trim() != "病区")
                        continue;

                    //Add by wwj 2012-07-17
                    if (node.Attributes["name"].Value.Trim() == "医师签名" && node.InnerText.Trim() != "医师签名")
                        continue;

                    if (node.Attributes["name"].Value.Trim() == "当前日期" && node.InnerText.Trim() != "当前日期")
                        continue;
                    if (node.Attributes["name"].Value.Trim() == "当前时间" && node.InnerText.Trim() != "当前时间")
                        continue;

                    node.InnerText = GetDataByNameForMacro(node.Attributes["name"].Value);
                }
                padForm.zyEditorControl1.EMRDoc.HeadString = headerdoc.OuterXml;

                //替换文档中的宏
                //获得所有宏元素列表
                ArrayList al = new ArrayList();
                ZYTextDocument doc = padForm.zyEditorControl1.EMRDoc;
                doc.GetAllSpecElement(al, doc.RootDocumentElement, ElementType.Macro, null);

                //循环每个宏元素，根据宏元素的Name属性，查询并赋值线Text属性
                foreach (ZYMacro m in al)
                {
                    if (m.Name == "科室" && m.Text.Trim() != "科室")
                        continue;
                    if (m.Name == "病区" && m.Text.Trim() != "病区")
                        continue;

                    //Add by wwj 2012-07-17
                    if (m.Name == "医师签名" && m.text.Trim() != "医师签名")
                        continue;

                    if (m.Name == "当前日期" && m.text.Trim() != "当前日期")
                        continue;
                    if (m.Name == "当前时间" && m.text.Trim() != "当前时间")
                        continue;

                    m.Text = GetDataByNameForMacro(m.Name);
                }

                doc.RefreshSize();
                doc.ContentChanged();
                doc.OwnerControl.Refresh();
                doc.UpdateCaret();

                padForm.zyEditorControl1.EMRDoc.Modified = isModified;
            }
        }

        string GetDataByNameForMacro(string name)
        {
            //此处应该写具体的实现方法
            return MacroUtil.FillMarcValue(m_CurrentInpatient.NoOfFirstPage.ToString(), name, m_app.User.Id);
        }

        #endregion

        #region 右侧工具栏的病人历史病历

        /// <summary>
        /// 初始化病人历史记录树
        /// </summary>
        /// <param name="noofinpat"></param>
        private void InitTreeListHistory(int noofinpat)
        {
            if (treeListHistory.Nodes.Count == 0)
            {
                //加载病人信息
                LoadHistoryPat(noofinpat);
                //加载病人对应的病历信息
                LoadHistoryEmr();
            }
        }

        /// <summary>
        /// 加载病人信息
        /// </summary>
        /// <param name="dt"></param>
        private void LoadHistoryPat(int noofinpat)
        {
            DataTable dt = m_RecordDal.GetHistoryInpatient(noofinpat);
            foreach (DataRow dr in dt.Rows)
            {
                string noofinpatHistory = dr["noofinpat"].ToString();
                string nameHistory = dr["name"].ToString();
                string admitdateHistory = dr["admitdate"].ToString();

                TreeListNode node = treeListHistory.AppendNode(new object[] { "入院时间:" + admitdateHistory }, null);
                node.Tag = new HistoryInpatient { NoOfInpat = noofinpatHistory, Name = nameHistory };
            }
        }

        /// <summary>
        /// 加载病人的病历记录
        /// </summary>
        /// <param name="noofinpat"></param>
        private void LoadHistoryEmr()
        {
            foreach (TreeListNode node in treeListHistory.Nodes)
            {
                HistoryInpatient pat = node.Tag as HistoryInpatient;
                if (pat == null) continue;
                DataTable datatable = m_RecordDal.GetHistoryInpatientFolder(Convert.ToInt32(pat.NoOfInpat));
                foreach (DataRow dr in datatable.Rows)
                {
                    //加载病历文件夹
                    TreeListNode subNode = treeListHistory.AppendNode(new object[] { dr["CNAME"].ToString() }, node);
                    EmrModelContainer container = new EmrModelContainer(dr);
                    subNode.Tag = container;

                    if (container.EmrContainerType != ContainerType.None)
                    {
                        //加载具体的病历
                        DataTable leafs = m_patUtil.GetPatRecInfo(container.ContainerCatalog, pat.NoOfInpat);
                        {
                            foreach (DataRow leaf in leafs.Rows)
                            {
                                EmrModel leafmodel = new EmrModel(leaf);
                                TreeListNode leafNode = treeListHistory.AppendNode(new object[] { leafmodel.ModelName }, subNode);
                                leafNode.Tag = leafmodel;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 历史病人节点
        /// </summary>
        class HistoryInpatient
        {
            /// <summary>
            /// 病人首页序号
            /// </summary>
            public string NoOfInpat { get; set; }

            /// <summary>
            /// 病人名字
            /// </summary>
            public string Name { get; set; }
        }

        #region 双击历史病人节点
        private void treeListHistory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeListHitInfo hitInfo = treeListHistory.CalcHitInfo(e.Location);
            if (hitInfo != null && hitInfo.Node != null)
            {
                TreeListNode node = hitInfo.Node;
                EmrModel model = node.Tag as EmrModel;
                if (model != null)
                {
                    //弹出病历内容
                    HistoryEmrForm printForm = new HistoryEmrForm(m_patUtil, m_RecordDal, model, node);
                    printForm.ShowDialog();
                    CurrentForm.Focus();
                    if (printForm.IsNeedInsertContent && CurrentForm != null)
                    {
                        if (CanEdit())
                        {
                            CurrentForm.zyEditorControl1.EMRDoc._Paste();
                            Clipboard.Clear();
                        }
                    }
                }
            }
        }
        #endregion

        #region 历史病人图片绑定
        private void treeListHistory_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            TreeListNode node = e.Node;
            if (e.Node.Tag is HistoryInpatient)
            {
                e.NodeImageIndex = 15;
            }
            else if (e.Node.Tag is EmrModelContainer)
            {
                e.NodeImageIndex = 2;
            }
            else if (e.Node.Tag is EmrModel)
            {
                e.NodeImageIndex = 0;
            }
        }
        #endregion

        #endregion

        #region 全角半角转换

        /// <summary>
        /// 切换成半角输入法
        /// </summary>
        private void ChangeHalf()
        {
            //IntPtr HIme = ImmGetContext(this.Handle);
            ////如果输入法处于打开状态  
            //if (ImmGetOpenStatus(HIme))
            //{
            //    int iMode = 0;
            //    int iSentence = 0;
            //    //检索输入法信息   
            //    bool bSuccess = ImmGetConversionStatus(HIme, ref   iMode, ref   iSentence);
            //    if (bSuccess)
            //    {
            //        //如果是全角,转换成半角  
            //        if ((iMode & IME_CMODE_FULLSHAPE) > 0)
            //            ImmSimulateHotKey(this.Handle, IME_CHOTKEY_SHAPE_TOGGLE);
            //    }

            //}
        }
        [DllImport("imm32.dll")]
        public static extern IntPtr ImmGetContext(IntPtr hwnd);
        [DllImport("imm32.dll")]
        public static extern bool ImmGetOpenStatus(IntPtr himc);
        [DllImport("imm32.dll")]
        public static extern bool ImmSetOpenStatus(IntPtr himc, bool b);
        [DllImport("imm32.dll")]
        public static extern bool ImmGetConversionStatus(IntPtr himc, ref   int lpdw, ref   int lpdw2);
        [DllImport("imm32.dll")]
        public static extern int ImmSimulateHotKey(IntPtr hwnd, int lngHotkey);
        private const int IME_CMODE_FULLSHAPE = 0x8;
        private const int IME_CHOTKEY_SHAPE_TOGGLE = 0x11;

        #endregion

        #region 开放三级检诊的开关

        /// <summary>
        /// 是否开放三级检诊功能
        /// </summary>
        /// <returns></returns>
        private bool IsOpenThreeLevelAudit()
        {
            try
            {
                string emrConfig = BasicSettings.GetStringConfig("EmrSetting");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(emrConfig);
                XmlNodeList nodeList = doc.GetElementsByTagName("OpenThreeLevelAudit");
                if (nodeList.Count > 0)
                {
                    XmlElement ele = nodeList[0] as XmlElement;
                    if (ele.InnerText.Trim() == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 刷新文书录入中的宏元素  此方法供外部反射调用，用于刷新宏元素的值
        /// </summary>
        public void RefreshMacroData()
        {
            MacroUtil.MacroSource = null;
        }

        #region 增加NavBarGroup的提示【支持鼠标拖拽】、【支持鼠标双击】
        //Add by wwj 2012-06-25 按照老板的要求，增加NavBarGroup的提示
        List<string> m_NavBarGroupListMouseDrag;
        List<string> m_NavBarGroupListMouseDoubleClick;
        private void ShowTipForNavBarGroup(NavBarGroup barGroup)
        {
            if (m_NavBarGroupListMouseDrag == null || m_NavBarGroupListMouseDoubleClick == null)
            {
                m_NavBarGroupListMouseDrag = new List<string>();
                m_NavBarGroupListMouseDrag.Add(navBarGroup_Image.Name);
                m_NavBarGroupListMouseDrag.Add(navBarGroup_Cy.Name);
                m_NavBarGroupListMouseDrag.Add(navBarGroup_TZ.Name);
                m_NavBarGroupListMouseDrag.Add(navBarGroup_Symb.Name);
                m_NavBarGroupListMouseDrag.Add(navBarGroup_ZZ.Name);
                m_NavBarGroupListMouseDrag.Add(navBarGroup_DBL.Name);

                m_NavBarGroupListMouseDoubleClick = new List<string>();
                m_NavBarGroupListMouseDoubleClick.Add(navBarGroup_Bl.Name);
            }

            ClearTipForNavBarGroup(barGroup);
            ClearTipForNavBarGroup(navBarGroup_Image);
            ClearTipForNavBarGroup(navBarGroup_Cy);
            ClearTipForNavBarGroup(navBarGroup_TZ);
            ClearTipForNavBarGroup(navBarGroup_Symb);
            ClearTipForNavBarGroup(navBarGroup_ZZ);
            ClearTipForNavBarGroup(navBarGroup_DBL);
            ClearTipForNavBarGroup(navBarGroup_Bl);

            if (m_NavBarGroupListMouseDrag.Contains(barGroup.Name))
            {
                barGroup.Caption += "【支持鼠标拖拽】";
            }
            else if (m_NavBarGroupListMouseDoubleClick.Contains(barGroup.Name))
            {
                barGroup.Caption += "【支持鼠标双击】";
            }
        }

        private void ClearTipForNavBarGroup(NavBarGroup barGroup)
        {
            if (barGroup.Caption.EndsWith("【支持鼠标拖拽】"))
            {
                barGroup.Caption = barGroup.Caption.Substring(0, barGroup.Caption.Length - "【支持鼠标拖拽】".Length);
            }
            if (barGroup.Caption.EndsWith("【支持鼠标双击】"))
            {
                barGroup.Caption = barGroup.Caption.Substring(0, barGroup.Caption.Length - "【支持鼠标双击】".Length);
            }
        }
        #endregion
    }
}
