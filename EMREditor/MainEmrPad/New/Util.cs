using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common.Eop;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.MainEmrPad.New
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 获取MainForm中的当前病人
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public static Inpatient GetCurrentInpatient(Control ctl)
        {
            try
            {
                UCEmrInput ucEmrInput = GetParentUserControl<UCEmrInput>(ctl);
                return ucEmrInput.CurrentInpatient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取MainForm中的IEmrHost
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public static IEmrHost GetEmrHost(Control ctl)
        {
            try
            {
                UCEmrInput ucEmrInput = GetParentUserControl<UCEmrInput>(ctl);
                return ucEmrInput.App;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取父控件中的指定用户控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public static T GetParentUserControl<T>(Control ctl)
            where T : UserControl
        {
            try
            {
                Control ctlParent = ctl.Parent;
                while (!(ctlParent is T))
                {
                    if (ctlParent != null)
                    {
                        ctlParent = ctlParent.Parent;
                    }
                    else
                    {
                        return null;
                    }
                }
                return ctlParent as T;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取treeList中首次病程的EmrModel
        /// </summary>
        /// <param name="treeList"></param>
        /// <returns></returns>
        public static EmrModel GetFirstDailyEmrModel(TreeList treeList)
        {
            try
            {
                GetFirstDailyEmrModelInner(treeList, null);
                return FirstDailyEmrModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static EmrModel FirstDailyEmrModel { get; set; }
        public static void GetFirstDailyEmrModelInner(TreeList treeList, TreeListNodes nodes)
        {
            try
            {
                if (nodes == null)
                {
                    FirstDailyEmrModel = null;
                    nodes = treeList.Nodes;
                }
                if (FirstDailyEmrModel != null)
                    return;
                foreach (TreeListNode subNode in nodes)
                {
                    EmrModel model = subNode.Tag as EmrModel;
                    if (model != null && model.FirstDailyEmrModel)
                    {
                        FirstDailyEmrModel = model;
                    }
                    GetFirstDailyEmrModelInner(treeList, subNode.Nodes);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取页面所有病历节点Model
        /// 注：只针对病程记录
        /// </summary>
        /// <returns></returns>
        public static List<EmrModel> GetAllEmrModels(TreeListNodes nodes, List<EmrModel> list)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return list;
                }
                if (null == list)
                {
                    list = new List<EmrModel>();
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null != node.Tag && node.Tag is EmrModel)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        if (modelevey.DailyEmrModel)
                        {
                            list.Add(modelevey);
                        }
                    }
                    else if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        list = GetAllEmrModels(node.Nodes, list);
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
        /// 处理病历内容，包括savelog和各个节点的修改人，删除人的属性
        /// </summary>
        /// <param name="emrContent"></param>
        /// <returns></returns>
        public static string ProcessEmrContent(string emrContent)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(emrContent);
                doc.GetElementsByTagName("savelog").Cast<XmlNode>().ToList().ForEach(node =>
                    {
                        //node.RemoveAll();
                        node.ParentNode.RemoveChild(node);
                    });

                //doc.ChildNodes.Cast<XmlNode>().ToList().ForEach(node =>
                //    {
                //        XmlElement ele = node as XmlElement;
                //        if (ele != null)
                //        {
                //            if (ele.HasAttribute("creator"))
                //            {
                //                ele.RemoveAttribute("creator");
                //            }
                //            else if (ele.HasAttribute("deleter"))
                //            {
                //                ele.RemoveAttribute("deleter");
                //            }
                //        }
                //    });

                ProcessEmrContentInner(doc);
                return doc.InnerXml;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void ProcessEmrContentInner(XmlNode node)
        {
            foreach (XmlNode subNode in node.ChildNodes)
            {
                XmlElement ele = subNode as XmlElement;
                if (ele != null)
                {
                    if (ele.HasAttribute("creator"))
                    {
                        ele.RemoveAttribute("creator");
                    }
                    else if (ele.HasAttribute("deleter"))
                    {
                        ele.RemoveAttribute("deleter");
                    }
                }

                ProcessEmrContentInner(subNode);
            }
        }
    }
}
