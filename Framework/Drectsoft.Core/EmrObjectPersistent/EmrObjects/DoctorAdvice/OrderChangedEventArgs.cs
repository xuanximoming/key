using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Common.Eop
{
   ///// <summary>
   ///// 医嘱改变事件委托
   ///// </summary>
   ///// <param name="sender"></param>
   ///// <param name="e"></param>
   //public delegate void OrderChangedEventHandler(object sender, OrderChangedEventArgs e);

   /// <summary>
   /// 提供医嘱改变事件(OrderChanged)的参数
   /// </summary>
   public class OrderChangedEventArgs : EventArgs
   {
      /// <summary>
      /// 更新后的医嘱对应的医嘱序号
      /// </summary>
      public decimal NewSerialNo
      {
         get { return _newSerialNo; }
      }
      private decimal _newSerialNo;

      /// <summary>
      /// 被更新的医嘱原始的医嘱序号（除了更新医嘱序号，其余情况下都为-1）
      /// </summary>
      public decimal OldSerialNo
      {
         get { return _oldSerialNo; }
      }
      private decimal _oldSerialNo;

      /// <summary>
      /// 医嘱改变事件参数
      /// </summary>
      /// <param name="newSerialNo">更新后的医嘱对应的医嘱序号</param>
      public OrderChangedEventArgs(decimal newSerialNo)
      {
         _newSerialNo = newSerialNo;
         _oldSerialNo = -1;
      }

      /// <summary>
      /// 医嘱改变事件参数
      /// </summary>
      /// <param name="newSerialNo">更新后的医嘱对应的医嘱序号</param>
      /// <param name="oldSerialNo">被更新的医嘱原始的医嘱序号</param>
      public OrderChangedEventArgs(decimal newSerialNo, decimal oldSerialNo)
      {
         _newSerialNo = newSerialNo;
         _oldSerialNo = oldSerialNo;
      }
   }
}
