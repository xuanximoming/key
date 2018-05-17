using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 创建医嘱内容的工厂类
   /// </summary>
   public sealed class OrderContentFactory
   {
       private static string ColumnNameOrderKind = "OrderType";
      private OrderContentFactory()
      { }

      /// <summary>
      /// 根据DataRow中制定列的值确定合适的医嘱内容类名
      /// </summary>
      /// <param name="kindValue"></param>
      /// <returns></returns>
      public static string GetOrderContentClassName(object kindValue)
      {
         if (kindValue == null)
            throw new ArgumentNullException("kindValue"
               , MessageStringManager.GetString("CommonParameterIsNull", "医嘱类型"));
         
         OrderContentKind orderKind =
            (OrderContentKind)Enum.Parse(typeof(OrderContentKind), kindValue.ToString());

         switch (orderKind)
         {
            case OrderContentKind.Druggery: //药品医嘱
               return  "DruggeryOrderContent";
            case OrderContentKind.ChargeItem://普通项目医嘱
               return "ChargeItemOrderContent";
            case OrderContentKind.GeneralItem://常规医嘱
               return "GeneralOrderContent";
            case OrderContentKind.ClinicItem://临床项目医嘱
               return "ClinicItemOrderContent";
            case OrderContentKind.OutDruggery://出院带药
               return "OutDruggeryContent";
            case OrderContentKind.Operation://手术医嘱
               return "OperationOrderContent";
            //case OrderKindFlags.CeaseLong://停长期医嘱
            //   break;
            case OrderContentKind.TextNormal://纯医嘱(普通)
               return "TextOrderContent";
            case OrderContentKind.TextShiftDept://纯医嘱(转科)
               return "ShiftDeptOrderContent";
            case OrderContentKind.TextAfterOperation://纯医嘱(术后)
               return "AfterOperationContent";
            case OrderContentKind.TextLeaveHospital://纯医嘱(出院)
               return "LeaveHospitalOrderContent";
            default:
               throw new ArgumentException(MessageStringManager.GetString("ClsssNotImplement"));
         }
      }
      
      /// <summary>
      /// 根据指定的DataRow创建医嘱内容对象。首先将从DataRow中提取医嘱类别。
      /// </summary>
      /// <param name="sourceRow">包含初始数据的医嘱DataRow</param>
      /// <returns>合适的医嘱对象</returns>
      public static OrderContent CreateOrderContent(DataRow sourceRow)
      {
         if (sourceRow == null)
            return null;

         OrderContentKind orderKind;

         if ((sourceRow != null)
            && (sourceRow.Table.Columns.Contains(ColumnNameOrderKind)))
            orderKind = (OrderContentKind)Convert.ToInt32(sourceRow[ColumnNameOrderKind]);
         else
            orderKind = OrderContentKind.Druggery; // -1; 默认创建药品医嘱
         return CreateOrderContent(orderKind, sourceRow);
      }

      /// <summary>
      /// 根据指定的医嘱类型和DataRow创建医嘱内容对象
      /// </summary>
      /// <param name="orderKind">医嘱类别</param>
      /// <param name="sourceRow">包含初始数据的医嘱DataRow</param>
      /// <returns>合适的医嘱对象</returns>
      public static OrderContent CreateOrderContent(OrderContentKind orderKind, DataRow sourceRow)
      {
         OrderContent content;
         // 根据医嘱类别+项目类别创建医嘱项目
         switch (orderKind)
         {
            case OrderContentKind.Druggery: //药品医嘱
               content = new DruggeryOrderContent(sourceRow);
               break;
            case OrderContentKind.ChargeItem://普通项目医嘱
               content = new ChargeItemOrderContent(sourceRow);
               break;
            case OrderContentKind.GeneralItem://常规医嘱
               content = new GeneralOrderContent(sourceRow);
               break;
            case OrderContentKind.ClinicItem://临床项目医嘱
               content = new ClinicItemOrderContent(sourceRow);
               break;
            case OrderContentKind.OutDruggery://出院带药
               content = new OutDruggeryContent(sourceRow);
               break;
            case OrderContentKind.Operation://手术医嘱
               content = new OperationOrderContent(sourceRow);
               break;
            //case OrderKindFlags.CeaseLong://停长期医嘱
            //   break;
            case OrderContentKind.TextNormal://纯医嘱(普通)
               content = new TextOrderContent(sourceRow);
               break;
            case OrderContentKind.TextShiftDept://纯医嘱(转科)
               content = new ShiftDeptOrderContent(sourceRow);
               break;
            case OrderContentKind.TextAfterOperation://纯医嘱(术后)
               content = new AfterOperationContent(sourceRow);
               break;
            case OrderContentKind.TextLeaveHospital://纯医嘱(出院)
               content = new LeaveHospitalOrderContent(sourceRow);
               break;
            default:
               throw new ArgumentException(MessageStringManager.GetString("ClsssNotImplement"));
         }
         content.InnerOrderKind = orderKind;
         return content;
      }
   }
}
