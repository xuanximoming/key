using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


using System.Globalization;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 出院带药医嘱内容.继承自DruggerOrderContent
   /// </summary>
   public class OutDruggeryContent : DruggeryOrderContent
   {
      #region properties
      /// <summary>
      /// 标记是否允许停止
      /// </summary>
      public override bool CanCeased
      {
         get { return false; }
      }

      /// <summary>
      /// 执行天数
      /// </summary>
      public int ExecuteDays
      {
         get { return _executeDays; }
         set
         {
            if (value <= 0)
               throw new ArgumentException(MessageStringManager.GetString("CommonValueIsLess", 0));

            _executeDays = value;
            FireOrderContentChanged(new EventArgs());
         }
      }
      private int _executeDays;

      /// <summary>
      /// 药品总数量(为出院带药保留,使用剂量单位)
      /// </summary>
      public decimal TotalAmount
      {
         get { return _totalAmount; }
         set { _totalAmount = value; }
      }
      private decimal _totalAmount;
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public OutDruggeryContent()
         : base()
      {
         InnerOrderKind = OrderContentKind.OutDruggery;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public OutDruggeryContent(DataRow sourceRow)
         : base(sourceRow)
      {
         InnerOrderKind = OrderContentKind.OutDruggery;
      }
      #endregion

      /// <summary>
      /// 重设当前可显示的内容,为各项内容间加上空格
      /// </summary>
      protected override void ResetDisplayTexts()
      {
         InitBaseDisplayTexts();

         // 顺序为：项目 数量单位 [用法] [频次] 天数 [嘱托]
         if (Item == null) // 如果没有项目，则默认为显示内容为空
            return;

         string tail = ""; // 长度大于1表示已经有结尾了，已便于在各项内容间插入空格
         if ((Attributes & OrderAttributeFlag.Provide4Oneself) > 0)
         {
            Texts.Insert(0, new OutputInfoStruct("自备", OrderOutputTextType.SelfProvide));
            tail = " ";
         }
         if (!String.IsNullOrEmpty(EntrustContent))
         {
            Texts.Insert(0, new OutputInfoStruct(EntrustContent.Trim(), OrderOutputTextType.EntrustContent));
            tail = " ";
         }
         Texts.Insert(0, new OutputInfoStruct(ExecuteDays.ToString("#'天'", CultureInfo.CurrentCulture), OrderOutputTextType.ItemDays));
         if (tail.Length == 0)
            tail = " ";

         if ((ItemFrequency != null) && (ItemFrequency.KeyInitialized))
            Texts.Insert(0, new OutputInfoStruct(ItemFrequency.ToString().Trim() + tail, OrderOutputTextType.ItemFrequency));
         if ((ItemUsage != null) && (ItemUsage.KeyInitialized))
            Texts.Insert(0, new OutputInfoStruct(ItemUsage.ToString().Trim() + tail, OrderOutputTextType.ItemUsage));

         Texts.Insert(0, new OutputInfoStruct(Amount.ToString("#.##", CultureInfo.CurrentCulture) + CurrentUnit.Name.Trim() + tail, OrderOutputTextType.ItemAmount));

         Texts.Insert(0, new OutputInfoStruct(Item.ToString().Trim() + tail, OrderOutputTextType.ItemName));
      }

      /// <summary>
      /// 校验属性值
      /// </summary>
      /// <returns>返回字符串不为空表示有属性的值不正确</returns>
      public override string CheckProperties()
      {
         StringBuilder errMsg = new StringBuilder();
         if ((Item == null) || (String.IsNullOrEmpty(Item.KeyValue)))
            errMsg.AppendLine("必须选择药品");
         if ((Amount <= 0) || (Amount > 1000))
            errMsg.AppendLine("药品数量应在0～1000范围内");
         if (CurrentUnit.IsEmpty)
            errMsg.AppendLine("必须选择单位");
         if (ItemUsage == null)
            errMsg.AppendLine("必须选择用法");
         if (ItemFrequency == null)
            errMsg.AppendLine("必须选择频次");
         if ((ExecuteDays <= 0) || (ExecuteDays > 30))
            errMsg.AppendLine("执行天数应在1～30之间");

         return errMsg.ToString();
      }

      /// <summary>
      /// 重新计算总数量的数值
      /// </summary>
      public void ReCalcTotalAmount()
      {
         TotalAmount = Amount * ItemFrequency.ExecuteTimesPerDay * ExecuteDays;
      }
   }
}
