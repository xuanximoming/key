using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DrectSoft.Common.Eop;
using System.Globalization;
using System.Collections.ObjectModel;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 医嘱对象表的视图
   /// (OrderTableView和OrderCollection、OrderTable设计还不是很好，有些地方处理不够清晰！！！)
   /// </summary>
   public sealed class OrderTableView : IBindingList, IList, ICollection, IEnumerable, ISupportInitialize
   {
      #region private fields
      /// <summary>
      /// 有效医嘱的视图列表
      /// </summary>
      private List<OrderView> InnerList
      {
         get
         {
            if (_innerList == null)
            {
               ResetInnerList();
            }

            return _innerList;
         }
      }
      private List<OrderView> _innerList;
      #endregion

      #region custom public properties
      /// <summary>
      /// 关联的医嘱对象表
      /// </summary>
      public OrderTable Table
      {
         get { return _table; }
         //set { 暂未实现更换对应Table的功能}
      }
      private OrderTable _table;

      /// <summary>
      /// 指定视图显示何种状态的医嘱(-1表示全显示)
      /// </summary>
      public OrderState State
      {
         get { return _state; }
         set
         {
            if (_state != value)
            {
               // 状态改变时清空医嘱缓存
               _state = value;

               // 根据当前设置的状态调整AllowNew属性
               AllowNew = ((value == OrderState.All) || (value == OrderState.New));
               AllowRemove = AllowNew;

               FireViewListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
         }
      }
      private OrderState _state;

      public OrderView this[int index]
      {
         get
         {
            return GetOrderView(index);
         }
      }
      #endregion

      #region private variables
      /// <summary>
      /// 标记是否正在更新属性(防止频繁调用更新事件)
      /// </summary>
      private bool m_IsEditing;
      /// <summary>
      /// 标记是否展开全部草药明细，
      /// true: 全展
      /// false：全收（还要根据下一参数才能决定是否要折叠明细）
      /// </summary>
      private bool m_ExpandAllHerbDetail;
      /// <summary>
      /// 记录所有展开的草药明细的分组序号
      /// </summary>
      private Collection<decimal> m_GroupSerialNoOfExpandedHerbs;
      #endregion

      #region IBindingList properties
      /// <summary>
      /// 允许编辑
      /// </summary>
      public bool AllowEdit
      {
         get { return _allowEdit; }
         set { _allowEdit = value; }
      }
      private bool _allowEdit;

      /// <summary>
      /// 允许添加医嘱
      /// </summary>
      public bool AllowNew
      {
         get { return _allowNew; }
         set { _allowNew = value; }
      }
      private bool _allowNew;

      /// <summary>
      /// 允许删除医嘱
      /// </summary>
      public bool AllowRemove
      {
         get { return _allowRemove; }
         set { _allowRemove = value; }
      }
      private bool _allowRemove;

      public bool IsSorted
      {
         get { return true; }
      }

      public ListSortDirection SortDirection
      {
         get { return ListSortDirection.Ascending; }
      }

      public PropertyDescriptor SortProperty
      {
         get { return null; }
      }

      public bool SupportsSorting
      {
         get { return false; }
      }

      public bool SupportsChangeNotification
      {
         get { return true; }
      }

      public bool SupportsSearching
      {
         get { return true; }
      }
      #endregion

      #region IList properties
      public bool IsFixedSize
      {
         get { return false; }
      }

      public bool IsReadOnly
      {
         get { return false; }
      }

      object IList.this[int index]
      {
         get { return GetOrderView(index); }
         set
         {
            throw new NotSupportedException();
         }
      }
      #endregion

      #region ICollection propteties
      public int Count
      {
         get { return InnerList.Count; }
      }

      public bool IsSynchronized
      {
         get { return false; }
      }

      public object SyncRoot
      {
         get { return this; }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 根据传入的医嘱表数据创建临时或长期医嘱的OrderView对象
      /// </summary>
      /// <param name="orderTable">医嘱对象表</param>
      public OrderTableView(OrderTable orderTable)
      {
         if (orderTable == null)
            throw new ArgumentNullException(ConstMessages.ExceptionNullOrderTable);

         _table = orderTable;
         _state = OrderState.All;
         _allowNew = true;
         _allowEdit = true;
         _allowRemove = true;

         m_ExpandAllHerbDetail = !CoreBusinessLogic.BusinessLogic.AutoHideHerbDetail;
         m_GroupSerialNoOfExpandedHerbs = new Collection<decimal>();

         Table.ListChanged += new ListChangedEventHandler(DoAfterOrderListChanged);
      }
      #endregion

      #region custom event handler
      /// <summary>
      /// 医嘱表视图的List改变事件
      /// </summary>
      public event ListChangedEventHandler ListChanged
      {
         add
         {
            onListChanged = (ListChangedEventHandler)Delegate.Combine(onListChanged, value);
         }
         remove
         {
            onListChanged = (ListChangedEventHandler)Delegate.Remove(onListChanged, value);
         }
      }
      private ListChangedEventHandler onListChanged;

      private void FireViewListChanged(ListChangedEventArgs e)
      {
         // 同步InnerList(将此步操作放在前面执行，以便View的list和实际的list随时保持一致)
         ResetInnerList();

         if (m_IsEditing)
            return;

         if (onListChanged != null)
            onListChanged(this, e);
      }
      #endregion

      #region public custom Methods
      /// <summary>
      /// 添加新行
      /// </summary>
      /// <returns></returns>
      public OrderView AddNew()
      {
         OrderView view;
         Order newOrder;
         try
         {
            if (!AllowNew)
            {
               throw new ArgumentException(ConstMessages.ExceptionNotAllowAddNew);
            }

            newOrder = Table.NewOrder();
            Table.AddOrder(newOrder);

            FireViewListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, Count - 1));
            view = this[Count - 1];
         }
         finally
         {
         }
         return view;
      }

      /// <summary>
      /// 删除指定位置的医嘱及其视图
      /// </summary>
      /// <param name="index"></param>
      public void Delete(int index)
      {
         int actualIndex = Table.Orders.IndexOf(InnerList[index].SerialNo);
         // 允许删除最近新增的医嘱
         //if ((m_AddNewOrder != null) && (index == Count))
         if (InnerList[index].OrderCache.EditState == OrderEditState.Added)
         {
            Table.Orders.RemoveAt(actualIndex);
         }
         else
         {
            if (!AllowRemove)
            {
               throw new ArgumentException(ConstMessages.ExceptionNotAllowDeleteOrder);
            }
            Table.Orders.RemoveAt(actualIndex);
         }
         FireViewListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
      }

      /// <summary>
      /// 查找指定序号的医嘱在列表中的位置
      /// </summary>
      /// <param name="serialNo"></param>
      /// <returns></returns>
      public int IndexOf(decimal serialNo)
      {
         for (int index = 0; index < Count; index++)
            if (InnerList[index].SerialNo == serialNo)
               return index;

         return -1;
      }

      /// <summary>
      /// 获取视图中指定序号的对象在原始对象列表中的位置
      /// </summary>
      /// <param name="index">视图中对象的位置</param>
      /// <returns>在原始对象中的位置</returns>
      public int GetOriginalIndex(int index)
      {
         if (index < 0)
            return -1;
         if (Count <= index)
            return Table.Orders.Count;

         return Table.Orders.IndexOf(InnerList[index].SerialNo);
      }

      /// <summary>
      /// 将指定位置的记录移动到新位置
      /// </summary>
      /// <param name="oldIndex">记录原先的位置</param>
      /// <param name="newIndex">记录要移动到的位置</param>
      public void Move(int oldIndex, int newIndex)
      {
         if ((oldIndex < 0) || (oldIndex >= Count))
            throw new ArgumentOutOfRangeException();
         if ((newIndex < -1) || (newIndex > Count))
            throw new ArgumentOutOfRangeException();

         if (oldIndex == newIndex)
            return;
         // 将View中的索引位置转换成实际的索引位置，然后使用Table的方法进行移位
         Table.MoveOrder(GetOriginalIndex(oldIndex), GetOriginalIndex(newIndex));
      }

      /// <summary>
      /// 展开指定位置医嘱关联的草药明细
      /// </summary>
      /// <param name="index"></param>
      public void ExpandHerbDetail(int index)
      {
         // 应该在索引范围内
         if ((index < 0) || (index >= Count))
            throw new IndexOutOfRangeException();

         TextOrderContent textContent = this[index].OrderCache.Content as TextOrderContent;
         // 应该是草药的汇总信息
         // 将关联序号插入可显示的列表，重新生成list
         if ((textContent != null) && (textContent.IsSummaryOfHerbDetail))
         {
            if (!m_GroupSerialNoOfExpandedHerbs.Contains(textContent.GroupSerialNoOfLinkedHerbs))
               m_GroupSerialNoOfExpandedHerbs.Add(textContent.GroupSerialNoOfLinkedHerbs);
            FireViewListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
         }
      }

      /// <summary>
      /// 展开所有草药明细
      /// </summary>
      public void ExpandAllHerbDetail()
      {
         m_ExpandAllHerbDetail = true;
         FireViewListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      }

      /// <summary>
      /// 折叠指定位置的草药明细
      /// </summary>
      /// <param name="index"></param>
      public void CollapseHerbDetail(int index)
      {
         // 应该在索引范围内
         if ((index < 0) || (index >= Count))
            throw new IndexOutOfRangeException();

         // 应该是草药的明细信息
         // 将关联序号从可显示的列表中移除，重新生成list
         if ((this[index].OrderCache.Content.Item.Kind == ItemKind.HerbalMedicine)
            && (this[index].GroupPosFlag != GroupPositionKind.SingleOrder))
         {
            if (m_GroupSerialNoOfExpandedHerbs.Contains(this[index].GroupSerialNo))
               m_GroupSerialNoOfExpandedHerbs.Remove(this[index].GroupSerialNo);
            FireViewListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
         }
      }

      /// <summary>
      /// 折叠所有草药明细
      /// </summary>
      public void CollapseAllHerbDetail()
      {
         m_ExpandAllHerbDetail = false;
         m_GroupSerialNoOfExpandedHerbs.Clear();
         FireViewListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      }
      #endregion

      #region IBindingList Members
      void IBindingList.AddIndex(PropertyDescriptor property)
      {
         throw new NotSupportedException();
      }

      object IBindingList.AddNew()
      {
         return AddNew();
      }

      void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
      {
         throw new NotSupportedException();
      }

      int IBindingList.Find(PropertyDescriptor property, object key)
      {
         if (property != null)
         {
            throw new NotSupportedException();
         }
         return -1;
      }

      void IBindingList.RemoveIndex(PropertyDescriptor property)
      {
         throw new NotSupportedException();
      }

      void IBindingList.RemoveSort()
      {
         throw new NotSupportedException();
      }

      #endregion

      #region IList Members

      int IList.Add(object value)
      {
         if (value == null)
         {
            AddNew();
            return (Count - 1);
         }
         throw new ArgumentException(ConstMessages.ExceptionInsertError);
      }

      void IList.Clear()
      {
         throw new NotImplementedException();
      }

      bool IList.Contains(object value)
      {
         //Order order = value as Order;
         //if (value != null)
         //{
         //   return Table.Orders.Contains(order.SerialNo);
         //}
         return false;
      }

      int IList.IndexOf(object value)
      {
         OrderView view = value as OrderView;
         if (value != null)
         {
            return InnerList.IndexOf(view);
         }
         return -1;
      }

      void IList.Insert(int index, object value)
      {
         throw new ArgumentException(ConstMessages.ExceptionInsertError);
      }

      void IList.Remove(object value)
      {
         ((IList)this).RemoveAt(((IList)this).IndexOf(value));
      }

      void IList.RemoveAt(int index)
      {
         Delete(index);
      }

      #endregion

      #region ICollection Members
      void ICollection.CopyTo(Array array, int index)
      {
         throw new NotSupportedException();
         //for (int num1 = 0; num1 < Count; num1++)
         //{
         //   // 暂未实现数组复制功能！！！
         //   array.SetValue(new Order(m_ViewOrders[num1].  this, num1), (int)(num1 + index));
         //}
      }
      #endregion

      #region IEnumerable Members
      public IEnumerator GetEnumerator()
      {
         return InnerList.GetEnumerator();
      }
      #endregion

      #region private methods
      /// <summary>
      /// 医嘱列表改变时，要同步其对应的医嘱视图列表
      /// </summary>
      /// <param name="Sender"></param>
      /// <param name="e"></param>
      private void DoAfterOrderListChanged(object Sender, ListChangedEventArgs e)
      {
         FireViewListChanged(e);
      }

      /// <summary>
      /// 按照当前设定的医嘱状态重置InnerList
      /// </summary>
      private void ResetInnerList()
      {
         if (_innerList == null)
            _innerList = new List<OrderView>();
         else
            _innerList.Clear();

         bool isHerbDetail; // 标记是否是草药明细
         //bool isHerbSummary; // 标记是否是草药汇总
         TextOrderContent textContent;

         foreach (OrderView view in Table.Orders.OrderViewList)
         {
            if ((view.OrderCache.EditState == OrderEditState.Deleted)
               || (view.OrderCache.EditState == OrderEditState.Detached)
               || ((State != OrderState.All) && (view.OrderCache.State != State)))
               continue;

            isHerbDetail = (view.OrderCache.Content.Item != null)
               && (view.OrderCache.Content.Item.Kind == ItemKind.HerbalMedicine)
               && (view.OrderCache.GroupPosFlag != GroupPositionKind.SingleOrder);// 单条记录不存在汇总信息，所以直接显示

            // 对于草药记录要做特殊处理
            //   全展时不显示汇总记录不强制收起来
            //   全收时不显示明细记录(对于已经展开的明细)
            if (isHerbDetail)
            {
               if (m_ExpandAllHerbDetail)
               {
                  _innerList.Add(view);
                  if (!m_GroupSerialNoOfExpandedHerbs.Contains(view.GroupSerialNo))
                     m_GroupSerialNoOfExpandedHerbs.Add(view.GroupSerialNo);
               }
               else
               {
                  if (m_GroupSerialNoOfExpandedHerbs.Contains(view.GroupSerialNo))
                     _innerList.Add(view);
               }
            }
            else
            {
               textContent = view.OrderCache.Content as TextOrderContent;
               if ((textContent != null) && (textContent.IsSummaryOfHerbDetail))
               {
                  if ((!m_ExpandAllHerbDetail) && (!m_GroupSerialNoOfExpandedHerbs.Contains(textContent.GroupSerialNoOfLinkedHerbs)))
                     _innerList.Add(view);
               }
               else
                  _innerList.Add(view);
            }
         }
         m_ExpandAllHerbDetail = false; // 强制设成false,以便在重新刷新list时保留明细的折叠或展开状态
      }

      private OrderView GetOrderView(int index)
      {
         if ((index < 0) || (Count <= index))
            throw new IndexOutOfRangeException(index.ToString(CultureInfo.CurrentCulture));

         return InnerList[index];
      }
      #endregion

      #region ISupportInitialize Members

      public void BeginInit()
      {
         m_IsEditing = true;
      }

      public void EndInit()
      {
         m_IsEditing = false;
         FireViewListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      }

      #endregion
   }
}
