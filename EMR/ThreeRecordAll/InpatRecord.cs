using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.CommonTableConfig;
using DrectSoft.Core.CommonTableConfig.CommonNoteUse;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DrectSoft.EMR.ThreeRecordAll
{
    /// <summary>
    /// 病人列表和使用单据树图界面
    /// xlb 2013-01-01-17
    /// </summary>
    public partial class InpatRecord : DevBaseForm
    {
        IEmrHost m_app;
        InCommonNoteBiz m_inCommonNoteBiz;
        InCommonNoteEnmtity m_inCommonNote;
        List<InPatientSim> currentInpat;//病人集合用于查询
        #region 方法 xlb 2013-01-17

        /// <summary>
        /// 构造
        /// </summary>
        public InpatRecord()
        {
            try
            {
                InitializeComponent();
                DS_SqlHelper.CreateSqlHelper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 注销 xlb 2013-01-28
        ///// <summary>
        ///// 重载构造
        ///// </summary>
        ///// <param name="app"></param>
        //public InpatRecord(IEmrHost app, CommonNoteEntity commonNoteEntity): this()
        //{
        //    try
        //    {
        //        RegisterEvent();
        //        m_app = app;
        //        m_commonNoteEntity = commonNoteEntity;
        //        InitInpatRecord();
        //        SetTreeView();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        /// <summary>
        /// 重载构造
        /// </summary>
        /// <param name="app"></param>
        public InpatRecord(IEmrHost app)
            : this()
        {
            try
            {
                RegisterEvent();
                m_app = app;
                InitInpatRecord();
                SetTreeView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置树的效果
        /// xlb
        /// </summary>
        private void SetTreeView()
        {
            try
            {
                //去掉选中节点的虚框
                this.treeListInpat.OptionsView.ShowFocusedFrame = false;
                //选中节点的背景色
                this.treeListInpat.Appearance.FocusedCell.BackColor = System.Drawing.Color.SkyBlue;
                this.treeListInpat.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.SkyBlue;
                this.treeListInpat.Appearance.FocusedCell.Options.UseBackColor = true;
                this.treeListInpat.Appearance.FocusedCell.Options.UseTextOptions = true;
                this.treeListInpat.Appearance.FocusedCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化病人信息
        /// xlb 2013-01-17
        /// </summary>
        private void InitInpatRecord()
        {
            try
            {
                treeListInpat.ClearNodes();
                m_inCommonNoteBiz = new InCommonNoteBiz(m_app);
                string currentDept = m_app.User.CurrentDeptId == null ? "" : m_app.User.CurrentDeptId;
                string currentWard = m_app.User.CurrentWardId == null ? "" : m_app.User.CurrentWardId;
                currentInpat = m_inCommonNoteBiz.GetAllInpatient(currentDept, currentWard);
                MakeTreeOne(currentInpat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建病人列表一级节点
        /// xlb 2013-01-17
        /// </summary>
        private void MakeTreeOne(List<InPatientSim> inPatientList)
        {
            try
            {
                TreeListNode parentNode = null;//父节点
                if (inPatientList == null || inPatientList.Count <= 0)
                {
                    return;
                }
                foreach (var item in inPatientList)
                {
                    string patInf = "床号：" + item.OutBed + "|" + "住院号：" + item.PatId;
                    parentNode = treeListInpat.AppendNode(new object[] { item.Name, "Folder", patInf }, null);
                    parentNode.Tag = item;
                    List<InCommonNoteEnmtity> inCommonNoteList = m_inCommonNoteBiz.GetSimInCommonNote(item.NoofInpat.ToString());
                    MakeTreeATwo(inCommonNoteList, parentNode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建树二级节点
        /// 即病人使用单据
        /// xlb 2013-01-17
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="treeNode"></param>
        private void MakeTreeATwo(List<InCommonNoteEnmtity> inCommonNote, TreeListNode treeNode)
        {
            try
            {
                TreeListNode Node = null;
                if (inCommonNote == null || inCommonNote.Count <= 0)
                {
                    return;
                }
                foreach (var item in inCommonNote)
                {
                    string createTIme = DateUtil.getDateTime(item.CreateDateTime, DateUtil.NORMAL_LONG);
                    string commmonNote = "创建人：" + item.CreateDoctorName + "|" + "时间：" + createTIme;

                    Node = treeListInpat.AppendNode(new object[] { item.InCommonNoteName, "Leaf", commmonNote }, treeNode);
                    Node.Tag = item;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 建立事件的触发
        /// xlb 2013-01-17
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                //treeListInpat.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeListInpat_FocusedNodeChanged);
                treeListInpat.GetStateImage += new GetStateImageEventHandler(treeListInpat_GetStateImage);
                txtSearchForInpat.EditValueChanged += new EventHandler(txtSearchForInpat_EditValueChanged);
                treeListInpat.MouseUp += new MouseEventHandler(treeListInpat_MouseUp);
                barButtonItemAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItemAdd_ItemClick);
                barButtonItemDel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItemDel_ItemClick);
                //treeListInpat.AfterExpand+=new NodeEventHandler(treeListInpat_AfterExpand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将CommonNoeEntity->InCommonNoteEnmtity;
        /// xlb 2013-01-19
        /// </summary>
        /// <param name="commonNote"></param>
        /// <returns></returns>
        private InCommonNoteEnmtity ConvertToByCommonNote(CommonNoteEntity commonNote, InPatientSim inPatient)
        {
            try
            {
                //用来接收选择的模板的新模板名称
                string commonNoteNames = commonNote.CommonNoteName == null ? "" : commonNote.CommonNoteName;
                CommonNoteBiz commonNoteBiz = new CommonNoteBiz(m_app);
                commonNote = commonNoteBiz.GetDetailCommonNote(commonNote.CommonNoteFlow);
                commonNote.CommonNoteName = commonNoteNames;//模板名称修改
                InCommonNoteEnmtity inCommonNote = InCommonNoteBiz.ConvertCommonToInCommon(commonNote);
                InCommonNoteBiz incommonNoteBiz = new InCommonNoteBiz(m_app);
                DataTable dtPatient = incommonNoteBiz.GetInpatient(inPatient.NoofInpat.ToString());
                inCommonNote.CurrDepartID = dtPatient.Rows[0]["OUTHOSDEPT"].ToString();
                inCommonNote.CurrDepartName = dtPatient.Rows[0]["DEPARTNAME"].ToString();
                inCommonNote.CurrWardID = dtPatient.Rows[0]["OUTHOSWARD"].ToString();
                inCommonNote.CurrWardName = dtPatient.Rows[0]["WARDNAME"].ToString();
                inCommonNote.NoofInpatient = inPatient.NoofInpat.ToString();
                inCommonNote.InPatientName = dtPatient.Rows[0]["NAME"].ToString();
                inCommonNote.CreateDateTime = DateTime.Now.ToString();
                inCommonNote.Valide = "1";
                string message = "";
                bool saveResult = incommonNoteBiz.SaveInCommomNoteAll(inCommonNote, ref message);
                if (saveResult)
                {
                    return inCommonNote;
                }
                else
                {
                    MessageBox.Show("创建单据失败");
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 事件 xlb 2013-01-17

        /// <summary>
        /// 右击TreeList弹出右键菜单
        /// xlb 2013-01-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListInpat_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                barCheckItemDel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                if (e.Button == MouseButtons.Right)
                {
                    TreeListHitInfo hitInfo = treeListInpat.CalcHitInfo(e.Location);
                    if (hitInfo != null && hitInfo.Node != null)
                    {
                        treeListInpat.FocusedNode = hitInfo.Node;
                        TreeListNode node = hitInfo.Node;
                        if (node != null && node.Tag is InPatientSim)
                        {
                            //节点是父节点右键删除不可用
                            barButtonItemDel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            //父节点右键新增可使用
                            barButtonItemAdd.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                        else if (node != null && node.Tag is InCommonNoteEnmtity)
                        {
                            //子节点右键删除可用
                            barButtonItemDel.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                            //子节点右键新增隐藏
                            barButtonItemAdd.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        }
                        popupMenu1.ShowPopup(treeListInpat.PointToScreen(new Point(e.X, e.Y)));
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 已注销 xlb 2013-01-29
        ///// <summary>
        ///// 展开节点触发的事件
        ///// xlb 2013-01-29
        ///// 解决存在大量数据时一次加载引起的卡屏
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void treeListInpat_AfterExpand(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        //{
        //    try
        //    {
        //        //如果有子节点则展开
        //        if (e.Node.HasChildren)
        //        {
        //            e.Node.ExpandAll();
        //        }
        //        TreeListNode parentNode = e.Node;//以当前要展开的节点为父节点
        //        InPatientSim inpatient=new InPatientSim();
        //        if (e.Node.Tag is InPatientSim)
        //        {
        //            inpatient = e.Node.Tag as InPatientSim;
        //        }

        //        List<InCommonNoteEnmtity> inCommonNoteList = m_inCommonNoteBiz.GetSimInCommonNote(inpatient.NoofInpat.ToString());
        //        MakeTreeATwo(inCommonNoteList, parentNode);
        //    }
        //    catch (Exception ex)
        //    {
        //        MyMessageBox.Show(1, ex);
        //    }
        //}
        #endregion

        /// <summary>
        /// 右键新增事件
        /// xlb 2013-01-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                InPatientSim inpatient = null;
                TreeListNode node = treeListInpat.FocusedNode;//父节点
                TreeListNode node2 = null;//子节点
                if (node != null && node.Tag != null && node.Tag is InPatientSim)
                {
                    inpatient = node.Tag as InPatientSim;
                }

                CommonChooseForm commonChooseForm = new Core.CommonTableConfig.CommonNoteUse.CommonChooseForm("护理单据模板选择", m_app);
                //窗体运行起始位置居中
                commonChooseForm.StartPosition = FormStartPosition.CenterScreen;

                if (commonChooseForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                InCommonNoteEnmtity inCommonNote = ConvertToByCommonNote(commonChooseForm.SelectCommonNoteEntity, inpatient);
                if (inCommonNote == null)
                {
                    return;
                }

                InCommonNoteBiz icbiz = new InCommonNoteBiz(m_app);
                icbiz.GetDetaliInCommonNote(ref inCommonNote);
                string createTIme = DateUtil.getDateTime(inCommonNote.CreateDateTime, DateUtil.NORMAL_LONG);
                string commmonNote = "创建人：" + inCommonNote.CreateDoctorName + "|" + "时间：" + createTIme;
                node2 = treeListInpat.AppendNode(new object[] { inCommonNote.InCommonNoteName, "Leaf", commmonNote }, node);
                node2.Tag = inCommonNote;
                treeListInpat.FocusedNode = node2;//定位在新增子节点上
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 右键删除事件
        /// xlb 2013-01-19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TreeListNode treeList = treeListInpat.FocusedNode;
                if (treeList != null && treeList.Tag != null && treeList.Tag is InCommonNoteEnmtity)
                {
                    m_inCommonNote = treeList.Tag as InCommonNoteEnmtity;
                }
                else
                {
                    barCheckItemDel.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                if (MyMessageBox.Show("您确定删除吗？", "提示", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                if (m_inCommonNoteBiz == null)
                {
                    m_inCommonNoteBiz = new InCommonNoteBiz(m_app);
                }
                string message = "";
                //用来判断要删除的单据是否有记录，有则无法删除
                int count = m_inCommonNoteBiz.GetCommonItemCount(m_inCommonNote.InCommonNoteFlow);
                if (count != 0)
                {
                    //throw new Exception("已被使用无法删除");
                    MessageBox.Show("已被使用无法删除");
                    return;
                }
                bool delResult = m_inCommonNoteBiz.DelInCommonNote(m_inCommonNote, ref message);
                if (delResult)
                {
                    treeListInpat.DeleteNode(treeList);//删除节点
                    MessageBox.Show("删除单据成功");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 加载节点图片
        /// xlb 2013-01-17
        /// 设置节点状态图标需绑定图片集控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListInpat_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            try
            {
                TreeListNode node = e.Node;
                if (node.GetValue("MODERFIY") != null && node.GetValue("MODERFIY").ToString() == "Folder")
                {
                    if (node.Expanded)
                    {
                        e.NodeImageIndex = 17;
                    }
                    else
                    {
                        e.NodeImageIndex = 16;
                    }
                }
                else
                {
                    e.NodeImageIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查询条件改变触发的事件
        /// xlb 2013-01-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearchForInpat_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (currentInpat == null)
                {
                    currentInpat = new List<InPatientSim>();
                }
                //避免集合中存在为null项引起报错
                foreach (var item in currentInpat)
                {
                    if (item.OutBed == null)
                    {
                        item.OutBed = "";
                    }
                    if (item.Name == null)
                    {
                        item.Name = "";
                    }
                    if (item.PatId == null)
                    {
                        item.PatId = "";
                    }
                }
                string searchtext = txtSearchForInpat.Text.Trim();
                var inPat = (from patInfo in currentInpat
                             where patInfo.Name.Contains(searchtext)
                                || patInfo.OutBed.Contains(searchtext)
                                || patInfo.PatId.Contains(searchtext)
                             select patInfo).ToList();
                treeListInpat.ClearNodes();//清除节点
                MakeTreeOne(inPat);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}