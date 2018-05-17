using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
///////////////////////序列化需要的引用
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    /// <summary>
    /// 自然语言电子病历文档内容管理器
    /// </summary>
    /// <remarks>本对象用于维护一个列表，该列表中全部为<link>DrectSoft.Library.EmrEditor.Src.Document.ZYTextElement</link>类型，
    /// 包括删除修改和插入元素，电子病历文本文档对象可以借助它来维护自己的可显示元素的列表
    /// 本元素还使用<a href="#ZYTextDocumentLib.ZYContent.SelectStart">SelectStart</a>属性和<a href="#ZYTextDocumentLib.ZYContent.SelectLength">SelectLength</a>属性
    /// 来管理文档中的插入点和选择区域，并提供一系列函数来辅助管理这两个属性
    /// 此外还提供了一些处理文档内容的通用例程
    /// 本对象使用<link>ZYTextDocumentLib.IEMRContentDocument</link>接口来调用文档对象本身
    /// </remarks>
    /// 
    [Serializable]
    public class ZYContent
    {
        private ZYTextDocument myDocument = null;
        /// <summary>
        /// 所有显示的元素集合,在ZYDocument的构造函数中初始化
        /// </summary>
        private System.Collections.ArrayList myElements = null;

        /// <summary>
        /// mfb 用来保留上一次鼠标所在的索引位置
        /// 如果是第一次打开文档,鼠标位置默认在第一个元素处,当然为0
        /// </summary>
        private int intSelectStart = 0;
        /// <summary>
        /// mfb 用来保留上一次鼠标选择的长度,或者叫步长
        /// 如果为0, 代表单击,并没有划选.
        /// </summary>
        private int intSelectLength = 0;

        private string strFixLenText = null;

        private bool bolModified = false;
        private bool bolAutoClearSelection = true;

        private int intLastXPos = -1;

        private bool bolLineEndFlag = false;


        public bool LineEndFlag
        {
            get { return bolLineEndFlag; }
        }
        /// <summary>
        /// 本内容所属的文档对象
        /// </summary>
        public ZYTextDocument Document
        {
            get { return myDocument; }
            set { myDocument = value; }
        }

        /// <summary>
        /// 是否自动清除选择状态,若为True则插入点位置修改时会自动设置SelectLength属性，否则会根据旧的插入点的位置计算SelectLength长度
        /// </summary>
        public bool AutoClearSelection
        {
            get { return bolAutoClearSelection; }
            set
            {
                bolAutoClearSelection = value;
            }
        }

        /// <summary>
        /// 所有显示的元素列表
        /// </summary>
        public System.Collections.ArrayList Elements
        {
            get { return myElements; }
            set
            {
                myElements = value;
                bolModified = false;
                strFixLenText = null;
                this.SetSelection(0, 0);
            }
        }

        public int IndexOf(ZYTextElement e)
        {
            return myElements.IndexOf(e);
        }

        /// <summary>
        /// 设置,返回文档内容是否改变
        /// </summary>
        public bool Modified
        {
            get { return bolModified; }
            set { bolModified = value; }
        }



        #region 获取元素函数群
        /// <summary>
        /// 获得当前插入点前最近的一个字符元素
        /// </summary>
        /// <returns>最近的字符元素，若没找到则返回空引用</returns>
        public ZYTextChar GetPreChar()
        {
            for (int iCount = (intSelectStart == 0 && myElements.Count > 1 ? 1 : intSelectStart - 1); iCount >= 0; iCount--)
                if (myElements[iCount] is ZYTextChar)
                {
                    return (ZYTextChar)myElements[iCount];
                }
            return null;
        }


        /// <summary>
        /// <para>正常模式</para>
        /// <para>获得光标左侧第N个元素，0为光标处元素（A）</para>
        /// <para>1 为 （3） 以此类推   …… 1 2 3 光标 A B C ……</para>
        /// <para>维文模式</para>
        /// <para>获取光标右侧第N个元素，0为光标处元素（A）</para>
        /// <para>1 为 （B） 以此类推   …… 1 2 3 光标 A B C ……</para>
        /// </summary>
        /// <returns>最近的字符元素，若没找到则返回空引用</returns>
        public ZYTextChar GetPreChar(int index)
        {
            int iCount = (intSelectStart == 0 && myElements.Count > 1 ? -index : intSelectStart - index);
            //小于索引值，返回非维文字符
            if (iCount < 0)
                return new ZYTextChar();
            if (myElements[iCount] is ZYTextChar)
            {
                return (ZYTextChar)myElements[iCount];
            }
            return new ZYTextChar();
        }
        /// <summary>
        /// <para>正常模式</para>
        /// <para>获得光标右侧第N个元素，0为光标处元素（A）</para>
        /// <para>1 为 （B） 以此类推   …… 1 2 3 光标 A B C ……</para>
        /// <para>维文模式</para>
        /// <para>获取光标左侧第N个元素，0为光标处元素（A）</para>
        /// <para>1 为 （3） 以此类推   …… 1 2 3 光标 A B C ……</para>
        /// </summary>
        /// <returns></returns>
        public ZYTextChar GetFontChar(int index)
        {
            int iCount = (intSelectStart == 0 && myElements.Count > 1 ? index : intSelectStart + index);
            //超出总字符个数，返回非维文字符
            if (iCount > myElements.Count - 1)
                return new ZYTextChar();
            if (myElements[iCount] is ZYTextChar)
            {
                return (ZYTextChar)myElements[iCount];
            }
            return new ZYTextChar();
        }
        /// <summary>
        /// 插入点的位置
        /// </summary>
        public int SelectStart
        {
            get { return intSelectStart; }
            set
            {
                if (bolAutoClearSelection)
                    this.SetSelection(value, 0);
                else
                    this.SetSelection(value, intSelectStart - value);
            }
        }

        /// <summary>
        /// 获得上一行
        /// </summary>
        public ZYTextLine PreLine
        {
            get
            {
                try
                {
                    ZYTextLine myLine = this.CurrentLine;
                    if (myDocument.Lines.IndexOf(myLine) > 0)
                    {
                        for (int iCount = intSelectStart - 1; iCount >= 0; iCount--)
                        {
                            ZYTextElement myElement = (ZYTextElement)myElements[iCount];
                            if (myElement.OwnerLine != myLine)
                                return myElement.OwnerLine;
                        }
                        return null;
                    }
                    else
                        return myLine;
                }
                catch { }
                return null;
            }
        }

        /// <summary>
        /// 获得下一行
        /// </summary>
        public ZYTextLine NextLine
        {
            get
            {
                try
                {
                    ZYTextLine myLine = this.CurrentLine;
                    if (myDocument.Lines.IndexOf(myLine) < myDocument.Lines.Count - 1)
                    {
                        for (int iCount = intSelectStart + 1; iCount < myElements.Count; iCount++)
                        {
                            ZYTextElement myElement = (ZYTextElement)myElements[iCount];
                            if (myElement.OwnerLine != myLine)
                                return myElement.OwnerLine;
                        }
                        return null;
                    }
                    else
                        return myLine;
                }
                catch { }
                return null;
            }
        }

        /// <summary>
        /// 获得当前行
        /// </summary>
        public ZYTextLine CurrentLine
        {
            get
            {
                if (myElements.Count == 0)
                    return null;
                else
                {
                    if (myElements != null && intSelectStart >= 0 && intSelectStart < myElements.Count)
                    {
                        ZYTextLine myLine = ((ZYTextElement)myElements[intSelectStart]).OwnerLine;
                        if (this.bolLineEndFlag && myDocument.Lines.IndexOf(myLine) > 0)
                            return (ZYTextLine)myDocument.Lines[myDocument.Lines.IndexOf(myLine) - 1];
                        else
                            return myLine;
                    }
                    else
                        return ((ZYTextElement)myElements[myElements.Count - 1]).OwnerLine;
                }
            }
        }// CurrenLine 

        /// <summary>
        /// 获得当前元素
        /// </summary>
        public ZYTextElement CurrentElement
        {
            get
            {
                ZYTextElement myElement = null;
                if (myElements.Count == 0)
                    return null;
                else
                {

                    if (myElements != null && intSelectStart >= 0 && intSelectStart < myElements.Count)
                    {
                        myElement = (ZYTextElement)myElements[intSelectStart];
                    }

                    else
                    {
                        myElement = (ZYTextElement)myElements[myElements.Count - 1];
                    }
                    return myElement;
                }
            }
            set
            {
                if (myElements.Contains(value))
                    this.MoveSelectStart(myElements.IndexOf(value));
                intSelectStart = this.FixIndex(intSelectStart);
                Debug.WriteLine("设置当前元素 " + value + " value.RealTop:" + value.RealTop);
            }
        }

        /// <summary>
        /// 正常模式
        /// 获得光标左侧第N个元素，0为光标处元素（A）
        /// 1 为 （3） 以此类推   …… 1 2 3 光标 A B C ……
        /// </summary>
        /// <returns>最近的元素，若没找到则返回空引用</returns>
        public ZYTextElement GetLeftElement(int index)
        {
            int iCount = (intSelectStart == 0 && myElements.Count > 1 ? -index : intSelectStart - index);
            //小于索引值，返回非维文字符
            if (iCount < 0)
                return new ZYTextElement();
            if (myElements[iCount] is ZYTextElement)
            {
                return (ZYTextElement)myElements[iCount];
            }
            return new ZYTextElement();
        }
        /// <summary>
        /// 正常模式
        /// 获得光标右侧第N个元素，0为光标处元素（A）
        /// 1 为 （B） 以此类推   …… 1 2 3 光标 A B C ……
        /// </summary>
        /// <returns></returns>
        public ZYTextElement GetRightElement(int index)
        {
            int iCount = (intSelectStart == 0 && myElements.Count > 1 ? index : intSelectStart + index);
            //超出总字符个数，返回非维文字符
            if (iCount > myElements.Count - 1)
                return new ZYTextElement();
            if (myElements[iCount] is ZYTextElement)
            {
                return (ZYTextElement)myElements[iCount];
            }
            return new ZYTextElement();
        }

        /// <summary>
        /// 获得当前选中的元素,若没有选中元素或选中多个元素则返回空
        /// </summary>
        public ZYTextElement CurrentSelectElement
        {
            get
            {
                if (myElements.Count == 0 || (intSelectLength != 1 && intSelectLength != -1))
                    return null;
                else
                    return (ZYTextElement)myElements[this.AbsSelectStart];
            }
            set
            {
                if (myElements.Contains(value))
                {
                    this.SetSelection(myElements.IndexOf(value) + 1, -1);
                }
            }
        }
        /// <summary>
        /// 获得当前位置的前一个元素
        /// </summary>
        public ZYTextElement PreElement
        {
            get
            {
                if (myElements != null && myElements.Count > 0 && intSelectStart > 0 && intSelectStart < myElements.Count)
                    return (ZYTextElement)myElements[intSelectStart - 1];
                else
                    return null;
            }
        }

        /// <summary>
        /// 获得指定元素的前一个元素
        /// </summary>
        /// <param name="refElement">指定的元素</param>
        /// <returns>该元素的前一个元素若没找到则返回空</returns>
        public ZYTextElement GetPreElement(ZYTextElement refElement)
        {
            int index = myElements.IndexOf(refElement);
            if (index >= 1)
                return (ZYTextElement)myElements[index - 1];
            else
                return null;
        }

        /// <summary>
        /// 获得指定元素的后一个元素
        /// </summary>
        /// <param name="refElement">指定的元素</param>
        /// <returns>该元素的前一个元素，若没有找到则返回空</returns>
        public ZYTextElement GetNextElement(ZYTextElement refElement)
        {
            int index = myElements.IndexOf(refElement);
            if (index >= 0 && index < myElements.Count - 1)
                return (ZYTextElement)myElements[index + 1];
            else
                return null;
        }

        protected int intUserLevel = 0;
        /// <summary>
        /// 当前用户等级
        /// </summary>
        public int UserLevel
        {
            get { return intUserLevel; }
            set { intUserLevel = value; }
        }
        public bool IsLock(int index)
        {
            if (index >= 0)
            {
                for (int iCount = index; iCount < myElements.Count; iCount++)
                {
                    if (myElements[iCount] is ZYTextLock)
                    {
                        ZYTextLock Lock = (ZYTextLock)myElements[iCount];
                        if (Lock.Level >= intUserLevel)
                            return true;
                    }


                }

                #region bwy : 固定文本不可删
                if (myElements[index] is ZYFixedText || myElements[index] is ZYText)
                {
                    return true;
                }
                #endregion bwy :
            }
            return false;
        }

        public bool IsLock(ZYTextElement element)
        {
            //			if( element is ZYTextFlag )
            //				return ! myDocument.Info.DesignMode ;
            //			if( element is ZYTextLock )
            //				return ! myDocument.Info.DesignMode ;
            //			int index = myElements.IndexOf( element );
            //			if( index >= 0 )
            //			{
            //				return IsLock( index );
            //			}
            return false;
        }

        /// <summary>
        /// 判断一个元素是否当前元素
        /// </summary>
        /// <param name="myElement"></param>
        /// <returns></returns>
        public bool isCurrentElement(ZYTextElement myElement)
        {
            return (this.CurrentElement == myElement && intSelectLength == 0);
        }

        /// <summary>
        /// 获得插入点所在行的所有的元素
        /// </summary>
        /// <returns>元素列表</returns>
        public System.Collections.ArrayList GetCurrentLineElements()
        {
            intSelectStart = this.FixIndex(intSelectStart);
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
            int LineIndex = myElement.LineIndex;
            // 获得当前行第一个元素的编号
            int StartIndex = 0;
            for (int iCount = intSelectStart - 1; iCount >= 0; iCount--)
            {
                myElement = (ZYTextElement)myElements[iCount];
                if (myElement.LineIndex != LineIndex)
                {
                    StartIndex = iCount + 1;
                    break;
                }
            }
            // 填充当前行元素列表
            System.Collections.ArrayList myList = new System.Collections.ArrayList();
            for (int iCount = StartIndex; iCount < myElements.Count; iCount++)
            {
                myElement = (ZYTextElement)myElements[iCount];
                if (myElement.LineIndex == LineIndex)
                {
                    myList.Add(myElement);
                }
                else
                    break;
            }
            return myList;
        }

        #endregion


        /// <summary>
        /// 返回当前行行首的空白字符串
        /// </summary>
        /// <returns>行首的空白字符串</returns>
        public string GetCurrentLineHeadBlank()
        {
            intSelectStart = this.FixIndex(intSelectStart);
            System.Collections.ArrayList myList = this.GetCurrentLineElements();
            System.Text.StringBuilder myStr = new System.Text.StringBuilder();
            for (int iCount = 0; iCount < myList.Count; iCount++)
            {
                ZYTextChar myChar = myList[iCount] as ZYTextChar;
                if (myChar == null)
                    break;
                else
                {
                    if (char.IsWhiteSpace(myChar.Char))
                        myStr.Append(myChar.Char);
                    else
                        break;
                }
            }
            return myStr.ToString();
        }

        #region ********************* 选择区域 *********************

        /// <summary>
        /// 选择区域的长度,可小于0
        /// </summary>
        public int SelectLength
        {
            get { return intSelectLength; }
            set { intSelectLength = value; }
        }
        /// <summary>
        /// 选择区域的绝对开始位置
        /// </summary>
        public int AbsSelectStart
        {
            get { return (intSelectLength > 0 ? intSelectStart : intSelectStart + intSelectLength); }
        }
        /// <summary>
        /// 选择区域的绝对结束位置
        /// </summary>
        public int AbsSelectEnd
        {
            get
            {
                int intValue;
                if (intSelectLength >= 0)
                    intValue = intSelectStart + intSelectLength;//- 1;
                else
                    intValue = intSelectStart;// -1;

                if (intValue >= myElements.Count - 1)
                    intValue = myElements.Count - 1;
                return intValue;
            }
        }

        /// <summary>
        /// 判断指定的元素是否处于选中区域
        /// </summary>
        /// <param name="myElement">文档元素对象</param>
        /// <returns>是否处于选中区域</returns>
        public bool isSelected(ZYTextElement myElement)
        {
            if (intSelectLength == 0 || myElement == null)
                return false;
            else
            {
                if (myElement is TPTextCell)
                {
                    return (myElement as TPTextCell).Selected;
                }
                int index = myElement.Index;// myElements.IndexOf( myElement);
                if (intSelectLength > 0 && index >= intSelectStart && index < intSelectStart + intSelectLength)
                    return true;
                if (intSelectLength < 0 && index >= intSelectStart + intSelectLength && index < intSelectStart)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 判断是否存在选择的项目
        /// </summary>
        /// <returns></returns>
        public bool HasSelected()
        {
            return (intSelectLength != 0);
        }

        /// <summary>
        /// 判断是否选择了文本
        /// </summary>
        /// <returns></returns>
        public bool HasSelectedText()
        {
            System.Collections.ArrayList myList = this.GetSelectElements();
            if (myList != null && myList.Count > 0)
            {
                foreach (ZYTextElement myElement in myList)
                {
                    if (myElement.isTextElement())
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得选中的文本
        /// </summary>
        /// <returns></returns>
        public string GetSelectedText()
        {
            System.Collections.ArrayList myList = this.GetSelectElements();
            if (myList != null && myList.Count > 0)
                return ZYTextElement.GetElementsText(myList);
            return null;
        }

        /// <summary>
        /// 判断是否选择了文本
        /// </summary>
        /// <returns></returns>
        public bool HasSelectedChar()
        {
            System.Collections.ArrayList myList = this.GetSelectElements();
            if (myList != null && myList.Count > 0)
            {
                foreach (object obj in myList)
                {
                    if (obj is ZYTextChar)
                        return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 将插入点尽量移动到指定位置
        /// </summary>
        /// <param name="x">指定位置的X坐标</param>
        /// <param name="y">指定位置的Y坐标</param>
        public void MoveTo(int x, int y)
        {
            //intLastXPos = -1;
            if (myDocument != null)
            {
                ZYTextLine CurrentLine = null;

                //mfb 2009-7-14 改了定位行的方法
                foreach (ZYTextLine myLine in myDocument.Lines)
                {
                    Rectangle lineRec = new Rectangle(myLine.RealLeft, myLine.RealTop, myLine.ContentWidth, myLine.FullHeight);

                    if (lineRec.Contains(new Point(x, y)))
                    {
                        CurrentLine = myLine;
                        break;
                    }
                }
                // 若没有找到当前行则设置最后一行为当前行
                if (CurrentLine == null && myDocument.Lines.Count > 0)
                {
                    CurrentLine = (ZYTextLine)myDocument.Lines[myDocument.Lines.Count - 1];
                }

                // 若最后还是没有找到当前行则函数处理失败，立即返回
                if (CurrentLine == null)
                    return;

                bool bolFlag = false;
                int index = 0;
                x -= CurrentLine.RealLeft;

                // 确定当前元素,当前元素是当前行中右边缘大于指定的Ｘ坐标的元素
                ZYTextElement CurrentElement = null;
                //当前行是一个空行,是一个空的P元素
                if (CurrentLine.Elements.Count == 0)
                {
                    CurrentElement = CurrentLine.Container;
                }
                foreach (ZYTextElement myElement in CurrentLine.Elements)
                {
                    if (WeiWenProcess.weiwen)
                    {
                        if (x > myElement.Left)
                        {
                            if (x < myElement.Left + myElement.Width / 2)
                                continue;
                            if (myElement.Parent.WholeElement)
                                return;
                            CurrentElement = myElement;
                            break;
                        }
                    }
                    else
                    {
                        if (x < myElement.Left + myElement.Width)
                        {
                            if (x > (myElement.Left + myElement.Width / 2))
                            {
                                continue;
                            }
                            if (myElement.Parent.WholeElement)
                                return;
                            CurrentElement = myElement;
                            break;
                        }

                    }
                }

                if (CurrentElement == null)
                {
                    // 若没找到当前元素则表示当前位置已向右超出当前行的范围
                    // 若当前行已换行符结尾则设置该换行符为当前元素
                    // 否则设置为当前行最后一个元素的下一个元素,并设置行尾标志
                    CurrentElement = CurrentLine.LastElement;
                    if (CurrentLine.HasLineEnd)
                    {
                        index = myElements.IndexOf(CurrentLine.LastElement);
                    }
                    else
                    {
                        index = myElements.IndexOf(CurrentLine.LastElement) + 1;
                        bolFlag = true;
                    }
                }
                else
                {
                    //找到正常的字符
                    index = myElements.IndexOf(CurrentElement);
                    bolFlag = false;
                }
                // 修正当前元素序号
                if (index > myElements.Count)
                {
                    index = myElements.Count - 1;
                    bolFlag = false;
                }
                if (index < 0)
                {
                    index = 0;
                    bolFlag = false;
                }
                this.MoveSelectStart(index);

                if (bolLineEndFlag != bolFlag)
                {
                    bolLineEndFlag = bolFlag;
                    ((ZYTextDocument)myDocument).UpdateTextCaret();
                }
            }
        }

        /// <summary>
        /// 设置选择区域大小
        /// </summary>
        /// <param name="NewSelectStart">新的选择区域开始的序号</param>
        /// <param name="NewSelectLength">新选择区域的长度</param>
        /// <returns>操作是否成功</returns>
        public bool SetSelection(int NewSelectStart, int NewSelectLength)
        {
            bolLineEndFlag = false;
            if (myElements == null || myElements.Count == 0)
            {
                return false;
            }

            int sourceIndex = 0; //原来的索引位置
            int sourceLength = 0; //原来的选择长度,未选择则为0

            int newIndex = 0; //最新的索引位置
            int newLength = 0; //最新的选择长度

            int intTemp = 0;

            NewSelectStart = FixIndex(NewSelectStart);
            int iStep = (NewSelectStart > intSelectStart ? 1 : -1);

            bool bolZeroSelection = (NewSelectLength == 0);

            // 若选择区域未发生改变则直接退出函数
            if (intSelectStart == NewSelectStart && intSelectLength == NewSelectLength)
                return true;
            int OldSelectStart = intSelectStart;

            //ZYTextElement OldElement = ( ZYTextElement  )myElements[OldSelectStart] ;

            // 如果没有选择多个元素,或者说未发生 "划划划" 选
            // 只是鼠标点了一下而已,没什么大惊小怪的.
            if (NewSelectLength == 0 && intSelectLength == 0)
            {
                //保存本次点击的索引位置
                intSelectStart = NewSelectStart;

                //如果超出范围,该咋办咋办.反正不是定位到第一个元素,就是定位到最后一个元素
                if (intSelectStart < 0)
                {
                    intSelectStart = 0;
                }
                if (intSelectStart >= myElements.Count)
                {
                    intSelectStart = myElements.Count - 1;
                }
                //获取点击处的元素
                ZYTextElement NewElement = (ZYTextElement)myElements[intSelectStart];

                if (myDocument != null)
                {
                    myDocument.SelectionChanged(OldSelectStart, 0, intSelectStart, 0);
                }
                if (OldSelectStart >= 0 && OldSelectStart < myElements.Count)
                {
                    ((ZYTextElement)myElements[OldSelectStart]).HandleLeave();
                }
                ((ZYTextElement)myElements[intSelectStart]).HandleEnter();
                return true;
            }
            if (intSelectLength > 0)
            {
                sourceIndex = intSelectStart;
                sourceLength = intSelectStart + intSelectLength;
            }
            else //难道还有小于0的情况?
            {
                sourceIndex = intSelectStart + intSelectLength;
                sourceLength = intSelectStart;
            }
            if (NewSelectLength > 0)
            {
                newIndex = NewSelectStart;
                newLength = NewSelectStart + NewSelectLength;
            }
            else
            {
                newIndex = NewSelectStart + NewSelectLength;
                newLength = NewSelectStart;
            }
            if (sourceIndex > newIndex)
            {
                intTemp = sourceIndex;
                sourceIndex = newIndex;
                newIndex = intTemp;
            }
            if (sourceLength > newLength)
            {
                intTemp = sourceLength;
                sourceLength = newLength;
                newLength = intTemp;
            }
            if (newIndex > sourceLength)
            {
                intTemp = newIndex;
                newIndex = sourceLength;
                sourceLength = intTemp;
            }
            intSelectStart = NewSelectStart;
            intSelectLength = NewSelectLength;


            FixSelection();


            sourceIndex = FixIndex(sourceIndex);
            sourceLength = FixIndex(sourceLength);
            newIndex = FixIndex(newIndex);
            newLength = FixIndex(newLength);
            if (sourceIndex != newIndex)
            {
                for (int iCount = sourceIndex; iCount <= newIndex; iCount++)
                    if (((ZYTextElement)myElements[iCount]).HandleSelectedChange())
                        return false;
            }
            if (sourceLength != newLength)
            {
                for (int iCount = sourceLength; iCount <= newLength; iCount++)
                    if (((ZYTextElement)myElements[iCount]).HandleSelectedChange())
                        return false;
            }

            if (myDocument != null)
            {
                myDocument.SelectionChanged(sourceIndex, sourceLength, newIndex, newLength);
            }
            if (OldSelectStart >= 0 && OldSelectStart < myElements.Count)
            {
                ((ZYTextElement)myElements[OldSelectStart]).HandleLeave();
            }
            ((ZYTextElement)myElements[intSelectStart]).HandleEnter();
            return true;
        }

        /// <summary>
        /// 修正元素序号以保证需要在元素列表的范围内
        /// </summary>
        /// <param name="index">原始的序号</param>
        /// <returns>修正后的序号</returns>
        private int FixIndex(int index)
        {
            if (index <= 0)
            {
                return 0;
            }
            if (index >= myElements.Count)
            {
                return myElements.Count - 1;
            }
            return index;
        }

        /// <summary>
        /// 选择所有的元素
        /// </summary>
        public void SelectAll()
        {
            this.SetSelection(0, myElements.Count);
        }

        #endregion

        /// <summary>
        /// 移动当前插入点的位置
        /// </summary>
        /// <param name="index">插入点的新的位置</param>
        /// <returns>操作是否成功</returns>
        public bool MoveSelectStart(int index)
        {
            index = this.FixIndex(index);
            int length = bolAutoClearSelection ? 0 : intSelectLength + intSelectStart - index;
            //Debug.WriteLine("zycontent MoveSelectStart 设置选择范围 " + index + "-" + length);
            return SetSelection(index, length);
        }

        /// <summary>
        /// 将插入点移动到指定元素前面
        /// </summary>
        /// <param name="refElement">指定的元素</param>
        /// <returns>操作是否成功</returns>
        public bool MoveSelectStart(ZYTextElement refElement)
        {
            if (myElements.IndexOf(refElement) >= 0)
            {
                return MoveSelectStart(myElements.IndexOf(refElement));
            }
            return false;
        }

        /// <summary>
        /// 获得所有选择区域的元素
        /// </summary>
        /// <returns>包含所有选中区域的元素的列表</returns>
        public System.Collections.ArrayList GetSelectElements()
        {
            if (myElements == null)
                return null;
            else
            {
                System.Collections.ArrayList myList = new System.Collections.ArrayList();
                int intEnd = this.AbsSelectEnd;
                for (int iCount = this.AbsSelectStart; iCount < intEnd; iCount++)
                    myList.Add(myElements[iCount]);
                return myList;
            }
        }

        /// <summary>
        /// 获得所有选择区域的段落元素mfb
        /// </summary>
        /// <returns>包含选择区域中所有段落元素的列表,若操作失败则返回空引用</returns>
        public System.Collections.ArrayList GetSelectParagraph()
        {
            if (myElements == null)
                return null;
            else
            {
                System.Collections.ArrayList myList = new System.Collections.ArrayList();
                int intEnd = this.AbsSelectEnd;
                for (int iCount = this.AbsSelectStart; iCount <= intEnd; iCount++)
                {
                    if ((myElements[iCount] as ZYTextElement).Parent is ZYTextParagraph)
                    {
                        if (!myList.Contains((myElements[iCount] as ZYTextElement).Parent))
                        {
                            myList.Add((myElements[iCount] as ZYTextElement).Parent);
                        }
                    }
                }
                return myList;
            }
        }

        /// <summary>
        /// 获得两个序号之间的所有元素
        /// </summary>
        /// <param name="Index1">序号1</param>
        /// <param name="Index2">序号2</param>
        /// <returns>保存序号在指定的两个序号之间的所有元素的列表对象</returns>
        public System.Collections.ArrayList GetElementsRange(int Index1, int Index2)
        {
            if (myElements == null)
                return null;
            else
            {
                System.Collections.ArrayList myList = new System.Collections.ArrayList();
                int Temp = 0;
                if (Index1 > Index2)
                {
                    Temp = Index1;
                    Index1 = Index2;
                    Index2 = Temp;
                }
                Index1 = this.FixIndex(Index1);
                Index2 = this.FixIndex(Index2);

                for (int iCount = Index1; iCount < Index2; iCount++)
                    myList.Add(myElements[iCount]);
                return myList;
            }
        }

        /// <summary>
        /// 若当前选中的一个元素则返回当前选择的元素,否则返回空引用
        /// </summary>
        /// <returns></returns>
        public ZYTextElement GetSelectElement()
        {
            if (intSelectLength == 1)
                return (ZYTextElement)myElements[intSelectStart];
            if (intSelectLength == -1)
                return (ZYTextElement)myElements[intSelectStart - 1];
            return null;
        }

        /// <summary>
        /// 获得选中区域的文本内容
        /// </summary>
        /// <returns></returns>
        public string GetSelectText()
        {
            return ZYTextElement.GetElementsText(this.GetSelectElements());
        }

        /// <summary>
        /// 使用指定文本替换选择区域
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public bool ReplaceSelection(string strText)
        {
            if (strText == null || strText.Length == 0)
                return false;
            this.DeleteSeleciton();
            this.InsertString(strText);
            bolModified = true;
            return true;
        }

        /// <summary>
        /// 插入一批元素
        /// </summary>
        /// <param name="myList"></param>
        public void InsertRangeElements(System.Collections.ArrayList myList)
        {
            if (myElements == null || myElements.Count == 0 || myList == null || myList.Count == 0)
                return;
            if (IsLock(intSelectStart))
                return;

            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
            ZYTextContainer myParent = myElement.Parent;
            //不能在ZYTextBlock插入子元素
            if (myParent is ZYTextBlock)
            {
                myElement = myParent;
                myParent = myParent.Parent;
            }

            // 向当前元素所在的父对象插入新增的字符元素
            #region bwy
            //要修改此处，如果遇到EOF要产生新的段落，方法未完
            ArrayList templist = new ArrayList();
            foreach (object o in myList)
            {
                if (o is ZYTextEOF)
                {
                    myParent.InsertRangeBefore(templist, myElement);
                    myParent.RefreshLine();

                    templist.Clear();
                    this.Document._InsertChar('\r');

                    myElement = (ZYTextElement)myElements[intSelectStart];
                    myParent = myElement.Parent;
                    //不能在ZYTextBlock插入子元素
                    if (myParent is ZYTextBlock)
                    {
                        myElement = myParent;
                        myParent = myParent.Parent;
                    }
                }
                else
                {
                    templist.Add(o);
                }
            }
            myParent.InsertRangeBefore(templist, myElement);
            myParent.RefreshLine();

            #endregion bwy
            //myParent.InsertRangeBefore(myList, myElement);//原
            //myParent.RefreshLine();
            bolModified = true;
            // 移动当前插入点到新增的字符串的末尾
            if (myDocument != null) myDocument.ContentChanged();
            this.AutoClearSelection = true;

            #region bwy : 将上面一句改为如下
            this.MoveSelectStart(myElement);
            #endregion bwy :
        }

        /// <summary>
        /// 在当前位置插入一个字符串
        /// </summary>
        /// <param name="strText">字符串</param>
        public void InsertString(string strText)
        {
            if (myElements == null || myElements.Count == 0) return;
            if (IsLock(intSelectStart))
                return;

            this.Document.BeginUpdate();
            this.Document.BeginContentChangeLog();

            ZYTextChar NewChar = null;
            ZYTextChar defChar = null;
            defChar = GetPreChar();

            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
            ZYTextContainer myParent = myElement.Parent;

            //Add by wwj 2013-01-23
            //解决在结构化元素前粘贴内容的时候，被粘贴的内容与结构化元素的内容融合为一体的Bug
            //如：{a,b,{d,e,f}} -------其中 a、b为自由文本， d、e、f是结构化元素的内容
            //这个时候往b和d的中间插入自由文本c，则插入后的效果应该是 {a,b,c,{d,e,f}} 而不是 {a,b,{c,d,e,f}}
            //注意：确保当前元素是其父元素下的第一个子元素时才进入此逻辑
            if (myParent != null && !(myParent is ZYTextParagraph) && myParent.Parent is ZYTextParagraph && myParent.ChildElements.IndexOf(myElement) == 0)
            {
                myElement = myParent;
                myParent = myParent.Parent;
            }

            // 根据字符串新增一系列字符元素对象
            System.Collections.ArrayList myList = new System.Collections.ArrayList();


            for (int iCount = 0; iCount < strText.Length; iCount++)
            {
                if (strText[iCount] == '\n')
                {
                    myList.Add(new ZYTextLineEnd());
                }
                else if (strText[iCount] != '\r')
                {
                    char myPreChar, myFontChar;
                    if (iCount == 0)
                        myPreChar = ' ';
                    else
                        myPreChar = strText[iCount - 1];

                    if (iCount == strText.Length - 1)
                        myFontChar = ' ';
                    else
                        myFontChar = strText[iCount + 1];

                    NewChar = myElement.OwnerDocument.CreateChar(WeiWenProcess.strPase(strText[iCount], myPreChar, myFontChar));
                    if (defChar != null)
                        defChar.Attributes.CopyTo(NewChar.Attributes);

                    //如果是在一个空段处，那么以默认字体大小为准 add by wwj 2012-05-29
                    if (myElement is ZYTextEOF && myElement == myParent.FirstElement)
                    {
                        NewChar.Attributes.SetValue(ZYTextConst.c_FontSize, myElement.OwnerDocument.OwnerControl.GetDefaultFontSize());
                    }
                    myList.Add(NewChar);
                }
            }

            //Add by wwj 2013-02-01 移除ArrayList末尾的ZYTextLineEnd元素,防止出现硬回车和软回车叠加的问题
            RemoveTheEndOFZYTextLineEnd(myList);

            // 向当前元素所在的父对象插入新增的字符元素
            myParent.InsertRangeBefore(myList, myElement);
            myParent.RefreshLine();
            bolModified = true;

            this.SelectLength = 0;
            this.Document.EndContentChangeLog();
            this.Document.EndUpdate();

            // 移动当前插入点到新增的字符串的末尾
            if (myDocument != null) myDocument.ContentChanged();
            this.AutoClearSelection = true;
            this.MoveSelectStart(intSelectStart + myList.Count);//Modify by wwj 2013-02-01 移动光标至正确的位置


        }

        /// <summary>
        /// 移除ArrayList末尾的ZYTextLineEnd元素 Add by wwj 2013-02-01
        /// </summary>
        /// <param name="list"></param>
        void RemoveTheEndOFZYTextLineEnd(ArrayList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[list.Count - 1] is ZYTextLineEnd)
                {
                    list.RemoveAt(list.Count - 1);
                }
                else
                {
                    return;
                }
            }
        }


        /// <summary>
        /// 在当前位置插入一个字符串
        /// </summary>
        /// <param name="vChar">字符</param>
        /// <returns>新增的字符对象</returns>
        public ZYTextChar InsertChar(char vChar)
        {
            if (myElements == null || myElements.Count == 0) return null;
            if (IsLock(intSelectStart))
            {
                return null;
            }

            if (intSelectStart < 0) intSelectStart = 0;
            if (intSelectStart >= myElements.Count) intSelectStart = myElements.Count - 1;

            ZYTextChar NewChar = null;
            // 首先试图找到向前最近的一个字符类型的数据
            ZYTextChar defChar = GetPreChar();
            ZYTextElement myElement = this.CurrentElement;
            ZYTextContainer myParent = myElement.Parent;
            ZYTextContainer grandParent = myElement.Parent.Parent;

            bool bolSetParent = false;
            if ((myParent is ZYTextBlock && myElement == myParent.GetFirstElement()) || myParent.WholeElement)
            {
                bolSetParent = true;
            }
            if (myElement.OwnerDocument.ContentChangeLog != null)
            {
                myElement.OwnerDocument.ContentChangeLog.CanLog = false;
            }
            NewChar = myElement.OwnerDocument.CreateChar(vChar);

            if (defChar != null)
            {
                defChar.Attributes.CopyTo(NewChar.Attributes);
                //不继承上下标
                NewChar.Attributes.SetValue(ZYTextConst.c_Sup, false);
                NewChar.Attributes.SetValue(ZYTextConst.c_Sub, false);
            }
            NewChar.CreatorIndex = this.Document.SaveLogs.CurrentIndex;
            #region bwy 如果是在一个空段处，且设置了回车符的字体大小，那么应该以这个为准 屏蔽
            //**************************Modified by wwj 2012-05-29**********************************
            //if (myElement is ZYTextEOF && myElement == myParent.FirstElement && (myElement as ZYTextEOF).FontSize != 0)
            //{
            //    NewChar.Attributes.SetValue(ZYTextConst.c_FontSize, (myElement as ZYTextEOF).FontSize);
            //}
            //**************************************************************************************

            //NewChar.UpdateAttrubute();
            #endregion


            #region 如果是在一个空段处，那么以默认字体大小为准
            if (myElement is ZYTextEOF && myElement == myParent.FirstElement)
            {
                NewChar.Attributes.SetValue(ZYTextConst.c_FontSize, myElement.OwnerDocument.OwnerControl.GetDefaultFontSize());
            }
            #endregion
            if (myElement.OwnerDocument.ContentChangeLog != null)
            {
                myElement.OwnerDocument.ContentChangeLog.CanLog = true;
            }

            if (bolSetParent)
            {
                grandParent.InsertBefore(NewChar, myParent);
            }
            else
            {
                myParent.InsertBefore(NewChar, myElement);
            }

            bolModified = true;

            if (myDocument != null)
            {
                myDocument.ContentChanged();
            }

            this.AutoClearSelection = true;
            // 移动当前插入点到新增的字符串的末尾 update by Ukey zhang 2017-11-05维文光标不用改变
            if (WeiWenProcess.weiwen)
                this.MoveSelectStart(intSelectStart + 1);
            else
            {
                this.MoveSelectStart(intSelectStart + 1);
            }


            return NewChar;
        }

        /// <summary>
        /// 在当前位置插入一个元素
        /// </summary>
        /// <param name="NewElement"></param>
        public void InsertElement(ZYTextElement NewElement)
        {
            if (myElements == null || myElements.Count == 0)
                return;
            if (this.IsLock(intSelectStart))
                return;
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
            ZYTextContainer myParent = myElement.Parent;

            #region bwy:
            //不能在ZYTextBlock插入子元素

            if (myParent is ZYTextBlock)
            {
                myElement = myParent;
                myParent = myParent.Parent;
            }
            #endregion bwy:


            if (myParent.InsertBefore(NewElement, myElement))
            {
                bolModified = true;
                if (myDocument != null)
                {
                    myDocument.ContentChanged();
                }
                this.AutoClearSelection = true;
                this.MoveSelectStart(intSelectStart + 1);
            }
        }

        /// <summary>
        /// 回车时插入一个段落
        /// </summary>
        /// <param name="NewElement">一个新的空段落元素</param>
        public void InsertParagraph(ZYTextElement NewElement)
        {
            if (myElements == null || myElements.Count == 0)
            {
                return;
            }
            bool isContentChange = false;

            //当前元素(这里为一个eof符)
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
            if (myElement.Parent is ZYTextBlock)
            {
                myElement = myElement.Parent;
            }
            //当前元素所属的段落对象

            ZYTextElement parent = myElement.Parent;
            while (!(parent is ZYTextParagraph))
            {
                parent = parent.Parent;
            }
            ZYTextParagraph Paraparent = parent as ZYTextParagraph;//myElement.Parent as ZYTextParagraph;

            //当前元素所属的容器的容器
            //ZYTextContainer divParent = myElement.Parent.Parent;
            ZYTextContainer divParent = Paraparent.Parent;

            if (myElement == Paraparent.LastElement)//当前元素为段落中最后一个元素
            {
                divParent.InsertAfter(NewElement, Paraparent);
                myElement = (NewElement as ZYTextParagraph).FirstElement;
                isContentChange = true;
            }
            else if (myElement == Paraparent.FirstElement)//当前元素为段落中第一个元素
            {
                divParent.InsertBefore(NewElement, Paraparent);
                myElement = (Paraparent as ZYTextParagraph).FirstElement;
                isContentChange = true;
            }
            else//当前元素为段落中的元素
            {
                int currentIndex = Paraparent.IndexOf(myElement);

                #region bwy
                ArrayList myList = new ArrayList();
                myList = Paraparent.ChildElements.GetRange(currentIndex, Paraparent.ChildElements.Count - currentIndex - 1);
                //copy 当前段中所有元素
                System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
                myDoc.PreserveWhitespace = true;
                myDoc.AppendChild(myDoc.CreateElement(ZYTextConst.c_ClipboardDataFormat));
                ZYTextElement.ElementsToXML(myList, myDoc.DocumentElement);

                //删除当段中当前元素后的元素

                Paraparent.RemoveChildRange(myList);

                ZYTextParagraph secondPara = new ZYTextParagraph();
                divParent.InsertAfter(secondPara, Paraparent);

                //还原copy并插入到上一段,past
                ArrayList newList = new ArrayList();

                this.Document.LoadElementsToList(myDoc.DocumentElement, newList);

                if (newList.Count > 0)
                {
                    foreach (ZYTextElement ele in newList)
                    {
                        ele.RefreshSize();
                    }
                    //this.InsertRangeElements(myList);
                    secondPara.InsertRangeBefore(newList, secondPara.LastElement);
                    myElement = newList[0] as ZYTextElement;

                }

                #endregion bwy

            }
            isContentChange = true;
            if (isContentChange)
            {
                bolModified = true;
                if (myDocument != null)
                {
                    myDocument.ContentChanged();
                }
            }
            this.AutoClearSelection = true;

            if (myElement is ZYTextBlock)
            {
                myElement = (myElement as ZYTextBlock).FirstElement;
            }

            //this.MoveSelectStart(intSelectStart + 1);
            this.MoveSelectStart(myElement);
        }

        #region ▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅ 表格相关方法 ▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅

        /// <summary>
        /// 在当前插入点处插入一个表格
        /// </summary>
        /// <param name="NewElement">表示一个表格元素</param>
        public void InsertTable(ZYTextElement NewElement)
        {
            if (myElements == null || myElements.Count == 0)
                return;
            if (this.IsLock(intSelectStart))
                return;
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];

            //获得当前元素所在的段落
            ZYTextContainer secondaryParent = GetParentByElement(myElement, ZYTextConst.c_P) as ZYTextContainer;

            ZYTextContainer rootParent = GetParentByElement(myElement, ZYTextConst.c_Div) as ZYTextDiv;

            //在元素所属的段落后插入一个表格.
            rootParent.InsertAfter(NewElement, secondaryParent);
            //然后再表格后再插入一个新的段落元素.
            rootParent.InsertAfter(secondaryParent, new ZYTextParagraph());

            bolModified = true;
            if (myDocument != null)
            {
                myDocument.ContentChanged();
            }
            this.AutoClearSelection = true;
        }

        /// <summary>
        /// mfb 在插入点处插入若干行
        /// </summary>
        /// <param name="RowNum">要插入的行数</param>
        /// <param name="IsAfter">是否在插入点后插入</param>
        public void InsertRows(int RowNum, bool IsAfter)
        {
            if (myElements == null || myElements.Count == 0)
                return;
            if (this.IsLock(intSelectStart))
                return;
            //获得当前元素
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];

            //获得当前表格
            TPTextTable currentTable = GetParentByElement(myElement, ZYTextConst.c_Table) as TPTextTable;
            if (currentTable != null)
            {
                //获得当前行
                TPTextRow currentRow = GetParentByElement(myElement, ZYTextConst.c_Row) as TPTextRow;


                //获得当前行所在表格的索引
                int rowIndex = currentTable.IndexOf(currentRow);

                //如果在当前行前插入
                for (int k = 0; k < RowNum; k++)
                {
                    if (!IsAfter)
                    {
                        currentTable.InsertRow(rowIndex, currentRow);
                    }
                    else
                    {
                        //在最后一行之后插入
                        if (rowIndex == currentTable.AllRows.Count - 1)
                        {
                            currentTable.AddRow(currentRow);
                        }
                        else
                        {
                            currentTable.InsertRow(rowIndex + 1, currentRow);
                        }
                    }
                }
                //重新设置表格内所有单元格的边框宽度
                currentTable.SetEveryCellBorderWidth();
            }
            bolModified = true;
            if (myDocument != null)
            {
                myDocument.RefreshSize();
                myDocument.ContentChanged();
            }
            this.AutoClearSelection = true;
        }

        /// <summary>
        /// mfb 在插入点处插入若干个列
        /// </summary>
        /// <param name="columnNum">要插入的列数</param>
        /// <param name="IsAfter">是否在插入点之后插入</param>
        public void InsertColumns(int columnNum, bool IsAfter)
        {
            if (myElements == null || myElements.Count == 0)
                return;
            if (this.IsLock(intSelectStart))
                return;

            //获得当前元素
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];

            //获得当前表格
            TPTextTable currentTable = GetParentByElement(myElement, ZYTextConst.c_Table) as TPTextTable;
            if (currentTable != null)
            {
                //获得当前行
                TPTextRow currentRow = GetParentByElement(myElement, ZYTextConst.c_Row) as TPTextRow;

                //获得当前单元格
                TPTextCell currentCell = GetParentByElement(myElement, ZYTextConst.c_Cell) as TPTextCell;

                //获得当前行所在表格的索引
                int rowIndex = currentTable.IndexOf(currentRow);

                //获得当前单元格所在行的索引
                int cellIndex = currentRow.IndexOf(currentCell);

                for (int k = 0; k < columnNum; k++)
                {
                    //如果在当前列前插入
                    if (!IsAfter)
                    {
                        currentTable.InsertColumns(cellIndex, currentCell);
                    }
                    else
                    {
                        //如果是最后一个单元格
                        if (cellIndex == currentRow.Cells.Count - 1)
                        {
                            currentTable.AddColumns(currentCell);
                        }
                        else
                        {
                            currentTable.InsertColumns(cellIndex + 1, currentCell);
                        }
                    }
                }
                //重新设置表格内所有单元格的边框宽度
                currentTable.SetEveryCellBorderWidth();
            }
            bolModified = true;
            if (myDocument != null)
            {
                myDocument.RefreshSize();
                myDocument.ContentChanged();
            }
            this.AutoClearSelection = true;

        }

        /// <summary>
        /// mfb 删除选择元素所在的表格
        /// </summary>
        public void DeleteTable()
        {
            if (myElements == null || myElements.Count == 0)
                return;
            if (this.IsLock(intSelectStart))
                return;
            //如果有有选择了多项内容,则判断这些内容是不是都在表格内的.
            //如果是则删除表格,如果不是则不进行动作
            if (HasSelected())
            {
                int StartIndex = this.AbsSelectStart;
                int EndIndex = this.AbsSelectEnd;
                int iLen = (intSelectLength > 0 ? intSelectLength : 0 - intSelectLength);

                System.Collections.ArrayList myList = this.GetSelectElements();

                //是否所有选择的元素都在table中
                bool isParentTable = true;

                foreach (ZYTextElement ele in myList)
                {
                    if (GetParentByElement(ele, ZYTextConst.c_Table) == null)
                    {
                        isParentTable = false;
                        break;
                    }
                }
                if (isParentTable)
                {
                    //获得选中元素列表的第一个元素
                    ZYTextElement myElement = (ZYTextElement)myElements[StartIndex];
                    ZYTextElement currentTable = GetParentByElement(myElement, ZYTextConst.c_Table);

                    ZYTextContainer bodyElement = GetRootElement(myElement) as ZYTextContainer;

                    bodyElement.RemoveChild(currentTable);
                }
            }
            else
            {
                //获得当前元素
                ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];

                //获得当前表格
                TPTextTable currentTable = GetParentByElement(myElement, ZYTextConst.c_Table) as TPTextTable;

                if (currentTable != null)
                {
                    ZYTextContainer bodyElement = GetRootElement(myElement) as ZYTextContainer;
                    bodyElement.RemoveChild(currentTable);
                }
            }

            bolModified = true;
            if (myDocument != null)
            {
                myDocument.RefreshSize();
                myDocument.ContentChanged();
            }
            this.AutoClearSelection = true;
        }

        /// <summary>
        /// mfb 删除选择元素所在的行
        /// </summary>
        public void DeleteRows()
        {
            if (myElements == null || myElements.Count == 0)
                return;
            if (this.IsLock(intSelectStart))
                return;

            //获得当前元素
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
            //获得当前表格
            TPTextTable currentTable = GetParentByElement(myElement, ZYTextConst.c_Table) as TPTextTable;
            if (currentTable != null)
            {
                //保存所有选择的元素所属的行列表
                ArrayList rowList = new ArrayList();
                foreach (TPTextRow row in currentTable)
                {
                    foreach (TPTextCell cell in row)
                    {
                        if (true == cell.CanAccess)
                        {
                            rowList.Add(row);
                            break;
                        }
                    }
                }

                //遍历所有选择的行,并删除之
                foreach (TPTextRow rowElement in rowList)
                {
                    currentTable.AllRows.Remove(rowElement);
                    currentTable.RemoveChild(rowElement);
                }

                currentTable.SetEveryCellBorderWidth();
            }

            bolModified = true;
            if (myDocument != null)
            {
                myDocument.RefreshSize();
                myDocument.ContentChanged();
            }
            this.AutoClearSelection = true;
        }

        /// <summary>
        /// mfb 删除选择元素所在的列
        /// </summary>
        public void DeleteColumns()
        {
            if (myElements == null || myElements.Count == 0)
                return;
            if (this.IsLock(intSelectStart))
                return;

            //获得当前元素
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
            //获得当前表格
            TPTextTable currentTable = GetParentByElement(myElement, ZYTextConst.c_Table) as TPTextTable;
            if (currentTable != null)
            {
                bool isFind = false;
                List<int> needDelCol = new List<int>();
                foreach (TPTextRow row in currentTable)
                {
                    foreach (TPTextCell cell in row)
                    {
                        if (true == cell.CanAccess)
                        {
                            isFind = true;
                            needDelCol.Add(row.IndexOf(cell));
                        }
                    }
                    if (isFind)
                    {
                        goto DeleteColumn;
                    }
                }
            DeleteColumn:
                foreach (int column in needDelCol)
                {
                    //TODO: 这里还需要斟酌一下.
                    //因为要删除的列都是紧挨着的,删除完第一个后. 后面的列的索引
                    //随之也会变化(自动减1),所以始终用第一个列号应该是正确的.
                    //功能虽然暂时达到,但是感觉不是回事,逻辑不严谨.
                    currentTable.DeleteColumn(needDelCol[0]);
                }
                currentTable.SetEveryCellBorderWidth();
            }
            bolModified = true;
            if (myDocument != null)
            {
                myDocument.RefreshSize();
                myDocument.ContentChanged();
            }
            this.AutoClearSelection = true;
        }

        /// <summary>
        /// mfb 判断当前元素(或容器)是否在一个cell中
        /// </summary>
        /// <returns></returns>
        public bool IsParaInCell(ZYTextElement CurrentElement)
        {
            if (CurrentElement.GetXMLName() == ZYTextConst.c_Div)
            {
                return false;
            }
            if (CurrentElement.Parent.GetXMLName() == ZYTextConst.c_Cell)
            {
                return true;
            }
            return IsParaInCell(CurrentElement.Parent);
        }

        /// <summary>
        /// mfb 获取当前元素所属的父级元素
        /// </summary>
        /// <param name="currentElement">当前元素</param>
        /// <param name="findName">要找的父级元素的xmlName</param>
        /// <returns></returns>
        public ZYTextElement GetParentByElement(ZYTextElement currentElement, string findName)
        {
            if (currentElement == null)
            {
                return null;
            }
            if (currentElement.GetXMLName() == findName)
            {
                return currentElement;
            }
            return GetParentByElement(currentElement.Parent, findName);
        }


        #endregion ▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅ end 表格相关插入方法 end ▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅


        /// <summary>
        ///  mfb 查找顶级body元素
        /// </summary>
        /// <param name="CurrentElement">The current element.</param>
        /// <returns></returns>
        public ZYTextElement GetRootElement(ZYTextElement CurrentElement)
        {
            if (CurrentElement.Parent != null)
            {
                return GetRootElement(CurrentElement.Parent);
            }
            return CurrentElement;
        }


        /// <summary>
        /// mfb 获得当前元素所属的最外层容器元素
        /// </summary>
        /// <param name="CurrentElement">The current element.</param>
        /// <returns></returns>
        public ZYTextElement GetSecondaryElement(ZYTextElement CurrentElement)
        {
            if (CurrentElement.Parent.Parent.GetXMLName() != "div")
            {
                return GetRootElement(CurrentElement.Parent);
            }
            return CurrentElement.Parent;
        }



        /// <summary>
        /// 插入签名
        /// </summary>
        /// <param name="NewElement"></param>
        public void InsertLock(ZYTextElement NewElement)
        {
            if (myElements == null || myElements.Count == 0)
                return;
            if (this.IsLock(intSelectStart))
                return;
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
            ZYTextContainer myParent = myElement.Parent;
            if (this.CurrentLine.Elements.Count > 1)
            {
                ZYTextParagraph myP = new ZYTextParagraph();
                myP.OwnerDocument = this.myDocument;
                this.InsertElement(myP);
            }
            //int i = 0;
            this.InsertString("                             ");
            if (myParent.InsertBefore(NewElement, myElement))
            {
                //myParent.RefreshLineFast( myParent.IndexOf( NewElement)); 
                bolModified = true;
                if (myDocument != null)
                {
                    myDocument.ContentChanged();
                }
                this.AutoClearSelection = true;
                //this.myDocument.SetAlign(ParagraphAlignConst.Right);

                this.MoveSelectStart(intSelectStart + 1);
            }
        }

        /// <summary>
        /// 删除选中区域的元素
        /// </summary>
        /// <param name="flag">删除标识，true 全部删除，false 固定文本不删除</param>
        public void DeleteSeleciton(bool flag)
        {
            this.Document.DeleteFlag = flag;
            DeleteSeleciton();
            this.Document.DeleteFlag = false;

        }
        /// <summary>
        /// 删除选中区域的元素  
        /// </summary>
        public bool DeleteSeleciton()//Modify by wwj 2013-02-01 增加返回值
        {
            //一定要基于Undo/redo 的几个方法才行
            if (myElements == null || myElements.Count == 0) return false;
            if (IsLock(intSelectStart))
                return false;

            ArrayList alp = this.GetSelectParagraph();

            //绝对的开始与结束位置，一定是前小后大

            int StartIndex = this.AbsSelectStart;
            int EndIndex = this.AbsSelectEnd;

            int iLen = (intSelectLength > 0 ? intSelectLength : 0 - intSelectLength);
            bool bolChanged = false;

            ///选择的字符列表
            System.Collections.ArrayList mySelList = this.GetSelectElements();
            //删除列表
            System.Collections.ArrayList myRemoveList = new System.Collections.ArrayList();
            //删除的真正元素列表
            System.Collections.ArrayList myRealRemoveList = new System.Collections.ArrayList();

            myRealRemoveList = this.Document.GetRealElements(mySelList);

            int intDelete = 0;
            foreach (ZYTextElement ele in myRealRemoveList)
            {
                intDelete = myDocument.isDeleteElement(ele);
            }
            if (intDelete == 0)
            {
                //myRealRemoveList = this.Document.GetRealElements(mySelList);
                if (!this.Document.DeleteFlag)
                {
                    if (this.Document.Info.DocumentModel != DocumentModel.Design)
                    {
                        foreach (ZYTextElement ele in mySelList)
                        {
                            if (ele.Parent is ZYFixedText || ele.Parent is ZYText)
                            {
                                MessageBox.Show("选择范围中包含固定内容，不能删除。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                            //Add By wwj 2012-03-30 判断对ZYButton的删除操作
                            if (ele is ZYButton && !((ZYButton)ele).CanDelete)
                            {
                                MessageBox.Show("选择范围中包含固定内容，不能删除。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                            //Add By wwj 2013-08-01 判断对ZYFlag的删除操作
                            if (ele is ZYFlag && !((ZYFlag)ele).CanDelete)
                            {
                                MessageBox.Show("选择范围中包含固定内容，不能删除。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }
                        }
                    }
                }

                #region 获取当前被选中的表格,被选中表格的条件是从表格头选到表格尾 Add by wwj 2013-01-21
                Dictionary<ZYTextElement, TPTextTable> selectionTableDict = new Dictionary<ZYTextElement, TPTextTable>();
                foreach (ZYTextElement ele in mySelList)
                {
                    ZYTextElement element = GetParentByElement(ele, ZYTextConst.c_Table);
                    if (element != null)
                    {
                        TPTextTable tb = element as TPTextTable;
                        if (tb != null && !selectionTableDict.ContainsValue(tb))
                        {
                            ZYTextElement lastElement = tb.GetLastElement();
                            if (mySelList.Contains(lastElement))
                            {
                                selectionTableDict.Add(ele, tb);
                            }
                        }
                    }
                }
                #endregion

                for (int i = 0; i < alp.Count; i++)
                {
                    ZYTextParagraph p = alp[i] as ZYTextParagraph;
                    int pIndexStart = p.FirstElement.Index;

                    if (p.FirstElement is ZYTextBlock)
                    {
                        pIndexStart = (p.FirstElement as ZYTextBlock).FirstElement.Index;
                    }

                    //p.LastElement 永远应是 EOF 
                    int pIndexEnd = p.LastElement.Index;

                    if (StartIndex <= pIndexStart && pIndexEnd <= EndIndex)
                    {
                        //如果要删除的段落在单元格内，则不用删除所有的段落,每个单元格中至少保留一个回车符，即至少保留一个段落 Add By wwj 2012-04-24 解决单元格内容删除的Bug
                        ZYTextElement textElement = this.GetParentByElement(p, ZYTextConst.c_Cell);
                        if (textElement != null)
                        {
                            if (((ZYTextContainer)textElement).ChildCount > 1)
                            {
                                //整段删除
                                p.Parent.RemoveChild(p);
                                p.Parent.RefreshLine();
                                StartIndex += p.ChildCount;
                            }
                            else
                            {
                                //删除选择部分元素
                                myRemoveList = Elements.GetRange(StartIndex, EndIndex - StartIndex);
                                //转换为真正元素
                                myRealRemoveList = this.Document.GetRealElements(myRemoveList);
                                p.RemoveChildRange(myRealRemoveList);
                                p.RefreshLine();
                            }
                        }
                        else
                        {
                            //Add by wwj 2013-02-01 增加段落删除前的判断
                            //由于在粘贴内容之前会清除选中的内容，如果当前为空行则不能删除此行
                            //增加此判断为了防止出现在粘贴内容后出现吃行的情况
                            if (StartIndex != EndIndex)
                            {
                                //整段删除
                                p.Parent.RemoveChild(p);
                                p.Parent.RefreshLine();
                            }
                        }
                    }
                    //选择在段中间
                    else if (pIndexStart <= StartIndex && EndIndex <= pIndexEnd)
                    {
                        //删除选择部分元素
                        myRemoveList = Elements.GetRange(StartIndex, EndIndex - StartIndex);
                        //转换为真正元素
                        myRealRemoveList = this.Document.GetRealElements(myRemoveList);
                        p.RemoveChildRange(myRealRemoveList);
                        p.RefreshLine();
                    }

                    //是第一个被选择段 
                    else if (StartIndex > pIndexStart)
                    {
                        //删除选择部分元素
                        myRemoveList = Elements.GetRange(StartIndex, p.LastElement.Index - StartIndex);
                        //转换为真正元素
                        myRealRemoveList = this.Document.GetRealElements(myRemoveList);
                        p.RemoveChildRange(myRealRemoveList);
                        p.RefreshLine();
                    }
                    //是最后一个被选择段
                    else if (pIndexEnd > EndIndex)
                    {
                        myRemoveList = Elements.GetRange(pIndexStart, EndIndex - pIndexStart);
                        myRealRemoveList = this.Document.GetRealElements(myRemoveList);
                        p.RemoveChildRange(myRealRemoveList);
                        p.RefreshLine();
                    }

                }

                //还原StartIndex变量的值 Add By wwj 2012-04-24
                StartIndex = this.AbsSelectStart;

                //如果文档中一个段都没有了，需要new一个新段，加入文档中，否则，无法输入
                if (this.Document.RootDocumentElement.ChildCount == 0)
                {
                    //MessageBox.Show("一个空段都没有了，还不快加一个");
                    ZYTextParagraph myP = new ZYTextParagraph();
                    myP.OwnerDocument = this.Document;
                    this.InsertParagraph(myP);
                }

                #region 删除选中的表格 Add by wwj 2013-01-21
                if (selectionTableDict.Count > 0)
                {
                    foreach (KeyValuePair<ZYTextElement, TPTextTable> pair in selectionTableDict)
                    {
                        ZYTextContainer bodyElement = GetRootElement(pair.Key) as ZYTextContainer;
                        bodyElement.RemoveChild(pair.Value);
                    }
                }
                #endregion
            }
            bolChanged = true;
            if (bolChanged)
            {
                bolModified = true;
                FixSelection();
                if (myDocument != null) myDocument.ContentChanged();
                this.SetSelection(StartIndex, 0);
                FixSelection();
            }
            #region bwy : 消除删除最末尾文本后产生的下划线或上划线，刷新这个区域。
            this.Document.OwnerControl.Invalidate();
            #endregion bwy :

            return true;
        }

        /// <summary>
        /// 检查选择区域数据,若数据错误在修复之
        /// </summary>
        private void FixSelection()
        {
            if (intSelectStart >= myElements.Count)
                intSelectStart = myElements.Count - 1;
            if (intSelectStart < 0)
                intSelectStart = 0;
            if (intSelectLength > 0 && (intSelectStart + intSelectLength > myElements.Count))
                intSelectLength = 0;
            if (intSelectLength < 0 && (intSelectStart + intSelectLength < 0))
                intSelectLength = 0;
        }

        /// <summary>
        /// 删除当前元素,函数返回0:确认删除元素 1:不删除该元素 2:对该元素进行逻辑删除
        /// </summary>
        /// <param name="flag">根据此参数决定删除固定元素时，是否需要进行提示</param>
        /// <returns>操作结果</returns>
        public int DeleteCurrentElement(params object[] flag)
        {
            if (myElements == null || myElements.Count == 0) return 1;
            if (IsLock(intSelectStart)) return 1;

            if (this.CheckSelectStart())
            {
                ZYTextElement myElement = this.CurrentElement;
                //如果是固定文本，删除
                //如果是固定文本，且在编辑状，不删除
                if ((myElement.Parent is ZYFixedText || myElement.Parent is ZYText) && this.Document.Info.DocumentModel != DocumentModel.Design)
                {
                    if (flag.Length > 0)
                    {
                        MessageBox.Show("删除范围中包含固定内容，不能删除。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return 1;
                }

                #region Add By wwj 2013-08-01 解决在不可删除的元素ZYButton、ZYFlag之前定位光标后，按下Delete键后元素被删除的问题
                if (myElement is ZYButton)
                {
                    if (!((ZYButton)myElement).CanDelete)
                    {
                        MessageBox.Show("删除范围中包含固定内容，不能删除。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return 1;
                    }
                }
                else if (myElement is ZYFlag)
                {
                    if (!((ZYFlag)myElement).CanDelete)
                    {
                        MessageBox.Show("删除范围中包含固定内容，不能删除。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return 1;
                    }
                }
                #endregion

                // 若当前元素不是最后一个元素则删除之
                if (myElement != myElements[myElements.Count - 1])
                {

                    ZYTextElement afterElement = (ZYTextElement)myElements[intSelectStart + 1];

                    ZYTextParagraph parentPara = myElement.Parent as ZYTextParagraph;
                    ZYTextContainer parentDiv = myElement.Parent.Parent;
                    #region bwy :
                    if (myElement.Parent is ZYTextBlock)
                    {
                        if (myElements.Count > intSelectStart + myElement.Parent.ChildCount)//Add by wwj 2013-05-07 解决在删除文档末尾的结构化元素出错的问题
                        {
                            afterElement = (ZYTextElement)myElements[intSelectStart + myElement.Parent.ChildCount];
                        }
                        parentPara = myElement.Parent.Parent as ZYTextParagraph;
                        parentDiv = myElement.Parent.Parent.Parent;
                    }
                    #endregion bwy :

                    int intDelete = myDocument.isDeleteElement(myElement);
                    if (intDelete == 0)
                    {
                        //bool isEndOfLine = false;

                        //如果光标所在处为一个空段落,则删除这个段落
                        if (myElement == parentPara.FirstElement && parentPara.ChildCount == 1 && parentPara.FirstElement is ZYTextEOF && !IsParaInCell(myElement))
                        {
                            //获取当前段落的上一个容器.
                            object tmpEle = parentDiv.ChildElements[parentDiv.ChildElements.IndexOf(parentPara) + 1];
                            //如果不是table,则正常处理,否则不执行任何动作.
                            if (!(tmpEle is TPTextTable))
                            {
                                myElement = (tmpEle as ZYTextParagraph).FirstElement;
                                parentDiv.RemoveChild(parentPara);
                                Document.RefreshElements();
                            }

                        }
                        //如果光标在段落的最后元素处,且段落不为空,则合并这个段落到上个段落中.
                        else if (myElement == parentPara.LastElement && parentPara.ChildCount > 1)
                        {
                            int currentParaIndex = parentDiv.IndexOf(parentPara);

                            //不是容器中的第一个元素
                            if (currentParaIndex < parentDiv.ChildCount - 1)
                            {
                                for (int i = 0; i < parentPara.ChildCount - 1; i++)
                                {
                                    #region 解决在段落的末尾按下Delete键时，出现结构化元素合并的情况 2013-05-17
                                    //Add by wwj 2013-05-17
                                    ZYTextElement paragraph = null;
                                    ZYTextElement afterElementNew = null;
                                    GetParagraph(afterElement, out paragraph, out afterElementNew);
                                    (parentPara.ChildElements[i] as ZYTextElement).Parent = (ZYTextParagraph)paragraph;
                                    ((ZYTextParagraph)paragraph).InsertBefore((parentPara.ChildElements[i] as ZYTextElement), afterElementNew);

                                    //Delete by wwj 2013-05-17
                                    //(parentPara.ChildElements[i] as ZYTextElement).Parent = afterElement.Parent;
                                    //afterElement.Parent.InsertBefore((parentPara.ChildElements[i] as ZYTextElement), afterElement); 
                                    #endregion

                                    //(afterElement.Parent.ChildElements[i] as ZYTextElement).Parent = parentPara;
                                    //parentPara.InsertBefore((afterElement.Parent.ChildElements[i] as ZYTextElement), parentPara.LastElement);
                                }
                                parentDiv.RemoveChild(parentPara);
                            }
                        }
                        else if (myElement != parentPara.LastElement && !(myElement is ZYTextContainer))
                        {
                            bool bolSetParent = false;
                            if (myElement.Parent.WholeElement)
                            {
                                bolSetParent = true;
                            }
                            if (myElement.Parent is ZYTextBlock && myElement == myElement.Parent.GetLastElement())
                            {
                                bolSetParent = true;
                            }
                            if (bolSetParent)
                            {
                                myElement = myElement.Parent;
                            }
                            parentPara.RemoveChild(myElement);
                        }

                        #region bwy :
                        if (myElement != parentPara.LastElement && myElement.Parent is ZYTextBlock)
                        {
                            parentPara.RemoveChild(myElement.Parent);
                        }
                        #endregion bwy :
                        bolModified = true;
                        myDocument.ContentChanged();

                        this.SetSelection(intSelectStart, 0);
                    }
                    else if (intDelete == 2)
                    {
                        this.SetSelection(intSelectStart + 1, 0);
                    }
                    return intDelete;
                }
            }
            return 1;
        }

        /// <summary>
        /// Add by wwj 2013-05-17
        /// 获取元素element所在的段落paragraph，以及元素element在段落paragraph中的元素textElement
        /// 当元素element为自由文本时，element == textElement
        /// 当元素element为结构化元素中的某个对象时，element != textElement
        /// </summary>
        /// <param name="element"></param>
        /// <param name="paragraph"></param>
        /// <param name="textElement"></param>
        public void GetParagraph(ZYTextElement element, out ZYTextElement paragraph, out ZYTextElement textElement)
        {
            paragraph = element.Parent;
            textElement = element;
            if (!(paragraph is ZYTextParagraph))
            {
                paragraph = element.Parent.Parent;
                textElement = element.Parent;
            }
        }

        /// <summary>
        /// 删除当前元素前一个元素,函数返回0:确认删除元素 1:不删除该元素 2:对该元素进行逻辑删除
        /// </summary>
        /// <returns>操作结果</returns>
        public int DeletePreElement(params object[] flag)
        {
            if (myElements == null || myElements.Count == 0)
            {
                return 1;
            }
            if (IsLock(intSelectStart - 1))
            {
                return 1;
            }
            //光标在整片文档的中间,即不再开始,也不再末尾.
            if (intSelectStart > 0 && intSelectStart < myElements.Count)
            {
                //判断是否光标在文档的结尾,判断这个是因为.
                //如果是最后一个元素,那么当删除这个元素时myElements.Count的值会变化.
                //随之intSelectStart也会跟着变,而其他情况不会这样.
                bool isLastElement = (intSelectStart == (myElements.Count - 1)) ? true : false;

                //光标后的那个元素
                ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
                //光标前的那个元素
                ZYTextElement preElement = (ZYTextElement)myElements[intSelectStart - 1];
                //如果是固定文本，且在编辑状，不删除
                if ((preElement.Parent is ZYFixedText || preElement.Parent is ZYText) && this.Document.Info.DocumentModel != DocumentModel.Design)
                {
                    if (flag.Length > 0)
                    {
                        MessageBox.Show("删除范围中包含固定内容，不能删除。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return 1;
                }
                //Add By wwj 2012-03-30 判断对ZYButton的删除操作
                if (preElement is ZYButton) if (!((ZYButton)preElement).CanDelete) return 1;

                //Add By wwj 2013-08-01 判断对ZYFlag的删除操作
                if (preElement is ZYFlag) if (!((ZYFlag)preElement).CanDelete) return 1;

                //当前元素的父元素
                ZYTextParagraph parentPara = null;
                //但前元素的父元素的父元素(在表格中为cell,在普通文档中为body)
                ZYTextContainer parentDiv = null;
                //如果当前元素是ZYTextBlock, 则将ZYTextBlock做为一个整体处理
                if (myElement.Parent is ZYTextBlock)
                {
                    myElement = myElement.Parent;
                    parentPara = myElement.Parent as ZYTextParagraph;
                    parentDiv = parentPara.Parent;
                }
                else
                {
                    parentPara = myElement.Parent as ZYTextParagraph;
                    parentDiv = myElement.Parent.Parent;
                }

                int intDelete = myDocument.isDeleteElement(preElement);
                int OldIndex = myElements.IndexOf(preElement);
                if (intDelete == 0)
                {
                    //如果是空行合并到上一行,则myElement等于preElement.
                    //如果是带内容的行合并到上一行, 则myElement要合并行的第一个元素,而preElement为合并后行的最后一个元素
                    #region 合并段落代码块

                    //如果光标所在处为一个空段落,且不在cell中.则删除这个段落
                    if (myElement == parentPara.FirstElement && parentPara.ChildCount == 1 && parentPara.FirstElement is ZYTextEOF && !IsParaInCell(myElement))
                    {
                        //获取当前段落的上一个容器.
                        object tmpEle = parentDiv.ChildElements[parentDiv.ChildElements.IndexOf(parentPara) - 1];
                        //如果不是table,则正常处理,否则不执行任何动作.
                        if (!(tmpEle is TPTextTable))
                        {
                            myElement = (tmpEle as ZYTextParagraph).LastElement;
                            parentDiv.RemoveChild(parentPara);
                            myDocument.RefreshElements();
                        }
                    }

                    //如果光标所在处为一个空段落,且在cell中.
                    //且当前段落不是cell的第一个元素
                    else if (myElement == parentPara.FirstElement && parentPara.ChildCount == 1 && parentPara.FirstElement is ZYTextEOF &&
                        IsParaInCell(myElement) && parentPara != parentDiv.FirstElement)
                    {
                        myElement = (parentDiv.ChildElements[parentDiv.ChildElements.IndexOf(parentPara) - 1] as ZYTextParagraph).LastElement;
                        parentDiv.RemoveChild(parentPara);
                        myDocument.RefreshElements();
                    }

                    //如果光标在段落的第一个元素处,且段落不为空,则合并这个段落到上个段落中.
                    else if (myElement == parentPara.FirstElement && parentPara.ChildCount > 1 && !(parentPara.FirstElement is ZYTextEOF))
                    {
                        int currentParaIndex = parentDiv.IndexOf(parentPara);

                        //Add by wwj 2012-05-29 【由于在表格的单元格中，光标位于最前面，按下删除键后，在下面一行中会报错，所以这里加上判断，避免报错】 todo
                        if (currentParaIndex - 1 < 0) return 1;

                        object tmpEle = parentDiv.ChildElements[currentParaIndex - 1];
                        //不是容器中的第一个元素
                        if (currentParaIndex > 0 && !(tmpEle is TPTextTable))
                        {
                            //创建一个内存区域
                            System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
                            myDoc.PreserveWhitespace = true;
                            myDoc.AppendChild(myDoc.CreateElement(ZYTextConst.c_ClipboardDataFormat));

                            //copy 当前段中所有元素到一个列表中，EOF除外
                            ArrayList myList = new ArrayList();
                            for (int i = 0; i < parentPara.ChildCount - 1; i++)
                            {
                                myList.Add(parentPara.ChildElements[i]);
                            }
                            //将列表中的所有元素以xml的形式存到内存中
                            ZYTextElement.ElementsToXML(this.Document.GetRealElements(myList), myDoc.DocumentElement);

                            //还原copy并插入到上一段,past
                            parentDiv.RemoveChild(parentPara);
                            myList.Clear();
                            this.Document.LoadElementsToList(myDoc.DocumentElement, myList);
                            if (myList.Count > 0)
                            {
                                foreach (ZYTextElement ele in myList)
                                {
                                    ele.RefreshSize();
                                }
                                preElement.Parent.InsertRangeBefore(myList, preElement.Parent.LastElement);
                                myDocument.RefreshElements();
                                myElement = preElement.Parent.ChildElements[preElement.Parent.ChildElements.Count - myList.Count - 1] as ZYTextElement;
                            }
                        }
                    }
                    #endregion


                    //如果当前元素不是当前段落的第一个元素, 且前一个元素不是容器
                    else if (myElement != parentPara.FirstElement && !(preElement is ZYTextContainer))
                    {
                        bool bolSetParent = false;
                        if (preElement.Parent.WholeElement)
                        {
                            bolSetParent = true;
                        }
                        if (preElement.Parent is ZYTextBlock && preElement == preElement.Parent.GetLastElement())
                        {
                            bolSetParent = true;
                        }
                        if (bolSetParent)
                        {
                            preElement = preElement.Parent;
                        }
                        parentPara.RemoveChild(preElement);

                        #region bwy
                        myDocument.RefreshElements();
                        parentPara.RefreshLine();
                        #endregion bwy
                    }

                    bolModified = true;

                    //如果光标在文档的结尾
                    if (isLastElement)
                    {
                        this.SetSelection(intSelectStart, 0);
                    }
                    else
                    {
                        //修正了元素在一段开头处退格时，光标位置不对的问题
                        if (myElement is ZYTextBlock)
                        {
                            this.MoveSelectStart((myElement as ZYTextBlock).FirstElement);
                        }
                        else
                        {
                            this.MoveSelectStart(myElement);
                        }
                    }

                    myDocument.ContentChanged();

                }
                else if (intDelete == 2)
                {
                    this.SetSelection(OldIndex, 0);
                }
                return intDelete;

            }
            return 1;
        }// void DeletePreElement


        /// <summary>
        /// 获得所有文本内容
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            return ZYTextElement.GetElementsText(myElements);
        }


        /// <summary>
        /// 获得插入点前第一个单词的起始位置
        /// </summary>
        /// <returns></returns>
        public int GetPreWordIndex()
        {
            //intSelectStart = this.FixIndex( intSelectStart );
            int index = -1;
            ZYTextLine myLine = this.CurrentLine;
            for (int iCount = intSelectStart - 1; iCount >= 0; iCount--)
            {
                if (myElements[iCount] is ZYTextChar)
                {
                    ZYTextChar myChar = (ZYTextChar)myElements[iCount];
                    if (char.IsLetter(myChar.Char) && myChar.OwnerLine == myLine)
                        index = iCount;
                    else
                        break;
                }
                else
                    break;
            }
            return index;
        }// int GetPreWordIndex()

        /// <summary>
        /// 获得插入点前第一个单词的起始位置
        /// </summary>
        /// <param name="myElement">指定的元素对象</param>
        /// <returns></returns>
        public int GetPreWordIndex(ZYTextElement myElement)
        {
            //intSelectStart = this.FixIndex( intSelectStart );
            int index = -1;
            if (myElement == null || myElements.Contains(myElement) == false)
                return -1;
            for (int iCount = myElements.IndexOf(myElement) - 1; iCount >= 0; iCount--)
            {
                if (myElements[iCount] is ZYTextChar)
                {
                    if (char.IsLetter((myElements[iCount] as ZYTextChar).Char))
                        index = iCount;
                    else
                        break;
                }
                else
                    break;
            }
            return index;
        }// int GetPreWordIndex()


        /// <summary>
        /// 获得插入点前的单词
        /// </summary>
        /// <returns>获得的单词，若不存在则返回空引用</returns>
        public string GetPreWord()
        {
            int index = this.GetPreWordIndex();
            System.Text.StringBuilder myStr = new System.Text.StringBuilder();
            ZYTextChar myChar = null;
            if (index >= 0)
            {
                for (int iCount = index; iCount < intSelectStart; iCount++)
                {
                    myChar = myElements[iCount] as ZYTextChar;
                    if (myChar != null)
                    {
                        if (char.IsLetter(myChar.Char))
                        {
                            myStr.Append(myChar.Char);
                        }
                        else
                            break;
                    }
                    else
                        break;
                }
            }
            if (myStr.Length == 0)
                return null;
            else
                return myStr.ToString();
        }// string GetPreWord()

        /// <summary>
        /// 获得指定元素前的单词
        /// </summary>
        /// <param name="myElement">指定的元素对象</param>
        /// <returns>获得的单词，若不存在则返回空引用</returns>
        public string GetPreWord(ZYTextElement myElement)
        {
            int index = this.GetPreWordIndex(myElement);
            System.Text.StringBuilder myStr = new System.Text.StringBuilder();
            ZYTextChar myChar = null;
            if (index >= 0)
            {
                for (int iCount = index; iCount < myElements.Count; iCount++)
                {
                    myChar = myElements[iCount] as ZYTextChar;
                    if (myChar != null)
                    {
                        if (char.IsLetter(myChar.Char))
                            myStr.Append(myChar.Char);
                        else
                            break;
                    }
                    else
                        break;
                }
            }
            if (myStr.Length == 0)
                return null;
            else
                return myStr.ToString();
        }// string GetPreWord()

        /// <summary>
        /// 获得指定范围内的元素的文本
        /// </summary>
        /// <param name="intStartIndex">选择区域的开始序号</param>
        /// <param name="intEndIndex">选择区域的结束序号</param>
        /// <returns>选择区域中所有的元素的文本</returns>
        public string GetRangeText(int intStartIndex, int intEndIndex)
        {
            intStartIndex = FixIndex(intStartIndex);
            intEndIndex = FixIndex(intEndIndex);
            System.Text.StringBuilder myStr = new System.Text.StringBuilder();
            for (int iCount = intStartIndex; iCount <= intEndIndex; iCount++)
            {
                myStr.Append(((ZYTextElement)myElements[iCount]).ToEMRString());
            }
            return myStr.ToString();
        }

        /// <summary>
        /// 内部使用的获得同元素列表长度相等的字符串文本
        /// </summary>
        /// <returns></returns>
        internal string GetFixLenText()
        {
            if (myElements == null)
                return null;
            else
            {
                if (bolModified == false && strFixLenText != null)
                    return strFixLenText;
                else
                {
                    char[] vChar = new char[myElements.Count];
                    ZYTextChar myChar = null;
                    for (int iCount = 0; iCount < myElements.Count; iCount++)
                    {
                        myChar = myElements[iCount] as ZYTextChar;
                        if (myChar == null)
                            vChar[iCount] = (char)1;
                        else
                            vChar[iCount] = myChar.Char;
                    }
                    strFixLenText = new string(vChar);
                    return strFixLenText;
                }
            }
        }



        #region 查找，替换函数群
        /// <summary>
        /// 查找字符串,若找到则设置选择区域为找到的字符串
        /// </summary>
        /// <param name="strText">需要查找的字符串</param>
        /// <returns>是否找到字符串</returns>
        public bool FindText(string strText)
        {
            if (strText != null && strText.Length != 0)
            {

                GetFixLenText();
                if (strFixLenText != null)
                {
                    int Index = strFixLenText.IndexOf(strText, this.SelectStart);
                    if (Index >= 0)
                    {
                        this.Document.Content.MoveSelectStart(Index);
                        this.Document.OwnerControl.ScrollViewtopToCurrentElement();
                        this.SetSelection(Index + strText.Length, 0 - strText.Length);
                    }
                    return (Index >= 0);
                }
            }
            return false;
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="strFind">需要查找的字符串</param>
        /// <param name="strReplace">需要替换的字符串</param>
        /// <returns>是否替换了字符串</returns>
        public bool ReplaceText(string strFind, string strReplace, out string msg)
        {
            msg = "";
            if (this.GetSelectText() == strFind)
            {
                //如果要替换的内容在元素中，则不能替换
                bool flag = true;
                foreach (ZYTextElement ele in this.GetSelectElements())
                {
                    if (ele.Parent is ZYTextBlock || ele is ZYElement)
                    {
                        msg = "只能替换纯文本，不能替换元素内部文本。";
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    this.ReplaceSelection(strReplace);
                }
            }
            return true;
        }

        /// <summary>
        /// 替换所有的字符串
        /// </summary>
        /// <param name="strFind">查找的字符串</param>
        /// <param name="strReplace">替换的字符串</param>
        /// <returns>替换的次数</returns>
        public int ReplaceTextAll(string strFind, string strReplace)
        {
            int iCount = 0;
            this.SetSelection(0, 0);
            while (FindText(strFind))
            {
                this.ReplaceSelection(strReplace);
                iCount++;
            }
            return iCount;
        }
        #endregion


        #region 移动当前插入点位置的函数群
        /// <summary>
        /// 将插入点向上移动一行
        /// </summary>
        public void MoveUpOneLine()
        {
            ZYTextLine myLine = this.PreLine;
            if (myLine != null)
            {
                if (intLastXPos <= 0)
                {
                    ZYTextElement StartElement = (ZYTextElement)myElements[intSelectStart];
                    intLastXPos = StartElement.RealLeft;
                }
                for (int i = 0; i < myLine.Elements.Count; i++)//foreach (ZYTextElement myElement in myLine.Elements)
                {
                    ZYTextElement myElement = myLine.Elements[i] as ZYTextElement;
                    if (myElement.RealLeft >= intLastXPos)
                    {
                        #region bwy:
                        //如果在元素中
                        if (myElement.Parent is ZYTextBlock)
                        {
                            myElement = (myElement.Parent as ZYTextBlock).FirstElement;
                        }
                        #endregion bwy:
                        this.MoveSelectStart(myElement);
                        return;
                    }
                }
                this.MoveSelectStart(myLine.LastElement);
            }
            #region comment
            // MoveDownOneLine

            //
            //			ZYTextElement StartElement =(ZYTextElement) myElements[intSelectStart];
            //			//int OldLineIndex = StartElement.LineIndex ;
            //			int OldLeft	 = intLastXPos ;
            //			if( intLastXPos <= 0 )
            //			{
            //				OldLeft = StartElement.RealLeft + StartElement.Width  ;
            //				intLastXPos = OldLeft ;
            //			}
            //
            //			ZYTextElement myElement = null;
            //			bool bolLineChanged = false;
            //			ZYTextElement LastParent = StartElement.Parent ;
            //			//int LineIndex = 0 ;
            //			ZYTextLine OldLine = StartElement.OwnerLine ;
            //			for( int iCount = intSelectStart - 1 ; iCount >= 0  ; iCount -- )
            //			{
            //				myElement = ( ZYTextElement ) myElements[iCount];
            //				if( bolLineChanged == false && ( myElement.OwnerLine != OldLine  ))
            //				{
            //					bolLineChanged = true;
            //					OldLine = myElement.OwnerLine ;
            //				}
            //				if( bolLineChanged)
            //				{
            //					if( myElement.OwnerLine != OldLine  )
            //					{
            //						this.MoveSelectStart( iCount +1 );
            //						break;
            //					}
            //
            //					if( myElement.RealLeft <= OldLeft )
            //					{
            //						this.MoveSelectStart( iCount ) ;
            //						break;
            //					}
            //				}
            //				LastParent = myElement.Parent ;
            //			}
            //this.MoveSelectStart(0);
            #endregion
        }

        /// <summary>
        /// 将插入点向下移动一行
        /// </summary>
        public void MoveDownOneLine()
        {
            ZYTextLine myLine = this.NextLine;
            if (myLine != null)
            {
                if (intLastXPos <= 0)
                {
                    ZYTextElement StartElement = (ZYTextElement)myElements[intSelectStart];
                    intLastXPos = StartElement.RealLeft;
                }
                for (int i = 0; i < myLine.Elements.Count; i++)//foreach (ZYTextElement myElement in myLine.Elements)
                {
                    ZYTextElement myElement = myLine.Elements[i] as ZYTextElement;
                    if (myElement.RealLeft >= intLastXPos)
                    {
                        #region bwy:
                        //如果在元素中
                        if (myElement.Parent is ZYTextBlock)
                        {
                            myElement = this.GetNextElement((myElement.Parent as ZYTextBlock).LastElement);
                        }
                        #endregion bwy:
                        this.MoveSelectStart(myElement);
                        return;
                    }
                }
                this.MoveSelectStart(myLine.LastElement);
            }// MoveDownOneLine
        }

        /// <summary>
        /// 将插入点向左移动一个元素
        /// </summary>
        public void MoveLeft()
        {
            intLastXPos = -1;
            if (intSelectStart > 0)
            {
                //如果移动到了元素内部，则修正
                ZYTextElement ele = this.Elements[intSelectStart - 1] as ZYTextElement;
                if (ele.Parent is ZYTextBlock)
                {
                    intSelectStart = this.Elements.IndexOf((ele.Parent as ZYTextBlock).FirstElement);
                    this.MoveSelectStart(intSelectStart);
                }
                else
                {
                    this.MoveSelectStart(intSelectStart - 1);
                }
            }
        }

        /// <summary>
        /// 将插入点向右移动一个元素
        /// </summary>
        public void MoveRight()
        {
            intLastXPos = -1;
            if (intSelectStart < myElements.Count - 1)
            {
                //如果移动到了元素内部，则修正
                ZYTextElement ele = this.Elements[intSelectStart] as ZYTextElement;
                if (ele.Parent is ZYTextBlock)
                {
                    intSelectStart = this.Elements.IndexOf((ele.Parent as ZYTextBlock).LastElement);
                    this.MoveSelectStart(intSelectStart + 1);
                }
                else
                {
                    this.MoveSelectStart(intSelectStart + 1);
                }
            }
        }

        /// <summary>
        /// 将插入点移动到当前行的最后一个元素处
        /// </summary>
        public void MoveEnd()
        {
            try
            {
                ZYTextLine myLine = this.CurrentLine;
                if (myLine != null && bolLineEndFlag == false)
                {
                    intLastXPos = -1;
                    //this.CurrentElement = myLine.LastElement;
                    ZYTextElement ele = myLine.LastElement;
                    if (ele.Parent is ZYTextBlock)
                    {
                        ele = (ele.Parent as ZYTextBlock).FirstElement;
                        this.MoveSelectStart(ele);
                        return;
                    }

                    if (ele.isNewLine())//(myLine.LastElement.isNewLine())
                    {
                        this.MoveSelectStart(ele);//(myLine.LastElement);
                    }
                    else
                    {
                        //this.MoveSelectStart(myElements.IndexOf(myLine.LastElement) + 1);

                        this.MoveSelectStart(myElements.IndexOf(ele) + 1);
                        bolLineEndFlag = true;
                        ((ZYTextDocument)myDocument).UpdateTextCaret();
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 移动当前插入点到当前行的行首
        /// </summary>
        public void MoveHome()
        {
            ZYTextLine myLine = null;
            myLine = this.CurrentLine;
            if (myLine != null)
            {
                intLastXPos = -1;

                #region bwy:
                ZYTextElement ele = myLine.FirstElement;
                if (ele.Parent is ZYTextBlock && ele != ele.Parent.FirstElement)
                {
                    ele = this.GetNextElement((ele.Parent as ZYTextBlock).LastElement);
                }
                int FirstIndex = myElements.IndexOf(ele);
                #endregion bwy:
                // 获得第一个非空白字符元素的序号
                int FirstNBlank = 0;

                foreach (ZYTextElement myElement in myLine.Elements)
                {
                    ZYTextChar myChar = myElement as ZYTextChar;
                    if (myChar == null || char.IsWhiteSpace(myChar.Char) == false)
                    {
                        FirstNBlank = myLine.Elements.IndexOf(myElement);
                        break;
                    }
                }
                if (FirstNBlank == 0 || intSelectStart == (FirstIndex + FirstNBlank))
                {
                    this.MoveSelectStart(FirstIndex);
                }
                else
                {
                    this.MoveSelectStart(FirstIndex + FirstNBlank);
                }
            }
        }// void MoveHome

        /// <summary>
        /// 获得指定位置处的元素
        /// 2009-7-2 22:00 mfb重新实现. 参照了MoveTo(x,y)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public ZYTextElement GetElementAt(int x, int y)
        {
            if (myDocument != null && myDocument.Lines != null && myDocument.Lines.Count > 0)
            {
                ZYTextLine CurrentLine = null;

                foreach (ZYTextLine myLine in myDocument.Lines)
                {
                    if (myLine.RealTop + myLine.Height >= y)
                    {
                        if (myLine.RealLeft <= x && (myLine.RealLeft + myLine.ContentWidth) >= x)
                        {
                            CurrentLine = myLine;
                            break;
                        }
                    }
                }
                if (CurrentLine == null && myDocument.Lines.Count > 0)
                    CurrentLine = (ZYTextLine)myDocument.Lines[myDocument.Lines.Count - 1];

                if (CurrentLine == null)
                    return null;
                x -= CurrentLine.RealLeft;


                ZYTextElement CurrentElement = null;

                if (CurrentLine.Elements.Count == 0)
                {
                    CurrentElement = CurrentLine.Container;
                }
                foreach (ZYTextElement myElement in CurrentLine.Elements)
                {
                    if (WeiWenProcess.weiwen)
                    {
                        //维文形式，鼠标的X坐标是 左->右，而取字符是 左<-右 
                        if (x > myElement.Left && x < myElement.Left + myElement.Width)
                        {
                            if (x > (myElement.Left + myElement.Width))
                            {
                                continue;
                            }
                            if (myElement.Parent.WholeElement)
                                return null;
                            CurrentElement = myElement;
                            break;
                        }
                    }
                    else
                    {
                        if (x < myElement.Left + myElement.Width)
                        {
                            //为什么小于了还会大于
                            if (x > (myElement.Left + myElement.Width))
                            {
                                continue;
                            }
                            if (myElement.Parent.WholeElement)
                                return null;
                            CurrentElement = myElement;
                            break;
                        }
                    }
                }

                if (CurrentElement == null)
                {
                    CurrentElement = CurrentLine.LastElement;
                }
                return CurrentElement;

            }
            return null;
        }



        /// <summary>
        /// 检测当前的插入点位置是否正确
        /// </summary>
        /// <returns></returns>
        private bool CheckSelectStart()
        {
            if (myElements == null)
                return false;
            else
                return (intSelectStart >= 0 && intSelectStart <= myElements.Count - 1);
        }



        /// <summary>
        /// 移动当前插入点
        /// </summary>
        /// <param name="iStep">纵向的移动距离</param>
        public void MoveStep(int iStep)
        {
            ZYTextElement StartElement = (ZYTextElement)myElements[intSelectStart];
            this.MoveTo(StartElement.RealLeft, StartElement.RealTop + iStep);
        }


        #endregion

        /// <summary>
        /// 初始化对象
        /// </summary>
        public ZYContent()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 判断是否可以删除选中元素,如果选中多个单元格的内容则不允许删除 Add By wwj 2012-04-24
        /// </summary>
        /// <returns></returns>
        public bool CanDeleteSelectElement()
        {
            //表示是否有元素在单元格的外部
            bool isHasElementOutCell = false;

            System.Collections.ArrayList myList = this.GetSelectElements();
            List<ZYTextElement> cellList = new List<ZYTextElement>();
            foreach (ZYTextElement ele in myList)
            {
                ZYTextElement textElement = this.GetParentByElement(ele, ZYTextConst.c_Cell);
                if (textElement != null)
                {
                    //既有在单元格内的，又有在单元格外的，则不允许删除元素
                    if (isHasElementOutCell) return false;

                    if (!cellList.Contains(textElement))
                    {
                        cellList.Add(textElement);
                        if (cellList.Count > 1)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    isHasElementOutCell = true;//表示此元素在单元格的外部

                    //当即选中表格外的元素，又选择了表格中的元素则不允许删除操作
                    if (cellList.Count > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 得到编辑器中第一个表格 Add by wwj 2012-05-29
        /// </summary>
        /// <returns></returns>
        public TPTextTable GetFirstTable()
        {
            ArrayList list = myElements;
            for (int i = 0; i < list.Count; i++)
            {
                ZYTextElement myElement = (ZYTextElement)myElements[i];
                ZYTextElement table = GetParentByElement(myElement, ZYTextConst.c_Table);
                if (table != null && table is TPTextTable)
                {
                    return ((TPTextTable)table).Clone();
                }
            }
            return null;
        }

        /// <summary>
        /// 设置单元格是否可以换行 Add By wwj 2012-06-06
        /// </summary>
        public void SetTableCellCanInsertEnter(bool canEnter)
        {
            if (myElements == null || myElements.Count == 0)
                return;
            if (this.IsLock(intSelectStart))
                return;

            //获得当前元素
            ZYTextElement myElement = (ZYTextElement)myElements[intSelectStart];
            //获得当前表格
            TPTextTable currentTable = GetParentByElement(myElement, ZYTextConst.c_Table) as TPTextTable;
            if (currentTable != null)
            {
                List<TPTextCell> listCells = new List<TPTextCell>();
                foreach (TPTextRow row in currentTable)
                {
                    foreach (TPTextCell cell in row)
                    {
                        if (true == cell.CanAccess)
                        {
                            cell.CanInsertEnter = canEnter;
                        }
                    }
                }
            }
        }
    }
}
