using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;

using DrectSoft.Common.Eop;
using DrectSoft.Core;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// 通过SQL语句或已有数据集动态创建字典，以满足特殊场合的使用
   /// </summary>
   [TypeConverter(typeof(SqlWordbookConverter))]
   public sealed class SqlWordbook : BaseWordbook
   {
      #region properties
      /// <summary>
      /// 代码值
      /// </summary>
      public string CodeValue
      {
         get { return _codeValue; }
      }
      private string _codeValue;

      /// <summary>
      /// 名称值
      /// </summary>
      public string NameValue
      {
         get { return _nameValue; }
      }
      private string _nameValue;

      /// <summary>
      /// 是否需要使用查询语句生成数据集
      /// </summary>
      public bool UseSqlStatement
      {
         get { return _useSqlStatement; }
      }
      private bool _useSqlStatement;

      /// <summary>
      /// 包含字典数据DataTable。此属性赋值后就不需要用语句去生成数据集
      /// </summary>
      public DataTable BookData
      {
         get { return _bookData; }
         set 
         { 
            if ((value != null) && (value.TableName == _bookData.TableName))
            _bookData = value; 
         }
      }
      private DataTable _bookData;

      /// <summary>
      /// Sql字典默认的显示样式
      /// </summary>
      public GridColumnStyleCollection DefaultGridStyle
      {
         get
         {
            if (ShowStyles.Count == 0)
               return new GridColumnStyleCollection();
            else
               return ShowStyles[0];
         }
      }
      #endregion

      private bool m_AutoAddShortCode;

      #region ctors
      /// <summary>
      /// 用指定的SQL语句等信息创建SQL字典类实例。
      /// 字段的显示名称、显示哪些列、列宽等信息在columnStyles中指定
      /// </summary>
      /// <param name="uniqueName">局部唯一的名称</param>
      /// <param name="query">查询语句</param>
      /// <param name="codeField">代码字段名</param>
      /// <param name="nameField">名称字段名</param>
      /// <param name="columnStyles">字段的显示名称、显示哪些列、列宽等信息</param>
      public SqlWordbook(string uniqueName, string query, string codeField, string nameField, GridColumnStyleCollection columnStyles)
         : base()
      {
         Debug.Assert(uniqueName != null, "类名不能为NULL");
         Debug.Assert(uniqueName.Length > 0, "类名不能为空");
         WordbookName = uniqueName;
         Caption = WordbookName;
         ExtraCondition = "";

         if (query.Length == 0)
            throw new ArgumentNullException("必须指定查询语句");
         QuerySentence = query;

         if (codeField.Length == 0)
            throw new ArgumentNullException("必须指定代码字段");
         CodeField = codeField;
         QueryCodeField = CodeField;

         if (nameField.Length == 0)
            throw new ArgumentNullException("必须指定名称字段");
         NameField = nameField;

         DefaultFilterFields = new Collection<string>();
         // 使用默认的代码列和名称列进行过滤
         DefaultFilterFields.Add(codeField);
         DefaultFilterFields.Add(nameField);

         CurrentMatchFields = new Collection<string>();
         foreach(string field in DefaultFilterFields)
            CurrentMatchFields.Add(field);

         Parameters = new FilterParameterCollection();

         ShowStyles = new Collection<GridColumnStyleCollection>();
         ShowStyles.Add(columnStyles);
         SelectedStyleIndex = 0;

         // Sql字典默认为不缓存数据
         CacheTime = -1;

         _useSqlStatement = true;
         //_bookData = null;
         //m_DrawConditions = new Collection<CustomDrawSetting>();
      }

      /// <summary>
      /// 用指定的SQL语句等信息创建SQL字典类实例。
      /// 字段的显示名称、显示哪些列、列宽等信息在columnStyles中指定
      /// </summary>
      /// <param name="uniqueName">局部唯一的名称</param>
      /// <param name="query">查询语句</param>
      /// <param name="codeField">代码字段名</param>
      /// <param name="nameField">名称字段名</param>
      /// <param name="columnStyles">字段的显示名称、显示哪些列、列宽等信息</param>
      /// <param name="matchFieldComb">指定用来匹配输入数据的字段名，多个时用“//”格开,可以传入空</param>
      public SqlWordbook(string uniqueName, string query, string codeField, string nameField, GridColumnStyleCollection columnStyles, string matchFieldComb)
         : this(uniqueName, query, codeField, nameField, columnStyles)
      {
         string[] separator = new string[] { SeparatorSign.ListSeparator };
         string[] values = matchFieldComb.Split(separator, StringSplitOptions.RemoveEmptyEntries);

         if (values.Length > 0)
         {
            DefaultFilterFields.Clear();
            CurrentMatchFields.Clear();
            for (int i = 0; i < values.Length; i++)
            {
               DefaultFilterFields.Add(values[i]);
               CurrentMatchFields.Add(values[i]);
            }
         }
      }
      
      /// <summary>
      /// 用指定的SQL语句等信息创建SQL字典类实例。
      /// 字段的显示名称、显示哪些列、列宽等信息在columnStyles中指定
      /// </summary>
      /// <param name="uniqueName">局部唯一的名称</param>
      /// <param name="query">查询语句</param>
      /// <param name="codeField">代码字段名</param>
      /// <param name="nameField">名称字段名</param>
      /// <param name="columnStyles">字段的显示名称、显示哪些列、列宽等信息</param>
      /// <param name="autoAddShortCode">自动添加拼音、五笔列</param>
      public SqlWordbook(string uniqueName, string query, string codeField, string nameField, GridColumnStyleCollection columnStyles, bool autoAddShortCode)
         : this(uniqueName, query, codeField, nameField, columnStyles)
      {
         if (autoAddShortCode)
         {
            DefaultFilterFields.Add("py");
            DefaultFilterFields.Add("wb");
            CurrentMatchFields.Add("py");
            CurrentMatchFields.Add("wb");
            m_AutoAddShortCode = true;
         }
      }

      /// <summary>
      /// 用指定的SQL语句等信息创建SQL字典类实例。
      /// 字段的显示名称、显示哪些列、列宽等信息在columnStylesString中指定
      /// </summary>
      /// <param name="uniqueName">局部唯一的名称</param>
      /// <param name="query">查询语句</param>
      /// <param name="codeField">代码字段名</param>
      /// <param name="nameField">名称字段名</param>
      /// <param name="columnStylesComb">字段的显示名称、显示哪些列、列宽等信息，以“//”和“///”分隔</param>
      /// <param name="matchFieldComb">指定用来匹配输入数据的字段名，多个时用“//”格开,可以传入空</param>
      public SqlWordbook(string uniqueName, string query, string codeField, string nameField, string columnStylesComb, string matchFieldComb)
         : this(uniqueName, query, codeField, nameField, CreateColumnStyleCollectionByString(columnStylesComb), matchFieldComb)
      { }

      /// <summary>
      /// 用指定的DataTable等信息创建SQL字典类实例。
      /// 列的显示名用DataColumn的Caption代替，需要显示的列及其列宽通过columnWidthes设置
      /// </summary>
      /// <param name="uniqueName">局部唯一的字典名</param>
      /// <param name="sourceData">数据集</param>
      /// <param name="codeField">代码字段名</param>
      /// <param name="nameField">名称字段名</param>
      /// <param name="columnWidthes">需要显示的列名及其列宽</param>
      public SqlWordbook(string uniqueName, DataTable sourceData, string codeField, string nameField, Dictionary<string, int> columnWidthes)
         : base()
      {
         Debug.Assert(uniqueName != null, "类名不能为NULL");
         Debug.Assert(uniqueName.Length > 0, "类名不能为空");
         WordbookName = uniqueName;
         Caption = WordbookName;

         if (sourceData == null)
            throw new ArgumentNullException("数据集不能为空");
         //if (sourceData.Rows.Count == 0)
         //   throw new ArgumentNullException("数据集中未包含数据");
         _bookData = sourceData;

         // DataTable中默认RowFilter作为扩展条件保存
         ExtraCondition = _bookData.DefaultView.RowFilter;

         QuerySentence = "";

         if (codeField.Length == 0)
            throw new ArgumentNullException("必须指定代码字段");
         if (!sourceData.Columns.Contains(codeField))
            throw new ArgumentOutOfRangeException("指定的代码字段在数据集中不存在");

         CodeField = codeField;
         QueryCodeField = CodeField;

         if (nameField.Length == 0)
            throw new ArgumentNullException("必须指定名称字段");
         if (!sourceData.Columns.Contains(nameField))
            throw new ArgumentOutOfRangeException("指定的名称字段在数据集中不存在");
         NameField = nameField;

         DefaultFilterFields = new Collection<string>();
         // 使用默认的代码列和名称列进行过滤
         DefaultFilterFields.Add(codeField);
         DefaultFilterFields.Add(nameField);

         CurrentMatchFields = new Collection<string>();
         foreach (string field in DefaultFilterFields)
            CurrentMatchFields.Add(field);

         Parameters = new FilterParameterCollection();

         ShowStyles = new Collection<GridColumnStyleCollection>();
         GridColumnStyleCollection styleCollection = new GridColumnStyleCollection();
         GridColumnStyle style;
         foreach (KeyValuePair<string, int> keyValue in columnWidthes)
         {
            style = new GridColumnStyle(keyValue.Key
               , sourceData.Columns[keyValue.Key].Caption, keyValue.Value);
            styleCollection.Add(style);
         }
         if (styleCollection.Count == 0)
            throw new ArgumentException("必须设置字典的显示列信息");
         ShowStyles.Add(styleCollection);
         SelectedStyleIndex = 0;

         // Sql字典默认为不缓存数据
         CacheTime = -1;

         //_useSqlStatement = false;
         //_autoAddShortCode = false;
         //_shortCodeAdded = true;

         //m_DrawConditions = new Collection<CustomDrawSetting>();
      }

      /// <summary>
      /// 用指定的DataTable等信息创建SQL字典类实例。
      /// 列的显示名用DataColumn的Caption代替，需要显示的列及其列宽通过columnWidthes设置
      /// </summary>
      /// <param name="uniqueName">局部唯一的字典名</param>
      /// <param name="sourceData">数据集</param>
      /// <param name="codeField">代码字段名</param>
      /// <param name="nameField">名称字段名</param>
      /// <param name="columnWidthes">需要显示的列名及其列宽</param>
      /// <param name="autoAddShortCode">自动添加拼音、五笔列</param>
      public SqlWordbook(string uniqueName, DataTable sourceData, string codeField, string nameField, Dictionary<string, int> columnWidthes, bool autoAddShortCode)
         : this(uniqueName, sourceData, codeField, nameField, columnWidthes)
      {
         if (autoAddShortCode)
         {
            DefaultFilterFields.Add(GenerateShortCode.FieldPy);
            DefaultFilterFields.Add(GenerateShortCode.FieldWb);
            CurrentMatchFields.Add(GenerateShortCode.FieldPy);
            CurrentMatchFields.Add(GenerateShortCode.FieldWb);
            m_AutoAddShortCode = true;
         }
      }

      /// <summary>
      /// 用指定的DataTable等信息创建SQL字典类实例。
      /// 列的显示名用DataColumn的Caption代替，需要显示的列及其列宽通过columnWidthes设置
      /// </summary>
      /// <param name="uniqueName">局部唯一的字典名</param>
      /// <param name="sourceData">数据集</param>
      /// <param name="codeField">代码字段名</param>
      /// <param name="nameField">名称字段名</param>
      /// <param name="columnWidthes">需要显示的列名及其列宽</param>
      /// <param name="matchFieldComb">指定用来匹配输入数据的字段名，多个时用“//”格开,可以传入空</param>
      public SqlWordbook(string uniqueName, DataTable sourceData, string codeField, string nameField, Dictionary<string, int> columnWidthes, string matchFieldComb)
         : this(uniqueName, sourceData, codeField, nameField, columnWidthes)
      {
         string[] separator = new string[] { SeparatorSign.ListSeparator };
         string[] values = matchFieldComb.Split(separator, StringSplitOptions.RemoveEmptyEntries);

         if (values.Length > 0)
         {
            DefaultFilterFields.Clear();
            CurrentMatchFields.Clear();
            for (int i = 0; i < values.Length; i++)
            {
               if (!sourceData.Columns.Contains(values[i]))
                  throw new ArgumentOutOfRangeException("指定的查询字段在数据集中不存在");
               DefaultFilterFields.Add(values[i]);
               CurrentMatchFields.Add(values[i]);
            }
         }
      }
      #endregion

      #region public methods
      /// <summary>
      /// 确认数据集（在这里统一处理SQL和Table两种类型的数据源，根据设置添加拼音五笔字段）
      /// </summary>
      /// <param name="sqlHelper">执行SQL时需要</param>
      /// <param name="shortCodeHelper">自动生成拼音五笔代码时需要</param>
      public void EnsureBookData(IDataAccess sqlHelper, GenerateShortCode shortCodeHelper)
      {
         if (UseSqlStatement)
         {
            if (sqlHelper == null)
               throw new ArgumentNullException("数据访问对象为空");
            _bookData = sqlHelper.ExecuteDataTable(QuerySentence, CommandType.Text);
         }
         if (m_AutoAddShortCode)
         {
            // 如果已存在‘py’和'wb'列则不处理，以节省时间
            if (BookData.Columns.Contains(GenerateShortCode.FieldPy)
               && BookData.Columns.Contains(GenerateShortCode.FieldWb))
               return;

            if (shortCodeHelper == null)
               throw new ArgumentNullException("拼音五笔代码生成类为空");
            shortCodeHelper.AutoAddShortCode(BookData, NameField);
         }
      }

      /// <summary>
      /// 获取对象的 Expression（如果存在的话）。
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         if (UseSqlStatement)
            return String.Format(CultureInfo.CurrentCulture
               , "{0}; {1}; {2}; {3}; {4}; {5}"
               , WordbookName
               , QuerySentence
               , CodeField
               , NameField
               , MatchFieldComb
               , ShowStyles[0]);
         else
            return String.Format(CultureInfo.CurrentCulture
               , "{0}; {1}; {2}; {3}; {4}; {5}"
               , WordbookName
               , BookData.TableName
               , CodeField
               , NameField
               , MatchFieldComb
               , ShowStyles[0]);
      }

      ///// <summary>
      ///// 根据数据集创建Grid中列的显示样式
      ///// </summary>
      ///// <param name="sourceTable"></param>
      //public void GenerateShowStyle(DataTable sourceTable)
      //{
      //   _showStyles.Clear();
      //   SelectedStyleIndex = -1;
      //   if (sourceTable == null)
      //      return;

      //   DataColumnCollection cols = sourceTable.Columns;
      //   _showStyles.Add(new GridColumnStyleCollection());

      //   int width;
      //   // 如果未设置显示名称，则直接显示原字段名（以下划线开头的字段名表示该列不显示）
      //   if ((_columnNames != null) && (_columnNames.Count > 0))
      //   {
      //      foreach (KeyValuePair<string, string> name in _columnNames)
      //      {
      //         if (cols.Contains(name.Key))
      //         {
      //            if (cols[name.Key].DataType == Type.GetType("System.String"))
      //            {
      //               width = 150;
      //            }
      //            else
      //               width = GetStringLength(name.Value) * 15 / 2;
      //            _showStyles[0].Add(new GridColumnStyle(name.Key, name.Value, width));
      //         }
      //      }
      //   }
      //   else
      //   {
      //      foreach (DataColumn col in cols)
      //      {
      //         if (col.ColumnName.StartsWith("_"))
      //            continue;
      //         width = GetStringLength(col.Caption) * 15 / 2;
      //         if (col.DataType != Type.GetType("System.String"))
      //         {
      //            width = Math.Min(width, 100);
      //         }
      //         _showStyles[0].Add(new GridColumnStyle(col.ColumnName, col.Caption, width));
      //      }
      //   }
      //   SelectedStyleIndex = 0;
      //}

      /// <summary>
      /// 清除属性的值
      /// </summary>
      public void ClearValueFields()
      {
         _codeValue = "";
         _nameValue = "";
      }

      /// <summary>
      /// 用选中的记录初始化属性值
      /// </summary>
      /// <param name="sourceRow"></param>
      public void InitValueFields(DataRow sourceRow)
      {
         ClearValueFields();

         if (sourceRow != null)
         {
            DataColumnCollection cols = sourceRow.Table.Columns;
            if (cols.Contains(CodeField))
            {
               _codeValue = sourceRow[CodeField].ToString();
               if ((cols[CodeField].DataType == Type.GetType("System.String"))
                  || (cols[CodeField].DataType == Type.GetType("System.Char")))
                  CodeFieldIsString = true;
               else
                  CodeFieldIsString = false;
            }
            if (cols.Contains(NameField))
               _nameValue = sourceRow[NameField].ToString();
         }
      }

      /// <summary>
      /// 用字典实例创建 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// 将传入的字符串转换为GridColumnStyleCollection对象
      /// </summary>
      /// <param name="stylesComb">列样式字符串组合</param>
      /// <returns></returns>
      public static GridColumnStyleCollection CreateColumnStyleCollectionByString(string stylesComb)
      {
         GridColumnStyleCollection styleCollection = new GridColumnStyleCollection();
         if (String.IsNullOrEmpty(stylesComb))
            return styleCollection;
         // 先分解出单个的列显示样式
         string[] sepColumn = new string[] { SeparatorSign.OtherSeparator };
         string[] styles = stylesComb.Split(sepColumn, StringSplitOptions.None);
         string[] sepValue = new string[] { SeparatorSign.ListSeparator };
         string[] values;
         foreach (string styleString in styles)
         {
            values = styleString.Split(sepValue, StringSplitOptions.None);
            styleCollection.Add(new GridColumnStyle(values[0], values[1]
               , Convert.ToInt32(values[2], CultureInfo.CurrentCulture)));
         }
         return styleCollection;
      }
      #endregion
   }
}
