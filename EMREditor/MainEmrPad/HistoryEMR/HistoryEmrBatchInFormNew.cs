using DevExpress.XtraEditors.Controls;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.Core.MainEmrPad.New;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad.HistoryEMR
{
    /// <summary>
    /// 历史病历批量导入 Add by wwj 2013-04-03
    /// </summary>
    public partial class HistoryEmrBatchInFormNew : DevBaseForm
    {
        private Inpatient m_CurrentInpatient;

        private IEmrHost m_App;

        HistoryEMRBLLNew m_HistoryEmrBll;

        EditorForm m_CurrentEditorForm;

        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_CurrentUCEmrInput;

        //病历需要导入的科室和病区
        string m_DeptChangeID;
        //获取焦点的勾选项
        private CheckedListBoxItem m_FocusedCheckItem;

        public HistoryEmrBatchInFormNew(IEmrHost app, Inpatient inpatient, string deptChangeID, DrectSoft.Core.MainEmrPad.New.UCEmrInput ucEmrInput)
        {
            try
            {
                InitializeComponent();
                RegisterEvent();
                m_App = app;
                m_CurrentInpatient = inpatient;
                m_DeptChangeID = deptChangeID;
                m_CurrentUCEmrInput = ucEmrInput;
                InitDateEdit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryEmrBatchInForm_Load(object sender, EventArgs e)
        {
            try
            {
                dateEditBegin.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                dateEditEnd.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
                AddEditorForm();
                Search();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        private void InitDateEdit()
        {
            try
            {
                //默认抓取1年的数据
                dateEditBegin.EditValue = DateTime.Now.AddMonths(-24).ToString("yyyy-MM-dd");
                dateEditEnd.EditValue = DateTime.Now.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化历史病人记录
        /// </summary>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        private void InitDataHistory(string dateBegin, string dateEnd)
        {
            try
            {
                DateTime? dtBegin = null, dtEnd = null;
                if (null != dateBegin && !string.IsNullOrEmpty(dateBegin.Trim()))
                {
                    dtBegin = DateTime.Parse(dateBegin);
                }
                if (null != dateEnd && !string.IsNullOrEmpty(dateEnd.Trim()))
                {
                    dtEnd = DateTime.Parse(dateEnd);
                }
                m_HistoryEmrBll = new HistoryEMRBLLNew(m_App, m_CurrentInpatient);
                DataTable dt = DS_SqlService.GetHistoryInpatients((int)m_CurrentInpatient.NoOfFirstPage, 0, dtBegin, dtEnd);
                gridControlInpatientList.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        private void Search()
        {
            try
            {
                string beginDateTime = dateEditBegin.Text.ToString();
                string endDateTime = dateEditEnd.Text.ToString();
                InitDataHistory(beginDateTime, endDateTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckBeforeQuery())
                {
                    ClearForm();
                    Search();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckBeforeQuery()
        {
            DateTime beginDate = Convert.ToDateTime(dateEditBegin.EditValue);
            DateTime endDate = Convert.ToDateTime(dateEditEnd.EditValue);
            if ((endDate - beginDate).TotalSeconds < 0)
            {
                MyMessageBox.Show("起始日期不能大于终止日期", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                dateEditBegin.Focus();
                return false;
            }
            return true;
        }

        private void gridViewInpatientList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                InitDeptList();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitDeptList()
        {
            try
            {
                DataRow drv = gridViewInpatientList.GetDataRow(gridViewInpatientList.FocusedRowHandle);
                if (drv != null)
                {
                    UnRegisterEvent();
                    gridControlDeptList.DataSource = null;
                    gridViewDeptList.FocusedRowHandle = -1;
                    checkedListBoxControlEmrNode.Items.Clear();
                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.ClearContent();
                    RegisterEvent();

                    string noofinpat = drv["noofinpat"].ToString();
                    DataTable dt = m_HistoryEmrBll.GetDeptListByInpatient(noofinpat);
                    gridControlDeptList.DataSource = dt;
                    if (null == dt || dt.Rows.Count == 0)
                    {
                        InitEmrTreeNodes();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                    checkedListBoxControlEmrNode.Items.Clear();
                    foreach (DataRow dr in dt.Rows)
                    {
                        checkedListBoxControlEmrNode.Items.Add(new EmrNode(dr), false);
                    }
                }
                if (null != checkedListBoxControlEmrNode.Items && checkedListBoxControlEmrNode.Items.Count > 0)
                {
                    m_FocusedCheckItem = checkedListBoxControlEmrNode.Items[0];
                    SelectedIndexChanged(0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void checkedListBoxControlEmrNode_MouseMove(object sender, MouseEventArgs e)
        {
            int itemIndex = checkedListBoxControlEmrNode.IndexFromPoint(checkedListBoxControlEmrNode.PointToClient(Cursor.Position));
            if (itemIndex >= 0)
            {
                ListBoxItem item = checkedListBoxControlEmrNode.Items[itemIndex];
                if (item != null)
                {
                    string name = ((EmrNode)item.Value).Name;

                    if (toolTipControllerEmrNode.GetToolTip(checkedListBoxControlEmrNode) != name)
                    {
                        toolTipControllerEmrNode.SetToolTip(checkedListBoxControlEmrNode, name);
                        toolTipControllerEmrNode.ShowHint(name, new Point(Cursor.Position.X + 20, Cursor.Position.Y - 40));
                    }
                }
            }
            else
            {
                toolTipControllerEmrNode.HideHint();
                toolTipControllerEmrNode.SetToolTip(checkedListBoxControlEmrNode, "");
            }
        }

        private void checkedListBoxControlEmrNode_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != System.Windows.Forms.MouseButtons.Left)
                {
                    return;
                }
                Point point = checkedListBoxControlEmrNode.PointToClient(Cursor.Position);
                int itemIndex = checkedListBoxControlEmrNode.IndexFromPoint(point);
                if (itemIndex >= 0)
                {
                    CheckedListBoxItem item = (CheckedListBoxItem)checkedListBoxControlEmrNode.GetItem(itemIndex);
                    if (point.X >= 4 && point.X <= 16)
                    {
                        if (m_FocusedCheckItem != item)
                        {
                            if (item.CheckState == CheckState.Checked)
                            {
                                item.CheckState = CheckState.Unchecked;
                            }
                            else
                            {
                                item.CheckState = CheckState.Checked;
                            }
                        }
                        m_FocusedCheckItem = item;
                        return;
                    }

                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.ClearContent();
                    if (item != null)
                    {

                        EmrNode emrNode = item.Value as EmrNode;
                        if (emrNode != null)
                        {
                            if (emrNode.DataRowItem["content"].ToString() == "")
                            {
                                emrNode.DataRowItem["content"] = m_HistoryEmrBll.GetEmrContentByID(emrNode.ID);
                            }
                            string content = emrNode.DataRowItem["content"].ToString();
                            if (content != "")
                            {
                                try
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.PreserveWhitespace = true;
                                    doc.LoadXml(content);
                                    m_CurrentEditorForm.CurrentEditorControl.LoadXML(doc);
                                    InitEditor();
                                    m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Content.SelectStart = 0;
                                    m_CurrentEditorForm.CurrentEditorControl.ScrollViewtopToCurrentElement();
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    m_FocusedCheckItem = item;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void checkedListBoxControlEmrNode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectedIndexChanged(checkedListBoxControlEmrNode.SelectedIndex);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void SelectedIndexChanged(int selectedIndex)
        {
            try
            {
                if (selectedIndex < 0)
                {
                    return;
                }
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.ClearContent();
                EmrNode emrNode = checkedListBoxControlEmrNode.Items[selectedIndex].Value as EmrNode;
                if (emrNode != null)
                {
                    if (emrNode.DataRowItem["content"].ToString() == "")
                    {
                        emrNode.DataRowItem["content"] = m_HistoryEmrBll.GetEmrContentByID(emrNode.ID);
                    }
                    string content = emrNode.DataRowItem["content"].ToString();
                    if (content != "")
                    {
                        try
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.PreserveWhitespace = true;
                            doc.LoadXml(content);
                            m_CurrentEditorForm.CurrentEditorControl.LoadXML(doc);
                            InitEditor();
                            m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Content.SelectStart = 0;
                            m_CurrentEditorForm.CurrentEditorControl.ScrollViewtopToCurrentElement();
                        }
                        catch
                        {
                        }
                    }
                }
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
                m_CurrentEditorForm.Dock = DockStyle.Fill;
                groupControlEmrContent.Controls.Add(m_CurrentEditorForm);
                InitEditor();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitEditor()
        {
            try
            {
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.ShowHeader = false;
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.ShowFooter = false;
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.ShowFooterLine = false;
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.ShowHeaderLine = false;
                m_CurrentEditorForm.CurrentEditorControl.PageSpacing = 0;
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DrectSoft.Library.EmrEditor.Src.Document.DocumentModel.Read;
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.Info.ShowParagraphFlag = false;
                m_CurrentEditorForm.EditorRefresh();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ClearForm()
        {
            try
            {
                UnRegisterEvent();
                gridControlInpatientList.DataSource = null;
                gridViewInpatientList.FocusedRowHandle = -1;
                gridControlDeptList.DataSource = null;
                gridViewDeptList.FocusedRowHandle = -1;
                checkedListBoxControlEmrNode.Items.Clear();
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.ClearContent();
                RegisterEvent();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void checkEditCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkEditCheckAll.Checked)
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
                MyMessageBox.Show(1, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkedListBoxControlEmrNode.CheckedItems.Count == 0)
                {
                    MyMessageBox.Show("没有需要导入的病历，请勾选【病历列表】中的病历。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                else
                {
                    List<EmrNode> emrNodeList = new List<EmrNode>();
                    foreach (ListBoxItem item in checkedListBoxControlEmrNode.CheckedItems)
                    {
                        EmrNode emrNode = item.Value as EmrNode;
                        emrNodeList.Add(emrNode);
                    }
                    if (emrNodeList.Any(node =>
                        {
                            if (node.DataRowItem["sortid"].ToString() == "AC") return true;
                            return false;
                        }))
                    {
                        //检查在内存中或数据库中是否存在首次病程
                        bool isHaveFirstDailyEmr = CheckIsHaveFirstDailyEmr();

                        //判断是否选择首次病程
                        if (emrNodeList.Any(node =>
                        {
                            if (node.DataRowItem["sortid"].ToString() == "AC"
                                && node.DataRowItem["isfirstdaily"].ToString() == "1")
                            {
                                return true;
                            }
                            return false;
                        }))
                        {//有选择首次病程
                            if (isHaveFirstDailyEmr)
                            {
                                MyMessageBox.Show("已经存在首次病程，不能重复导入", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                                return;
                            }
                        }
                        else
                        {//没有选择首次病程
                            if (!isHaveFirstDailyEmr)
                            {
                                MyMessageBox.Show("导入病程时同时需要导入首次病程，请选择", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                                return;
                            }
                        }

                        int dayCnt = 0;
                        emrNodeList = emrNodeList.OrderBy(node => { return Convert.ToDateTime(node.DataRowItem["captiondatetime"]); }).ToList();
                        foreach (EmrNode node in emrNodeList)
                        {
                            if (node.DataRowItem["sortid"].ToString() == "AC")
                            {
                                node.DataRowItem["captiondatetime"] = System.DateTime.Now.AddDays(dayCnt).ToString("yyyy-MM-dd HH:mm:ss");
                                dayCnt++;
                            }
                        }

                        HistoryEmrTimeAndCaption timeAndCaption = new HistoryEmrTimeAndCaption(emrNodeList, m_CurrentUCEmrInput);
                        timeAndCaption.StartPosition = FormStartPosition.CenterScreen;
                        if (timeAndCaption.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            //保存选中的历史病历
                            m_HistoryEmrBll.HistoryEmrBatchIn(emrNodeList, m_DeptChangeID);
                            MyMessageBox.Show("历史病历导入成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                            this.Close();
                            RefreshEMRMainPad();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }

                    //保存选中的历史病历
                    m_HistoryEmrBll.HistoryEmrBatchIn(emrNodeList, m_DeptChangeID);
                    MyMessageBox.Show("历史病历导入成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                    this.Close();
                    RefreshEMRMainPad();
                    return;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void checkedListBoxControlEmrNode_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (e.State == CheckState.Checked)
                {
                    EmrNode emrNode = checkedListBoxControlEmrNode.Items[e.Index].Value as EmrNode;
                    if (emrNode != null)
                    {
                        if (emrNode.DataRowItem["isfirstdaily"].ToString() == "1")
                        {
                            EmrModel firstDailyEmrModelInMemory = Util.GetFirstDailyEmrModel(m_CurrentUCEmrInput.CurrentInputBody.CurrentTreeList);//获取内存中是否存在首次病程的节点
                            bool isHasFirstDailyEmrInDataBase = m_HistoryEmrBll.IsHasFirstDailyEmr(m_CurrentInpatient.NoOfFirstPage.ToString());//获取数据库中是否存在首次病程的节点
                            if (firstDailyEmrModelInMemory != null && isHasFirstDailyEmrInDataBase)
                            {
                                MyMessageBox.Show("该病人已经存在首次病程，不能重复导入。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                                checkedListBoxControlEmrNode.Items[e.Index].CheckState = CheckState.Unchecked;
                                return;
                            }
                        }
                    }
                    foreach (CheckedListBoxItem subItem in checkedListBoxControlEmrNode.Items)
                    {
                        if (subItem.CheckState == CheckState.Unchecked)
                        {
                            SetCheckAllStateUnEvent(false);
                            return;
                        }
                    }
                    SetCheckAllStateUnEvent(true);
                }
                else
                {
                    SetCheckAllStateUnEvent(false);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void SetCheckAllStateUnEvent(bool boo)
        {
            try
            {
                checkEditCheckAll.CheckedChanged -= new EventHandler(checkEditCheckAll_CheckedChanged);
                checkEditCheckAll.Checked = boo;
                checkEditCheckAll.CheckedChanged += new EventHandler(checkEditCheckAll_CheckedChanged);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检查在内存中或数据库中是否存在首次病程
        /// </summary>
        /// <returns></returns>
        private bool CheckIsHaveFirstDailyEmr()
        {
            //获取数据库中的首次病程
            string displayDateTime = m_HistoryEmrBll.GetFirstDailyEmrCaptionDateTime(m_CurrentUCEmrInput.CurrentInpatient.NoOfFirstPage.ToString());

            //获取内存中的首次病程
            EmrModel firstDailyEmrModel = Util.GetFirstDailyEmrModel(m_CurrentUCEmrInput.CurrentInputBody.CurrentTreeList);

            //表示首程不存在,如果需要导入日常病程，则需要和首程一起导入
            if (displayDateTime != "" || firstDailyEmrModel != null)
            {
                return true;
            }
            return false;
        }

        private void gridViewDeptList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                InitEmrTreeNodes();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitEmrTreeNodes()
        {
            try
            {
                DataRow drv = gridViewInpatientList.GetDataRow(gridViewInpatientList.FocusedRowHandle);
                m_CurrentEditorForm.CurrentEditorControl.EMRDoc.ClearContent();
                checkedListBoxControlEmrNode.Items.Clear();
                checkEditCheckAll.Checked = false;

                if (drv != null)
                {
                    string noofinpat = drv["noofinpat"].ToString();
                    if (gridViewDeptList.FocusedRowHandle >= 0)
                    {
                        string id = ((DataTable)gridControlDeptList.DataSource).Rows[gridViewDeptList.FocusedRowHandle]["id"].ToString();
                        DataTable dt = m_HistoryEmrBll.GetEmrListByNoofinpatAndDeptChangeID(noofinpat, id);
                        SetCheckBoxListSource(dt);
                    }
                    else
                    {
                        DataTable depts = gridControlDeptList.DataSource as DataTable;
                        if (null == depts || depts.Rows.Count == 0)
                        {
                            DataTable dt = m_HistoryEmrBll.GetEmrListByNoofinpatAndDeptChangeID(noofinpat, string.Empty);
                            var rows = dt.AsEnumerable().Where(p => p["SORTID"].ToString() != "AC");
                            SetCheckBoxListSource((null == rows || rows.Count() == 0) ? dt.Clone() : rows.CopyToDataTable());
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
        /// 重新加载整个文书录入界面
        /// </summary>
        private void RefreshEMRMainPad()
        {
            try
            {
                m_App.ChoosePatient(m_CurrentInpatient.NoOfFirstPage);
                m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 事件的注册和注销
        private void RegisterEvent()
        {
            UnRegisterEvent();
            this.checkEditCheckAll.CheckedChanged += new System.EventHandler(this.checkEditCheckAll_CheckedChanged);
            this.checkedListBoxControlEmrNode.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.checkedListBoxControlEmrNode_ItemCheck);
            //this.checkedListBoxControlEmrNode.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxControlEmrNode_SelectedIndexChanged);
            this.checkedListBoxControlEmrNode.MouseMove += new System.Windows.Forms.MouseEventHandler(this.checkedListBoxControlEmrNode_MouseMove);
            this.checkedListBoxControlEmrNode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedListBoxControlEmrNode_MouseDown);
            this.gridViewDeptList.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewDeptList_FocusedRowChanged);
            this.gridViewInpatientList.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewInpatientList_FocusedRowChanged);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
        }

        private void UnRegisterEvent()
        {
            this.checkEditCheckAll.CheckedChanged -= new System.EventHandler(this.checkEditCheckAll_CheckedChanged);
            this.checkedListBoxControlEmrNode.ItemCheck -= new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.checkedListBoxControlEmrNode_ItemCheck);
            //this.checkedListBoxControlEmrNode.SelectedIndexChanged -= new System.EventHandler(this.checkedListBoxControlEmrNode_SelectedIndexChanged);
            this.checkedListBoxControlEmrNode.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.checkedListBoxControlEmrNode_MouseMove);
            this.checkedListBoxControlEmrNode.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.checkedListBoxControlEmrNode_MouseDown);
            this.gridViewDeptList.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewDeptList_FocusedRowChanged);
            this.gridViewInpatientList.FocusedRowChanged -= new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewInpatientList_FocusedRowChanged);
            this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
            this.btnOk.Click -= new System.EventHandler(this.btnOk_Click);
        }
        #endregion

    }

    /// <summary>
    /// 复选框列表中的每一项
    /// </summary>
    //public class EmrNode
    //{
    //    public string Name { get; set; }
    //    public string ID { get; set; }
    //    public DataRow DataRowItem { get; set; }

    //    public EmrNode(DataRow dr)
    //    {
    //        Name = dr["name"].ToString();
    //        ID = dr["id"].ToString();
    //        DataRowItem = dr;
    //    }

    //    public override string ToString()
    //    {
    //        return Name;
    //    }
    //}
}