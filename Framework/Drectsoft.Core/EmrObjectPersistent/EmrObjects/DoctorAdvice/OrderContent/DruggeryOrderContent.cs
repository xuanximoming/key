using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Text;


using System.Globalization;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 药品医嘱内容
   /// </summary> 
   public class DruggeryOrderContent : OrderContent
   {
      #region properties
      /// <summary>
      /// 标记是否允许停止
      /// </summary>
      public override bool CanCeased
      {
         get { return true; }
      }

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { throw new NotImplementedException(); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
         get { throw new NotImplementedException(); }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public DruggeryOrderContent()
         : base()
      {
         InnerOrderKind = OrderContentKind.Druggery;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public DruggeryOrderContent(DataRow sourceRow)
         : base(sourceRow)
      {
         InnerOrderKind = OrderContentKind.Druggery;
      }
      #endregion

      /// <summary>
      /// 重设当前可显示的内容,为各项内容间加上空格
      /// </summary>
      protected override void ResetDisplayTexts()
      {
         InitBaseDisplayTexts();

         // 顺序为：项目 数量单位 [用法] [频次] [嘱托]
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
         // 长期医嘱才显示频次
         if ((ParentOrder == null) || (ParentOrder.GetType() == typeof(LongOrder)))
         {
            if ((ItemFrequency != null) && (ItemFrequency.KeyInitialized))
            {
               Texts.Insert(0, new OutputInfoStruct(ItemFrequency.ToString().Trim() + tail, OrderOutputTextType.ItemFrequency));
               if (tail.Length == 0)
                  tail = " ";
            }
         }
         if ((ItemUsage != null) && (ItemUsage.KeyInitialized))
         {
            Texts.Insert(0, new OutputInfoStruct(ItemUsage.ToString().Trim() + tail, OrderOutputTextType.ItemUsage));
            if (tail.Length == 0)
               tail = " ";
         }

         Texts.Insert(0, new OutputInfoStruct(Amount.ToString() + CurrentUnit.Name.Trim() + tail, OrderOutputTextType.ItemAmount));

         Texts.Insert(0, new OutputInfoStruct(Item.ToString().Trim() + " ", OrderOutputTextType.ItemName));
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
         if ((ItemUsage == null) || (!ItemUsage.KeyInitialized))
            errMsg.AppendLine("必须选择用法");
         if ((ItemFrequency == null) || (!ItemFrequency.KeyInitialized))
            errMsg.AppendLine("必须选择频次");
         
         return errMsg.ToString();
      }
   }
}
