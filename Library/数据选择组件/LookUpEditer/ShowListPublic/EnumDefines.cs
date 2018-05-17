
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace DrectSoft.Common.Library
{
   #region ShowListForm's mode
   /// <summary>
   /// ShowListForm的显示模式
   /// </summary>
   [CLSCompliantAttribute(true)]
   public enum ShowListFormMode
   {
      /// <summary>
      /// 完整显示
      /// </summary>
      Full, 
      /// <summary>
      /// 简洁模式
      /// </summary>
      Concision
   }
   #endregion

   #region enum match type
   /// <summary>
   /// ShowList窗口匹配数据的模式枚举
   /// </summary>
   [CLSCompliantAttribute(true)]
   public enum ShowListMatchType
   {
      /// <summary>
      /// 完全匹配
      /// </summary>
      Full,  
      /// <summary>
      /// 前导相似
      /// </summary>
      Begin, 
      /// <summary>
      /// 模糊查询
      /// </summary>
      Any 
   }
   #endregion

   #region enum 
   /// <summary>
   /// ShowList选择窗口的调用模式
   /// </summary>
   public enum ShowListCallType
   { 
      /// <summary>
      /// 初试化ShowListBox
      /// </summary>
      Initialize,  
      /// <summary>
      /// 普通调用模式
      /// </summary>
      Normal,  
      /// <summary>
      /// 直接显示ShowListWindow模式
      /// </summary>
      ShowDirectly 
   }
   #endregion
}
