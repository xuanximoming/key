//using ZYCommon;
using DrectSoft.Library.EmrEditor.Src.Actions;
using DrectSoft.Library.EmrEditor.Src.Document;

namespace ZYTextDocumentLib
{
    /// <summary>
    /// 用于处理文档元素新增或删除操作的动作
    /// </summary>
    /// <remarks>本动作对象比较特别特别</remarks>
    public class A_ContentChangeLog : ZYEditorAction
    {
        /// <summary>
        /// 对象是否可以进行记录
        /// </summary>
        internal bool CanLog = true;
        /// <summary>
        /// 本操作涉及到的元素的父元素
        /// </summary>
        internal ZYTextContainer Container = null;
        /// <summary>
        /// 新插入的元素在元素列表中的序号
        /// </summary>
        internal int index = 0;
        private System.Collections.ArrayList myUndoSteps = new System.Collections.ArrayList();
        /// <summary>
        /// 对象显示的名称
        /// </summary>
        public string strUndoName = null;

        /// <summary>
        /// 内部用于保存一个撤销操作信息的对象
        /// </summary>
        private class UndoStep
        {
            /// <summary>
            /// 元素在子元素列表中的序号
            /// </summary>
            internal int index;
            /// <summary>
            /// 涉及到的元素对象
            /// </summary>
            internal ZYTextElement Element;
            /// <summary>
            /// 涉及到的元素的数组
            /// </summary>
            internal System.Collections.ArrayList Elements;
            /// <summary>
            /// 涉及到的属性对象
            /// </summary>
            internal ZYAttribute Attribute;
            /// <summary>
            /// 涉及到的旧的数值
            /// </summary>
            internal object OldValue;
            /// <summary>
            /// 涉及到的新的数值
            /// </summary>
            internal object NewValue;
            /// <summary>
            /// 撤销操作信息类型
            /// </summary>
            internal int Style = 0;
        }

        /// <summary>
        /// 添加修改对象属性的记录
        /// </summary>
        /// <param name="vElement">该属性所属的元素</param>
        /// <param name="vAttribute">属性对象</param>
        /// <param name="NewValue">将要设置的新的属性值</param>
        public void LogAttribute(ZYTextElement vElement, ZYAttribute vAttribute, object NewValue)
        {
            if (CanLog)
            {
                UndoStep NewStep = new UndoStep();
                NewStep.Element = vElement;
                NewStep.Attribute = vAttribute;
                NewStep.OldValue = vAttribute.Value;
                NewStep.NewValue = NewValue;
                NewStep.Style = 4;
                myUndoSteps.Add(NewStep);
            }
        }

        /// <summary>
        /// 登记新增元素动作
        /// </summary>
        /// <param name="index">新增元素在元素列表中的序号</param>
        /// <param name="NewElement">新增的元素</param>
        public void LogInsert(int index, ZYTextElement NewElement)
        {
            if (CanLog)
            {
                UndoStep NewStep = new UndoStep();
                NewStep.index = index;
                NewStep.Element = NewElement;
                NewStep.Style = 0;
                myUndoSteps.Add(NewStep);
            }
        }
        /// <summary>
        /// 登记新增多个元素动作
        /// </summary>
        /// <param name="index">第一个新增元素在元素列表中的序号</param>
        /// <param name="myList">新增的元素集合</param>
        public void LogInsertRange(int index, System.Collections.ArrayList myList)
        {
            if (CanLog)
            {
                UndoStep NewStep = new UndoStep();
                NewStep.index = index;
                NewStep.Elements = myList;
                NewStep.Style = 1;
                myUndoSteps.Add(NewStep);
            }
        }

        /// <summary>
        /// 登记追加元素动作
        /// </summary>
        /// <param name="NewElement">追加的元素</param>
        public void LogAdd(ZYTextElement NewElement)
        {
            if (CanLog)
            {
                UndoStep NewStep = new UndoStep();
                NewStep.Style = 2;
                NewStep.Element = NewElement;
                myUndoSteps.Add(NewStep);
            }
        }
        /// <summary>
        /// 登记追加多个元素动作
        /// </summary>
        /// <param name="myList">追加的元素列表</param>
        public void LogAddRang(System.Collections.ArrayList myList)
        {
            if (CanLog)
            {
                UndoStep NewStep = new UndoStep();
                NewStep.Elements = myList;
                NewStep.Style = 5;
                myUndoSteps.Add(NewStep);
            }
        }
        /// <summary>
        /// 登记删除元素动作
        /// </summary>
        /// <param name="index">元素在元素列表中的序号</param>
        /// <param name="myElement">要删除的元素</param>
        public void LogRemove(int index, ZYTextElement myElement)
        {
            if (CanLog)
            {
                UndoStep NewStep = new UndoStep();
                NewStep.Style = 3;
                NewStep.index = index;
                NewStep.Element = myElement;
                myUndoSteps.Add(NewStep);
            }
        }

        public void LogRemoveRange(int index, System.Collections.ArrayList myList)
        {
            if (CanLog)
            {
                UndoStep NewStep = new UndoStep();
                NewStep.Elements = new System.Collections.ArrayList();
                NewStep.Elements.AddRange(myList);
                NewStep.index = index;
                NewStep.Style = 6;
                myUndoSteps.Add(NewStep);
            }
        }

        /// <summary>
        /// 已重载:当对象没有撤销信息时对象不可用
        /// </summary>
        /// <returns></returns>
        public override bool isEnable()
        {
            return myUndoSteps.Count > 0;
        }

        /// <summary>
        /// 已重载:执行重复操作
        /// </summary>
        /// <returns></returns>
        public override bool Redo()
        {
            bool bolRefreshElements = false;
            int NewIndex = -1;
            ZYTextElement NewStart = null;
            if (myUndoSteps.Count > 0)
            {
                foreach (UndoStep myStep in myUndoSteps)
                {
                    switch (myStep.Style)
                    {
                        case 0:
                            Container = myStep.Element.Parent;

                            bolRefreshElements = true;
                            Container.ChildElements.Insert(myStep.index, myStep.Element);
                            myStep.Element.OwnerDocument = Container.OwnerDocument;
                            myStep.Element.Parent = Container;
                            NewStart = myStep.Element;
                            break;
                        case 1:
                            Container = (myStep.Elements[0] as ZYTextElement).Parent;
                            bolRefreshElements = true;
                            Container.ChildElements.InsertRange(myStep.index, myStep.Elements);
                            if (myStep.Elements.Count > 0)
                                NewStart = (ZYTextElement)myStep.Elements[0];
                            break;
                        case 5:
                            Container = (myStep.Elements[0] as ZYTextElement).Parent;
                            bolRefreshElements = true;
                            Container.ChildElements.AddRange(myStep.Elements);
                            if (myStep.Elements.Count > 0)
                                NewStart = (ZYTextElement)myStep.Elements[0];
                            break;
                        case 6:
                            Container = (myStep.Elements[0] as ZYTextElement).Parent;
                            bolRefreshElements = true;
                            NewIndex = myStep.index;
                            Container.ChildElements.RemoveRange(myStep.index, myStep.Elements.Count);
                            break;
                        case 2:
                            Container = myStep.Element.Parent;
                            bolRefreshElements = true;
                            Container.ChildElements.Add(myStep.Element);
                            NewStart = myStep.Element;
                            break;
                        case 3:
                            Container = myStep.Element.Parent;
                            bolRefreshElements = true;
                            NewIndex = myStep.index;
                            Container.ChildElements.Remove(myStep.Element);
                            break;
                        case 4:

                            myStep.Attribute.SetValue(myStep.NewValue);
                            myStep.Element.RefreshSize();
                            NewStart = myStep.Element;
                            myStep.Element.UpdateAttrubute();
                            break;
                    }
                }
                if (bolRefreshElements)
                    OwnerDocument.RefreshElements();
                OwnerDocument.RefreshLine();
                OwnerDocument.UpdateView();

                //这句以前是被注释掉的，现在也放出来
                myOwnerDocument.Content.SelectLength = 0;


                if (NewStart != null)
                    myOwnerDocument.Content.CurrentElement = NewStart;
                else if (NewIndex >= 0)
                    myOwnerDocument.Content.MoveSelectStart(NewIndex);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 已重载:执行撤销操作
        /// </summary>
        /// <returns></returns>
        public override bool Undo()
        {
            ZYTextElement NewStart = null;
            int NewIndex = -1;
            bool bolRefreshElements = false;
            if (myUndoSteps.Count > 0)
            {
                for (int iCount = myUndoSteps.Count - 1; iCount >= 0; iCount--)
                {
                    UndoStep myStep = (UndoStep)myUndoSteps[iCount];

                    switch (myStep.Style)
                    {
                        case 0:
                            Container = myStep.Element.Parent;
                            bolRefreshElements = true;
                            NewIndex = myStep.Element.Index;
                            Container.ChildElements.Remove(myStep.Element);
                            break;

                        case 1:
                            bolRefreshElements = true;
                            foreach (ZYTextElement e in myStep.Elements)
                            {
                                Container = e.Parent;
                                NewIndex = e.Index;
                                Container.ChildElements.Remove(e);
                            }
                            break;
                        case 2:
                            Container = myStep.Element.Parent;
                            bolRefreshElements = true;
                            NewIndex = myStep.Element.Index;
                            Container.ChildElements.Remove(myStep.Element);
                            break;
                        case 3:
                            Container = myStep.Element.Parent;
                            bolRefreshElements = true;
                            //先从此处控制，索引小于0的不进行操作edit by ywk 2013年3月11日17:31:02 
                            if (myStep.index >= 0)
                            {
                                Container.ChildElements.Insert(myStep.index, myStep.Element);
                                NewStart = myStep.Element;
                            }
                            //Container.ChildElements.Insert(myStep.index, myStep.Element);
                            //NewStart = myStep.Element;
                            break;

                        case 4:

                            myStep.Attribute.SetValue(myStep.OldValue);
                            myStep.Element.UpdateAttrubute();
                            myStep.Element.RefreshSize();
                            NewStart = myStep.Element;
                            break;
                        case 5:

                            bolRefreshElements = true;
                            foreach (ZYTextElement e in myStep.Elements)
                            {
                                Container = e.Parent;
                                NewIndex = e.Index;
                                Container.ChildElements.Remove(e);
                            }
                            break;

                        case 6:
                            Container = (myStep.Elements[0] as ZYTextElement).Parent;
                            bolRefreshElements = true;
                            //索引需大于等于0才进入  add by ywk 2013年4月2日16:16:48 
                            if (myStep.index >= 0)
                            {
                                Container.ChildElements.InsertRange(myStep.index, myStep.Elements);
                                NewIndex = myStep.index;
                            }
                            break;

                    }
                }
                if (bolRefreshElements)
                    myOwnerDocument.RefreshElements();
                myOwnerDocument.RefreshLine();
                myOwnerDocument.UpdateView();

                #region  以前是被注释的，现在放出来
                myOwnerDocument.Content.SelectLength = 0;
                if (NewStart != null)
                    myOwnerDocument.Content.CurrentElement = NewStart;
                else if (NewIndex >= 0)
                    myOwnerDocument.Content.MoveSelectStart(NewIndex);
                #endregion



                return true;
            }
            return false;
        }

        /// <summary>
        /// 已重载:本对象可以撤销
        /// </summary>
        /// <returns></returns>
        public override bool isUndoable()
        {
            return true;
        }
        /// <summary>
        /// 已重载:对象名称为 strUndoName 值
        /// </summary>
        /// <returns></returns>
        public override string UndoName()
        {
            return strUndoName;
        }

        /// <summary>
        /// 已重载:将自己添加到文档对象的撤销对象列表中
        /// </summary>
        /// <returns></returns>
        public override ZYEditorAction Clone()
        {
            return this;
        }
    }
}