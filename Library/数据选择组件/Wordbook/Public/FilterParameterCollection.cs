using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// 过滤参数集合类
   /// </summary>
   [CLSCompliantAttribute(true)]
   public class FilterParameterCollection : ICollection, IEnumerable
   {
      #region Properties
      /// <summary>
      /// 参数数量
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

      private List<FilterParameter> InnerList
      {
         get
         {
            List<FilterParameter> list1 = this._items;
            if (list1 == null)
            {
               list1 = new List<FilterParameter>();
               this._items = list1;
            }
            return list1;
         }
      }

      //public bool IsDirty
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

      #region comment code
      /// <summary>
      /// 指示 IList 是否具有固定大小。
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
      #endregion

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
      /// 通过参数索引号查找参数
      /// </summary>
      /// <param name="index"></param>
      /// <returns></returns>
      public FilterParameter this[int index]
      {
         get
         {
            return (FilterParameter)this.GetParameter(index);
         }
         set
         {
            this.SetParameter(index, value);
         }
      }

      //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
      /// <summary>
      /// 通过参数名查找参数
      /// </summary>
      /// <param name="parameterName"></param>
      /// <returns></returns>
      public FilterParameter this[string parameterName]
      {
         get
         {
            return (FilterParameter)this.GetParameter(parameterName);
         }
         set
         {
            this.SetParameter(parameterName, value);
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

      // Fields
      //private bool _isDirty;
      private List<FilterParameter> _items;
      private static Type ItemType = typeof(FilterParameter);

      // Ctors
      //static FilterParameterCollection()
      //{
      //   FilterParameterCollection.ItemType = typeof(FilterParameter);
      //}

      internal FilterParameterCollection()
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

      private FilterParameter GetParameter(int index)
      {
         this.RangeCheck(index);
         return this.InnerList[index];
      }

      private FilterParameter GetParameter(string parameterName)
      {
         int num1 = this.IndexOf(parameterName);
         if (num1 < 0)
         {
            throw new ArgumentOutOfRangeException();
         }
         return this.InnerList[num1];
      }


      private static int IndexOf(IEnumerable items, string parameterName)
      {
         if (items != null)
         {
            int num1 = 0;
            foreach (FilterParameter parameter2 in items)
            {
               if (parameterName == parameter2.Caption)
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
         List<FilterParameter> list1 = this.InnerList;
         ValidateType(newValue);
         this.Validate(index, newValue);
         list1[index] = (FilterParameter)newValue;
      }

      private void SetParameter(int index, FilterParameter value)
      {
         //this.OnChange();
         this.RangeCheck(index);
         this.Replace(index, value);
      }

      private void SetParameter(string parameterName, FilterParameter value)
      {
         //this.OnChange();
         int num1 = this.IndexOf(parameterName);
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
         if (!FilterParameterCollection.ItemType.IsInstanceOfType(value))
         {
            throw new InvalidCastException();
         }
      }
      #endregion

      #region public method
      /// <summary>
      /// 向参数列表添加参数
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public FilterParameter Add(FilterParameter value)
      {
         this.Add((object)value.Clone());
         return value;
      }

      //[EditorBrowsable(EditorBrowsableState.Never)]
      /// <summary>
      /// 向参数列表添加参数
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public int Add(object value)
      {
         //this.OnChange();
         ValidateType(value);
         this.Validate(-1, value);
         this.InnerList.Add(((FilterParameter)value).Clone());
         return (this.Count - 1);
      }

      /// <summary>
      /// 创建新参数,并加入到参数列表中
      /// </summary>
      /// <param name="fieldName">对应的数据集字段名</param>
      /// <param name="name">参数的显示名称</param>
      /// <param name="isString">是否是字符型</param>
      /// <param name="description">参数描述信息</param>
      /// <param name="allowUserEdit">是否允许用户编辑</param>
      /// <param name="sign">参数运算符</param>
      /// <returns></returns>
      public FilterParameter Add(string fieldName, string name, bool isString
         , string description, bool allowUserEdit, CompareOperator sign)
      {
         return this.Add(new FilterParameter(fieldName, name, isString
         , description, allowUserEdit, sign));
      }

      /// <summary>
      /// 创建新参数,并加入到参数列表中
      /// </summary>
      /// <param name="fieldName">对应的数据集字段名</param>
      /// <param name="name">参数的显示名称</param>
      /// <param name="isString">是否是字符型</param>
      /// <param name="description">参数描述信息</param>
      /// <param name="allowUserEdit">是否允许用户编辑</param>
      /// <param name="sign">参数运算符</param>
      /// <param name="value">参数默认值</param>
      /// <returns></returns>
      public FilterParameter Add(string fieldName, string name, bool isString
         , string description, bool allowUserEdit, CompareOperator sign, object value)
      {
         return this.Add(new FilterParameter(fieldName, name, isString
         , description, allowUserEdit, sign, value));
      }

      /// <summary>
      /// 创建数值型参数,并加入到参数列表
      /// </summary>
      /// <param name="fieldName">对应的数据集字段名</param>
      /// <param name="name">参数的显示名称</param>
      /// <param name="dataSort">参数所属的数据类别</param>
      /// <param name="description">参数描述信息</param>
      /// <param name="sign"></param>
      /// <returns></returns>
      public FilterParameter Add(string fieldName, string name, int dataSort
         , string description, CompareOperator sign)
      {
         return this.Add(new FilterParameter(fieldName, name, dataSort
         , description, sign));
      }

      /// <summary>
      /// 创建数值型参数,并加入到参数列表
      /// </summary>
      /// <param name="fieldName">对应的数据集字段名</param>
      /// <param name="name">参数的显示名称</param>
      /// <param name="dataSort">参数所属的数据类别</param>
      /// <param name="description">参数描述信息</param>
      /// <param name="sign">参数运算符</param>
      /// <param name="value">参数默认值</param>
      /// <returns></returns>
      public FilterParameter Add(string fieldName, string name, int dataSort
         , string description, CompareOperator sign, object value)
      {
         return this.Add(new FilterParameter(fieldName, name, dataSort
         , description, sign, value));
      }

      /// <summary>
      /// 批量添加参数
      /// </summary>
      /// <param name="values"></param>
      public void AddRange(FilterParameter[] values)
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
         foreach (FilterParameter parameter1 in values)
         {
            this.Validate(-1, parameter1);
            this.InnerList.Add(parameter1.Clone());
         }
      }

      /// <summary>
      /// 批量添加参数
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
         foreach (FilterParameter parameter1 in values)
         {
            this.Validate(-1, parameter1);
            this.InnerList.Add(parameter1.Clone());
         }
      }

      /// <summary>
      /// 清空参数列表
      /// </summary>
      public void Clear()
      {
         //this.OnChange();
         List<FilterParameter> list1 = this.InnerList;
         if (list1 != null)
         {
            List<FilterParameter>.Enumerator enumerator1 = (List<FilterParameter>.Enumerator)list1.GetEnumerator();
            enumerator1.Dispose();
            list1.Clear();
         }
      }

      /// <summary>
      /// 检查参数列表是否包含指定的参数
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public bool Contains(FilterParameter value)
      {
         return (-1 != this.IndexOf(value));
      }

      /// <summary>
      /// 检查参数列表是否包含指定的参数
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public bool Contains(object value)
      {
         return (-1 != this.IndexOf(value));
      }

      ///// <summary>
      ///// 检查参数列表是否包含指定的参数
      ///// </summary>
      ///// <param name="value"></param>
      ///// <returns></returns>
      //public bool Contains(string value)
      //{
      //   return (-1 != this.IndexOf(value));
      //}

      /// <summary>
      /// 从指定索引开始向过滤参数数组中复制对象
      /// </summary>
      /// <param name="array"></param>
      /// <param name="index"></param>
      public void CopyTo(FilterParameter[] array, int index)
      {
         this.InnerList.CopyTo(array, index);
      }

      /// <summary>
      /// 返回IEnumerator实例
      /// </summary>
      /// <returns></returns>
      public IEnumerator GetEnumerator()
      {
         return this.InnerList.GetEnumerator();
      }

      /// <summary>
      /// 查找指定参数对象在List中的位置
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public int IndexOf(FilterParameter value)
      {
         return this.IndexOf(value);
      }

      /// <summary>
      /// 查找指定参数对象在List中的位置
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public int IndexOf(object value)
      {
         if (value != null)
         {
            ValidateType(value);
            List<FilterParameter> list1 = this.InnerList;
            FilterParameter temp = (FilterParameter)value;
            if (list1 != null)
            {
               for (int num1 = 0; num1 < list1.Count; num1++)
               {
                  if ( temp == list1[num1])
                  {
                     return num1;
                  }
               }
            }
         }
         return -1;
      }

      /// <summary>
      /// 查找与指定名称匹配的参数在List中的位置
      /// </summary>
      /// <param name="parameterName"></param>
      /// <returns></returns>
      public int IndexOf(string parameterName)
      {
         return FilterParameterCollection.IndexOf(this.InnerList, parameterName);
      }

      /// <summary>
      /// 将参数List转换为DataTable对象
      /// </summary>
      /// <returns></returns>
      public DataTable Convert2DataTable()
      {
         DataTable paramTable = new DataTable();
         paramTable.Locale = CultureInfo.CurrentCulture;
         paramTable.Columns.AddRange(new DataColumn[]{
              new DataColumn("FieldName",Type.GetType("System.String"))
            , new DataColumn("Name",Type.GetType("System.String"))
            , new DataColumn("IsString",Type.GetType("System.Boolean"))
            , new DataColumn("Description",Type.GetType("System.String"))
            , new DataColumn("DataSort",Type.GetType("System.Int32"))
            , new DataColumn("AllowUserEdit",Type.GetType("System.Boolean"))
            , new DataColumn("Enable",Type.GetType("System.Boolean"))
            , new DataColumn("Sign",Type.GetType("System.String"))
            , new DataColumn("Value",Type.GetType("System.String"))
         });

         DataRow newRow;
         foreach (FilterParameter para in this.InnerList)
         {
            newRow = paramTable.NewRow();
            newRow["FieldName"] = para.FieldName;
            newRow["Name"] = para.Caption;
            newRow["IsString"] = para.IsString;
            newRow["Description"] = para.Description;
            newRow["DataSort"] = para.DataCatalog;
            newRow["AllowUserEdit"] = para.AllowUserEdit;
            newRow["Enable"] = para.Enabled;
            newRow["Sign"] = para.Operator.ToString();
            newRow["Value"] = para.Value;

            paramTable.Rows.Add(newRow);
         }

         return paramTable;
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

   }
}
