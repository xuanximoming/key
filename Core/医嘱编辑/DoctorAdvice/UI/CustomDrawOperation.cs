using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Eop;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using System.Reflection;
using DevExpress.XtraGrid.Columns;

namespace DrectSoft.Core.DoctorAdvice
{
   public static class CustomDrawOperation
   {
      #region properties
      /// <summary>
      /// 有关CustomDraw的设置
      /// </summary>
      public static GridCustomDrawSetting DrawSetting
      {
         get { return CoreBusinessLogic.CustomSetting.CustomDrawSetting; }
      }

      /// <summary>
      /// 有关Grid的设置
      /// </summary>
      public static OrderGridSetting GridSetting
      {
         get { return CoreBusinessLogic.CustomSetting.GridSetting; }
      }
      #endregion

      #region private methods
      /// <summary>
      /// stringList转换为字符串
      /// </summary>
      /// <param name="list"></param>
      /// <param name="startIndex"></param>
      /// <param name="length"></param>
      /// <returns></returns>
      private static string ListToString(Collection<string> list, int startIndex, int length)
      {
         StringBuilder result = new StringBuilder();
         for (int index = startIndex; index < (startIndex + length); index++)
            result.Append(list[index]);
         return result.ToString();
      }

      /// <summary>
      /// 根据传入的信息创建输出信息结构体(处于同一行的数据将合并在一起)
      /// </summary>
      /// <param name="texts"></param>
      /// <param name="widthes"></param>
      /// <param name="font"></param>
      /// <param name="count">显示的元素个数</param>
      /// <param name="startIndexOfLine2"></param>
      /// <param name="outputWidth"></param>
      /// <returns></returns>
      private static Collection<OutputInfoStruct> CreateOutputList(Collection<string> texts, Font font, int count, int startIndexOfLine2, int outputWidth)
      {
         // 同一行的内容合并后一起输出,输出区域是相对于单元格的尺寸
         Collection<OutputInfoStruct> results = new Collection<OutputInfoStruct>();

         if (startIndexOfLine2 == 0) // 只有一行数据, 居中显示
         {
            int fontHeight = Convert.ToInt32(font.GetHeight());
            results.Add(new OutputInfoStruct(
                            ListToString(texts, 0, count)
                          , new Rectangle(DrawSetting.Margin.Left
                                         , DrawSetting.Margin.Top
                                          + ((DrawSetting.OutputSizeOfContent.Height - fontHeight) / 2)
                                         , outputWidth
                                         , fontHeight)
                          , font
                          , OrderOutputTextType.NormalText));
         }
         else
         {
            // 0 ～ (startIndexOfLine2 - 1) 的内容显示在上半部
            results.Add(new OutputInfoStruct(
                            ListToString(texts, 0, startIndexOfLine2)
                          , new Rectangle(DrawSetting.Margin.Left
                                         , DrawSetting.Margin.Top
                                         , outputWidth
                                         , DrawSetting.OutputSizeOfContent.Height / 2)
                          , font
                          , OrderOutputTextType.NormalText));
            // startIndexOfLine2 ～ 最后 的内容显示在下半部
            results.Add(new OutputInfoStruct(
                            ListToString(texts, startIndexOfLine2
                                        , count - startIndexOfLine2)
                          , new Rectangle(DrawSetting.Margin.Left
                                         , DrawSetting.Margin.Top
                                          + (DrawSetting.OutputSizeOfContent.Height / 2)
                                         , outputWidth
                                         , DrawSetting.OutputSizeOfContent.Height / 2)
                          , font
                          , OrderOutputTextType.NormalText));
         }

         return results;
      }

      /// <summary>
      /// 计算各段文本在指定字体下的宽度(以象素为单位)
      /// </summary>
      /// <param name="widthes"></param>
      /// <param name="texts"></param>
      /// <param name="font"></param>
      private static void CalcTextsWidth(List<int> widthes, Collection<string> texts, Font font)
      {
         widthes.Clear();
         for (int index = 0; index < texts.Count; index++)
            widthes.Add(CalcStringWidth(texts[index], font));
      }

      /// <summary>
      /// 计算字符串在给定字体下的宽度
      /// </summary>
      /// <param name="text"></param>
      /// <param name="font"></param>
      private static int CalcStringWidth(string text, Font font)
      {
         return TextRenderer.MeasureText(text, font).Width;
      }

      /// <summary>
      /// 获取指定类型分组标记的输出内容结构体
      /// </summary>
      /// <param name="groupType">分组标记的类型</param>
      /// <param name="groupTextWidth">分组公共信息的宽度</param>
      /// <returns></returns>
      private static OutputInfoStruct GetGroupFlagOutput(OrderOutputTextType groupType, int groupTextWidth)
      {
         Rectangle bounds = new Rectangle();
         int startPosition = GridSetting.WidthOfContentCell
            - DrawSetting.Margin.Right - groupTextWidth - DrawSetting.GroupFlagWidth + 1;
         int flagWidth = DrawSetting.GroupFlagWidth - 3;
         switch (groupType)
         {
            case OrderOutputTextType.GroupStart:
               bounds = new Rectangle(startPosition
                  , GridSetting.RowHeight / 2
                  , flagWidth
                  , GridSetting.RowHeight / 2);
               break;
            case OrderOutputTextType.GroupMiddle:
               bounds = new Rectangle(startPosition, 0
                  , flagWidth, GridSetting.RowHeight);
               break;
            case OrderOutputTextType.GroupEnd:
               bounds = new Rectangle(startPosition, 0
                  , flagWidth, GridSetting.RowHeight / 2);
               break;
         }

         return new OutputInfoStruct("", bounds, DrawSetting.DefaultFont.Font, groupType);
      }

      /// <summary>
      /// 获取分组公共信息的输出内容结构体
      /// </summary>
      /// <param name="groupText">分组公共信息</param>
      /// <param name="groupTextWidth">分组公共信息的宽度</param>
      /// <returns></returns>
      private static OutputInfoStruct GetGroupPublicOutput(string groupText, int groupTextWidth)
      {
         // 靠右输出，垂直居中
         int fontHeight = Convert.ToInt32(DrawSetting.DefaultFont.Font.GetHeight());
         return new OutputInfoStruct(groupText
            , new Rectangle(GridSetting.WidthOfContentCell - DrawSetting.Margin.Right - groupTextWidth
                           , (GridSetting.RowHeight - fontHeight) / 2
                           , CalcStringWidth(groupText, DrawSetting.DefaultFont.Font)
                           , fontHeight)
            , DrawSetting.DefaultFont.Font
            , OrderOutputTextType.NormalText);
         //// 用法、频次紧跟着分组标志，垂直居中
         //int fontHeight = Convert.ToInt32(DefaultFont.GetHeight());
         //return new OutputInfoStruct(groupText
         //   , new Rectangle(groupTextWidth
         //                  , (CellSize.Height - fontHeight) / 2
         //                  , CalcStringWidth(groupText, DefaultFont)
         //                  , fontHeight)
         //   , DefaultFont
         //   , OrderOutputTextType.NormalText);
      }

      private static string[] ProcessFirstElement(string text, Font font, int maxWidth)
      {
         int moreWidth = CalcStringWidth(text, font) - maxWidth;
         int index = text.Length - 1;
         StringBuilder first = new StringBuilder();
         StringBuilder second = new StringBuilder();

         if (moreWidth > 0)
         {
            for (; index >= 0; index--)
            {
               second.Insert(0, text[index]);
               if (CalcStringWidth(second.ToString(), font) >= moreWidth)
                  break;
            }
            index--;
         }
         for (int start = 0; start <= index; start++)
            first.Append(text[start]);

         return new string[2] { first.ToString(), second.ToString() };
      }

      /// <summary>
      /// 创建一般的输出内容
      /// </summary>
      /// <param name="outputsTexts"></param>
      /// <param name="maxWidth"></param>
      /// <returns></returns>
      private static Collection<OutputInfoStruct> CreateNormalOutputInfos(Collection<string> outputTexts, int maxWidth)
      {
         Font font = DrawSetting.DefaultFont.Font;// 内容输出时使用的字体，初始从默认字体开始
         float fontSize = font.Size;            // 输出字体的字号，初始从默认字号开始
         List<int> widthes = new List<int>();   // 输出内容的文本宽度
         int totalWidth;                        // 计算宽度和的临时变量
         int lineNum;                           // 记录在当前字号下可以显示几行内容（最多两行）
         int startIndexOfLine2 = 0;             // 第二行内容第一个元素的索引号
         int nextIndex;                         // 作为循环中索引的临时变量
         int maxIndex;                          // 在此索引之前的内容可以显示在输出区域中
         bool hadSplited = false;               // 标记第一个元素是否被分成两行
         string[] firstElements;

         do
         {
            // 首先对第一个元素进行处理, 如果在当前字体下不能完全放在第一行，则将余下的内容放到下一行
            if (hadSplited)
            {
               firstElements = ProcessFirstElement(outputTexts[0] + outputTexts[1]
                  , font, maxWidth); // 已拆分，则由前两个元素组合而成
               outputTexts.RemoveAt(1);
               hadSplited = false;
            }
            else
               firstElements = ProcessFirstElement(outputTexts[0], font, maxWidth);
            outputTexts[0] = firstElements[0];
            if (firstElements[1].Length > 0)
            {
               outputTexts.Insert(1, firstElements[1]);
               hadSplited = true;
            }

            // 以当前字体计算每段文本的宽度
            CalcTextsWidth(widthes, outputTexts, font);

            // 判断是否能折成两行显示
            if (DrawSetting.OutputSizeOfContent.Height >= (font.GetHeight() * 2))
               lineNum = 2;
            else
               lineNum = 1;
            // 依次向行中添加元素，直到达到行的极限
            nextIndex = 0;
            maxIndex = 0;
            for (int lineHandle = 0; lineHandle < lineNum; lineHandle++) // 逐行添加
            {
               totalWidth = 0;
               startIndexOfLine2 = nextIndex; // 记录每行的起始索引号
               for (; nextIndex < widthes.Count; nextIndex++)
               {
                  totalWidth += widthes[nextIndex];
                  if (totalWidth > maxWidth) // 加入当前元素时，超过了一行的宽度
                  {
                     // 每行至少要显示一段内容，以避免第一个元素太长，从而导致显示的信息太少
                     if (nextIndex != startIndexOfLine2)
                        break;
                  }
               }
               maxIndex = nextIndex;
            }
            // 判断内容是否有剩余
            if (maxIndex == widthes.Count)
               break;

            // 如果还有内容没放下，且已经达到最小字体，则对最后一段内容替换成"…"
            // 否则缩小半号字体，再次进行计算
            if (fontSize == DrawSetting.MinFontSize)
            {
               if (maxIndex > 1) // 不能替换第一个元素
                  outputTexts[maxIndex - 1] = "…";
               break;
            }

            fontSize -= 0.5F;
            font = new Font(DrawSetting.DefaultFont.FontFamily, fontSize);
         } while (fontSize >= DrawSetting.MinFontSize); // 应该在此条件未成立前就已退出循环

         return CreateOutputList(outputTexts, font, maxIndex, startIndexOfLine2, maxWidth);
      }

      private static GridViewInfo GetGridViewInfo(GridView view)
      {
         FieldInfo fi;
         fi = typeof(GridView).GetField("fViewInfo", BindingFlags.NonPublic | BindingFlags.Instance);
         return fi.GetValue(view) as GridViewInfo;
      }
      #endregion

      #region public methods
      /// <summary>
      /// 根据传入的文本信息，生成输出信息列表(包括)
      /// </summary>
      /// <param name="texts"></param>
      /// <returns></returns>
      public static Collection<OutputInfoStruct> CreateOutputeInfo(Collection<OutputInfoStruct> texts)
      {
         if ((texts == null) || (texts.Count == 0))
            return new Collection<OutputInfoStruct>();

         int maxWidth = DrawSetting.OutputSizeOfContent.Width; // 供一般信息输出的最大宽度(有分组和无分组时是不一样的)
         Collection<string> outputTexts = new Collection<string>();  // 实际需要输出的文本
         OrderOutputTextType groupType = OrderOutputTextType.NormalText; // 分组类型，NormalText表示非分组记录
         string usageText = "";                          // 用法信息
         string frequencyText = "";                      // 频次信息
         string groupText = "";                          // 分组时输出的公共信息 
         int groupTextWidth = 0;                         // 分组信息在默认字体下的宽度
         int insertPos = -1;                             // 未分组时，向输出的List中插入用法、频次
         string cancelText = "";                         // 取消信息
         OutputInfoStruct info;

         for (int index = 0; index < texts.Count; index++)
         {
            info = texts[index];
            switch (info.OutputType)
            {
               case OrderOutputTextType.GroupStart:
               case OrderOutputTextType.GroupMiddle:
               case OrderOutputTextType.GroupEnd:
                  groupType = info.OutputType;
                  break;
               case OrderOutputTextType.CancelInfo:
                  cancelText = info.Text;
                  break;
               case OrderOutputTextType.ItemUsage:
                  insertPos = index; // 用法肯定在频次前面
                  usageText = info.Text;
                  break;
               case OrderOutputTextType.ItemFrequency:
                  frequencyText = info.Text;
                  break;
               default:
                  outputTexts.Add(info.Text);
                  break;
            }
         }

         // 判断是否是分组的记录(同组的用法、频次只显示一次), 然后提取需要输出的文本
         if (groupType != OrderOutputTextType.NormalText)
         {
            groupText = usageText + frequencyText.Trim();
            groupTextWidth = CalcStringWidth(groupText, DrawSetting.DefaultFont.Font);
            // 重新计算一般信息允许输出的最大宽度，在处理完一般信息后，再处理分组信息的输出
            maxWidth = maxWidth - groupTextWidth - DrawSetting.GroupFlagWidth;
         }
         else
         {
            if (insertPos > 0)
            {
               outputTexts.Insert(insertPos, usageText);
               outputTexts.Insert(insertPos + 1, frequencyText);
            }
            else
            {
               if (usageText.Length > 0)
                  outputTexts.Add(usageText);
               if (frequencyText.Length > 0)
                  outputTexts.Add(frequencyText);
            }
         }

         // 首先生成一般信息的输出
         Collection<OutputInfoStruct> results = CreateNormalOutputInfos(outputTexts, maxWidth);

         // 处理分组信息
         if (groupType != OrderOutputTextType.NormalText)
         {
            // 原先的想法是让分组信息紧跟着前面的信息，但是因为每一条记录是单独处理
            // 同一组的记录其分组标志的位置可能错位。因此改成从右到左显示分组信息

            // 先处理公共信息(只有组的第一条记录才需要分组的公共信息)
            if (groupType == OrderOutputTextType.GroupStart)
               results.Add(GetGroupPublicOutput(groupText, groupTextWidth));
            // 再画分组标记
            results.Add(GetGroupFlagOutput(groupType, groupTextWidth));

            //maxWidth = 0;
            //foreach (OutputInfoStruct text in results)
            //   if (maxWidth < text.Bounds.Width)
            //      maxWidth = text.Bounds.Width;

            //// 分组标志紧跟着最长的那行
            //results.Add(GetGroupFlagOutput(groupType, maxWidth));
            //// 只有组的第一条记录才需要分组的公共信息
            //if (groupType == OrderOutputTextType.GroupStart)
            //   results.Add(GetGroupPublicOutput(groupText, maxWidth + GroupFlagWidth));
         }
         // 处理取消信息
         if (cancelText.Length > 0)
            results.Add(new OutputInfoStruct(cancelText, DrawSetting.BoundsOfCancel
               , DrawSetting.FontOfCancel.Font
               , OrderOutputTextType.CancelInfo));

         return results;
      }

      /// <summary>
      /// 根据医嘱状态和是否选中等信息获取前景色
      /// </summary>
      /// <param name="orderState">医嘱状态</param>
      /// <param name="isSelected">是否被选中</param>
      /// <param name="hadSynched">医嘱数据是否已同步(未同步时有单独的颜色设置)</param>
      /// <returns></returns>
      public static Color GetForeColorByState(OrderState orderState, bool isSelected, bool hadSynched)
      {
         if (hadSynched)
         {
            switch (orderState)
            {
               case OrderState.New:
                  if (isSelected)
                     return DrawSetting.NewOrderColor.SelectedColor.ForeColor;
                  else
                     return DrawSetting.NewOrderColor.NormalColor.ForeColor;
               case OrderState.Audited:
                  if (isSelected)
                     return DrawSetting.AuditedColor.SelectedColor.ForeColor;
                  else
                     return DrawSetting.AuditedColor.NormalColor.ForeColor;
               case OrderState.Cancellation:
                  if (isSelected)
                     return DrawSetting.CancelledColor.SelectedColor.ForeColor;
                  else
                     return DrawSetting.CancelledColor.NormalColor.ForeColor;
               case OrderState.Executed:
                  if (isSelected)
                     return DrawSetting.ExecutedColor.SelectedColor.ForeColor;
                  else
                     return DrawSetting.ExecutedColor.NormalColor.ForeColor;
               case OrderState.Ceased:
                  if (isSelected)
                     return DrawSetting.CeasedColor.SelectedColor.ForeColor;
                  else
                     return DrawSetting.CeasedColor.NormalColor.ForeColor;
               default:
                  if (isSelected)
                     return DrawSetting.DefaultColor.SelectedColor.ForeColor;
                  else
                     return DrawSetting.DefaultColor.NormalColor.ForeColor;
            }
         }
         else
         {
            if (isSelected)
               return DrawSetting.NotSynchColor.SelectedColor.ForeColor;
            else
               return DrawSetting.NotSynchColor.NormalColor.ForeColor;
         }
      }

      /// <summary>
      /// 根据医嘱状态和是否选中等信息获取背景色
      /// </summary>
      /// <param name="orderState">医嘱状态</param>
      /// <param name="isSelected">是否被选中</param>
      /// <param name="hadSynched">医嘱数据是否已同步(未同步时有单独的颜色设置)</param>
      /// <returns></returns>
      public static Color GetBackColorByState(OrderState orderState, bool isSelected, bool hadSynched)
      {
         if (hadSynched)
         {
            switch (orderState)
            {
               case OrderState.New:
                  if (isSelected)
                     return DrawSetting.NewOrderColor.SelectedColor.BackColor;
                  else
                     return DrawSetting.NewOrderColor.NormalColor.BackColor;
               case OrderState.Audited:
                  if (isSelected)
                     return DrawSetting.AuditedColor.SelectedColor.BackColor;
                  else
                     return DrawSetting.AuditedColor.NormalColor.BackColor;
               case OrderState.Cancellation:
                  if (isSelected)
                     return DrawSetting.CancelledColor.SelectedColor.BackColor;
                  else
                     return DrawSetting.CancelledColor.NormalColor.BackColor;
               case OrderState.Executed:
                  if (isSelected)
                     return DrawSetting.ExecutedColor.SelectedColor.BackColor;
                  else
                     return DrawSetting.ExecutedColor.NormalColor.BackColor;
               case OrderState.Ceased:
                  if (isSelected)
                     return DrawSetting.CeasedColor.SelectedColor.BackColor;
                  else
                     return DrawSetting.CeasedColor.NormalColor.BackColor;
               default:
                  if (isSelected)
                     return DrawSetting.DefaultColor.SelectedColor.BackColor;
                  else
                     return DrawSetting.DefaultColor.NormalColor.BackColor;
            }
         }
         else
         {
            if (isSelected)
               return DrawSetting.NotSynchColor.SelectedColor.BackColor;
            else
               return DrawSetting.NotSynchColor.NormalColor.BackColor;
         }
      }

      public static Image CreateColorLegend(Color backColor, Color foreColor, string text)
      {
         Font font = new Font("SimSun", 9f); 
         int textWidth = CalcStringWidth(text, font);
         Bitmap legend = new Bitmap(textWidth, 16);
         
         Graphics gp = Graphics.FromImage(legend);
         // 先画个反色的框，再填充(因为DxBarItem会自动设透明色，背景图案就看不到了)
         Color penColor = Color.Beige; // Color.FromArgb(backColor.A, 255 - backColor.R, 255 - backColor.G, 255 - backColor.B);
         gp.DrawRectangle(new Pen(penColor, 1), 0, 0, textWidth, 16); 
         gp.FillRectangle(new SolidBrush(backColor), 1, 1, textWidth - 1, 15);
         gp.DrawString(text, font, new SolidBrush(foreColor), 2, 2);

         return legend;
      }

      public static Rectangle GetGridCellRect(GridView view, int rowHandle, GridColumn column)
      {
         GridViewInfo info = GetGridViewInfo(view);
         GridCellInfo cell = info.GetGridCellInfo(rowHandle, column);
         if (cell != null)
            return cell.Bounds;
         return Rectangle.Empty;
      }

      #endregion
   }
}
