using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;


using System.Globalization;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 出院医嘱内容, 需要填写出院时间
   /// </summary>
   public sealed class LeaveHospitalOrderContent : TextOrderContent
   {
      #region properties
      /// <summary>
      /// 隐藏嘱托字段，避免外部直接访问
      /// </summary>
      private new string EntrustContent
      {
         get { return ""; }
      }
      internal string InnerEntrustContent
      {
         get
         {
            return _leaveHospitalTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
         }
         set
         {
            ConvertEntrustContent2DateTime(value);
         }
      }

      /// <summary>
      /// 出院时间(保存在嘱托字段中)
      /// </summary>
      public DateTime LeaveHospitalTime
      {
         get
         {
            return _leaveHospitalTime;
         }
         set
         {
            if (_leaveHospitalTime != value)
            {
               _leaveHospitalTime = value;
               //_entrustContent = _leaveHospitalTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
               FireOrderContentChanged(new EventArgs());
            }
         }
      }
      private DateTime _leaveHospitalTime;
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public LeaveHospitalOrderContent()
         : base()
      {
         InnerOrderKind = OrderContentKind.TextLeaveHospital;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public LeaveHospitalOrderContent(DataRow sourceRow)
         : base(sourceRow)
      {
         InnerOrderKind = OrderContentKind.TextLeaveHospital;
      }
      #endregion

      /// <summary>
      /// 从嘱托中解析出出院时间，并赋值给对应属性
      /// </summary>
      private void ConvertEntrustContent2DateTime(string originalEntrust)
      {
         // 从嘱托中解析出出院时间
         if (String.IsNullOrEmpty(originalEntrust))
            _leaveHospitalTime = DateTime.MinValue;
         else
            _leaveHospitalTime = Convert.ToDateTime(originalEntrust, CultureInfo.CurrentCulture);
      }

      /// <summary>
      /// 重设当前可显示的内容,为各项内容间加上空格
      /// </summary>
      protected override void ResetDisplayTexts()
      {
         InitBaseDisplayTexts();

         // 顺序为：出院时间 “出院”
         if (LeaveHospitalTime == DateTime.MinValue)
            Texts.Insert(0, new OutputInfoStruct("出院医嘱", OrderOutputTextType.ItemName));
         else
            Texts.Insert(0, new OutputInfoStruct(LeaveHospitalTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture) + "出院"
               , OrderOutputTextType.ItemName));
      }

      /// <summary>
      /// 校验属性值
      /// </summary>
      /// <returns>返回字符串不为空表示有属性的值不正确</returns>
      public override string CheckProperties()
      {
         StringBuilder errMsg = new StringBuilder();
         if (LeaveHospitalTime <= DateTime.Now)
            errMsg.AppendLine("必须输入出院时间，并且不能在当前时间之前");

         return errMsg.ToString();
      }
   }
}