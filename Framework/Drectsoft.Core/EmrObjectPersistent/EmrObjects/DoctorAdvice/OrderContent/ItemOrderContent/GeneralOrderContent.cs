using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Text;


namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 常规医嘱内容类.继承自ChargeItemContent类.（应该只出现在长期医嘱中）
   /// 不需要用法,频次
   /// </summary>
   public class GeneralOrderContent : ChargeItemOrderContent
   {
      #region properties
      /// <summary>
      /// 是否允许停止
      /// </summary>
      public override bool CanCeased
      {
         get { return true; }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public GeneralOrderContent()
         : base()
      {
         InnerOrderKind = OrderContentKind.GeneralItem;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public GeneralOrderContent(DataRow sourceRow)
         : base(sourceRow)
      {
         InnerOrderKind = OrderContentKind.GeneralItem;
      }
      #endregion

      /// <summary>
      /// 重设当前可显示的内容,为各项内容间加上空格
      /// </summary>
      protected override void ResetDisplayTexts()
      {
         InitBaseDisplayTexts();

         // 顺序为：项目 [嘱托]
         if (Item == null) // 如果没有项目，则默认为显示内容为空
            return;

         string tail = ""; // 长度大于1表示已经有结尾了，已便于在各项内容间插入空格
         if (EntrustContent.Length > 0)
         {
            Texts.Insert(0, new OutputInfoStruct(EntrustContent.Trim()
               , OrderOutputTextType.EntrustContent));
            tail = " ";
         }
         Texts.Insert(0, new OutputInfoStruct(Item.ToString().Trim() + tail
            , OrderOutputTextType.ItemName));
      }

      /// <summary>
      /// 校验属性值
      /// </summary>
      /// <returns>返回字符串不为空表示有属性的值不正确</returns>
      public override string CheckProperties()
      {
         StringBuilder errMsg = new StringBuilder();
         if ((Item == null) || (String.IsNullOrEmpty(Item.Code)))
            errMsg.AppendLine("必须选择项目");
         
         return errMsg.ToString();
      }
   }
}