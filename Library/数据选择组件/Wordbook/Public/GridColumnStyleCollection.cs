using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// Grid列显示方案集合类
   /// </summary>
   public class GridColumnStyleCollection : ICollection, IEnumerable
   {
      #region Properties
      /// <summary>
      /// 包含的列显示样式数量
      /// </summary>
      public int Count
      {
         get
         {
            if (this._items == null)
            {
               return 0;
            }
            return this._items.Count;
         }
      }

      /// <summary>
      /// 列表尺寸是否固定
      /// </summary>
      public bool IsFixedSize
      {
         get
         {
            return ((IList)this.InnerList).IsFixedSize;
         }
      }

      /// <summary>
      /// 是否只读
      /// </summary>
      public bool IsReadOnly
      {
         get
         {
            return ((IList)this.InnerList).IsReadOnly;
         }
      }

      //[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      /// <summary>
      /// 对 ICollection 的访问是否同步（线程安全）。
      /// </summary>
      public bool IsSynchronized
      {
         get
         {
            return ((ICollection)this.InnerList).IsSynchronized;
         }
      }

      /// <summary>
      /// 通过索引号查找列的显示样式对象
      /// </summary>
      /// <param name="index"></param>
      /// <returns></returns>
      public GridColumnStyle this[int index]
      {
         get
         {
            return (GridColumnStyle)this.GetParameter(index);
         }
         set
         {
            this.SetParameter(index, value);
         }
      }

      //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
      /// <summary>
      /// 通过名称查找列的显示样式对象
      /// </summary>
      /// <param name="fieldName"></param>
      /// <returns></returns>
      public GridColumnStyle this[string fieldName]
      {
         get
         {
            return (GridColumnStyle)this.GetParameter(fieldName);
         }
         set
         {
            this.SetParameter(fieldName, value);
         }
      }

      /// <summary>
      /// 可用于同步对 ICollection 的访问的对象
      /// </summary>
      public object SyncRoot
      {
         get
         {
            return ((ICollection)this.InnerList).SyncRoot;
         }
      }
      #endregion

      #region private variables & properties
      //private bool _isDirty;
      private List<GridColumnStyle> _items;
      private static Type ItemType = typeof(GridColumnStyle);

      private List<GridColumnStyle> InnerList
      {
         get
         {
            List<GridColumnStyle> list1 = this._items;
            if (list1 == null)
            {
               list1 = new List<GridColumnStyle>();
               this._items = list1;
            }
            return list1;
         }
      }

      //private bool IsDirty
      //{
      //   get
      //   {
      //      return this._isDirty;
      //   }
      //   set
      //   {
      //      this._isDirty = value;
      //   }
      //}
      #endregion

      /// <summary>
      /// 
      /// </summary>
      public GridColumnStyleCollection()
      {
      }

      #region private method
      //private int CheckName(string parameterName)
      //{
      //   int num1 = this.IndexOf(parameterName);
      //   if (num1 < 0)
      //   {
      //      throw new ArgumentOutOfRangeException();
      //   }
      //   return num1;
      //}

      private GridColumnStyle GetParameter(int index)
      {
         this.RangeCheck(index);
         return this.InnerList[index];
      }

      private GridColumnStyle GetParameter(string fieldName)
      {
         int num1 = this.IndexOf(fieldName);
         if (num1 < 0)
         {
            throw new ArgumentOutOfRangeException();
         }
         return this.InnerList[num1];
      }

      private static int IndexOf(IEnumerable items, string fieldName)
      {
         if (items != null)
         {
            int num1 = 0;
            foreach (GridColumnStyle field2 in items)
            {
               if (fieldName == field2.FieldName)
               {
                  return num1;
               }
               num1++;
            }
         }
         return -1;
      }

      //private void OnChange()
      //{
      //   //this._isDirty = true;
      //}

      private void RangeCheck(int index)
      {
         if ((index < 0) || (this.Count <= index))
         {
            throw new IndexOutOfRangeException(index.ToString(CultureInfo.CurrentCulture));
         }
      }

      private void Replace(int index, object newValue)
      {
         List<GridColumnStyle> list1 = this.InnerList;
         ValidateType(newValue);
         this.Validate(index, newValue);
         list1[index] = (GridColumnStyle)newValue;
      }

      private void SetParameter(int index, GridColumnStyle value)
      {
         //this.OnChange();
         this.RangeCheck(index);
         this.Replace(index, value);
      }

      private void SetParameter(string fieldName, GridColumnStyle value)
      {
         //this.OnChange();
         int num1 = this.IndexOf(fieldName);
         if (num1 < 0)
         {
            throw new ArgumentOutOfRangeException(MessageStringManager.GetString("ParameterOutOfRange"));
         }
         this.Replace(num1, value);
      }

      private void Validate(int index, object value)
      {
         if (value == null)
         {
            throw new ArgumentNullException();
         }
         if ((index != -1) && (index > InnerList.Count))
         {
            throw new IndexOutOfRangeException();
         }
      }

      private static void ValidateType(object value)
      {
         if (value == null)
         {
            throw new ArgumentNullException();
         }
         if (!GridColumnStyleCollection.ItemType.IsInstanceOfType(value))
         {
            throw new InvalidCastException();
         }
      }
      #endregion

      #region public method
      /// <summary>
      /// 向列表添加显示样式对象
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public GridColumnStyle Add(GridColumnStyle value)
      {
         this.Add((object)value);
         return value;
      }

      //[EditorBrowsable(EditorBrowsableState.Never)]
      /// <summary>
      /// 向列表添加显示样式对象
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public int Add(object value)
      {
         //this.OnChange();
         ValidateType(value);
         this.Validate(-1, value);
         this.InnerList.Add((GridColumnStyle)value);
         return (this.Count - 1);
      }

      /// <summary>
      /// 创建新显示样式对象,并加入到列表
      /// </summary>
      /// <param name="fieldName"></param>
      /// <param name="caption"></param>
      /// <param name="width"></param>
      /// <returns></returns>
      public GridColumnStyle Add(string fieldName, string caption, int width)
      {
         return this.Add(new GridColumnStyle(fieldName, caption, width));
      }

      /// <summary>
      /// 批量添加对象
      /// </summary>
      /// <param name="values"></param>
      public void AddRange(GridColumnStyle[] values)
      {
         //this.OnChange();
         if (values == null)
         {
            throw new ArgumentNullException();
         }
         foreach (object obj1 in values)
         {
            ValidateType(obj1);
         }
         foreach (GridColumnStyle styles in values)
         {
            this.Validate(-1, styles);
            this.InnerList.Add((GridColumnStyle)styles);
         }
      }

      /// <summary>
      /// 批量添加对象
      /// </summary>
      /// <param name="values"></param>
      public void AddRange(Array values)
      {
         //this.OnChange();
         if (values == null)
         {
            throw new ArgumentNullException();
         }
         foreach (object obj1 in values)
         {
            ValidateType(obj1);
         }
         foreach (GridColumnStyle styles in values)
         {
            this.Validate(-1, styles);
            this.InnerList.Add((GridColumnStyle)styles);
         }
      }

      /// <summary>
      /// 清空列表
      /// </summary>
      public void Clear()
      {
         //this.OnChange();
         List<GridColumnStyle> list1 = this.InnerList;
         if (list1 != null)
         {
            List<GridColumnStyle>.Enumerator enumerator1 = (List<GridColumnStyle>.Enumerator)list1.GetEnumerator();
            enumerator1.Dispose();
            list1.Clear();
         }
      }

      /// <summary>
      /// 列表是否包含指定的对象
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public bool Contains(GridColumnStyle value)
      {
         return (-1 != this.IndexOf(value));
      }

      /// <summary>
      /// 列表是否包含指定的对象
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public bool Contains(object value)
      {
         return (-1 != this.IndexOf(value));
      }

      //public bool Contains(string value)
      //{
      //   return (-1 != this.IndexOf(value));
      //}

      /// <summary>
      /// 从指定索引开始向列显示样式数组赋值数据
      /// </summary>
      /// <param name="array"></param>
      /// <param name="index"></param>
      public void CopyTo(GridColumnStyle[] array, int index)
      {
         this.InnerList.CopyTo(array, index);
      }

      /// <summary>
      /// 查找与指定对象匹配的列显示样式对象在列表中的位置
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public int IndexOf(GridColumnStyle value)
      {
         return this.IndexOf(value);
      }

      /// <summary>
      /// 查找与指定对象匹配的列显示样式对象在列表中的位置
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public int IndexOf(object value)
      {
         if (value != null)
         {
            ValidateType(value);
            List<GridColumnStyle> list1 = this.InnerList;
            if (list1 != null)
            {
               for (int num1 = 0; num1 < list1.Count; num1++)
               {
                  if ((GridColumnStyle)value == list1[num1])
                  {
                     return num1;
                  }
               }
            }
         }
         return -1;
      }

      /// <summary>
      /// 查找指定名称的字段对应的显示样式对象在列表中的位置
      /// </summary>
      /// <param name="fieldName"></param>
      /// <returns></returns>
      public int IndexOf(string fieldName)
      {
         return GridColumnStyleCollection.IndexOf(this.InnerList, fieldName);
      }

      /// <summary>
      /// 获取对象的 Expression（如果存在的话）。
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         if (Count == 0)
            return "";
         StringBuilder result = new StringBuilder();
         foreach (GridColumnStyle style in InnerList)
         {
            if (result.Length > 0)
               result.Append(SeparatorSign.OtherSeparator); // 分隔每一个列的样式信息
            result.Append(style.FieldName);
            result.Append(SeparatorSign.ListSeparator);
            result.Append(style.Caption);
            result.Append(SeparatorSign.ListSeparator);
            result.Append(style.Width);
         }
         return result.ToString();
      }
      #endregion

      #region ICollection Members

      /// <summary>
      /// 未实现
      /// </summary>
      /// <param name="array"></param>
      /// <param name="index"></param>
      public void CopyTo(Array array, int index)
      {
         throw new NotImplementedException();
      }

      #endregion

      #region IEnumerable Members

      /// <summary>
      /// 返回IEnumerator实例
      /// </summary>
      /// <returns></returns>
      public IEnumerator GetEnumerator()
      {
         return this.InnerList.GetEnumerator();
      }

      #endregion
   }
}
