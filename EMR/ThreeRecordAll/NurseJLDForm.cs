using DevExpress.XtraTreeList;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.CommonTableConfig;
using DrectSoft.Core.CommonTableConfig.CommonNoteUse;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.EMR.ThreeRecordAll
{
    /// <summary>	
    /// <title>护理记录单的批量录入</title>
    /// <auth>xuliangliang</auth>
    /// <date>2012-11-23</date>
    /// </summary>
    public partial class NurseJLDForm : DevBaseForm
    {
        IEmrHost m_app;
        string m_noofInpat;
        CommonNoteEntity m_commonNoteEntity;
        //InPatListForm inPatListForm;//原窗体被注销 xlb 
        InpatRecord inpatRecode;//新窗体 by xlb 2013-01-17
        DataRow m_drInpatient;
        bool hassave = false;//是否需要保存
        UCInCommonNote uCInCommonNote;

        public NurseJLDForm(IEmrHost app, CommonNoteEntity commonNoteEntity)
        {
            try
            {
                this.m_app = app;
                this.m_commonNoteEntity = commonNoteEntity;
                InitializeComponent();

                if (m_commonNoteEntity.CommonNote_TabList == null)
                {
                    CommonNoteBiz commonNoteBiz = new DrectSoft.Core.CommonTableConfig.CommonNoteBiz(m_app);
                    m_commonNoteEntity = commonNoteBiz.GetDetailCommonNote(m_commonNoteEntity.CommonNoteFlow);
                }

                //InitInpatient();//xlb
                InitPatient();
                InitTool();
                //SetDockPanelHidden();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }

        private void InitTool()
        {
            try
            {
                ToolForm toolForm = new ToolForm(m_app);
                toolForm.Dock = DockStyle.Fill;
                toolForm.TopLevel = false;
                toolForm.FormBorderStyle = FormBorderStyle.None;
                toolForm.Show();
                pcGJX.Controls.Clear();
                pcGJX.Controls.Add(toolForm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据配置项设置工具栏是否自动隐藏
        /// </summary>
        private void SetDockPanelHidden()
        {
            try
            {
                //dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
                if (DS_BaseService.IsShowThisMD("IsShowDockPanel", "Nurse"))
                {
                    dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                }
                else
                {
                    dockPanel2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 构造方法
        /// xlb 2013-01-28
        /// </summary>
        /// <param name="app"></param>
        public NurseJLDForm(IEmrHost app)
        {
            try
            {
                this.m_app = app;
                InitializeComponent();
                InitPatient();
                InitTool();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region 已注销 by xlb 2013-01-17
        //private void InitInpatient()
        //{
        //    try
        //    {
        //        inPatListForm = new InPatListForm(m_app);
        //        inPatListForm.gvInpatient.DoubleClick += new EventHandler(gvInpatient_DoubleClick);
        //        inPatListForm.gvInpatient.KeyUp += new KeyEventHandler(gvInpatient_KeyUp);
        //        inPatListForm.Dock = DockStyle.Fill;
        //        inPatListForm.TopLevel = false;
        //        inPatListForm.FormBorderStyle = FormBorderStyle.None;
        //        inPatListForm.Show();
        //        pcInpatient.Controls.Clear();
        //        pcInpatient.Controls.Add(inPatListForm);
        //        gvInpatient_DoubleClick(inPatListForm.gvInpatient, null);

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        #endregion

        /// <summary>
        /// 初始化病人列表信息窗体
        /// xlb 2013-01-17
        /// </summary>
        private void InitPatient()
        {
            try
            {
                inpatRecode = new InpatRecord(m_app);
                inpatRecode.Dock = DockStyle.Fill;
                inpatRecode.TopLevel = false;
                inpatRecode.FormBorderStyle = FormBorderStyle.None;
                inpatRecode.Show();
                pcInpatient.Controls.Clear();
                pcInpatient.Controls.Add(inpatRecode);
                inpatRecode.treeListInpat.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeListInpat_FocusedNodeChanged);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region 设置病人使用单据事件 已注销 by xlb 2013-01-19
        //void btnSet_Click(object sender, EventArgs e)
        //{

        //    try
        //    {
        //        InCommonNoteEnmtity MyinCommonNoteEnmtity = null;
        //        InCommonNoteBiz inCommonNoteBiz = new InCommonNoteBiz(m_app);
        //        List<InCommonNoteEnmtity> inCommonNoteListAll = inCommonNoteBiz.GetSimInCommonNote(this.m_noofInpat);
        //        List<InCommonNoteEnmtity> inCommonNoteList = inCommonNoteListAll.FindAll(a => a.CommonNoteFlow == m_commonNoteEntity.CommonNoteFlow) as List<InCommonNoteEnmtity>;

        //        InCommListForm inCommListForm = new InCommListForm(inCommonNoteList, m_app, m_noofInpat, m_commonNoteEntity);
        //        DialogResult diaResult = inCommListForm.ShowDialog();
        //        if (diaResult != DialogResult.OK)
        //        {
        //            return;
        //        }
        //        MyinCommonNoteEnmtity = inCommListForm.MyInCommonNoteEnmtity;
        //        AddUCInCommonNote(MyinCommonNoteEnmtity);
        //        lblJLDName.Text = m_drInpatient["NAME"].ToString() + "：" + MyinCommonNoteEnmtity.InCommonNoteName;
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MessageBox.Show(ex.StackTrace);
        //    }


        //}
        #endregion

        void gvInpatient_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            gvInpatient_DoubleClick(sender, null);
        }

        /// <summary>
        /// 树选择节点改变事件
        /// xlb 2013-01-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListInpat_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                InCommonNoteEnmtity inCommonNoteEntity = null;
                if (e.Node == null || !(e.Node.Tag is InCommonNoteEnmtity))
                {
                    //删除通用单据
                    lblJLDName.Text = "";
                    pcJLD.Controls.Clear();
                    return;
                }
                inCommonNoteEntity = e.Node.Tag as InCommonNoteEnmtity;
                if (pcJLD.Controls != null && pcJLD.Controls.Count != 0)
                {
                    uCInCommonNote = (pcJLD.Controls[0] as UCInCommonNote);
                    if (uCInCommonNote != null)
                    {
                        //判断是否存在未保存数据
                        bool HasSave = uCInCommonNote.HasInfoSave();
                        if (HasSave == true)
                        {
                            if (MessageBox.Show("是否需要保存数据？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                //不保存则不处理前节点的数据
                                AddUCInCommonNote(inCommonNoteEntity);
                                return;
                            }
                            //选择保存则执行该方法
                            uCInCommonNote.SaveAllDate();
                        }
                    }
                }
                AddUCInCommonNote(inCommonNoteEntity);
                //判断单据是否被删除，删除则删除该节点 xlb 2013-02-01
                if (uCInCommonNote.m_inCommonNote.InCommonNoteFlow == null)
                {
                    e.Node.TreeList.DeleteNode(e.Node);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void gvInpatient_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var gvInpatient = (sender as DevExpress.XtraGrid.Views.Grid.GridView);
                if (gvInpatient == null) { return; }
                m_drInpatient = gvInpatient.GetFocusedDataRow();
                if (m_drInpatient == null || string.IsNullOrEmpty(m_drInpatient["NOOFINPAT"].ToString())) return;


                //未保存数据提示
                if (pcJLD.Controls != null && pcJLD.Controls.Count != 0)
                {
                    UCInCommonNote uCInCommonNote = (pcJLD.Controls[0] as UCInCommonNote);
                    if (uCInCommonNote != null)
                    {
                        bool HasSave = uCInCommonNote.HasInfoSave();
                        if (HasSave == true)
                        {
                            DialogResult diaResult = DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("存在数据未保存，确定退出吗？", "提示", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo);
                            if (diaResult == DialogResult.No)
                            {
                                return;
                            }
                        }
                    }
                }

                m_noofInpat = m_drInpatient["NOOFINPAT"].ToString();
                InCommonNoteEnmtity MyinCommonNoteEnmtity = null;
                InCommonNoteBiz inCommonNoteBiz = new InCommonNoteBiz(m_app);
                List<InCommonNoteEnmtity> inCommonNoteListAll = inCommonNoteBiz.GetSimInCommonNote(this.m_noofInpat);
                List<InCommonNoteEnmtity> inCommonNoteList = inCommonNoteListAll.FindAll(a => a.CommonNoteFlow == m_commonNoteEntity.CommonNoteFlow) as List<InCommonNoteEnmtity>;
                if (inCommonNoteList != null && inCommonNoteList.Count >= 2)
                {
                    InCommListForm inCommListForm = new InCommListForm(inCommonNoteList, m_app, m_noofInpat, m_commonNoteEntity);
                    DialogResult diaResult = inCommListForm.ShowDialog();
                    if (diaResult == DialogResult.OK)
                    {
                        MyinCommonNoteEnmtity = inCommListForm.MyInCommonNoteEnmtity;
                        AddUCInCommonNote(MyinCommonNoteEnmtity);

                    }
                    else
                    {
                        return;
                    }
                }
                else if (inCommonNoteList == null || inCommonNoteList.Count == 0)
                {
                    MyinCommonNoteEnmtity = ConverBycommonNote(m_commonNoteEntity);
                    if (MyinCommonNoteEnmtity == null) return;
                    AddUCInCommonNote(MyinCommonNoteEnmtity);
                }
                else
                {
                    MyinCommonNoteEnmtity = inCommonNoteList[0];
                    AddUCInCommonNote(MyinCommonNoteEnmtity);
                }
                lblJLDName.Text = m_drInpatient["NAME"].ToString() + "：" + MyinCommonNoteEnmtity.InCommonNoteName;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }


        /// <summary>
        /// 将commonNoteEntity转成InCommonNoteEnmtity，并保存
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        /// <returns></returns>
        private InCommonNoteEnmtity ConverBycommonNote(CommonNoteEntity commonNoteEntity)
        {
            try
            {
                CommonNoteBiz commonNoteBiz = new DrectSoft.Core.CommonTableConfig.CommonNoteBiz(m_app);
                commonNoteEntity = commonNoteBiz.GetDetailCommonNote(commonNoteEntity.CommonNoteFlow);
                InCommonNoteEnmtity inCommonNote = InCommonNoteBiz.ConvertCommonToInCommon(commonNoteEntity);
                InCommonNoteBiz icombiz = new DrectSoft.Core.CommonTableConfig.CommonNoteUse.InCommonNoteBiz(m_app);
                DataTable inpatientDt = icombiz.GetInpatient(m_noofInpat);
                inCommonNote.CurrDepartID = inpatientDt.Rows[0]["OUTHOSDEPT"].ToString();
                inCommonNote.CurrDepartName = inpatientDt.Rows[0]["DEPARTNAME"].ToString();
                inCommonNote.CurrWardID = inpatientDt.Rows[0]["OUTHOSWARD"].ToString();
                inCommonNote.CurrWardName = inpatientDt.Rows[0]["WARDNAME"].ToString();
                inCommonNote.NoofInpatient = m_noofInpat;
                inCommonNote.InPatientName = inpatientDt.Rows[0]["NAME"].ToString();
                string message = "";
                bool saveResult = icombiz.SaveInCommomNoteAll(inCommonNote, ref message);
                if (saveResult)
                {
                    return inCommonNote;
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("创建单据失败");
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 创建护理单据控件 添加到界面
        /// edit by xlb 2013-01-29
        /// </summary>
        /// <param name="MyinCommonNoteEnmtity"></param>
        private void AddUCInCommonNote(InCommonNoteEnmtity MyinCommonNoteEnmtity)
        {
            try
            {
                if (MyinCommonNoteEnmtity == null)
                {
                    return;
                }
                CommonNoteBiz commonNoteBiz = new Core.CommonTableConfig.CommonNoteBiz(m_app);
                var commonnote = commonNoteBiz.GetDetailCommonNote(MyinCommonNoteEnmtity.CommonNoteFlow);
                int noofinpat = Convert.ToInt32(MyinCommonNoteEnmtity.NoofInpatient);
                DataTable dt = DS_SqlService.GetInpatientByID(noofinpat, 2);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("病人不存在");
                    return;
                }
                string deptID = dt.Rows[0]["outhosdept"].ToString();
                bool canEdit = true;
                if (deptID != MyinCommonNoteEnmtity.CurrDepartID)
                {
                    canEdit = false;
                }
                uCInCommonNote = new UCInCommonNote(m_app, commonnote, MyinCommonNoteEnmtity, canEdit);

                pcJLD.Controls.Clear();
                uCInCommonNote.Dock = DockStyle.Fill;
                pcJLD.Controls.Add(uCInCommonNote);
                string createTIme = DateUtil.getDateTime(MyinCommonNoteEnmtity.CreateDateTime, DateUtil.NORMAL_LONG);
                lblJLDName.Text = "病人：" + MyinCommonNoteEnmtity.InPatientName + " " + MyinCommonNoteEnmtity.InCommonNoteName;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void dockPanel1_Click(object sender, EventArgs e)
        {

        }

        private void NurseJLDForm_Load(object sender, EventArgs e)
        {
            SetDockPanelHidden();
        }



    }
}