using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Text;



namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 临时医嘱
   /// </summary> 
   public class TempOrder : Order
   {
      #region properites

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return ""; }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
         get { return ""; }
      }
      //  tzxh		utXh12		null	,	--停止序号(BL_CQYZK.cqyzxh)
      //  tzrq		utDatetime	null	,	--停止日期

      /// <summary>
      /// (医技)申请单序号
      /// </summary>
      public decimal ApplySerialNo
      {
         get { return _applySerialNo; }
         set { _applySerialNo = value; }
      }
      private decimal _applySerialNo;
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public TempOrder()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public TempOrder(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion
   }
}
