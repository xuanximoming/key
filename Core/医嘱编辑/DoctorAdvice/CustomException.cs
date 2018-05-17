using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 自定义数据检查异常
   /// </summary>
   public class DataCheckException : Exception
   {
      /// <summary>
      /// 外部用来设置被校验数据的行号
      /// </summary>
      public int RowIndex
      {
         get { return _rowIndex; }
         set { _rowIndex = value; }
      }
      private int _rowIndex;

      /// <summary>
      /// 外部可以用来保存被校验医嘱的序号
      /// </summary>
      public decimal OrderSerialNo
      {
         get { return _orderSerialNo; }
         set { _orderSerialNo = value; }
      }
      private decimal _orderSerialNo;

      /// <summary>
      /// 表示被检查数据内容的名称
      /// </summary>
      public string DataName
      {
         get 
         {
            if (_dataName == null)
               return "";
            else
               return _dataName;
         }
      }
      private string _dataName;

      /// <summary>
      /// 警告级别：0：警告  1：错误
      /// </summary>
      public int WarnningLevel
      {
         get
         {
            return _warnningLevel;
         }
      }
      private int _warnningLevel;

      public DataCheckException()
      {
         _warnningLevel = 1;
      }

      public DataCheckException(string message, string dataName)
         : base(message)
      {
         _dataName = dataName;
         _warnningLevel = 1;
      }

      public DataCheckException(string message, string dataName, int warnningLevel)
         : base(message)
      {
         _dataName = dataName;
         _warnningLevel = warnningLevel;
      }

      public DataCheckException(string message, string dataName, int warnningLevel, Exception inner)
         : base(message, inner)
      {
         _dataName = dataName;
         _warnningLevel = warnningLevel;
      }
   }

   /// <summary>
   /// 自定义调用remoting服务异常
   /// </summary>
   public class CallRemotingException : Exception
   {
      public CallRemotingException() : base ("")
      { }

      public CallRemotingException(string message): base(message)
      { }
   }
}
