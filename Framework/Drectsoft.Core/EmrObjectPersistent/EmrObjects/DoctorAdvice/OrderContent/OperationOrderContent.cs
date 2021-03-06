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
   /// 手术医嘱内容（应该只出现在临时医嘱中）,需要填写手术时间
   /// </summary>
   public sealed class OperationOrderContent : OrderContent
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
      /// 医嘱嘱托字段
      /// </summary>
      public new string EntrustContent
      {
         get
         {
            if (_entrustContent == null)
               _entrustContent = "";
            //   ParseInfoFromEntrustContent();

            return _entrustContent;
         }
         set { _entrustContent = value; }
      }
      private string _entrustContent;

      internal string InnerEntrustContent
      {
         get { return ComposeEntrustContent(); }
         set { ParseInfoFromEntrustContent(value); }
      }

      /// <summary>
      /// 手术时间(保存在嘱托字段中)
      /// </summary>
      public DateTime OperationTime
      {
         get
         {
            //if (_operationTime == DateTime.MinValue)
            //   ParseInfoFromEntrustContent();

            return _operationTime;
         }
         set
         {
            if (_operationTime != value)
            {
               _operationTime = value;
               //ComposeEntrustContent();
               FireOrderContentChanged(new EventArgs());
            }
         }
      }
      private DateTime _operationTime;

      /// <summary>
      /// 麻醉方法(保存在嘱托中)
      /// </summary>
      public Anesthesia AnesthesiaOperation
      {
         get
         {
            //if (_anesthesiaOperation == null)
            //   ParseInfoFromEntrustContent();

            return _anesthesiaOperation;
         }
         set
         {
            _anesthesiaOperation = value;
            //ComposeEntrustContent();
            FireOrderContentChanged(new EventArgs());
         }
      }
      private Anesthesia _anesthesiaOperation;

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
      public OperationOrderContent()
         : base()
      {
         InnerOrderKind = OrderContentKind.Operation;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public OperationOrderContent(DataRow sourceRow)
         : base(sourceRow)
      {
         InnerOrderKind = OrderContentKind.Operation;
      }
      #endregion

      /// <summary>
      /// 将手术时间和麻醉代码组合到嘱托中
      /// </summary>
      private string ComposeEntrustContent()
      {
         if (OperationTime == DateTime.MinValue)
            return "";

         string anesthesia;
         string entrust;
         if ((_anesthesiaOperation != null) && _anesthesiaOperation.KeyInitialized)
            anesthesia = AnesthesiaOperation.Code;
         else
            anesthesia = "";
         if (String.IsNullOrEmpty(EntrustContent))
            entrust = "";
         else
            entrust = EntrustContent;

         return String.Format(CultureInfo.CurrentCulture, "{1}{0}{2}{0}{3}"
               , OrderContent.CombFlag
               , OperationTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture)
               , anesthesia
               , entrust);
      }

      /// <summary>
      /// 从嘱托中解析出手术时间和麻醉代码，并赋值给对应属性
      /// </summary>
      private void ParseInfoFromEntrustContent(string originalEntrust)
      {
         if (_anesthesiaOperation == null)
            _anesthesiaOperation = new Anesthesia();

         if (originalEntrust == null)
            originalEntrust = "";
         string[] splits = new string[] { OrderContent.CombFlag };
         string[] infos = originalEntrust.Split(splits, StringSplitOptions.RemoveEmptyEntries);
         if ((infos != null) && (infos.Length > 0))
         {
            _operationTime = Convert.ToDateTime(infos[0], CultureInfo.CurrentCulture);
            if (infos.Length >= 2)
               _anesthesiaOperation.Code = infos[1];
            if (infos.Length >= 3)
               EntrustContent = infos[2];
         }
         else
         {
            _operationTime = DateTime.MinValue;
            EntrustContent = "";
         }
      }

      /// <summary>
      /// 重设当前可显示的内容,为各项内容间加上空格
      /// </summary>
      protected override void ResetDisplayTexts()
      {
         InitBaseDisplayTexts();

         // 顺序为：手术时间 手术项目
         if (Item == null) // 如果没有项目，则默认为显示内容为空
            return;

         Texts.Insert(0, new OutputInfoStruct("行 " + Item.Name.Trim(), OrderOutputTextType.ItemName));

         if (AnesthesiaOperation.KeyInitialized)
         {
            if (String.IsNullOrEmpty(AnesthesiaOperation.Name.Trim()))
               AnesthesiaOperation.ReInitializeProperties();
            Texts.Insert(0, new OutputInfoStruct("在 " + AnesthesiaOperation.Name.Trim() + " 下 ", OrderOutputTextType.NormalText));
         }
         if (OperationTime > DateTime.MinValue)
            Texts.Insert(0, new OutputInfoStruct(OperationTime.ToString("M月d日 HH:mm", CultureInfo.CurrentCulture) + " "
               , OrderOutputTextType.EntrustContent));

      }

      /// <summary>
      /// 校验属性值
      /// </summary>
      /// <returns>返回字符串不为空表示有属性的值不正确</returns>
      public override string CheckProperties()
      {
         StringBuilder errMsg = new StringBuilder();
         if ((Item == null) || (String.IsNullOrEmpty(Item.KeyValue)))
            errMsg.AppendLine("必须选择手术项目");
         if (Amount != 1)
            errMsg.AppendLine("项目数量只能为1");
         //if ((ItemFrequency == null) || (!ItemFrequency.KeyInitialized))
         //   errMsg.AppendLine("必须选择频次");
         if (OperationTime == DateTime.MinValue)
            errMsg.AppendLine("必须输入手术时间");

         return errMsg.ToString();
      }

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         base.ReInitializeAllProperties();
         if (AnesthesiaOperation != null)
            AnesthesiaOperation.ReInitializeAllProperties();
      }
   }
}
