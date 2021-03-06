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
   /// 医嘱基类
   /// </summary> 
   public abstract class Order : EPBaseObject
   {
      #region const variables
      /// <summary>
      /// 用来判断文字医嘱是否是草药汇总信息的标志
      /// </summary>
      public static string HerbSummaryFlag = "草药汇总";
      /// <summary>
      /// 草药汇总信息的格式化串
      /// </summary>
      public static string HerbSummaryFormat = "草药 {0} 贴 {1} {2}";
      #endregion

      #region properties
      /// <summary>
      /// 医嘱序号
      /// </summary>
      public decimal SerialNo
      {
         get { return _serialNo; }
         set
         {
            if (ReadOnly)
               throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

            if (value <= 0)
               throw new ArgumentException(MessageStringManager.GetString("CommonValueIsLess", 0));

            if (_serialNo != value)
            {
               decimal oldSerialNo = _serialNo;
               _serialNo = value;
               FireOrderChanged(new OrderChangedEventArgs(value, oldSerialNo));
            }
         }
      }
      private decimal _serialNo;

      /// <summary>
      /// 首页序号
      /// TODO: 以后改成关联病人对象,由调用程序提供
      /// </summary>
      public decimal PatientId
      {
         get { return _patientId; }
         set
         {
            if (ReadOnly)
               throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

            _patientId = value;
            FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         }
      }
      private decimal _patientId;

      /// <summary>
      /// 分组序号(所在组的第一条医嘱的序号)
      /// </summary>
      public decimal GroupSerialNo
      {
         get { return _groupSerialNo; }
         set
         {
            if (ReadOnly)
               throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

            _groupSerialNo = value;
            FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         }
      }
      private decimal _groupSerialNo;

      /// <summary>
      /// 在组中的位置标记(唯一/头/中间/尾)
      /// </summary>
      public GroupPositionKind GroupPosFlag
      {
         get { return _groupPosFlag; }
         set
         {
            //if (ReadOnly)
            //   throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

            _groupPosFlag = value;

            FireOrderChanged(new OrderChangedEventArgs(SerialNo));
            if (Content != null)
               Content.ResetContentOutputText();
         }
      }
      private GroupPositionKind _groupPosFlag = GroupPositionKind.SingleOrder;

      /// <summary>
      /// 医嘱开始时间
      /// </summary>
      public DateTime StartDateTime
      {
         get { return _startDateTime; }
         set
         {
            //if (ReadOnly)
            //   throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

            _startDateTime = value;
            FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         }
      }
      private DateTime _startDateTime;

      /// <summary>
      /// 医嘱内容
      /// </summary>
      public OrderContent Content
      {
         get { return _content; }
         set
         {
            if (ReadOnly)
               throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));
            //if (value == null)
            //   throw new ArgumentException(MessageStringManager.GetString("CannotSetNullValue"));

            //if ((_content == null) || (_content.OrderKind != value.OrderKind))
            //{
            //   _content = OrderContentFactory.CreateOrderContent(
            //      value.OrderKind, null, SqlExecutor);
            //   _content._parentOrder = this;
            //}
            if (_content != null)
               _content.OrderContentChanged -= new EventHandler(DoAfterOrderContentChanged);
            _content = value;
            if (value != null)
            {
               _content._parentOrder = this;
               _content.ProcessCreateOutputeInfo = value.ProcessCreateOutputeInfo;
               _content.EndInit(); // 强制重新生成输出信息。因为value的ParentOrder和当前不一定相同，创建出来的输出信息不正确
               _content.OrderContentChanged += new EventHandler(DoAfterOrderContentChanged);
            }
            FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         }
      }
      private OrderContent _content;

      /// <summary>
      /// 创建信息
      /// </summary>
      public OrderOperateInfo CreateInfo
      {
         get { return _createInfo; }
         set
         {
            if (ReadOnly)
               throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

            _createInfo = value;
            FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         }
      }
      private OrderOperateInfo _createInfo;

      /// <summary>
      /// 审核信息
      /// </summary>
      public OrderOperateInfo AuditInfo
      {
         get { return InnerAuditInfo; }
      }
      internal OrderOperateInfo InnerAuditInfo
      {
         get { return _auditInfo; }
         set { _auditInfo = value; }
      }
      private OrderOperateInfo _auditInfo;

      /// <summary>
      /// 执行信息
      /// </summary>
      public OrderOperateInfo ExecuteInfo
      {
         get { return InnerExecuteInfo; }
      }
      internal OrderOperateInfo InnerExecuteInfo
      {
         get { return _executeInfo; }
         set { _executeInfo = value; }
      }
      private OrderOperateInfo _executeInfo;

      /// <summary>
      /// 取消信息
      /// </summary>
      public OrderOperateInfo CancelInfo
      {
         get { return InnerCancelInfo; }
      }
      internal OrderOperateInfo InnerCancelInfo
      {
         get { return _cancelInfo; }
         set
         {
            _cancelInfo = value;
            if ((_cancelInfo != null) && _cancelInfo.HadInitialized)
            {
               InnerState = OrderState.Cancellation;
               // 取消医嘱后会影响到医嘱内容的显示文字，所以强制改变医嘱输出内容
               if (Content != null)
                  Content.ResetContentOutputText();

               FireOrderChanged(new OrderChangedEventArgs(SerialNo));
            }
         }
      }
      private OrderOperateInfo _cancelInfo;

      /// <summary>
      /// 医嘱状态
      /// </summary>
      public OrderState State
      {
         get { return InnerState; }
      }
      internal OrderState InnerState
      {
         get { return _state; }
         set { _state = value; }
      }
      private OrderState _state;

      /// <summary>
      /// 录入医嘱时病人所在科室
      /// </summary>
      public Department OriginalDepartment
      {
         get { return _originalDept; }
         set
         {
            if (ReadOnly)
               throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

            _originalDept = value;
            FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         }
      }
      private Department _originalDept;

      /// <summary>
      /// 录入医嘱时病人所在病区
      /// </summary>
      public Ward OriginalWard
      {
         get { return _originalWard; }
         set
         {
            if (ReadOnly)
               throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

            _originalWard = value;
            FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         }
      }
      private Ward _originalWard;

      /// <summary>
      /// 项目的执行科室
      /// </summary>
      public Department ExecuteDept
      {
         get { return _executeDept; }
         set { _executeDept = value; }
      }
      private Department _executeDept;

      /// <summary>
      /// 同步标志(是否已同步到HIS)
      /// </summary>
      public bool HadSync
      {
         get { return _hadSync; }
         set
         {
            if (_hadSync != value)
            {
               _hadSync = value;

               if (!m_IsEditing)
                  m_IsFireAfterSetSynchFlag = true;
               //if (!value) // 由True变为False时表示数据发生改变，需要重新发送
               FireOrderChanged(new OrderChangedEventArgs(SerialNo));
               if (!m_IsEditing)
                  m_IsFireAfterSetSynchFlag = false;
            }
         }
      }
      private bool _hadSync;

      /// <summary>
      /// 医嘱内容文本
      /// </summary>
      public string Text
      {
         get
         {
            _text = Content.ToString();
            return _text;
         }
         set { _text = value; }
      }
      private string _text;

      /// <summary>
      /// 标记主键是否已初始化
      /// </summary>
      public override bool KeyInitialized
      {
         get
         {
            if (SerialNo <= 0)
               return false;
            else
               return true;
         }
      }

      /// <summary>
      /// 只读(该条医嘱是否允许修改)
      /// </summary>
      public bool ReadOnly
      {
         get
         {
            return false;
            //// 根据医嘱状态决定是否只读
            //if ((State == OrderState.None) || (State == OrderState.New))
            //   return false;
            //else
            //   return true;
         }
      }

      /// <summary>
      /// 医嘱编辑状态
      /// </summary>
      public OrderEditState EditState
      {
         get { return _editState; }
         internal set
         {
            switch (value)
            {
               case OrderEditState.Modified: // 只允许在已修改或未改变状态下
               case OrderEditState.Unchanged:
                  _editState = value;
                  break;
               default:
                  break;
            }
         }
      }
      private OrderEditState _editState = OrderEditState.Unchanged;

      /// <summary>
      /// 检查医嘱对象是否可以移除
      /// </summary>
      /// <returns></returns>
      public bool CanRemove
      {
         get
         {
            // 只有新增医嘱且不是删除、分离状态的可以移除
            if ((!ReadOnly)
               && (State == OrderState.New)
               && (EditState != OrderEditState.Deleted)
               && (EditState != OrderEditState.Detached))
               return true;
            return false;
         }
      }

      /// <summary>
      /// 获取取消信息的字符串
      /// </summary>
      public string CancelInfoText
      {
         get
         {
            if ((State == OrderState.Cancellation) && (CancelInfo != null) && (CancelInfo.HadInitialized))
            {
               if (String.IsNullOrEmpty(CancelInfo.Executor.Name))
                  CancelInfo.Executor.ReInitializeProperties();
               return String.Format(CultureInfo.CurrentCulture
                  , "取消 {0},{1}", CancelInfo.Executor.Name,CancelInfo.InnerExecuteTime);
            }
            else
               return "";
         }
      }
      #endregion

      #region private variables
      /// <summary>
      /// 标记是否正在更新属性(防止频繁调用更新事件)
      /// </summary>
      private bool m_IsEditing;
      /// <summary>
      /// 标记是在更新同步标志后触发改变事件
      /// (在修改其它属性时会默认将同步标志置为否，而初始化数据时要保留原始的同步标志值)
      /// </summary>
      private bool m_IsFireAfterSetSynchFlag;
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      protected Order()
         : base()
      {
         InnerState = OrderState.New;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      protected Order(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      #region event handle
      /// <summary>
      /// 医嘱改变事件
      /// </summary>
      public event EventHandler<OrderChangedEventArgs> OrderChanged
      {
         add
         {
            onOrderChanged += value;
         }
         remove
         {
            onOrderChanged -= value;
         }
      }
      private EventHandler<OrderChangedEventArgs> onOrderChanged;

      /// <summary>
      /// 医嘱列表改变事件
      /// </summary>
      /// <param name="e"></param>
      protected void FireOrderChanged(OrderChangedEventArgs e)
      {
         if (m_IsEditing)
            return;

         // 修改属性值
         if (EditState == OrderEditState.Unchanged)
         {
            _editState = OrderEditState.Modified;
            if (!m_IsFireAfterSetSynchFlag)
               _hadSync = false;
         }

         if (onOrderChanged != null)
            onOrderChanged(this, e);
      }

      /// <summary>
      /// 处理医嘱内容改变事件(用来同步触发医嘱的改变事件)
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void DoAfterOrderContentChanged(object sender, EventArgs e)
      {
         //if (KeyInitialized)
         FireOrderChanged(new OrderChangedEventArgs(SerialNo));
      }
      #endregion

      #region public methods
      /// <summary>
      /// 返回医嘱内容
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         // 直接返回医嘱内容
         if (Content == null)
            return "";

         return Content.ToString();
      }

      /// <summary>
      /// 提交自上次调用 AcceptChanges 以来对该医嘱进行的所有更改
      /// </summary>
      /// <remarks>在调用 AcceptChanges 时，EndEdit 方法被隐式调用，以便终止任何编辑。如果行的 EditState 是“已添加”或“已修改”，则 EditState 变成“未更改”。如果 EditState 是“删除”，则该行将被移除</remarks>
      /// TODO: 删除还未实现
      public void AcceptChanges()
      {
         EndInit();
         if ((EditState != OrderEditState.Detached)
            && (EditState != OrderEditState.Deleted))
            _editState = OrderEditState.Unchanged;
         else
         { }
      }

      /// <summary>
      /// 被加入到医嘱对象集合中,更新医嘱状态
      /// </summary>
      public void Added()
      {
         _editState = OrderEditState.Added;
         InnerState = OrderState.New;
      }

      /// <summary>
      /// 删除自己
      /// </summary>
      public void Delete()
      {
         // 如果医嘱已经存在数据库中了并且是新医嘱，则标记为已删除
         // 如果已经被删除了或未加到医嘱列表中则不处理
         // 如果是新增的，则应该由医嘱列表来删除，这里处理不了
         if (((EditState == OrderEditState.Modified)
            || (EditState == OrderEditState.Unchanged))
            && (State == OrderState.New))
            _editState = OrderEditState.Deleted;
      }

      /// <summary>
      /// 审核医嘱
      /// </summary>
      /// <param name="auditorCode"></param>
      /// <param name="auditTime"></param>
      /// <returns></returns>
      public bool AuditOrder(string auditorCode, DateTime auditTime)
      {
         if (ReadOnly)
            throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));
         // 审核前可以添加一些校验：是否需要审核、操作员权限是否够等？？？
         if (_auditInfo == null)
            _auditInfo = new OrderOperateInfo(auditorCode, auditTime);
         else
            _auditInfo.SetPropertyValue(auditorCode, auditTime);

         InnerState = OrderState.Audited;

         EditState = OrderEditState.Modified;

         FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         return true;
      }

      /// <summary>
      /// 执行医嘱
      /// </summary>
      /// <param name="executorCode"></param>
      /// <param name="executeTime"></param>
      /// <returns></returns>
      public bool ExecuteOrder(string executorCode, DateTime executeTime)
      {
         if (ReadOnly)
            throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

         if (_executeInfo == null)
            _executeInfo = new OrderOperateInfo(executorCode, executeTime);
         else
            _executeInfo.SetPropertyValue(executorCode, executeTime);

         InnerState = OrderState.Executed;

         EditState = OrderEditState.Modified;

         FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         return true;
      }

      /// <summary>
      /// 取消医嘱
      /// </summary>
      /// <param name="cancellerCode"></param>
      /// <param name="executeTime"></param>
      /// <returns></returns>
      public bool CancelOrder(string cancellerCode, DateTime executeTime)
      {
         if (ReadOnly)
            throw new InvalidOperationException(MessageStringManager.GetString("ObjectIsReadOnly"));

         InnerCancelInfo = new OrderOperateInfo(cancellerCode, executeTime);

         // FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         return true;
      }

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         if (CreateInfo != null)
            CreateInfo.ReInitializeAllProperties();
         if (AuditInfo != null)
            AuditInfo.ReInitializeAllProperties();
         if (ExecuteInfo != null)
            ExecuteInfo.ReInitializeAllProperties();
         if (CancelInfo != null)
            CancelInfo.ReInitializeAllProperties();
         if (OriginalDepartment != null)
            OriginalDepartment.ReInitializeAllProperties();
         if (OriginalWard != null)
            OriginalWard.ReInitializeAllProperties();
         if (ExecuteDept != null)
            ExecuteDept.ReInitializeAllProperties();
      }
      #endregion

      #region ISupportInitialize Members

      /// <summary>
      /// 
      /// </summary>
      public override void BeginInit()
      {
         m_IsEditing = true;
         m_IsFireAfterSetSynchFlag = true;
      }

      /// <summary>
      /// 
      /// </summary>
      public override void EndInit()
      {
         m_IsEditing = false;
         FireOrderChanged(new OrderChangedEventArgs(SerialNo));
         m_IsFireAfterSetSynchFlag = false;
         _editState = OrderEditState.Unchanged;
      }

      #endregion
   }
}
