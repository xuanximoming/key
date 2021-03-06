using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;



namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 医嘱内容基类。不同的医嘱其医嘱内容有很大差别，处理方式也各不相同。
   /// 所以抽象出医嘱内容基类，在继承类中实现各自的处理方式、显示控制及特有属性。
   /// 现在每一个医嘱类别对应一个医嘱内容子类
   /// </summary> 
   public abstract class OrderContent : EPBaseObject
   {
      /// <summary>
      /// 组合字段时使用的分隔符
      /// </summary>
      public static string CombFlag = "///";

      #region properties
      /// <summary>
      /// 医嘱项目
      /// </summary>
      public ItemBase Item
      {
         get { return _item; }
         set
         {
            _item = value;
            FireOrderContentChanged(new EventArgs());
         }
      }
      private ItemBase _item;

      /// <summary>
      /// 项目数量
      /// </summary>
      public decimal Amount
      {
         get { return _amount; }
         set
         {
            _amount = value;
            FireOrderContentChanged(new EventArgs());
         }
      }
      private decimal _amount;

      /// <summary>
      /// 当前使用的单位
      /// </summary>
      public ItemUnit CurrentUnit
      {
         get { return _currentUnit; }
         set
         {
            _currentUnit = value;
            FireOrderContentChanged(new EventArgs());
         }
      }
      private ItemUnit _currentUnit;

      /// <summary>
      /// 用法对象
      /// </summary>
      public OrderUsage ItemUsage
      {
         get { return _itemUsage; }
         set
         {
            _itemUsage = value;
            FireOrderContentChanged(new EventArgs());
         }
      }
      private OrderUsage _itemUsage;

      /// <summary>
      /// 频次对象
      /// </summary>
      public OrderFrequency ItemFrequency
      {
         get { return _itemFrequency; }
         set
         {
            _itemFrequency = value;
            FireOrderContentChanged(new EventArgs());
         }
      }
      private OrderFrequency _itemFrequency;

      /// <summary>
      /// 医嘱类别
      /// </summary>
      public OrderContentKind OrderKind
      {
         get
         {
            // TODO: 医嘱类别应该根据当前医嘱内容计算得到

            return InnerOrderKind;
         }
      }
      internal OrderContentKind InnerOrderKind
      {
         get { return _orderKind; }
         set { _orderKind = value; }
      }
      private OrderContentKind _orderKind;

      /// <summary>
      /// 特殊标记
      /// </summary>
      public OrderAttributeFlag Attributes
      {
         get { return _attributes; }
         set
         {
            _attributes = value;
            FireOrderContentChanged(new EventArgs());
         }
      }
      private OrderAttributeFlag _attributes;

      /// <summary>
      /// 嘱托
      /// </summary>
      public string EntrustContent
      {
         get { return _entrustContent; }
         set
         {
            _entrustContent = value;
            FireOrderContentChanged(new EventArgs());
         }
      }
      private string _entrustContent;

      /// <summary>
      /// 关联的医嘱对象
      /// </summary>
      public Order ParentOrder
      {
         get { return _parentOrder; }
      }
      internal Order _parentOrder;

      /// <summary>
      /// 该内容的医嘱是否可以停止（一般药品、项目的长期医嘱才可以设停止时间）
      /// </summary>
      public abstract bool CanCeased
      { get; }

      /// <summary>
      /// 无主键，永远是False
      /// </summary>
      public override bool KeyInitialized
      {
         get { return false; }
      }

      /// <summary>
      /// 当前医嘱对应的输出内容(CustomDraw时使用)
      /// </summary>
      public Collection<OutputInfoStruct> Outputs
      {
         get
         {
            if ((_outputs == null) || (_outputs.Count == 0))
               GenerateOutputs();

            return _outputs;
         }
      }
      private Collection<OutputInfoStruct> _outputs;

      /// <summary>
      /// 处理输出信息创建
      /// </summary>
      public GenerateOutputInfo ProcessCreateOutputeInfo
      {
         get { return _processCreateOutputeInfo; }
         set { _processCreateOutputeInfo = value; }
      }
      private GenerateOutputInfo _processCreateOutputeInfo;

      ///// <summary>
      ///// 标记是否正在修改属性值
      ///// </summary>
      //private int m_IsEditing;

      /// <summary>
      /// 当前可显示的内容集合
      /// </summary>
      protected Collection<OutputInfoStruct> Texts
      {
         get
         {
            if (_texts == null)
               ResetDisplayTexts();
            return _texts;
         }
      }
      private Collection<OutputInfoStruct> _texts;
      #endregion

      #region ctors
      /// <summary>
      /// Inherited constructor
      /// </summary>
      protected OrderContent()
         : base()
      { }

      //public OrderContent(object code, IDataAccess sqlExecutor)
      //{
      //   SqlExecutor = sqlExecutor;
      //   Initialize(code);
      //}

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      protected OrderContent(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      private bool m_IsEditing;

      #region event handler
      /// <summary>
      /// 医嘱内容改变事件
      /// </summary>
      public event EventHandler OrderContentChanged
      {
         add
         {
            onOrderContentChanged = (EventHandler)Delegate.Combine(onOrderContentChanged, value);
         }
         remove
         {
            onOrderContentChanged = (EventHandler)Delegate.Remove(onOrderContentChanged, value);
         }
      }
      private EventHandler onOrderContentChanged;

      /// <summary>
      /// 医嘱列表改变事件
      /// </summary>
      /// <param name="e"></param>
      protected void FireOrderContentChanged(EventArgs e)
      {
         if (m_IsEditing)
            return;

         // 重设可显示内容和输出内容列表
         ResetContentOutputText();

         if (onOrderContentChanged != null)
            onOrderContentChanged(this, e);
      }
      #endregion

      /// <summary>
      /// 输出信息创建方法委托
      /// </summary>
      /// <param name="texts"></param>
      /// <returns></returns>
      public delegate Collection<OutputInfoStruct> GenerateOutputInfo(Collection<OutputInfoStruct> texts);

      /// <summary>
      /// 生成输出内容
      /// </summary>
      private void GenerateOutputs()
      {
         if (_processCreateOutputeInfo != null)
            _outputs = _processCreateOutputeInfo(Texts);
         else
            _outputs = new Collection<OutputInfoStruct>();
      }

      /// <summary>
      /// 重设当前可显示的内容,为各项内容间加上空格
      /// </summary>
      protected virtual void ResetDisplayTexts()
      {
         InitBaseDisplayTexts();
      }

      /// <summary>
      /// 创建并初始化基本的显示内容
      /// </summary>
      protected void InitBaseDisplayTexts()
      {
         if (_texts == null)
            _texts = new Collection<OutputInfoStruct>();
         else if (_texts.Count > 0)
            _texts.Clear();
         // 添加取消信息和分组标记
         if (ParentOrder != null)
         {
            // 分组标记的位置会重新计算
            switch (ParentOrder.GroupPosFlag)
            {
               case GroupPositionKind.GroupStart:
                  _texts.Add(new OutputInfoStruct("", OrderOutputTextType.GroupStart));
                  break;
               case GroupPositionKind.GroupMiddle:
                  _texts.Add(new OutputInfoStruct("", OrderOutputTextType.GroupMiddle));
                  break;
               case GroupPositionKind.GroupEnd:
                  _texts.Add(new OutputInfoStruct("", OrderOutputTextType.GroupEnd));
                  break;
            }
            if ((ParentOrder.State == OrderState.Cancellation) && (!String.IsNullOrEmpty(ParentOrder.CancelInfoText)))
               _texts.Add(new OutputInfoStruct(ParentOrder.CancelInfoText, OrderOutputTextType.CancelInfo));
         }
      }

      /// <summary>
      /// 重设可显示内容和输出内容列表
      /// </summary>
      public void ResetContentOutputText()
      {
         ResetDisplayTexts();
         GenerateOutputs();
      }

      /// <summary>
      /// 默认以"项目[规格], 数量 单位, 用法, 频次, 嘱托"的形式返回医嘱内容
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         StringBuilder result = new StringBuilder();
         foreach (OutputInfoStruct info in Texts)
         {
            switch (info.OutputType)
            {
               case OrderOutputTextType.CancelInfo:
               case OrderOutputTextType.GroupStart:
               case OrderOutputTextType.GroupMiddle:
               case OrderOutputTextType.GroupEnd:
                  break;
               default:
                  result.Append(info.Text);
                  break;
            }
         }
         return result.ToString();
      }

      /// <summary>
      /// 检查医嘱内容的属性是否有效。
      /// </summary>
      /// <returns>检查结果。空串表示无问题</returns>
      public abstract string CheckProperties();

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         if (Item != null)
            Item.ReInitializeAllProperties();
         if (ItemUsage != null)
            ItemUsage.ReInitializeAllProperties();
         if (ItemFrequency != null)
            ItemFrequency.ReInitializeAllProperties();
      }

      #region ISupportInitialize Members

      /// <summary>
      /// 
      /// </summary>
      public override void BeginInit()
      {
         m_IsEditing = true;
      }

      /// <summary>
      /// 
      /// </summary>
      public override void EndInit()
      {
         m_IsEditing = false;
         FireOrderContentChanged(new EventArgs());
      }

      #endregion
   }
}
