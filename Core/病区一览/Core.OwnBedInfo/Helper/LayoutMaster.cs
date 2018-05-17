using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraGrid.Views.Card;
using System.Collections.ObjectModel;
using DevExpress.XtraGrid.Views.Base;

namespace DrectSoft.Core.OwnBedInfo
{
    /// <summary>
    /// 屏幕分辨率
    /// </summary>
    public enum ScreenSize
    {
        /// <summary>
        /// 小型屏幕(640*480)
        /// </summary>
        SmallScreen = 2,
        /// <summary>
        /// 中等屏幕(800*600)
        /// </summary>
        MiddleScreen = 3,
        /// <summary>
        /// 标准屏幕(1024*768)
        /// </summary>
        StandardScreen = 4,
        /// <summary>
        /// 宽屏(1440*900)
        /// </summary>
        WideScreen = 5,
        /// <summary>
        /// 较大屏幕(1280*960)
        /// </summary>
        SuperiorScreen = 6,
        /// <summary>
        /// 大型屏幕(1280*1024)
        /// </summary>
        BigScreen = 7,
        /// <summary>
        /// 超大屏幕(1600*1200及以上)
        /// </summary>
        SupramaxilmalScreen = 8
    }
    /// <summary>
    /// 布局管理器
    /// </summary>
    public static class LayoutMaster
    {
        private static Dictionary<string, Font> m_CardFont;

        /// <summary>
        /// 自动调整cardView适应窗体
        /// </summary>
        /// <param name="cardView"></param>
        /// <returns></returns>
        internal static Dictionary<string, Font> GetGridFont(BaseView cardView)
        {
            m_CardFont = new Dictionary<string, Font>();
            if (cardView == null)
            {
                return null;
            }
            m_CardFont.Add("FieldCaption", new Font("Tahoma", 10F));
            m_CardFont.Add("FieldValue", new Font("Tahoma", 10F));
            m_CardFont.Add("CardCaption", new Font("宋体", 15F));
            m_CardFont.Add("FocusedCardCaption", new Font("Tahoma", 15F));
            m_CardFont.Add("SelectedCardCaption", new Font("Tahoma", 15F));
            return m_CardFont;
        }
        private const int OneRowHeight = 19;//19
        private const int OneCardHeight = 105;//105;//xjt
        private const int OneCaptionHeight = 28;
        private const int OnePageRowCount = 5;
        //病区一览标题栏高度
        private const int PatientBar = 28;
        //除了主床位卡区域以外的程序上部空间的高度
        private const int SolidRegion = 85;
        //页数栏高度
        private const int ToolBarHeight = 27;
        private const int RegionAbovePageBarAndCard = 114;
        private const int RegionBottom = 32;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowCount">卡片行数</param>
        /// <param name="bedsCount">床位总数</param>
        /// <param name="isObstetricWard"></param>
        /// <param name="expandedRowsCount"></param>
        /// <returns></returns>
        internal static Action CalcHeight(int rowCount, int bedsCount, bool isObstetricWard, out int expandedRowsCount)
        {
            int width = Screen.PrimaryScreen.WorkingArea.Width;
            int height = Screen.PrimaryScreen.WorkingArea.Height;
            int pages = (bedsCount > (rowCount * 10)) ? 2 : 1;
            int extraHeight = 0;
            expandedRowsCount = 0;
            int extraRowHeight = 0;
            int oneCardHeight = isObstetricWard ? (OneCardHeight + OneRowHeight) : OneCardHeight;

            if (pages == 1)
            {
                //一页模式，这里计算的是用工作区高减去固定存在的上部空间及下部空间和卡片间隔，算出每个卡片行的可用高度，3为修正值
                //extraHeight = height - 6 * (rowCount + 1) - PatientBar - ToolBarHeight - SolidRegion - oneCardHeight * rowCount - 3;
                extraHeight = height - 6 * 2 - (rowCount - 1) * 5 - RegionAbovePageBarAndCard - RegionBottom - oneCardHeight * rowCount - 9;
                extraRowHeight = extraHeight / rowCount;
                if (extraRowHeight > OneRowHeight)
                {
                    //可以扩展的行数
                    expandedRowsCount = extraRowHeight / OneRowHeight;
                    return Action.ExpandRow;
                }
                if (extraRowHeight < 0)
                    return Action.NotEnoughRegion;
                return Action.NoAction;
            }
            else
            {
                //两页及以上模式，这里计算的使用工作区高减去固定存在的上部空间及下部空间和卡片间隔和页数工具条的高，算出每个卡片的可用高度,3为修正值
                //extraHeight = height - 6 * (rowCount + 1) - PatientBar - SolidRegion - ToolBarHeight - ToolBarHeight - oneCardHeight * rowCount - 3;
                extraHeight = height - 6 * 2 - (rowCount - 1) * 5 - RegionAbovePageBarAndCard - RegionBottom - ToolBarHeight - oneCardHeight * rowCount - 3;
                if (extraHeight > 0)
                {
                    if (extraHeight >= oneCardHeight)
                        return Action.NextRowCount;
                    else
                    {
                        extraRowHeight = extraHeight / rowCount;
                        if (extraRowHeight >= OneRowHeight)
                        {
                            //可以扩展的行数
                            expandedRowsCount = extraRowHeight / OneRowHeight;
                            return Action.ExpandRow;
                        }
                        return Action.NoAction;
                    }
                }
                else
                    return Action.NotEnoughRegion;
            }
        }

        internal enum Action
        {
            /// <summary>
            /// 无动作
            /// </summary>
            NoAction = 0,
            /// <summary>
            /// 扩展行高
            /// </summary>
            ExpandRow = 1,
            /// <summary>
            /// 继续下一个行数的判断
            /// </summary>
            NextRowCount = 2,
            /// <summary>
            /// 不足空间显示
            /// </summary>
            NotEnoughRegion = 3
        }

        private static int ColCount
        {
            get { return _colCount; }
        }
        private static int _colCount;

        /// <summary>
        /// 容器宽
        /// </summary>
        public static int ContainerWidth
        {
            set
            {
                m_ContainerWidth = value;
                if (m_ContainerWidth == 0)
                    m_ContainerWidth = Screen.PrimaryScreen.Bounds.Size.Width;
            }
        }
        private static int m_ContainerWidth;

        /// <summary>
        /// 设置卡片的数量和宽度
        /// </summary>
        internal static Dictionary<string, int> GetScreenAutoSize(int bedsCount, bool isObstetricWard)
        {
            int rowCardView, colCardView, cardInterval, cardWidth, rowWholeNumber;
            cardInterval = 4;
            rowCardView = 4;
            cardWidth = 170;// (m_PanelWidth / 8) + 1;// ((Screen.PrimaryScreen.Bounds.Size.Width - 10 - (colCardView + 6) * cardInterval) / colCardView) - 1;
            colCardView = (m_ContainerWidth) / cardWidth;
            _colCount = colCardView;
            int expandedRowCount;
            for (int i = OnePageRowCount; ; i++)
            {
                Action action = CalcHeight(i, bedsCount, isObstetricWard, out expandedRowCount);
                if (action == Action.NextRowCount)
                    continue;
                else
                {
                    switch (action)
                    {
                        case Action.NoAction:
                            rowCardView = i;
                            break;
                        case Action.ExpandRow:
                            rowCardView = i;
                            break;
                        case Action.NextRowCount:
                            break;
                        case Action.NotEnoughRegion:
                            rowCardView = i - 1;
                            CalcHeight(rowCardView, bedsCount, isObstetricWard, out expandedRowCount);
                            break;
                        default:
                            break;
                    }
                    break;
                }
            }
            rowWholeNumber = rowCardView * colCardView;
            Dictionary<string, int> result = new Dictionary<string, int>();
            result.Add("rowCardView", rowCardView);
            result.Add("colCardView", colCardView);
            result.Add("rowWholeNumber", rowWholeNumber);
            result.Add("cardInterval", cardInterval);
            result.Add("cardWidth", cardWidth);
            result.Add("expandedRows", expandedRowCount);
            return result;
        }
    }
}
