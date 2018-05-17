using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 医嘱编辑状态枚举
   /// </summary>
   [Flags]
   public enum OrderEditState
   {
      /// <summary>
      /// 已被加到医嘱列表中，还未存到数据库
      /// </summary>
      Added = 4, 
      /// <summary>
      /// 已从医嘱列表中删除，还未存到数据库
      /// </summary>
      Deleted = 8, 
      /// <summary>
      /// 医嘱已创建，但不属于任何医嘱集合
      /// </summary>
      Detached = 1, 
      /// <summary>
      /// 已被修改，还未存到数据库
      /// </summary>
      Modified = 16, 
      /// <summary>
      /// 未做任何修改
      /// </summary>
      Unchanged = 2 
   }

   /// <summary>
   /// 输出内容类型标志(便于输出控制)
   /// </summary>
   public enum OrderOutputTextType
   {
      /// <summary>
      /// 普通文本
      /// </summary>
      NormalText = 99, 
      /// <summary>
      /// 项目名称(含规格)
      /// </summary>
      ItemName = 0, 
      /// <summary>
      /// 项目数量(含单位)
      /// </summary>
      ItemAmount = 1, 
      /// <summary>
      /// 项目用法名称
      /// </summary>
      ItemUsage = 2, 
      /// <summary>
      /// 项目频次名称
      /// </summary>
      ItemFrequency = 3, 
      /// <summary>
      /// 项目天数
      /// </summary>
      ItemDays = 4, 
      /// <summary>
      /// 嘱托内容
      /// </summary>
      EntrustContent = 5, 
      /// <summary>
      /// 取消信息(含取消者名字)
      /// </summary>
      CancelInfo = 6, 
      /// <summary>
      /// 分组开始标记
      /// </summary>
      GroupStart = 7, 
      /// <summary>
      /// 分组中间标记
      /// </summary>
      GroupMiddle = 8, 
      /// <summary>
      /// 分组结束标记
      /// </summary>
      GroupEnd = 9,
      /// <summary>
      /// 自备
      /// </summary>
      SelfProvide = 10
   }
}
