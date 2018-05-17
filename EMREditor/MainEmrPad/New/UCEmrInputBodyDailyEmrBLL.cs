using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraTab;
using DrectSoft.Emr.Util;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Library.EmrEditor.Src.Document;
using DevExpress.XtraTreeList.Nodes;
using System.Data;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad.New
{
    /// <summary>
    /// 处理病程预览区与病程编辑区相关逻辑
    /// </summary>
    public partial class UCEmrInputBody
    {
        public Dictionary<string, UCEmrInputPreView> TempDailyPreViewCollection
        {
            get
            {
                return m_TempDailyPreViewCollection;
            }
        }
        /// <summary>
        /// 保存已经打开的病程预览控件的集合
        /// </summary>
        Dictionary<string, UCEmrInputPreView> m_TempDailyPreViewCollection = new Dictionary<string, UCEmrInputPreView>();

        /// <summary>
        /// 在Page中添加病程预览区
        /// </summary>
        /// <param name="emrModel">当前需要显示的病历EmrModel</param>
        /// <param name="page">当前的选中的XtraTabPage</param>
        private void AddDailyEmrPreView(EmrModel emrModel, XtraTabPage page)
        {
            try
            {
                if (page == null || emrModel == null) 
                { 
                    return; 
                }

                if (emrModel.ModelCatalog == ContainerCatalog.BingChengJiLu)
                {
                    UCEmrInputPreView inputPreView = null;
                    if (null != emrModel.DeptChangeID && m_TempDailyPreViewCollection.ContainsKey(emrModel.DeptChangeID))
                    {
                        //获取集合中的病程预览
                        inputPreView = m_TempDailyPreViewCollection[emrModel.DeptChangeID];
                        //inputPreView.Restore();
                    }
                    else
                    {
                        //新建的病程预览加入到集合中
                        inputPreView = new UCEmrInputPreView(emrModel.DeptChangeID, emrModel.InstanceId.ToString());
                        inputPreView.Height = 0;
                        m_TempDailyPreViewCollection.Add(emrModel.DeptChangeID, inputPreView);
                    }
                    if (page != null)
                    {
                        SplitterControl splitter = GetControlByType<SplitterControl>(page);
                        if (splitter == null)
                        {
                            CurrentInputTabPages.AddSplitterControl(page);
                        }
                        UCEmrInputPreView preView = GetControlByType<UCEmrInputPreView>(page);
                        if (preView == null)
                        {
                            CurrentInputTabPages.AddEmrInputPreView(inputPreView, page);
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
        /// 移除m_TempDailyPreViewCollection中对应的emrModel
        /// </summary>
        /// <param name="emrModel"></param>
        private void RemoveDailyPreViewCollection(EmrModel emrModel, XtraTabPage page)
        {
            if (emrModel != null && emrModel.ModelCatalog == ContainerCatalog.BingChengJiLu)
            {
                //获取m_TempModelPages中还存在同样科室病程的数目
                int count = m_TempModelPages.Where(pair =>
                {
                    if (pair.Key.ModelCatalog == ContainerCatalog.BingChengJiLu 
                        && pair.Key.DeptChangeID == emrModel.DeptChangeID)
                    {
                        return true;
                    }
                    return false;
                }).Count();
                if (count == 0)
                {
                    m_TempDailyPreViewCollection.Remove(emrModel.DeptChangeID);
                    var preView = m_TempDailyPreViewCollection.FirstOrDefault(p => p.Key == emrModel.DeptChangeID);
                    CurrentInputTabPages.RemoveEmrInputPreView(preView.Value, page);
                }
            }
        }

        /// <summary>
        /// 在XtraTabPage中获取指定类型的控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        private T GetControlByType<T>(XtraTabPage page) where T : Control
        {
            try
            {
                foreach (Control ctl in page.Controls)
                {
                    if (ctl is T)
                    {
                        return ctl as T;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 刷新病历预览区
        /// </summary>
        /// <param name="model">病历</param>
        /// <param name="editState">状态</param>
        public void RefreashEmrView(EmrModel model, EditState editState)
        {
            try
            {
                if (null == model || null == m_CurrentModel)
                {
                    return;
                }
                ///刷新预览区
                var preViewArray = m_TempDailyPreViewCollection.FirstOrDefault(p => !string.IsNullOrEmpty(m_CurrentModel.DeptChangeID) && p.Key == m_CurrentModel.DeptChangeID);
                if (null != preViewArray.Value && null != preViewArray.Value.PreViewInner)
                {
                    if (editState == EditState.Add || editState == EditState.Edit)
                    {///保存
                        preViewArray.Value.PreViewInner.AfterSaveEmr(m_CurrentModel.InstanceId.ToString(), m_CurrentModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else if (editState == EditState.Delete)
                    {///删除
                        preViewArray.Value.PreViewInner.AfterDeleteEmr(m_CurrentModel.InstanceId.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 关于编辑器
        /// <summary>
        /// 判断光标当前所在位置是否可以进行编辑操作
        /// </summary>
        /// <returns></returns>
        public bool CanEdit()
        {
            try
            {
                if (CurrentInputTabPages.CurrentForm == null)
                {
                    return false;
                }

                return CurrentInputTabPages.CurrentForm.CurrentEditorControl.CanEdit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑器获取焦点方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CurrentForm_GotFocus()
        {
            try
            {
                ///焦点所在TabPage
                XtraTabPage selectedPage = xtraTabControlEmr.SelectedTabPage;
                ///焦点所在TabPage对应的EmrModel
                EmrModel thisModel = m_TempModelPages.FirstOrDefault(p => p.Value == selectedPage).Key;
                if (null != thisModel)
                {///焦点所在TabPage为病历
                    if (null != treeListEmrNodeList.FocusedNode && null != treeListEmrNodeList.FocusedNode.Tag && treeListEmrNodeList.FocusedNode.Tag is EmrModel)
                    {
                        EmrModel focusedNodeModel = treeListEmrNodeList.FocusedNode.Tag as EmrModel;
                        if (thisModel == focusedNodeModel)
                        {
                            return;
                        }
                    }
                }
                ///焦点所在TabPage对应的EmrModelContainer
                EmrModelContainer thisContainer = m_TempContainerPages.FirstOrDefault(p => p.Value == selectedPage).Key;
                if (null != thisContainer)
                {///焦点所在TabPage为非病历
                    if (null != treeListEmrNodeList.FocusedNode && null != treeListEmrNodeList.FocusedNode.Tag && treeListEmrNodeList.FocusedNode.Tag is EmrModelContainer)
                    {
                        EmrModelContainer focusedNodeContainer = treeListEmrNodeList.FocusedNode.Tag as EmrModelContainer;
                        if (thisContainer == focusedNodeContainer)
                        {
                            return;
                        }
                    }
                }
                SelectedPageChanged(selectedPage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置当前编辑器为只读模式
        /// </summary>
        public void SetDocmentReadOnlyMode()
        {
            try
            {
                if (CurrentForm == null)
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Read;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置当前编辑器为可编辑模式
        /// </summary>
        public void SetDomentEditMode()
        {
            try
            {
                if (CurrentForm == null)
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置当前编辑器为设计模式
        /// </summary>
        public void SetDocmentDesginMode()
        {
            try
            {
                if (CurrentForm == null)
                {
                    return;
                }
                this.CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Design;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重置宏元素
        /// </summary>
        /// <param name="contentXml"></param>
        /// <returns></returns>
        public string ResetMacro(string contentXml)
        {
            try
            {
                if (null == contentXml || string.IsNullOrEmpty(contentXml.Trim()))
                {
                    return contentXml;
                }
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(contentXml);
                XmlNodeList list = doc.SelectNodes("//macro");
                if (null != list && list.Count > 0)
                {
                    foreach (XmlNode node in list)
                    {
                        if (null != node && node.LocalName == "macro" && null != node.Attributes["name"])
                        {
                            node.InnerText = node.Attributes["name"].Value;
                        }
                    }
                    return doc.OuterXml;
                }

                return contentXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 重新设置病程记录在历史病历中的节点 将“病程记录”节点下的病历重新分类
        /// </summary>
        private void ReSetDailyEmrNodeInHistory()
        {
            try
            {
                //foreach (TreeListNode inpatientNode in treeListEmrNodeList.Nodes)
                //{
                //    foreach (TreeListNode node in inpatientNode.Nodes)
                //    {
                //        EmrModelContainer container = node.Tag as EmrModelContainer;
                //        if (node.GetValue(0).ToString() == "病程记录" && node.Nodes.Count > 0)
                //        {
                //            if (node.Nodes.Count > 0)
                //            {
                //                EmrModel emrModel = node.Nodes[0].Tag as EmrModel;
                //                if (emrModel != null)
                //                {
                //                    DataTable dt = m_PatUtil.GetInpatientDeptChange(emrModel.InstanceId.ToString());
                //                    if (dt.Rows.Count > 0)
                //                    {
                //                        List<TreeListNode> childNodes = node.Nodes.Cast<TreeListNode>().ToList();
                //                        node.Nodes.Clear();
                //                        foreach (DataRow dr in dt.Rows)
                //                        {
                //                            TreeListNode subNode = treeListHistory.AppendNode(new object[] { dr["deptname"].ToString() + " " + dr["wardname"].ToString() }, node);
                //                            var dailyEmrNode = childNodes.Where(treeListNode =>
                //                                {
                //                                    EmrModel model = treeListNode.Tag as EmrModel;
                //                                    if (model != null && model.DeptChangeID == dr["id"].ToString())
                //                                    {
                //                                        return true;
                //                                    }
                //                                    return false;
                //                                });

                //                            dailyEmrNode.ToList().ForEach(treeListNode => { subNode.Nodes.Add(treeListNode); });
                //                        }
                //                    }
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
