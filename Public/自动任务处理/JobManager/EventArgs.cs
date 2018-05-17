using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using System.Collections.ObjectModel;

namespace DrectSoft.JobManager
{
   //public delegate void AdjustIntervalEventHander(object sender, IntervalEventArgs e);

   /// <summary>
   /// 同步事件参数
   /// </summary>
   public class JobExecuteInfoArgs : EventArgs
   {
      #region properties
      private string _tableName;
      /// <summary>
      /// 表名
      /// </summary>
      public string TableName
      {
         get { return _tableName; }
      }

      private int _recordsCount;
      /// <summary>
      /// 记录总数
      /// </summary>
      public int RecordsCount
      {
         get { return _recordsCount; }
      }

      private int _changedCount;
      /// <summary>
      /// 改变的记录数
      /// </summary>
      public int ChangedCount
      {
         get { return _changedCount; }
      }

      private DateTime _startTime;
      /// <summary>
      /// 开始时间
      /// </summary>
      public DateTime StartTime
      {
         get { return _startTime; }
      }

      private bool _success;
      /// <summary>
      /// 成功标志
      /// </summary>
      public bool Success
      {
         get { return _success; }
      }

      private object _tag;
      /// <summary>
      /// 其他信息
      /// </summary>
      public object Tag
      {
         get { return _tag; }
         set { _tag = value; }
      }

      private string _memo;
      /// <summary>
      /// 备注
      /// </summary>
      public string Memo
      {
         get { return _memo; }
         set { _memo = value; }
      }

      private TraceLevel _level;
      /// <summary>
      /// 本次同步的信息级别
      /// </summary>
      public TraceLevel Level
      {
         get { return _level; }
         set { _level = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public Job Sender
      {
         get { return _sender; }
      }
      private Job _sender;
      #endregion
      
      /// <summary>
      /// 带参构造
      /// </summary>
      /// <param name="tableName"></param>
      /// <param name="recordsCount"></param>
      /// <param name="changedCount"></param>
      /// <param name="startTime"></param>
      /// <param name="success"></param>
      public JobExecuteInfoArgs(Job sender, string tableName, int recordsCount, int changedCount
         , DateTime startTime, bool success, string memo, TraceLevel level)
      {
         _sender = sender;
         _tableName = string.IsNullOrEmpty(tableName) ? "系统" : tableName;
         _changedCount = changedCount;
         _recordsCount = recordsCount;
         _startTime = startTime;
         _success = success;
         _memo = memo;
         _level = level;
      }

      /// <summary>
      /// 无参构造
      /// </summary>
      public JobExecuteInfoArgs(Job sender)
         : this(sender, "（信息）", 0, 0, DateTime.Now, true, "无", TraceLevel.Info)
      { }

      /// <summary>
      /// 普通信息构造
      /// </summary>
      /// <param name="info"></param>
      /// <param name="level"></param>
      public JobExecuteInfoArgs(Job sender, string info, TraceLevel level)
         : this(sender, "（信息）", 0, 0, DateTime.Now, true, info, level)
      { }

      /// <summary>
      /// 专用于错误信息的构造
      /// </summary>
      /// <param name="error"></param>
      public JobExecuteInfoArgs(Job sender, string error)
         : this(sender, "（错误）", 0, 0, DateTime.Now, false, error, TraceLevel.Error)
      { }

      /// <summary>
      /// 专用于错误信息的构造(暂时不开堆栈信息，否则记录太大)
      /// </summary>
      /// <param name="tableName"></param>
      /// <param name="err"></param>
      public JobExecuteInfoArgs(Job sender, string tableName, Exception err)
         : this(sender, "（错误）", 0, 0, DateTime.Now, false,  err.Message+err.StackTrace
                  + Environment.NewLine + "类型：" + err.TargetSite.DeclaringType
                  + Environment.NewLine + "方法：" + err.TargetSite.Name
                  + Environment.NewLine + "所属程序集：" + err.Source
         //+ Environment.NewLine + "堆栈信息：" + err.StackTrace
         , TraceLevel.Error)
      { }
   }

   //public class EnableEventArgs : EventArgs
   //{
   //   private bool _value;
   //   /// <summary>
   //   /// 
   //   /// </summary>
   //   public bool Value
   //   {
   //      get { return _value; }
   //      set { _value = value; }
   //   }

   //   public EnableEventArgs(bool value)
   //   {
   //      _value = value;
   //   }
   //}

   //public class IntervalEventArgs : EventArgs
   //{
   //   private decimal _interval;
   //   /// <summary>
   //   /// 时间间隔值
   //   /// </summary>
   //   public decimal Interval
   //   {
   //      get { return _interval; }
   //      set { _interval = value; }
   //   }

   //   private object _sender;
   //   /// <summary>
   //   /// 事件发送参数
   //   /// </summary>
   //   public object Sender
   //   {
   //      get { return _sender; }
   //      set { _sender = value; }
   //   }
   //}

   //public class SynchTreeViewEventArgs : EventArgs
   //{
   //   private Collection<TreeNode> _selectedNodes;
   //   /// <summary>
   //   /// 被勾选的树节点
   //   /// </summary>
   //   public Collection<TreeNode> SelectedNodes
   //   {
   //      get { return _selectedNodes; }
   //   }

   //   public SynchTreeViewEventArgs(Collection<TreeNode> selectedNodes)
   //   {
   //      _selectedNodes = selectedNodes;
   //   }
   //}

   //public class ScheduleSettingEventArgs : EventArgs
   //{
   //   /// <summary>
   //   /// 
   //   /// </summary>
   //   public JobPlan Schedule
   //   {
   //      get { return _schedule; }
   //   }
   //   private JobPlan _schedule;

   //   public ScheduleSettingEventArgs(JobPlan schedule)
   //   {
   //      _schedule = schedule;
   //   }
   //}

   public class SearchSettingEventArgs : EventArgs
   {
      private SearchParameter _parameter;
      /// <summary>
      /// 获得或设置事件参数
      /// </summary>
      public SearchParameter Parameter
      {
         get { return _parameter; }
         set { _parameter = value; }
      }

      public SearchSettingEventArgs(SearchParameter parameter)
      {
         _parameter = parameter;
      }

   }

   public class SearchEventArgs : EventArgs
   {
      private int _index;
      /// <summary>
      /// 索引
      /// </summary>
      public int Index
      {
         get { return _index; }
         set { _index = value; }
      }

      public SearchEventArgs(int index)
      {
         _index = index;
      }
   }
}
