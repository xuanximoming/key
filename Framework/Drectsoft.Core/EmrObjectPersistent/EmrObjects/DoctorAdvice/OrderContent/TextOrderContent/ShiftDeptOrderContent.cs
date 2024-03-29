using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Globalization;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 转科医嘱内容，需要填写转往科室和病区
   /// </summary>
   public sealed class ShiftDeptOrderContent : TextOrderContent
   {
      #region properties
      /// <summary>
      /// 隐藏嘱托字段，避免外部直接访问
      /// </summary>
      private new string EntrustContent
      {
         get
         {
            return "";
         }
         //set { _entrustContent = value; }
      }
      internal string InnerEntrustContent
      {
         get { return ComposeEntrustContent(); }
         set { ParseCodeFromEntrustContent(value); }
      }
      //private string _entrustContent;

      /// <summary>
      /// 转往科室代码(和转往病区代码组合在一起保存在嘱托字段中)
      /// </summary>
      public Department ShiftDept
      {
         get
         {
            return _shiftDept;
         }
         set
         {
            _shiftDept = value;
            //ComposeEntrustContent();
            FireOrderContentChanged(new EventArgs());
         }
      }
      private Department _shiftDept;

      /// <summary>
      /// 转往病区代码(和转往科室代码组合在一起保存在嘱托字段中)
      /// </summary>
      public Ward ShiftWard
      {
         get
         {
            return _shiftWard;
         }
         set
         {
            _shiftWard = value;
            FireOrderContentChanged(new EventArgs());
         }
      }
      private Ward _shiftWard;
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public ShiftDeptOrderContent()
         : base()
      {
         InnerOrderKind = OrderContentKind.TextShiftDept;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public ShiftDeptOrderContent(DataRow sourceRow)
         : base(sourceRow)
      {
         InnerOrderKind = OrderContentKind.TextShiftDept;
      }
      #endregion

      /// <summary>
      /// 将转往科室和病区的代码组合到嘱托中
      /// </summary>
      private string ComposeEntrustContent()
      {
         if ((_shiftDept == null) || (_shiftWard == null))
            return "";
         else if (ShiftDept.KeyInitialized && ShiftWard.KeyInitialized)
            return String.Format(CultureInfo.CurrentCulture, "{0}{2}{1}", ShiftDept.Code, ShiftWard.Code, OrderContent.CombFlag);
         else
            return "";
      }

      /// <summary>
      /// 从嘱托中解析出转往科室和病区代码，并赋值给对应属性
      /// </summary>
      private void ParseCodeFromEntrustContent(string originalEntrust)
      {
         if (_shiftDept == null)
            _shiftDept = new Department();
         if (_shiftWard == null)
            _shiftWard = new Ward();

         if (originalEntrust == null)
            originalEntrust = "";

         string[] splits = new string[] { OrderContent.CombFlag };
         string[] codes = originalEntrust.Split(splits, StringSplitOptions.RemoveEmptyEntries);
         if ((codes != null) && (codes.Length == 2))
         {
            _shiftDept.Code = codes[0];
            _shiftDept.ReInitializeProperties();
            _shiftWard.Code = codes[1];
            _shiftWard.ReInitializeProperties();
         }
      }

      /// <summary>
      /// 重设当前可显示的内容,为各项内容间加上空格
      /// </summary>
      protected override void ResetDisplayTexts()
      {
         InitBaseDisplayTexts();

         if (ShiftDept.KeyInitialized && ShiftWard.KeyInitialized)
            Texts.Insert(0, new OutputInfoStruct(String.Format(CultureInfo.CurrentCulture, "转往 {0}[{1}]", ShiftDept.Name.Trim(), ShiftWard.Name.Trim())
               , OrderOutputTextType.ItemName));
         else
            Texts.Insert(0, new OutputInfoStruct("转科医嘱", OrderOutputTextType.ItemName));
      }

      /// <summary>
      /// 校验属性值
      /// </summary>
      /// <returns>返回字符串不为空表示有属性的值不正确</returns>
      public override string CheckProperties()
      {
         StringBuilder errMsg = new StringBuilder();
         if (ShiftDept.KeyInitialized && ShiftWard.KeyInitialized)
            return "";
         else
            errMsg.AppendLine("必须同时选择转往科室和病区");

         return errMsg.ToString();
      }

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         base.ReInitializeAllProperties();
         if (ShiftDept != null)
            ShiftDept.ReInitializeAllProperties();
         if (ShiftWard != null)
            ShiftWard.ReInitializeAllProperties();
      }
   }
}
