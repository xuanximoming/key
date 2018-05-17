using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Library;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Wordbook;

namespace DrectSoft.Core.DoctorAdvice
{
   public partial class FrequencyInputor : XtraUserControl
   {
      #region properties
      /// <summary>
      /// 当前处理的医嘱频次
      /// </summary> 
      [Browsable(false),
       ReadOnly(true)]
      public OrderFrequency Frequency
      {
         get 
         {
            if (_showListWindow != null)
            {
               if (frequencyInput.HadGetValue && (frequencyInput.PersistentObjects.Count > 0))
                  return frequencyInput.PersistentObjects[0] as OrderFrequency;
            }
            return null;
         }
         set
         {
            if (value != null)
               SetFrequency(value.Code);
            else
               SetFrequency("");
            //_currentFrequency = value;
         }
      }
      //private OrderFrequency _currentFrequency;

      /// <summary>
      /// 
      /// </summary>
      [Browsable(false),
       ReadOnly(true)]
      public bool HadGetValue
      {
         get { return frequencyInput.HadGetValue; }
      }

      /// <summary>
      /// 频次字典
      /// </summary>
      [Browsable(false),
       ReadOnly(true)]
      public BaseWordbook FrequencyBook
      {
         get { return frequencyInput.NormalWordbook; }
         set { frequencyInput.NormalWordbook = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      [Browsable(false),
       ReadOnly(true)]
      public bool ShowFormImmediately
      {
         get { return frequencyInput.ShowFormImmediately; }
         set { frequencyInput.ShowFormImmediately = value; }
      }

      /// <summary>
      /// 数据选择窗口
      /// </summary>
      public LookUpWindow LookUpWindow
      {
         get { return _showListWindow; }
         set 
         { 
            _showListWindow = value;
            frequencyInput.ListWindow = value;
         }
      }
      private LookUpWindow _showListWindow;
      #endregion

      #region private variables
      private bool m_IsInit;
      #endregion

      #region ctor
      public FrequencyInputor()
      {
         InitializeComponent();
         if (!DesignMode)
            InitializeRuntimeUI();
      }
      #endregion

      #region public methods
      /// <summary>
      /// 
      /// </summary>
      /// <param name="frequencyCode"></param>
      public void SetFrequency(string frequencyCode)
      {
         if (!String.IsNullOrEmpty(frequencyCode))
            frequencyInput.CodeValue = frequencyCode;
         else
            frequencyInput.CodeValue = "";
         //_currentFrequency = value;
         ResetFrequencyDetailUI();
      }
      #endregion

      #region private methods
      private void InitializeRuntimeUI()
      {
         m_IsInit = true;
         // 调整控件位置，撑满界面
         panelContainer.Location = new Point(0, 0);
         this.Size = panelContainer.Size;
         m_IsInit = false;
      }

      /// <summary>
      /// 根据当前选中的频次，设置频次的明细信息
      /// </summary>
      private void ResetFrequencyDetailUI()
      {
         // 执行日期：
         //    只对"周"有效
         //    如果预设的执行日期不包括当天(或明天,根据设置来),则将预设日期平行移动,自动调整到当天(或明天)
         //    可以手工调整,选择的日期数量根据执行周期来定
         // 执行时间:
         //    只对"周""天"有效
         //    可以手工调整,选择的时间点数量根据执行次数来定
         //    --对于"小时",要根据执行周期控制时间间隔

         // 先设置控件的初始数据，再设置界面属性
         UncheckListBox(listBoxWeekDay);
         UncheckListBox(listBoxHour);

         listBoxWeekDay.Enabled = false;
         listBoxHour.Enabled = false;

         if (Frequency != null)
         {
            if (Frequency.PeriodUnitFlag == OrderExecPeriodUnitKind.Week)
            {
               ResetWeedDayListBox(Frequency.WeekDays);
               listBoxWeekDay.Enabled = true;
            }

            if ((Frequency.PeriodUnitFlag == OrderExecPeriodUnitKind.Week)
               || ((Frequency.PeriodUnitFlag == OrderExecPeriodUnitKind.Day)))
            {
               ResetHourListBox(Frequency.ExecuteTime);
               listBoxHour.Enabled = true;
            }
            frequencyDetail.Text = Frequency.Text;
         }
         else
            frequencyDetail.Text = "";

         frequencyDetail.Enabled = listBoxHour.Enabled;
      }

      /// <summary>
      /// 重设频次详细信息（在修改当前频次设置后调用)
      /// </summary>
      /// <returns>频次的详细信息</returns>
      private string CheckFrequencyDetail()
      {
         // 首先判断选中的执行日期、时间数量是否超过频次规定的数量，超过的话，则从末位开始去掉多余的选项
         // 将执行日期和执行时间选择同步到频次对象，然后显示频次内容
         if (Frequency != null)
         {
            StringBuilder warningMsg = new StringBuilder();
            int moreCount;
            // 执行周期单位是周时，检查选中的执行日期天数是否超过默认值
            if (Frequency.PeriodUnitFlag == OrderExecPeriodUnitKind.Week)
            {
               moreCount = listBoxWeekDay.CheckedIndices.Count - Frequency.Period;
               if (moreCount > 0)
               {
                  UncheckListBox(listBoxWeekDay, moreCount);
                  warningMsg.AppendLine(ConstMessages.CheckSelectedTooManyExecuteDate);
               }
               else if (moreCount < 0)
               {
                  ResetWeedDayListBox(Frequency.WeekDays);
                  warningMsg.AppendLine(ConstMessages.CheckSelectedTooFewExecuteDate);
               }
            }

            // 周期单位是周、天时，检查选中的执行时间数是否超过默认值
            if ((Frequency.PeriodUnitFlag == OrderExecPeriodUnitKind.Week)
               || ((Frequency.PeriodUnitFlag == OrderExecPeriodUnitKind.Day)))
            {
               moreCount = listBoxHour.CheckedIndices.Count - Frequency.ExecuteTimesPerPeriod;
               if (moreCount > 0)
               {
                  UncheckListBox(listBoxHour, moreCount);
                  warningMsg.AppendLine(ConstMessages.CheckSelectedTooManyExecuteTime);
               }
               else if (moreCount < 0)
               {
                  ResetHourListBox(Frequency.ExecuteTime);
                  warningMsg.AppendLine(ConstMessages.CheckSelectedTooFewExecuteTime);
               }
            }

            Frequency.WeekDays = CombExecuteDay();
            Frequency.ExecuteTime = CombExecuteTime();

            frequencyDetail.ErrorText = warningMsg.ToString();
            return Frequency.Text;
         }
         else
         {
            return "";
         }
      }

      /// <summary>
      /// 将指定CheckListBox中的所有项目置为Unchecked
      /// </summary>
      /// <param name="listBox"></param>
      private static void UncheckListBox(CheckedListBoxControl listBox)
      {
         foreach (CheckedListBoxItem item in listBox.Items)
            item.CheckState = CheckState.Unchecked;
      }

      /// <summary>
      /// 将指定CheckListBox中的最后n个选中的项目置为Unchecked
      /// </summary>
      /// <param name="listBox"></param>
      /// <param name="lastCount"></param>
      private static void UncheckListBox(CheckedListBoxControl listBox, int lastCount)
      {
         int count = 0;
         for (int i = listBox.Items.Count - 1; i >= 0; i--)
         {
            if (listBox.Items[i].CheckState != CheckState.Checked)
               continue;

            listBox.Items[i].CheckState = CheckState.Unchecked;
            count++;
            if (count == lastCount)
               break;
         }
      }

      /// <summary>
      /// 用传入的字符串初始化日期选择ListBox。
      /// </summary>
      /// <param name="days">对应于星期中的某天的数组合而成的字符串</param>
      private void ResetWeedDayListBox(string days)
      {
         foreach (char dw in days)
         {
            if ((dw < 49) || (dw > 55))
               continue;

            listBoxWeekDay.Items[dw - 49].CheckState = CheckState.Checked;
         }
      }

      /// <summary>
      /// 将选中的执行日期组合成字符串
      /// </summary>
      /// <returns></returns>
      private string CombExecuteDay()
      {
         if (!listBoxWeekDay.Enabled)
            return "";

         StringBuilder dws = new StringBuilder();
         for (int i = 0; i < listBoxWeekDay.Items.Count; i++)
         {
            if (listBoxWeekDay.Items[i].CheckState != CheckState.Checked)
               continue;

            dws.Append(i + 1);
         }
         return dws.ToString();
      }

      /// <summary>
      /// 用传入的执行时间组合字符串初始化时间选择ListBox
      /// </summary>
      /// <param name="hourCombination">执行时间组合字符串</param>
      private void ResetHourListBox(string hourCombination)
      {
         string[] hours = hourCombination.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
         for (int i = 0; i <= hours.Length - 1; i++)
         {
            listBoxHour.Items[listBoxHour.FindStringExact(hours[i])].CheckState = CheckState.Checked;
         }
      }

      /// <summary>
      /// 将选中的执行时间组合成字符串
      /// </summary>
      /// <returns></returns>
      private string CombExecuteTime()
      {
         if ((!listBoxHour.Enabled) || (listBoxHour.CheckedIndices.Count < 1))
            return "";

         StringBuilder hours = new StringBuilder();
         for (int i = 0; i < listBoxHour.Items.Count; i++)
         {
            if (listBoxHour.Items[i].CheckState != CheckState.Checked)
               continue;
            if (hours.Length > 0)
               hours.Append(",");
            hours.Append(listBoxHour.Items[i].Value);
         }

         return hours.ToString();
      }
      #endregion

      #region event handle
      private void FrequencyInputor_Resize(object sender, EventArgs e)
      {
         if (!m_IsInit)
         {
            // 同步修改内部控件的尺寸
            m_IsInit = true;
            frequencyDetail.Width = this.Size.Width - frequencyInput.Width;
            this.Height = frequencyInput.Height;
            m_IsInit = false;
         }
      }

      private void frequencyInput_CodeValueChanged(object sender, EventArgs e)
      {
         ResetFrequencyDetailUI();
      }

      private void frequencyDetail_Closed(object sender, DevExpress.XtraEditors.Controls.ClosedEventArgs e)
      {
         frequencyDetail.Text = CheckFrequencyDetail();
      }

      private void frequencyDetail_TextChanged(object sender, EventArgs e)
      {
         frequencyDetail.ToolTip = frequencyDetail.Text;
      }

      private void listBox_DrawItem(object sender, ListBoxDrawItemEventArgs e)
      {
         if ((sender as CheckedListBoxControl).GetItemCheckState(e.Index) == CheckState.Checked)
         {
            e.Appearance.BackColor = Color.FromArgb(163, 224, 148);
         }
      }
      #endregion
   }
}
